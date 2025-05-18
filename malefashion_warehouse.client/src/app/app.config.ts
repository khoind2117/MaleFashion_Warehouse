import {provideHttpClient, withInterceptors, withInterceptorsFromDi} from '@angular/common/http';
import {ApplicationConfig, importProvidersFrom} from '@angular/core';
import { provideAnimationsAsync } from '@angular/platform-browser/animations/async';
import { provideRouter, RouterModule, withEnabledBlockingInitialNavigation, withInMemoryScrolling } from '@angular/router';
import Aura from '@primeng/themes/aura';
import { providePrimeNG } from 'primeng/config';
import { appRoutes } from './app.routes';
import {FormsModule, ReactiveFormsModule } from '@angular/forms';
import { CommonModule } from '@angular/common';
import {ErrorInterceptor, JwtInterceptor } from 'src/common-lib/src/lib/core/auth/interceptors';

export const appConfig: ApplicationConfig = {
  providers: [
    provideRouter(appRoutes,
      withInMemoryScrolling({
        anchorScrolling: 'enabled',
        scrollPositionRestoration: 'enabled'
      }),
      withEnabledBlockingInitialNavigation()
    ),
    provideHttpClient(withInterceptorsFromDi()),
    provideAnimationsAsync(),
    providePrimeNG({
      theme: {
        preset: Aura,
        options: {
          darkModeSelector: '.app-dark'
        }
      }
    }),

    importProvidersFrom(
      RouterModule.forRoot(appRoutes),
      CommonModule,
      FormsModule,
      ReactiveFormsModule,
    ),

    // Interceptor
    // provideHttpClient(
    //   withInterceptors([JwtInterceptor, ErrorInterceptor])
    // ),
  ]
};
