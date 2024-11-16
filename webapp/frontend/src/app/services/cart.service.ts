import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { BehaviorSubject, Observable, of } from 'rxjs';
import { AuthService } from './auth.service';
import { switchMap, catchError, map, tap } from 'rxjs/operators';
import { Router } from '@angular/router';

@Injectable({
  providedIn: 'root',
})
export class CartService {
  private baseUrl = 'https://localhost:7023/api';
  private cartItemsSubject = new BehaviorSubject<any[]>([]);
  cartItems$ = this.cartItemsSubject.asObservable();

  constructor(private http: HttpClient, private authService: AuthService, private router: Router) {
    this.authService.authStatusChanged$.subscribe((isLoggedIn) => {
      if (isLoggedIn) {
        this.loadCartItems();
      } else {
        this.clearCart();
      }
    });
  }
  addToCart(productId: number, quantity: number): Observable<any> {
    return this.getCartId().pipe(
      switchMap((shoppingCartId) => {
        const url = `${this.baseUrl}/ShoppingCartItem/CreateNewShoppingCartItem`;
        const payload = {
          productId: productId,
          shoppingCartId: shoppingCartId,
          quantity: quantity
        };
        return this.http.post(url, payload);
      }),
      switchMap(async () => this.loadCartItems())
    );
  }

  createCheckoutSession(couponCode: string | null = null): Observable<{ sessionId: string }> {
    return this.cartItems$.pipe(
      switchMap((cartItems) => {
        const url = `${this.baseUrl}/Stripe/create-checkout-session`;
        return this.http.post<{ sessionId: string }>(url, {
          cartItems: cartItems,
          couponCode: couponCode
        });
      })
    );
  }

  handlePaymentSuccess(sessionId: string) {
    this.router.navigate(['/payment-success'], { queryParams: { session_id: sessionId } });
  }

  deleteCartItem(id: number): Observable<any> {
    return this.http.delete<boolean>(`${this.baseUrl}/ShoppingCartItem/DeleteShopCartItem?id=${id}`)
      .pipe(
        switchMap(success => {
          if (success) {
            return this.getCartItems();
          } else {
            return of(false);
          }
        }),
        catchError(error => {
          console.error('Error deleting item:', error);
          return of(false);
        })
      );
  }

  clearCartItems(userId: string): Observable<any> {
    return this.http.delete<boolean>(`${this.baseUrl}/ShoppingCartItem/DeleteAllShoppingCartItems?userId=${userId}`)
      .pipe(
        switchMap(success => {
          if (success) {
            return this.getCartItems();
          } else {
            return of(false);
          }
        }),
        catchError(error => {
          console.error('Error clearing cart:', error);
          return of(false);
        })
      );
  }

  getCartId(): Observable<string> {
    const userId = this.authService.getCurrentUserId();
    const url = `${this.baseUrl}/ShoppingCart/GetShoppingCartByUserId?id=${userId}`;

    return this.http.get<any>(url).pipe(map((response) => response.id));
  }

  getCartItems(): Observable<any[]> {
    return this.http
      .get<any[]>(`${this.baseUrl}/ShoppingCartItem/GetAllShoppingCartItems`)
      .pipe(
        map((items) => this.filterItemsByCurrentUser(items)),
        tap((filteredItems) => this.cartItemsSubject.next(filteredItems))
      );
  }

  private filterItemsByCurrentUser(items: any[]): any[] {
    const currentUserEmail = this.authService.getCurrentUserEmail();
    return items.filter(
      (item) => item.shoppingCart.user.email === currentUserEmail
    );
  }

  loadCartItems(): void {
    this.getCartItems().subscribe(
      () => {},
      (error) => console.error('Error loading cart items:', error)
    );
  }

  clearCart(): void {
    this.cartItemsSubject.next([]);
  }
}
