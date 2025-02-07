import { Component, OnInit } from '@angular/core';
import { FeatherIconsComponent } from '@shared/components/feather-icons/feather-icons.component';
import { NgScrollbar } from 'ngx-scrollbar';
import { MatIconModule } from '@angular/material/icon';
import { MatMenuModule } from '@angular/material/menu';
import { MatButtonModule } from '@angular/material/button';
import { SharedModule } from '@shared/shared.module';
import { UserService } from 'app/pages/service/user.service';
import { SnackBarAlerts } from '@shared/common/snack-bar.alerts';

@Component({
  selector: 'app-main',
  templateUrl: './main.component.html',
  styleUrls: ['./main.component.scss'],
  standalone: true,
  imports: [
    SharedModule,
    MatButtonModule,
    MatMenuModule,
    MatIconModule,
    NgScrollbar,
    FeatherIconsComponent,
  ],
})
export class MainComponent implements OnInit {
  IrrRealBalance: number = 0;
  IrrAvailableBalance: number = 0;
  UsdtRealBalance: number = 0;
  UsdtAvailableBalance: number = 0;
  breadscrums = [
    {
      title: 'Dashboad',
      items: [],
      active: 'Dashboard',
    },
  ];

  constructor(private userService: UserService, private snackBar: SnackBarAlerts) {
    //constructor
  }

  ngOnInit() {
    this.getUserCredits();
  }

  getUserCredits() {
    this.userService.getUserCredits().subscribe((response) => {
      if (response.isSuccess === true) {
        response.result.forEach((element: any) => {
          if (element.currencyType === 'IRR') {
            this.IrrRealBalance = element.realBalance;
            this.IrrAvailableBalance = element.availableBalance;
          }

          if (element.currencyType === 'USDT') {
            this.UsdtRealBalance = element.realBalance;
            this.UsdtAvailableBalance = element.availableBalance;
          }
        });
      } else {
        this.snackBar.errorSnack(response.message);
        return;
      }
    });
  }
}