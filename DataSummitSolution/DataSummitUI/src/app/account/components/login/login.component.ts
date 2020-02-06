import { Component, OnInit } from '@angular/core';
import { FormGroup, Validators, FormBuilder } from '@angular/forms';
import { SimpleEmailValidator } from '../../../validators/simple-email.validator';
import { take } from 'rxjs/operators';
import { ApiService, ApiOptions } from '../../../shared/services/api.service';
import { HttpClient, HttpHeaders, HttpParams } from '@angular/common/http';
import { AuthService } from 'src/app/shared/services/auth.service';

@Component({
    selector: 'ds-login',
    templateUrl: 'login.component.html'
})

export class LoginComponent implements OnInit {

    signInForm: FormGroup;

    constructor(private apiService: ApiService,
        private fb: FormBuilder,
        private http: HttpClient,
        private authService: AuthService) {

    }

    ngOnInit() {
        this.initsignInForm();
    }

    initsignInForm() {
        this.signInForm = this.fb.group({
            username: this.fb.control('', [Validators.required, SimpleEmailValidator()]),
            password: this.fb.control('', Validators.required),
        })
    }

    testLoggedIn() {
        this.apiService.get("api/values")
            .pipe(take(1))
            .subscribe(result => {
                console.log(result);
            }, error => {
                console.log(error);
            })
    }

    signIn() {
        const signInParams: { username: string, password: string } = this.signInForm.getRawValue();
        this.setSession(signInParams.username);
        this.authService.login(signInParams.username, signInParams.password);
    }

    private setSession(email) {
      sessionStorage.setItem('userEmail', email);
    }
}
