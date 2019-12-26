import { NgModule } from '@angular/core';

// import { PricingComponent } from './components/pricing.component';
import { PricingRoutingModule } from './pricing.routing';
import { CommonModule } from '@angular/common';
import { ReactiveFormsModule, FormsModule } from '@angular/forms';

@NgModule({
    imports: [
        CommonModule,
        PricingRoutingModule,
        FormsModule,
        ReactiveFormsModule
    ],
    exports: [],
    // declarations: [PricingComponent],
    providers: [],
})
export class PricingModule { }
