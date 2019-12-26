import { Routes, RouterModule } from "@angular/router";
import { AboutUsComponent} from "./components/aboutus.component"
import { NgModule } from "@angular/core";

const helpRoutes: Routes = [
    { path: "", component: AboutUsComponent },
];

@NgModule({
    imports: [
      RouterModule.forChild(helpRoutes)
    ],
    exports: [
      RouterModule
    ]
  })
  export class AboutUsRoutingModule { }
