import { Injectable } from '@angular/core';
import { HttpClient, HttpResponse } from '@angular/common/http';
import { BehaviorSubject, Observable, Subject, map, of, throwError } from 'rxjs';
import { User } from '../models/user';
import { Role } from '@core/models/role';
import { SocialAuthService, SocialUser } from "@abacritt/angularx-social-login";
import { GoogleLoginProvider } from "@abacritt/angularx-social-login";
import { environment } from 'environments/environment';

@Injectable({
  providedIn: 'root',
})
export class AuthService {
  private currentUserSubject: BehaviorSubject<User>;
  public currentUser: Observable<User>;

  private authChangeSub = new Subject<boolean>();
  private extAuthChangeSub = new Subject<SocialUser>();
  public authChanged = this.authChangeSub.asObservable();
  public extAuthChanged = this.extAuthChangeSub.asObservable();
  
  constructor(private http: HttpClient
  ) {
    this.currentUserSubject = new BehaviorSubject<User>(
      JSON.parse(localStorage.getItem('currentUser') || '{}')
    );
    this.currentUser = this.currentUserSubject.asObservable();
  }

  public get currentUserValue(): User {
    return this.currentUserSubject.value;
  }

  register(email: string, password: string, confirmPassword: string) {
    return this.http.post<any>(`${environment.apiUrl}/auth/registration`,
      { email: email, password: password, confirmPassword: confirmPassword, clientURI: environment.forgotPasswordClientUrl })
      .pipe(map((response) => {
        if (response.isSuccess === true) {
          return of(new HttpResponse({ status: 200, body: { response: response } }));
        } else {
          return this.error(response.message);
        }
      }));
  }

  login(email: string, password: string) {
    //const user = this.users.find((u) => u.username === username && u.password === password);
    return this.http.post<any>(`${environment.apiUrl}/auth/login`,
      { email: email, password: password })//, clientURI: environment.resetPasswordClientUrl })
      .pipe(map((response) => {
        if (response.isSuccess === true) { 
          let user = response.result.currentUser;
          if (!user) {
            return this.error('Username or password is incorrect');
          } else {
            // user.img = 'assets/images/user/admin.jpg';
            localStorage.setItem('currentUser', JSON.stringify(user));
            this.currentUserSubject.next(user);
            return this.ok(user);
          }
        } else {
          return this.error(response.message);
        }
    }));
  }

  // ok(body?: {
  //   id: number;
  //   img: string;
  //   userName: string;
  //   firstName: string;
  //   lastName: string;
  //   fullName: string;
  //   role: Role,
  //   token: string;
  // }) {
  //   return of(new HttpResponse({ status: 200, body }));
  // }

  ok(body?: User) {
    return of(new HttpResponse({ status: 200, body }));
  }

  error(message: string) {
    throw new Error(message);
  }

  confirmEmail(email: string, token: string) {
    return this.http.post<any>(`${environment.apiUrl}/auth/email-confirmation`,
      { email: email, token: token })
      .pipe(map((response) => {
        if (response.isSuccess === true) {
          return of(new HttpResponse({ status: 200, body: { response: response } }));
        } else {
          return this.error(response.message);
        }
      }));
  }

  forgotPassword(email: string) {
    return this.http.post<any>(`${environment.apiUrl}/auth/forgot-password`,
      { email: email, clientURI: environment.resetPasswordClientUrl })
      .pipe(map((response) => {
        if (response.isSuccess === true) {
          return of(new HttpResponse({ status: 200, body: { response: response } }));
        } else {
          return this.error(response.message);
        }
      }));
  }

  resetPassword(password: string, confirmPassword: string, email: string, token: string) {
    return this.http.post<any>(`${environment.apiUrl}/auth/reset-password`,
      { password: password, confirmPassword: confirmPassword, email: email, token: token  })
      .pipe(map((response) => {
        if (response.isSuccess === true) {
          return of(new HttpResponse({ status: 200, body: { response: response } }));
        } else {
          return this.error(response.message);
        }
      }));
  }

  // signInWithGoogle = ()=> {
  //   this.socialAuthService.signIn(GoogleLoginProvider.PROVIDER_ID);
  // }

  signInWithGoogle(idToken: string, provider: string) {
    return this.http.post<any>(`${environment.apiUrl}/auth/external-login`,
      { idToken: idToken, provider: provider })
      .pipe(map((response) => {
        if (response.isSuccess === true) {
          let user = response.result.currentUser;
          if (!user) {
            return this.error('Username or password is incorrect');
          } else {
            //user.img = 'assets/images/user/admin.jpg';
            localStorage.setItem('currentUser', JSON.stringify(user));
            this.currentUserSubject.next(user);
            return this.ok(user);
            // return this.ok({
            //   id: user.userId,
            //   userName: user.userName,
            //   firstName: user.firstName,
            //   lastName: user.lastName,
            //   fullName: user.fullName,
            //   role: user.role == 'User' ? Role.User : Role.None,
            //   img: user.image === '' ? 'assets/images/user/admin.jpg' : user.image,
            //   token: user.token,
            // });
          }
        } else {
          return this.error(response.message);
        }
      }));
  }
  
  logout() {
    // remove user from local storage to log user out
    localStorage.removeItem('config_currentUser');
    localStorage.removeItem('currentUser');
    this.currentUserSubject.next(this.currentUserValue);
    return of({ success: false });
  }
}
