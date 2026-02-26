import { Component, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormBuilder, FormGroup, Validators, ReactiveFormsModule } from '@angular/forms';
import { Router, RouterModule } from '@angular/router';
import { MatCardModule } from '@angular/material/card';
import { MatFormFieldModule } from '@angular/material/form-field';
import { MatInputModule } from '@angular/material/input';
import { MatButtonModule } from '@angular/material/button';
import { MatSelectModule } from '@angular/material/select';
import { MatIconModule } from '@angular/material/icon';
import { AuthService } from '../../../core/services/auth.service';
import { NotificationService } from '../../../core/services/notification.service';
import { LoginRequest } from '../../../core/models/auth.model';

@Component({
  selector: 'app-login',
  standalone: true,
  imports: [
    CommonModule,
    ReactiveFormsModule,
    RouterModule,
    MatCardModule,
    MatFormFieldModule,
    MatInputModule,
    MatButtonModule,
    MatSelectModule,
    MatIconModule,
  ],
  templateUrl: './login.html',
  styleUrl: './login.scss',
})
export class LoginComponent implements OnInit {
  loginForm!: FormGroup;
  hidePassword = true;
  isLoading = false;
  userType: 'admin' | 'voter' = 'admin';

  constructor(
    private fb: FormBuilder,
    private authService: AuthService,
    private router: Router,
    private notificationService: NotificationService,
  ) {}

  ngOnInit(): void {
    this.loginForm = this.fb.group({
      username: ['', [Validators.required, Validators.minLength(3)]],
      password: ['', [Validators.required, Validators.minLength(6)]],
    });
  }

  onUserTypeChange(type: 'admin' | 'voter'): void {
    this.userType = type;
    this.loginForm.reset();
  }

  onSubmit(): void {
    if (this.loginForm.invalid) {
      Object.keys(this.loginForm.controls).forEach((key) => {
        this.loginForm.get(key)?.markAsTouched();
      });
      return;
    }

    this.isLoading = true;
    const request: LoginRequest = this.loginForm.value;

    const loginObservable =
      this.userType === 'admin'
        ? this.authService.adminLogin(request)
        : this.authService.voterLogin(request);

    loginObservable.subscribe({
      next: (response) => {
        this.notificationService.success('Login successful!');

        // Redirect based on role
        if (response.role === 'Admin' || response.role === 'SuperAdmin') {
          this.router.navigate(['/admin/dashboard']);
        } else if (response.role === 'Voter') {
          this.router.navigate(['/voter/dashboard']);
        }
      },
      error: (error) => {
        this.isLoading = false;
        this.notificationService.error('Invalid username or password');
      },
      complete: () => {
        this.isLoading = false;
      },
    });
  }

  get username() {
    return this.loginForm.get('username');
  }
  get password() {
    return this.loginForm.get('password');
  }
}
