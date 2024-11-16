import { ComponentFixture, TestBed } from '@angular/core/testing';

import { ManufaturerEditDialogComponent } from './manufacturer-edit-dialog.component';

describe('ManufaturerEditDialogComponent', () => {
  let component: ManufaturerEditDialogComponent;
  let fixture: ComponentFixture<ManufaturerEditDialogComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ ManufaturerEditDialogComponent ]
    })
    .compileComponents();

    fixture = TestBed.createComponent(ManufaturerEditDialogComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
