import { Injectable } from '@angular/core';
import { HttpClient, HttpParams, HttpHeaders } from '@angular/common/http';
import { Observable } from 'rxjs';
import { City } from '../shared/models/city.model';

@Injectable({
  providedIn: 'root',
})
export class CityService {
  private citiesApiUrl = 'https://localhost:7023/api/City';

  constructor(private http: HttpClient) {}

  getAllCities(): Observable<City[]> {
    return this.http.get<City[]>(`${this.citiesApiUrl}/GetAllCities`);
  }

  addCity(cityData: City): Observable<{ success: boolean; message: string; city: City }> {
    return this.http.post<{ success: boolean; message: string; city: City }>(`${this.citiesApiUrl}/CreateCity`, cityData);
  }

  editCity(cityId: number, cityData: any): Observable<any> {
    const headers = new HttpHeaders().set('Content-Type', 'application/json');
    const params = new HttpParams()
      .set('id', cityId.toString())
      .set('countryid', cityData.countryId)
      .set('name', cityData.name)
      .set('zipcode', cityData.zipCode);

    return this.http.put<any>(`${this.citiesApiUrl}/UpdateCity`, cityData, { headers, params });
  }

  deleteCity(cityId: number): Observable<any> {
    return this.http.delete<any>(`${this.citiesApiUrl}/DeleteCity?Id=${cityId}`);
  }
}