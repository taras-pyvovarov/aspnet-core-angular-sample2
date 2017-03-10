"use strict";

//Angular doesn't work without these modules.
//Webpack will crash, require it explicitly.
var rxjs = require('rxjs');
var zonejs = require('zone.js');

//Imports.
var webpack = require('webpack');
var extractTextPlugin = require('extract-text-webpack-plugin');
var path = require('path');

//App hardcodes.
var dist = 'wwwroot';

//*********Building 3-rd party libraries and assets*********

//Creating instance of extract text plugin for vendor bundle
var extractBundleCSS = new extractTextPlugin('[name].css');

module.exports = {
    entry: {
        //List of modules in 'angular' bundle.
        'angular': [
            '@angular/common',
            '@angular/compiler',
            '@angular/core',
            '@angular/http',
            '@angular/platform-browser',
            '@angular/platform-browser-dynamic',
            '@angular/router',
            'angular2-universal',
            'angular2-universal-polyfills',
        ],

        //List of modules in 'vendor' bundle.
        'vendor': [
            'jquery',
            'tether',
            'bootstrap',
            'bootstrap/dist/css/bootstrap.css',
        ],
    },

    output: {
        filename: '[name].bundle.js',
        path: path.join(__dirname, dist),

        // The name of the global variable which the library's
        // require() function will be assigned to
        library: '[name]_[hash]',
    },

    module: {
        loaders: [
            //!!!
            { test: /\.css$/, loader: extractBundleCSS.extract({ fallback: 'style-loader', use: 'css-loader' }) },
        ]
    },

    plugins: [
        //Apply text extraction plugin.
        extractBundleCSS,

        //bootstap depends on jQuery and tether libs to be already loaded and global variables available.
        new webpack.ProvidePlugin({ $: "jquery", jQuery: "jquery", Tether: 'tether' }),

        //Apply DLL plugin to split out 3-rd party libraries from dependent app code.
        new webpack.DllPlugin({
            // The path to the manifest file which maps between modules included in a bundle and the internal IDs within that bundle.
            path: path.join(__dirname, dist, '[name]-manifest.json'),
            // The name of the global variable which the library's require function has been assigned to. This must match the output.library option above
            name: '[name]_[hash]'
        }),
    ],
}