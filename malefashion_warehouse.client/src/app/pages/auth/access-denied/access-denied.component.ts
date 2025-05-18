import { Component } from '@angular/core';
import { RouterModule } from '@angular/router';
import { ButtonModule } from 'primeng/button';
import { RippleModule } from 'primeng/ripple';
import {AppFloatingConfigurator} from "../../../layout/component";

@Component({
    selector: 'app-access-denied',
    standalone: true,
    templateUrl: './access-denied.component.html',
    imports: [
        ButtonModule,
        RouterModule,
        RippleModule,
        AppFloatingConfigurator,
        ButtonModule
    ],
})
export class AccessDeniedComponent {}
