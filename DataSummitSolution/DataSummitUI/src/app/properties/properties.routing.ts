import { Routes, RouterModule } from "@angular/router";
import { NgModule } from "@angular/core";
import { PropertiesComponent } from "./components/properties.component";

const archiveRoutes: Routes = [
  { path: ":propertyId", component: PropertiesComponent },
];

@NgModule({
  imports: [
    RouterModule.forChild(archiveRoutes)
  ],
  exports: [
    RouterModule
  ]
})
export class PropertiesRoutingModule { }
