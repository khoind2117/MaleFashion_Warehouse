import { Component, ElementRef } from '@angular/core';
import { AppMenuComponent } from './app.menu.component';
import {Button} from "primeng/button";

@Component({
    selector: 'app-sidebar',
    standalone: true,
    imports: [AppMenuComponent, Button],
    templateUrl: './app.sidebar.component.html',
})
export class AppSidebarComponent {
    constructor(public el: ElementRef) {}
}
