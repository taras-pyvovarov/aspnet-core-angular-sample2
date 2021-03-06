//Root module, AppModule by convension.
//Angular module class describes how the application parts fit together.

import { NgModule } from '@angular/core';
import { UniversalModule } from 'angular2-universal';
import { RouterModule } from '@angular/router';

//App components.
import { AppComponent } from './components/root-component/root-component';
import { TopMenuComponent } from './components/top-menu-component/top-menu-component';
import { HomeComponent } from './components/home-component/home-component';
import { CalculatorComponent } from './components/calculator-component/calculator-component';

//Decorator function. Properties contain metadata about module.
@NgModule({
    //!!!
    bootstrap: [AppComponent],

    //Declaration of all used components.
    declarations: [
        AppComponent, 
        TopMenuComponent, 
        HomeComponent, 
        CalculatorComponent,
    ],

    imports: [
        //Must be first import. This automatically imports BrowserModule, HttpModule, and JsonpModule too.
        UniversalModule,

        //!!!
        RouterModule.forRoot([
            { path: '', component: HomeComponent },
            { path: 'home', component: HomeComponent },
            { path: 'calc', component: CalculatorComponent },
        ])
    ],
})

//Declaration of root module.
export class AppModule { }