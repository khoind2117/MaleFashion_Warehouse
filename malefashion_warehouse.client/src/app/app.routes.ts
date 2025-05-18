import { Routes } from '@angular/router';
import {DashboardComponent} from "./pages/dashboard/dashboard.component";
import {AuthGuard, UnAuthGuard} from "@common-lib";
import { AppLayoutComponent } from './layout/component/app.layout.component';


export const appRoutes: Routes = [
    {
        path: '',
        component: AppLayoutComponent,
        children: [
            {
                path: 'dashboard',
                component: DashboardComponent,
                // canActivate: [AuthGuard],
            },
        ]
    },
    {
        path: 'auth',
        loadChildren: () =>
            import('./pages/auth/auth.routes').then((m) => m.authRoutes),
        canActivate: [UnAuthGuard],
    },
];
