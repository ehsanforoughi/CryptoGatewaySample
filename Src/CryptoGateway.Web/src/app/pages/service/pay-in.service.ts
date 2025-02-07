import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { environment } from 'environments/environment';

@Injectable({
  providedIn: 'root'
})
export class PayInService {

  constructor(private http: HttpClient) { }

  getPayInList(page: number, pageSize: number) {
    return this.http.get<any>(`${environment.apiUrl}/deposits/list?page=${page}&pageSize=${pageSize}`)
  }
}
