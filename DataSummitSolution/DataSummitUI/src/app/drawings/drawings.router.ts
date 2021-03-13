import { Routes, RouterModule } from "@angular/router";
import { NgModule } from "@angular/core";
import { DocumentsComponent } from "../documents/components/documents.component";
import { PropertiesComponent } from "../properties/components/properties.component";

const archiveRoutes: Routes = [
  { path: ":projectId", component: DocumentsComponent },
  { path: "properties/:documentId", component: PropertiesComponent }
];

@NgModule({
  imports: [
    RouterModule.forChild(archiveRoutes)
  ],
  exports: [
    RouterModule
  ]
})
export class DocumentsRoutingModule { }
