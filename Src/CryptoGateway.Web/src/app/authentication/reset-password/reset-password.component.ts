import { Component, OnInit } from '@angular/core';
import { FormsModule, ReactiveFormsModule, UntypedFormBuilder, UntypedFormGroup, Validators } from '@angular/forms';
import { MatButtonModule } from '@angular/material/button';
import { MatFormFieldModule } from '@angular/material/form-field';
import { MatIconModule } from '@angular/material/icon';
import { MatInputModule } from '@angular/material/input';
import { ActivatedRoute, Params, RouterLink } from '@angular/router';
import { AuthService } from '@core';
import { SnackBarAlerts } from '@shared/common/snack-bar.alerts';
import { mergeMap } from 'rxjs';

@Component({
  selector: 'app-reset-password',
  standalone: true,
  imports: [
    FormsModule,
    ReactiveFormsModule,
    MatFormFieldModule,
    MatInputModule,
    MatIconModule,
    MatButtonModule,
    RouterLink
  ],
  templateUrl: './reset-password.component.html',
  styleUrl: './reset-password.component.scss'
})
export class ResetPasswordComponent implements OnInit {
  authForm!: UntypedFormGroup;
  submitted = false;
  returnUrl!: string;
  hide = true;
  chide = true;
  email: string = '';
  token: string = '';

  constructor(
    private formBuilder: UntypedFormBuilder,
    private route: ActivatedRoute,
    private authService: AuthService,
    private aletrs: SnackBarAlerts
  ) { }
  
  ngOnInit(): void {
    this.authForm = this.formBuilder.group({
      password: ['', Validators.required],
      cpassword: ['', Validators.required],
    });
    // get return url from route parameters or default to '/'
    this.returnUrl = this.route.snapshot.queryParams['returnUrl'] || '/';

    this.route.queryParams.subscribe((value: Params) => {
      this.email = value['email'];
      this.token = value['token'];
    });
  }

  get f() {
    return this.authForm.controls;
  }

  onSubmit() {
    this.submitted = true;
    // stop here if form is invalid
    if (this.authForm.invalid) {
      this.aletrs.errorSnack('Please fill inputs');
      return;
    } else {
      if (this.f['password'].value !== this.f['cpassword'].value) {
        this.aletrs.errorSnack('The password and confirmation password do not match.');
        return;
      }
      this.authService
        .resetPassword(this.f['password'].value, this.f['cpassword'].value, this.email, this.token)
        .subscribe({
          next: (res) => {
            if (res) {
              this.aletrs.successSnack('The password was reset.');
            } else {
              this.aletrs.errorSnack('Error');
            }
          },
          error: (error) => {
            this.aletrs.errorHtmlSnack(error, true);
          },
        });
    }
  }
}
