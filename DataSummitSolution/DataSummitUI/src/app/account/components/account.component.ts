import { Component, OnInit } from '@angular/core';

@Component({
    selector: 'ds-account',
    templateUrl: 'account.component.html'
})

export class AccountComponent implements OnInit {

    title: string = 'Account Component'

    constructor() { }

    ngOnInit() { }
    
}