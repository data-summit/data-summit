import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { HomeComponent } from './components/home.component';

import { CarouselModule, WavesModule, ButtonsModule, InputsModule, TooltipModule, IconsModule } from 'angular-bootstrap-md';
import { ReactiveFormsModule, FormsModule } from '@angular/forms';;
import { TermsAndConditionsComponent } from './components/terms-and-conditions/terms-and-conditions.component'
import { DataSummitSharedModule } from '../shared/data-summit-shared.module';
import { TelephoneCodeService } from '../shared/datasets/telephone-codes';
//Input masking
import { PhoneMaskDirective } from '../shared/masks/phone.directive';

@NgModule({
  imports: [
    CommonModule,
    ReactiveFormsModule,
    FormsModule,
    DataSummitSharedModule,
    CarouselModule,
    ButtonsModule,
    WavesModule,
    InputsModule,
    TooltipModule,
    IconsModule
  ],
  declarations: [
    HomeComponent,
    TermsAndConditionsComponent,
    PhoneMaskDirective
  ],
  providers: [
    TelephoneCodeService
  ],
  exports: [
    HomeComponent,
    PhoneMaskDirective
  ]
})
export class HomeModule { }
