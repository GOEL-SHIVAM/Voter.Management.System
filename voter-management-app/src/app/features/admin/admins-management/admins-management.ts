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
import { MatChipsModule } from '@angular/material/chips';
import { AdminService } from '../../../core/services/admin.service';
import { NotificationService } from '../../../core/services/notification.service';
import { AuthService } from '../../../core/services/auth.service';
import { Admin, CreateAdminRequest } from '../../../core/models/admin.model';
import { MatTooltipModule } from '@angular/material/tooltip';

@Component({
  selector: 'app-admins-management',
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
    MatChipsModule,
    MatTooltipModule,
  ],
  templateUrl: './admins-management.html',
  styleUrl: './admins-management.scss',
})
export class AdminsManagementComponent implements OnInit {
  admins: Admin[] = [];
  displayedColumns: string[] = ['adminId', 'username', 'role', 'actions'];
  isLoading = false;
  showForm = false;
  adminForm!: FormGroup;
  currentUser: any;

  constructor(
    private adminService: AdminService,
    private notificationService: NotificationService,
    private authService: AuthService,
    private fb: FormBuilder,
  ) {
    this.currentUser = this.authService.currentUserValue;
  }

  ngOnInit(): void {
    this.initForm();
    this.loadAdmins();
  }

  initForm(): void {
    this.adminForm = this.fb.group({
      username: [
        '',
        [
          Validators.required,
          Validators.minLength(3),
          Validators.maxLength(50),
          Validators.pattern(/^[a-zA-Z0-9_]+$/),
        ],
      ],
      password: ['', [Validators.required, Validators.minLength(6)]],
    });
  }

  loadAdmins(): void {
    this.isLoading = true;
    this.adminService.getAllAdmins().subscribe({
      next: (admins) => {
        this.admins = admins;
        this.isLoading = false;
      },
      error: () => {
        this.isLoading = false;
      },
    });
  }

  onAdd(): void {
    this.showForm = true;
    this.adminForm.reset();
  }

  onCancel(): void {
    this.showForm = false;
    this.adminForm.reset();
  }

  onSubmit(): void {
    if (this.adminForm.invalid) {
      Object.keys(this.adminForm.controls).forEach((key) => {
        this.adminForm.get(key)?.markAsTouched();
      });
      return;
    }

    const request: CreateAdminRequest = this.adminForm.value;

    this.adminService.createAdmin(request).subscribe({
      next: () => {
        this.notificationService.success('Admin created successfully');
        this.showForm = false;
        this.adminForm.reset();
        this.loadAdmins();
      },
      error: () => {
        // Error handled by interceptor
      },
    });
  }

  onDelete(admin: Admin): void {
    if (admin.username === 'flyxz') {
      this.notificationService.error('Cannot delete super admin account');
      return;
    }

    if (admin.username === this.currentUser?.username) {
      this.notificationService.error('Cannot delete your own account');
      return;
    }

    if (confirm(`Are you sure you want to delete admin "${admin.username}"?`)) {
      this.adminService.deleteAdmin(admin.username).subscribe({
        next: () => {
          this.notificationService.success('Admin deleted successfully');
          this.loadAdmins();
        },
        error: () => {
          // Error handled by interceptor
        },
      });
    }
  }

  canDelete(admin: Admin): boolean {
    return admin.username !== 'flyxz' && admin.username !== this.currentUser?.username;
  }
}
