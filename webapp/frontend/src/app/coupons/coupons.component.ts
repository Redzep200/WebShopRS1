import { Component, OnInit } from '@angular/core';
import { CouponService } from '../services/coupon.service';
import { FormsModule } from '@angular/forms';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';


@Component({
  selector: 'app-coupons',
  templateUrl: './coupons.component.html',
  styleUrls: ['./coupons.component.css']
})
export class CouponComponent implements OnInit {
  coupons: any[] = [];
  newCoupon = { discountPerc: 0, value: '', expDate: '' };

  addCouponForm!: FormGroup;

  constructor(private couponService: CouponService,private fb: FormBuilder) { }

  ngOnInit(): void {
    this.addCouponForm = this.fb.group({
      percentDiscount: ['', Validators.required],
      value: ['', Validators.required],
      expirationDate: ['', Validators.required]
    });
    this.loadCoupons();
}

  loadCoupons(): void {
    this.couponService.getCoupons().subscribe(
      (data) => this.coupons = data,
      (error) => console.error('Error loading coupons', error)
    );
  }
  

  addCoupon(): void {
    if (this.addCouponForm.valid) {
      const formValues = this.addCouponForm.value;   
  
      const formattedDate = new Date(formValues.expDate)
        .toLocaleDateString('en-US', { year: 'numeric', month: '2-digit', day: '2-digit' })
        .replace(/\//g, '-');
  
      const couponToAdd = {
        ...formValues,
        expDate: formattedDate
      };
  
      this.couponService.createCoupon(couponToAdd).subscribe(
        () => {
          this.loadCoupons();
          this.addCouponForm.reset();
        },
        (error) => console.error('Error adding coupon', error)
      );
    } else {
      this.addCouponForm.markAllAsTouched();
    }
  }

  onSubmitCoupon(): void {
    if (this.addCouponForm.valid) {
      this.addCoupon(); 
    } else {
      this.addCouponForm.markAllAsTouched(); 
    }
  }
  

  deleteCoupon(id: number): void {
    this.couponService.deleteCoupon(id).subscribe(
      () => this.loadCoupons(),
      (error) => console.error('Error deleting coupon', error)
    );
  }
}
