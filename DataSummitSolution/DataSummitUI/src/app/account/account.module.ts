import { NgModule } from '@angular/core';

import { AccountComponent } from './components/account.component';
import { AccountRoutingModule } from './account.routing';
import { CommonModule } from '@angular/common';
import { LoginComponent } from './components/login/login.component';
import { RegisterComponent } from './components/register/register.component';
import { CheckboxModule, ButtonsModule, CardsModule } from 'angular-bootstrap-md';
import { ApiService } from '../shared/services/api.service';
import { ReactiveFormsModule, FormsModule } from '@angular/forms';
import { HttpClientModule } from '@angular/common/http';
import { VerifyAccountComponent } from './components/verify/verify-account.component';
import { TwoFactorComponent } from './components/twofactor/twofactor.component';

@NgModule({
    imports: [
        CommonModule,
        HttpClientModule,
        ReactiveFormsModule,
        FormsModule,
        AccountRoutingModule,
        CheckboxModule,
        ButtonsModule,
        CardsModule
    ],
    exports: [],
    declarations: [
        AccountComponent,
        LoginComponent,
        RegisterComponent,
        VerifyAccountComponent,
        TwoFactorComponent
    ],
    providers: [
        ApiService
    ],
})
export class AccountModule { }
