import { Injectable } from '@angular/core';
import { CanActivate, Router, ActivatedRouteSnapshot, RouterStateSnapshot } from '@angular/router';
import { AuthService } from './auth.service';
import { environment } from "../../../environments/environment";

@Injectable()
export class AuthGuardService implements CanActivate {

    constructor(private authService: AuthService,
        private router: Router) {

    }

    canActivate(route: ActivatedRouteSnapshot, state: RouterStateSnapshot): boolean 
    {
        if (!environment.bypassSecurity)
        {
            if (this.authService.isLoggedIn()) {
                return true
            }
            this.authService.storeRoute(state.url)
            this.router.navigate(['/account/login'])
        }
        else
        { 
            // if (this.router.url == "http://localhost:56156")
            // { 
            //     this.authService.login("turkeyboy100@hotmail.com", "2bollocks");
            //     var r = this.authService.isLoggedIn();
            // }
            return true }
        return false
    }
}