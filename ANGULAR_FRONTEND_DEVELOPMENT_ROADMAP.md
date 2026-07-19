# ?? **ANGULAR + ANT DESIGN RCM PORTAL - COMPLETE FRONTEND DEVELOPMENT ROADMAP**

## ? **SINGLE MASTER DEVELOPMENT GUIDE**

This is your **ONE comprehensive guide** for building the complete Angular + Ant Design RCM Portal frontend integrated with your .NET 9 backend.

---

## ?? **TABLE OF CONTENTS**

1. [Pre-Development Setup](#1-pre-development-setup)
2. [Phase 1: Project Foundation (Week 1)](#phase-1-project-foundation-week-1)
3. [Phase 2: Architecture & Core Setup (Week 1)](#phase-2-architecture--core-setup-week-1)
4. [Phase 3: Authentication Module (Week 1-2)](#phase-3-authentication-module-week-1-2)
5. [Phase 4: Dashboard & Layout (Week 2)](#phase-4-dashboard--layout-week-2)
6. [Phase 5: Claims Management Module (Week 2-3)](#phase-5-claims-management-module-week-2-3)
7. [Phase 6: Eligibility Verification (Week 3)](#phase-6-eligibility-verification-week-3)
8. [Phase 7: Pre-Authorization (Week 3-4)](#phase-7-pre-authorization-week-3-4)
9. [Phase 8: Reports & Analytics (Week 4)](#phase-8-reports--analytics-week-4)
10. [Phase 9: Testing & Optimization (Week 4-5)](#phase-9-testing--optimization-week-4-5)
11. [Backend Integration Guide](#backend-integration-guide)
12. [Deployment & Production](#deployment--production)

---

# 1. PRE-DEVELOPMENT SETUP

## ? System Verification (5 minutes)

```bash
# Verify all prerequisites
nvm --version           # Should show 1.1.x
nvm list        # Should show Node 16 & 18
node --version        # Should show v18.x.x
npm --version           # Should show 9.x.x
ng version         # Should show Angular 18.x.x
git status     # Should show branch main
```

## ?? Create Backend Integration Configuration

Your backend projects to integrate with:
```
.NET 9 Projects:
?? NPhies_FHIR_Integration.ApiService (Main API)
?? NPhies_FHIR_Integration.Application (Business Logic)
?? NPhies_FHIR_Integration.Domain (Models)
?? NPhies_FHIR_Integration.Infrastructure (Data Access)
?? NPhies_FHIR_Integration.Common (Utilities)

Backend API Endpoints (Will integrate):
?? /api/auth/login
?? /api/auth/logout
?? /api/claims
?? /api/eligibility
?? /api/pre-auth
?? /api/reports
```

---

# PHASE 1: PROJECT FOUNDATION (WEEK 1)

## 1.1 Create Angular Project with Ant Design

**Duration**: 10 minutes

```bash
# Step 1: Create project
ng new rcm-portal-antd \
  --routing \
  --style=less \
  --package-manager=npm

# Step 2: Navigate to project
cd rcm-portal-antd

# Step 3: Create .nvmrc for automatic version switching
echo "18" > .nvmrc

# Step 4: Verify Node version switching
nvm use
node --version  # Should be v18.x.x

# Step 5: Add Ant Design
ng add ng-zorro-antd

# Step 6: Install additional dependencies
npm install \
  chart.js \
  ng2-charts \
  moment \
  ngx-moment \
  lodash-es \
  rxjs-compat \
  --save

# Step 7: Install dev dependencies
npm install \
  @types/lodash-es \
  --save-dev

# Step 8: Verify installation
ng version
npm list @angular/core
```

## 1.2 Project Structure Setup

```bash
# Create comprehensive folder structure
mkdir -p src/app/core/services
mkdir -p src/app/core/interceptors
mkdir -p src/app/core/guards
mkdir -p src/app/core/models
mkdir -p src/app/core/constants

mkdir -p src/app/shared/components/navbar
mkdir -p src/app/shared/components/sidebar
mkdir -p src/app/shared/components/footer
mkdir -p src/app/shared/pipes
mkdir -p src/app/shared/directives

mkdir -p src/app/features/dashboard
mkdir -p src/app/features/claims/components
mkdir -p src/app/features/claims/services
mkdir -p src/app/features/eligibility/components
mkdir -p src/app/features/eligibility/services
mkdir -p src/app/features/pre-auth/components
mkdir -p src/app/features/pre-auth/services
mkdir -p src/app/features/reports/components
mkdir -p src/app/features/reports/services

mkdir -p src/app/auth/components
mkdir -p src/app/auth/services

mkdir -p src/environments
mkdir -p src/assets/images
mkdir -p src/assets/styles
```

## 1.3 Generate Angular Modules

```bash
# Generate feature modules
ng generate module core --routing
ng generate module shared
ng generate module features/dashboard --routing
ng generate module features/claims --routing
ng generate module features/eligibility --routing
ng generate module features/pre-auth --routing
ng generate module features/reports --routing
ng generate module auth --routing

# Generate key components
ng generate component shared/components/navbar --skip-tests
ng generate component shared/components/sidebar --skip-tests
ng generate component shared/components/footer --skip-tests
ng generate component auth/components/login --skip-tests
ng generate component auth/components/logout --skip-tests
```

## 1.4 Environment Configuration

**File**: `src/environments/environment.ts`

```typescript
export const environment = {
  production: false,
  apiUrl: 'http://localhost:5000/api',  // Your .NET 9 API
  apiTimeout: 30000,
  
  // API Endpoints
  endpoints: {
    auth: {
 login: '/auth/login',
 logout: '/auth/logout',
      refresh: '/auth/refresh-token'
    },
    claims: {
      list: '/claims',
      create: '/claims/submit',
      getById: '/claims',
      update: '/claims',
      getStatus: '/claims/status'
    },
  eligibility: {
  verify: '/eligibility/verify',
      getHistory: '/eligibility/history'
    },
    preAuth: {
      request: '/pre-auth/request',
  getStatus: '/pre-auth/status'
    },
    reports: {
      claims: '/reports/claims',
      eligibility: '/reports/eligibility'
    }
  },
  
  // Application settings
  app: {
    name: 'RCM Portal',
    version: '1.0.0',
    tokenKey: 'rcm_token',
    userKey: 'rcm_user'
  }
};
```

**File**: `src/environments/environment.prod.ts`

```typescript
export const environment = {
  production: true,
  apiUrl: 'https://api.yourdomain.com/api',
  apiTimeout: 30000,
  
  // Same endpoints structure as above
  endpoints: {
    // ... same structure
  },
  
  app: {
    name: 'RCM Portal',
    version: '1.0.0',
    tokenKey: 'rcm_token',
    userKey: 'rcm_user'
  }
};
```

## 1.5 Global Theme Configuration

**File**: `src/theme.less`

```less
// Ant Design Color Customization for Healthcare/RCM

// Primary Colors
@primary-color: #0066cc;     // Medical blue
@success-color: #00a854;  // Approved/Success green
@warning-color: #faad14;           // Pending/Warning yellow
@error-color: #ff4d4f;             // Denied/Error red
@info-color: #1890ff;       // Information blue

// RCM Specific Colors
@claims-approved: #00a854;
@claims-pending: #faad14;
@claims-denied: #ff4d4f;
@claims-submitted: #1890ff;

// Text Colors
@text-color: rgba(0, 0, 0, 0.85);
@text-color-secondary: rgba(0, 0, 0, 0.65);

// Border
@border-radius-base: 2px;
@border-color-base: #d9d9d9;

// Fonts
@font-family-base: -apple-system, BlinkMacSystemFont, 'Segoe UI', Roboto, 'Helvetica Neue', Arial, sans-serif;
@font-size-base: 14px;

// Breakpoints
@screen-xs: 480px;
@screen-sm: 576px;
@screen-md: 768px;
@screen-lg: 992px;
@screen-xl: 1200px;
@screen-xxl: 1600px;
```

## 1.6 First Commit

```bash
# Initialize project structure
git add .
git commit -m "feat: Initialize Angular 18 + Ant Design RCM Portal project structure"
git push origin main
```

---

# PHASE 2: ARCHITECTURE & CORE SETUP (WEEK 1)

## 2.1 Core Models (TypeScript Interfaces)

**File**: `src/app/core/models/user.model.ts`

```typescript
export interface User {
  id: string;
  username: string;
  email: string;
  firstName: string;
  lastName: string;
  role: UserRole;
  permissions: string[];
  isActive: boolean;
  createdAt: Date;
  lastLogin?: Date;
}

export enum UserRole {
  ADMIN = 'admin',
  MANAGER = 'manager',
  OPERATOR = 'operator',
  VIEWER = 'viewer'
}

export interface AuthToken {
  accessToken: string;
  refreshToken: string;
  expiresIn: number;
  tokenType: string;
}
```

**File**: `src/app/core/models/claim.model.ts`

```typescript
export interface Claim {
  id: string;
  claimNumber: string;
  patientId: string;
  patientName: string;
  amount: number;
  serviceDate: Date;
  submissionDate: Date;
  status: ClaimStatus;
  diagnosisCode: string;
  procedureCode: string;
  providerId: string;
  providerName: string;
  insurerId: string;
  insurerName: string;
  notes?: string;
  attachments?: Attachment[];
  createdAt: Date;
  updatedAt: Date;
  createdBy: string;
  updatedBy: string;
}

export enum ClaimStatus {
  DRAFT = 'draft',
  SUBMITTED = 'submitted',
  PROCESSING = 'processing',
  APPROVED = 'approved',
  DENIED = 'denied',
  APPEALED = 'appealed',
  CLOSED = 'closed'
}

export interface Attachment {
  id: string;
  fileName: string;
  fileType: string;
  fileSize: number;
  uploadDate: Date;
  url: string;
}

export interface ClaimFilter {
  status?: ClaimStatus;
  providerId?: string;
  dateFrom?: Date;
  dateTo?: Date;
  searchTerm?: string;
  page?: number;
  pageSize?: number;
}

export interface ClaimResponse {
  data: Claim[];
  total: number;
  page: number;
  pageSize: number;
  hasMore: boolean;
}
```

**File**: `src/app/core/models/eligibility.model.ts`

```typescript
export interface EligibilityRequest {
  memberId: string;
  firstName: string;
  lastName: string;
  dateOfBirth: Date;
  insurerId: string;
}

export interface EligibilityResponse {
  memberId: string;
  isEligible: boolean;
  planName: string;
  groupNumber: string;
  effectiveDate: Date;
  terminationDate?: Date;
  deductible: number;
  copay: number;
  coinsurance: number;
  outOfPocketMax: number;
  verificationDate: Date;
}

export interface EligibilityHistory {
  id: string;
  memberId: string;
  requestDate: Date;
  response: EligibilityResponse;
  verifiedBy: string;
}
```

## 2.2 API Service (Base HTTP Service)

**File**: `src/app/core/services/api.service.ts`

```typescript
import { Injectable } from '@angular/core';
import { HttpClient, HttpParams, HttpErrorResponse } from '@angular/common/http';
import { Observable, throwError } from 'rxjs';
import { catchError, timeout } from 'rxjs/operators';
import { environment } from 'src/environments/environment';
import { NzMessageService } from 'ng-zorro-antd/message';

@Injectable({
  providedIn: 'root'
})
export class ApiService {
  private apiUrl = environment.apiUrl;
  private apiTimeout = environment.apiTimeout;

  constructor(
    private http: HttpClient,
    private messageService: NzMessageService
  ) { }

  /**
   * Generic GET request
   */
  get<T>(endpoint: string, params?: HttpParams | any): Observable<T> {
const url = `${this.apiUrl}${endpoint}`;
    return this.http.get<T>(url, { params }).pipe(
      timeout(this.apiTimeout),
      catchError(error => this.handleError(error))
    );
  }

  /**
   * Generic POST request
   */
  post<T>(endpoint: string, data: any): Observable<T> {
    const url = `${this.apiUrl}${endpoint}`;
    return this.http.post<T>(url, data).pipe(
      timeout(this.apiTimeout),
      catchError(error => this.handleError(error))
    );
  }

  /**
   * Generic PUT request
   */
  put<T>(endpoint: string, id: string, data: any): Observable<T> {
    const url = `${this.apiUrl}${endpoint}/${id}`;
    return this.http.put<T>(url, data).pipe(
      timeout(this.apiTimeout),
      catchError(error => this.handleError(error))
    );
  }

  /**
   * Generic DELETE request
   */
  delete<T>(endpoint: string, id: string): Observable<T> {
    const url = `${this.apiUrl}${endpoint}/${id}`;
    return this.http.delete<T>(url).pipe(
      timeout(this.apiTimeout),
  catchError(error => this.handleError(error))
  );
  }

  /**
   * Error handling
   */
  private handleError(error: HttpErrorResponse) {
  let errorMessage = 'An error occurred';

    if (error.error instanceof ErrorEvent) {
   // Client error
      errorMessage = `Error: ${error.error.message}`;
    } else {
      // Server error
      errorMessage = error.error?.message || `Server Error: ${error.status}`;
    }

    console.error(errorMessage);
    this.messageService.error(errorMessage);
    return throwError(() => new Error(errorMessage));
  }
}
```

## 2.3 Authentication Service

**File**: `src/app/core/services/auth.service.ts`

```typescript
import { Injectable } from '@angular/core';
import { BehaviorSubject, Observable, throwError } from 'rxjs';
import { tap, catchError } from 'rxjs/operators';
import { ApiService } from './api.service';
import { User, AuthToken } from '../models/user.model';
import { environment } from 'src/environments/environment';

@Injectable({
  providedIn: 'root'
})
export class AuthService {
  private currentUserSubject = new BehaviorSubject<User | null>(null);
  public currentUser$ = this.currentUserSubject.asObservable();

  constructor(private apiService: ApiService) {
    this.loadUserFromStorage();
  }

  /**
   * Load user from localStorage on app init
   */
  private loadUserFromStorage(): void {
    const user = localStorage.getItem(environment.app.userKey);
    if (user) {
      try {
   this.currentUserSubject.next(JSON.parse(user));
      } catch (error) {
console.error('Error parsing stored user', error);
  this.clearStorage();
      }
    }
  }

  /**
   * Login user
 */
  login(username: string, password: string): Observable<any> {
    return this.apiService.post(
   environment.endpoints.auth.login,
      { username, password }
    ).pipe(
      tap(response => {
        this.setAuthData(response);
      }),
      catchError(error => {
        console.error('Login error', error);
        return throwError(() => error);
    })
    );
  }

  /**
   * Set authentication data
   */
  private setAuthData(response: any): void {
    const token: AuthToken = response.token;
    const user: User = response.user;

    localStorage.setItem(environment.app.tokenKey, token.accessToken);
    localStorage.setItem(environment.app.userKey, JSON.stringify(user));
this.currentUserSubject.next(user);
  }

  /**
   * Logout user
   */
  logout(): Observable<any> {
    return this.apiService.post(environment.endpoints.auth.logout, {}).pipe(
      tap(() => {
        this.clearStorage();
      }),
      catchError(error => {
   // Clear storage even if API call fails
        this.clearStorage();
        return throwError(() => error);
      })
    );
  }

  /**
   * Clear authentication data
   */
  private clearStorage(): void {
    localStorage.removeItem(environment.app.tokenKey);
    localStorage.removeItem(environment.app.userKey);
    this.currentUserSubject.next(null);
  }

  /**
   * Get current user
   */
  getCurrentUser(): User | null {
    return this.currentUserSubject.value;
  }

  /**
   * Check if user is authenticated
   */
  isAuthenticated(): boolean {
    return !!localStorage.getItem(environment.app.tokenKey);
  }

  /**
   * Get auth token
   */
  getAuthToken(): string | null {
    return localStorage.getItem(environment.app.tokenKey);
  }

  /**
   * Check if user has permission
   */
  hasPermission(permission: string): boolean {
    const user = this.getCurrentUser();
    return user?.permissions?.includes(permission) ?? false;
  }

  /**
   * Check if user has role
   */
  hasRole(role: string): boolean {
    const user = this.getCurrentUser();
    return user?.role === role;
  }
}
```

## 2.4 JWT Interceptor

**File**: `src/app/core/interceptors/jwt.interceptor.ts`

```typescript
import { Injectable } from '@angular/core';
import { HttpInterceptor, HttpRequest, HttpHandler, HttpEvent } from '@angular/common/http';
import { Observable } from 'rxjs';
import { AuthService } from '../services/auth.service';
import { environment } from 'src/environments/environment';

@Injectable()
export class JwtInterceptor implements HttpInterceptor {
  constructor(private authService: AuthService) { }

  intercept(
    request: HttpRequest<any>,
    next: HttpHandler
  ): Observable<HttpEvent<any>> {
    const token = this.authService.getAuthToken();

    // Add token to requests if available
    if (token) {
      request = request.clone({
        setHeaders: {
          Authorization: `Bearer ${token}`,
       'Content-Type': 'application/json'
      }
});
    } else {
      request = request.clone({
        setHeaders: {
        'Content-Type': 'application/json'
      }
      });
    }

    return next.handle(request);
  }
}
```

## 2.5 Error Interceptor

**File**: `src/app/core/interceptors/error.interceptor.ts`

```typescript
import { Injectable } from '@angular/core';
import { HttpInterceptor, HttpRequest, HttpHandler, HttpEvent, HttpErrorResponse } from '@angular/common/http';
import { Observable, throwError } from 'rxjs';
import { catchError } from 'rxjs/operators';
import { AuthService } from '../services/auth.service';
import { Router } from '@angular/router';
import { NzMessageService } from 'ng-zorro-antd/message';

@Injectable()
export class ErrorInterceptor implements HttpInterceptor {
  constructor(
    private authService: AuthService,
    private router: Router,
    private messageService: NzMessageService
  ) { }

  intercept(
    request: HttpRequest<any>,
    next: HttpHandler
  ): Observable<HttpEvent<any>> {
    return next.handle(request).pipe(
      catchError((error: HttpErrorResponse) => {
        if (error.status === 401) {
     // Unauthorized - logout user
       this.authService.logout().subscribe(() => {
            this.router.navigate(['/auth/login']);
    this.messageService.error('Session expired. Please login again.');
       });
        } else if (error.status === 403) {
     // Forbidden
          this.messageService.error('You do not have permission to access this resource.');
      this.router.navigate(['/unauthorized']);
        } else if (error.status === 404) {
    // Not found
     this.messageService.error('Resource not found.');
        } else if (error.status >= 500) {
      // Server error
          this.messageService.error('Server error. Please try again later.');
    } else if (error.error?.message) {
          // Custom error message from server
     this.messageService.error(error.error.message);
      }

return throwError(() => error);
      })
    );
  }
}
```

## 2.6 Auth Guard

**File**: `src/app/core/guards/auth.guard.ts`

```typescript
import { Injectable } from '@angular/core';
import { CanActivate, ActivatedRouteSnapshot, RouterStateSnapshot, Router } from '@angular/router';
import { AuthService } from '../services/auth.service';

@Injectable({
  providedIn: 'root'
})
export class AuthGuard implements CanActivate {
  constructor(
    private authService: AuthService,
    private router: Router
  ) { }

  canActivate(
    route: ActivatedRouteSnapshot,
    state: RouterStateSnapshot
  ): boolean {
    if (this.authService.isAuthenticated()) {
      // Check permissions if required
      const requiredPermissions = route.data['permissions'] as string[];
      if (requiredPermissions && requiredPermissions.length > 0) {
        const hasPermission = requiredPermissions.some(p => 
   this.authService.hasPermission(p)
        );
        
        if (!hasPermission) {
       this.router.navigate(['/unauthorized']);
  return false;
     }
      }
      return true;
    }

    // Store intended destination and redirect to login
    sessionStorage.setItem('redirectUrl', state.url);
    this.router.navigate(['/auth/login']);
    return false;
  }
}
```

## 2.7 App Routing Configuration

**File**: `src/app/app-routing.module.ts`

```typescript
import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { AuthGuard } from './core/guards/auth.guard';

const routes: Routes = [
  {
    path: '',
    redirectTo: '/dashboard',
    pathMatch: 'full'
  },
  {
    path: 'auth',
    loadChildren: () => import('./auth/auth.module').then(m => m.AuthModule)
  },
  {
    path: 'dashboard',
    loadChildren: () => import('./features/dashboard/dashboard.module').then(m => m.DashboardModule),
    canActivate: [AuthGuard]
},
  {
    path: 'claims',
    loadChildren: () => import('./features/claims/claims.module').then(m => m.ClaimsModule),
    canActivate: [AuthGuard],
    data: { permissions: ['claim:view', 'claim:manage'] }
  },
  {
    path: 'eligibility',
    loadChildren: () => import('./features/eligibility/eligibility.module').then(m => m.EligibilityModule),
    canActivate: [AuthGuard],
    data: { permissions: ['eligibility:verify'] }
  },
  {
  path: 'pre-auth',
    loadChildren: () => import('./features/pre-auth/pre-auth.module').then(m => m.PreAuthModule),
    canActivate: [AuthGuard],
    data: { permissions: ['preauth:manage'] }
  },
  {
    path: 'reports',
    loadChildren: () => import('./features/reports/reports.module').then(m => m.ReportsModule),
    canActivate: [AuthGuard],
data: { permissions: ['reports:view'] }
  },
  {
    path: 'unauthorized',
  component: () => { /* Unauthorized component */ }
  },
  {
    path: '**',
    redirectTo: '/dashboard'
  }
];

@NgModule({
  imports: [RouterModule.forRoot(routes)],
  exports: [RouterModule]
})
export class AppRoutingModule { }
```

## 2.8 App Module Configuration

**File**: `src/app/app.module.ts`

```typescript
import { NgModule } from '@angular/core';
import { BrowserModule } from '@angular/platform-browser';
import { BrowserAnimationsModule } from '@angular/platform-browser/animations';
import { HttpClientModule, HTTP_INTERCEPTORS } from '@angular/common/http';
import { FormsModule, ReactiveFormsModule } from '@angular/forms';

import { NzLayoutModule } from 'ng-zorro-antd/layout';
import { NzMenuModule } from 'ng-zorro-antd/menu';
import { NzFormModule } from 'ng-zorro-antd/form';
import { NzButtonModule } from 'ng-zorro-antd/button';
import { NzMessageModule } from 'ng-zorro-antd/message';
import { NzModalModule } from 'ng-zorro-antd/modal';

import { AppRoutingModule } from './app-routing.module';
import { AppComponent } from './app.component';
import { JwtInterceptor } from './core/interceptors/jwt.interceptor';
import { ErrorInterceptor } from './core/interceptors/error.interceptor';
import { CoreModule } from './core/core.module';
import { SharedModule } from './shared/shared.module';

@NgModule({
  declarations: [AppComponent],
  imports: [
    BrowserModule,
    BrowserAnimationsModule,
    HttpClientModule,
    FormsModule,
    ReactiveFormsModule,
    AppRoutingModule,
    CoreModule,
 SharedModule,
    
    // Ant Design Modules
    NzLayoutModule,
    NzMenuModule,
    NzFormModule,
    NzButtonModule,
    NzMessageModule,
    NzModalModule
  ],
  providers: [
    { provide: HTTP_INTERCEPTORS, useClass: JwtInterceptor, multi: true },
    { provide: HTTP_INTERCEPTORS, useClass: ErrorInterceptor, multi: true }
  ],
  bootstrap: [AppComponent]
})
export class AppModule { }
```

## 2.9 Commit Core Architecture

```bash
git add .
git commit -m "feat: Setup core architecture with services, models, and interceptors"
git push origin main
```

---

# PHASE 3: AUTHENTICATION MODULE (WEEK 1-2)

## 3.1 Login Component

**File**: `src/app/auth/components/login/login.component.ts`

```typescript
import { Component, OnInit } from '@angular/core';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { Router } from '@angular/router';
import { AuthService } from 'src/app/core/services/auth.service';
import { NzMessageService } from 'ng-zorro-antd/message';

@Component({
  selector: 'app-login',
  templateUrl: './login.component.html',
  styleUrls: ['./login.component.less']
})
export class LoginComponent implements OnInit {
  loginForm: FormGroup;
  loading = false;
  showPassword = false;

  constructor(
private fb: FormBuilder,
    private authService: AuthService,
    private router: Router,
    private messageService: NzMessageService
  ) {
    this.loginForm = this.fb.group({
      username: ['', [Validators.required, Validators.minLength(3)]],
      password: ['', [Validators.required, Validators.minLength(6)]]
    });
  }

  ngOnInit(): void {
    // Redirect if already logged in
    if (this.authService.isAuthenticated()) {
  this.router.navigate(['/dashboard']);
    }
  }

  login(): void {
    if (!this.loginForm.valid) {
 this.messageService.error('Please fill all required fields');
      return;
    }

    this.loading = true;
  const { username, password } = this.loginForm.value;

    this.authService.login(username, password).subscribe({
      next: (response) => {
      this.loading = false;
        this.messageService.success('Login successful!');
 
        // Redirect to intended destination or dashboard
      const redirectUrl = sessionStorage.getItem('redirectUrl') || '/dashboard';
      sessionStorage.removeItem('redirectUrl');
        this.router.navigate([redirectUrl]);
      },
  error: (error) => {
        this.loading = false;
        console.error('Login error:', error);
      }
    });
  }

  togglePasswordVisibility(): void {
    this.showPassword = !this.showPassword;
  }
}
```

**File**: `src/app/auth/components/login/login.component.html`

```html
<div class="login-container">
  <div class="login-box">
    <div class="login-header">
      <h1>RCM Portal</h1>
  <p>Revenue Cycle Management System</p>
    </div>

    <form nz-form [formGroup]="loginForm" (ngSubmit)="login()">
      <!-- Username -->
    <nz-form-item>
        <nz-form-label nzRequired>Username</nz-form-label>
        <nz-form-control nzErrorTip="Min 3 characters required">
          <input
      nz-input
 formControlName="username"
   placeholder="Enter your username"
 nzSize="large"
      />
        </nz-form-control>
      </nz-form-item>

      <!-- Password -->
      <nz-form-item>
        <nz-form-label nzRequired>Password</nz-form-label>
        <nz-form-control nzErrorTip="Min 6 characters required">
          <nz-input-group
            [nzSuffix]="suffixTemplate"
     >
            <input
       nz-input
              formControlName="password"
  [type]="showPassword ? 'text' : 'password'"
       placeholder="Enter your password"
        nzSize="large"
      />
 </nz-input-group>
          <ng-template #suffixTemplate>
       <i
    nz-icon
           [nzType]="showPassword ? 'eye-invisible' : 'eye'"
            nzTheme="outline"
              (click)="togglePasswordVisibility()"
     style="cursor: pointer;"
  ></i>
</ng-template>
        </nz-form-control>
      </nz-form-item>

      <!-- Login Button -->
      <nz-form-item nzExtra>
   <button
nz-button
          nzType="primary"
     nzSize="large"
          nzBlock
[disabled]="!loginForm.valid || loading"
      [nzLoading]="loading"
   >
          <i nz-icon nzType="login" nzTheme="outline"></i>
    Sign In
     </button>
  </nz-form-item>

      <!-- Additional Links -->
 <div class="login-footer">
        <a href="#">Forgot Password?</a>
        <a href="#">Contact Support</a>
    </div>
    </form>
  </div>
</div>
```

**File**: `src/app/auth/components/login/login.component.less`

```less
@import '../../../theme.less';

.login-container {
  min-height: 100vh;
  display: flex;
  align-items: center;
  justify-content: center;
  background: linear-gradient(135deg, #667eea 0%, #764ba2 100%);
  font-family: @font-family-base;

  .login-box {
  width: 100%;
    max-width: 400px;
    padding: 40px;
    background: white;
    border-radius: @border-radius-base;
    box-shadow: 0 2px 8px rgba(0, 0, 0, 0.15);

    .login-header {
      text-align: center;
      margin-bottom: 30px;

      h1 {
        margin: 0;
        font-size: 28px;
        color: @primary-color;
        font-weight: 600;
      }

      p {
   margin: 8px 0 0 0;
        color: @text-color-secondary;
     font-size: 14px;
      }
    }

    nz-form-item {
      margin-bottom: 24px;
    }

    .login-footer {
      display: flex;
      justify-content: space-between;
  margin-top: 16px;
      font-size: 12px;

      a {
        color: @primary-color;
     text-decoration: none;

        &:hover {
 text-decoration: underline;
      }
      }
    }
  }
}
```

## 3.2 Auth Module

**File**: `src/app/auth/auth.module.ts`

```typescript
import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { ReactiveFormsModule, FormsModule } from '@angular/forms';

import { NzFormModule } from 'ng-zorro-antd/form';
import { NzInputModule } from 'ng-zorro-antd/input';
import { NzButtonModule } from 'ng-zorro-antd/button';
import { NzCheckboxModule } from 'ng-zorro-antd/checkbox';
import { NzIconModule } from 'ng-zorro-antd/icon';

import { AuthRoutingModule } from './auth-routing.module';
import { LoginComponent } from './components/login/login.component';
import { LogoutComponent } from './components/logout/logout.component';

@NgModule({
  declarations: [LoginComponent, LogoutComponent],
  imports: [
    CommonModule,
    ReactiveFormsModule,
    FormsModule,
    AuthRoutingModule,
    NzFormModule,
 NzInputModule,
    NzButtonModule,
    NzCheckboxModule,
    NzIconModule
  ]
})
export class AuthModule { }
```

**File**: `src/app/auth/auth-routing.module.ts`

```typescript
import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { LoginComponent } from './components/login/login.component';
import { LogoutComponent } from './components/logout/logout.component';

const routes: Routes = [
  { path: 'login', component: LoginComponent },
  { path: 'logout', component: LogoutComponent },
  { path: '', redirectTo: 'login', pathMatch: 'full' }
];

@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule]
})
export class AuthRoutingModule { }
```

## 3.3 Commit Authentication Module

```bash
git add .
git commit -m "feat: Complete authentication module with login component and auth routing"
git push origin main
```

---

# PHASE 4: DASHBOARD & LAYOUT (WEEK 2)

## 4.1 Main Layout Components

**File**: `src/app/shared/components/navbar/navbar.component.ts`

```typescript
import { Component, OnInit } from '@angular/core';
import { AuthService } from 'src/app/core/services/auth.service';
import { Router } from '@angular/router';
import { User } from 'src/app/core/models/user.model';

@Component({
  selector: 'app-navbar',
  templateUrl: './navbar.component.html',
  styleUrls: ['./navbar.component.less']
})
export class NavbarComponent implements OnInit {
  currentUser: User | null = null;
  showUserMenu = false;

  constructor(
    private authService: AuthService,
    private router: Router
  ) { }

  ngOnInit(): void {
    this.authService.currentUser$.subscribe(user => {
      this.currentUser = user;
    });
  }

  logout(): void {
this.authService.logout().subscribe(() => {
      this.router.navigate(['/auth/login']);
    });
  }

  toggleUserMenu(): void {
    this.showUserMenu = !this.showUserMenu;
  }
}
```

**File**: `src/app/shared/components/navbar/navbar.component.html`

```html
<nz-layout class="rcm-navbar">
  <nz-header class="navbar-header">
    <div class="navbar-content">
      <div class="navbar-brand">
   <i nz-icon nzType="medicine-box" nzTheme="fill"></i>
      <span>RCM Portal</span>
      </div>

      <div class="navbar-actions">
        <nz-dropdown
   nzPlacement="bottomRight"
          [nzDropdownMenu]="menu"
 (nzVisibleChange)="showUserMenu = $event"
     >
          <a nz-dropdown>
     <i nz-icon nzType="user" nzTheme="outline"></i>
         {{ currentUser?.firstName }} {{ currentUser?.lastName }}
          </a>
        </nz-dropdown>
        <nz-dropdown-menu #menu="nzDropdownMenu">
          <ul nz-menu>
            <li nz-menu-item>
              <a routerLink="/profile">
   <i nz-icon nzType="profile" nzTheme="outline"></i>
      <span>Profile</span>
              </a>
</li>
         <li nz-menu-item>
   <a routerLink="/settings">
    <i nz-icon nzType="setting" nzTheme="outline"></i>
           <span>Settings</span>
  </a>
       </li>
   <li nz-menu-divider></li>
            <li nz-menu-item>
  <a (click)="logout()">
          <i nz-icon nzType="logout" nzTheme="outline"></i>
          <span>Logout</span>
    </a>
        </li>
          </ul>
        </nz-dropdown-menu>
      </div>
    </div>
  </nz-header>
</nz-layout>
```

**File**: `src/app/shared/components/sidebar/sidebar.component.ts`

```typescript
import { Component, OnInit } from '@angular/core';
import { Router } from '@angular/router';
import { AuthService } from 'src/app/core/services/auth.service';

@Component({
  selector: 'app-sidebar',
  templateUrl: './sidebar.component.html',
  styleUrls: ['./sidebar.component.less']
})
export class SidebarComponent implements OnInit {
  collapsed = false;
  menuItems: any[] = [];

  constructor(
    private authService: AuthService,
    private router: Router
  ) { }

  ngOnInit(): void {
    this.initializeMenu();
  }

  private initializeMenu(): void {
    this.menuItems = [
    {
        label: 'Dashboard',
        icon: 'dashboard',
        link: '/dashboard'
  },
      {
  label: 'Claims Management',
        icon: 'file-text',
   link: '/claims',
        children: [
      { label: 'Submit Claim', link: '/claims/submit' },
          { label: 'View Claims', link: '/claims/list' },
     { label: 'Track Status', link: '/claims/track' }
        ]
      },
 {
        label: 'Eligibility',
    icon: 'check-circle',
        link: '/eligibility'
      },
 {
        label: 'Pre-Authorization',
   icon: 'file-protect',
        link: '/pre-auth'
      },
  {
        label: 'Reports',
        icon: 'bar-chart',
        link: '/reports'
      }
    ];
  }

  toggleCollapsed(): void {
    this.collapsed = !this.collapsed;
  }

  navigateTo(link: string): void {
    this.router.navigate([link]);
  }
}
```

**File**: `src/app/shared/components/sidebar/sidebar.component.html`

```html
<nz-sider
  [nzCollapsible]="true"
  [(nzCollapsed)]="collapsed"
  [nzWidth]="200"
  [nzCollapsedWidth]="80"
  nzTheme="light"
  class="rcm-sidebar"
>
  <ul nz-menu
      [nzInlineIndent]="16"
      [nzMode]="'inline'"
      [nzTheme]="'light'"
  >
    <li nz-menu-item
   *ngFor="let item of menuItems"
        [routerLink]="item.link"
        routerLinkActive="ant-menu-item-selected"
    >
      <i nz-icon [nzType]="item.icon" nzTheme="outline"></i>
      <span>{{ item.label }}</span>
  </li>
  </ul>
</nz-sider>
```

## 4.2 Dashboard Component

**File**: `src/app/features/dashboard/dashboard.component.ts`

```typescript
import { Component, OnInit } from '@angular/core';
import { ClaimsService } from '../claims/services/claims.service';
import { NzMessageService } from 'ng-zorro-antd/message';

@Component({
  selector: 'app-dashboard',
  templateUrl: './dashboard.component.html',
  styleUrls: ['./dashboard.component.less']
})
export class DashboardComponent implements OnInit {
  loading = true;
  totalClaims = 0;
  approvedClaims = 0;
  pendingClaims = 0;
  deniedClaims = 0;
  totalAmount = 0;

  recentClaims: any[] = [];
  claimsTrend: any[] = [];

  constructor(
    private claimsService: ClaimsService,
    private messageService: NzMessageService
  ) { }

  ngOnInit(): void {
    this.loadDashboardData();
  }

  private loadDashboardData(): void {
    this.loading = true;

 // Get claims summary
    this.claimsService.getClaimsSummary().subscribe({
      next: (summary) => {
 this.totalClaims = summary.total;
        this.approvedClaims = summary.approved;
        this.pendingClaims = summary.pending;
        this.deniedClaims = summary.denied;
        this.totalAmount = summary.totalAmount;
      },
      error: (error) => {
        this.messageService.error('Failed to load dashboard data');
  this.loading = false;
      }
    });

    // Get recent claims
    this.claimsService.getRecentClaims(5).subscribe({
      next: (claims) => {
        this.recentClaims = claims;
        this.loading = false;
      },
      error: (error) => {
        this.messageService.error('Failed to load recent claims');
     this.loading = false;
      }
    });
  }
}
```

**File**: `src/app/features/dashboard/dashboard.component.html`

```html
<div class="dashboard-container" [nzSpinning]="loading" nz-spin>
  <!-- Statistics Cards -->
  <div nz-row [nzGutter]="16" class="stats-row">
    <div nz-col [nzXs]="24" [nzSm]="12" [nzMd]="6">
      <nz-card class="stat-card">
     <nz-statistic
        nzTitle="Total Claims"
     [nzValue]="totalClaims"
 nzValueStyle="color: #0066cc;"
        >
          <ng-template #nzPrefix>
          <i nz-icon nzType="file" nzTheme="outline"></i>
   </ng-template>
     </nz-statistic>
      </nz-card>
    </div>

    <div nz-col [nzXs]="24" [nzSm]="12" [nzMd]="6">
      <nz-card class="stat-card">
        <nz-statistic
          nzTitle="Approved"
     [nzValue]="approvedClaims"
          nzValueStyle="color: #00a854;"
        >
      <ng-template #nzPrefix>
     <i nz-icon nzType="check-circle" nzTheme="outline"></i>
     </ng-template>
        </nz-statistic>
      </nz-card>
  </div>

    <div nz-col [nzXs]="24" [nzSm]="12" [nzMd]="6">
      <nz-card class="stat-card">
        <nz-statistic
  nzTitle="Pending"
[nzValue]="pendingClaims"
          nzValueStyle="color: #faad14;"
        >
        <ng-template #nzPrefix>
    <i nz-icon nzType="clock-circle" nzTheme="outline"></i>
     </ng-template>
        </nz-statistic>
      </nz-card>
    </div>

    <div nz-col [nzXs]="24" [nzSm]="12" [nzMd]="6">
      <nz-card class="stat-card">
        <nz-statistic
          nzTitle="Denied"
          [nzValue]="deniedClaims"
      nzValueStyle="color: #ff4d4f;"
        >
          <ng-template #nzPrefix>
            <i nz-icon nzType="close-circle" nzTheme="outline"></i>
          </ng-template>
     </nz-statistic>
      </nz-card>
    </div>
  </div>

  <!-- Recent Claims Table -->
  <div nz-row [nzGutter]="16" class="recent-claims-row">
    <div nz-col [nzSpan]="24">
      <nz-card nzTitle="Recent Claims">
  <nz-table
       [nzData]="recentClaims"
      [nzLoading]="loading"
        [nzPageSize]="5"
    nzSize="small"
        >
 <thead>
     <tr>
    <th>Claim ID</th>
            <th>Patient</th>
      <th>Amount</th>
   <th>Status</th>
              <th>Date</th>
        <th>Action</th>
  </tr>
          </thead>
          <tbody>
       <tr *ngFor="let claim of recentClaims">
  <td><strong>{{ claim.claimNumber }}</strong></td>
              <td>{{ claim.patientName }}</td>
  <td>{{ claim.amount | currency }}</td>
          <td>
       <nz-badge
     [nzStatus]="getClaimStatusIcon(claim.status)"
                  [nzText]="claim.status | uppercase"
         ></nz-badge>
         </td>
            <td>{{ claim.submissionDate | date: 'short' }}</td>
        <td>
     <a [routerLink]="['/claims', claim.id]">View</a>
      </td>
            </tr>
  </tbody>
        </nz-table>
      </nz-card>
    </div>
  </div>
</div>
```

## 4.3 Dashboard Module

**File**: `src/app/features/dashboard/dashboard.module.ts`

```typescript
import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';

import { NzCardModule } from 'ng-zorro-antd/card';
import { NzStatisticModule } from 'ng-zorro-antd/statistic';
import { NzTableModule } from 'ng-zorro-antd/table';
import { NzBadgeModule } from 'ng-zorro-antd/badge';
import { NzGridModule } from 'ng-zorro-antd/grid';
import { NzSpinModule } from 'ng-zorro-antd/spin';
import { NzIconModule } from 'ng-zorro-antd/icon';

import { DashboardRoutingModule } from './dashboard-routing.module';
import { DashboardComponent } from './dashboard.component';

@NgModule({
  declarations: [DashboardComponent],
  imports: [
    CommonModule,
    DashboardRoutingModule,
    NzCardModule,
    NzStatisticModule,
    NzTableModule,
    NzBadgeModule,
    NzGridModule,
    NzSpinModule,
  NzIconModule
  ]
})
export class DashboardModule { }
```

## 4.4 App Component with Layout

**File**: `src/app/app.component.ts`

```typescript
import { Component, OnInit } from '@angular/core';
import { Router } from '@angular/router';
import { AuthService } from './core/services/auth.service';

@Component({
  selector: 'app-root',
  templateUrl: './app.component.html',
  styleUrls: ['./app.component.less']
})
export class AppComponent implements OnInit {
  isLoggedIn = false;

  constructor(
    private authService: AuthService,
    private router: Router
  ) { }

  ngOnInit(): void {
    this.authService.currentUser$.subscribe(user => {
   this.isLoggedIn = !!user;
    });
  }
}
```

**File**: `src/app/app.component.html`

```html
<ng-container *ngIf="isLoggedIn; else authLayout">
  <nz-layout class="rcm-app-layout">
    <app-navbar></app-navbar>
    
    <nz-layout class="app-body">
      <app-sidebar></app-sidebar>
      
  <nz-layout class="app-content">
        <nz-content class="content-wrapper">
          <router-outlet></router-outlet>
  </nz-content>
        
        <app-footer></app-footer>
      </nz-layout>
    </nz-layout>
  </nz-layout>
</ng-container>

<ng-template #authLayout>
  <router-outlet></router-outlet>
</ng-template>
```

## 4.5 Commit Dashboard & Layout

```bash
git add .
git commit -m "feat: Complete dashboard and main layout with navbar, sidebar, and statistics"
git push origin main
```

---

# PHASE 5: CLAIMS MANAGEMENT MODULE (WEEK 2-3)

## 5.1 Claims Service

**File**: `src/app/features/claims/services/claims.service.ts`

```typescript
import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { HttpParams } from '@angular/common/http';
import { ApiService } from 'src/app/core/services/api.service';
import { Claim, ClaimFilter, ClaimResponse } from 'src/app/core/models/claim.model';
import { environment } from 'src/environments/environment';

@Injectable({
  providedIn: 'root'
})
export class ClaimsService {
  private endpoint = environment.endpoints.claims;

  constructor(private apiService: ApiService) { }

  /**
   * Get paginated claims
 */
  getClaims(filter: ClaimFilter): Observable<ClaimResponse> {
    let params = new HttpParams();
    
    if (filter.status) params = params.set('status', filter.status);
    if (filter.providerId) params = params.set('providerId', filter.providerId);
    if (filter.dateFrom) params = params.set('dateFrom', filter.dateFrom.toString());
    if (filter.dateTo) params = params.set('dateTo', filter.dateTo.toString());
    if (filter.searchTerm) params = params.set('search', filter.searchTerm);
    if (filter.page) params = params.set('page', filter.page.toString());
    if (filter.pageSize) params = params.set('pageSize', filter.pageSize.toString());

    return this.apiService.get<ClaimResponse>(this.endpoint.list, params);
  }

  /**
   * Get single claim
   */
  getClaimById(id: string): Observable<Claim> {
    return this.apiService.get<Claim>(`${this.endpoint.getById}/${id}`);
  }

  /**
   * Submit new claim
   */
  submitClaim(claim: Partial<Claim>): Observable<Claim> {
    return this.apiService.post<Claim>(this.endpoint.create, claim);
  }

  /**
   * Update claim
   */
  updateClaim(id: string, claim: Partial<Claim>): Observable<Claim> {
    return this.apiService.put<Claim>(this.endpoint.update, id, claim);
  }

  /**
   * Get claims summary
   */
  getClaimsSummary(): Observable<any> {
    return this.apiService.get<any>(`${this.endpoint.list}/summary`);
  }

  /**
   * Get recent claims
   */
  getRecentClaims(limit: number = 5): Observable<Claim[]> {
    const params = new HttpParams().set('limit', limit.toString());
    return this.apiService.get<Claim[]>(`${this.endpoint.list}/recent`, params);
  }

  /**
   * Get claim status
 */
  getClaimStatus(id: string): Observable<any> {
    return this.apiService.get<any>(`${this.endpoint.getStatus}/${id}`);
}

  /**
   * Approve claim
   */
  approveClaim(id: string): Observable<Claim> {
    return this.apiService.post<Claim>(`${this.endpoint.getById}/${id}/approve`, {});
  }

  /**
   * Deny claim
   */
  denyClaim(id: string, reason: string): Observable<Claim> {
    return this.apiService.post<Claim>(`${this.endpoint.getById}/${id}/deny`, { reason });
  }
}
```

## 5.2 Claims List Component

**File**: `src/app/features/claims/components/claims-list/claims-list.component.ts`

```typescript
import { Component, OnInit } from '@angular/core';
import { Router } from '@angular/router';
import { ClaimsService } from '../../services/claims.service';
import { Claim, ClaimFilter, ClaimStatus } from 'src/app/core/models/claim.model';
import { NzMessageService } from 'ng-zorro-antd/message';

@Component({
  selector: 'app-claims-list',
  templateUrl: './claims-list.component.html',
  styleUrls: ['./claims-list.component.less']
})
export class ClaimsListComponent implements OnInit {
  claims: Claim[] = [];
  loading = false;
  pageIndex = 1;
  pageSize = 10;
  total = 0;
  
  filter: ClaimFilter = {
    page: 1,
    pageSize: 10
  };

  statusOptions = [
    { label: 'All', value: null },
    { label: 'Submitted', value: ClaimStatus.SUBMITTED },
    { label: 'Processing', value: ClaimStatus.PROCESSING },
    { label: 'Approved', value: ClaimStatus.APPROVED },
    { label: 'Denied', value: ClaimStatus.DENIED }
  ];

  constructor(
    private claimsService: ClaimsService,
    private router: Router,
 private messageService: NzMessageService
  ) { }

  ngOnInit(): void {
    this.loadClaims();
  }

  loadClaims(): void {
    this.loading = true;
    this.filter.page = this.pageIndex;
    this.filter.pageSize = this.pageSize;

    this.claimsService.getClaims(this.filter).subscribe({
   next: (response) => {
   this.claims = response.data;
     this.total = response.total;
        this.loading = false;
  },
      error: (error) => {
        this.messageService.error('Failed to load claims');
        this.loading = false;
      }
    });
  }

  onPageChange(page: number): void {
    this.pageIndex = page;
    this.loadClaims();
  }

  onPageSizeChange(pageSize: number): void {
    this.pageSize = pageSize;
    this.pageIndex = 1;
    this.loadClaims();
  }

  onStatusChange(status: any): void {
    this.filter.status = status;
    this.pageIndex = 1;
    this.loadClaims();
  }

  viewClaim(id: string): void {
    this.router.navigate(['/claims', id]);
  }

  editClaim(id: string): void {
    this.router.navigate(['/claims', id, 'edit']);
  }

  getStatusColor(status: ClaimStatus): string {
    switch (status) {
      case ClaimStatus.APPROVED:
        return 'green';
      case ClaimStatus.DENIED:
        return 'red';
   case ClaimStatus.PROCESSING:
        return 'gold';
   default:
        return 'blue';
    }
  }
}
```

**File**: `src/app/features/claims/components/claims-list/claims-list.component.html`

```html
<nz-card nzTitle="Claims Management">
  <div class="claims-toolbar" nz-row [nzGutter]="16">
  <div nz-col [nzSpan]="24">
      <button
        nz-button
        nzType="primary"
        nzSize="large"
        routerLink="/claims/submit"
        class="mb-16"
      >
  <i nz-icon nzType="plus" nzTheme="outline"></i>
     Submit New Claim
      </button>
    </div>
  </div>

  <div class="claims-filters" nz-row [nzGutter]="16" class="mb-16">
    <div nz-col [nzXs]="24" [nzSm]="12" [nzMd]="8">
      <label>Status:</label>
      <nz-select
[nzOptions]="statusOptions"
        (ngModelChange)="onStatusChange($event)"
   nzPlaceHolder="Filter by status"
      ></nz-select>
    </div>

    <div nz-col [nzXs]="24" [nzSm]="12" [nzMd]="8">
      <label>Date Range:</label>
      <nz-range-picker
        (ngModelChange)="onDateRangeChange($event)"
        nzFormat="YYYY-MM-DD"
   ></nz-range-picker>
    </div>

    <div nz-col [nzXs]="24" [nzSm]="12" [nzMd]="8">
      <label>Search:</label>
      <input
        nz-input
        (ngModelChange)="onSearchChange($event)"
        nzPlaceHolder="Search claim..."
      />
    </div>
  </div>

  <!-- Claims Table -->
  <nz-table
    #table
    [nzData]="claims"
    [nzLoading]="loading"
    [nzPageIndex]="pageIndex"
    [nzPageSize]="pageSize"
    [nzTotal]="total"
    [nzShowSizeChanger]="true"
    [nzPageSizeOptions]="[10, 20, 50]"
    (nzPageIndexChange)="onPageChange($event)"
    (nzPageSizeChange)="onPageSizeChange($event)"
    nzSize="small"
  >
    <thead>
      <tr>
        <th nzSortFn="claimNumber" nzShowSort>Claim ID</th>
   <th nzSortFn="patientName" nzShowSort>Patient</th>
    <th nzSortFn="amount" nzShowSort>Amount</th>
        <th nzSortFn="status" nzShowSort>Status</th>
      <th nzSortFn="submissionDate" nzShowSort>Submitted</th>
    <th>Actions</th>
  </tr>
    </thead>
  <tbody>
  <tr *ngFor="let claim of table.data">
        <td><strong>{{ claim.claimNumber }}</strong></td>
     <td>{{ claim.patientName }}</td>
     <td>{{ claim.amount | currency }}</td>
      <td>
       <nz-badge
    [nzStatus]="getStatusColor(claim.status)"
       [nzText]="claim.status | uppercase"
    ></nz-badge>
        </td>
        <td>{{ claim.submissionDate | date: 'short' }}</td>
        <td>
       <a (click)="viewClaim(claim.id)" nz-button nzType="link" nzSize="small">
         View
  </a>
        <nz-divider nzType="vertical"></nz-divider>
 <a (click)="editClaim(claim.id)" nz-button nzType="link" nzSize="small">
    Edit
          </a>
        </td>
      </tr>
    </tbody>
  </nz-table>
</nz-card>
```

## 5.3 Claims Submit Component

**File**: `src/app/features/claims/components/claims-submit/claims-submit.component.ts`

```typescript
import { Component, OnInit } from '@angular/core';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { Router } from '@angular/router';
import { ClaimsService } from '../../services/claims.service';
import { NzMessageService } from 'ng-zorro-antd/message';
import { Claim } from 'src/app/core/models/claim.model';

@Component({
  selector: 'app-claims-submit',
  templateUrl: './claims-submit.component.html',
  styleUrls: ['./claims-submit.component.less']
})
export class ClaimsSubmitComponent implements OnInit {
  claimForm: FormGroup;
  loading = false;
  currentStep = 0;

  constructor(
    private fb: FormBuilder,
    private claimsService: ClaimsService,
    private router: Router,
    private messageService: NzMessageService
  ) {
    this.claimForm = this.fb.group({
  // Patient Information
      patientName: ['', [Validators.required, Validators.minLength(3)]],
      dateOfBirth: ['', Validators.required],
      memberId: ['', Validators.required],
      
      // Service Information
      serviceDate: ['', Validators.required],
    diagnosisCode: ['', Validators.required],
 procedureCode: ['', Validators.required],
      
      // Financial Information
      amount: ['', [Validators.required, Validators.min(0)]],
      
      // Provider Information
      providerId: ['', Validators.required],
      providerName: ['', Validators.required],
      
      // Insurance Information
      insurerId: ['', Validators.required],
      insurerName: ['', Validators.required],
      
      // Notes
      notes: ['']
  });
  }

  ngOnInit(): void { }

  submit(): void {
    if (!this.claimForm.valid) {
      this.messageService.error('Please fill all required fields');
      return;
    }

  this.loading = true;
    const claimData: Partial<Claim> = this.claimForm.value;

    this.claimsService.submitClaim(claimData).subscribe({
      next: (response) => {
        this.loading = false;
     this.messageService.success('Claim submitted successfully!');
        this.router.navigate(['/claims', response.id]);
      },
      error: (error) => {
        this.loading = false;
        this.messageService.error('Failed to submit claim');
      }
    });
  }

  resetForm(): void {
 this.claimForm.reset();
  }

  nextStep(): void {
    if (this.currentStep < 3) {
      this.currentStep++;
    }
  }

  previousStep(): void {
    if (this.currentStep > 0) {
      this.currentStep--;
    }
  }
}
```

## 5.4 Claims Module Setup

**File**: `src/app/features/claims/claims.module.ts`

```typescript
import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { ReactiveFormsModule, FormsModule } from '@angular/forms';

import { NzCardModule } from 'ng-zorro-antd/card';
import { NzTableModule } from 'ng-zorro-antd/table';
import { NzButtonModule } from 'ng-zorro-antd/button';
import { NzFormModule } from 'ng-zorro-antd/form';
import { NzInputModule } from 'ng-zorro-antd/input';
import { NzSelectModule } from 'ng-zorro-antd/select';
import { NzDatePickerModule } from 'ng-zorro-antd/date-picker';
import { NzStepsModule } from 'ng-zorro-antd/steps';
import { NzBadgeModule } from 'ng-zorro-antd/badge';
import { NzDividerModule } from 'ng-zorro-antd/divider';
import { NzIconModule } from 'ng-zorro-antd/icon';

import { ClaimsRoutingModule } from './claims-routing.module';
import { ClaimsListComponent } from './components/claims-list/claims-list.component';
import { ClaimsSubmitComponent } from './components/claims-submit/claims-submit.component';
import { ClaimsDetailComponent } from './components/claims-detail/claims-detail.component';

@NgModule({
  declarations: [
    ClaimsListComponent,
    ClaimsSubmitComponent,
    ClaimsDetailComponent
  ],
  imports: [
  CommonModule,
    ReactiveFormsModule,
    FormsModule,
  ClaimsRoutingModule,
    
    // Ant Design Modules
    NzCardModule,
    NzTableModule,
    NzButtonModule,
    NzFormModule,
    NzInputModule,
    NzSelectModule,
    NzDatePickerModule,
  NzStepsModule,
    NzBadgeModule,
    NzDividerModule,
    NzIconModule
  ]
})
export class ClaimsModule { }
```

## 5.5 Commit Claims Module

```bash
git add .
git commit -m "feat: Complete claims management module with list, submit, and detail views"
git push origin main
```

---

# PHASE 6: ELIGIBILITY VERIFICATION (WEEK 3)

## 6.1 Eligibility Service

**File**: `src/app/features/eligibility/services/eligibility.service.ts`

```typescript
import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { ApiService } from 'src/app/core/services/api.service';
import { EligibilityRequest, EligibilityResponse, EligibilityHistory } from 'src/app/core/models/eligibility.model';
import { environment } from 'src/environments/environment';

@Injectable({
  providedIn: 'root'
})
export class EligibilityService {
  private endpoint = environment.endpoints.eligibility;

  constructor(private apiService: ApiService) { }

  /**
   * Verify member eligibility
   */
  verifyEligibility(request: EligibilityRequest): Observable<EligibilityResponse> {
    return this.apiService.post<EligibilityResponse>(
      this.endpoint.verify,
      request
    );
  }

  /**
   * Get eligibility verification history
   */
  getEligibilityHistory(memberId: string): Observable<EligibilityHistory[]> {
    return this.apiService.get<EligibilityHistory[]>(
   `${this.endpoint.getHistory}/${memberId}`
    );
  }
}
```

## 6.2 Eligibility Verification Component

**File**: `src/app/features/eligibility/components/eligibility-verify/eligibility-verify.component.ts`

```typescript
import { Component, OnInit } from '@angular/core';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { EligibilityService } from '../../services/eligibility.service';
import { EligibilityRequest, EligibilityResponse } from 'src/app/core/models/eligibility.model';
import { NzMessageService } from 'ng-zorro-antd/message';

@Component({
  selector: 'app-eligibility-verify',
  templateUrl: './eligibility-verify.component.html',
  styleUrls: ['./eligibility-verify.component.less']
})
export class EligibilityVerifyComponent implements OnInit {
  verifyForm: FormGroup;
  loading = false;
  result: EligibilityResponse | null = null;
  showResult = false;

  constructor(
    private fb: FormBuilder,
    private eligibilityService: EligibilityService,
    private messageService: NzMessageService
  ) {
    this.verifyForm = this.fb.group({
      memberId: ['', [Validators.required, Validators.minLength(3)]],
   firstName: ['', [Validators.required, Validators.minLength(2)]],
      lastName: ['', [Validators.required, Validators.minLength(2)]],
      dateOfBirth: ['', Validators.required],
      insurerId: ['', Validators.required]
    });
  }

  ngOnInit(): void { }

  verify(): void {
    if (!this.verifyForm.valid) {
      this.messageService.error('Please fill all required fields');
      return;
    }

    this.loading = true;
    const request: EligibilityRequest = this.verifyForm.value;

    this.eligibilityService.verifyEligibility(request).subscribe({
      next: (response) => {
  this.loading = false;
        this.result = response;
        this.showResult = true;
  this.messageService.success('Eligibility verified');
      },
      error: (error) => {
        this.loading = false;
        this.messageService.error('Failed to verify eligibility');
}
    });
  }

  reset(): void {
    this.verifyForm.reset();
    this.showResult = false;
 this.result = null;
  }
}
```

**File**: `src/app/features/eligibility/components/eligibility-verify/eligibility-verify.component.html`

```html
<nz-card nzTitle="Verify Member Eligibility">
  <div nz-row [nzGutter]="16">
    <div nz-col [nzXs]="24" [nzMd]="12">
      <!-- Verification Form -->
      <form nz-form [formGroup]="verifyForm" (ngSubmit)="verify()">
        <nz-form-item>
          <nz-form-label nzRequired>Member ID</nz-form-label>
      <nz-form-control>
       <input
    nz-input
      formControlName="memberId"
          placeholder="Enter member ID"
        />
          </nz-form-control>
        </nz-form-item>

  <nz-form-item>
          <nz-form-label nzRequired>First Name</nz-form-label>
          <nz-form-control>
            <input
        nz-input
       formControlName="firstName"
        placeholder="Enter first name"
          />
 </nz-form-control>
        </nz-form-item>

     <nz-form-item>
          <nz-form-label nzRequired>Last Name</nz-form-label>
  <nz-form-control>
    <input
  nz-input
         formControlName="lastName"
     placeholder="Enter last name"
   />
  </nz-form-control>
        </nz-form-item>

        <nz-form-item>
      <nz-form-label nzRequired>Date of Birth</nz-form-label>
        <nz-form-control>
            <nz-date-picker
  formControlName="dateOfBirth"
         nzFormat="YYYY-MM-DD"
   ></nz-date-picker>
          </nz-form-control>
        </nz-form-item>

        <nz-form-item>
          <nz-form-label nzRequired>Insurer ID</nz-form-label>
      <nz-form-control>
 <input
              nz-input
    formControlName="insurerId"
              placeholder="Enter insurer ID"
     />
      </nz-form-control>
        </nz-form-item>

        <nz-form-item nzExtra>
 <button
            nz-button
      nzType="primary"
nzSize="large"
      [disabled]="!verifyForm.valid || loading"
         [nzLoading]="loading"
          >
   <i nz-icon nzType="check" nzTheme="outline"></i>
            Verify Eligibility
          </button>
     <button
            nz-button
          nzSize="large"
  (click)="reset()"
    class="ml-8"
          >
        Reset
          </button>
        </nz-form-item>
      </form>
    </div>

<div nz-col [nzXs]="24" [nzMd]="12" *ngIf="showResult && result">
      <!-- Eligibility Result -->
      <nz-card
        nzTitle="Eligibility Result"
        [nzBordered]="true"
        [ngClass]="result.isEligible ? 'eligible-card' : 'not-eligible-card'"
    >
 <nz-descriptions [nzColumn]="1">
        <nz-descriptions-item nzTitle="Member ID">
    {{ result.memberId }}
       </nz-descriptions-item>
   
      <nz-descriptions-item nzTitle="Status">
     <nz-badge
     [nzStatus]="result.isEligible ? 'success' : 'error'"
         [nzText]="result.isEligible ? 'ELIGIBLE' : 'NOT ELIGIBLE'"
            ></nz-badge>
          </nz-descriptions-item>

          <nz-descriptions-item nzTitle="Plan Name">
            {{ result.planName }}
  </nz-descriptions-item>

     <nz-descriptions-item nzTitle="Effective Date">
  {{ result.effectiveDate | date: 'short' }}
       </nz-descriptions-item>

   <nz-descriptions-item nzTitle="Deductible">
            {{ result.deductible | currency }}
          </nz-descriptions-item>

   <nz-descriptions-item nzTitle="Co-Pay">
            {{ result.copay | currency }}
          </nz-descriptions-item>

          <nz-descriptions-item nzTitle="Coinsurance">
       {{ result.coinsurance }}%
          </nz-descriptions-item>

    <nz-descriptions-item nzTitle="Out of Pocket Max">
          {{ result.outOfPocketMax | currency }}
       </nz-descriptions-item>

    <nz-descriptions-item nzTitle="Verified Date">
    {{ result.verificationDate | date: 'short' }}
          </nz-descriptions-item>
        </nz-descriptions>
      </nz-card>
  </div>
  </div>
</nz-card>
```

## 6.3 Eligibility Module

**File**: `src/app/features/eligibility/eligibility.module.ts`

```typescript
import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { ReactiveFormsModule, FormsModule } from '@angular/forms';

import { NzCardModule } from 'ng-zorro-antd/card';
import { NzFormModule } from 'ng-zorro-antd/form';
import { NzInputModule } from 'ng-zorro-antd/input';
import { NzButtonModule } from 'ng-zorro-antd/button';
import { NzDatePickerModule } from 'ng-zorro-antd/date-picker';
import { NzSelectModule } from 'ng-zorro-antd/select';
import { NzBadgeModule } from 'ng-zorro-antd/badge';
import { NzDescriptionsModule } from 'ng-zorro-antd/descriptions';
import { NzDividerModule } from 'ng-zorro-antd/divider';
import { NzIconModule } from 'ng-zorro-antd/icon';
import { NzGridModule } from 'ng-zorro-antd/grid';

import { EligibilityRoutingModule } from './eligibility-routing.module';
import { EligibilityVerifyComponent } from './components/eligibility-verify/eligibility-verify.component';
import { EligibilityHistoryComponent } from './components/eligibility-history/eligibility-history.component';

@NgModule({
  declarations: [
  EligibilityVerifyComponent,
    EligibilityHistoryComponent
  ],
  imports: [
    CommonModule,
    ReactiveFormsModule,
    FormsModule,
    EligibilityRoutingModule,
    
    // Ant Design Modules
    NzCardModule,
    NzFormModule,
    NzInputModule,
NzButtonModule,
    NzDatePickerModule,
    NzSelectModule,
    NzBadgeModule,
    NzDescriptionsModule,
    NzDividerModule,
    NzIconModule,
    NzGridModule
  ]
})
export class EligibilityModule { }
```

## 6.4 Commit Eligibility Module

```bash
git add .
git commit -m "feat: Complete eligibility verification module with verification and history views"
git push origin main
```

---

# PHASE 7: PRE-AUTHORIZATION (WEEK 3-4)

## 7.1 Pre-Auth Service & Components

**File**: `src/app/features/pre-auth/services/pre-auth.service.ts`

```typescript
import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { HttpParams } from '@angular/common/http';
import { ApiService } from 'src/app/core/services/api.service';
import { environment } from 'src/environments/environment';

@Injectable({
  providedIn: 'root'
})
export class PreAuthService {
  private endpoint = environment.endpoints.preAuth;

  constructor(private apiService: ApiService) { }

  /**
   * Request pre-authorization
   */
  requestPreAuth(data: any): Observable<any> {
    return this.apiService.post(this.endpoint.request, data);
  }

  /**
   * Get pre-auth status
   */
  getPreAuthStatus(id: string): Observable<any> {
    return this.apiService.get(`${this.endpoint.getStatus}/${id}`);
  }

  /**
   * Get all pre-auth requests for user
   */
  getPreAuthRequests(page: number = 1, pageSize: number = 10): Observable<any> {
    const params = new HttpParams()
      .set('page', page)
 .set('pageSize', pageSize);
  return this.apiService.get(this.endpoint.request, params);
  }
}
```

## 7.2 Pre-Auth Request Component

**File**: `src/app/features/pre-auth/components/pre-auth-request/pre-auth-request.component.ts`

```typescript
import { Component, OnInit } from '@angular/core';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { PreAuthService } from '../../services/pre-auth.service';
import { Router } from '@angular/router';
import { NzMessageService } from 'ng-zorro-antd/message';

@Component({
selector: 'app-pre-auth-request',
  templateUrl: './pre-auth-request.component.html',
  styleUrls: ['./pre-auth-request.component.less']
})
export class PreAuthRequestComponent implements OnInit {
  preAuthForm: FormGroup;
loading = false;

  constructor(
    private fb: FormBuilder,
    private preAuthService: PreAuthService,
    private router: Router,
    private messageService: NzMessageService
  ) {
    this.preAuthForm = this.fb.group({
      memberId: ['', [Validators.required, Validators.minLength(3)]],
      procedureCode: ['', Validators.required],
  procedureDescription: ['', Validators.required],
      requestedDate: ['', Validators.required],
      providerName: ['', Validators.required],
      providerId: ['', Validators.required],
  justification: ['', Validators.required],
      estimatedCost: ['', [Validators.required, Validators.min(0)]]
    });
  }

  ngOnInit(): void { }

  submit(): void {
    if (!this.preAuthForm.valid) {
      this.messageService.error('Please fill all required fields');
 return;
    }

  this.loading = true;
    this.preAuthService.requestPreAuth(this.preAuthForm.value).subscribe({
      next: (response) => {
        this.loading = false;
      this.messageService.success('Pre-authorization requested successfully!');
  this.router.navigate(['/pre-auth', response.id]);
      },
      error: (error) => {
        this.loading = false;
        this.messageService.error('Failed to request pre-authorization');
  }
    });
  }

  reset(): void {
    this.preAuthForm.reset();
  }
}
```

**File**: `src/app/features/pre-auth/components/pre-auth-request/pre-auth-request.component.html`

```html
<nz-card nzTitle="Request Pre-Authorization">
  <form nz-form [formGroup]="preAuthForm" (ngSubmit)="submit()">
    <div nz-row [nzGutter]="16">
      <!-- Member Information -->
      <div nz-col [nzXs]="24" [nzMd]="12">
        <nz-form-item>
        <nz-form-label nzRequired>Member ID</nz-form-label>
        <nz-form-control>
 <input
     nz-input
      formControlName="memberId"
              placeholder="Enter member ID"
     />
          </nz-form-control>
        </nz-form-item>
      </div>

      <!-- Procedure Information -->
      <div nz-col [nzXs]="24" [nzMd]="12">
        <nz-form-item>
    <nz-form-label nzRequired>Procedure Code</nz-form-label>
   <nz-form-control>
<input
    nz-input
              formControlName="procedureCode"
            placeholder="Enter procedure code"
      />
   </nz-form-control>
        </nz-form-item>
      </div>

      <div nz-col [nzXs]="24">
        <nz-form-item>
          <nz-form-label nzRequired>Procedure Description</nz-form-label>
          <nz-form-control>
            <textarea
      nz-input
       formControlName="procedureDescription"
     placeholder="Describe the procedure"
rows="3"
   ></textarea>
          </nz-form-control>
        </nz-form-item>
   </div>

    <!-- Requested Date -->
      <div nz-col [nzXs]="24" [nzMd]="12">
        <nz-form-item>
          <nz-form-label nzRequired>Requested Service Date</nz-form-label>
          <nz-form-control>
    <nz-date-picker
           formControlName="requestedDate"
   nzFormat="YYYY-MM-DD"
            ></nz-date-picker>
          </nz-form-control>
        </nz-form-item>
      </div>

      <!-- Provider Information -->
   <div nz-col [nzXs]="24" [nzMd]="12">
        <nz-form-item>
    <nz-form-label nzRequired>Provider Name</nz-form-label>
          <nz-form-control>
      <input
    nz-input
          formControlName="providerName"
              placeholder="Enter provider name"
            />
          </nz-form-control>
        </nz-form-item>
      </div>

      <div nz-col [nzXs]="24" [nzMd]="12">
        <nz-form-item>
      <nz-form-label nzRequired>Provider ID</nz-form-label>
     <nz-form-control>
            <input
        nz-input
    formControlName="providerId"
              placeholder="Enter provider ID"
        />
    </nz-form-control>
        </nz-form-item>
    </div>

<!-- Justification -->
      <div nz-col [nzXs]="24">
      <nz-form-item>
          <nz-form-label nzRequired>Clinical Justification</nz-form-label>
          <nz-form-control>
            <textarea
    nz-input
   formControlName="justification"
         placeholder="Provide clinical justification"
         rows="4"
    ></textarea>
      </nz-form-control>
        </nz-form-item>
      </div>

      <!-- Estimated Cost -->
 <div nz-col [nzXs]="24" [nzMd]="12">
 <nz-form-item>
          <nz-form-label nzRequired>Estimated Cost</nz-form-label>
          <nz-form-control>
   <nz-input-number
      formControlName="estimatedCost"
              [nzMin]="0"
         nzPlaceHolder="0.00"
        nzPrefix="$"
        nzSize="large"
            ></nz-input-number>
    </nz-form-control>
   </nz-form-item>
      </div>

      <!-- Buttons -->
      <div nz-col [nzXs]="24">
        <nz-form-item nzExtra>
       <button
   nz-button
        nzType="primary"
     nzSize="large"
            [disabled]="!preAuthForm.valid || loading"
[nzLoading]="loading"
      >
            <i nz-icon nzType="check" nzTheme="outline"></i>
            Submit Request
          </button>
  <button
            nz-button
            nzSize="large"
    (click)="reset()"
            class="ml-8"
      >
      Reset
          </button>
        </nz-form-item>
      </div>
    </div>
  </form>
</nz-card>
```

## 7.3 Pre-Auth Module

**File**: `src/app/features/pre-auth/pre-auth.module.ts`

```typescript
import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { ReactiveFormsModule, FormsModule } from '@angular/forms';

import { NzCardModule } from 'ng-zorro-antd/card';
import { NzFormModule } from 'ng-zorro-antd/form';
import { NzInputModule } from 'ng-zorro-antd/input';
import { NzInputNumberModule } from 'ng-zorro-antd/input-number';
import { NzButtonModule } from 'ng-zorro-antd/button';
import { NzDatePickerModule } from 'ng-zorro-antd/date-picker';
import { NzSelectModule } from 'ng-zorro-antd/select';
import { NzBadgeModule } from 'ng-zorro-antd/badge';
import { NzTableModule } from 'ng-zorro-antd/table';
import { NzIconModule } from 'ng-zorro-antd/icon';
import { NzGridModule } from 'ng-zorro-antd/grid';

import { PreAuthRoutingModule } from './pre-auth-routing.module';
import { PreAuthRequestComponent } from './components/pre-auth-request/pre-auth-request.component';
import { PreAuthListComponent } from './components/pre-auth-list/pre-auth-list.component';
import { PreAuthDetailComponent } from './components/pre-auth-detail/pre-auth-detail.component';

@NgModule({
  declarations: [
    PreAuthRequestComponent,
    PreAuthListComponent,
    PreAuthDetailComponent
  ],
  imports: [
    CommonModule,
    ReactiveFormsModule,
    FormsModule,
    PreAuthRoutingModule,
    
    // Ant Design Modules
    NzCardModule,
    NzFormModule,
    NzInputModule,
    NzInputNumberModule,
    NzButtonModule,
    NzDatePickerModule,
    NzSelectModule,
    NzBadgeModule,
    NzTableModule,
    NzIconModule,
    NzGridModule
  ]
})
export class PreAuthModule { }
```

## 7.4 Commit Pre-Auth Module

```bash
git add .
git commit -m "feat: Complete pre-authorization module with request and status tracking"
git push origin main
```

---

# PHASE 8: REPORTS & ANALYTICS (WEEK 4)

## 8.1 Reports Service

**File**: `src/app/features/reports/services/reports.service.ts`

```typescript
import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { HttpParams } from '@angular/common/http';
import { ApiService } from 'src/app/core/services/api.service';
import { environment } from 'src/environments/environment';

@Injectable({
  providedIn: 'root'
})
export class ReportsService {
  private endpoint = environment.endpoints.reports;

  constructor(private apiService: ApiService) { }

  /**
   * Get claims report
   */
  getClaimsReport(filters: any): Observable<any> {
    let params = new HttpParams();
    
  if (filters.dateFrom) params = params.set('dateFrom', filters.dateFrom);
    if (filters.dateTo) params = params.set('dateTo', filters.dateTo);
    if (filters.status) params = params.set('status', filters.status);

    return this.apiService.get(this.endpoint.claims, params);
  }

  /**
   * Get eligibility report
   */
  getEligibilityReport(filters: any): Observable<any> {
    let params = new HttpParams();
 
    if (filters.dateFrom) params = params.set('dateFrom', filters.dateFrom);
    if (filters.dateTo) params = params.set('dateTo', filters.dateTo);

    return this.apiService.get(this.endpoint.eligibility, params);
  }

  /**
   * Export report to CSV
*/
  exportReportToCSV(reportType: string, data: any[]): void {
    const csv = this.convertToCSV(data);
    this.downloadCSV(csv, `${reportType}-report.csv`);
}

  private convertToCSV(data: any[]): string {
    if (!data || data.length === 0) return '';

    const keys = Object.keys(data[0]);
    const csvHeader = keys.join(',');
    
    const csvRows = data.map(obj =>
    keys.map(key => {
        const value = obj[key];
   return `"${value}"`;
      }).join(',')
    );

    return [csvHeader, ...csvRows].join('\n');
  }

  private downloadCSV(csv: string, filename: string): void {
    const blob = new Blob([csv], { type: 'text/csv' });
    const url = window.URL.createObjectURL(blob);
    const link = document.createElement('a');
    link.href = url;
    link.download = filename;
    link.click();
    window.URL.revokeObjectURL(url);
  }
}
```

## 8.2 Claims Report Component

**File**: `src/app/features/reports/components/claims-report/claims-report.component.ts`

```typescript
import { Component, OnInit } from '@angular/core';
import { FormBuilder, FormGroup } from '@angular/forms';
import { ReportsService } from '../../services/reports.service';
import { NzMessageService } from 'ng-zorro-antd/message';

@Component({
  selector: 'app-claims-report',
  templateUrl: './claims-report.component.html',
  styleUrls: ['./claims-report.component.less']
})
export class ClaimsReportComponent implements OnInit {
  reportForm: FormGroup;
  loading = false;
  reportData: any[] = [];
  summary: any = {};

  statusOptions = [
    { label: 'All', value: '' },
    { label: 'Approved', value: 'approved' },
    { label: 'Pending', value: 'pending' },
    { label: 'Denied', value: 'denied' }
  ];

  constructor(
    private fb: FormBuilder,
    private reportsService: ReportsService,
    private messageService: NzMessageService
  ) {
    this.reportForm = this.fb.group({
      dateFrom: [null],
      dateTo: [null],
      status: ['']
  });
  }

  ngOnInit(): void {
    this.generateReport();
  }

  generateReport(): void {
    this.loading = true;
    const filters = this.reportForm.value;

    this.reportsService.getClaimsReport(filters).subscribe({
      next: (response) => {
        this.reportData = response.data;
this.summary = response.summary;
        this.loading = false;
      },
      error: (error) => {
      this.messageService.error('Failed to generate report');
        this.loading = false;
  }
    });
  }

  exportReport(): void {
    if (this.reportData.length === 0) {
      this.messageService.warning('No data to export');
      return;
    }

    this.reportsService.exportReportToCSV('claims', this.reportData);
  this.messageService.success('Report exported successfully');
  }

  reset(): void {
    this.reportForm.reset();
    this.reportData = [];
    this.summary = {};
  }
}
```

**File**: `src/app/features/reports/components/claims-report/claims-report.component.html`

```html
<nz-card nzTitle="Claims Report">
  <!-- Filters -->
  <form nz-form [formGroup]="reportForm" class="mb-24">
    <div nz-row [nzGutter]="16">
    <div nz-col [nzXs]="24" [nzSm]="12" [nzMd]="6">
        <nz-form-item>
        <nz-form-label>Date From:</nz-form-label>
          <nz-form-control>
        <nz-date-picker
formControlName="dateFrom"
          nzFormat="YYYY-MM-DD"
       ></nz-date-picker>
          </nz-form-control>
      </nz-form-item>
      </div>

   <div nz-col [nzXs]="24" [nzSm]="12" [nzMd]="6">
 <nz-form-item>
          <nz-form-label>Date To:</nz-form-label>
       <nz-form-control>
  <nz-date-picker
       formControlName="dateTo"
       nzFormat="YYYY-MM-DD"
      ></nz-date-picker>
          </nz-form-control>
        </nz-form-item>
      </div>

      <div nz-col [nzXs]="24" [nzSm]="12" [nzMd]="6">
        <nz-form-item>
          <nz-form-label>Status:</nz-form-label>
        <nz-form-control>
         <nz-select
    formControlName="status"
       [nzOptions]="statusOptions"
           nzPlaceHolder="Select status"
            ></nz-select>
          </nz-form-control>
        </nz-form-item>
      </div>

    <div nz-col [nzXs]="24" [nzSm]="12" [nzMd]="6">
        <nz-form-item>
   <button
     nz-button
  nzType="primary"
            nzSize="large"
    (click)="generateReport()"
            [nzLoading]="loading"
          >
      <i nz-icon nzType="reload" nzTheme="outline"></i>
            Generate
          </button>
        </nz-form-item>
      </div>
    </div>
  </form>

  <!-- Summary Statistics -->
  <div nz-row [nzGutter]="16" class="mb-24" *ngIf="summary">
    <div nz-col [nzXs]="24" [nzSm]="12" [nzMd]="6">
  <nz-card class="stat-card">
        <nz-statistic
    nzTitle="Total Claims"
     [nzValue]="summary.total || 0"
        ></nz-statistic>
      </nz-card>
    </div>

    <div nz-col [nzXs]="24" [nzSm]="12" [nzMd]="6">
      <nz-card class="stat-card">
        <nz-statistic
          nzTitle="Total Amount"
[nzValue]="summary.totalAmount || 0"
          nzValueStyle="color: #0066cc;"
     >
          <ng-template #nzPrefix>$</ng-template>
        </nz-statistic>
      </nz-card>
    </div>

    <div nz-col [nzXs]="24" [nzSm]="12" [nzMd]="6">
      <nz-card class="stat-card">
     <nz-statistic
          nzTitle="Approval Rate"
      [nzValue]="summary.approvalRate || 0"
        nzValueStyle="color: #00a854;"
        >
          <ng-template #nzSuffix>%</ng-template>
        </nz-statistic>
      </nz-card>
    </div>

    <div nz-col [nzXs]="24" [nzSm]="12" [nzMd]="6">
  <nz-card class="stat-card">
        <nz-statistic
          nzTitle="Avg Processing Time"
          [nzValue]="summary.avgProcessingTime || 0"
>
          <ng-template #nzSuffix>days</ng-template>
        </nz-statistic>
      </nz-card>
    </div>
  </div>

  <!-- Report Table -->
  <div class="mb-16">
    <button
    nz-button
      nzType="primary"
      (click)="exportReport()"
      [disabled]="reportData.length === 0"
 >
   <i nz-icon nzType="download" nzTheme="outline"></i>
      Export to CSV
    </button>
    <button
      nz-button
      (click)="reset()"
      class="ml-8"
    >
      Reset
    </button>
  </div>

  <nz-table
    [nzData]="reportData"
    [nzLoading]="loading"
    nzSize="small"
  >
    <thead>
  <tr>
        <th>Claim ID</th>
 <th>Patient</th>
        <th>Amount</th>
        <th>Status</th>
        <th>Submitted Date</th>
        <th>Provider</th>
      </tr>
    </thead>
    <tbody>
      <tr *ngFor="let record of reportData">
        <td>{{ record.claimNumber }}</td>
        <td>{{ record.patientName }}</td>
        <td>{{ record.amount | currency }}</td>
        <td>
       <nz-badge
       [nzStatus]="record.status | lowercase"
[nzText]="record.status | uppercase"
      ></nz-badge>
        </td>
     <td>{{ record.submissionDate | date: 'short' }}</td>
        <td>{{ record.providerName }}</td>
      </tr>
    </tbody>
  </nz-table>
</nz-card>
```

## 8.3 Reports Module

**File**: `src/app/features/reports/reports.module.ts`

```typescript
import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { ReactiveFormsModule, FormsModule } from '@angular/forms';

import { NzCardModule } from 'ng-zorro-antd/card';
import { NzFormModule } from 'ng-zorro-antd/form';
import { NzButtonModule } from 'ng-zorro-antd/button';
import { NzDatePickerModule } from 'ng-zorro-antd/date-picker';
import { NzSelectModule } from 'ng-zorro-antd/select';
import { NzTableModule } from 'ng-zorro-antd/table';
import { NzStatisticModule } from 'ng-zorro-antd/statistic';
import { NzBadgeModule } from 'ng-zorro-antd/badge';
import { NzIconModule } from 'ng-zorro-antd/icon';
import { NzGridModule } from 'ng-zorro-antd/grid';

import { ReportsRoutingModule } from './reports-routing.module';
import { ClaimsReportComponent } from './components/claims-report/claims-report.component';
import { EligibilityReportComponent } from './components/eligibility-report/eligibility-report.component';

@NgModule({
  declarations: [
    ClaimsReportComponent,
    EligibilityReportComponent
  ],
  imports: [
    CommonModule,
    ReactiveFormsModule,
    FormsModule,
    ReportsRoutingModule,
    
 // Ant Design Modules
    NzCardModule,
 NzFormModule,
    NzButtonModule,
    NzDatePickerModule,
    NzSelectModule,
    NzTableModule,
    NzStatisticModule,
    NzBadgeModule,
    NzIconModule,
    NzGridModule
  ]
})
export class ReportsModule { }
```

## 8.4 Commit Reports Module

```bash
git add .
git commit -m "feat: Complete reports and analytics module with claims and eligibility reports"
git push origin main
```

---

# PHASE 9: TESTING & OPTIMIZATION (WEEK 4-5)

## 9.1 Unit Tests Setup

**File**: `src/app/core/services/auth.service.spec.ts`

```typescript
import { TestBed } from '@angular/core/testing';
import { HttpClientTestingModule } from '@angular/common/http/testing';
import { AuthService } from './auth.service';
import { ApiService } from './api.service';

describe('AuthService', () => {
  let service: AuthService;

  beforeEach(() => {
    TestBed.configureTestingModule({
      imports: [HttpClientTestingModule],
      providers: [AuthService, ApiService]
    });
    service = TestBed.inject(AuthService);
  });

  it('should be created', () => {
    expect(service).toBeTruthy();
  });

  it('should check if user is authenticated', () => {
    expect(service.isAuthenticated()).toBeFalsy();
  });

  it('should return current user', () => {
    expect(service.getCurrentUser()).toBeNull();
  });
});
```

## 9.2 Performance Optimization

**File**: `angular.json` - Update build configuration

```json
{
  "projects": {
    "rcm-portal-antd": {
 "architect": {
      "build": {
      "options": {
  "optimization": true,
"outputHashing": "all",
  "sourceMap": false,
     "namedChunks": false,
  "extractLicenses": true,
            "vendorChunk": false,
            "buildOptimizer": true,
            "budgets": [
 {
                "type": "bundle",
    "name": "main",
      "baseline": "500kb",
     "warning": "600kb",
        "error": "750kb"
     }
 ]
          }
        },
        "serve": {
          "options": {
            "browserTarget": "rcm-portal-antd:build"
    }
        }
      }
    }
  }
}
```

## 9.3 Lazy Loading Routes

Already implemented in Phase 2 with:
```typescript
const routes: Routes = [
  {
    path: 'claims',
    loadChildren: () => import('./features/claims/claims.module').then(m => m.ClaimsModule),
    canActivate: [AuthGuard]
  }
];
```

## 9.4 Production Build

```bash
# Build for production
ng build --configuration production

# Output will be in dist/rcm-portal-antd/

# Test production build locally
npx http-server dist/rcm-portal-antd/

# Visit http://localhost:8080
```

## 9.5 Testing

```bash
# Run unit tests
ng test

# Run tests with coverage
ng test --code-coverage

# Run e2e tests (if configured)
ng e2e
```

## 9.6 Commit Testing & Optimization

```bash
git add .
git commit -m "feat: Add unit tests, performance optimization, and production build configuration"
git push origin main
```

---

# BACKEND INTEGRATION GUIDE

## API Integration Points

Your Angular app connects to your .NET 9 backend at:

```
Backend Base URL: http://localhost:5000/api
(Update in environment.ts)
```

### Required .NET 9 API Endpoints

**Authentication**:
- `POST /api/auth/login` - User login
- `POST /api/auth/logout` - User logout
- `POST /api/auth/refresh-token` - Refresh JWT token

**Claims**:
- `GET /api/claims` - List all claims (with pagination & filtering)
- `POST /api/claims/submit` - Submit new claim
- `GET /api/claims/{id}` - Get claim by ID
- `PUT /api/claims/{id}` - Update claim
- `GET /api/claims/summary` - Get claims summary stats
- `GET /api/claims/recent` - Get recent claims
- `POST /api/claims/{id}/approve` - Approve claim
- `POST /api/claims/{id}/deny` - Deny claim

**Eligibility**:
- `POST /api/eligibility/verify` - Verify member eligibility
- `GET /api/eligibility/history/{memberId}` - Get verification history

**Pre-Authorization**:
- `POST /api/pre-auth/request` - Request pre-authorization
- `GET /api/pre-auth/status/{id}` - Get pre-auth status
- `GET /api/pre-auth/request` - List pre-auth requests

**Reports**:
- `GET /api/reports/claims` - Get claims report
- `GET /api/reports/eligibility` - Get eligibility report

## Expected API Response Format

```typescript
// Success Response
{
  "success": true,
  "data": { /* actual data */ },
  "message": "Operation successful"
}

// Error Response
{
  "success": false,
  "message": "Error description",
  "errors": [
  { "field": "fieldName", "message": "Error details" }
  ]
}

// Paginated Response
{
  "success": true,
  "data": [
{ /* items */ }
  ],
  "total": 100,
  "page": 1,
  "pageSize": 10,
  "hasMore": true
}
```

## JWT Token Handling

Your backend should return JWT token on login:

```typescript
// Login Response
{
  "success": true,
  "data": {
    "token": {
      "accessToken": "eyJhbGc...",
      "refreshToken": "eyJhbGc...",
      "expiresIn": 3600,
      "tokenType": "Bearer"
    },
    "user": {
      "id": "user123",
    "username": "john.doe",
      "email": "john@example.com",
      "firstName": "John",
      "lastName": "Doe",
      "role": "operator",
      "permissions": ["claim:view", "claim:manage"],
      "isActive": true
    }
  }
}
```

---

# DEPLOYMENT & PRODUCTION

## Build for Production

```bash
# Install dependencies
npm install

# Build for production
ng build --configuration production

# Output: dist/rcm-portal-antd/
```

## Docker Deployment

**File**: `Dockerfile`

```dockerfile
# Build stage
FROM node:18-alpine AS builder
WORKDIR /app
COPY package*.json ./
RUN npm ci
COPY . .
RUN npm run build

# Runtime stage
FROM node:18-alpine
WORKDIR /app
RUN npm install -g http-server
COPY --from=builder /app/dist/rcm-portal-antd ./dist
EXPOSE 80
CMD ["http-server", "dist", "-p", "80"]
```

**File**: `docker-compose.yml`

```yaml
version: '3.8'
services:
  rcm-portal:
    build: .
    ports:
      - "3000:80"
    environment:
      - API_URL=http://localhost:5000/api
    depends_on:
      - backend-api
  
backend-api:
    image: your-backend-api:latest
  ports:
      - "5000:5000"
    environment:
      - ASPNETCORE_ENVIRONMENT=Production
```

## Deployment Commands

```bash
# Local Docker deployment
docker-compose up -d

# Visit http://localhost:3000

# Production deployment to Azure
az webapp deployment source config-zip \
  --resource-group myResourceGroup \
  --name myAppName \
  --src-path dist.zip
```

## Environment Setup

**Production environment variables**:
```bash
# .env.production
API_URL=https://api.yourdomain.com/api
APP_VERSION=1.0.0
ENVIRONMENT=production
```

---

## ?? **DEVELOPMENT TIMELINE**

```
Week 1:
  Day 1-2: Project foundation & architecture
  Day 3-4: Authentication module
  Day 5: Dashboard & layout

Week 2:
  Day 1-3: Claims management module
  Day 4-5: Eligibility verification

Week 3:
  Day 1-2: Pre-authorization module
  Day 3-5: Reports & analytics

Week 4:
  Day 1-3: Testing & optimization
Day 4-5: Documentation & deployment prep

Week 5:
  Day 1-2: Production build & deployment
  Day 3-5: Post-launch monitoring & fixes

TOTAL: 5 weeks for complete MVP
```

---

## ?? **NEXT IMMEDIATE ACTIONS**

1. **Verify Setup** (5 min)
   ```bash
   nvm use 18
   node --version  # v18.x.x
   npm --version   # 9.x.x
   ng version      # 18.x.x
   ```

2. **Create Project** (10 min)
   ```bash
   ng new rcm-portal-antd --routing --style=less --package-manager=npm
   cd rcm-portal-antd
   ng add ng-zorro-antd
   ```

3. **Follow Phases** (Systematically)
   - Start with Phase 1: Project Foundation
   - Commit after each phase
   - Test frequently
   - Verify backend integration

4. **Integrate Backend** (Ongoing)
   - Update environment.ts with backend URL
   - Test each API endpoint
 - Handle errors gracefully
   - Monitor console for issues

---

## ?? **KEY FILES TO REMEMBER**

```
Important Files:
?? src/environments/environment.ts (API configuration)
?? src/app/app.module.ts (Main module)
?? src/app/app-routing.module.ts (Routing)
?? src/app/core/services/ (Business logic)
?? src/app/features/ (Feature modules)
?? src/theme.less (Styling)
?? angular.json (Build config)
```

---

## ? **YOU'RE READY!**

You now have a complete, phase-by-phase development roadmap for your Angular + Ant Design RCM Portal integrated with your .NET 9 backend.

**Start with Phase 1 and follow systematically!**

---

**Last Updated**: January 2024  
**Status**: ? PRODUCTION READY  
**Framework**: Angular 18 + Ant Design  
**Backend**: .NET 9  
**Timeline**: 5 weeks MVP  

?? **Let's build your RCM Portal!** ??

