import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';

@Injectable({
  providedIn: 'root'
})
export class RoleService {
  private roleApiUrl = 'https://localhost:7023/api/Role';

  constructor(private http: HttpClient) {}

  getAllRoles(): Observable<any[]> {
    return this.http.get<any[]>(`${this.roleApiUrl}/GetAllRoles`);
  }

  getRoleById(id: number): Observable<any> {
    return this.http.get<any>(`${this.roleApiUrl}/GetRole?id=${id}`);
  }
}