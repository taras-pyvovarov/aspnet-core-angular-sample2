import { Component } from '@angular/core';

@Component({
    selector: 'calculator',
    template: require('./calculator-component.html'),
    styles: [require('./calculator-component.scss')],
})

export class CalculatorComponent
{
    var1: string = 'Simplest Angular2 binding';
    generatedNumber: number;

    func1() {
        let a1: string = 'Hello';
        let a2: string = 'from';
        let a3: string = 'function';

        return `${a1} ${a2} ${a3}!`;
    };

    generateNumber() {
        let tempNum: number = Math.random();
        this.generatedNumber = tempNum;
    }
}