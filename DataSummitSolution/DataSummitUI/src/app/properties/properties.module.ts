// Angular Imports
import { NgModule } from '@angular/core';

import { PropertiesComponent } from './components/properties.component';
import { CommonModule } from '@angular/common';
import { MDBBootstrapModule } from 'angular-bootstrap-md';
import { ReactiveFormsModule, FormsModule } from '@angular/forms';
import { DataSummitSharedModule } from '../shared/data-summit-shared.module';
import { ApiService } from '../shared/services/api.service';

import { ButtonModule } from "primeng/button"
import { TooltipModule } from "primeng/tooltip"

@NgModule({
    imports: [
        CommonModule,
        FormsModule,
        ReactiveFormsModule,
        DataSummitSharedModule,
        MDBBootstrapModule,
        ButtonModule,
        TooltipModule
    ],
    exports: [
        PropertiesComponent
    ],
    declarations: [
        PropertiesComponent
    ],
    providers: [
        ApiService
    ],
})
export class PropertiesModule { }
