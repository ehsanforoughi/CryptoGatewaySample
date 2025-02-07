import { Component, OnInit } from '@angular/core';
import { FormControl, FormGroup, Validators } from '@angular/forms';
import { NgxMaskDirective, provideNgxMask } from 'ngx-mask';
import { MyGrid } from '@shared/components/my-grid/my-grid.model';
import Swal from 'sweetalert2';
import { WalletService } from 'app/pages/service/wallet.service';
import { SharedModule } from '@shared/shared.module';
import { Pagination } from '@shared/components/my-grid/pagination.model';
import { SnackBarAlerts } from '@shared/common/snack-bar.alerts';

@Component({
  selector: 'wallets',
  standalone: true,
  imports: [
    SharedModule
  ],
  templateUrl: './wallets.component.html',
  styleUrl: './wallets.component.scss',
  providers: [provideNgxMask()]
})
export class WalletsComponent implements OnInit {
  pagination: Pagination = new Pagination();
  source: any;
  buttonDisabled: boolean = false;
  onUpdating: boolean = false;
  breadscrums = [
    {
      title: 'Wallets',
      items: ['Settings'],
      active: 'Wallets',
    },
  ];

  formGroup = new FormGroup({
    id: new FormControl(),
    title: new FormControl(''),
    address: new FormControl(''),
    // memo: new FormControl(''),
    // tag: new FormControl(''),
    // state: new FormControl(''),
    currencyType: new FormControl('USDT'),
    network: new FormControl('TRC20'),
    // approvingState: new FormControl(''),
    // approvedBy: new FormControl(''),
    // approver: new FormControl(''),
    // reviewerDescription: new FormControl(''),
  });
  
  constructor(private walletService: WalletService,
              private snackBar: SnackBarAlerts) {
    //constructor
  }

  condition: ((row: any) => boolean) | undefined;

  columns: MyGrid[] = [
    //{ prop: 'icon', name: '', isIcon: true, iconSize: 40 },
    { prop: 'title', name: 'Title' },
    { prop: 'currencyType', name: 'Currency Type' },
    { prop: 'address', name: 'Address' },
    { prop: 'approvingState', name: 'State' },
    { prop: 'reviewerDescription', name: 'Approver Desc' },
  ];

  ngOnInit() {
    this.getWallets(1, 5);
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

  getWallets(pageNumber: number, pageSize: number) {
    this.walletService.getWalletList(pageNumber, pageSize)
      .subscribe((response: any) => {
        if (response.isSuccess === true) {
          this.source = [];
          response.result.items.forEach((element: any) => {
            let res: any = {};
            res.id = element.walletId;
            res.title = element.title;
            res.currencyType = element.currencyType;
            res.network = element.network;
            res.address = element.address;
            res.memo = element.memo;
            res.tag = element.tag;
            res.state = element.state;
            res.approvingState = element.approvingState;
            res.approvedBy = element.approvedBy;
            res.approver = element.approver;
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

  onCancel() {
    this.formGroup.reset();
    this.formGroup.get('currencyType')?.setValue('USDT');
    this.formGroup.get('network')?.setValue('TRC20');
    this.onUpdating = false;
  }

  onSubmitClick() {
    if (this.formGroup.valid) {
      this.buttonDisabled = true;
      this.walletService.createWallet(this.formGroup.value.title!,
        this.formGroup.value.currencyType!, this.formGroup.value.network!,
        this.formGroup.value.address!)
        .subscribe((response) => {
          if (response.isSuccess === true) {
            this.getWallets(1, 5);
            this.formGroup.reset();
            this.formGroup.get('currencyType')?.setValue('USDT');
            this.formGroup.get('network')?.setValue('TRC20');
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
        this.walletService.RemoveWallet($event.id).subscribe((response) => {
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
    this.walletService.EditWallet(this.formGroup.value.id, this.formGroup.value.title!,
      this.formGroup.value.currencyType!, this.formGroup.value.network!,
      this.formGroup.value.address!).subscribe((response) => {
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
    this.getWallets($event.pageIndex + 1, $event.pageSize);
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
