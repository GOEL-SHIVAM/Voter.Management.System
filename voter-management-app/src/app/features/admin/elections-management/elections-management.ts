import { Component, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormBuilder, FormGroup, Validators, ReactiveFormsModule } from '@angular/forms';
import { MatTableModule } from '@angular/material/table';
import { MatButtonModule } from '@angular/material/button';
import { MatIconModule } from '@angular/material/icon';
import { MatCardModule } from '@angular/material/card';
import { MatFormFieldModule } from '@angular/material/form-field';
import { MatInputModule } from '@angular/material/input';
import { MatSelectModule } from '@angular/material/select';
import { MatProgressSpinnerModule } from '@angular/material/progress-spinner';
import { MatChipsModule } from '@angular/material/chips';
import { MatBadgeModule } from '@angular/material/badge';
import { ElectionService } from '../../../core/services/election.service';
import { PartyService } from '../../../core/services/party.service';
import { NotificationService } from '../../../core/services/notification.service';
import {
  Election,
  CreateElectionRequest,
  AddPartyToElectionRequest,
} from '../../../core/models/election.model';
import { Party } from '../../../core/models/party.model';

@Component({
  selector: 'app-elections-management',
  standalone: true,
  imports: [
    CommonModule,
    ReactiveFormsModule,
    MatTableModule,
    MatButtonModule,
    MatIconModule,
    MatCardModule,
    MatFormFieldModule,
    MatInputModule,
    MatSelectModule,
    MatProgressSpinnerModule,
    MatChipsModule,
    MatBadgeModule,
  ],
  templateUrl: './elections-management.html',
  styleUrl: './elections-management.scss',
})
export class ElectionsManagementComponent implements OnInit {
  elections: Election[] = [];
  allParties: Party[] = [];
  displayedColumns: string[] = [
    'electionId',
    'electionCode',
    'status',
    'partyCount',
    'totalVotes',
    'winner',
    'actions',
  ];
  isLoading = false;
  showForm = false;
  showPartyForm = false;
  electionForm!: FormGroup;
  partyForm!: FormGroup;
  selectedElection: Election | null = null;
  electionParties: string[] = [];

  constructor(
    private electionService: ElectionService,
    private partyService: PartyService,
    private notificationService: NotificationService,
    private fb: FormBuilder,
  ) {}

  ngOnInit(): void {
    this.initForms();
    this.loadElections();
    this.loadParties();
  }

  initForms(): void {
    this.electionForm = this.fb.group({
      electionCode: ['', [Validators.required, Validators.minLength(3), Validators.maxLength(50)]],
    });

    this.partyForm = this.fb.group({
      partyName: ['', [Validators.required]],
    });
  }

  loadElections(): void {
    this.isLoading = true;
    this.electionService.getAllElections().subscribe({
      next: (elections) => {
        this.elections = elections;
        this.isLoading = false;
      },
      error: () => {
        this.isLoading = false;
      },
    });
  }

  loadParties(): void {
    this.partyService.getAllParties().subscribe({
      next: (parties) => {
        this.allParties = parties;
      },
    });
  }

  onAdd(): void {
    this.showForm = true;
    this.electionForm.reset();
  }

  onCancel(): void {
    this.showForm = false;
    this.electionForm.reset();
  }

  onSubmit(): void {
    if (this.electionForm.invalid) {
      Object.keys(this.electionForm.controls).forEach((key) => {
        this.electionForm.get(key)?.markAsTouched();
      });
      return;
    }

    const request: CreateElectionRequest = this.electionForm.value;

    this.electionService.createElection(request).subscribe({
      next: () => {
        this.notificationService.success('Election created successfully');
        this.showForm = false;
        this.electionForm.reset();
        this.loadElections();
      },
      error: () => {
        // Error handled by interceptor
      },
    });
  }

  onManageParties(election: Election): void {
    this.selectedElection = election;
    this.showPartyForm = true;
    this.loadElectionParties(election.electionCode);
  }

  loadElectionParties(electionCode: string): void {
    this.electionService.getElectionParties(electionCode).subscribe({
      next: (parties) => {
        this.electionParties = parties;
      },
    });
  }

  onAddPartyToElection(): void {
    if (this.partyForm.invalid || !this.selectedElection) {
      return;
    }

    const request: AddPartyToElectionRequest = {
      partyName: this.partyForm.value.partyName,
    };

    this.electionService.addPartyToElection(this.selectedElection.electionCode, request).subscribe({
      next: () => {
        this.notificationService.success('Party added to election');
        this.partyForm.reset();
        this.loadElectionParties(this.selectedElection!.electionCode);
        this.loadElections();
      },
      error: () => {
        // Error handled by interceptor
      },
    });
  }

  onRemovePartyFromElection(partyName: string): void {
    if (!this.selectedElection) return;

    if (confirm(`Remove ${partyName} from this election?`)) {
      this.electionService
        .removePartyFromElection(this.selectedElection.electionCode, partyName)
        .subscribe({
          next: () => {
            this.notificationService.success('Party removed from election');
            this.loadElectionParties(this.selectedElection!.electionCode);
            this.loadElections();
          },
          error: () => {
            // Error handled by interceptor
          },
        });
    }
  }

  onClosePartyForm(): void {
    this.showPartyForm = false;
    this.selectedElection = null;
    this.electionParties = [];
    this.partyForm.reset();
  }

  onStartElection(election: Election): void {
    if (confirm(`Start election "${election.electionCode}"? Voting will begin.`)) {
      this.electionService.startElection(election.electionCode).subscribe({
        next: () => {
          this.notificationService.success('Election started successfully');
          this.loadElections();
        },
        error: () => {
          // Error handled by interceptor
        },
      });
    }
  }

  onStopElection(election: Election): void {
    if (confirm(`Stop election "${election.electionCode}"? Results will be calculated.`)) {
      this.electionService.stopElection(election.electionCode).subscribe({
        next: () => {
          this.notificationService.success('Election stopped. Results calculated.');
          this.loadElections();
        },
        error: () => {
          // Error handled by interceptor
        },
      });
    }
  }

  getStatusColor(status: string): string {
    switch (status) {
      case 'Registered':
        return 'accent';
      case 'Started':
        return 'primary';
      case 'Ended':
        return 'warn';
      default:
        return '';
    }
  }

  canManageParties(election: Election): boolean {
    return election.status === 'Registered';
  }

  canStartElection(election: Election): boolean {
    return election.status === 'Registered' && election.partyCount > 0;
  }

  canStopElection(election: Election): boolean {
    return election.status === 'Started';
  }
}
