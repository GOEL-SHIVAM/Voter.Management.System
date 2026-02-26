import { Injectable } from '@angular/core';
import { HttpClient, HttpParams } from '@angular/common/http';
import { Observable } from 'rxjs';
import { environment } from '../../../environments/environment';
import { Voter, CreateVoterRequest, UpdateVoterRequest, VoteHistory } from '../models/voter.model';
import { ChangePasswordRequest } from '../models/auth.model';

@Injectable({
  providedIn: 'root',
})
export class VoterService {
  private apiUrl = `${environment.apiUrl}/voters`;

  constructor(private http: HttpClient) {}

  getAllVoters(): Observable<Voter[]> {
    return this.http.get<Voter[]>(this.apiUrl);
  }

  getVoter(aadhar: string): Observable<Voter> {
    return this.http.get<Voter>(`${this.apiUrl}/${aadhar}`);
  }

  createVoter(request: CreateVoterRequest): Observable<any> {
    return this.http.post(this.apiUrl, request);
  }

  updateVoter(aadhar: string, request: UpdateVoterRequest): Observable<any> {
    return this.http.put(`${this.apiUrl}/${aadhar}`, request);
  }

  deleteVoter(aadhar: string): Observable<any> {
    return this.http.delete(`${this.apiUrl}/${aadhar}`);
  }

  changePassword(request: ChangePasswordRequest): Observable<any> {
    return this.http.put(`${this.apiUrl}/change-password`, request);
  }

  getMyProfile(): Observable<Voter> {
    return this.http.get<Voter>(`${this.apiUrl}/my-profile`);
  }

  getMyVotes(count: number = 10): Observable<VoteHistory[]> {
    const params = new HttpParams().set('count', count.toString());
    return this.http.get<VoteHistory[]>(`${this.apiUrl}/my-votes`, { params });
  }
}
