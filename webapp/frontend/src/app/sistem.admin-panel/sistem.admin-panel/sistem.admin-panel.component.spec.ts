import { ComponentFixture, TestBed } from '@angular/core/testing';

import { SistemAdminPanelComponent } from './sistem.admin-panel.component';

describe('SistemAdminPanelComponent', () => {
  let component: SistemAdminPanelComponent;
  let fixture: ComponentFixture<SistemAdminPanelComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ SistemAdminPanelComponent ]
    })
    .compileComponents();

    fixture = TestBed.createComponent(SistemAdminPanelComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
