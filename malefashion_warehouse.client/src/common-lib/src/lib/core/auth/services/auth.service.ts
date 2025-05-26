import { Injectable } from '@angular/core';
import {Observable, shareReplay, tap, throttleTime} from 'rxjs';
import { Router } from '@angular/router';
import {ApiService} from "../../../shared";
import {AccountService} from "./account.service";
import {Credentials} from "../../../../../../app/pages/auth/login/login.component";
import {environment} from "@env";

@Injectable({
  providedIn: 'root'
})

export class AuthService {
    isAuthenticated = false;
    authenticationStatus$: Observable<any>;

    constructor(
        private _router: Router,
        private _apiService: ApiService,
        private _accountService: AccountService,
    ) {
        this.authenticationStatus$ = this._apiService.get(`${environment.apiNode}/status`)
          .pipe(
            tap(() => (this.isAuthenticated = true)),
            throttleTime(1000),
            shareReplay(1),
          );
    }

    validate(credentials: Credentials) {
        return this._apiService.insert(`${environment.apiNode}/authenticate`, credentials);
    }

    checkAuthentication() {
        return this._apiService.get(`${environment.apiNode}/secure`)
            .pipe(
                tap(() => (this.isAuthenticated = true))
            );
    }

    getAuthenticationStatus(): Observable<any> {
        return this.authenticationStatus$;
    }

    getIsAuthenticated() {
        return this.isAuthenticated;
    }

    setIsAuthenticated(value: boolean) {
        this.isAuthenticated = value;
    }

    get logout(): Observable<void> {
        return new Observable((observer) => {
          this._accountService.authenticate(null);
            this._apiService.insert(`${environment.apiNode}/logout`, {}).subscribe({
                next: () => {
                    this.isAuthenticated = false;
                    void this._router.navigate(['/auth/login']);
                    observer.next();
                    observer.complete();
                },
                error: (err) => observer.error(err),
            });
        })
    }
}
