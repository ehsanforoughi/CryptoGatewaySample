import { Injectable } from '@angular/core';
import { Router, ActivatedRouteSnapshot, RouterStateSnapshot } from '@angular/router';

import { AuthService } from '../service/auth.service';
import { Role } from '@core/models/role';
import { SnackBarAlerts } from '@shared/common/snack-bar.alerts';

@Injectable({
  providedIn: 'root',
})
export class AuthGuard {
  constructor(private authService: AuthService,
    private router: Router,
    private snackAlerts: SnackBarAlerts) { }

  canActivate(route: ActivatedRouteSnapshot, state: RouterStateSnapshot) {
  
    if (this.authService.currentUserValue && Object.keys(this.authService.currentUserValue).length !== 0) {
      const userRole = this.authService.currentUserValue.role;
      if (!userRole && route.data['role'] && route.data['role'].indexOf(userRole) === -1 &&
          this.authService.currentUserValue.role !== Role.User) {
        this.router.navigate(['/authentication/signin']);
        this.snackAlerts.errorSnack("You don't have permission to access on this site");
        return false;
      }
      return true;
    }

    this.router.navigate(['/authentication/signin']);
    return false;
  }
}
