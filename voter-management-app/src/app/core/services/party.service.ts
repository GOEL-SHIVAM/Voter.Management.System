import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';
import { environment } from '../../../environments/environment';
import { Party, CreatePartyRequest } from '../models/party.model';

@Injectable({
  providedIn: 'root',
})
export class PartyService {
  private apiUrl = `${environment.apiUrl}/parties`;

  constructor(private http: HttpClient) {}

  getAllParties(): Observable<Party[]> {
    return this.http.get<Party[]>(this.apiUrl);
  }

  getParty(partyName: string): Observable<Party> {
    return this.http.get<Party>(`${this.apiUrl}/${partyName}`);
  }

  createParty(request: CreatePartyRequest): Observable<any> {
    return this.http.post(this.apiUrl, request);
  }

  deleteParty(partyName: string): Observable<any> {
    return this.http.delete(`${this.apiUrl}/${partyName}`);
  }
}
