import { AfterViewInit, Component, OnInit, ViewChild } from '@angular/core';
import { MatPaginator } from '@angular/material/paginator';
import { MatTableDataSource } from '@angular/material/table';
import { PaymentModalComponent } from '././payment-modal/payment-modal.component';
import { MatDialog } from '@angular/material/dialog';
import { PaymentService } from 'app/pages/service/payment.service';
import { SharedModule } from '@shared/shared.module';
import { MyGrid } from '@shared/components/my-grid/my-grid.model';
import { Pagination } from '@shared/components/my-grid/pagination.model';
import { SnackBarAlerts } from '@shared/common/snack-bar.alerts';
import { ClipboardService } from 'ngx-clipboard';

export interface PeriodicElement {
  //no: number;
  paymentId: string,
  orderId: string;
  price: number;
  currency: string;
  paymentLink: string;
  createdAt: string;
}

@Component({
  selector: 'app-payment-link',
  standalone: true,
  imports: [
    SharedModule
  ],
  templateUrl: './create-payment-link.component.html',
  styleUrl: './create-payment-link.component.scss'
})
export class CreatePaymentLinkComponent implements OnInit, AfterViewInit {
  source: any;
  pagination: Pagination = new Pagination();
  breadscrums = [
    {
      title: 'Create Link',
      items: ['Payment'],
      active: 'Create Link',
    },
  ];

  displayedColumns: string[] = [
    //'no',
    'paymentId',
    'orderId',
    'price',
    'currency',
    'paymentLink',
    'createdAt',
  ];
  ELEMENT_DATA: PeriodicElement[] = [];
  dataSource = new MatTableDataSource<PeriodicElement>(this.ELEMENT_DATA);
  @ViewChild(MatPaginator) paginator!: MatPaginator;
  
  columns: MyGrid[] = [
    // { prop: 'paymentId', name: 'Payment ID' },
    { prop: 'customerId', name: 'Customer ID' },
    { prop: 'orderId', name: 'order ID' },
    { prop: 'orderDesc', name: 'order Desc' },
    { prop: 'priceAmount', name: 'Amount', isDecimal: true },
    { prop: 'priceCurrencyType', name: 'Currency Type' },
    { prop: 'paymentLink', name: 'Payment Link', isHyperLink: true, hyperLinkText: "Link..." },
    { prop: 'createdAt', name: 'Created At' },
  ];
  
  constructor(private dialogModel: MatDialog,
    private paymentService: PaymentService,
    private clipboardService: ClipboardService,
    private snackBar: SnackBarAlerts) {
    //constructor
  }

  ngAfterViewInit() {
    this.dataSource.paginator = this.paginator;
  }

  ngOnInit() {
    this.getPaymentList(1, 10);
  }

  openDialog(): void {
    const dialogRef = this.dialogModel.open(PaymentModalComponent, {
      width: '640px',
      disableClose: true,
    });

    dialogRef.afterClosed().subscribe((result) => {
      if (result === 'confirmed')
        this.getPaymentList(1, 10);
    });
  }

  onRefresh($event: any) {
    this.getPaymentList($event.pageIndex + 1, $event.pageSize);
  }

  onCopyAction($event: any) {
    this.clipboardService.copyFromContent($event.paymentLink);
    this.snackBar.successSnack('Copied to the clipboard');
  }
  
  private setPagination(values: any): Pagination {
    return Object.assign(new Pagination(), {
      currentPage: values.pageNumber,
      pageSize: values.pageSize,
      totalCount: values.totalCount,
      collectionSize: values.totalCount
    });
  }
  
  getPaymentList(pageNumber: number, pageSize: number) {
    this.paymentService.getPaymentList(pageNumber, pageSize)
      .subscribe((response) => {
        // console.log('response', response);
        if (response.isSuccess) {
          this.source = [];
          response.result.items.forEach((element: any) => {
            let res: any = {};
            res.paymentId = element.paymentId;
            res.customerId = element.customerId;
            res.orderId = element.orderId;
            res.orderDesc = element.orderDesc;
            res.priceAmount = element.priceAmount;
            res.priceCurrencyType = element.priceCurrencyType;
            res.createdAt = element.createdAt;
            res.paymentLink = element.paymentLink;
  
            this.source.push(res);
          });

          this.pagination = this.setPagination(response.result);
        }else {
          this.snackBar.errorSnack(response.message);
          return;
        }

        // this.dataSource.data = this.ELEMENT_DATA;
        // this.dataSource.paginator = this.paginator;
      });
  }
}
