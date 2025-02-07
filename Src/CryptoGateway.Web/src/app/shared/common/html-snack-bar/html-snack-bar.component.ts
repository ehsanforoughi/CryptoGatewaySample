import { Component, Inject } from '@angular/core';
import { MAT_SNACK_BAR_DATA, MatSnackBar, MatSnackBarRef } from '@angular/material/snack-bar';
import { SharedModule } from '@shared/shared.module';

@Component({
  selector: 'app-html-snack-bar',
  template: `<div [innerHTML]="data.html"></div>
             <button mat-flat-button color="primary" (click)="snackBar.dismiss()">Dismiss</button>`,
  standalone: true,
  imports: [
    SharedModule
  ]
})
export class HtmlSnackBar {
  constructor(@Inject(MAT_SNACK_BAR_DATA) public data: any,
    public snackBar: MatSnackBar,
    private snackBarRef: MatSnackBarRef<HtmlSnackBar>) { }
  
  close(){
    this.snackBarRef.dismiss();
  }
}
