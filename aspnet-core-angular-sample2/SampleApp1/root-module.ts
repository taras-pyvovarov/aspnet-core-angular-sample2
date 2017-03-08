//Root module, AppModule by convension.
//Angular module class describes how the application parts fit together.

import { NgModule } from '@angular/core';
import { UniversalModule } from 'angular2-universal';
import { RouterModule } from '@angular/router';

//App components.
import { AppComponent } from './components/root-component/root-component';
import { HomeComponent } from './components/home-component/home-component';

//Decorator function. Properties contain metadata about module.
@NgModule({
    //!!!
    bootstrap: [AppComponent],

    //!!!
    declarations: [AppComponent, HomeComponent],

    imports: [
        //Must be first import. This automatically imports BrowserModule, HttpModule, and JsonpModule too.
        UniversalModule,

        //!!!
        RouterModule.forRoot([
            { path: '', component: HomeComponent },
            { path: 'home', component: HomeComponent },
        ])
    ],
})

//Declaration of root module.
export class AppModule { }