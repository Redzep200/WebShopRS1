import { Component, OnInit, OnDestroy } from '@angular/core';
import { FormBuilder } from '@angular/forms';
import { Subject } from 'rxjs';
import { debounceTime, distinctUntilChanged, takeUntil } from 'rxjs/operators';
import { ShopService } from './shop.service';
import { ProductService } from '../services/product.service';
import { FormGroup } from '@angular/forms';
import { ManufacturerService } from '../services/manufacturer.service';
import { TranslateService } from '@ngx-translate/core';

@Component({
  selector: 'app-shop',
  templateUrl: './shop.component.html',
  styleUrls: ['./shop.component.css']
})
export class ShopComponent implements OnInit, OnDestroy {
  products: any[] = [];
  categories: any[] = [];
  manufacturers:any[] = [];
  filterForm: FormGroup;

  minPrice: number = 0;
  maxPrice: number = 200;
  priceOptions: any = {
    floor: 0,
    ceil: 200,
    step: 1,
    
  };
  
  private unsubscribe$ = new Subject<void>();

  constructor(
    private shopService: ShopService,
    private formBuilder: FormBuilder,
    private productService: ProductService,
    private manufacturerService: ManufacturerService,
    private translate: TranslateService
  ) {
    this.filterForm = this.formBuilder.group({
      productName: [''],
      categoryId: [null],
      priceRange: [100],
      minPrice: [0],
      maxPrice: [500],
      manufacturerId: [null]
    });
    translate.setDefaultLang('en');
    translate.use('en');
  }

  ngOnInit() {
    this.loadCategories();
    this.loadManufacturers();
    this.loadAllProducts();
    this.initializeFilterForm();
    this.applyFilters();

    this.filterForm.patchValue({
      minPrice: this.minPrice,
      maxPrice: this.maxPrice
    });
  }

  ngOnDestroy() {
    this.unsubscribe$.next();
    this.unsubscribe$.complete();
  }

  private loadCategories() {
    this.shopService.getAllCategories().subscribe({
      next: (categories: any[]) => this.categories = categories,
      error: (error) => console.error('Error fetching categories:', error)
    });
  }

  private loadManufacturers(){
    this.manufacturerService.getAllManufacturers().subscribe({
      next: (manufacturers: any[]) => this.manufacturers = manufacturers,
      error: (error) => console.error('Error fetching manufacturers:', error)
    })
  }

  private loadAllProducts(){
    this.productService.getAllProducts().subscribe({
      next: (products: any[]) => this.products = products,
      error: (error) => console.error('Error fetching manufacturers:', error)
    })
  }

  private initializeFilterForm() {
    this.filterForm.valueChanges.pipe(
      debounceTime(300),
      distinctUntilChanged(),
      takeUntil(this.unsubscribe$)
    ).subscribe(() => this.applyFilters());
  }

  private applyFilters() {
    const filters = this.filterForm.value;
    this.productService.getFilteredProducts(
      filters.productName,
      filters.categoryId,
      filters.minPrice,
      filters.maxPrice,
      filters.manufacturerId
    ).subscribe({
      next: (products: any[]) => this.products = products,
      error: (error) => console.error('Error fetching products:', error)
    });
  }

  resetFilters() {
    this.filterForm.reset({
      productName: '',
      categoryId: null,
      priceRange: 200,
      minPrice: 0,
      maxPrice: 200,
      manufacturerId:null
    });
    this.loadAllProducts();
  }

  onPriceRangeChange(event: any) {
    const value = event.target.value;
    this.filterForm.patchValue({
      minPrice: 0,
      maxPrice: value
    });
  }

  onPriceInputChange(type: string, event: any) {
    const value = event.target.value;
    if (type === 'minPrice') {
      this.minPrice = value;
    } else if (type === 'maxPrice') {
      this.maxPrice = value;
    }
    this.filterForm.patchValue({
      minPrice: this.minPrice,
      maxPrice: this.maxPrice
    });
  }

  onSliderChange(value: number, isMaxPrice: boolean = false) {
    if (isMaxPrice) {
      this.filterForm.patchValue({ maxPrice: value });
    } else {
      this.filterForm.patchValue({ minPrice: value });
    }
  }

}
