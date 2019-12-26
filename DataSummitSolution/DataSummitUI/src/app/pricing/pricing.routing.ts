import { Routes, RouterModule } from "@angular/router";
// import { PricingComponent} from "./components/pricing.component"
import { NgModule } from "@angular/core";

const pricingRoutes: Routes = [
    // { path: "", component: PricingComponent },
];

@NgModule({
    imports: [
      RouterModule.forChild(pricingRoutes)
    ],
    exports: [
      RouterModule
    ]
  })
  export class PricingRoutingModule { }
