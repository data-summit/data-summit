// Angular Imports
import { NgModule } from '@angular/core';

// This Module's Components
import { ProjectsRoutingModule } from './projects.routing';
import { ProjectsComponent } from './components/projects.component';
import { CommonModule } from '@angular/common';
import { MDBBootstrapModule } from 'angular-bootstrap-md';
import { ReactiveFormsModule, FormsModule } from '@angular/forms';
import { DataSummitSharedModule } from '../shared/data-summit-shared.module';
import { ApiService } from '../shared/services/api.service';

import { FileUploadModule } from "primeng/fileupload"
import { ButtonModule } from "primeng/button"
import { TooltipModule } from "primeng/tooltip"
import { DocumentsModule } from '../documents/documents.module';

@NgModule({
    imports: [
        CommonModule,
        FormsModule,
        ReactiveFormsModule,
        ProjectsRoutingModule,
        DocumentsModule,
        DataSummitSharedModule,
        MDBBootstrapModule,
        FileUploadModule,
        ButtonModule,
        TooltipModule,
    ],
    exports: [
        ProjectsComponent
    ],
    declarations: [
        ProjectsComponent
    ],
    providers: [
        ApiService
    ],
})
export class ProjectsModule { }
