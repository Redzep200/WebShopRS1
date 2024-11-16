import { Injectable } from '@angular/core';
import { CanActivate, Router } from '@angular/router';
import { AuthService } from '../services/auth.service';
import { Observable } from 'rxjs';
import { map, take } from 'rxjs/operators';

@Injectable({
  providedIn: 'root',
})
export class CustomerGuard implements CanActivate {
  constructor(private authService: AuthService, private router: Router) {}

  canActivate(): Observable<boolean> | boolean {
    return this.authService.isCustomer().pipe(
      take(1),
      map((isCustomer) => {
        if (!isCustomer) {
          this.router.navigate(['/shop']);
          return false;
        }
        return true;
      })
    );
  }
}
