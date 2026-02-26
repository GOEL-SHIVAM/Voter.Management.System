import { Routes } from '@angular/router';
import { authGuard } from './core/guards/auth.guard';
import { adminGuard } from './core/guards/admin.guard';
import { voterGuard } from './core/guards/voter.guard';
import { superAdminGuard } from './core/guards/super-admin.guard';

export const routes: Routes = [
  {
    path: '',
    redirectTo: '/login',
    pathMatch: 'full',
  },
  {
    path: 'login',
    loadComponent: () => import('./features/auth/login/login').then((m) => m.LoginComponent),
  },
  {
    path: 'register',
    loadComponent: () =>
      import('./features/auth/register/register').then((m) => m.RegisterComponent),
  },
  {
    path: 'admin',
    canActivate: [authGuard, adminGuard],
    children: [
      {
        path: '',
        redirectTo: 'dashboard',
        pathMatch: 'full',
      },
      {
        path: 'dashboard',
        loadComponent: () =>
          import('./features/admin/dashboard/dashboard').then((m) => m.AdminDashboardComponent),
      },
      {
        path: 'voters',
        loadComponent: () =>
          import('./features/admin/voters-management/voters-management').then(
            (m) => m.VotersManagementComponent,
          ),
      },
      {
        path: 'parties',
        loadComponent: () =>
          import('./features/admin/parties-management/parties-management').then(
            (m) => m.PartiesManagementComponent,
          ),
      },
      {
        path: 'elections',
        loadComponent: () =>
          import('./features/admin/elections-management/elections-management').then(
            (m) => m.ElectionsManagementComponent,
          ),
      },
      {
        path: 'admins',
        canActivate: [superAdminGuard],
        loadComponent: () =>
          import('./features/admin/admins-management/admins-management').then(
            (m) => m.AdminsManagementComponent,
          ),
      },
    ],
  },
  {
    path: 'voter',
    canActivate: [authGuard, voterGuard],
    children: [
      {
        path: '',
        redirectTo: 'dashboard',
        pathMatch: 'full',
      },
      {
        path: 'dashboard',
        loadComponent: () =>
          import('./features/voter/dashboard/dashboard').then((m) => m.VoterDashboardComponent),
      },
      {
        path: 'cast-vote',
        loadComponent: () =>
          import('./features/voter/cast-vote/cast-vote').then((m) => m.CastVoteComponent),
      },
      {
        path: 'history',
        loadComponent: () =>
          import('./features/voter/vote-history/vote-history').then((m) => m.VoteHistoryComponent),
      },
      {
        path: 'change-password',
        loadComponent: () =>
          import('./features/voter/change-password/change-password').then(
            (m) => m.ChangePasswordComponent,
          ),
      },
    ],
  },
  {
    path: 'results/:electionCode',
    loadComponent: () =>
      import('./features/public/results/results').then((m) => m.ResultsComponent),
  },
  {
    path: 'unauthorized',
    loadComponent: () =>
      import('./features/public/unauthorized/unauthorized').then((m) => m.UnauthorizedComponent),
  },
  {
    path: '**',
    redirectTo: '/login',
  },
];
