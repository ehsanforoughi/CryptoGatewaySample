import { Component, OnInit } from '@angular/core';
import { Router, ActivatedRoute, RouterLink } from '@angular/router';
import { UntypedFormBuilder, UntypedFormGroup, Validators, FormsModule, ReactiveFormsModule } from '@angular/forms';
import { MatButtonModule } from '@angular/material/button';
import { MatIconModule } from '@angular/material/icon';
import { MatInputModule } from '@angular/material/input';
import { MatFormFieldModule } from '@angular/material/form-field';
import { SnackBarAlerts } from '@shared/common/snack-bar.alerts';
import { AuthService } from '@core';
@Component({
  selector: 'app-forgot-password',
  templateUrl: './forgot-password.component.html',
  styleUrls: ['./forgot-password.component.scss'],
  standalone: true,
  imports: [
    FormsModule,
    ReactiveFormsModule,
    MatFormFieldModule,
    MatInputModule,
    MatIconModule,
    MatButtonModule,
    RouterLink,
  ],
})
export class ForgotPasswordComponent implements OnInit {
  authForm!: UntypedFormGroup;
  submitted = false;
  returnUrl!: string;
  constructor(
    private formBuilder: UntypedFormBuilder,
    private route: ActivatedRoute,
    //private router: Router,
    private authService: AuthService,
    private aletrs: SnackBarAlerts
  ) { }

  ngOnInit() {
    this.authForm = this.formBuilder.group({
      email: [
        '',
        [Validators.required, Validators.email, Validators.minLength(5)],
      ],
    });
    // get return url from route parameters or default to '/'
    this.returnUrl = this.route.snapshot.queryParams['returnUrl'] || '/';
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
      this.authService
        .forgotPassword(this.f['email'].value)
        .subscribe({
          next: (res) => {
            if (res) {
              this.aletrs.successSnack('A confirmation email has been sent to you.');
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
