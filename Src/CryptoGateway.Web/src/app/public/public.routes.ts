import { Route } from "@angular/router";
import { Page404Component } from "app/authentication/page404/page404.component";
import { CustodyAccountLinkComponent } from "./custody-account-link/custody-account-link.component";
import { PaymentLinkComponent } from "./payment-link/payment-link.component";

export const PUBLIC_ROUTE: Route[] = [
  {
    path: 'custody-account/:id',
    component: CustodyAccountLinkComponent,
  },
  {
    path: 'payment/:id',
    component: PaymentLinkComponent,
  },
  { path: '**', component: Page404Component },
];
