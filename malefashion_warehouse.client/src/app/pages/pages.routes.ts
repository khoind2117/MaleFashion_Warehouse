import { Routes } from '@angular/router';
import { Documentation } from './documentation/documentation';
import { Crud } from './crud/crud';
import { Empty } from './empty/empty';

export const pageRoutes: Routes = [

    {
        path: '**',
        redirectTo: '/notfound',
    }
] as Routes;
