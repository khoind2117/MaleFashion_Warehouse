import { inject } from "@angular/core";
import { HttpRequest, HttpEvent, HttpInterceptorFn, HttpHandlerFn } from "@angular/common/http";
import { Observable } from "rxjs";
import { AuthService } from "@common-lib";

export const JwtInterceptor
: HttpInterceptorFn = (request: HttpRequest<any>, next: HttpHandlerFn)
: Observable<HttpEvent<any>> => {
    const authService = inject(AuthService);

    let token = authService.authenticationStatus$;
    let headers: any = {};

    if (token != null) {
        headers["Authorization"] = `Bearer ${token}`;
    }

    request = request.clone({
        setHeaders: headers,
    });

    return next(request);
};
