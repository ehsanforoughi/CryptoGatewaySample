import { Component, OnInit } from '@angular/core';
import { FormGroup, FormControl } from '@angular/forms';
import { SnackBarAlerts } from '@shared/common/snack-bar.alerts';
import { MyGrid } from '@shared/components/my-grid/my-grid.model';
import { Pagination } from '@shared/components/my-grid/pagination.model';
import { SharedModule } from '@shared/shared.module';
import { PayoutService } from 'app/pages/service/payout.service';
import { UserService } from 'app/pages/service/user.service';
import { WalletService } from 'app/pages/service/wallet.service';
import { CurrencyMaskModule } from "ng2-currency-mask";

@Component({
  selector: 'app-crypto-payout',
  standalone: true,
  imports: [
    SharedModule,
    CurrencyMaskModule
  ],
  templateUrl: './crypto-payout.component.html',
  styleUrl: './crypto-payout.component.scss'
})
export class CryptoPayoutComponent {
  pagination: Pagination = new Pagination();
  source: any;
  wallets: any[] = [];
  buttonDisabled: boolean = false;
  onUpdating: boolean = false;
  UsdtRealBalance: number = 0;
  UsdtAvailableBalance: number = 0;
  walletId: number = 0;
  breadscrums = [
    {
      title: 'Crypto Payout',
      items: ['payout'],
      active: 'Crypto Payout',
    },
  ];

  formGroup = new FormGroup({
    id: new FormControl(),
    walletAddress: new FormControl(''),
    value: new FormControl(0),
    currencyType: new FormControl(''),
  });

  constructor(
    private payoutService: PayoutService,
    private walletService: WalletService,
    private userService: UserService,          
    private snackBar: SnackBarAlerts
  ) {}
  
  columns: MyGrid[] = [
    { prop: 'value', name: 'Value' , isDecimal: true},
    { prop: 'currencyType', name: 'Currency Type' },
    { prop: 'walletAddress', name: 'Wallet Address' },
    { prop: 'approverFullName', name: 'Approver' },
    { prop: 'approvingState', name: 'State' },
    { prop: 'approverDesc', name: 'Approver Desc' },
    { prop: 'CreatedAt', name: 'Created At' },
  ];

  ngOnInit() {
    this.getUserCredits();
    this.getCryptoPayouts(1, 5);
    this.getApprovedWalletList();
  }

  private setPagination(values: any): Pagination {
    return Object.assign(new Pagination(), {
      currentPage: values.pageNumber,
      pageSize: values.pageSize,
      totalCount: values.totalCount,
      collectionSize: values.totalCount
    });
  }
 
  getCryptoPayouts(pageNumber: number, pageSize: number) {
    this.payoutService.getCryptoPayoutList(pageNumber, pageSize)
      .subscribe((response: any) => {
        if (response.isSuccess === true) {
          this.source = [];
          response.result.items.forEach((element: any) => {
            let res: any = {};
            res.id = element.payoutId;
            res.approverFullName = element.approverFullName;
            res.value = element.value;
            res.currencyType = element.currencyType;
            res.walletAddress = element.walletAddress;
            res.transactionUrl = element.transactionUrl;
            res.approvingState = element.approvingState;
            res.approverDesc = element.approverDesc;
            res.createdAt = element.createdAt;

            this.source.push(res);
          });
          this.pagination = this.setPagination(response.result);
        } else {
          this.snackBar.errorSnack(response.message);
          return;
        }
      });
  }

  getApprovedWalletList() {
    this.walletService.getApprovedWalletList()
      .subscribe((response: any) => {
        if (response.isSuccess === true) {
          this.wallets = [];
          response.result.items.forEach((element: any) => {
            let res: any = {};
            res.walletId = element.walletId;
            res.walletTitle = element.walletTitle;
            res.walletAddress = element.walletAddress;
            res.currencyType = element.currencyType;
            res.selectTitle = element.selectTitle;
  
            this.wallets.push(res);
            return;
          });
        } else {
          this.snackBar.errorSnack(response.message);
          return;
        }
      });
  }

  onCancel() {
    this.formGroup.reset();
    //this.bankSelectedValue = 0;
    this.onUpdating = false;
  }

  onSubmitClick() {
    if (this.formGroup.valid) {
      if (this.formGroup.value.value === 0) {
        this.snackBar.errorSnack('Please enter the crypto value');
        return;
      }
      this.buttonDisabled = true;
      this.payoutService.createCryptoPayout(this.formGroup.value.value!, this.walletId)
        .subscribe((response) => {
          if (response.isSuccess === true) {
            this.formGroup.reset();
            Object.keys(this.formGroup.controls).forEach(key => {
              let control = this.formGroup.get(key);
              if (control) {
                control.setErrors(null);
              }
            });
            this.buttonDisabled = false;
            this.getCryptoPayouts(1, 5);
            this.snackBar.successSnack(response.message);
            return;
          } else {
            this.snackBar.errorSnack(response.message);
            this.buttonDisabled = false;
            return;
          }
        });
      this.buttonDisabled = false;
    }
  }

  onRefresh($event: any) {
    this.getCryptoPayouts($event.pageIndex + 1, $event.pageSize);
  }

  onWalletChanged($event: any) {
    this.walletId = $event.value;
    this.formGroup.get('walletAddress')?.setValue(this.wallets
        .filter(x => x.walletId == this.walletId)[0]!.walletAddress);
  }

  getUserCredits() {
    this.userService.getUserCredits().subscribe((response) => {
      if (response.isSuccess === true) {
        response.result.forEach((element: any) => {
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

  creditRefresh() {
    this.getUserCredits();
    this.snackBar.successSnack('Refreshing done');
  }
}
