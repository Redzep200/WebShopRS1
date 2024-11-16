import { NgModule } from '@angular/core';
import { FormsModule, ReactiveFormsModule } from '@angular/forms';
import { CommonModule } from '@angular/common';
import { BrowserModule } from '@angular/platform-browser';
import { RouterModule, Routes } from '@angular/router';
import { DatePipe } from '@angular/common';
import { AppComponent } from './app.component';
import { CitiesComponent } from './components/cities/cities/cities.component';
import { HttpClientModule } from '@angular/common/http';
import { CountriesComponentComponent } from './components/countries/countries-component/countries-component.component';
import { LoginComponent } from './components/login/login.component';
import { RegisterComponent } from './components/register/register.component';
import { NavBarComponent } from './core/nav-bar/nav-bar.component';
import { CoreModule } from './core/core.module';
import { ShopModule } from './shop/shop.module';
import { ShopComponent } from './shop/shop.component';
import { BrowserAnimationsModule } from '@angular/platform-browser/animations';
import { MatDialogModule } from '@angular/material/dialog';
import { MatButtonModule } from '@angular/material/button';
import { LoginModalComponent } from './login-modal/login-modal.component';
import { AdminPanelComponent } from './admin-panel/admin-panel.component';
import { AdminGuard } from './guards/admin.guard';
import { SupportGuard } from './guards/support.guard';
import { CustomerGuard } from './guards/customer-guard';
import { ShopService } from './shop/shop.service';
import { CityEditDialogComponent } from './admin-panel/city-edit-dialog/city-edit-dialog.component';
import { CountryEditDialogComponent } from './admin-panel/country-edit-dialog/country-edit-dialog.component';
import { SupplierEditDialogComponent } from './admin-panel/supplier-edit-dialog/supplier-edit-dialog.component';
import { CategoryEditDialogComponent } from './admin-panel/category-edit-dialog/category-edit-dialog.component';
import { ProductDetailsComponent } from './shop/product-details/product-details.component';
import { CartComponent } from './shop/cart/cart.component';
import { ManufacturerEditDialogComponent } from './admin-panel/manufacturer-edit-dialog/manufacturer-edit-dialog.component';
import { SupportMessagesComponent } from './CustomerSupport/support-messages/support-messages.component';
import { CustomerMessagesComponent } from './CustomerSupport/customer-messages/customer-messages/customer-messages.component';
import { PaymentSuccessComponent } from './shop/payment-success/payment-success.component';
import { CheckoutComponent } from './shop/checkout/checkout.component';
import { OrdersComponent } from './orders/orders.component';
import { DashboardComponent } from './admin-panel/dashboard/dashboard.component';
import { SistemAdminPanelComponent } from './sistem.admin-panel/sistem.admin-panel/sistem.admin-panel.component';
import { SistemAdminGuard } from './guards/sistemAdmin.guard';
import { EditUserDialogComponent } from './sistem.admin-panel/edit.user-dialog/edit.user-dialog/edit.user-dialog.component';
import { TranslateModule, TranslateLoader } from '@ngx-translate/core';
import { TranslateHttpLoader } from '@ngx-translate/http-loader';
import { HttpClient } from '@angular/common/http';
import { CouponComponent } from './coupons/coupons.component';
import { EditProductPopupComponent } from './admin-panel/edit-product-dialog/edit-product-dialog.component';
import { CUSTOM_ELEMENTS_SCHEMA } from '@angular/core';
import { HTTP_INTERCEPTORS } from '@angular/common/http';
import { AuthInterceptor } from './http-interceptors/auth-interceptor';
import { VerifyEmailComponent } from './components/register/verify-email/verify-email.component';
import { StoreComponent } from './sistem.admin-panel/store/store.component';
import { GoogleMapsModule } from '@angular/google-maps';
import { VisitUsComponent } from './visit.us/visit.us.component';

export function HttpLoaderFactory(http: HttpClient) {
  return new TranslateHttpLoader(http, './assets/i18n/', '.json');
}

const routes: Routes = [
  { path: 'shop', component: ShopComponent },
  { path: 'cities', component: CitiesComponent },
  { path: 'countries', component: CountriesComponentComponent },
  { path: 'login', component: LoginComponent },
  { path: 'register', component: RegisterComponent },
  {
    path: 'admin-panel',
    component: AdminPanelComponent,
    canActivate: [AdminGuard],
  },
  { path: 'product/:id', component: ProductDetailsComponent },
  {
    path: 'support-messages',
    component: SupportMessagesComponent,
    canActivate: [SupportGuard], 
  },
  {
    path: 'customer-messages',
    component: CustomerMessagesComponent,
    canActivate: [CustomerGuard], 
  },
  {
    path: 'sistem.admin-panel',
    component: SistemAdminPanelComponent,
    canActivate: [SistemAdminGuard], 
  },
  { path: '', redirectTo:'shop', pathMatch:'full'},
  { path: 'payment-success', component: PaymentSuccessComponent },
  { path: 'checkout', component: CheckoutComponent},
  { path:'orders', component:OrdersComponent},
  { path:'dashboard', component:DashboardComponent},
  { path: 'verify-email', component: VerifyEmailComponent },
  { path: 'visit-us', component: VisitUsComponent },
  {path: 'store', component: StoreComponent},
  {path:'coupons', component:CouponComponent}
];
@NgModule({
  declarations: [
    AppComponent,
    CitiesComponent,
    CountriesComponentComponent,
    LoginComponent,
    RegisterComponent,
    LoginModalComponent,
    AdminPanelComponent,
    CityEditDialogComponent,
    CountryEditDialogComponent,
    SupplierEditDialogComponent,
    CategoryEditDialogComponent,
    CartComponent,
    ManufacturerEditDialogComponent,
    SupportMessagesComponent,
    CustomerMessagesComponent,
    OrdersComponent,
    DashboardComponent,
    SistemAdminPanelComponent,
    EditUserDialogComponent,
    PaymentSuccessComponent,
    OrdersComponent,
    CouponComponent,
    EditProductPopupComponent,
    VerifyEmailComponent,
    StoreComponent,
    VisitUsComponent
  ],
  imports: [
    BrowserModule,
    RouterModule.forRoot(routes),
    FormsModule,
    HttpClientModule,
    CommonModule,
    ReactiveFormsModule,
    CoreModule,
    ShopModule,
    BrowserAnimationsModule,
    MatDialogModule,
    MatButtonModule,
    GoogleMapsModule,
    TranslateModule,
    TranslateModule.forRoot({
      loader: {
        provide: TranslateLoader,
        useFactory: HttpLoaderFactory,
        deps: [HttpClient]
      }
    })
  ],
  exports: [[RouterModule]],
  providers: [DatePipe, ShopService, { provide: HTTP_INTERCEPTORS, useClass: AuthInterceptor, multi: true }],
  bootstrap: [AppComponent],
  entryComponents: [CityEditDialogComponent],
  schemas:[CUSTOM_ELEMENTS_SCHEMA]
})
export class AppModule {}
