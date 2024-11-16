import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { BehaviorSubject, Observable, tap, timer } from 'rxjs';
import { jwtDecode } from 'jwt-decode';
import { of } from 'rxjs';
import { Router } from '@angular/router';
import { Subject } from 'rxjs';
import { SignalRService } from './signalr.service';
import { MatDialog } from '@angular/material/dialog';
import { LoginModalComponent } from '../login-modal/login-modal.component';

@Injectable({
  providedIn: 'root',
})
export class AuthService {
  private apiURL = 'https://localhost:7023/api/User';
  private currentUserSubject = new BehaviorSubject<string | null>(null);
  private currentUserEmailSubject = new BehaviorSubject<string | null>(null);
  private currentUserId = new BehaviorSubject<string | null>(null);
  private isLoggedInSubject = new BehaviorSubject<boolean>(false);
  private currentUserRoleSubject = new BehaviorSubject<string | null>(null);

  private authStatusChanged = new Subject<boolean>();
  authStatusChanged$ = this.authStatusChanged.asObservable();


  private updateAuthStatus(isLoggedIn: boolean) {
    this.isLoggedInSubject.next(isLoggedIn);
    this.authStatusChanged.next(isLoggedIn);
  }

  constructor(private http: HttpClient, private router: Router,private signalRService: SignalRService, private dialog:MatDialog) {
    this.checkToken();
    this.initializeSignalR();
  }


  get isLoggedIn(): Observable<boolean> {
    return this.isLoggedInSubject.asObservable();
  }

  login(email: string, password: string): Observable<any> {
    return this.http
      .post<{ token: string, role: string }>(`${this.apiURL}/Authenticate`, {
        email,
        password,
      })
      .pipe(
        tap((res) => {
          localStorage.setItem('token', res.token);
          localStorage.setItem('userEmail', email);     
          const decoded = jwtDecode<any>(res.token);
          this.currentUserSubject.next(decoded.unique_name);
          this.currentUserEmailSubject.next(email);
          this.currentUserRoleSubject.next(res.role);
          this.currentUserId.next(decoded.nameid);
          this.isLoggedInSubject.next(true);
          console.log(this.currentUserId);
          this.updateAuthStatus(true);
          this.reloadPage(); 
        })
      );
  }

  logout(): void {
    localStorage.removeItem('token');
    localStorage.removeItem('userEmail');
    this.currentUserSubject.next(null);
    this.currentUserEmailSubject.next(null);
    this.updateAuthStatus(false);
    this.currentUserRoleSubject.next(null);
    this.isLoggedInSubject.next(false);
    this.router.navigate(['/shop']);
  }

  checkToken(): void {
    const token = localStorage.getItem('token');
    const email = localStorage.getItem('userEmail');
    if (token) {
      const decoded = jwtDecode<any>(token);
      this.currentUserSubject.next(decoded.unique_name);
      this.isLoggedInSubject.next(true);
      this.currentUserEmailSubject.next(decoded.email);
      this.updateAuthStatus(true);
    } else {
      this.updateAuthStatus(false);
      this.isLoggedInSubject.next(false);
    }
  }

  isAdmin(): Observable<boolean> {
    const token = localStorage.getItem('token');
    if (token) {
      const decoded = jwtDecode<any>(token);
      return of(decoded?.role === 'Web shop admin');
    }
    return of(false);
  }

  isSistemAdmin(): Observable<boolean> {
    const token = localStorage.getItem('token');
    if (token) {
      const decoded = jwtDecode<any>(token);
      return of(decoded?.role === 'SistemAdmin');
    }
    return of(false);
  }

  isSupport(): Observable<boolean> {
    const token = localStorage.getItem('token');
    if (token) {
      const decoded = jwtDecode<any>(token);
      return of(decoded?.role === 'Support');
    }
    return of(false);
  }

  isEmployee(): Observable<boolean> {
    const token = localStorage.getItem('token');
    if (token) {
      const decoded = jwtDecode<any>(token);
      return of(decoded?.role === 'Employee');
    }
    return of(false);
  }

  isCustomer(): Observable<boolean> {
    const token = localStorage.getItem('token');
    if (token) {
      const decoded = jwtDecode<any>(token);
      return of(decoded?.role === 'Customer');
    }
    return of(false);
  }

  getCurrentUser(): Observable<string | null> {
    return this.currentUserSubject.asObservable();
  }

  getCurrentUserEmail(): string | null {
    return this.currentUserEmailSubject.value || localStorage.getItem('userEmail');
  }

  getCurrentUserId(): string | null {
    if (this.currentUserId.value) {
      return this.currentUserId.value;
    }
    const token = localStorage.getItem('token');
    if (token) {
      const decoded = jwtDecode<any>(token);
      return decoded.nameid;
    }
    return null;
  }


  
  reloadPage() {
    window.location.reload();
  }

  getCurrentRole(): Observable<string> {
    const token = localStorage.getItem('token');
    if (token) {
      const decoded = jwtDecode<any>(token);
      return of(decoded.role);
    }
    return of('Unknown');
  }

  getCurrentUserRole(): Observable<string | null> {
    return this.currentUserRoleSubject.asObservable();
  }

  updateCurrentUserRole(newRole: string): void {
    this.currentUserRoleSubject.next(newRole);
  }

  private initializeSignalR(): void {
    this.signalRService.startConnections();
    this.setupRoleChangeListener();
  }

  private setupRoleChangeListener(): void {
    this.signalRService.userRoleChanged.subscribe(change => {
      if (change && change.userId.toString() === this.getCurrentUserId()) {
        this.handleRoleChange();
      }
    });
  }

  private handleRoleChange(): void {
    const message = "Your role has been altered. You have been logged out.";
    alert(message);  
      this.logout();
      const dialogRef = this.dialog.open(LoginModalComponent);
  }
}
