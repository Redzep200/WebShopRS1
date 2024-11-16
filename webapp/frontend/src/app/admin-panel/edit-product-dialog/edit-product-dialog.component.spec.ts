import { ComponentFixture, TestBed } from '@angular/core/testing';

import { EditProductPopupComponent } from './edit-product-dialog.component';

describe('EditProductDialogComponent', () => {
  let component: EditProductPopupComponent;
  let fixture: ComponentFixture<EditProductPopupComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ EditProductPopupComponent ]
    })
    .compileComponents();

    fixture = TestBed.createComponent(EditProductPopupComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
