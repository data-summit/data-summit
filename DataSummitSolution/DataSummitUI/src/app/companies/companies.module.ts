// Angular Imports
import { NgModule } from '@angular/core';

// This Module's Components
import { CompaniesRoutingModule } from './companies.routing';
import { CompaniesComponent } from './components/companies.component';
import { CommonModule } from '@angular/common';
import { WavesModule, CheckboxModule, MDBBootstrapModule, TooltipModule } from 'angular-bootstrap-md';
import { DataSummitSharedModule } from '../shared/data-summit-shared.module';
import { ReactiveFormsModule, FormsModule } from '@angular/forms';
import { ApiService } from '../shared/services/api.service';
import { ProjectsModule } from '../projects/projects.module';
import { ProfileVersionsModule } from '../profileVersion/profileVersions.module';
import { ProfileAttributesModule } from '../profileAttributes/profileAttributes.module';
import { StandardAttributeModule } from '../standardAttribute/standardAttribute.module';
import { DrawingsModule } from '../drawings/drawings.module';
import { PropertiesComponent } from '../properties/components/properties.component';
import { PropertiesModule } from '../properties/properties.module';

@NgModule({
    imports: [
        CommonModule,
        FormsModule,
        ReactiveFormsModule,
        CompaniesRoutingModule,
        DataSummitSharedModule,
        ProjectsModule,
        DrawingsModule,
        PropertiesModule,
        MDBBootstrapModule,
        TooltipModule,
        ProfileVersionsModule,
        ProfileAttributesModule,
        StandardAttributeModule
    ],
    exports: [],
    declarations: [
        CompaniesComponent
    ],
    providers: [
        ApiService
    ],
})
export class CompaniesModule { }
