import { Component } from '@angular/core';
import { RouterModule } from '@angular/router';
import { ButtonModule } from 'primeng/button';
import { RippleModule } from 'primeng/ripple';
import {AppFloatingConfigurator} from "../../../layout/component";

@Component({
    selector: 'app-error',
    standalone: true,
    templateUrl: './error.component.html',
    imports: [
        ButtonModule,
        RippleModule,
        RouterModule,
        AppFloatingConfigurator,
        ButtonModule
    ],
})
export class ErrorComponent {}
