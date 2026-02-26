import { Component, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { ActivatedRoute } from '@angular/router';
import { MatCardModule } from '@angular/material/card';
import { MatProgressSpinnerModule } from '@angular/material/progress-spinner';
import { MatIconModule } from '@angular/material/icon';
import { MatProgressBarModule } from '@angular/material/progress-bar';
import { MatChipsModule } from '@angular/material/chips';
import { ElectionService } from '../../../core/services/election.service';
import { NotificationService } from '../../../core/services/notification.service';

@Component({
  selector: 'app-results',
  standalone: true,
  imports: [
    CommonModule,
    MatCardModule,
    MatProgressSpinnerModule,
    MatIconModule,
    MatProgressBarModule,
    MatChipsModule,
  ],
  templateUrl: './results.html',
  styleUrl: './results.scss',
})
export class ResultsComponent implements OnInit {
  electionCode: string = '';
  results: any = null;
  isLoading = false;

  constructor(
    private route: ActivatedRoute,
    private electionService: ElectionService,
    private notificationService: NotificationService,
  ) {}

  ngOnInit(): void {
    this.route.params.subscribe((params) => {
      this.electionCode = params['electionCode'];
      if (this.electionCode) {
        this.loadResults();
      }
    });
  }

  loadResults(): void {
    this.isLoading = true;
    this.electionService.getDetailedResults(this.electionCode).subscribe({
      next: (data) => {
        this.results = data;
        this.isLoading = false;
      },
      error: () => {
        this.isLoading = false;
        this.notificationService.error('Failed to load results');
      },
    });
  }

  getWinnerParty(): any {
    if (!this.results?.partyResults) return null;
    return this.results.partyResults.find((p: any) => p.partyName === this.results.winner);
  }

  getOtherParties(): any[] {
    if (!this.results?.partyResults) return [];
    return this.results.partyResults.filter((p: any) => p.partyName !== this.results.winner);
  }
}
