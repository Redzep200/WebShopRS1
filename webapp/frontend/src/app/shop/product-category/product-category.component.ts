import { Component, Input, OnInit } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { MyConfig } from 'src/app/my-config';
@Component({
  selector: 'app-product-category',
  templateUrl: './product-category.component.html',
  styleUrls: ['./product-category.component.css'],
})
export class ProductCategoryComponent {
  @Input() categories: any;
  selected: boolean = false;

  constructor(private http: HttpClient) {}

  handleItemClick() {
    this.selected = !this.selected; // Toggle the selected state
    // Add any other click event handling logic here if needed
  }
}
