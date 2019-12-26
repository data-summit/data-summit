import { Component, OnInit } from '@angular/core';
import { Router } from '@angular/router';

@Component({
    selector: 'ds-verify-account',
    templateUrl: 'verify-account.component.html'
})

export class VerifyAccountComponent implements OnInit {
    
    constructor(private router: Router) { }

    ngOnInit() { }

    verify() {
        this.router.navigate(["/projects"])
    }
}