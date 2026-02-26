import { Component, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { MatCardModule } from '@angular/material/card';
import { MatTableModule } from '@angular/material/table';
import { MatIconModule } from '@angular/material/icon';
import { MatProgressSpinnerModule } from '@angular/material/progress-spinner';
import { MatChipsModule } from '@angular/material/chips';
import { MatButtonModule } from '@angular/material/button';
import { Router } from '@angular/router';
import { ElectionService } from '../../../core/services/election.service';
import { VoteHistory } from '../../../core/models/voter.model';

@Component({
  selector: 'app-vote-history',
  standalone: true,
  imports: [
    CommonModule,
    MatCardModule,
    MatTableModule,
    MatIconModule,
    MatProgressSpinnerModule,
    MatChipsModule,
    MatButtonModule,
  ],
  templateUrl: './vote-history.html',
  styleUrl: './vote-history.scss',
})
export class VoteHistoryComponent implements OnInit {
  voteHistory: VoteHistory[] = [];
  displayedColumns: string[] = ['index', 'electionCode', 'partyName', 'votedAt', 'actions'];
  isLoading = false;
  totalVotes = 0;
  electionsParticipated = 0;

  constructor(
    private electionService: ElectionService,
    private router: Router,
  ) {}

  ngOnInit(): void {
    this.loadVoteHistory();
    this.loadVoteStats();
  }

  loadVoteHistory(): void {
    this.isLoading = true;
    this.electionService.getVoteHistory(50).subscribe({
      next: (history) => {
        this.voteHistory = history;
        this.isLoading = false;
      },
      error: () => {
        this.isLoading = false;
      },
    });
  }

  loadVoteStats(): void {
    this.electionService.getVoteCount().subscribe({
      next: (stats) => {
        this.totalVotes = stats.totalVotes || 0;
        this.electionsParticipated = stats.electionsParticipated || 0;
      },
    });
  }

  viewResults(electionCode: string): void {
    this.router.navigate(['/results', electionCode]);
  }
}
