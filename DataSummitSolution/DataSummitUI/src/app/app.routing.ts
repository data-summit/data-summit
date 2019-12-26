import { Routes, RouterModule, PreloadAllModules } from "@angular/router";
import { HomeComponent } from "./home/components/home.component"
import { NgModule } from "@angular/core";
import { TermsAndConditionsComponent } from "./home/components/terms-and-conditions/terms-and-conditions.component";
import { AuthGuardService } from "./shared/services/auth-guard.service";

const appRoutes: Routes = [
    { path: "", redirectTo: "/home", pathMatch: "full" },
    { path: "home", component: HomeComponent },
    { path: "terms-and-conditions", component: TermsAndConditionsComponent, canActivate: [AuthGuardService]},
    { path: "scan", loadChildren: "./scan/scan.module#ScanModule", canActivate: [AuthGuardService]},
    { path: "archive", loadChildren: "./archive/archive.module#ArchiveModule" },
    { path: "account", loadChildren: "./account/account.module#AccountModule" },
    { path: "help", loadChildren: "./help/help.module#HelpModule" },
    // { path: "pricing", loadChildren: "./pricing/pricing.module#PricingModule" },
    { path: "aboutus", loadChildren: "./aboutus/aboutus.module#AboutUsModule" },
    { path: "companies", loadChildren: "./companies/companies.module#CompaniesModule" },
];

const config = {
    useHash: false,
    enableTracing: false, // Turn this on to log routing events to the console
    preloadingStrategy: PreloadAllModules
}
  
  @NgModule({
    imports: [
      RouterModule.forRoot(
        appRoutes, config
      )
    ],
    exports: [
      RouterModule
    ],
    providers: [
      AuthGuardService
    ]
  })
  export class AppRoutingModule { }
