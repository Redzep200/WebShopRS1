import { Component, OnInit } from '@angular/core';
import { MatDialog } from '@angular/material/dialog';
import { ProductService } from '../services/product.service';
import { CategoryService } from '../services/category.service';
import { CityService } from '../services/city.service';
import { CountryService } from '../services/country.service';
import { SupplierService } from '../services/supplier.service';
import { ManufacturerService } from '../services/manufacturer.service';
import { CityEditDialogComponent } from './city-edit-dialog/city-edit-dialog.component';
import { CountryEditDialogComponent } from './country-edit-dialog/country-edit-dialog.component';
import { SupplierEditDialogComponent } from './supplier-edit-dialog/supplier-edit-dialog.component';
import { CategoryEditDialogComponent } from './category-edit-dialog/category-edit-dialog.component';
import { ManufacturerEditDialogComponent } from './manufacturer-edit-dialog/manufacturer-edit-dialog.component';
import { HttpClient } from '@angular/common/http';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { AbstractControl, ValidationErrors, ValidatorFn } from '@angular/forms';
import { PromotionService } from '../services/promotion.service';


export function textValidator(): ValidatorFn {
  return (control: AbstractControl): ValidationErrors | null => {
    const valid = /^[A-Za-z\s]+$/.test(control.value);
    return valid ? null : { invalidName: true };
  };
}

@Component({
  selector: 'app-admin-panel',
  templateUrl: './admin-panel.component.html',
  styleUrls: ['./admin-panel.component.css'],
})
export class AdminPanelComponent implements OnInit {
  cities: any[] = [];
  newCity: any = { countryId: null };
  selectedCityForEdit: any;

  countries: any[] = [];
  newCountry: any = {};

  suppliers: any[] = [];
  newSupplier: any = {};

  categories: any[] = [];
  newCategory: any = {};

  products: any[] = [];
  newProduct: any = {};
  editingProduct: any = null;

  manufacturers: any[] = [];
  newManufacturer: any = {};

  promotions: any[] = [];
  newPromotion: any = {}

  selectedPromotionId: number | null = null;
  dropdownOpen: number | null = null;

  currentSection: string = 'products';

  addProductForm! :FormGroup;
  addCategoryForm!: FormGroup;
  addCityForm!: FormGroup;
  addCountryForm!: FormGroup;
  addSupplierForm!: FormGroup;
  addManufacturerForm!: FormGroup;
  addPromotionForm!: FormGroup;

  showSection(section: string) {
    this.currentSection = section;
  }

  
  constructor(private productService: ProductService,
    private promotionService: PromotionService,
    private categoryService: CategoryService,
    private cityService: CityService,
    private countryService: CountryService,
    private supplierService: SupplierService,
    private manufacturerService: ManufacturerService,
    private dialog: MatDialog,
    private httpclient:HttpClient,
    private fb: FormBuilder) {}

  

  ngOnInit(): void {
    this.addProductForm = this.fb.group({
      name: ['', Validators.required],
      description: ['', [Validators.required, textValidator()]],
      price: ['', Validators.required],
      categoryId: ['', Validators.required],
      manufacturerId: ['', Validators.required]     
    });
    this.addCategoryForm = this.fb.group({
      name: ['', [Validators.required, textValidator()]],
      description: ['', [Validators.required, textValidator()]]
    });
    this.addCityForm = this.fb.group({
      name: ['', [Validators.required, textValidator()]],
      zipCode: ['', Validators.required],
      countryId: ['', Validators.required]
    });
    this.addCountryForm = this.fb.group({
      name: ['', [Validators.required, textValidator()]]
    });
    this.addSupplierForm = this.fb.group({
      name: ['', [Validators.required, textValidator()]],
      adress: ['', Validators.required],
      contactPhone: ['', Validators.required],
      email: ['', Validators.required],
      cityId: ['', Validators.required]
    });
    this.addManufacturerForm = this.fb.group({
      name: ['', [Validators.required, textValidator()]],
      address: ['', Validators.required],
      email: ['', Validators.required]
    });
    this.addPromotionForm = this.fb.group({
      name: ['', [Validators.required, textValidator()]],
      description: ['', [Validators.required, textValidator()]],
      startDate:['', Validators.required],
      endDate:['', Validators.required],
      discountPercentage:['', Validators.required]
    })
    this.getCategories();
    this.getCities();
    this.getCountries();
    this.getProducts();
    this.getSuppliers();
    this.getManufacturers();
    this.getPromotions();
  }

  toggleDropdown(productId: number): void {
    this.dropdownOpen = this.dropdownOpen === productId ? null : productId;
  }

    getPromotions() {
      this.promotionService.getAllPromotions().subscribe((data) => {
        this.promotions = data;
      })
  }

  addPromotion(): void {
    if (this.addPromotionForm.valid) {
      this.promotionService.addPromotion(this.addPromotionForm.value).subscribe(() => {
        this.getPromotions();
        this.addPromotionForm.reset();
      });
    }
  }

