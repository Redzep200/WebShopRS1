import { Injectable } from '@angular/core';
import { HttpClient, HttpHeaders, HttpParams } from '@angular/common/http';
import { Observable } from 'rxjs';

@Injectable({
  providedIn: 'root',
})
export class CouponService {
  private couponApiUrl = 'https://localhost:7023/api/Coupon'; 

  constructor(private http: HttpClient) {}

  validateCoupon(couponCode: string): Observable<{valid: boolean, discount?: number, message?: string}> {
    return this.http.post<{valid: boolean, discount?: number, message?: string}>(
      `${this.couponApiUrl}/ValidateCoupon`, 
      JSON.stringify(couponCode),
      { headers: { 'Content-Type': 'application/json' } }
    );
  }

  getCoupons(): Observable<any[]> {
    return this.http.get<any[]>(`${this.couponApiUrl}/GetAllCoupons`);
  }

  createCoupon(couponData: any): Observable<any> {
    return this.http.post<any>(`${this.couponApiUrl}/CreateCoupon`, couponData, {
      headers: new HttpHeaders({ 'Content-Type': 'application/json' })
    });
  }

  deleteCoupon(id: number): Observable<any> {
    return this.http.delete(`${this.couponApiUrl}/DeleteCoupon`, { params: { id: id.toString() } });
  }
  
}