import { Routes, RouterModule } from "@angular/router";
import { NgModule } from "@angular/core";
import { StandardAttributeComponent } from "./components/standardAttribute.component";
import { ProfileVersionsComponent } from "../../app/profileVersion/components/profileVersions.component";

const archiveRoutes: Routes = [
  { path: ":profileVersionId", component: StandardAttributeComponent },
];

@NgModule({
  imports: [
    RouterModule.forChild(archiveRoutes)
  ],
  exports: [
    RouterModule
  ]
})
export class StandardAttributeRoutingModule { }