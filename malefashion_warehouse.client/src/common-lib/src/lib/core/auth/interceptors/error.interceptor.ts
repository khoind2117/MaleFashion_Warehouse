import { inject, NgZone } from '@angular/core';
import { HttpRequest, HttpEvent, HttpInterceptorFn, HttpHandlerFn } from '@angular/common/http';
import { Router } from '@angular/router';
import { Observable, throwError } from 'rxjs';
import { catchError } from 'rxjs/operators';

export const ErrorInterceptor
: HttpInterceptorFn = (request: HttpRequest<any>, next: HttpHandlerFn)
: Observable<HttpEvent<any>> => {
  const ngZone = inject(NgZone);
  const router = inject(Router);

  return next(request).pipe(
    catchError((error) => {
      if (error.status === 401) {

      } else if (error.status === 406) {
        navigate('/logout');
      } else if (error.status === 302) {
        navigate('');
      }

      return throwError(() => error);
    })
  );

  function navigate(path: string): void {
    ngZone.run(() => router.navigateByUrl(path)).then();
  }
}
