import { Routes } from '@angular/router';
import {AuthGuard, UnAuthGuard} from "@common-lib";
import {AppLayoutComponent} from "./layout/component";
import {ROUTING_MAPPING} from "./config/mapping-routing.config";
import {DashboardComponent} from "./pages/dashboard/dashboard.component";
import {RouteDataModel} from "../common-lib/src/lib/core/models/core/route-data.model";

export const appRoutes: Routes = [
    {
        path: '',
        redirectTo: ROUTING_MAPPING.dashboard.root.fullPath,
        pathMatch: 'full',
    },
    {
        path: '',
        component: AppLayoutComponent,
        children: [
            {
                path: ROUTING_MAPPING.dashboard.root.absolutePath,
                component: DashboardComponent,
                data: {
                    activeTab: false,
                    sidebarOverlay: false,
                    authorities: [],
                    isHomePage: true,
                } as RouteDataModel,
                canActivate: [AuthGuard],
            },
        ]
    },
    {
        path: ROUTING_MAPPING.auth.root.absolutePath,
        loadChildren: () =>
            import('./pages/auth/auth.routes').then((m) => m.authRoutes),
        canActivate: [UnAuthGuard],
    },
    {
        path: '**',
        pathMatch: 'full',
        redirectTo: 'error/404',
    }
];
