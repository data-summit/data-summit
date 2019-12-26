import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { RouterModule } from '@angular/router';


import { HeaderComponent } from './components/header.component';
import { FooterComponent } from './components/footer.component';
import { NavbarModule, WavesModule } from 'angular-bootstrap-md';

@NgModule({
  imports: [
    CommonModule,
    RouterModule,
    NavbarModule,
    WavesModule
  ],
  declarations: [
    HeaderComponent, 
    FooterComponent
  ],
  exports: [
    HeaderComponent, 
    FooterComponent
  ]
})
export class LayoutModule { }
