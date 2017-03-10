import 'angular2-universal-polyfills/browser';
import { enableProdMode } from '@angular/core';
import { platformUniversalDynamic } from 'angular2-universal';

import { AppModule } from './root-module';

//!!!
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

const platform = platformUniversalDynamic();

//!!! remove?
const bootApplication = () => { platform.bootstrapModule(AppModule); };

// Boot the application, either now or when the DOM content is loaded
if (document.readyState === 'complete') {
    bootApplication();
} else {
    document.addEventListener('DOMContentLoaded', bootApplication);
}