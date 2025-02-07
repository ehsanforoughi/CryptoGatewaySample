import { Route } from '@angular/router';
import { MainLayoutComponent } from './layout/app-layout/main-layout/main-layout.component';
import { AuthGuard } from '@core/guard/auth.guard';
import { AuthLayoutComponent } from './layout/app-layout/auth-layout/auth-layout.component';
import { Page404Component } from './authentication/page404/page404.component';
import { Role } from '@core';
import { PublicLayoutComponent } from './layout/app-layout/public-layout/public-layout.component';

export const APP_ROUTE: Route[] = [
  {
    path: '',
    component: MainLayoutComponent,
    canActivate: [AuthGuard],
    children: [
      //{ path: 'authentication', redirectTo: '/authentication/signin', pathMatch: 'full' },
      { path: '', redirectTo: '/dashboard', pathMatch: 'full' },

      // {
      //   path: 'admin',
      //   canActivate: [AuthGuard],
      //   data: {
      //     role: Role.Admin,
      //   },
      //   loadChildren: () =>
      //     import('./admin/admin.routes').then((m) => m.ADMIN_ROUTE),
      // },
      {
        path: 'dashboard',
        canActivate: [AuthGuard],
        data: {
          role: Role.User,
        },
        loadChildren: () =>
          import('./pages/dashboard/dashboard.routes').then((m) => m.DASHBOARD_ROUTE),
      }, 
      // {
      //   path: 'custodial-account',
      //   canActivate: [AuthGuard],
      //   data: {
      //     role: Role.User,
      //   },
      //   loadChildren: () =>
      //     import('./pages/custody-account/custodial-account.routes').then((m) => m.CUSTODY_ACCOUNT_ROUTE),
      // }, 
      {
        path: 'payment',
        canActivate: [AuthGuard],
        data: {
          role: Role.User,
        },
        loadChildren: () =>
          import('./pages/payment/payment.routes').then((m) => m.PAYMENT_ROUTE),
      },  
      {
        path: 'payin',
        canActivate: [AuthGuard],
        data: {
          role: Role.User,
        },
        loadChildren: () =>
          import('./pages/payin/payin.routes').then((m) => m.PAYIN_ROUTE),
      },
      {
        path: 'payout',
        canActivate: [AuthGuard],
        data: {
          role: Role.User,
        },
        loadChildren: () =>
          import('./pages/payout/payout.routes').then((m) => m.PAYOUT_ROUTE),
      },
      {
        path: 'settings',
        canActivate: [AuthGuard],
        data: {
          role: Role.User,
        },
        loadChildren: () =>
          import('./pages/settings/settings.routes').then((m) => m.SETTINGS_ROUTE),
      },
    ],
  },
  {
    path: 'link',
    component: PublicLayoutComponent,
    loadChildren: () =>
      import('./public/public.routes').then((m) => m.PUBLIC_ROUTE),
  },
  // {
  //   path: 'payment/:id',
  //   component: PublicLayoutComponent,
  //   loadChildren: () =>
  //     import('./public/public.routes').then((m) => m.PUBLIC_ROUTE),
  // },
  {
    path: 'authentication',
    component: AuthLayoutComponent,
    loadChildren: () =>
      import('./authentication/auth.routes').then((m) => m.AUTH_ROUTE),
  },
  { path: '**', component: Page404Component },
];
