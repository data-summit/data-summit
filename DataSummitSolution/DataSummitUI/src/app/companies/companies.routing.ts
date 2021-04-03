import { Routes, RouterModule } from "@angular/router";
import { NgModule } from "@angular/core";
import { CompaniesComponent } from "./components/companies.component";
import { ProjectsComponent } from "../projects/components/projects.component";
import { ProfileVersionsComponent } from "../profileVersion/components/profileVersions.component";
import { ProfileAttributesComponent } from "../profileAttributes/components/profileAttributes.component";
import { DocumentsComponent } from "../documents/components/documents.component";
import { PropertiesComponent } from "../properties/components/properties.component";
import { StandardAttributeComponent } from "../../obsolete/standardAttribute/components/standardAttribute.component";
import { CreateProfileVersionComponent } from "../profileVersion/create/components/createProfileVersion.component";
//import { Template } from "@angular/compiler/src/render3/r3_ast";


const archiveRoutes: Routes = [
  { path: "", component: CompaniesComponent },
  { path: ":companyId/profileversions", component: ProfileVersionsComponent },
  { path: ":companyId/profileversions/:profileVersionId/profileAttributes", component: ProfileAttributesComponent },
  { path: ":companyId/profileversions/:profileVersionId/standardAttributes", component: StandardAttributeComponent },
  { path: ":companyId/profileversions/create", component: CreateProfileVersionComponent},
  { path: ":companyId/projects", component: ProjectsComponent },
  { path: ":companyId/projects/:projectId/documents", component: DocumentsComponent },
  { path: ":companyId/projects/:projectId/documents/:documentId/properties", component: PropertiesComponent }
];

@NgModule({
  imports: [
    RouterModule.forChild(archiveRoutes)
  ],
  exports: [
    RouterModule
  ]
})
export class CompaniesRoutingModule { }