import { Component, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormBuilder, FormGroup, Validators, ReactiveFormsModule } from '@angular/forms';
import { MatTableModule } from '@angular/material/table';
import { MatButtonModule } from '@angular/material/button';
import { MatIconModule } from '@angular/material/icon';
import { MatDialogModule, MatDialog } from '@angular/material/dialog';
import { MatCardModule } from '@angular/material/card';
import { MatFormFieldModule } from '@angular/material/form-field';
import { MatInputModule } from '@angular/material/input';
import { MatDatepickerModule } from '@angular/material/datepicker';
import { MatNativeDateModule } from '@angular/material/core';
import { MatProgressSpinnerModule } from '@angular/material/progress-spinner';
import { VoterService } from '../../../core/services/voter.service';
import { NotificationService } from '../../../core/services/notification.service';
import { Voter, CreateVoterRequest } from '../../../core/models/voter.model';

@Component({
  selector: 'app-voters-management',
  standalone: true,
  imports: [
    CommonModule,
    ReactiveFormsModule,
    MatTableModule,
    MatButtonModule,
    MatIconModule,
    MatDialogModule,
    MatCardModule,
    MatFormFieldModule,
    MatInputModule,
    MatDatepickerModule,
    MatNativeDateModule,
    MatProgressSpinnerModule,
  ],
  templateUrl: './voters-management.html',
  styleUrl: './voters-management.scss',
})
export class VotersManagementComponent implements OnInit {
  voters: Voter[] = [];
  displayedColumns: string[] = ['voterId', 'aadhar', 'name', 'birthDate', 'age', 'actions'];
  isLoading = false;
  showForm = false;
  voterForm!: FormGroup;
  maxDate: Date;

  constructor(
    private voterService: VoterService,
    private notificationService: NotificationService,
    private fb: FormBuilder,
    private dialog: MatDialog,
  ) {
    this.maxDate = new Date();
    this.maxDate.setFullYear(this.maxDate.getFullYear() - 18);
  }

  ngOnInit(): void {
    this.initForm();
    this.loadVoters();
  }

  initForm(): void {
    this.voterForm = this.fb.group({
      aadhar: [
        '',
        [
          Validators.required,
          Validators.pattern(/^\d{12}$/),
          Validators.minLength(12),
          Validators.maxLength(12),
        ],
      ],
      password: ['', [Validators.required, Validators.minLength(6)]],
      name: ['', [Validators.required, Validators.minLength(2)]],
      birthDate: ['', [Validators.required]],
    });
  }

  loadVoters(): void {
    this.isLoading = true;
    this.voterService.getAllVoters().subscribe({
      next: (voters) => {
        this.voters = voters;
        this.isLoading = false;
      },
      error: () => {
        this.isLoading = false;
      },
    });
  }

  onAdd(): void {
    this.showForm = true;
    this.voterForm.reset();
  }

  onCancel(): void {
    this.showForm = false;
    this.voterForm.reset();
  }

  onSubmit(): void {
    if (this.voterForm.invalid) {
      Object.keys(this.voterForm.controls).forEach((key) => {
        this.voterForm.get(key)?.markAsTouched();
      });
      return;
    }

    const request: CreateVoterRequest = this.voterForm.value;

    this.voterService.createVoter(request).subscribe({
      next: () => {
        this.notificationService.success('Voter created successfully');
        this.showForm = false;
        this.voterForm.reset();
        this.loadVoters();
      },
      error: () => {
        // Error handled by interceptor
      },
    });
  }

  onDelete(voter: Voter): void {
    if (confirm(`Are you sure you want to delete voter ${voter.name}?`)) {
      this.voterService.deleteVoter(voter.aadhar).subscribe({
        next: () => {
          this.notificationService.success('Voter deleted successfully');
          this.loadVoters();
        },
        error: () => {
          // Error handled by interceptor
        },
      });
    }
  }
}
