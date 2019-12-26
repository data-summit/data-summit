// Angular Imports
import { NgModule } from '@angular/core';

// This Module's Components
import { PropertiesRoutingModule } from './properties.routing';
import { PropertiesComponent } from './components/properties.component';
import { CommonModule } from '@angular/common';
import { WavesModule, CheckboxModule, MDBBootstrapModule } from 'angular-bootstrap-md';
import { ReactiveFormsModule, FormsModule } from '@angular/forms';
import { DataSummitSharedModule } from '../shared/data-summit-shared.module';
import { TableModule } from 'angular-bootstrap-md/angular-bootstrap-md/tables/index';
import { ApiService } from '../shared/services/api.service';

import { FileUploadModule } from "primeng/fileupload"
import { ButtonModule } from "primeng/button"
import { TooltipModule } from "primeng/tooltip"

@NgModule({
    imports: [
        CommonModule,
        FormsModule,
        ReactiveFormsModule,
        DataSummitSharedModule,
        MDBBootstrapModule,
        FileUploadModule,
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
