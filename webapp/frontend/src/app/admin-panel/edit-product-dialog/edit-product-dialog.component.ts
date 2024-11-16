import { Component, Input, Output, EventEmitter } from '@angular/core';

@Component({
  selector: 'app-edit-product-popup',
  template: `
    <div class="popup">
    <h2>Edit Product</h2>
<form #productForm="ngForm" (ngSubmit)="onSubmit()">
  <div>
    <label for="name">Name:</label>
    <input 
      type="text" 
      id="name" 
      [(ngModel)]="product.name" 
      name="name" 
      required
      #name="ngModel">
    <div *ngIf="name?.invalid && name?.touched">
      <small *ngIf="name?.errors?.['required']">Name is required.</small>
    </div>
  </div>

  <div>
    <label for="description">Description:</label>
    <textarea 
      id="description" 
      [(ngModel)]="product.description" 
      name="description"
      maxlength="255" 
      #description="ngModel"></textarea>
    <div *ngIf="description?.invalid && description?.touched">
      <small *ngIf="description?.errors?.['maxlength']">Description cannot exceed 255 characters.</small>
    </div>
  </div>

  <div>
    <label for="price">Price:</label>
    <input 
      type="number" 
      id="price" 
      [(ngModel)]="product.price" 
      name="price" 
      required 
      min="0" 
      #price="ngModel">
    <div *ngIf="price?.invalid && price?.touched">
      <small *ngIf="price?.errors?.['required']">Price is required.</small>
      <small *ngIf="price?.errors?.['min']">Price cannot be negative.</small>
    </div>
  </div>

  <div>
    <label for="categoryId">Category ID:</label>
    <input 
      type="number" 
      id="categoryId" 
      [(ngModel)]="product.categoryId" 
      name="categoryId" 
      required 
      min="1" 
      #categoryId="ngModel">
    <div *ngIf="categoryId?.invalid && categoryId?.touched">
      <small *ngIf="categoryId?.errors?.['required']">Category ID is required.</small>
      <small *ngIf="categoryId?.errors?.['min']">Category ID must be a positive number.</small>
    </div>
  </div>

  <div>
    <label for="manufacturerId">Manufacturer ID:</label>
    <input 
      type="number" 
      id="manufacturerId" 
      [(ngModel)]="product.manufacturerId" 
      name="manufacturerId" 
      required 
      min="1" 
      #manufacturerId="ngModel">
    <div *ngIf="manufacturerId?.invalid && manufacturerId?.touched">
      <small *ngIf="manufacturerId?.errors?.['required']">Manufacturer ID is required.</small>
      <small *ngIf="manufacturerId?.errors?.['min']">Manufacturer ID must be a positive number.</small>
    </div>
  </div>

  <button type="submit" [disabled]="productForm.invalid">Save</button>
  <button type="button" (click)="onCancel()">Cancel</button>
</form>


    </div>
  `,
  styles: [`
    .popup {
      position: fixed;
      top: 50%;
      left: 50%;
      transform: translate(-50%, -50%);
      background-color: white;
      padding: 20px;
      border-radius: 5px;
      box-shadow: 0 2px 10px rgba(0, 0, 0, 0.1);
    }
    form {
      display: flex;
      flex-direction: column;
    }
    label, input, textarea {
      margin-bottom: 10px;
    }
  `]
})
export class EditProductPopupComponent {
  @Input() product: any;
  @Output() save = new EventEmitter<any>();
  @Output() cancel = new EventEmitter<void>();

  onSubmit() {
    this.save.emit(this.product);
  }

  onCancel() {
    this.cancel.emit();
  }
}