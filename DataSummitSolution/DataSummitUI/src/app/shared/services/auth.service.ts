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

        this.http.post(environment.authority + "connect/token", params, this.httpOptions).pipe(take(1))
            .subscribe((result: any) => {
                console.log(result)
                this.setSession(result)
                this.navigateToReturnRoute()
            }, error => {
                console.log("Problem logging in")
                console.log(error)
            })
    }

    private setSession(authResult) {
        const expiresAt = moment().add(authResult.expires_in,'second');

        sessionStorage.setItem('access_token', authResult.access_token);
        sessionStorage.setItem("expires_at", JSON.stringify(expiresAt.valueOf()) );
    }   

    logout() {
        sessionStorage.removeItem("access_token")
        sessionStorage.removeItem("expires_at")
        this.router.navigate(['/'])
    }

    public isLoggedIn() {
        return moment().isBefore(this.getExpiration());
    }

    isLoggedOut() {
        return !this.isLoggedIn();
    }

    getExpiration() {
        const expiration = sessionStorage.getItem("expires_at");
        const expiresAt = JSON.parse(expiration);
        return moment(expiresAt);
    }

    storeRoute(urlSegment) {
        localStorage.setItem('returnRoute', urlSegment)
    }

    navigateToReturnRoute() {
        let url = localStorage.getItem('returnRoute') 
        if(url) {
            this.router.navigate([url])
        } else {
            this.router.navigate(['/'])
        }
    }
}
