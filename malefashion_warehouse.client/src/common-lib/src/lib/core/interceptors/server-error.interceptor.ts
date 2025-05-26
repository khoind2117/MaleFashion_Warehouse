import {
    HttpErrorResponse,
    HttpEvent,
    HttpHandler,
    HttpInterceptor,
    HttpRequest,
    HttpStatusCode
} from "@angular/common/http";
import {Injectable, Injector} from "@angular/core";
import {Router} from "@angular/router";
import {Observable, throwError} from "rxjs";
import {AuthService} from "@common-lib";
import {catchError} from "rxjs/operators";
import {HTTP_ERROR_RESPONSE} from "../consts";

@Injectable()
export class ServerErrorInterceptor implements HttpInterceptor {
    constructor(
        private _injector: Injector,
        private _router: Router,
    ) {}

    intercept(req: HttpRequest<any>, next: HttpHandler): Observable<HttpEvent<any>> {
        const authService = this._injector.get(AuthService);

        return next.handle(req).pipe(
            catchError((err: HttpErrorResponse) => {
                switch (err.status) {
                    case HttpStatusCode.InternalServerError: {
                        switch (err.error.message) {
                            case HTTP_ERROR_RESPONSE.USER_DEACTIVATE: {
                                authService.logout.subscribe();
                                break;
                            }
                            case HTTP_ERROR_RESPONSE.TOKEN_ERROR: {
                                authService.setIsAuthenticated(false);
                                void this._router.navigate(['/auth/login']);
                                break;
                            }
                            default: {
                                break;
                            }
                        }
                        break;
                    }

                    case HttpStatusCode.Unauthorized: {
                        if (!window.location.href.includes('/auth/login')) {
                            authService.logout.subscribe();
                        }
                        authService.setIsAuthenticated(false);
                        break;
                    }

                    case HttpStatusCode.BadRequest: {
                        switch (err.error.message) {
                            case HTTP_ERROR_RESPONSE.TOKEN_ERROR: {
                                location.reload();
                                break;
                            }
                            default: {
                                break;
                            }
                        }
                        break;
                    }

                    default: {
                        break;
                    }
                }

                return throwError(() => err);
            })
        )
    }
}
