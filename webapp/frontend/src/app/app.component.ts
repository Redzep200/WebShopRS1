import { HttpClient } from '@angular/common/http';
import { Component, OnInit } from '@angular/core';
import { MyConfig } from 'src/app/my-config';
import { ShopService } from './shop/shop.service';
import {TranslateService} from '@ngx-translate/core';

@Component({
  selector: 'app-root',
  templateUrl: './app.component.html',
  styleUrls: ['./app.component.css'],
})
export class AppComponent {
  title = 'frontend';

  constructor(private http: HttpClient, private shopService: ShopService, translate: TranslateService) {translate.setDefaultLang('en');
   translate.use('en');}

  searchQuery: string = '';

  handleSearchQueryChange(newSearchQuery: string) {
    this.shopService.updateSearchQuery(newSearchQuery);
  }

  // products: any[] = [];

  // ngOnInit(): void {
  //   this.http.get(MyConfig.APIurl + '/api/Product/GetAllProducts').subscribe({
  //     next: (response: any) => {
  //       this.products = response;
  //       console.log(this.products);
  //     },
  //     error: (error) => {
  //       console.log(error);
  //     },
  //   });
  // }
}
