import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';

@Injectable({
  providedIn: 'root'
})
export class StoreImageService {
  private apiUrl = 'https://localhost:7023/api/StoreImage';

  constructor(private http: HttpClient) {}

  addStoreImage(storeId: number, image: string): Observable<any> {
    return this.http.post(`${this.apiUrl}/AddStoreImage`, { storeId, image });
  }

  getStoreImage(storeId: number): Observable<any> {
    return this.http.get(`${this.apiUrl}/GetByStoreID?storeId=${storeId}`);
  }
}