import { Component, Input, OnInit } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Router } from '@angular/router';
import { Product } from 'src/app/shared/models/product.model';

@Component({
  selector: 'app-product-item',
  templateUrl: './product-item.component.html',
  styleUrls: ['./product-item.component.css'],
})
export class ProductItemComponent {
  constructor(private http: HttpClient, private router: Router) {}

  @Input()
  product!: Product;

  openProductDetails() {
    this.router.navigate(['/product', this.product.productId]);
  }

  ngOnInit(): void {}
}
