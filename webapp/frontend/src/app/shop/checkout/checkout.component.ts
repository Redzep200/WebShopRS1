import { Component, OnInit } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { CartService } from 'src/app/services/cart.service';
import { AuthService } from 'src/app/services/auth.service';
import { CouponService } from 'src/app/services/coupon.service';

declare var Stripe: any;

@Component({
  selector: 'app-checkout',
  templateUrl: './checkout.component.html',
  styleUrls: ['./checkout.component.css']
})
export class CheckoutComponent implements OnInit {
  cartItems: any[] = [];
  total: number = 0;
  subtotal: number = 0;
  couponCode: string = '';
  couponValid: boolean = false;
  couponDiscount: number = 0;
  private stripe: any;
  discountPercentage: number = 0;
  discountAmount: number = 0;


  constructor(
    private http: HttpClient,
    private cartService: CartService,
    private authService: AuthService,
    private couponService: CouponService
  ) {}

  ngOnInit() {
    this.loadCartItems();
    this.stripe = Stripe('pk_test_51PZv85BLSx3A6psQAoVDhp6WUag5MAHNjwJmtJCg1LkfA45tykteQf3LO8opF2hHV9E46iXtURPCn7v5HzMbK1ku005D9Oo2bZ');
    
  }

  loadCartItems() {
    this.cartService.getCartItems().subscribe(items => {
      this.cartItems = items;
      this.calculateTotals();
      console.log(this.cartItems)
    });
  }

  clearCart() {
    const userId = this.authService.getCurrentUserId();
    if (userId) 
      this.cartService.clearCartItems(userId).subscribe();
  }

  calculateTotals() {
    this.subtotal = this.cartItems.reduce((sum, item) => sum + item.totalItemPrice, 0);
    this.applyDiscount();
    console.log(this.subtotal)
  }

  applyDiscount() {
    this.discountAmount = (this.subtotal * this.discountPercentage) / 100;
    this.total = this.subtotal - this.discountAmount;
  }

  validateCoupon() {
    if (this.couponCode) {
      this.couponService.validateCoupon(this.couponCode).subscribe(
        (response) => {
          if (response.valid) {
            this.discountPercentage = response.discount || 0;
            this.applyDiscount();
            console.log(`Coupon applied: ${this.discountPercentage}% discount`);          
          } else {
            this.discountPercentage = 0;
            console.log('Invalid coupon:', response.message);
          }
        },
        (error) => {
          console.error('Error validating coupon:', error);
          this.discountPercentage = 0;
        }
      );
    }
  }

  proceedToPayment() {
  this.cartService.createCheckoutSession(this.couponCode).subscribe(
    (response: { sessionId: string }) => {   
      this.stripe.redirectToCheckout({ sessionId: response.sessionId });
    }
  );
}


}
