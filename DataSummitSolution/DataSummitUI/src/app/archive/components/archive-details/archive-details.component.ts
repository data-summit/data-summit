import { Component, OnInit, ViewChild } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';
import { IndustryType } from '../../../enums/industry-type.enum';
import { FormGroup, FormBuilder, Validators } from '@angular/forms';
import { IndustryVariablesService } from '../../../shared/services/industry-variables.service';
import { IndustryVariables } from '../../../shared/models/industry-variables';
import { ModalContainerComponent, MDBModalRef } from 'angular-bootstrap-md';
import { RegisterFormService } from 'src/app/shared/services/register-form.service';
import { ApiService } from 'src/app/shared/services/api.service';
import { take } from 'rxjs/operators';
import { NotifyService } from 'src/app/shared/services/notify.service';

@Component({
  selector: 'app-archive-details',
  templateUrl: './archive-details.component.html',
  styleUrls: ['./archive-details.component.scss']
})
export class ArchiveDetailsComponent implements OnInit {

  @ViewChild('termsModal') termsModal

  industry: IndustryType;

  detailsForm: FormGroup
  industryInfo: IndustryVariables;

  registerLoading: boolean;

  constructor(private router: Router,
    private route: ActivatedRoute,
    private fb: FormBuilder,
    private industryVars: IndustryVariablesService,
    private registerFormService: RegisterFormService,
    private api: ApiService,
    private notifyService: NotifyService) { }

  ngOnInit() {
    this.initReactiveForm()

    this.route.params.subscribe(param => {
      this.industryVars.setIndustry(parseInt(param.id))
      this.industryInfo = this.industryVars.getIndustryInfo()
    })
  }

  initReactiveForm() {
    this.detailsForm = this.fb.group({
      street: this.fb.control('', Validators.required),
      street2: this.fb.control(''),
      townCity: this.fb.control('', Validators.required),
      county: this.fb.control('', Validators.required),
      postCode: this.fb.control('', Validators.required),
      doB: this.fb.control('', Validators.required),
      company: this.fb.control('', Validators.required),
      jobRole: this.fb.control('', Validators.required)
    })
  }

  submitForm() {
    console.log(this.detailsForm);
    this.termsModal.show()
  }

  acceptTerms() {
    // this.router.navigate(["/projects"]);
    this.registerFormService.detailsForm = this.detailsForm
    let registerObj = this.registerFormService.getRegisterObject()
    console.log(registerObj);
    this.registerLoading = true;
    this.api.post('api/users/register', registerObj)
      .pipe(take(1))
      .subscribe(result => {
        this.registerLoading = false;
        this.notifyService.success('User Registered, Please log in')
        this.router.navigate(["/account/login"]);
      }, error => {
        this.notifyService.error('Unable To Register User')
        this.registerLoading = false;
        console.log(error)
      })
  }

}
