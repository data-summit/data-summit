// Angular Imports
import { NgModule } from '@angular/core';

// This Module's Components
import { ProfileAttributesRoutingModule } from './profileAttributes.routing';
import { ProfileAttributesComponent } from './components/profileAttributes.component';
import { CommonModule } from '@angular/common';
import { MDBBootstrapModule } from 'angular-bootstrap-md';
import { ReactiveFormsModule, FormsModule } from '@angular/forms';
import { DataSummitSharedModule } from '../shared/data-summit-shared.module';
import { ApiService } from '../shared/services/api.service';

import { FileUploadModule } from "primeng/fileupload"
import { ButtonModule } from "primeng/button"
import { TooltipModule } from "primeng/tooltip"
import { StandardAttributeModule } from '../standardAttribute/standardAttribute.module';

@NgModule({
    imports: [
        CommonModule,
        FormsModule,
        ReactiveFormsModule,
        StandardAttributeModule,
        DataSummitSharedModule,
        MDBBootstrapModule,
        FileUploadModule,
        ButtonModule,
        TooltipModule
    ],
    exports: [
        ProfileAttributesComponent
    ],
    declarations: [
        ProfileAttributesComponent
    ],
    providers: [
        ApiService
    ],
})
export class ProfileAttributesModule { }
