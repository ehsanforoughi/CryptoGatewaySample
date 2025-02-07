import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { environment } from 'environments/environment';

@Injectable({
  providedIn: 'root'
})
export class PaymentService {

  constructor(private http: HttpClient) { }

    createPaymentLink(payCurrencyCode: string, priceAmount: number, customerid: string, customerContact: string, orderId: string, orderDescription: string) {   
    return this.http.post<any>(`${environment.apiUrl}/payments`,
      { payCurrencyCode: payCurrencyCode, priceAmount: priceAmount, customerid: customerid, customerContact: customerContact, orderId: orderId, orderDescription: orderDescription });
  }

  getPaymentList(page: number, pageSize: number) {
    return this.http.get<any>(`${environment.apiUrl}/payments/list?page=${page}&pageSize=${pageSize}`)
  }

  getPayment(paymentId: string) {
    return this.http.get<any>(`${environment.apiUrl}/payments/${paymentId}`)
  }

}
