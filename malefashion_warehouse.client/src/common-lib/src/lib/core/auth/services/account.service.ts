import {Injectable} from "@angular/core";
import {ApiService} from "../../../shared";
import {Router} from "@angular/router";
import {map, Observable, of, ReplaySubject, tap} from "rxjs";
import {Account, SafeAny} from "../../models";
import {catchError, shareReplay} from "rxjs/operators";
import {environment} from "@env";

@Injectable({ providedIn: 'root' })
export class AccountService {
    private _userIdentity: Account | null = null;
    private _authenticationState = new ReplaySubject<Account | null>(1);
    private accountCache$?: Observable<Account | null>;

    constructor(
        private _router: Router,
        private _apiService: ApiService,
    ) {}

    authenticate(identity: Account | null): void {
        this._userIdentity = identity;
        this._authenticationState.next(this._userIdentity);
    }

    identity(force?: boolean): Observable<Account | null> {
        if (!this.accountCache$ || force || !this.isAuthenticated()) {
            this.accountCache$ = this.fetch().pipe(
                catchError(() => {
                    this._router.navigate(['/invalid']);
                    return of(null);
                }),
                tap((account: Account | null) => {
                    this.authenticate(account);

                    if (account) {
                        this.navigateToHomepage(account);
                    }
                }),
                shareReplay(),
            );
        }
        return this.accountCache$;
    }

    getUserIdentity(): Account {
        return this._userIdentity;
    }

    isAuthenticated(): boolean {
        return this._userIdentity !== null;
    }

    getAuthenticationState(): Observable<Account | null> {
        return this._authenticationState.asObservable();
    }

    private fetch(): Observable<Account> {
        return this._apiService.get<Account>('/api/account/info').pipe(
            map((res) => {
                return res.data;
            }),
        ) as SafeAny;
    }

    private navigateToHomepage(account: Account): void {
        function isValidHttpUrl(string: string) {
            let url;
            try {
                url = new URL(string);
            } catch (_) {
                return false;
            }
            return url.protocol === 'http:' || url.protocol === 'https:';
        }
    }
}
