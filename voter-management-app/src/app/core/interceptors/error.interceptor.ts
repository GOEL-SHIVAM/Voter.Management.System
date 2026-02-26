import { HttpInterceptorFn, HttpErrorResponse } from '@angular/common/http';
import { inject } from '@angular/core';
import { catchError, throwError } from 'rxjs';
import { Router } from '@angular/router';
import { AuthService } from '../services/auth.service';
import { NotificationService } from '../services/notification.service';

export const errorInterceptor: HttpInterceptorFn = (req, next) => {
  const router = inject(Router);
  const authService = inject(AuthService);
  const notificationService = inject(NotificationService);

  return next(req).pipe(
    catchError((error: HttpErrorResponse) => {
      let errorMessage = 'An unexpected error occurred';

      if (error.error instanceof ErrorEvent) {
        // Client-side error
        errorMessage = `Error: ${error.error.message}`;
      } else {
        // Server-side error
        switch (error.status) {
          case 400:
            // Validation errors
            if (error.error?.errors) {
              const validationErrors = error.error.errors
                .map((e: any) => e.error || e.message)
                .join(', ');
              errorMessage = validationErrors;
            } else if (error.error?.message) {
              errorMessage = error.error.message;
            } else {
              errorMessage = 'Bad request';
            }
            break;

          case 401:
            errorMessage = 'Unauthorized. Please login again.';
            authService.logout();
            router.navigate(['/login']);
            break;

          case 403:
            errorMessage = 'Access denied';
            router.navigate(['/unauthorized']);
            break;

          case 404:
            errorMessage = error.error?.message || 'Resource not found';
            break;

          case 500:
            errorMessage = 'Internal server error. Please try again later.';
            break;

          default:
            errorMessage = error.error?.message || `Error: ${error.status}`;
        }
      }

      notificationService.error(errorMessage);
      return throwError(() => new Error(errorMessage));
    }),
  );
};
