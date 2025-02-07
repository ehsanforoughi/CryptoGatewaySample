import { Component, EventEmitter, OnInit, Output } from '@angular/core';
import { MatDialog, MatDialogTitle, MatDialogContent } from '@angular/material/dialog';
import { FormsModule, ReactiveFormsModule, FormControl, FormGroup, Validators } from '@angular/forms';
import { MatButtonModule } from '@angular/material/button';
import { MatInputModule } from '@angular/material/input';
import { NgxMaskDirective, provideNgxMask } from 'ngx-mask';
import { CurrencyMaskModule } from "ng2-currency-mask";
import { FloatLabelType, MatFormFieldModule } from '@angular/material/form-field';
import { CustodyAccountService } from 'app/pages/service/custody-account.service';
import { SnackBarAlerts } from '@shared/common/snack-bar.alerts';

@Component({
  selector: 'app-dialogform',
  templateUrl: './custody-modal.component.html',
  styleUrls: ['./custody-modal.component.scss'],
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
export class CustodyModalComponent implements OnInit {
  @Output('onChange') onChange = new EventEmitter();
  floatLabelControl = new FormControl('auto' as FloatLabelType);

  formGroup = new FormGroup({
    title: new FormControl(),
    currency: new FormControl('USDT'),
    customerId: new FormControl(),
  });

  constructor(public dialog: MatDialog,
    private custodyAccountService: CustodyAccountService,
    private snackBar: SnackBarAlerts) { }

  public ngOnInit() {}

  closeDialog(): void {
    this.dialog.closeAll();
  }

  onSubmitClick() {
    if (this.formGroup.valid) {
      this.custodyAccountService.createCustodyAccountLink(
      this.formGroup.value.customerId, this.formGroup.value.currency!, this.formGroup.value.title)
        .subscribe((response) => {
          if (response.isSuccess === true) {
            this.onChange.emit(response);
            this.snackBar.successSnack(response.message);
          } else {
            console.error(response.message);
            this.snackBar.errorSnack(response.message);
          }
        });
      this.dialog.closeAll();
    }
  }

  getFloatLabelValue(): FloatLabelType {
    return this.floatLabelControl.value || 'auto';
  }
}
