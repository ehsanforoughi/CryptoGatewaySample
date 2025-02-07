import { Component, OnInit } from '@angular/core';
import { FormGroup, FormControl } from '@angular/forms';
import { SnackBarAlerts } from '@shared/common/snack-bar.alerts';
import { MyGrid } from '@shared/components/my-grid/my-grid.model';
import { Pagination } from '@shared/components/my-grid/pagination.model';
import { SharedModule } from '@shared/shared.module';
import { BankAccountService } from 'app/pages/service/bank-account.service';
import { PayoutService } from 'app/pages/service/payout.service';
import { UserService } from 'app/pages/service/user.service';
import { CurrencyMaskModule } from "ng2-currency-mask";

@Component({
  selector: 'app-fiat-payout',
  standalone: true,
  imports: [
    SharedModule,
    CurrencyMaskModule
  ],
  templateUrl: './fiat-payout.component.html',
  styleUrl: './fiat-payout.component.scss'
})
export class FiatPayoutComponent implements OnInit {
  pagination: Pagination = new Pagination();
  source: any;
  bankTypes: any[] = [];
  buttonDisabled: boolean = false;
  onUpdating: boolean = false;
  IrrRealBalance: number = 0;
  IrrAvailableBalance: number = 0;
  bankAccountId: number = 0;
  breadscrums = [
    {
      title: 'Fiat Payout',
      items: ['payout'],
      active: 'Fiat Payout',
    },
  ];

  formGroup = new FormGroup({
    id: new FormControl(),
    bankType: new FormControl(null),
    value: new FormControl(0),
    currencyType: new FormControl(''),
    cardNumber: new FormControl(''),
  });

  constructor(
    private payoutService: PayoutService,
    private bankAccountService: BankAccountService,
    private userService: UserService,          
    private snackBar: SnackBarAlerts
  ) {}
  
  columns: MyGrid[] = [
    { prop: 'bankType', name: 'Bank' },
    { prop: 'value', name: 'Value' , isDecimal: true},
    { prop: 'currencyType', name: 'Currency Type' },
    { prop: 'cardNumber', name: 'Card Number' },
    { prop: 'approverFullName', name: 'Approver' },
    { prop: 'approvingState', name: 'State' },
    { prop: 'approverDesc', name: 'Approver Desc' },
    { prop: 'CreatedAt', name: 'Created At' },
  ];

  ngOnInit() {
    this.getUserCredits();
    this.getFiatPayouts(1, 5);
    this.getApprovedBankAccountList();
  }

  private setPagination(values: any): Pagination {
    return Object.assign(new Pagination(), {
      currentPage: values.pageNumber,
      pageSize: values.pageSize,
      totalCount: values.totalCount,
      collectionSize: values.totalCount
    });
  }
 
  getFiatPayouts(pageNumber: number, pageSize: number) {
    this.payoutService.getFiatPayoutList(pageNumber, pageSize)
      .subscribe((response: any) => {
        if (response.isSuccess === true) {
          this.source = [];
          response.result.items.forEach((element: any) => {
            let res: any = {};
            res.id = element.payoutId;
            res.approverFullName = element.approverFullName;
            res.bankType = element.bankType;
            res.value = element.value;
            res.currencyType = element.currencyType;
            res.cardNumber = element.cardNumber;
            res.bankTrackingCode = element.bankTrackingCode;
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

  getApprovedBankAccountList() {
    this.bankAccountService.getApprovedBankList()
      .subscribe((response: any) => {
        if (response.isSuccess === true) {
          this.bankTypes = [];
          response.result.items.forEach((element: any) => {
            let res: any = {};
            res.bankAccountId = element.bankAccountId;
            res.selectTitle = element.selectTitle;
            res.cardNumber = element.cardNumber;
  
            this.bankTypes.push(res);
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
        this.snackBar.errorSnack('Please enter the fiat value');
        return;
      }
      this.buttonDisabled = true;
      this.payoutService.createFiatPayout(this.formGroup.value.value!, this.bankAccountId)
        .subscribe((response) => {
          if (response.isSuccess === true) {
            this.buttonDisabled = false;
            this.getFiatPayouts(1, 5);
            this.formGroup.reset();
            Object.keys(this.formGroup.controls).forEach(key => {
              let control = this.formGroup.get(key);
              if (control) {
                control.setErrors(null);
              }
            });
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
    this.getFiatPayouts($event.pageIndex + 1, $event.pageSize);
  }

  onBankChanged($event: any) {
    this.bankAccountId = $event.value;
    this.formGroup.get('cardNumber')?.setValue(this.bankTypes
        .filter(x => x.bankAccountId == this.bankAccountId)[0]!.cardNumber);
  }

  getUserCredits() {
    this.userService.getUserCredits().subscribe((response) => {
      if (response.isSuccess === true) {
        response.result.forEach((element: any) => {
          if (element.currencyType === 'IRR') {
            this.IrrRealBalance = element.realBalance;
            this.IrrAvailableBalance = element.availableBalance;
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
