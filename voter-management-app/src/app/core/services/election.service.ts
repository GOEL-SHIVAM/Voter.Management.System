import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';
import { environment } from '../../../environments/environment';
import {
  Election,
  CreateElectionRequest,
  AddPartyToElectionRequest,
  CastVoteRequest,
  ElectionResult,
} from '../models/election.model';

@Injectable({
  providedIn: 'root',
})
export class ElectionService {
  private apiUrl = `${environment.apiUrl}/elections`;
  private votesUrl = `${environment.apiUrl}/votes`;
  private resultsUrl = `${environment.apiUrl}/results`;

  constructor(private http: HttpClient) {}

  // Elections
  getAllElections(): Observable<Election[]> {
    return this.http.get<Election[]>(this.apiUrl);
  }

  getElection(electionCode: string): Observable<Election> {
    return this.http.get<Election>(`${this.apiUrl}/${electionCode}`);
  }

  getElectionParties(electionCode: string): Observable<string[]> {
    return this.http.get<string[]>(`${this.apiUrl}/${electionCode}/parties`);
  }

  createElection(request: CreateElectionRequest): Observable<any> {
    return this.http.post(this.apiUrl, request);
  }

  addPartyToElection(electionCode: string, request: AddPartyToElectionRequest): Observable<any> {
    return this.http.post(`${this.apiUrl}/${electionCode}/parties`, request);
  }

  removePartyFromElection(electionCode: string, partyName: string): Observable<any> {
    return this.http.delete(`${this.apiUrl}/${electionCode}/parties/${partyName}`);
  }

  startElection(electionCode: string): Observable<any> {
    return this.http.put(`${this.apiUrl}/${electionCode}/start`, {});
  }

  stopElection(electionCode: string): Observable<any> {
    return this.http.put(`${this.apiUrl}/${electionCode}/stop`, {});
  }

  // Voting
  castVote(request: CastVoteRequest): Observable<any> {
    return this.http.post(this.votesUrl, request);
  }

  getVoteHistory(count: number = 10): Observable<any[]> {
    return this.http.get<any[]>(`${this.votesUrl}/my-history?count=${count}`);
  }

  getVoteCount(): Observable<any> {
    return this.http.get<any>(`${this.votesUrl}/my-vote-count`);
  }

  // Results
  getResults(electionCode: string): Observable<ElectionResult> {
    return this.http.get<ElectionResult>(`${this.resultsUrl}/${electionCode}`);
  }

  getDetailedResults(electionCode: string): Observable<any> {
    return this.http.get<any>(`${this.resultsUrl}/${electionCode}/detailed`);
  }

  getResultsSummary(electionCode: string): Observable<any> {
    return this.http.get<any>(`${this.resultsUrl}/${electionCode}/summary`);
  }
}
