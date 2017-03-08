"use strict";

//Angular doesn't work without these modules.
//Webpack will crash, require it explicitly.
var rxjs = require('rxjs');
var zonejs = require('zone.js');

//Imports.
var webpack = require('webpack');
var path = require('path');

//App hardcodes.
var dist = 'wwwroot';

module.exports = {
    entry: {
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
        ]
    },

    output: {
        filename: '[name].bundle.js',
        path: path.join(__dirname, dist),

        // The name of the global variable which the library's
        // require() function will be assigned to
        library: '[name]_[hash]',
    },

    plugins: [
      new webpack.DllPlugin({
          // The path to the manifest file which maps between
          // modules included in a bundle and the internal IDs
          // within that bundle
          path: path.join(__dirname, dist, '[name]-manifest.json'),
          // The name of the global variable which the library's
          // require function has been assigned to. This must match the
          // output.library option above
          name: '[name]_[hash]'
      }),
    ],
}