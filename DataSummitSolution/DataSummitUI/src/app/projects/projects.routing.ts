import { Routes, RouterModule } from "@angular/router";
import { NgModule } from "@angular/core";
import { ProjectsComponent } from "./components/projects.component";
import { DrawingsComponent } from "../drawings/components/drawings.component";

const archiveRoutes: Routes = [
  { path: ":companyId", component: ProjectsComponent },
  { path: "drawings/:projectId", component: DrawingsComponent }
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
