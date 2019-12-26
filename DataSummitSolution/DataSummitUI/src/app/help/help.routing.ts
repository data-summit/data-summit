import { Routes, RouterModule } from "@angular/router";
import { HelpComponent} from "./components/help.component"
import { NgModule } from "@angular/core";

const helpRoutes: Routes = [
    { path: "", component: HelpComponent },
];

@NgModule({
    imports: [
      RouterModule.forChild(helpRoutes)
    ],
    exports: [
      RouterModule
    ]
  })
  export class HelpRoutingModule { }
