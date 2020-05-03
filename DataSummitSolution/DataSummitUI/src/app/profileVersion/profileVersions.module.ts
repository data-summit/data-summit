// Angular Imports
import { NgModule } from '@angular/core';

// This Module's Components
import { CommonModule } from '@angular/common';
import { MDBBootstrapModule } from 'angular-bootstrap-md';
import { ReactiveFormsModule, FormsModule } from '@angular/forms';
import { DataSummitSharedModule } from '../shared/data-summit-shared.module';
import { ApiService } from '../shared/services/api.service';

import { FileUploadModule } from "primeng/fileupload"
import { ButtonModule } from "primeng/button"
import { TooltipModule } from "primeng/tooltip"
import { ProfileVersionsComponent } from './components/profileVersions.component';
import { CreateProfileVersionComponent } from './create/components/createProfileVersion.component';
import { ProfileAttributesModule } from '../profileAttributes/profileAttributes.module';
import { StandardAttributeModule } from '../../obsolete/standardAttribute/standardAttribute.module';

@NgModule({
    imports: [
        CommonModule,
        FormsModule,
        ReactiveFormsModule,
        StandardAttributeModule,
        ProfileAttributesModule,
        DataSummitSharedModule,
        MDBBootstrapModule,
        FileUploadModule,
        ButtonModule,
        TooltipModule
    ],
    exports: [
        ProfileVersionsComponent,
        CreateProfileVersionComponent
    ],
    declarations: [
        ProfileVersionsComponent,
        CreateProfileVersionComponent
    ],
    providers: [
        ApiService
    ],
})
export class ProfileVersionsModule { }
