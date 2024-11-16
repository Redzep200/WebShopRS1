import { TestBed } from '@angular/core/testing';

import { SistemAdminGuard } from './sistemAdmin.guard';

describe('AdminGuard', () => {
  let guard: SistemAdminGuard;

  beforeEach(() => {
    TestBed.configureTestingModule({});
    guard = TestBed.inject(SistemAdminGuard);
  });

  it('should be created', () => {
    expect(guard).toBeTruthy();
  });
});
