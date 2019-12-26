import { NgModule } from '@angular/core';

import { ArchiveComponent } from './components/archive.component';
import { ArchiveRoutingModule } from './archive.routing';
import { CommonModule } from '@angular/common';
import { WavesModule, ButtonsModule, CheckboxModule, ModalModule, TooltipModule, MDBRootModule, MDBBootstrapModule } from 'angular-bootstrap-md';;
import { ArchiveDetailsComponent } from './components/archive-details/archive-details.component'
    ;
import { OrderCollectionComponent } from './components/order-collection/order-collection.component'
import { ReactiveFormsModule, FormsModule } from '@angular/forms';
import { IndustryVariablesService } from '../shared/services/industry-variables.service';
import { DataSummitSharedModule } from '../shared/data-summit-shared.module';
import { CollectionOrderFormComponent } from './components/collection-order-form/collection-order-form.component';
import { ApiService } from '../shared/services/api.service';

@NgModule({
    imports: [
        CommonModule,
        ReactiveFormsModule,
        FormsModule,
        ArchiveRoutingModule,
        DataSummitSharedModule,
        MDBBootstrapModule
    ],
    exports: [],
    declarations: [
        ArchiveComponent,
        ArchiveDetailsComponent,
        OrderCollectionComponent,
        CollectionOrderFormComponent
    ],
    providers: [
        IndustryVariablesService,
        ApiService
    ],
})
export class ArchiveModule { }
