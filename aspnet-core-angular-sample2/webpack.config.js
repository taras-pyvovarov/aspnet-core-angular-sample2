"use strict";

//Angular doesn't work without these modules.
//Webpack will crash, require it explicitly.
var rxjs = require('rxjs');
var zonejs = require('zone.js');

//Imports.
var webpack = require('webpack');
var path = require('path');
var nodeExternals = require('webpack-node-externals');

//App hardcodes.
const dist = 'wwwroot';
const distServer = 'wwwroot_server';
const sampleApp1Name = 'SampleApp1';
const sampleApp1RootClient = 'root-client.ts';
const sampleApp1RootServer = 'root-server.ts';

var clientBundleConfig = {
    entry: { 
        'sample-app1-client': path.join(__dirname, sampleApp1Name, sampleApp1RootClient)
    },

    output: {
        path: path.join(__dirname, dist),
        filename: '[name].js',
    },

    resolve: {
        extensions: ['.ts', '.js']
    },

    module: {
        loaders: [
            {
                loader: 'ts-loader',
                exclude: /node_modules/,
            }
        ]
    },

    plugins: [
        new webpack.DllReferencePlugin({
            context: __dirname,
            manifest: require(path.join(__dirname, dist, 'angular-manifest.json'))
        }),
    ],

    devtool: 'inline-source-map',
};

var serverBundleConfig = {
    entry: {
        'sample-app1-server': path.join(__dirname, sampleApp1Name, sampleApp1RootServer)
    },
    output: {
        //!!!
        libraryTarget: 'commonjs',
        path: path.join(__dirname, distServer),
        filename: '[name].js',
    },

    resolve: {
        extensions: ['.ts', '.js']
    },

    module: {
        loaders: [
            {
                loader: 'ts-loader',
                exclude: /node_modules/,
            }
        ]
    },

    //!!!
    target: 'node',

    devtool: 'inline-source-map',

    //!!!
    externals: [nodeExternals()]
};

module.exports = [clientBundleConfig, serverBundleConfig];