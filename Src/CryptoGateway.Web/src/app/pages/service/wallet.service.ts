import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { environment } from 'environments/environment';

@Injectable({
  providedIn: 'root'
})
export class WalletService {

  constructor(private http: HttpClient) { }

  createWallet(title: string, currencyType: string, network: string, address: string) {
    return this.http.post<any>(`${environment.apiUrl}/wallets`,
      {
        title: title, currencyType: currencyType, network: network, address: address
      });
  }

  EditWallet(walletId: number, title: string, currencyType: string, network: string, address: string) {
    return this.http.put<any>(`${environment.apiUrl}/wallets`,
      {
        walletId: walletId, title: title, currencyType: currencyType, network: network, address: address
      });
  }

  RemoveWallet(walletId: number) {   
    return this.http.delete<any>(`${environment.apiUrl}/wallets/${walletId}`);
  }

  getWalletList(page: number, pageSize: number) {
    return this.http.get<any>(`${environment.apiUrl}/wallets/list?page=${page}&pageSize=${pageSize}`)
  }

  getApprovedWalletList() {
    return this.http.get<any>(`${environment.apiUrl}/wallets/approved-list`)
  }
}
