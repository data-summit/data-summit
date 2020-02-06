import { of as observableOf, from as observableFrom, BehaviorSubject, Subject, Observable, ReplaySubject, AsyncSubject } from 'rxjs';

import { map, take } from 'rxjs/operators';
import { Injectable, EventEmitter, Output } from "@angular/core";
import { Router } from "@angular/router";
import { HttpParams, HttpHeaders, HttpClient } from '@angular/common/http';

import * as moment from "moment"
import { environment } from 'src/environments/environment';

@Injectable()
export class AuthService {

    oauthSettings = {
        grant_type: 'client_credentials',
        scope: 'values',
        client_id: 'postmanUserPwd',
        client_secret: 'DataSummitUISecret'
    }

    httpOptions = {
        headers: new HttpHeaders({
            'Content-Type': 'application/x-www-form-urlencoded'
        })
    };

    httpPostJsonOptions = {
      headers: new HttpHeaders({
          'Content-Type': 'application/json'
      })
  };

    constructor(private router: Router,
        private http: HttpClient) { }

    login(username: string, password: string) {
        const params = new HttpParams({
            fromObject: {
                grant_type: this.oauthSettings.grant_type,
                scope: this.oauthSettings.scope,
                client_id: this.oauthSettings.client_id,
                client_secret: this.oauthSettings.client_secret,
                username: username,
                password: password,
            }
        });

        // this.navigateToTwoFactorRoute(); return; // TODO - remove after debugging

        this.http.post(environment.authority + 'connect/token', params, this.httpOptions).pipe(take(1))
            .subscribe((result: any) => {
                console.log(result);
                this.setSessionToken(result);
                this.navigateToTwoFactorRoute();
            }, error => {
                console.log('Problem logging in');
                console.log(error);
            });
    }

    loginFromTwoFactor(username: string, password: string) {
        this.navigateToReturnRoute();
    }

    private setSessionToken(authResult) {
        const expiresAt = moment().add(authResult.expires_in, 'second');

        sessionStorage.setItem('access_token', authResult.access_token);
        sessionStorage.setItem('expires_at', JSON.stringify(expiresAt.valueOf()) );
    }

    logout() {
        sessionStorage.removeItem('access_token');
        sessionStorage.removeItem('expires_at');
        this.router.navigate(['/']);
    }

    public isLoggedIn() {
        return moment().isBefore(this.getExpiration());
    }

    isLoggedOut() {
        return !this.isLoggedIn();
    }

    getExpiration() {
        const expiration = sessionStorage.getItem('expires_at');
        const expiresAt = JSON.parse(expiration);
        return moment(expiresAt);
    }

    storeRoute(urlSegment) {
        localStorage.setItem('returnRoute', urlSegment);
    }

    navigateToReturnRoute() {
        const returnRoute = localStorage.getItem('returnRoute');
        const url = returnRoute ? returnRoute : '/';

        this.router.navigate([url]);
    }

    navigateToTwoFactorRoute() {
      this.router.navigate(['/account/two-factor']);
    }

  getTwoFactorCodes(username: string, secret: string, setQrCode) {
      const params = JSON.stringify({ Username: username, Secret: secret });

    let codes: string;
    this.http.post(environment.authority + 'api/twofactorauthentication/getcodes', params, this.httpPostJsonOptions).pipe(take(1))
        .subscribe((result: any) => {
          console.log(result);
          codes = result;

          setQrCode(codes);
          return codes;
        }, error => {
          console.log('Problem getting 2FA codes');
          console.log(error);
        });
    }

    validateUserCode(code: string, secret: string) {
      const params = JSON.stringify({ TwoFactorAuthCode: code, AccountSecretKey: secret });

      this.http.post(environment.authority + 'api/twofactorauthentication/validate', params, this.httpPostJsonOptions).pipe(take(1))
        .subscribe((result: any) => {
          console.log(result);

          if (result) {
            this.navigateToReturnRoute();
          }
        }, error => {
          console.log('Problem logging in');
          console.log(error);
        });
    }
}
