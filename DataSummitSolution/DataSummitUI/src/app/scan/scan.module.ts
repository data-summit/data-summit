import { NgModule } from '@angular/core';

import { ScanComponent } from './components/scan.component';
import { ScanRoutingModule } from './scan.routing';

@NgModule({
    imports: [
        ScanRoutingModule
    ],
    exports: [],
    declarations: [ScanComponent],
    providers: [],
})
export class ScanModule { }
