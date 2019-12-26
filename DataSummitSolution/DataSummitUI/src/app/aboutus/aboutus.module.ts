import { NgModule } from '@angular/core';

import { AboutUsComponent } from './components/aboutus.component';
import { AboutUsRoutingModule } from './aboutus.routing';
import { CommonModule } from '@angular/common';

@NgModule({
    imports: [
        CommonModule,
        AboutUsRoutingModule
    ],
    exports: [],
    declarations: [AboutUsComponent],
    providers: [],
})
export class AboutUsModule { }
