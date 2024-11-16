import { Injectable } from '@angular/core';
import { HttpClient, HttpHeaders, HttpParams } from '@angular/common/http';
import { Observable } from 'rxjs';

@Injectable({
  providedIn: 'root',
})
export class CategoryService {
  private categoriesApiUrl = 'https://localhost:7023/api/Category';

  constructor(private http: HttpClient) {}

  getAllCategories(): Observable<any[]> {
    return this.http.get<any[]>(`${this.categoriesApiUrl}/GetAllCategories`);
  }

  addCategory(categoryData: any): Observable<any> {
    return this.http.post<any>(`${this.categoriesApiUrl}/AddCategory`, categoryData, {
      headers: new HttpHeaders({ 'Content-Type': 'application/json' })
    });
  }

  editCategory(categoryId: number, categoryData: any): Observable<any> {
    const params = new HttpParams()
      .set('id', categoryId.toString())
      .set('name', categoryData.name)
      .set('description', categoryData.description);

    return this.http.put<any>(`${this.categoriesApiUrl}/UpdateCategory`, {}, { params });
  }

  deleteCategory(categoryId: number): Observable<any> {
    return this.http.delete<any>(`${this.categoriesApiUrl}/DeleteCategory?id=${categoryId}`);
  }
}