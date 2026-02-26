import { Component, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { Router, RouterModule } from '@angular/router';
import { MatCardModule } from '@angular/material/card';
import { MatButtonModule } from '@angular/material/button';
import { MatIconModule } from '@angular/material/icon';
import { MatGridListModule } from '@angular/material/grid-list';
import { AuthService } from '../../../core/services/auth.service';

@Component({
  selector: 'app-admin-dashboard',
  standalone: true,
  imports: [
    CommonModule,
    RouterModule,
    MatCardModule,
    MatButtonModule,
    MatIconModule,
    MatGridListModule,
  ],
  templateUrl: './dashboard.html',
  styleUrl: './dashboard.scss',
})
export class AdminDashboardComponent implements OnInit {
  currentUser: any;
  isSuperAdmin = false;

  menuItems = [
    {
      title: 'Voter Management',
      icon: 'people',
      route: '/admin/voters',
      description: 'Manage voter registrations and details',
      color: '#3f51b5',
    },
    {
      title: 'Party Management',
      icon: 'account_balance',
      route: '/admin/parties',
      description: 'Manage political parties',
      color: '#e91e63',
    },
    {
      title: 'Election Management',
      icon: 'how_to_vote',
      route: '/admin/elections',
      description: 'Create and manage elections',
      color: '#00bcd4',
    },
  ];

  constructor(
    private authService: AuthService,
    private router: Router,
  ) {
    this.currentUser = this.authService.currentUserValue;
  }

  ngOnInit(): void {
    this.isSuperAdmin = this.currentUser?.role === 'SuperAdmin';

    if (this.isSuperAdmin) {
      this.menuItems.push({
        title: 'Admin Management',
        icon: 'admin_panel_settings',
        route: '/admin/admins',
        description: 'Manage administrator accounts',
        color: '#ff9800',
      });
    }
  }

  navigate(route: string): void {
    this.router.navigate([route]);
  }
}
