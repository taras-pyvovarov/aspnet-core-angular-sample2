import { Component } from '@angular/core';

@Component({
    selector: 'calculator',
    template: require('./calculator-component.html'),
})

export class CalculatorComponent 
{
    var1: string = 'Simplest Angular2 binding';

    func1()
    {
        let a1: string = 'Hello';
        let a2: string = 'from';
        let a3: string = 'function';

        return `${a1} ${a2} ${a3}!`;
    }
}