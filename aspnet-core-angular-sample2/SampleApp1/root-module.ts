//Root module, AppModule by convension.
//Angular module class describes how the application parts fit together.

import { NgModule } from '@angular/core';
import { UniversalModule } from 'angular2-universal';
import { RouterModule } from '@angular/router';

import { AppComponent } from './components/root-component/root-component';

//Decorator function. Properties contain metadata about module.
@NgModule({
    bootstrap: [AppComponent],
    declarations: [AppComponent],
    imports: [
        UniversalModule,// Must be first import. This automatically imports BrowserModule, HttpModule, and JsonpModule too.
        RouterModule.forRoot([
            { path: '', component: AppComponent },
        ])
    ],
})

//Dow we need this???
export class AppModule { }