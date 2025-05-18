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
export class AuthGuard implements CanActivate {
  constructor(
    private _router: Router,
    private _authService: AuthService,
    private _accountService: AccountService,
  ) {}

  canActivate(route: ActivatedRouteSnapshot, state: RouterStateSnapshot): MaybeAsync<GuardResult> {
    return this._authService.getAuthenticationStatus()
      .pipe(
        throttleTime(1000),
        switchMap((response) => {
          if (!this._accountService.getUserIdentity()) {
            return throwError(() => new Error("Invalid User Identity"));
          }
          return of(response);
        }),
        map((response) => {
          if (response.getAuthenticationStatus) {
            return true;
          }

          return !!response;
        })
      )
  }
}
