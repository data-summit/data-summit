import { NgModule } from '@angular/core';


import { AccountComponent } from './components/account.component';
import { AccountRoutingModule } from './account.routing';
import { CommonModule } from '@angular/common';
import { LoginComponent } from './components/login/login.component';
import { RegisterComponent } from './components/register/register.component';
import { CheckboxModule, ButtonsModule, CardsFreeModule } from 'angular-bootstrap-md';
import { ApiService } from '../shared/services/api.service';
import { ReactiveFormsModule, FormsModule } from '@angular/forms';
import { HttpClientModule } from '@angular/common/http';
import { VerifyAccountComponent } from './components/verify/verify-account.component';

@NgModule({
    imports: [
        CommonModule,
        HttpClientModule,
        ReactiveFormsModule,
        FormsModule,
        AccountRoutingModule,
        CheckboxModule,
        ButtonsModule,
        CardsFreeModule
    ],
    exports: [],
    declarations: [
        AccountComponent,
        LoginComponent,
        RegisterComponent,
        VerifyAccountComponent
    ],
    providers: [
        ApiService
    ],
})
export class AccountModule { }
