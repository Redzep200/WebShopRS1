import { Injectable } from '@angular/core';
import { HttpClient, HttpHeaders, HttpParams } from '@angular/common/http';
import { Observable } from 'rxjs';

@Injectable({
  providedIn: 'root',
})
export class PromotionService {
    private promotionsApiUrl = 'https://localhost:7023/api/Promotion';
    private promotionProductApiUrl = 'https://localhost:7023/api/PromotionProduct';
    
  constructor(private http: HttpClient) {}

  getAllPromotions(): Observable<any[]> {
    return this.http.get<any[]>(`${this.promotionsApiUrl}/GetAllPromotions`);
  }

  addPromotion(promotionData: any): Observable<any> {
    return this.http.post<any>(`${this.promotionsApiUrl}/AddPromotion`, promotionData, {
      headers: new HttpHeaders({ 'Content-Type': 'application/json' })
    });
  }

  deletePromotion(promotionId: number): Observable<any> {
    return this.http.delete<any>(`${this.promotionsApiUrl}/DeletePromotion?id=${promotionId}`);
  }

  addProductToPromotion(promotionId: number, productId: number): Observable<any> {
    const url = `${this.promotionProductApiUrl}/CreatePromotionProduct`;
    const body = {
        PromotionID: promotionId,
        ProductID: productId
    };
    return this.http.post<any>(url, body);
}
}