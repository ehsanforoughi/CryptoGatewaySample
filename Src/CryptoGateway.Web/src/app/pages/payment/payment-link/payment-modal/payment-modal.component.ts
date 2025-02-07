import { Component, EventEmitter, Inject, OnInit, Output } from '@angular/core';
import { MatDialog, MatDialogTitle, MatDialogContent, MatDialogRef, MAT_DIALOG_DATA } from '@angular/material/dialog';
import { FormsModule, ReactiveFormsModule, FormControl, FormGroup, Validators } from '@angular/forms';
import { MatButtonModule } from '@angular/material/button';
import { MatInputModule } from '@angular/material/input';
import { NgxMaskDirective, provideNgxMask } from 'ngx-mask';
import { CurrencyMaskModule } from "ng2-currency-mask";
import { FloatLabelType, MatFormFieldModule } from '@angular/material/form-field';
import { PaymentService } from 'app/pages/service/payment.service';
import { SnackBarAlerts } from '@shared/common/snack-bar.alerts';
@Component({
  selector: 'app-dialogform',
  templateUrl: './payment-modal.component.html',
  styleUrls: ['./payment-modal.component.scss'],
  standalone: true,
  imports: [
    MatDialogTitle,
    MatDialogContent,
    FormsModule,
    ReactiveFormsModule,
    MatFormFieldModule,
    MatInputModule,
    MatButtonModule,
    NgxMaskDirective,
    CurrencyMaskModule
  ],
  providers: [provideNgxMask()],
})
export class PaymentModalComponent implements OnInit {
  @Output('onChange') onChange = new EventEmitter();
  floatLabelControl = new FormControl('auto' as FloatLabelType);

  formGroup = new FormGroup({
    customerId: new FormControl('', Validators.required),
    customerContact: new FormControl(''),
    orderId: new FormControl(''),
    orderDesc: new FormControl(''),
    payAmount: new FormControl(''),
    payCurrency: new FormControl('USDT'),
  });

  constructor(//public dialog: MatDialog,
    public dialogRef: MatDialogRef<PaymentModalComponent>,
    @Inject(MAT_DIALOG_DATA) public data: any,
    private paymentService: PaymentService, private snackBar: SnackBarAlerts) { }

  public ngOnInit() {}

  closeDialog(): void {
    //this.dialog.closeAll();
    this.dialogRef.close('canceled');
  }

  onSubmitClick() {
    if (this.formGroup.valid) {
      this.paymentService.createPaymentLink(this.formGroup.value.payCurrency!, Number(this.formGroup.value.payAmount),
      this.formGroup.value.customerId!, this.formGroup.value.customerContact!, this.formGroup.value.orderId!, this.formGroup.value.orderDesc!)
        .subscribe((response) => {
          if (response.isSuccess === true) {
            this.onChange.emit(response);
            this.snackBar.successSnack(response.message);
            this.dialogRef.close('confirmed');
            return;
          } else {
            console.error(response.message);
            this.snackBar.errorSnack(response.message);
            return;
          }
        });
      //this.dialog.closeAll();
      //this.dialogRef.close();
    }
  }

  getFloatLabelValue(): FloatLabelType {
    return this.floatLabelControl.value || 'auto';
  }
}
