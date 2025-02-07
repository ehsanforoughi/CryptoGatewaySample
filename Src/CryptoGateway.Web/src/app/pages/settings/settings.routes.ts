import { Route } from '@angular/router';
import { WalletsComponent } from './wallets/wallets.component';
import { BankAccountsComponent } from './bank-accounts/bank-accounts.component';
import { Page404Component } from 'app/authentication/page404/page404.component';

export const SETTINGS_ROUTE: Route[] = [
  {
    path: 'wallets',
    component: WalletsComponent,
  },
  {
    path: 'bank-accounts',
    component: BankAccountsComponent,
  },
  {
    path: 'settings',
    loadChildren: () =>
      import('./settings.routes').then(
        (m) => m.SETTINGS_ROUTE
      ),
  },
  { path: '**', component: Page404Component },
];