import { Injectable } from '@angular/core';
import { ProductService } from 'src/app/services/product.service';
import { CategoryService } from '../services/category.service';
import { CityService } from '../services/city.service';
import { CountryService } from '../services/country.service';
import { SupplierService } from '../services/supplier.service';
import { ManufacturerService } from '../services/manufacturer.service';
import { Observable, BehaviorSubject } from 'rxjs';
import * as signalR from "@microsoft/signalr";

@Injectable({
  providedIn: 'root',
})
export class ShopService {
  public searchQuerySubject: BehaviorSubject<string> = new BehaviorSubject<string>('');
  public searchQuery: Observable<string> = this.searchQuerySubject.asObservable();

  constructor(
    private productService: ProductService,
    private categoryService: CategoryService,
    private cityService: CityService,
    private countryService: CountryService,
    private supplierService: SupplierService,
    private manufacturerService: ManufacturerService
  ) {}

  updateSearchQuery(searchQuery: string) {
    this.searchQuerySubject.next(searchQuery);
  }

  // Products
  getAllProducts() {
    return this.productService.getAllProducts();
  }

  getProductById(id: number) {
    return this.productService.getProductById(id);
  }

  getProductsByName(name: string) {
    return this.productService.getProducts(name);
  }

  getProductsByPriceRange(minPrice?: number, maxPrice?: number) {
    return this.productService.getProductsByPriceRange(minPrice, maxPrice);
  }
  
  getProductsByCategory(categoryId: number) {
    return this.productService.getProductsByCategory(categoryId);
  }




  deleteProduct(productId: number) {
    return this.productService.deleteProduct(productId);
  }

  // Categories
  getAllCategories() {
    return this.categoryService.getAllCategories();
  }

  addCategory(categoryData: any) {
    return this.categoryService.addCategory(categoryData);
  }

  editCategory(categoryId: number, categoryData: any) {
    return this.categoryService.editCategory(categoryId, categoryData);
  }

  deleteCategory(categoryId: number) {
    return this.categoryService.deleteCategory(categoryId);
  }

  // Cities
  getAllCities() {
    return this.cityService.getAllCities();
  }

  addCity(cityData: any) {
    return this.cityService.addCity(cityData);
  }

  editCity(cityId: number, cityData: any) {
    return this.cityService.editCity(cityId, cityData);
  }

  deleteCity(cityId: number) {
    return this.cityService.deleteCity(cityId);
  }

  // Countries
  getAllCountries() {
    return this.countryService.getAllCountries();
  }

  addCountry(countryData: any) {
    return this.countryService.addCountry(countryData);
  }

  editCountry(countryId: number, countryData: any) {
    return this.countryService.editCountry(countryId, countryData);
  }

  deleteCountry(countryId: number) {
    return this.countryService.deleteCountry(countryId);
  }

  // Suppliers
  getAllSuppliers() {
    return this.supplierService.getAllSuppliers();
  }

  addSupplier(supplierData: any) {
    return this.supplierService.addSupplier(supplierData);
  }

  editSupplier(supplierId: number, supplierData: any) {
    return this.supplierService.editSupplier(supplierId, supplierData);
  }

  deleteSupplier(supplierId: number) {
    return this.supplierService.deleteSupplier(supplierId);
  }

  // Manufacturers
  getAllManufacturers() {
    return this.manufacturerService.getAllManufacturers();
  }
}