  addToPromotion(productId: number): void {
    if (this.selectedPromotionId) {
      this.promotionService.addProductToPromotion(this.selectedPromotionId, productId).subscribe(
        (response) => {
          console.log('Product added to promotion:', response);
          this.selectedPromotionId = null;
          this.getProducts();
        },
        (error) => {
          console.error('Error adding product to promotion:', error);
        }
      );
    }
  }

  onSubmitPromotion(){
    if (this.addPromotionForm.valid) {
      this.addPromotion();
    } else {
      Object.keys(this.addPromotionForm.controls).forEach(field => {
        const control = this.addPromotionForm.get(field);
        control?.markAsTouched({ onlySelf: true });
      });
    }
  }

  

  // Cities
  getCities() {
    this.cityService.getAllCities().subscribe((data) => {
      this.cities = data;
    });
  }

  editCity(city: any) {
    if (this.newCity.countryId !== 0) {
      const dialogRef = this.dialog.open(CityEditDialogComponent, {
        width: '500px',
        data: { city, countries: this.countries },
      });
  
      dialogRef.afterClosed().subscribe((result: any) => {
        if (result) {
          this.cityService.editCity(city.id, result).subscribe(() => {
            this.getCities();
          });
        }
      });
    } else {
      console.error('Please select a valid country before editing.');
    }
  }

  removeCity(cityId: number) {
    this.cityService.deleteCity(cityId).subscribe(() => {
      this.getCities();
    });
  }

  addCity(): void {
    if (this.addCityForm.valid) {
      this.cityService.addCity(this.addCityForm.value).subscribe(() => {
        this.getCities();
        this.addCityForm.reset();
      });
    }
  }

  onSubmitCity(): void {
    if (this.addCityForm.valid) {
      this.addCity();
    } else {
      Object.keys(this.addCityForm.controls).forEach(field => {
        const control = this.addCityForm.get(field);
        control?.markAsTouched({ onlySelf: true });
      });
    }
  }
  // Countries
  getCountries() {
    this.countryService.getAllCountries().subscribe((data) => {
      this.countries = data;
    });
  }
  editCountry(country: any) {
    const dialogRef = this.dialog.open(CountryEditDialogComponent, {
      width: '500px',
      data: { country }
    });
  
    dialogRef.afterClosed().subscribe(result => {
      if (result) {
        this.countryService.editCountry(result.id, result).subscribe(() => {
          this.getCountries();
        });
      }
    });
  }

  removeCountry(countryId: number) {
    this.countryService.deleteCountry(countryId).subscribe(() => {
      this.getCountries();
    });
  }

  addCountry(): void {
    if (this.addCountryForm.valid) {
      this.countryService.addCountry(this.addCountryForm.value).subscribe(() => {
        this.getCountries();
        this.addCountryForm.reset();
      });
    }
  }

  onSubmitCountry(): void {
    if (this.addCountryForm.valid) {
      this.addCountry();
    } else {
      Object.keys(this.addCountryForm.controls).forEach(field => {
        const control = this.addCountryForm.get(field);
        control?.markAsTouched({ onlySelf: true });
      });
    }
  }

  // Suppliers
  getSuppliers() {
    this.supplierService.getAllSuppliers().subscribe((data) => {
      this.suppliers = data;
    });
  }

  editSupplier(supplier: any) {
    const dialogRef = this.dialog.open(SupplierEditDialogComponent, {
      width: '500px',
      data: { supplier, cities: this.cities },
    });
  
    dialogRef.afterClosed().subscribe((result: any) => {
      if (result) {
        this.supplierService.editSupplier(supplier.id, result).subscribe(() => {
          this.getSuppliers();
        });
      }
    });
  }

  removeSupplier(supplierId: number) {
    this.supplierService.deleteSupplier(supplierId).subscribe(() => {
      this.getSuppliers();
    });
  }


  addSupplier(): void {
    if (this.addSupplierForm.valid) {
      this.supplierService.addSupplier(this.addSupplierForm.value).subscribe(() => {
        this.getSuppliers();
        this.addSupplierForm.reset();
      });
    }
  }

  onSubmitSupplier(): void {
    if (this.addSupplierForm.valid) {
      this.addSupplier();
    } else {
      Object.keys(this.addSupplierForm.controls).forEach(field => {
        const control = this.addSupplierForm.get(field);
        control?.markAsTouched({ onlySelf: true });
      });
    }
  }

  // Manufacturers

  getManufacturers() {
    this.manufacturerService.getAllManufacturers().subscribe((data) => {
      this.manufacturers = data;
    });
  }

