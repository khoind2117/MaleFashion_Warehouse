import { Component } from '@angular/core';
import { RouterModule } from '@angular/router';
import { ButtonModule } from 'primeng/button';
import { AppFloatingConfigurator } from '../../layout/component';

@Component({
    selector: 'app-notfound',
    standalone: true,
    templateUrl: './notfound.component.html',
    imports: [
      RouterModule,
      AppFloatingConfigurator,
      ButtonModule
    ],
})
export class NotfoundComponent {}
