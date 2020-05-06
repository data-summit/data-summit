// Angular Imports
import { NgModule } from '@angular/core';

// This Module's Components
import { StandardAttributeRoutingModule } from './standardAttribute.routing';
import { StandardAttributeComponent } from './components/standardAttribute.component';
import { CommonModule } from '@angular/common';
import { WavesModule, CheckboxModule, MDBBootstrapModule, TooltipModule } from 'angular-bootstrap-md';
import { DataSummitSharedModule } from '../../app/shared/data-summit-shared.module';
import { ReactiveFormsModule, FormsModule } from '@angular/forms';
import { ApiService } from '../../app/shared/services/api.service';

@NgModule({
    imports: [
        CommonModule,
        FormsModule,
        ReactiveFormsModule,
        DataSummitSharedModule,
        //ProfileVersionsModule,
        MDBBootstrapModule,
        TooltipModule
    ],
    exports: [
        StandardAttributeComponent
    ],
    declarations: [
        StandardAttributeComponent,
    ],
    providers: [
        ApiService
    ],
})
export class StandardAttributeModule { }
