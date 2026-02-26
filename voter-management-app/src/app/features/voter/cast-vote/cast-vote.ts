import { Component, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormBuilder, FormGroup, Validators, ReactiveFormsModule } from '@angular/forms';
import { Router } from '@angular/router';
import { MatCardModule } from '@angular/material/card';
import { MatFormFieldModule } from '@angular/material/form-field';
import { MatSelectModule } from '@angular/material/select';
import { MatButtonModule } from '@angular/material/button';
import { MatIconModule } from '@angular/material/icon';
import { MatProgressSpinnerModule } from '@angular/material/progress-spinner';
import { MatRadioModule } from '@angular/material/radio';
import { ElectionService } from '../../../core/services/election.service';
import { NotificationService } from '../../../core/services/notification.service';
import { Election, CastVoteRequest } from '../../../core/models/election.model';

@Component({
  selector: 'app-cast-vote',
  standalone: true,
  imports: [
    CommonModule,
    ReactiveFormsModule,
    MatCardModule,
    MatFormFieldModule,
    MatSelectModule,
    MatButtonModule,
    MatIconModule,
    MatProgressSpinnerModule,
    MatRadioModule,
  ],
  templateUrl: './cast-vote.html',
  styleUrl: './cast-vote.scss',
})
export class CastVoteComponent implements OnInit {
  voteForm!: FormGroup;
  elections: Election[] = [];
  startedElections: Election[] = [];
  selectedElectionParties: string[] = [];
  isLoading = false;
  isSubmitting = false;

  constructor(
    private fb: FormBuilder,
    private electionService: ElectionService,
    private notificationService: NotificationService,
    private router: Router,
  ) {}

  ngOnInit(): void {
    this.initForm();
    this.loadElections();
  }

  initForm(): void {
    this.voteForm = this.fb.group({
      electionCode: ['', [Validators.required]],
      partyName: ['', [Validators.required]],
    });

    // Listen for election selection changes
    this.voteForm.get('electionCode')?.valueChanges.subscribe((electionCode) => {
      if (electionCode) {
        this.loadElectionParties(electionCode);
        this.voteForm.patchValue({ partyName: '' });
      }
    });
  }

  loadElections(): void {
    this.isLoading = true;
    this.electionService.getAllElections().subscribe({
      next: (elections) => {
        this.elections = elections;
        this.startedElections = elections.filter((e) => e.status === 'Started');
        this.isLoading = false;

        if (this.startedElections.length === 0) {
          this.notificationService.info('No active elections available for voting');
        }
      },
      error: () => {
        this.isLoading = false;
      },
    });
  }

  loadElectionParties(electionCode: string): void {
    this.electionService.getElectionParties(electionCode).subscribe({
      next: (parties) => {
        this.selectedElectionParties = parties;
      },
      error: () => {
        this.selectedElectionParties = [];
      },
    });
  }

  onSubmit(): void {
    if (this.voteForm.invalid) {
      Object.keys(this.voteForm.controls).forEach((key) => {
        this.voteForm.get(key)?.markAsTouched();
      });
      return;
    }

    const request: CastVoteRequest = this.voteForm.value;

    this.isSubmitting = true;
    this.electionService.castVote(request).subscribe({
      next: () => {
        this.notificationService.success('Vote cast successfully!');
        this.voteForm.reset();
        this.selectedElectionParties = [];
        this.isSubmitting = false;

        // Navigate to history after a short delay
        setTimeout(() => {
          this.router.navigate(['/voter/history']);
        }, 1500);
      },
      error: () => {
        this.isSubmitting = false;
      },
    });
  }
}
