import { Component, OnInit, Inject, Input } from '@angular/core';
import { MAT_DIALOG_DATA } from '@angular/material/dialog';
import { CartService } from 'src/app/services/cart.service';
import { AuthService } from 'src/app/services/auth.service';
import { Router } from '@angular/router';

declare var Stripe: any;

@Component({
  selector: 'app-cart',
  templateUrl: './cart.component.html',
  styleUrls: ['./cart.component.css'],
})
export class CartComponent implements OnInit {
  cartItems: any[] = [];
  userEmail: string | null = null;
  @Input()
  position!: DOMRect;
  private stripe: any;

  constructor(private cartService: CartService, private authService: AuthService, private router: Router) {}

  ngOnInit() {
    this.loadCartItems();
    this.stripe = Stripe('pk_test_51PZv85BLSx3A6psQAoVDhp6WUag5MAHNjwJmtJCg1LkfA45tykteQf3LO8opF2hHV9E46iXtURPCn7v5HzMbK1ku005D9Oo2bZ');
  }

  loadCartItems() {
    this.cartService.cartItems$.subscribe((items) => {
      this.cartItems = items;
    });
  }

  deleteItem(id: number) {
    this.cartService.deleteCartItem(id).subscribe();
  }

  clearCart() {
    const userId = this.authService.getCurrentUserId();
    if (userId) 
      this.cartService.clearCartItems(userId).subscribe();
  }

  goToCheckout() {
    this.router.navigate(['/checkout']);
  }

}
