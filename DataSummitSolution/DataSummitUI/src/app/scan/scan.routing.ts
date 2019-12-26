import { Routes, RouterModule } from "@angular/router";
import { ScanComponent } from "./components/scan.component"
import { ModuleWithProviders, NgModule } from "@angular/core";

const scanRoutes: Routes = [
    { path: "", component: ScanComponent },
];

@NgModule({
    imports: [
      RouterModule.forChild(scanRoutes)
    ],
    exports: [
      RouterModule
    ]
  })
  export class ScanRoutingModule { }
