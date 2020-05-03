import { Routes, RouterModule, PreloadAllModules } from "@angular/router";
import { HomeComponent } from "./home/components/home.component"
import { NgModule } from "@angular/core";
import { TermsAndConditionsComponent } from "./home/components/terms-and-conditions/terms-and-conditions.component";
import { AuthGuardService } from "./shared/services/auth-guard.service";

const appRoutes: Routes = [
  { path: "", redirectTo: "/home", pathMatch: "full" },
  { path: "home", component: HomeComponent },
  { path: "terms-and-conditions", component: TermsAndConditionsComponent, canActivate: [AuthGuardService] },
  // { path: "scan", loadChildren: () => import('./scan/scan.module').then(m => m.ScanModule) },
  { path: "archive", loadChildren: () => import('./archive/archive.module').then(m => m.ArchiveModule) },
  { path: "account", loadChildren: () => import('./account/account.module').then(m => m.AccountModule) },
  { path: "help", loadChildren: () => import('./help/help.module').then(m => m.HelpModule) },
  { path: "aboutus", loadChildren: () => import('./aboutus/aboutus.module').then(m => m.AboutUsModule) },
  { path: "companies", loadChildren: () => import('./companies/companies.module').then(m => m.CompaniesModule) },
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
