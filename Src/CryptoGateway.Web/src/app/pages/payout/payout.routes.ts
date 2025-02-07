import { Route } from '@angular/router';
import { Page404Component } from 'app/authentication/page404/page404.component';
import { FiatPayoutComponent } from './fiat-payout/fiat-payout.component';
import { CryptoPayoutComponent } from './crypto-payout/crypto-payout.component';

export const PAYOUT_ROUTE: Route[] = [
  {
    path: 'fiat-payout',
    component: FiatPayoutComponent,
  },
  {
    path: 'crypto-payout',
    component: CryptoPayoutComponent,
  },
  {
    path: 'payout',
    loadChildren: () =>
      import('./payout.routes').then(
        (m) => m.PAYOUT_ROUTE
      ),
  },
  { path: '**', component: Page404Component },
];