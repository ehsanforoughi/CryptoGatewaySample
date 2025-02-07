import { Component, OnInit } from '@angular/core';
import { Router, ActivatedRoute, RouterLink } from '@angular/router';
import { UntypedFormBuilder, UntypedFormGroup, Validators, FormsModule, ReactiveFormsModule } from '@angular/forms';
import { MatButtonModule } from '@angular/material/button';
import { MatIconModule } from '@angular/material/icon';
import { MatInputModule } from '@angular/material/input';
import { MatFormFieldModule } from '@angular/material/form-field';
import { AuthService } from '@core';
import { SnackBarAlerts } from '@shared/common/snack-bar.alerts';
import { GoogleSigninButtonModule, SocialUser } from '@abacritt/angularx-social-login';
import { SocialAuthService } from "@abacritt/angularx-social-login";
import { mergeMap } from 'rxjs';

@Component({
  selector: 'app-signup',
  templateUrl: './signup.component.html',
  styleUrls: ['./signup.component.scss'],
  standalone: true,
  imports: [
    FormsModule,
    ReactiveFormsModule,
    MatFormFieldModule,
    MatInputModule,
    MatIconModule,
    RouterLink,
    MatButtonModule,
    GoogleSigninButtonModule
  ],
})
export class SignupComponent implements OnInit {
  authForm!: UntypedFormGroup;
  submitted = false;
  returnUrl!: string;
  loading = false;
  error = '';
  hide = true;
  chide = true;
  user: SocialUser = new SocialUser;
  
  constructor(
    private formBuilder: UntypedFormBuilder,
    private route: ActivatedRoute,
    private router: Router,
    private authService: AuthService,
    private socialAuthService: SocialAuthService,
    private aletrs: SnackBarAlerts
  ) { }

  ngOnInit() {
    this.authForm = this.formBuilder.group({
      //username: ['', Validators.required],
      email: [
        '',
        [Validators.required, Validators.email, Validators.minLength(5)],
      ],
      password: ['', Validators.required],
      cpassword: ['', Validators.required],
    });
    // get return url from route parameters or default to '/'
    this.returnUrl = this.route.snapshot.queryParams['returnUrl'] || '/';

    this.socialAuthService.authState.pipe(mergeMap((user) => {
      this.user = user;
      return this.authService.signInWithGoogle(user.idToken, user.provider)
    }))
      .subscribe({
        next: (res) => {
          // this.user = user;
          // this.loggedIn = (user != null);
          if (res) {
            this.loading = false;
            //const role = this.authService.currentUserValue.role;
            this.router.navigate(['/dashboard']);
          } else {
            this.error = 'Invalid Login';
            console.error(this.error);
            this.submitted = false;
            this.loading = false;
            this.router.navigate(['/authentication/signin']);
          }
        },
        error: (error) => {
          console.error(error);
          this.error = error;
          this.submitted = false;
          this.loading = false;
          this.router.navigate(['/authentication/signin']);
        },
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
        .register(this.f['email'].value, this.f['password'].value, this.f['cpassword'].value)
        .subscribe({
          next: (res) => {
            if (res) {
              this.aletrs.successSnack('A confirmation email has been sent to you.');
              this.router.navigate(['/authentication/signin']);
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
