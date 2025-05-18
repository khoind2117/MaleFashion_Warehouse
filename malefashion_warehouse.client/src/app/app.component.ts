import {Component, OnInit} from '@angular/core';
import { RouterOutlet } from '@angular/router';
import {Account, AccountService, AuthService} from "@common-lib";

@Component({
  selector: 'app-root',
  templateUrl: './app.component.html',
  imports: [RouterOutlet]
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
