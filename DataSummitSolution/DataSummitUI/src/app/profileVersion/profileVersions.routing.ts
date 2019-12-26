import { Routes, RouterModule } from "@angular/router";
import { NgModule } from "@angular/core";
import { ProfileVersionsComponent } from "./components/profileVersions.component";
import { CreateProfileVersionComponent } from "./create/components/createProfileVersion.component";
import { ProfileAttributesComponent } from "../profileAttributes/components/profileAttributes.component";
import { StandardAttributeComponent } from "../standardAttribute/components/standardAttribute.component";

const archiveRoutes: Routes = [
  { path: ":companyId", component: ProfileVersionsComponent },
  { path: "profileattributes/:profileVersionId", component: ProfileAttributesComponent },
  { path: "profileversions/create/:companyId", component: CreateProfileVersionComponent },
  { path: "standardAttributes/:profileVersionId", component: StandardAttributeComponent } //, canActivate: [AuthGuardService] }
];

@NgModule({
  imports: [
    RouterModule.forChild(archiveRoutes)
  ],
  exports: [
    RouterModule
  ]
})
export class ProfileVersionsRoutingModule { }
