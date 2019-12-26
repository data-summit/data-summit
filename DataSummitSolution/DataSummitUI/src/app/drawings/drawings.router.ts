import { Routes, RouterModule } from "@angular/router";
import { NgModule } from "@angular/core";
import { DrawingsComponent } from "../drawings/components/drawings.component";
import { PropertiesComponent } from "../properties/components/properties.component";

const archiveRoutes: Routes = [
  { path: ":projectId", component: DrawingsComponent },
  { path: "properties/:drawingId", component: PropertiesComponent }
];

@NgModule({
  imports: [
    RouterModule.forChild(archiveRoutes)
  ],
  exports: [
    RouterModule
  ]
})
export class DrawingsRoutingModule { }
