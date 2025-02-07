import { Route } from '@angular/router';
import { MainComponent } from './main/main.component';
import { Page404Component } from 'app/authentication/page404/page404.component';
import { AuthGuard } from '@core/guard/auth.guard';
import { Role } from '@core';
export const DASHBOARD_ROUTE: Route[] = [
  // {
  //   path: '',
  //   redirectTo: 'dashboard',
  //   pathMatch: 'full',
  // },
  {
    path: '',
    component: MainComponent,
    canActivate: [AuthGuard],
    data: {
      role: Role.User,
    },
  },
  { path: '**', component: Page404Component },
];
