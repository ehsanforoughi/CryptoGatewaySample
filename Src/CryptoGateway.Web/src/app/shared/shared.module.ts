import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { BreadcrumbComponent } from './components/breadcrumb/breadcrumb.component';
import { RouterLink, RouterModule } from '@angular/router';
import { MatButtonModule } from '@angular/material/button';
import { MatPaginator, MatPaginatorModule } from '@angular/material/paginator';
import { MatListModule } from '@angular/material/list';
import { MyGridComponent } from '@shared/components/my-grid/my-grid.component';
import { ClipboardModule } from 'ngx-clipboard';
import { FeatherModule } from 'angular-feather';
import { NgbAlertModule, NgbTooltipModule } from '@ng-bootstrap/ng-bootstrap';
import { FormsModule, ReactiveFormsModule } from '@angular/forms';
import { NgxMaskDirective } from 'ngx-mask';
import { MatFormFieldModule } from '@angular/material/form-field';
import { MatSelectModule } from '@angular/material/select';
import { MatInputModule } from '@angular/material/input';
import { MatIconModule } from '@angular/material/icon';
import { MatTableModule } from '@angular/material/table';
import { MatSort } from '@angular/material/sort';
import { SocialLoginModule, SocialAuthServiceConfig } from '@abacritt/angularx-social-login';
import { GoogleLoginProvider } from '@abacritt/angularx-social-login';

@NgModule({
  declarations: [
    BreadcrumbComponent,
    MyGridComponent,
  ],
  imports: [
    CommonModule,
    RouterLink,
    FormsModule,
    ReactiveFormsModule,
    FeatherModule,
    RouterModule,
    MatFormFieldModule,
    MatSelectModule,
    MatInputModule,
    MatIconModule,
    MatButtonModule,
    MatPaginator,
    MatSort,
    MatPaginatorModule,
    MatListModule,
    ClipboardModule,
    //NgbPaginationModule,
    NgbTooltipModule,
    NgxMaskDirective,
    MatTableModule,
    NgbAlertModule,
    SocialLoginModule
  ],
  exports: [
    CommonModule,
    BreadcrumbComponent,
    MyGridComponent,
    RouterLink,
    FormsModule,
    ReactiveFormsModule,
    FeatherModule,
    RouterModule,
    MatFormFieldModule,
    MatSelectModule,
    MatInputModule,
    MatIconModule,
    MatButtonModule,
    MatPaginator,
    MatSort,
    MatPaginatorModule,
    MatListModule,
    ClipboardModule,
    //NgbPaginationModule,
    NgbTooltipModule,
    NgxMaskDirective,
    MatTableModule,
    NgbAlertModule
  ]
})
export class SharedModule { }
