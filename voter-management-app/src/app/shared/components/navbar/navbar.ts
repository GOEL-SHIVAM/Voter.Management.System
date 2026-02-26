import { Component } from '@angular/core';
import { CommonModule } from '@angular/common';
import { Router, RouterModule } from '@angular/router';
import { MatToolbarModule } from '@angular/material/toolbar';
import { MatButtonModule } from '@angular/material/button';
import { MatIconModule } from '@angular/material/icon';
import { MatMenuModule } from '@angular/material/menu';
import { AuthService } from '../../../core/services/auth.service';

@Component({
  selector: 'app-navbar',
  standalone: true,
  imports: [
    CommonModule,
    RouterModule,
    MatToolbarModule,
    MatButtonModule,
    MatIconModule,
    MatMenuModule,
  ],
  templateUrl: './navbar.html',
  styleUrl: './navbar.scss',
})
export class NavbarComponent {
  currentUser: any;

  constructor(
    private authService: AuthService,
    private router: Router,
  ) {
    this.currentUser = this.authService.currentUserValue;
  }

  getHomeRoute(): string {
    const role = this.currentUser?.role;
    if (role === 'Admin' || role === 'SuperAdmin') {
      return '/admin/dashboard';
    } else if (role === 'Voter') {
      return '/voter/dashboard';
    }
    return '/login';
  }

  logout(): void {
    this.authService.logout();
  }
}
