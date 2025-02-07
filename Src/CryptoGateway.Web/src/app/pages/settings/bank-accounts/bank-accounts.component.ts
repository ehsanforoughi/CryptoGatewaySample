import { Component, OnInit } from '@angular/core';
import { FormControl, FormGroup } from '@angular/forms';
import { provideNgxMask } from 'ngx-mask';
import { MyGrid } from '@shared/components/my-grid/my-grid.model';
import { BankAccountService } from 'app/pages/service/bank-account.service';
import { SharedModule } from '@shared/shared.module';
import { Pagination } from '@shared/components/my-grid/pagination.model';
import { SnackBarAlerts } from '@shared/common/snack-bar.alerts';
import Swal from 'sweetalert2';
// import { NgForm } from '@angular/forms';

@Component({
  selector: 'bank-accounts',
  standalone: true,
  imports: [
    SharedModule],
  templateUrl: './bank-accounts.component.html',
  styleUrl: './bank-accounts.component.scss',
  providers: [provideNgxMask()]
})
export class BankAccountsComponent implements OnInit {
  pagination: Pagination = new Pagination();
  source: any;
  bankTypes: any;
  buttonDisabled: boolean = false;
  onUpdating: boolean = false;
  breadscrums = [
    {
      title: 'Bank Accounts',
      items: ['Settings'],
      active: 'Bank Accounts',
    },
  ];

  formGroup = new FormGroup({
    id: new FormControl(),
    title: new FormControl(''),
    bankType: new FormControl(null),
    bankName: new FormControl(''),
    //bankName: new FormControl(Validators.required),
    cardNumber: new FormControl(''),
    sheba: new FormControl(''),
    accountNumber: new FormControl(''),
    state: new FormControl(),
    approvingState: new FormControl(''),
    reviewerDescription: new FormControl(''),
  });

  constructor(private bankAccountService: BankAccountService,
              private snackBar: SnackBarAlerts
  ) {}
  
  condition: ((row: any) => boolean) | undefined;

  columns: MyGrid[] = [
    //{ prop: 'icon', name: '', isIcon: true, iconSize: 40 },
    { prop: 'title', name: 'Title' },
    { prop: 'bankName', name: 'Bank' },
    { prop: 'cardNumber', name: 'Card Number' },
    { prop: 'sheba', name: 'Sheba' },
    { prop: 'accountNumber', name: 'Account Number' },
    { prop: 'approvingState', name: 'State' },
    { prop: 'reviewerDescription', name: 'Approver Desc' },
  ];

  ngOnInit() {
    this.getBanks(1, 5);
    this.getBankType();
    this.condition = (row) => row.state === 1 || row.state === 3;
  }

  private setPagination(values: any): Pagination {
    return Object.assign(new Pagination(), {
      currentPage: values.pageNumber,
      pageSize: values.pageSize,
      totalCount: values.totalCount,
      collectionSize: values.totalCount
    });
  }
 
  getBanks(pageNumber: number, pageSize: number) {
    this.bankAccountService.getBankAccountList(pageNumber, pageSize)
      .subscribe((response: any) => {
        if (response.isSuccess === true) {
          this.source = [];
          response.result.items.forEach((element: any) => {
            let res: any = {};
            res.id = element.bankAccountId;
            res.title = element.title;
            res.bankType = element.bankType;
            res.bankName = element.bankName;
            res.accountNumber = element.accountNumber;
            res.cardNumber = element.cardNumber;
            res.sheba = element.sheba;
            res.state = element.state;
            res.approvingState = element.approvingState;
            res.reviewerDescription = element.desc;

            this.source.push(res);
          });
          this.pagination = this.setPagination(response.result);
        } else {
          this.snackBar.errorSnack(response.message);
          return;
        }
      });
  }

  getBankType() {
    this.bankAccountService.getBankTypeList()
      .subscribe((response: any) => {
        if (response.isSuccess === true) {
          this.bankTypes = [];
          response.result.forEach((element: any) => {
            let res: any = {};
            res.bankTypeId = element.bankTypeId;
            res.bankName = element.bankName;
            res.bankType = element.bankType;
  
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
      this.buttonDisabled = true;
      this.bankAccountService.createBankAccount(this.formGroup.value.title!,
        this.formGroup.value.bankType!, this.formGroup.value.cardNumber!,
        this.formGroup.value.sheba?.toString().replace(/\s/g, '')!, this.formGroup.value.accountNumber!)
        .subscribe((response) => {
          if (response.isSuccess === true) {
            this.getBanks(1, 5);
            this.formGroup.reset();
            Object.keys(this.formGroup.controls).forEach(key => {
              let control = this.formGroup.get(key);
              if (control) {
                control.setErrors(null);
              }
            });
            this.buttonDisabled = false;
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

  onDeleteConfirm($event: any) {
    this.buttonDisabled = true;    
    this.confirmDelete().then((result) => {
      if (result.value) {
        this.bankAccountService.RemoveBankAccount($event.id).subscribe((response) => {
          if (response.isSuccess === true) {
            this.onRefresh($event);
            this.buttonDisabled = false;
            this.snackBar.successSnack(response.message);
            return;
          }
          else {
            console.error(response.message);
            this.buttonDisabled = false;              
            this.snackBar.errorSnack(response.message);
            return;
          }
        });
      } else {
        this.buttonDisabled = false;
        result.dismiss;
      }
    });
  }

  //  For confirm action On Save
  onEditConfirm($event: any) {
    this.buttonDisabled = true;
    this.bankAccountService.EditBankAccount(this.formGroup.value.id, this.formGroup.value.title!,
      this.formGroup.value.bankType!, this.formGroup.value.cardNumber!,
      this.formGroup.value.sheba?.toString().replace(/\s/g, '')!,
      this.formGroup.value.accountNumber!).subscribe((response) => {
        if (response.isSuccess === true) {
          this.formGroup.reset();
          this.onRefresh($event);
          this.onUpdating = false;
          this.buttonDisabled = false;
          this.snackBar.successSnack(response.message);
          return;
        }
        else {
          console.error(response.message);
          this.snackBar.errorSnack(response.message);
          this.buttonDisabled = false;
          return;
        }
      });
  }

  onRefresh($event: any) {
    this.getBanks($event.pageIndex + 1, $event.pageSize);
  }

  onEdit($event: any) {
    this.formGroup.setValue($event);
    this.onUpdating = true;
    // this.bankRef.focus()
  }

  onDelete($event: any) {
    this.onDeleteConfirm($event);
  }

  confirmDelete() {
    return Swal
      .fire({
        title: 'Are you sure?',
        //text: text,
        icon: 'warning',
        showCancelButton: true,
        confirmButtonColor: '#3085d6',
        cancelButtonColor: '#d33',
        confirmButtonText: 'Yes, delete it!',
      });
  }
}
