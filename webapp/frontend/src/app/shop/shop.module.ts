import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { ShopComponent } from './shop.component';
import { ProductItemComponent } from './product-item/product-item.component';
import { ShopService } from './shop.service';
import { ProductCategoryComponent } from './product-category/product-category.component';
import { ProductDetailsComponent } from './product-details/product-details.component';
import { FormsModule } from '@angular/forms';
import { NgxSliderModule } from '@angular-slider/ngx-slider';
import { CheckoutComponent } from './checkout/checkout.component';
import { MatIconModule } from '@angular/material/icon';
import { ReactiveFormsModule } from '@angular/forms';
import { TranslateModule } from '@ngx-translate/core';

@NgModule({
  declarations: [
    ShopComponent,
    ProductItemComponent,
    ProductCategoryComponent,
    ProductDetailsComponent,
    CheckoutComponent,
  ],
  providers: [ShopService],
  imports: [CommonModule, FormsModule, NgxSliderModule, MatIconModule, ReactiveFormsModule, TranslateModule],
  exports: [ShopComponent],
})
export class ShopModule {}
