//Angular
import { BrowserModule } from '@angular/platform-browser';
import { NgModule, NO_ERRORS_SCHEMA } from '@angular/core';
import { BrowserAnimationsModule } from "@angular/platform-browser/animations";
import { MDBBootstrapModule } from "angular-bootstrap-md";
//Modules
import { LayoutModule } from './layout/layout.module';
import { HomeModule } from "./home/home.module";
//Routing
import { AppRoutingModule } from "./app.routing";
//Components
import { AppComponent } from './app.component';
import { ApiService } from './shared/services/api.service';
import { HttpClientModule } from '@angular/common/http';
import { AuthService } from './shared/services/auth.service';
import { AuthGuardService } from './shared/services/auth-guard.service';
import { RegisterFormService } from './shared/services/register-form.service';
import { MessageService } from "primeng/primeng";
import { NotifyService } from './shared/services/notify.service';
import { GrowlModule } from 'primeng/primeng';

@NgModule({
  declarations: [
    AppComponent
  ],
  imports: [
    BrowserModule,
    BrowserAnimationsModule,
    HttpClientModule,
    MDBBootstrapModule.forRoot(),
    AppRoutingModule,
    LayoutModule,
    HomeModule,
    GrowlModule
  ],
  schemas: [
    NO_ERRORS_SCHEMA
  ],
  providers: [
    MessageService,
    NotifyService,
    AuthService,
    AuthGuardService,
    RegisterFormService
  ],
  bootstrap: [AppComponent]
})
export class AppModule { }

