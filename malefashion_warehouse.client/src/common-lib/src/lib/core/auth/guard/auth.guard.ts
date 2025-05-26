import {
  ActivatedRouteSnapshot,
  CanActivate,
  GuardResult,
  MaybeAsync,
  Router,
  RouterStateSnapshot
} from '@angular/router';
import {AccountService, AuthService} from "../services";
import {map, of, retry, switchMap, throttleTime, throwError} from "rxjs";
import {Injectable} from "@angular/core";

@Injectable({
    providedIn: 'root',
})
export class AuthGuard {
    constructor(
        private _authService: AuthService,
        private _accountService: AccountService,
    ) {}

    /**
     * Guard to check if the user is authenticated before activating the route.
     *
     * Note: Retry logic is commented out here because user identity is preloaded at app startup via AppInitializer.
     * Without retry or AppInitializer, user identity might not be loaded when the guard runs,
     * causing a null value and blocking access incorrectly.
     */
    canActivate(route: ActivatedRouteSnapshot, state: RouterStateSnapshot): MaybeAsync<GuardResult> {
        return this._authService.getAuthenticationStatus()
            .pipe(
                throttleTime(1000),
                switchMap((res) => {
                    const userIdentity = this._accountService.getUserIdentity();
                    if (!userIdentity) {
                        return throwError(() => new Error("Invalid User Identity"));
                    }

                    return of(res);
                }),
                // retry({ count: 3, delay: 1000 }),
                map((res: {statusCode: number, isAuthenticated: boolean}) => {
                    if (res?.isAuthenticated) {
                        return true;
                    }

                    return !!res;
                })
            )
    }
}
