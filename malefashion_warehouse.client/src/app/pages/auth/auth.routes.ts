import { Routes } from '@angular/router';
import {LoginComponent} from "./login/login.component";
import {AuthComponent} from "./auth.component";
import {ROUTING_MAPPING} from "../../config/mapping-routing.config";


export const authRoutes: Routes = [
    {
        path: '',
        component: AuthComponent,
        children: [
            {
                path: ROUTING_MAPPING.auth.login.absolutePath,
                component: LoginComponent,
            },
        ],
    },
];
