import 'angular2-universal-polyfills/browser';
import { enableProdMode } from '@angular/core';
import { platformBrowserDynamic } from '@angular/platform-browser-dynamic';

import { AppModule } from './root-module';

//Add bootstrap import, so bundler will resolve it as a dependency.
//Without import, bootstrap js logics is not working.
import 'bootstrap';

//!!!
// Enable either Hot Module Reloading or production mode
if (module['hot']) {
    module['hot'].accept();
    module['hot'].dispose(() => { platform.destroy(); });
} else {
    enableProdMode();
}

const platform = platformBrowserDynamic();

//!!! remove?
const bootApplication = () => { platform.bootstrapModule(AppModule); };

// Boot the application, either now or when the DOM content is loaded
if (document.readyState === 'complete') {
    bootApplication();
} else {
    document.addEventListener('DOMContentLoaded', bootApplication);
}