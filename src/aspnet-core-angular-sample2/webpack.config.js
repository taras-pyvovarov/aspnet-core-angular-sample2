"use strict";

//Angular doesn't work without these modules.
//Webpack will crash, require it explicitly.
var rxjs = require('rxjs');
var zonejs = require('zone.js');

//Imports.
var webpack = require('webpack');
var merge = require('webpack-merge');
var nodeExternals = require('webpack-node-externals');
var path = require('path');

//*********Global variables and hardcodes*********
//It is dev build if '-p' passed as cmd parameter.
var isDevBuild = process.argv.indexOf('-p') < 0;
const dist = 'wwwroot';
const distServer = 'wwwroot_server';
const sampleApp1Name = 'SampleApp1';
const sampleApp1RootClient = 'root-client.ts';
const sampleApp1RootServer = 'root-server.ts';

//*********Building Angular app with server side rendering*********

//Common configs for server and client bundles.
var sharedConfig = {

    //!!!
    resolve: { extensions: ['.ts', '.js'] },

    //Output bundle configuration.
    output: {
        filename: '[name].js',
        //Webpack dev middleware, if enabled, handles requests for this URL prefix.
        //webpack-dev-server will find static assets in dist folder when they are referenced with 'publicPath' prefix.
        //Needed for code splitting, url/file-loader, etc.
        publicPath: '/'
    },

    //!!!
    module: {
        loaders: [
            //Use ts loader for typescript. It will compile into bundle. 
            { test: /\.ts$/, loader: 'ts-loader', exclude: /node_modules/ },

            //Use raw loader for html. It will inject html as a string inside bundle.
            { test: /\.html$/, loader: 'raw-loader' },
        ]
    },

    //This option controls if and how Source Maps are generated.
    //inline-source-map - A SourceMap is added as a DataUrl to the bundle.
    devtool: isDevBuild ? 'inline-source-map' : 'false',

    //Common plugins for server and client bundles.
    plugins: [
        //List of plugins here.
    ].concat(isDevBuild ? [] : [
        //Additionally plugins that apply in production builds only

        //Run js uglification, additionally remove js comments.
        new webpack.optimize.UglifyJsPlugin({ comments: false }),
    ])
};

//Common configs for client bundle.
var clientBundleConfig = merge(sharedConfig, {

    //!!!
    entry: {
        'sample-app1-client': path.join(__dirname, sampleApp1Name, sampleApp1RootClient)
    },

    //Output bundle configuration.
    output: {
        path: path.join(__dirname, dist),
    },

    //Plugins for client bundle.
    plugins: [
        new webpack.DllReferencePlugin({
            context: __dirname,
            manifest: require(path.join(__dirname, dist, 'vendor-manifest.json'))
        }),
        new webpack.DllReferencePlugin({
            context: __dirname,
            manifest: require(path.join(__dirname, dist, 'angular-manifest.json'))
        }),
    ],
});

//Common configs for server bundle.
var serverBundleConfig = merge(sharedConfig, {

    //!!!
    entry: {
        'sample-app1-server': path.join(__dirname, sampleApp1Name, sampleApp1RootServer)
    },

    //Output bundle configuration.
    output: {
        //!!!
        libraryTarget: 'commonjs',
        path: path.join(__dirname, distServer),
    },

    //!!!
    target: 'node',

    //!!!
    externals: [nodeExternals()]
});

//Exporting bundle configs.
module.exports = [clientBundleConfig, serverBundleConfig];