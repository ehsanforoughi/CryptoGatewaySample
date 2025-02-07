import { Component, OnInit } from '@angular/core';
import { SharedModule } from '@shared/shared.module';
import { MatDialog } from '@angular/material/dialog';
import { CustodyAccountService } from 'app/pages/service/custody-account.service';
import { CustodyModalComponent } from './custody-modal/custody-modal.component';
import { MyGrid } from '@shared/components/my-grid/my-grid.model';
import { ClipboardService } from 'ngx-clipboard';
import { Pagination } from '@shared/components/my-grid/pagination.model';
import { SnackBarAlerts } from '@shared/common/snack-bar.alerts';
@Component({
  selector: 'app-custody-acc',
  standalone: true,
  imports: [
    SharedModule,
  ],
  templateUrl: './custody-acc.component.html',
  styleUrl: './custody-acc.component.scss'
})
export class CustodyAccComponent implements OnInit {
  source: any;
  pagination: Pagination = new Pagination();
  breadscrums = [
    {
      title: 'Create Link',
      items: ['Custodial Account'],
      active: 'Create Link',
    },
  ];

  columns: MyGrid[] = [
    { prop: 'custodyAccId', name: 'Custody Account ID' },
    { prop: 'title', name: 'Title' },
    { prop: 'currencyType', name: 'Currency Type' },
    { prop: 'customerId', name: 'Customer ID' },
    { prop: 'custodyAccountLink', name: 'Custody Account Link', isHyperLink: true, hyperLinkText: "Link..." },
    { prop: 'createdAt', name: 'Created At' },
  ];
  
  constructor(private dialogModel: MatDialog,
    private custodyAccountService: CustodyAccountService,
    private clipboardService: ClipboardService,
    private snackBar: SnackBarAlerts) { }

  ngAfterViewInit() {
    //this.dataSource.paginator = this.paginator;
  }

  ngOnInit() {
    this.getCustodyAccList(1, 5);
  }


  openDialog(): void {
    this.dialogModel.open(CustodyModalComponent, {
      width: '640px',
      disableClose: true,
    });
  }

  onRefresh($event: any) {
    this.getCustodyAccList($event.pageIndex + 1, $event.pageSize);
  }

  private setPagination(values: any): Pagination {
    return Object.assign(new Pagination(), {
      currentPage: values.pageNumber,
      pageSize: values.pageSize,
      totalCount: values.totalCount,
      collectionSize: values.totalCount
    });
  }

  getCustodyAccList(pageNumber: number, pageSize: number) {
    this.custodyAccountService.getCustodyAccountList(pageNumber, pageSize)
      .subscribe((response) => {
        if (response.isSuccess === true) {
          this.source = [];
          response.result.items.forEach((element: any) => {
            let res: any = {};
            res.custodyAccId = element.custodyAccId;
            res.title = element.title;
            res.currencyType = element.currencyType;
            res.customerId = element.customerId;
            res.createdAt = element.createdAt;
            res.custodyAccountLink = element.custodyAccountLink;
          
            this.source.push(res);
          });

          this.pagination = this.setPagination(response.result);
        } else {
          this.snackBar.errorSnack(response.message);
          return;
        }
      });
  }

  onCopyAction($event: any) {
    this.clipboardService.copyFromContent($event.custodyAccountLink);
    this.snackBar.successSnack('Copied to the clipboard');
  }
}
