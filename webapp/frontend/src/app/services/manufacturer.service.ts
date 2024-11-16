import { Injectable } from '@angular/core';
import { HttpClient, HttpHeaders, HttpParams } from '@angular/common/http';
import { Observable } from 'rxjs';

@Injectable({
  providedIn: 'root',
})
export class ManufacturerService {
  private manufacturersApiUrl = 'https://localhost:7023/api/Manufacturer';

  constructor(private http: HttpClient) {}

  getAllManufacturers(): Observable<any[]> {
    return this.http.get<any[]>(`${this.manufacturersApiUrl}/GetAllManufacturers`);
  }
  addManufacturer(manufacturerData: any): Observable<any> {
    return this.http.post<any>(`${this.manufacturersApiUrl}/AddManufacturer`, manufacturerData, {
      headers: new HttpHeaders({ 'Content-Type': 'application/json' })
    });
  }

  editManufacturer(manufacturerId: number, manufacturerData: any): Observable<any> {
    const params = new HttpParams()
      .set('id', manufacturerId.toString())
      .set('name', manufacturerData.name)
      .set('address', manufacturerData.address)
      .set('email', manufacturerData.email);

    return this.http.put<any>(`${this.manufacturersApiUrl}/UpdateManufacturer`, {}, { params });
  }

  deleteManufacturer(manufacturerId: number): Observable<any> {
    return this.http.delete<any>(`${this.manufacturersApiUrl}/DeleteManufacturer?Id=${manufacturerId}`);
  }
}