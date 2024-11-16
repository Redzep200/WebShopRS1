import { Injectable } from '@angular/core';
import { HttpClient, HttpParams } from '@angular/common/http';
import { Observable, take } from 'rxjs';
import { tap } from 'rxjs/operators';
import { SignalRService } from './signalr.service';
import { AuthService } from './auth.service';
import { Router } from '@angular/router';
import { RoleService } from './role.service';
import { of, catchError, map } from 'rxjs';

@Injectable({
  providedIn: 'root',
})
export class UserService {
  private userApiUrl = 'https://localhost:7023/api/User';

  constructor(private http: HttpClient, 
      private signalRService: SignalRService,
      private authService: AuthService,
      private router: Router,
      private roleService: RoleService) {this.setupSignalRListeners();}

  getAllUsers(): Observable<any[]> {
    return this.http.get<any[]>(`${this.userApiUrl}/GetAllUsers`);
  }

  getUserById(id: number): Observable<any> {
    return this.http.get<any>(`${this.userApiUrl}/GetUserById?id=${id}`);
  }

  getUserByUsername(username: string): Observable<any> {
    return this.http.get<any>(`${this.userApiUrl}/GetUserByUsername?username=${username}`);
  }

  createUser(userData: any): Observable<any> {
    const params = new HttpParams()
      .set('roleId', userData.roleId)
      .set('name', userData.name)
      .set('surname', userData.surname)
      .set('email', userData.email)
      .set('password', userData.password)
      .set('username', userData.username);

    return this.http.post<any>(`${this.userApiUrl}/CreateUser`, {}, { params });
  }

  deleteUser(id: number): Observable<any> {
    return this.http.delete<any>(`${this.userApiUrl}/DeleteUser?id=${id}`).pipe(
      tap(() => {
        console.log('User deleted successfully');
      })
    );
  }

  updateUser(id: number, userData: any): Observable<any> {
    const params = new HttpParams()
      .set('id', id.toString())
      .set('roleId', userData.roleId)
      .set('name', userData.name)
      .set('surname', userData.surname)
      .set('email', userData.email)
      .set('password', userData.password)
      .set('activity', userData.activity.toString())
      .set('username', userData.username);

      return this.http.put<any>(`${this.userApiUrl}/UpdateUser`, null, { params }).pipe(
        tap(() => {
          console.log('User updated successfully');
          // Update the current user's role if it's the logged-in user
          if (id.toString() === this.authService.getCurrentUserId()) {
            this.roleService.getRoleById(userData.roleId).pipe(take(1)).subscribe(role => {
              this.authService.updateCurrentUserRole(role.name);
            });
          }
        })
      );
    
  }

  private setupSignalRListeners(): void {
    this.signalRService.userRoleChanged.subscribe(change => {
      console.log('Role change detected:', change);
      if (change) {
        this.handleUserRoleChange(change.userId, change.newRoleId);
      }
    });
  }

  private handleUserRoleChange(userId: number, newRoleId: number): void {
    console.log('Handling user role change');
    const currentUserId = this.authService.getCurrentUserId();
    if (currentUserId === userId.toString()) {
      this.roleService.getRoleById(newRoleId).pipe(take(1)).subscribe(newRole => {
        this.authService.updateCurrentUserRole(newRole.name);
      });
    }
  }
  

  
  authenticate(loginData: any): Observable<any> {
    return this.http.post<any>(`${this.userApiUrl}/Authenticate`, loginData);
  }

  registerUser(registerData: any): Observable<any> {
    return this.http.post<any>(`${this.userApiUrl}/RegisterUser`, registerData);
  }

  hasEmailRegistered(email: string): Observable<boolean> {
    return this.http.get<any>(`${this.userApiUrl}/GetUserByEmail?email=${email}`)
      .pipe(
        map(user => user !== null),
        catchError(() => of(false))
      );
  }
  
  hasUsernameRegistered(username: string): Observable<boolean> {
    return this.http.get<any>(`${this.userApiUrl}/GetUserByUsername?username=${username}`)
      .pipe(
        map(user => user !== null),
        catchError(() => of(false))
      );
  }
}