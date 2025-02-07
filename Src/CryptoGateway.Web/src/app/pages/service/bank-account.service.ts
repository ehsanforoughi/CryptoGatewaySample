import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { environment } from 'environments/environment';

@Injectable({
  providedIn: 'root'
})
export class BankAccountService {

  constructor(private http: HttpClient) { }

  createBankAccount(title: string, bankType: number, cardNumber: string,
    sheba: string, accountNumber: string) {   
    return this.http.post<any>(`${environment.apiUrl}/bank-accounts`,
      {
        title: title, bankType: bankType, cardNumber: cardNumber,
        sheba: sheba, accountNumber: accountNumber
      });
  }

  EditBankAccount(bankAccountId: number, title: string, bankType: number, cardNumber: string,
    sheba: string, accountNumber: string) {   
    return this.http.put<any>(`${environment.apiUrl}/bank-accounts`,
      {
        bankAccountId: bankAccountId, title: title, bankType: bankType,
        cardNumber: cardNumber, sheba: sheba, accountNumber: accountNumber
      });
  }

  RemoveBankAccount(bankAccountId: number) {   
    return this.http.delete<any>(`${environment.apiUrl}/bank-accounts/${bankAccountId}`);
  }

  getBankAccountList(page: number, pageSize: number) {
    return this.http.get<any>(`${environment.apiUrl}/bank-accounts/list?page=${page}&pageSize=${pageSize}`)
  }

  getBankTypeList() {
    return this.http.get<any>(`${environment.apiUrl}/bank-accounts/bank-types`)
  }

  getApprovedBankList() {
    return this.http.get<any>(`${environment.apiUrl}/bank-accounts/approved-list`)
  }
}
