// Angular Imports
import { NgModule } from '@angular/core';

// This Module's Components
import { ProjectsRoutingModule } from './projects.routing';
import { ProjectsComponent } from './components/projects.component';
import { CommonModule } from '@angular/common';
import { WavesModule, CheckboxModule, MDBBootstrapModule } from 'angular-bootstrap-md';
import { ReactiveFormsModule, FormsModule } from '@angular/forms';
import { DataSummitSharedModule } from '../shared/data-summit-shared.module';
import { TableModule } from 'angular-bootstrap-md/angular-bootstrap-md/tables/index';
import { ApiService } from '../shared/services/api.service';

import { FileUploadModule } from "primeng/fileupload"
import { ButtonModule } from "primeng/button"
import { TooltipModule } from "primeng/tooltip"
import { DrawingsModule } from '../drawings/drawings.module';
import { DrawingsComponent } from '../drawings/components/drawings.component';

@NgModule({
    imports: [
        CommonModule,
        FormsModule,
        ReactiveFormsModule,
        ProjectsRoutingModule,
        DrawingsModule,
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
