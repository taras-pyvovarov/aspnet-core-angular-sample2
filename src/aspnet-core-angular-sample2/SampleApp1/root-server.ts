import 'angular2-universal-polyfills';
import 'zone.js';
import { createServerRenderer } from 'aspnet-prerendering';
import { enableProdMode } from '@angular/core';
import { platformNodeDynamic } from 'angular2-universal';

import { AppModule } from './root-module';

enableProdMode();
const platform = platformNodeDynamic();

export default createServerRenderer(params => {
    return new Promise((resolve, reject) => {
        const requestZone = Zone.current.fork({
            //!!!
            name: 'angular-universal request',

            //!!!
            properties: {
                //!!!
                baseUrl: '/',

                //!!!
                requestUrl: params.url,

                //!!!
                originUrl: params.origin,

                //!!!
                preboot: false,

                //!!!
                document: '<sample-app1></sample-app1>'
            },

            //!!!
            onHandleError: (parentZone, currentZone, targetZone, error) => {
                // If any error occurs while rendering the module, reject the whole operation
                reject(error);
                return true;
            }
        });

        return requestZone.run<Promise<string>>(() => platform.serializeModule(AppModule)).then(html => {
            resolve({ html: html });
        }, reject);
    });
});