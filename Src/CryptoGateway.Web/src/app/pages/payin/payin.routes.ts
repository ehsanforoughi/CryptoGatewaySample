import { Route } from '@angular/router';
import { Page404Component } from 'app/authentication/page404/page404.component';
import { CryptoPayinComponent } from './crypto-payin/crypto-payin.component';

export const PAYIN_ROUTE: Route[] = [
  {
    path: 'crypto-payin',
    component: CryptoPayinComponent,
  },
  {
    path: 'payin',
    loadChildren: () =>
      import('./payin.routes').then(
        (m) => m.PAYIN_ROUTE
      ),
  },
  { path: '**', component: Page404Component },
];