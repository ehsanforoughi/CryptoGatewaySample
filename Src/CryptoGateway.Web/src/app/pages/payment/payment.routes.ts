import { Route } from '@angular/router';
import { Page404Component } from 'app/authentication/page404/page404.component';
import { CreatePaymentLinkComponent } from './payment-link/create-payment-link.component';

export const PAYMENT_ROUTE: Route[] = [
  {
    path: 'payment-links',
    component: CreatePaymentLinkComponent,
  },
  {
    path: 'payment',
    loadChildren: () =>
      import('./payment.routes').then(
        (m) => m.PAYMENT_ROUTE
      ),
  },
  { path: '**', component: Page404Component },
];