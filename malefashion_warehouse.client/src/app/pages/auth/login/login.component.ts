import {Component, OnInit} from '@angular/core';
import {FormControl, FormGroup, FormsModule, ReactiveFormsModule, Validators} from '@angular/forms';
import {Router, RouterModule} from '@angular/router';
import {ButtonModule} from 'primeng/button';
import {CheckboxModule} from 'primeng/checkbox';
import {InputTextModule} from 'primeng/inputtext';
import {PasswordModule} from 'primeng/password';
import {RippleModule} from 'primeng/ripple';
import {
    AccountService,
    AuthService,
    DestroyService,
    NotificationService, Severity,
} from "@common-lib";
import {FloatLabel} from "primeng/floatlabel";

export interface Credentials {
    userName: string;
    password: string;
}

@Component({
    selector: 'app-login',
    standalone: true,
    templateUrl: './login.component.html',
    styleUrl: './login.component.scss',
    imports: [
        ButtonModule,
        CheckboxModule,
        InputTextModule,
        PasswordModule,
        FormsModule,
        RouterModule,
        RippleModule,
        ReactiveFormsModule,
        FloatLabel,
    ],
    providers: [
        AuthService,
        DestroyService,
    ]
})
export class LoginComponent implements OnInit {
    loginForm: FormGroup;

    constructor(
        private _router: Router,
        private _notificationService: NotificationService,
        private _authService: AuthService,
        private _accountService: AccountService,
    ) {}

    ngOnInit() {
        this.loginForm = new FormGroup({
            userName: new FormControl('', Validators.required),
            password: new FormControl('', Validators.required),
        })
    }

    onSubmit() {
        if (this.loginForm?.valid) {
            const credentials: Credentials = this.loginForm.getRawValue();
            this._authService.validate(credentials).subscribe({
                next: (res) => {
                    this._accountService.identity(true).subscribe({
                        next: (user) => {
                            if (user) {
                                this._notificationService.show(
                                    Severity.SUCCESS,
                                    'Login successful',
                                    `Welcome back, ${user.firstName}!`,
                                );
                                this._router.navigate(['/dashboard']);
                            } else {
                                this._notificationService.show(
                                    Severity.DANGER,
                                    'Authentication error',
                                    'Unable to fetch user information.',
                                );
                            }
                        },
                        error: (err) => {
                            this._notificationService.show(
                                Severity.DANGER,
                                'System error',
                                'Failed to retrieve user details.',
                                { life: 5000 }
                            );
                            console.error(err);
                        }
                    });
                },
                error: (err) => {
                    this._notificationService.show(
                        Severity.DANGER,
                        'Login failed',
                        'Invalid username or password.',
                        { life: 5000 }
                    );
                    console.error(err);
                }
            });
        } else {
            this.loginForm?.markAllAsTouched();
        }
    }
}
