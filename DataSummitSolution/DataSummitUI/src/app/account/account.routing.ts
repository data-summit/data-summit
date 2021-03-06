import { Routes, RouterModule } from "@angular/router";
import { AccountComponent} from "./components/account.component"
import { NgModule } from "@angular/core";
import { LoginComponent } from "./components/login/login.component";
import { RegisterComponent } from "./components/register/register.component";
import { VerifyAccountComponent } from "./components/verify/verify-account.component";
import { TwoFactorComponent } from './components/twofactor/twofactor.component';

const accountRoutes: Routes = [
    { path: "", component: AccountComponent },
    { path: "login", component: LoginComponent },
    { path: "register", component: RegisterComponent },
    { path: "verify-account", component: VerifyAccountComponent },
    { path: "two-factor", component: TwoFactorComponent },
];

@NgModule({
    imports: [
      RouterModule.forChild(accountRoutes)
    ],
    exports: [
      RouterModule
    ]
  })
  export class AccountRoutingModule { }
