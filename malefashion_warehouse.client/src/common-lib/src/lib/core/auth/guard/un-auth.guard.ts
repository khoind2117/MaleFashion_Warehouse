import { Router } from '@angular/router';
import { Injectable } from '@angular/core';
import {AuthService} from "@common-lib";

@Injectable({
    providedIn: 'root',
})
export class UnAuthGuard {
    constructor(private _router: Router, private _authService: AuthService) {}

    canActivate(): boolean {
        const isLogin = this._authService.getIsAuthenticated();
        if (isLogin) {
            this._router.navigate(['/dashboard']);
        }
        return true;
    }
}
