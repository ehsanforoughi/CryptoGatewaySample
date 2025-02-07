import { Component, ElementRef, OnInit, ViewChild } from '@angular/core';
import { NgForm } from '@angular/forms';
import { ActivatedRoute } from '@angular/router';
import { NgbAlertConfig } from '@ng-bootstrap/ng-bootstrap';
import { SnackBarAlerts } from '@shared/common/snack-bar.alerts';
import { SharedModule } from '@shared/shared.module';
import { CustodyAccountService } from 'app/pages/service/custody-account.service';
import { ClipboardService } from 'ngx-clipboard';
import { NgxQrcodeStylingModule, NgxQrcodeStylingService, Options } from 'ngx-qrcode-styling';
import { Observable, mergeMap, of } from 'rxjs';

@Component({
  selector: 'app-custody-account-link',
  standalone: true,
  imports: [
    SharedModule,
    NgxQrcodeStylingModule
  ],
  templateUrl: './custody-account-link.component.html',
  styleUrl: './custody-account-link.component.scss',
  providers: [NgbAlertConfig]
})
export class CustodyAccountLinkComponent implements OnInit  {
  @ViewChild('f') form!: NgForm;
  @ViewChild("canvas", { static: false }) canvas!: ElementRef;
  qrCodeValue: string = '';
  contactValue: string = '';
  txIdValue: string = '';
  custodyAccountId: string = '';
  currencyType: string = '';
  network: string = '';
  public config: Options = {
    // "frameOptions": {
    //   "style": 'FE_113',
    //   "width": 350,
    //   "height": 350,
    //   "x": 5,
    //   "y": 5,
    // },
    "width": 180,
    "height": 180,
    "margin": 0,
    "qrOptions": {
      "typeNumber": 0,
      "mode": "Byte",
      "errorCorrectionLevel": "Q"
    },
    "imageOptions": {
      "hideBackgroundDots": true,
      "imageSize": 0.4,
      "margin": 0
    },
    "dotsOptions": {
      "type": "dots",
      "color": "#e18937",
    },
    "backgroundOptions": {
      "color": "#ffffff"
    },
    "image": "data:image/svg+xml;base64,PHN2ZyB4bWxucz0iaHR0cDovL3d3dy53My5vcmcvMjAwMC9zdmciIHZpZXdCb3g9IjAgMCAyMDAwIDIwMDAiIHdpZHRoPSIyMDAwIiBoZWlnaHQ9IjIwMDAiPjxwYXRoIGQ9Ik0xMDAwLDBjNTUyLjI2LDAsMTAwMCw0NDcuNzQsMTAwMCwxMDAwUzE1NTIuMjQsMjAwMCwxMDAwLDIwMDAsMCwxNTUyLjM4LDAsMTAwMCw0NDcuNjgsMCwxMDAwLDAiIGZpbGw9IiM1M2FlOTQiLz48cGF0aCBkPSJNMTEyMy40Miw4NjYuNzZWNzE4SDE0NjMuNlY0OTEuMzRINTM3LjI4VjcxOEg4NzcuNVY4NjYuNjRDNjAxLDg3OS4zNCwzOTMuMSw5MzQuMSwzOTMuMSw5OTkuN3MyMDgsMTIwLjM2LDQ4NC40LDEzMy4xNHY0NzYuNWgyNDZWMTEzMi44YzI3Ni0xMi43NCw0ODMuNDgtNjcuNDYsNDgzLjQ4LTEzM3MtMjA3LjQ4LTEyMC4yNi00ODMuNDgtMTMzbTAsMjI1LjY0di0wLjEyYy02Ljk0LjQ0LTQyLjYsMi41OC0xMjIsMi41OC02My40OCwwLTEwOC4xNC0xLjgtMTIzLjg4LTIuNjJ2MC4yQzYzMy4zNCwxMDgxLjY2LDQ1MSwxMDM5LjEyLDQ1MSw5ODguMjJTNjMzLjM2LDg5NC44NCw4NzcuNjIsODg0VjEwNTAuMWMxNiwxLjEsNjEuNzYsMy44LDEyNC45MiwzLjgsNzUuODYsMCwxMTQtMy4xNiwxMjEtMy44Vjg4NGMyNDMuOCwxMC44Niw0MjUuNzIsNTMuNDQsNDI1LjcyLDEwNC4xNnMtMTgyLDkzLjMyLTQyNS43MiwxMDQuMTgiIGZpbGw9IiNmZmYiLz48L3N2Zz4=",
    "cornersSquareOptions": {
      "type": "extra-rounded",
      "color": "#585656"
    },
    "cornersDotOptions": {
      "type": "dot",
      "color": "#53ae94"
    },
  };

  constructor(alertConfig: NgbAlertConfig,
    private route: ActivatedRoute,
    private custodyAccService: CustodyAccountService,
    private snackBar: SnackBarAlerts,
    private qrcode: NgxQrcodeStylingService,
    private clipboardService: ClipboardService,
  ) {
		// customize default values of alerts used by this component tree
		alertConfig.type = 'warning';
		alertConfig.dismissible = false;
  }
  
  ngOnInit() {
    this.route.params.pipe(
      mergeMap((params) => this.custodyAccService.getCustodyAccount(params['id'])),
      mergeMap((response: any) => this.generateQrCode(response))
    ).subscribe();
  }
  
  generateQrCode(response: any): Observable<any> {
    if (response.isSuccess === true && response.result) {
      this.custodyAccountId = response.result.custodyAccountId;
      this.currencyType = response.result.currencyType;
      this.network = response.result.network;
      this.qrCodeValue = response.result.address;
      // res.customerId = response.result.customerId;
      // res.title = response.result.title;
      //res.createdAt = response.result.createdAt;
      this.config.data = response.result.address;
      return this.qrcode.create(this.config, this.canvas.nativeElement);
    } else {
      console.error(response.message);
      this.snackBar.errorSnack("The Url address is not valid");
      return of();
    }
  }
  
  copy(address: any) {
    this.clipboardService.copyFromContent(address);
    this.snackBar.successSnack('Copied to the clipboard');
  }

  submit() {
    this.custodyAccService.inform(this.custodyAccountId, this.txIdValue, this.contactValue)
      .subscribe((response) => {
        if (response.isSuccess === true) {
          this.form.reset();
          this.snackBar.successSnack(response.message);
        } else {
          console.error(response.message);
          this.snackBar.errorSnack(response.message);
        }
      });
  }
}
