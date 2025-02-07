import { Component, OnInit } from '@angular/core';
import { Router, ActivatedRoute, RouterLink } from '@angular/router';
import { UntypedFormBuilder, UntypedFormGroup, Validators, FormsModule, ReactiveFormsModule } from '@angular/forms';
import { AuthService, Role } from '@core';
import { UnsubscribeOnDestroyAdapter } from '@shared';
import { MatIconModule } from '@angular/material/icon';
import { MatInputModule } from '@angular/material/input';
import { MatFormFieldModule } from '@angular/material/form-field';
import { MatButtonModule } from '@angular/material/button';
import { GoogleSigninButtonModule, SocialUser } from '@abacritt/angularx-social-login';
import { SocialAuthService } from "@abacritt/angularx-social-login";
import { mergeMap } from 'rxjs';
@Component({
  selector: 'app-signin',
  templateUrl: './signin.component.html',
  styleUrls: ['./signin.component.scss'],
  standalone: true,
  imports: [
    RouterLink,
    MatButtonModule,
    FormsModule,
    ReactiveFormsModule,
    MatFormFieldModule,
    MatInputModule,
    MatIconModule,
    GoogleSigninButtonModule
  ],
})
export class SigninComponent
  extends UnsubscribeOnDestroyAdapter
  implements OnInit {
  authForm!: UntypedFormGroup;
  submitted = false;
  loading = false;
  error = '';
  hide = true;
  user: SocialUser = new SocialUser;
  //loggedIn: boolean = false;
  constructor(
    private formBuilder: UntypedFormBuilder,
    private route: ActivatedRoute,
    private router: Router,
    private authService: AuthService,
    private socialAuthService: SocialAuthService
  ) {
    super();
  }

  ngOnInit() {
    this.authForm = this.formBuilder.group({
      username: ['', Validators.required],
      password: ['', Validators.required],
    });

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
    this.loading = true;
    this.error = '';
    if (this.authForm.invalid) {
      this.error = 'Username and Password not valid !';
      return;
    } else {
      this.subs.sink = this.authService
        .login(this.f['username'].value, this.f['password'].value)
        .subscribe({
          next: (res) => {
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
  }

  signOut = () => {
    this.socialAuthService.signOut();
  }
}
