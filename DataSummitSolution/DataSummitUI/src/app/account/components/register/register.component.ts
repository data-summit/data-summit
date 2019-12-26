import { Component, OnInit } from '@angular/core';
import { ApiService } from '../../../shared/services/api.service';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { take } from 'rxjs/operators';
import { PasswordMatchValidation } from '../../../validators/password-match.validator';
import { SimpleEmailValidator } from '../../../validators/simple-email.validator';
import { RegisterFormService } from 'src/app/shared/services/register-form.service';
import { TelephoneCodeService } from 'src/app/shared/datasets/telephone-codes';
import { TelephoneCode } from 'src/app/shared/models/telephone-codes';

@Component({
    selector: 'ds-register',
    templateUrl: 'register.component.html'
})
export class RegisterComponent implements OnInit {

    registerForm: FormGroup
    allCodes: TelephoneCode[];

    constructor(private apiService: ApiService,
        private fb: FormBuilder,
        private telCode: TelephoneCodeService,
        private registerFormService: RegisterFormService) {

    }

    ngOnInit() {
        this.allCodes = this.telCode.getPhoneCodes();
        this.initRegisterForm()
    }

    initRegisterForm() {
        this.registerForm = this.fb.group({
            firstName: this.fb.control(''),
            lastName: this.fb.control(''),
            email: this.fb.control('', [Validators.required, SimpleEmailValidator()]),
            password: this.fb.control('', Validators.required),
            confirmPassword: this.fb.control('', Validators.required),
            telCode: this.fb.control(''),
            phone: this.fb.control('', Validators.required),
        })
        this.registerForm.setValidators(PasswordMatchValidation.MatchPassword);
        let savedForm = this.registerFormService.registerForm;
        if (savedForm) {
            this.registerForm.patchValue(savedForm.value)
        }
    }

    register() {
        const formValue = {
            grant_type: "client_credentials",
            username: this.registerForm.get('email').value,
            password: this.registerForm.get('password').value,
            client_id: "postmanUserPwd",
            scope: "values",
            client_secret: "DataSummitUISecret"
        }
        const url: string = 'api/account/register'

        console.log(formValue)
        this.apiService.post(url, formValue)
            .pipe(take(1))
            .subscribe(result => {
                //Do something with result (store token in session?)
                console.log(result)
            }, error => {
                console.log(error)
            })
    }

}
