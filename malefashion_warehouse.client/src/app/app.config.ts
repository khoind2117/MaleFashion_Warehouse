import {HTTP_INTERCEPTORS, provideHttpClient, withInterceptorsFromDi} from '@angular/common/http';
import {ApplicationConfig, inject, provideAppInitializer} from '@angular/core';
import { provideAnimationsAsync } from '@angular/platform-browser/animations/async';
import { provideRouter } from '@angular/router';
import Aura from '@primeng/themes/aura';
import { providePrimeNG } from 'primeng/config';
import { appRoutes } from './app.routes';
import {AccountService, ServerErrorInterceptor} from "@common-lib";
import {firstValueFrom, take} from "rxjs";
import {MessageService} from "primeng/api";

export const appConfig: ApplicationConfig = {
    providers: [
        // Router
        provideRouter(appRoutes),

        // HttpClient + Interceptors from DI
        provideHttpClient(
          withInterceptorsFromDi()
        ),
        { provide: HTTP_INTERCEPTORS, useClass: ServerErrorInterceptor, multi: true },

        // Animations (async)
        provideAnimationsAsync(),

        // PrimeNG theme
        providePrimeNG({
            theme: {
                preset: Aura,
                options: {
                    darkModeSelector: '.app-dark'
                }
            }
        }),

        // PrimeNG Message Service for Notification Service
        MessageService,

        /**
         * Preloads and caches the current user identity at app startup using AppInitializer.
         * This avoids delays or retries in route guards by ensuring user identity is ready early.
         */
        provideAppInitializer(() => {
            const accountService = inject(AccountService);
            return firstValueFrom(accountService.identity(true).pipe(take(1)));
        }),
    ]
};
