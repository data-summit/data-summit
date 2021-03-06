import { Routes, RouterModule } from "@angular/router";
import { NgModule } from "@angular/core";
import { ProjectsComponent } from "./components/projects.component";
import { ProfileVersionsComponent } from "../profileVersion/components/profileVersions.component";
import { DocumentsComponent } from "../documents/components/documents.component";

const archiveRoutes: Routes = [
  { path: ":companyId", component: ProjectsComponent },
  { path: ":companyId/projects/:projectId/profileversions", component: ProfileVersionsComponent },
  { path: ":companyId/profileversions", component: ProfileVersionsComponent },
  { path: "documents/:projectId", component: DocumentsComponent }
];

@NgModule({
  imports: [
    RouterModule.forChild(archiveRoutes)
  ],
  exports: [
    RouterModule
  ]
})
export class ProjectsRoutingModule { }
