import { Component, OnInit } from '@angular/core';
import { SharedModule } from '@shared/shared.module';
import { MyGrid } from '@shared/components/my-grid/my-grid.model';
import { PayInService } from 'app/pages/service/pay-in.service';
import { ClipboardService } from 'ngx-clipboard';
import { Pagination } from '@shared/components/my-grid/pagination.model';
import { SnackBarAlerts } from '@shared/common/snack-bar.alerts';

@Component({
  selector: 'app-pay-in-history',
  standalone: true,
  imports: [
    SharedModule
  ],
  templateUrl: './pay-in-history.component.html',
  styleUrl: './pay-in-history.component.scss'
})
export class PayInHistoryComponent implements OnInit {
  pagination: Pagination = new Pagination();
  source: any;
  breadscrums = [
    {
      title: 'Pay In (Deposit) History',
      items: ['Custodial Account'],
      active: 'Pay In (Deposit) History',
    },
  ];

  columns: MyGrid[] = [
    // { prop: 'id', name: 'Pay In ID' },
    { prop: 'customerId', name: 'Customer ID' },
    { prop: 'currencyType', name: 'Currency Type' },
    { prop: 'value', name: 'Value' },
    { prop: 'commissionValue', name: 'Commission' },
    { prop: 'approvingState', name: 'State' },
    { prop: 'txId', name: 'TX ID', isLong: true },
    { prop: 'customerContact', name: 'Customer Contact' },
    { prop: 'createdAt', name: 'Created At' },
    { prop: 'expiredAt', name: 'Expired At' },
  ];

  constructor(private payInService: PayInService,
              private snackBar: SnackBarAlerts,
              private clipboardService: ClipboardService) { }

  ngOnInit() {
    this.getPayInList(1, 5);
  }

  private setPagination(values: any): Pagination {
    return Object.assign(new Pagination(), {
      currentPage: values.pageNumber,
      pageSize: values.pageSize,
      totalCount: values.totalCount,
      collectionSize: values.totalCount
    });
  }

  getPayInList(pageNumber: number, pageSize: number) {
    this.payInService.getPayInList(pageNumber, pageSize)
      .subscribe((response) => {
        if (response.isSuccess === true) {
          this.source = [];
          response.result.items.forEach((element: any) => {
            let res: any = {};
              res.id = element.PayInId,
              res.customerId = element.customerId,
              res.value = element.value,
              res.commissionValue = element.commissionValue,
              res.currencyType = element.currencyType,
              res.approvingState = element.approvingState,
              res.txId = element.txId,
              res.customerContact = element.customerContact,
              res.createdAt = element.createdAt,
              res.modifiedAt = element.modifiedAt,
              res.expiredAt = element.expiredAt,
          
            this.source.push(res);
          });
          this.pagination = this.setPagination(response.result);
        } else {
          this.snackBar.errorSnack(response.message);
          return;
        }
      });
  }

  onCopyLongTextAction($event: any) {
    this.clipboardService.copyFromContent($event);
    this.snackBar.successSnack('Copied to the clipboard');
  }

  onRefresh($event: any) {
    this.getPayInList($event.pageIndex + 1, $event.pageSize);
  }
}
