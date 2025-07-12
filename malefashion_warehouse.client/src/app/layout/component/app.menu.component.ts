import {Component, OnInit} from '@angular/core';
import { CommonModule } from '@angular/common';
import { RouterModule } from '@angular/router';
import { MenuItem } from 'primeng/api';
import { AppMenuitem } from './app.menuitem';
import {Button} from "primeng/button";
import {Account, AccountService, AuthService} from "@common-lib";
import {Observable} from "rxjs";

@Component({
    selector: 'app-menu',
    standalone: true,
    imports: [CommonModule, AppMenuitem, RouterModule, Button],
    templateUrl: './app.menu.component.html',
})
export class AppMenuComponent implements OnInit {
    model: MenuItem[] = [];

    sideNavConfig$: Observable<MenuItem[]>;

    accountObj: Account;

    constructor(
        private _authService: AuthService,
        private _accountService: AccountService,
    ) { }

    ngOnInit() {
        this.model = [
            {
                label: 'Home',
                items: [{ label: 'Dashboard', icon: 'pi pi-home', routerLink: ['/dashboard'] }]
            },
            {
                label: 'Products',
                items: [{ label: 'Products', icon: 'pi pi-database', routerLink: ['/'] }]
            }
        ];

        this._accountService.getAuthenticationState().subscribe((account: Account) => {
            this.accountObj = account;
        })
    }

    openLogoutConfirmDialog() {
        this._confirmDialogService.show({
            message: 'Are you sure you want to logout?',
            onAccept: () => {
                this._authService.logout.subscribe();
                this._authService.setIsAuthenticated(false);
            }
        })
    }
}
