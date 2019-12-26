// Angular Imports
import { NgModule } from '@angular/core';

// This Module's Components
import { DrawingsRoutingModule } from './drawings.router';
import { DrawingsComponent } from '../drawings/components/drawings.component';
import { CommonModule } from '@angular/common';
import { WavesModule, CheckboxModule, MDBBootstrapModule } from 'angular-bootstrap-md';
import { ReactiveFormsModule, FormsModule } from '@angular/forms';
import { DataSummitSharedModule } from '../shared/data-summit-shared.module';
import { TableModule } from 'angular-bootstrap-md/angular-bootstrap-md/tables/index';
import { ApiService } from '../shared/services/api.service';

import { FileUploadModule } from "primeng/fileupload"
import { ButtonModule } from "primeng/button"
import { TooltipModule } from "primeng/tooltip"
import { PropertiesModule } from '../properties/properties.module';

@NgModule({
    imports: [
        CommonModule,
        FormsModule,
        ReactiveFormsModule,
        PropertiesModule,
        DataSummitSharedModule,
        MDBBootstrapModule,
        FileUploadModule,
        ButtonModule,
        TooltipModule
    ],
    exports: [
        DrawingsComponent
    ],
    declarations: [
        DrawingsComponent
    ],
    providers: [
        ApiService
    ],
})
export class DrawingsModule { }
