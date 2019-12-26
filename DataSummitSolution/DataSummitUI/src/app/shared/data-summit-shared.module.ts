//Angular
import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { TermsAndConditionsTextComponent } from './components/terms-and-conditions-text.component';
// import { RegisterFormService } from './services/register-form.service';
import { LoadingSpinnerComponent } from "./components/loading-spinner.component";

@NgModule({
  imports: [
    CommonModule
  ],
  declarations: [
    TermsAndConditionsTextComponent,
    LoadingSpinnerComponent
  ],
  exports: [
    TermsAndConditionsTextComponent,
    LoadingSpinnerComponent
  ],
  providers: [
    // RegisterFormService
  ]
})
export class DataSummitSharedModule { }
