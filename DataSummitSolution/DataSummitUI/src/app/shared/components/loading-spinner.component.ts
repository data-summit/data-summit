import { Component, OnInit, Input } from '@angular/core';

@Component({
    selector: 'loading-spinner',
    templateUrl: './loading-spinner.component.html'
})
export class LoadingSpinnerComponent {

    @Input() loading: boolean = false;
    @Input() message: string = "Please wait...";

}