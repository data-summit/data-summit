import { NgModule } from '@angular/core';

import { HelpComponent } from './components/help.component';
import { HelpRoutingModule } from './help.routing';
import { CommonModule } from '@angular/common';

@NgModule({
    imports: [
        CommonModule,
        HelpRoutingModule
    ],
    exports: [],
    declarations: [HelpComponent],
    providers: [],
})
export class HelpModule { }
