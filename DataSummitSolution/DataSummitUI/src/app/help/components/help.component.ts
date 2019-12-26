import { Component, OnInit } from '@angular/core';

@Component({
    selector: 'ds-help',
    templateUrl: 'help.component.html'
})

export class HelpComponent implements OnInit {

    title: string = 'Help Component'

    constructor() { }

    ngOnInit() { }
    
}