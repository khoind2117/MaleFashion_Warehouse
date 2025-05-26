import { Router } from '@angular/router';
import { Injectable } from '@angular/core';
import {AuthService} from "@common-lib";

@Injectable({
    providedIn: 'root',
})
export class UnAuthGuard {
    constructor(private _router: Router, private _authService: AuthService) {}

    /**
     * Guard to prevent authenticated users from accessing certain routes (e.g., login or sign up).
     * Redirects logged-in users to the dashboard.
     */
    canActivate(): boolean {
        const isLogin = this._authService.getIsAuthenticated();
        if (isLogin) {
            void this._router.navigate(['/dashboard']);
        }
        return true;
    }
}
