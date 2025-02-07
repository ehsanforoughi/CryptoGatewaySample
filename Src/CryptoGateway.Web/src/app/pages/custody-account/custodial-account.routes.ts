import { Route } from '@angular/router';
import { Page404Component } from 'app/authentication/page404/page404.component';
import { CustodyAccComponent } from './custody-acc/custody-acc.component';
import { PayInHistoryComponent } from './pay-in-history/pay-in-history.component';
import { CustodyAccountLinkComponent } from '../../public/custody-account-link/custody-account-link.component';

export const CUSTODY_ACCOUNT_ROUTE: Route[] = [
  {
    path: 'create-link',
    component: CustodyAccComponent,
  },
  {
    path: 'pay-in-history',
    component: PayInHistoryComponent,
  },
  {
    path: 'custodial-account',
    loadChildren: () =>
      import('./custodial-account.routes').then(
        (m) => m.CUSTODY_ACCOUNT_ROUTE
      ),
  },
  { path: '**', component: Page404Component },
];