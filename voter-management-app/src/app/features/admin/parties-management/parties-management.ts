import { Component, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormBuilder, FormGroup, Validators, ReactiveFormsModule } from '@angular/forms';
import { MatTableModule } from '@angular/material/table';
import { MatButtonModule } from '@angular/material/button';
import { MatIconModule } from '@angular/material/icon';
import { MatCardModule } from '@angular/material/card';
import { MatFormFieldModule } from '@angular/material/form-field';
import { MatInputModule } from '@angular/material/input';
import { MatProgressSpinnerModule } from '@angular/material/progress-spinner';
import { PartyService } from '../../../core/services/party.service';
import { NotificationService } from '../../../core/services/notification.service';
import { Party, CreatePartyRequest } from '../../../core/models/party.model';

@Component({
  selector: 'app-parties-management',
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
    MatProgressSpinnerModule,
  ],
  templateUrl: './parties-management.html',
  styleUrl: './parties-management.scss',
})
export class PartiesManagementComponent implements OnInit {
  parties: Party[] = [];
  displayedColumns: string[] = ['partyId', 'partyName', 'actions'];
  isLoading = false;
  showForm = false;
  partyForm!: FormGroup;

  constructor(
    private partyService: PartyService,
    private notificationService: NotificationService,
    private fb: FormBuilder,
  ) {}

  ngOnInit(): void {
    this.initForm();
    this.loadParties();
  }

  initForm(): void {
    this.partyForm = this.fb.group({
      partyName: ['', [Validators.required, Validators.minLength(2), Validators.maxLength(100)]],
      password: ['', [Validators.required, Validators.minLength(6)]],
    });
  }

  loadParties(): void {
    this.isLoading = true;
    this.partyService.getAllParties().subscribe({
      next: (parties) => {
        this.parties = parties;
        this.isLoading = false;
      },
      error: () => {
        this.isLoading = false;
      },
    });
  }

  onAdd(): void {
    this.showForm = true;
    this.partyForm.reset();
  }

  onCancel(): void {
    this.showForm = false;
    this.partyForm.reset();
  }

  onSubmit(): void {
    if (this.partyForm.invalid) {
      Object.keys(this.partyForm.controls).forEach((key) => {
        this.partyForm.get(key)?.markAsTouched();
      });
      return;
    }

    const request: CreatePartyRequest = this.partyForm.value;

    this.partyService.createParty(request).subscribe({
      next: () => {
        this.notificationService.success('Party created successfully');
        this.showForm = false;
        this.partyForm.reset();
        this.loadParties();
      },
      error: () => {
        // Error handled by interceptor
      },
    });
  }

  onDelete(party: Party): void {
    if (confirm(`Are you sure you want to delete party "${party.partyName}"?`)) {
      this.partyService.deleteParty(party.partyName).subscribe({
        next: () => {
          this.notificationService.success('Party deleted successfully');
          this.loadParties();
        },
        error: () => {
          // Error handled by interceptor
        },
      });
    }
  }
}
