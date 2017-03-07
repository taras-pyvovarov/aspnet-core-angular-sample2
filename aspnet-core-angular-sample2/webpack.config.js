"use strict";

//Angular doesn't work without these modules.
//Webpack will crash, require it explicitly.
var rxjs = require('rxjs');
var zonejs = require('zone.js');

var webpack = require('webpack');
var path = require('path');
var nodeExternals = require('webpack-node-externals');

var clientBundleConfig = {
    entry: { 'sample-app1-client': './SampleApp1/root-client.ts' },

    output: {
        path: path.join(__dirname, 'wwwroot'),
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
            context: '.',
            manifest: require(path.join(__dirname, 'wwwroot', 'angular-manifest.json'))
        }),
    ],

    devtool: 'inline-source-map',
};

var serverBundleConfig = {
    entry: {
        'sample-app1-server': './SampleApp1/root-server.ts'
    },
    output: {
        libraryTarget: 'commonjs',
        path: path.join(__dirname, 'wwwroot_server'),
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

    target: 'node',

    devtool: 'inline-source-map',

    externals: [nodeExternals()]
};

module.exports = [clientBundleConfig, serverBundleConfig];