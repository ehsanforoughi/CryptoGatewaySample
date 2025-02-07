import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { environment } from 'environments/environment';

@Injectable({
  providedIn: 'root'
})
export class PayoutService {

  constructor(private http: HttpClient) { }

  createFiatPayout(amount: number, bankAccountId: number) {   
    return this.http.post<any>(`${environment.apiUrl}/payouts/fiat`,
      {
        amount: amount, currencyCode: 'IRR', bankAccountId: bankAccountId
      });
  }

  createCryptoPayout(amount: number, walletId: number) {   
    return this.http.post<any>(`${environment.apiUrl}/payouts/crypto`,
      {
        amount: amount, currencyCode: 'USDT', walletId: walletId
      });
  }

  // EditBankAccount(bankAccountId: number, title: string, bankType: number, cardNumber: string,
  //   sheba: string, accountNumber: string) {   
  //   return this.http.put<any>(`${environment.apiUrl}/bank-accounts`,
  //     {
  //       bankAccountId: bankAccountId, title: title, bankType: bankType,
  //       cardNumber: cardNumber, sheba: sheba, accountNumber: accountNumber
  //     });
  // }

  // RemoveBankAccount(bankAccountId: number) {   
  //   return this.http.delete<any>(`${environment.apiUrl}/bank-accounts/${bankAccountId}`);
  // }

  getFiatPayoutList(page: number, pageSize: number) {
    return this.http.get<any>(`${environment.apiUrl}/payouts/fiat/list?page=${page}&pageSize=${pageSize}`)
  }

  getCryptoPayoutList(page: number, pageSize: number) {
    return this.http.get<any>(`${environment.apiUrl}/payouts/crypto/list?page=${page}&pageSize=${pageSize}`)
  }
}
