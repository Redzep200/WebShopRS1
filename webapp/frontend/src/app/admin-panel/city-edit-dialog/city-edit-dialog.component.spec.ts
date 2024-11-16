import { ComponentFixture, TestBed } from '@angular/core/testing';

import { CityEditDialogComponent } from './city-edit-dialog.component';

describe('CityEditDialogComponent', () => {
  let component: CityEditDialogComponent;
  let fixture: ComponentFixture<CityEditDialogComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ CityEditDialogComponent ]
    })
    .compileComponents();

    fixture = TestBed.createComponent(CityEditDialogComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
