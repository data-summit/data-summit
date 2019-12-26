import { Routes, RouterModule } from "@angular/router";
import { NgModule } from "@angular/core";
import { ProfileAttributesComponent } from "./components/profileAttributes.component";
import { StandardAttributeComponent } from "../standardAttribute/components/standardAttribute.component";

const archiveRoutes: Routes = [
  { path: ":profileversionId", component: ProfileAttributesComponent },
  { path: "profileVersions/:profileversionId", component: StandardAttributeComponent }
];

@NgModule({
  imports: [
    RouterModule.forChild(archiveRoutes)
  ],
  exports: [
    RouterModule
  ]
})
export class ProfileAttributesRoutingModule { }
