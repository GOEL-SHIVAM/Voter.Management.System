import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';
import { environment } from '../../../environments/environment';
import { Admin, CreateAdminRequest } from '../models/admin.model';
import { ChangePasswordRequest } from '../models/auth.model';

@Injectable({
  providedIn: 'root',
})
export class AdminService {
  private apiUrl = `${environment.apiUrl}/admins`;

  constructor(private http: HttpClient) {}

  getAllAdmins(): Observable<Admin[]> {
    return this.http.get<Admin[]>(this.apiUrl);
  }

  getAdmin(username: string): Observable<Admin> {
    return this.http.get<Admin>(`${this.apiUrl}/${username}`);
  }

  createAdmin(request: CreateAdminRequest): Observable<any> {
    return this.http.post(this.apiUrl, request);
  }

  deleteAdmin(username: string): Observable<any> {
    return this.http.delete(`${this.apiUrl}/${username}`);
  }

  changePassword(request: ChangePasswordRequest): Observable<any> {
    return this.http.put(`${this.apiUrl}/change-password`, request);
  }

  changeAdminPassword(username: string, request: ChangePasswordRequest): Observable<any> {
    return this.http.put(`${this.apiUrl}/${username}/change-password`, request);
  }
}
