import { Injectable } from '@angular/core';
import { HttpClient, HttpHeaders, HttpParams } from '@angular/common/http';
import { Observable , BehaviorSubject } from 'rxjs';
import { Product } from '../shared/models/product.model';

@Injectable({
  providedIn: 'root',
})
export class ProductService {
  private productsApiUrl = 'https://localhost:7023/api/Product';
  private productsImagesApiUrl = 'https://localhost:7023/api/ProductImage';
  public searchQuerySubject: BehaviorSubject<string> = new BehaviorSubject<string>('');
  public searchQuery: Observable<string> = this.searchQuerySubject.asObservable();

  constructor(private http: HttpClient) {}

  updateSearchQuery(searchQuery: string) {
    this.searchQuerySubject.next(searchQuery);
  }

  getAllProducts(): Observable<any[]> {
    return this.http.get<any[]>(`${this.productsApiUrl}/GetAllProductItems`);
  }
  getAllProductsNI(): Observable<any[]> {
    return this.http.get<any[]>(`${this.productsApiUrl}/GetAllProducts`);
  }

  getProductById(id: number): Observable<Product> {
    return this.http.get<Product>(
      `${this.productsApiUrl}/GetProductImagesByProductId`,
      {
        params: { productId: id.toString() },
      }
    );
  }

  getProducts(searchQuery: string): Observable<any[]> {
    const params = { name: searchQuery };
    return this.http.get<any[]>(`${this.productsApiUrl}/GetProductByName`, { params });
  }


  createProduct(productData: any): Observable<any> {
    return this.http.post<any>(`${this.productsApiUrl}/CreateProduct`, productData, {
      headers: new HttpHeaders({ 'Content-Type': 'application/json' })
    });
  }

  updateProduct(product: any): Observable<any> {
    const url = `${this.productsApiUrl}/UpdateProduct?id=${product.id}&categoryId=${product.categoryId}&manufacturerId=${product.manufacturerId}&name=${product.name}&description=${product.description}&price=${product.price}`;
    return this.http.put(url, {});
  }

  deleteProduct(productId: number): Observable<any> {
    return this.http.delete<any>(`${this.productsApiUrl}/DeleteProductItem?id=${productId}`);
  }

  getProductsByCategory(categoryId: number): Observable<any[]> {
    return this.http.get<any[]>(`${this.productsApiUrl}/GetProductByCategory`, {
      params: { id: categoryId },
    });
  }

  getProductsByPriceRange(minPrice?: number, maxPrice?: number): Observable<any> {
    const params = {
      minPrice: minPrice?.toString() ?? '1',
      maxPrice: maxPrice?.toString() ?? '1002',
    };
    return this.http.get(`${this.productsApiUrl}/GetProductItemsByPriceRange`, { params });
  }

  getFilteredProducts(productName: string, categoryId?: number, minPrice?: number, maxPrice?: number, manufacturerId?: number) {
    let url = 'https://localhost:7023/api/Product/GetFilteredProductItems';
    
    let params = new HttpParams().set('productName', productName);
    if (categoryId) params = params.set('categoryId', categoryId.toString());
    if (minPrice !== undefined) params = params.set('minPrice', minPrice.toString());
    if (maxPrice !== undefined) params = params.set('maxPrice', maxPrice.toString());
    if (manufacturerId) params = params.set('manufacturerId', manufacturerId.toString());
  
    return this.http.get<any[]>(url, { params });
  }
}
