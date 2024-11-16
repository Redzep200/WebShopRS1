import { Injectable } from '@angular/core';
import { HttpClient, HttpHeaders, HttpParams } from '@angular/common/http';
import { Observable } from 'rxjs';
import { Supplier } from '../shared/models/supplier.model';

@Injectable({
  providedIn: 'root',
})
export class SupplierService {
  private suppliersApiUrl = 'https://localhost:7023/api/Supplier';

  constructor(private http: HttpClient) {}

  getAllSuppliers(): Observable<any[]> {
    return this.http.get<any[]>(`${this.suppliersApiUrl}/GetAllSuppliers`);
  }

  addSupplier(supplierData: any): Observable<any> {
    return this.http.post<any>(`${this.suppliersApiUrl}/CreateSupplier`, supplierData, {
      headers: new HttpHeaders({ 'Content-Type': 'application/json' })
    });
  }

  editSupplier(supplierId: number, supplierData: any): Observable<any> {
    const params = new HttpParams()
      .set('id', supplierId.toString())
      .set('cityId', supplierData.cityId.toString())
      .set('supplierName', supplierData.supplierName)
      .set('adress', supplierData.address)
      .set('phone', supplierData.phone)
      .set('email', supplierData.email);

    return this.http.put<any>(`${this.suppliersApiUrl}/UpdateSupplier`, {}, { params });
  }

  deleteSupplier(supplierId: number): Observable<any> {
    return this.http.delete<any>(`${this.suppliersApiUrl}/DeleteSupplier?Id=${supplierId}`);
  }

  getSuppliersByCity(cityId: number): Observable<any> {
    return this.http.get(`${this.suppliersApiUrl}/GetSuppliersByCity?cityId=${cityId}`);
  }

  getSupplierById(id: number): Observable<Supplier> {
    return this.http.get<Supplier>(`${this.suppliersApiUrl}/GetSuppliersById?id=${id}`);
  }
}