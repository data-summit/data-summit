// Angular Imports
import { NgModule } from '@angular/core';

import { DocumentsComponent } from '../documents/components/documents.component';
import { CommonModule } from '@angular/common';
import { MDBBootstrapModule } from 'angular-bootstrap-md';
import { ReactiveFormsModule, FormsModule } from '@angular/forms';
import { DataSummitSharedModule } from '../shared/data-summit-shared.module';
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
        DocumentsComponent
    ],
    declarations: [
        DocumentsComponent
    ],
    providers: [
        ApiService
    ],
})
export class DocumentsModule { }
