import { Routes, RouterModule } from "@angular/router";
import { NgModule, ModuleWithProviders } from "@angular/core";
import { ArchiveComponent } from "./components/archive.component";
import { ArchiveDetailsComponent } from "./components/archive-details/archive-details.component";
import { OrderCollectionComponent } from "./components/order-collection/order-collection.component";
import { CollectionOrderFormComponent } from "./components/collection-order-form/collection-order-form.component";

const archiveRoutes: Routes = [
    { path: "", component: ArchiveComponent },
    { path: "details/:id", component: ArchiveDetailsComponent },
    { path: "order-collection", component: OrderCollectionComponent },
    { path: "collection-order-form", component: CollectionOrderFormComponent }
];

@NgModule({
    imports: [
      RouterModule.forChild(archiveRoutes)
    ],
    exports: [
      RouterModule
    ]
  })
  export class ArchiveRoutingModule { }