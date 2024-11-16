import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';

export interface ReportData {
  productNumber: number;
  userNumber: number;
  categoryNumber: number;
  mostRecentUser: string;
  categoryWithMostProducts: string;
  productCountInTopCategory: number;
  bestRatedProductName: string;
  bestRatedProductRating: number;
  mostRecentCommentUser: string;
  mostRecentCommentProduct: string;
  mostRecentCommentDate: Date;
  mostRecentCommentRating: number;
  mostRecentCommentContent: string;
}

@Injectable({
  providedIn: 'root'
})
export class ReportService {
  private apiUrl = 'https://localhost:7023/api/Report/GetDashboardData/dashboard-data'; // Update with your actual API URL

  constructor(private http: HttpClient) {}

  getReportData(): Observable<ReportData> {
    return this.http.get<ReportData>(this.apiUrl);
  }
}