  editManufacturer(manufacturer: any) {
    const dialogRef = this.dialog.open(ManufacturerEditDialogComponent, {
      width: '500px',
      data: { manufacturer, cities: this.cities },
    });
  
    dialogRef.afterClosed().subscribe((result: any) => {
      if (result) {
        this.manufacturerService.editManufacturer(manufacturer.id, result).subscribe(() => {
          this.getManufacturers();
        });
      }
    });
  }

  removeManufacturer(manufacturerId: number) {
    this.manufacturerService.deleteManufacturer(manufacturerId).subscribe(() => {
      this.getManufacturers();
    });
  }


  addManufacturer(): void {
    if (this.addManufacturerForm.valid) {
      this.manufacturerService.addManufacturer(this.addManufacturerForm.value).subscribe(() => {
        this.getManufacturers();
        this.addManufacturerForm.reset();
      });
    }
  }

  onSubmitManufacturer(): void {
    if (this.addManufacturerForm.valid) {
      this.addManufacturer();
    } else {
      Object.keys(this.addManufacturerForm.controls).forEach(field => {
        const control = this.addManufacturerForm.get(field);
        control?.markAsTouched({ onlySelf: true });
      });
    }
  }

  // Categories
  getCategories() {
    this.categoryService.getAllCategories().subscribe((data) => {
      this.categories = data;
    });
  }

  editCategory(category: any) {
    const dialogRef = this.dialog.open(CategoryEditDialogComponent, {
      width: '500px',
      data: { category },
    });
  
    dialogRef.afterClosed().subscribe((result: any) => {
      if (result) {
        this.categoryService.editCategory(category.id, result).subscribe(() => {
          this.getCategories();
        });
      }
    });
  }

  removeCategory(categoryId: number) {
    this.categoryService.deleteCategory(categoryId).subscribe(() => {
      this.getCategories();
    });
  }

  addCategory() {
  if (this.addCategoryForm.valid) {
    this.categoryService.addCategory(this.addCategoryForm.value).subscribe(() => {
      this.getCategories();
      this.addCategoryForm.reset(); 
    });
  }
}

  onSubmit(): void {
    if (this.addCategoryForm.valid) {
      this.addCategory();
    } else {
      Object.keys(this.addCategoryForm.controls).forEach(field => {
        const control = this.addCategoryForm.get(field);
        control?.markAsTouched({ onlySelf: true });
      });
    }
  }
  

  // Products
  getProducts() {
    this.productService.getAllProductsNI().subscribe((data) => {
      this.products = data.filter(product => !product.isDeleted);
      console.log(this.products);
    });
  }

  openEditPopup(product: any) {
    this.editingProduct = { ...product };
  }

  saveProduct(updatedProduct: any) {
    this.productService.updateProduct(updatedProduct).subscribe(
      () => {
        const index = this.products.findIndex(p => p.id === updatedProduct.id);
        if (index !== -1) {
          this.products[index] = updatedProduct;
        }
        this.editingProduct = null;
      },
      error => {
        console.error('Error updating product:', error);
      }
    );
  }

  cancelEdit() {
    this.editingProduct = null;
  }

  removeProduct(productId: number) {
    this.productService.deleteProduct(productId).subscribe(() => {
      this.getProducts();
    });
  }


  generateUniqueId(existingItems: any[]): number {
    const ids = existingItems.map(item => item.id);
    let newId = 1;
    while (ids.includes(newId)) {
      newId++;
    }
    return newId;
  }

  addProduct() {
    if (this.addProductForm.valid) {
      this.productService.createProduct(this.addProductForm.value).subscribe(() => {
        this.getProducts();
        this.addProductForm.reset(); 
      });
    }
  }

  onSubmitProduct(): void {
    if (this.addProductForm.valid) {
      this.addProduct();
    } else {
      Object.keys(this.addProductForm.controls).forEach(field => {
        const control = this.addProductForm.get(field);
        control?.markAsTouched({ onlySelf: true });
      });
    }
  }

  resetForm() {
    this.newProduct = {
      categoryId: 0,
      manufacturerId: 0,
      name: '',
      description: '',
      price: 0
    };
  }

  onFileSelected(event: Event, productId: number) {
    const file = (event.target as HTMLInputElement).files?.[0];
    if (file) {
      this.convertToBase64(file).then(base64 => {
        this.uploadImage(productId, base64);
      });
    }
  }

  convertToBase64(file: File): Promise<string> {
    return new Promise((resolve, reject) => {
      const reader = new FileReader();
      reader.readAsDataURL(file);
      reader.onload = () => resolve(reader.result as string);
      reader.onerror = error => reject(error);
    });
  }

  uploadImage(productId: number, base64Image: string) {
    const payload = {
      productId: productId,
      image: base64Image
    };

    this.httpclient.post('https://localhost:7023/api/ProductImage/AddProductImage', payload)
      .subscribe(
        response => {
          console.log('Image uploaded successfully', response);
          console.log(payload)
        },
        error => {
          console.error('Error uploading image', error);
        }
      );
  }
}

