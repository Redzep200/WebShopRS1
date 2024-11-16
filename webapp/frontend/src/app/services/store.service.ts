import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';
import { Store } from 'src/app/shared/models/store.model';

@Injectable({
  providedIn: 'root'
})
export class StoreService {
  private apiUrl = 'https://localhost:7023/api/Store';

  constructor(private http: HttpClient) {}

  getAllStores(): Observable<Store[]> {
    return this.http.get<Store[]>(`${this.apiUrl}/GetAllStores`);
  }

  getStoreById(id: number): Observable<Store> {
    return this.http.get<Store>(`${this.apiUrl}/GetStoreById/${id}`);
  }

  createStore(store: Store): Observable<Store> {
    return this.http.post<Store>(`${this.apiUrl}/CreateStore`, store);
  }

  updateStore(id: number, store: Store): Observable<any> {
    return this.http.put(`${this.apiUrl}/UpdateStore/${id}`, store);
  }

  deleteStore(id: number): Observable<any> {
    return this.http.delete(`${this.apiUrl}/DeleteStore/${id}`);
  }
}