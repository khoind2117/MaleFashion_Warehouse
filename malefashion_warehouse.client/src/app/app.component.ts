import {Component, OnInit, ViewEncapsulation} from '@angular/core';
import { RouterOutlet } from '@angular/router';
import {Account, AccountService, AuthService, LoadingComponent} from "@common-lib";
import {ToastModule} from "primeng/toast";
import {ConfirmDialog} from "primeng/confirmdialog";

@Component({
    selector: 'app-root',
    templateUrl: './app.component.html',
    styleUrls: ['./app.component.scss'],
    imports: [
        RouterOutlet,
        LoadingComponent,
        ToastModule,
        ConfirmDialog,
    ],
})
export class AppComponent implements OnInit {
    constructor(
        private _authService: AuthService,
        private _accountService: AccountService,
    ) {}

    ngOnInit() {
        this._authService.checkAuthentication()
            .subscribe(() => {
                this._accountService.identity(true)
                    .subscribe((account: Account) => {})
            })

        this._accountService.getAuthenticationState();
    }
}
