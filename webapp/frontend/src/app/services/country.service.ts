import { Injectable } from '@angular/core';
import { HttpClient, HttpHeaders, HttpParams } from '@angular/common/http';
import { Observable } from 'rxjs';
import { catchError, map } from 'rxjs/operators';
import { Country } from '../shared/models/country.model';

@Injectable({
  providedIn: 'root',
})
export class CountryService {
  private countriesApiUrl = 'https://localhost:7023/api/Country';

  constructor(private http: HttpClient) {}

  getAllCountries(): Observable<any[]> {
    return this.http.get<any[]>(`${this.countriesApiUrl}/GetAllCountries`);
  }

  addCountry(countryData: any): Observable<any> {
    return this.http.post<any>(`${this.countriesApiUrl}/AddCountry`, countryData, {
      headers: new HttpHeaders({ 'Content-Type': 'application/json' })
    });
  }

  editCountry(countryId: number, countryData: any): Observable<any> {
    const params = new HttpParams()
      .set('id', countryId.toString())
      .set('name', countryData.name);

    return this.http.put<any>(`${this.countriesApiUrl}/UpdateCountry`, {}, { params });
  }

  deleteCountry(countryId: number): Observable<any> {
    return this.http.delete<any>(`${this.countriesApiUrl}/DeleteCountry?Id=${countryId}`);
  }
  
  getCountryByName(name: string): Observable<any> {
    return this.http.get<any>(`${this.countriesApiUrl}/GetCountryByName?name=${name}`);
  }

  getOrCreateCountryByName(name: string): Observable<Country> {
    return this.http.get<Country>(`${this.countriesApiUrl}/GetOrCreateCountryByName?name=${name}`).pipe(
      catchError(() => {
        return this.addCountry({ name: name });
      }),
      map(country => country as Country)
    );
  }
}