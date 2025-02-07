import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { environment } from 'environments/environment';

@Injectable({
  providedIn: 'root'
})
export class CustodyAccountService {

  constructor(private http: HttpClient) { }

  createCustodyAccountLink(customerId: string, currencyType: string, title: string) {   
    return this.http.post<any>(`${environment.apiUrl}/custody-accounts/deposit`,
      { customerId: customerId, currencyType: currencyType, title: title });
  }

  getCustodyAccountList(page: number, pageSize: number) { 
    return this.http.get<any>(`${environment.apiUrl}/custody-accounts/list?page=${page}&pageSize=${pageSize}`)
  }

  getCustodyAccount(custodyAccountId: string) {
    return this.http.get<any>(`${environment.apiUrl}/custody-accounts/${custodyAccountId}`)
  }

  inform(custodyAccountId: string, txId: string, receiver?: string) {   
    return this.http.post<any>(`${environment.apiUrl}/custody-accounts/inform`,
      {
        custodyAccountId: custodyAccountId, txId: txId, receiver: receiver
      }, {headers:{skip:"true"}});
  }
}
