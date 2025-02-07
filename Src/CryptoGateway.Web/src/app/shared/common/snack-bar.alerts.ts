// snack-bar.alerts.ts
import { Injectable } from '@angular/core';
import { MatSnackBar, MatSnackBarHorizontalPosition, MatSnackBarVerticalPosition } from '@angular/material/snack-bar';
import { HtmlSnackBar } from './html-snack-bar/html-snack-bar.component';

@Injectable({
  providedIn: 'root'
})
export class SnackBarAlerts {

  constructor(private snackBar: MatSnackBar) { }

  successSnack(message: string) {
    this.snackBar.open(message, '', {
      duration: 5000,
      verticalPosition: 'bottom',
      horizontalPosition: 'center',
      panelClass: 'snackbar-success',
    });
  }
   
  errorSnack(message: string) {
    this.snackBar.open(message, '', {
      duration: 5000,
      verticalPosition: 'bottom',
      horizontalPosition: 'center',
      panelClass: 'snackbar-danger',
    });
  }

  errorHtmlSnack(message: string, isList: boolean) {
    if (isList) message = this.convertTextToList(message.toString());
    this.snackBar.openFromComponent(HtmlSnackBar, {
      data: { html: message },
      duration: 10000,
      verticalPosition: 'bottom',
      horizontalPosition: 'center',
      panelClass: 'snackbar-danger',
    });
  }

  convertTextToList(text: string): string {
    const splitText = text.split(" | ");

    let html = '<ul>';
    for (let i = 0; i < splitText.length; i++) {
      html += `<li>${splitText[i]}</li>`;
    }
    html += '</ul>';
    return html;
  }
}
