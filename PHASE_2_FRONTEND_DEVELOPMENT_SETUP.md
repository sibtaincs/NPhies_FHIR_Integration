# 🚀 **PHASE 2: FRONTEND DEVELOPMENT SETUP - START HERE**

**Status**: ✅ Environment Ready  
**Version**: Phase 2 - Development Phase
**Updated**: January 2024

---

## ✅ **ENVIRONMENT VERIFICATION**

Your development environment is **100% ready**:

```
✅ Node.js:      v18.20.8
✅ npm:  10.8.2
✅ Angular CLI:    18.2.21
✅ Angular Core:   18.2.14
✅ TypeScript:     5.5.4
✅ RxJS:  7.8.2
```

---

## 📊 **PHASE 2 ROADMAP**

```
Phase 2: Core Architecture & Services (1-1.5 days)
├─ Step 1: Create TypeScript Models
├─ Step 2: Build API Service Layer
├─ Step 3: Implement Authentication Service
├─ Step 4: Setup HTTP Interceptors
├─ Step 5: Create Route Guards
└─ Step 6: Configure App Module & Routing
```

---

## 🎯 **STEP 1: CREATE TYPESCRIPT MODELS/INTERFACES**

### **Why Models Matter**

Models provide type safety and structure for your data. They act as contracts between frontend and backend.

### **Create Core Models Directory**

**Location**: `src/app/core/models/`

These files should already exist, but let's add the models.

### **1.1 User Model**

**File**: `src/app/core/models/user.model.ts`

```typescript
export interface User {
  id: string;
  email: string;
  firstName: string;
  lastName: string;
  role: 'admin' | 'user' | 'manager';
  department?: string;
  isActive: boolean;
  createdAt: Date;
  updatedAt: Date;
}

export interface LoginRequest {
  email: string;
  password: string;
}

export interface LoginResponse {
  success: boolean;
  token: string;
  user: User;
  message: string;
}

export interface AuthState {
  currentUser: User | null;
  isAuthenticated: boolean;
  token: string | null;
  loading: boolean;
  error: string | null;
}
```

**Command to create:**

```powershell
# In VS Code terminal (Ctrl + `)
# Models are typically just interfaces, so you can create them manually

# Or use Angular CLI for type safety
ng generate interface core/models/user
```

### **1.2 Claim Model**

**File**: `src/app/core/models/claim.model.ts`

```typescript
export interface Claim {
  id: string;
  claimNumber: string;
  patientId: string;
  patientName: string;
  serviceDate: Date;
  serviceEndDate?: Date;
  amount: number;
  procedureCode: string;
  procedureDescription: string;
  providerName: string;
  providerNPI: string;
  status: ClaimStatus;
  createdAt: Date;
  updatedAt: Date;
  notes?: string;
}

export type ClaimStatus = 'submitted' | 'pending' | 'approved' | 'denied' | 'paid';

export interface ClaimFilter {
  status?: ClaimStatus;
  patientId?: string;
  dateFrom?: Date;
  dateTo?: Date;
  amount?: { min: number; max: number };
}

export interface ClaimListResponse {
  success: boolean;
  data: Claim[];
  total: number;
  pageSize: number;
  pageNumber: number;
  message: string;
}

export interface ClaimDetailResponse {
  success: boolean;
  data: Claim;
  message: string;
}
```

### **1.3 Eligibility Model**

**File**: `src/app/core/models/eligibility.model.ts`

```typescript
export interface EligibilityRequest {
  subscriberId: string;
  memberName: string;
  dateOfBirth: Date;
  serviceDateFrom: Date;
  serviceDateTo?: Date;
  planId?: string;
  procedureCode?: string;
}

export interface EligibilityResponse {
  success: boolean;
  data: EligibilityData;
  message: string;
}

export interface EligibilityData {
  isEligible: boolean;
  memberId: string;
  memberName: string;
  groupNumber: string;
  planName: string;
  dateOfBirth: Date;
  effectiveDate: Date;
  terminationDate?: Date;
  copay?: number;
  deductible?: number;
  deductibleApplied: number;
  outOfPocketMaximum?: number;
  outOfPocketApplied: number;
  coverage?: {
    coverageType: string;
    coveragePercentage: number;
 limitationType: string;
    limitationAmount: number;
  }[];
  benefits?: {
    benefitType: string;
  covered: boolean;
    percentage: number;
    limitations?: string;
  }[];
}

export interface EligibilityHistory {
  id: string;
  requestDate: Date;
  eligibilityData: EligibilityData;
  status: 'verified' | 'pending' | 'failed';
}
```

### **1.4 API Response Model**

**File**: `src/app/core/models/api-response.model.ts`

```typescript
export interface ApiResponse<T> {
  success: boolean;
  data: T;
  message: string;
  timestamp: Date;
  errors?: ApiError[];
}

export interface ApiError {
  code: string;
  message: string;
  field?: string;
}

export interface PaginatedResponse<T> {
  success: boolean;
  data: T[];
  total: number;
  pageSize: number;
  pageNumber: number;
  totalPages: number;
  hasNextPage: boolean;
  hasPreviousPage: boolean;
  message: string;
}

export interface PageRequest {
  pageNumber: number;
  pageSize: number;
  sortBy?: string;
  sortDirection?: 'asc' | 'desc';
}
```

### **Commands to Create All Models**

```powershell
cd C:\Users\sibtain.alimadad\source\repos\rcm-portal-antd

# Create interface files (these are just TypeScript interfaces, not components)
# You can create them directly or use ng generate interface

ng generate interface core/models/user
ng generate interface core/models/claim
ng generate interface core/models/eligibility
ng generate interface core/models/api-response
```

**Then manually add the content from above to each file.**

---

## 🎯 **STEP 2: BUILD API SERVICE LAYER**

### **Why Service Layer?**

Services encapsulate HTTP calls and business logic, making components cleaner and code more reusable.

### **2.1 Create Base API Service**

**File**: `src/app/core/services/api.service.ts`

```typescript
import { Injectable } from '@angular/core';
import { HttpClient, HttpParams } from '@angular/common/http';
import { environment } from '../../../environments/environment';
import { Observable } from 'rxjs';
import { ApiResponse, PaginatedResponse, PageRequest } from '../models/api-response.model';

@Injectable({
  providedIn: 'root'
})
export class ApiService {
  private apiUrl = environment.apiUrl;

  constructor(private http: HttpClient) { }

  /**
   * GET request
*/
  get<T>(endpoint: string, params?: any): Observable<T> {
    let httpParams = new HttpParams();
 if (params) {
 Object.keys(params).forEach(key => {
      if (params[key] !== null && params[key] !== undefined) {
  httpParams = httpParams.set(key, params[key]);
        }
      });
  }
    return this.http.get<T>(`${this.apiUrl}${endpoint}`, { params: httpParams });
  }

  /**
   * POST request
   */
  post<T>(endpoint: string, data: any): Observable<T> {
    return this.http.post<T>(`${this.apiUrl}${endpoint}`, data);
  }

  /**
   * PUT request
   */
  put<T>(endpoint: string, data: any): Observable<T> {
    return this.http.put<T>(`${this.apiUrl}${endpoint}`, data);
  }

  /**
 * PATCH request
   */
  patch<T>(endpoint: string, data: any): Observable<T> {
    return this.http.patch<T>(`${this.apiUrl}${endpoint}`, data);
  }

  /**
   * DELETE request
   */
  delete<T>(endpoint: string): Observable<T> {
  return this.http.delete<T>(`${this.apiUrl}${endpoint}`);
  }
}
```

**Command:**

```powershell
ng generate service core/services/api
```

Then replace the content with code above.

### **2.2 Create Claims Service**

**File**: `src/app/core/services/claims.service.ts`

```typescript
import { Injectable } from '@angular/core';
import { ApiService } from './api.service';
import { Claim, ClaimFilter, ClaimListResponse, ClaimDetailResponse, PageRequest } from '../models/claim.model';
import { Observable } from 'rxjs';
import { environment } from '../../../environments/environment';

@Injectable({
  providedIn: 'root'
})
export class ClaimsService {
  private endpoint = environment.endpoints.claims.list;

  constructor(private apiService: ApiService) { }

  /**
   * Get all claims with pagination
   */
  getAll(pageRequest: PageRequest): Observable<ClaimListResponse> {
    return this.apiService.get<ClaimListResponse>(this.endpoint, {
    pageNumber: pageRequest.pageNumber,
      pageSize: pageRequest.pageSize,
sortBy: pageRequest.sortBy,
      sortDirection: pageRequest.sortDirection
  });
  }

  /**
   * Get claim by ID
   */
  getById(id: string): Observable<ClaimDetailResponse> {
    return this.apiService.get<ClaimDetailResponse>(`${environment.endpoints.claims.getById}/${id}`);
  }

  /**
   * Create new claim
   */
  create(claim: Claim): Observable<ClaimDetailResponse> {
    return this.apiService.post<ClaimDetailResponse>(environment.endpoints.claims.create, claim);
  }

  /**
   * Update claim
 */
  update(id: string, claim: Claim): Observable<ClaimDetailResponse> {
    return this.apiService.put<ClaimDetailResponse>(`${environment.endpoints.claims.update}/${id}`, claim);
  }

  /**
   * Delete claim
   */
  delete(id: string): Observable<any> {
    return this.apiService.delete<any>(`${this.endpoint}/${id}`);
  }

  /**
   * Get claims by patient
   */
  getByPatient(patientId: string): Observable<ClaimListResponse> {
    return this.apiService.get<ClaimListResponse>(
      `${environment.endpoints.claims.byPatient}/${patientId}`
    );
  }

  /**
   * Get claims by status
   */
  getByStatus(status: string): Observable<ClaimListResponse> {
    return this.apiService.get<ClaimListResponse>(
      `${environment.endpoints.claims.byStatus}/${status}`
  );
  }

  /**
   * Search claims with filters
 */
  search(filter: ClaimFilter): Observable<ClaimListResponse> {
    return this.apiService.get<ClaimListResponse>(
      environment.endpoints.claims.list,
      filter
    );
  }
}
```

**Command:**

```powershell
ng generate service core/services/claims
```

### **2.3 Create Eligibility Service**

**File**: `src/app/core/services/eligibility.service.ts`

```typescript
import { Injectable } from '@angular/core';
import { ApiService } from './api.service';
import { 
  EligibilityRequest, 
  EligibilityResponse, 
  EligibilityHistory 
} from '../models/eligibility.model';
import { Observable } from 'rxjs';
import { environment } from '../../../environments/environment';

@Injectable({
  providedIn: 'root'
})
export class EligibilityService {

  constructor(private apiService: ApiService) { }

  /**
   * Check eligibility
   */
  check(request: EligibilityRequest): Observable<EligibilityResponse> {
    return this.apiService.post<EligibilityResponse>(
    environment.endpoints.eligibility.check,
      request
    );
  }

  /**
   * Get eligibility requests
   */
  getRequests(): Observable<any> {
    return this.apiService.get<any>(environment.endpoints.eligibility.requests);
  }

  /**
   * Get eligibility responses
   */
  getResponses(): Observable<any> {
    return this.apiService.get<any>(environment.endpoints.eligibility.responses);
  }

  /**
   * Get pending requests
   */
  getPendingRequests(): Observable<any> {
    return this.apiService.get<any>(environment.endpoints.eligibility.pending);
  }
}
```

**Command:**

```powershell
ng generate service core/services/eligibility
```

---

## 🎯 **STEP 3: IMPLEMENT AUTHENTICATION SERVICE**

### **3.1 Auth Service**

**File**: `src/app/core/services/auth.service.ts`

```typescript
import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { BehaviorSubject, Observable } from 'rxjs';
import { tap, map } from 'rxjs/operators';
import { environment } from '../../../environments/environment';
import { User, LoginRequest, LoginResponse, AuthState } from '../models/user.model';

@Injectable({
  providedIn: 'root'
})
export class AuthService {
  private currentUser$ = new BehaviorSubject<User | null>(null);
  private isAuthenticated$ = new BehaviorSubject<boolean>(false);
  private tokenKey = environment.app.tokenKey;
  private userKey = environment.app.userKey;

  constructor(private http: HttpClient) {
    this.loadFromStorage();
  }

  /**
   * Login user
   */
  login(email: string, password: string): Observable<LoginResponse> {
    const request: LoginRequest = { email, password };
    return this.http.post<LoginResponse>(
      `${environment.apiUrl}${environment.endpoints.auth.login}`,
      request
    ).pipe(
 tap(response => {
        if (response && response.token) {
          this.setToken(response.token);
      this.setUser(response.user);
      this.currentUser$.next(response.user);
    this.isAuthenticated$.next(true);
        }
      })
    );
  }

  /**
   * Logout user
   */
  logout(): void {
    localStorage.removeItem(this.tokenKey);
    localStorage.removeItem(this.userKey);
    this.currentUser$.next(null);
    this.isAuthenticated$.next(false);
  }

  /**
   * Get current user
   */
  getCurrentUser(): Observable<User | null> {
    return this.currentUser$.asObservable();
  }

  /**
   * Check if authenticated
   */
  isAuthenticated(): Observable<boolean> {
    return this.isAuthenticated$.asObservable();
  }

  /**
   * Get current user synchronously
   */
  getCurrentUserSync(): User | null {
    return this.currentUser$.value;
  }

  /**
   * Get authentication token
   */
  getToken(): string | null {
    return localStorage.getItem(this.tokenKey);
  }

  /**
   * Refresh token
   */
  refreshToken(): Observable<LoginResponse> {
    return this.http.post<LoginResponse>(
      `${environment.apiUrl}${environment.endpoints.auth.refresh}`,
      {}
    ).pipe(
      tap(response => {
if (response && response.token) {
      this.setToken(response.token);
        }
      })
    );
  }

  /**
   * Get user info
*/
  getUserInfo(): Observable<User> {
    return this.http.get<User>(
      `${environment.apiUrl}${environment.endpoints.auth.me}`
    ).pipe(
      tap(user => {
        this.setUser(user);
        this.currentUser$.next(user);
      })
    );
  }

  /**
   * Set token in storage
   */
  private setToken(token: string): void {
    localStorage.setItem(this.tokenKey, token);
  }

  /**
   * Set user in storage
   */
  private setUser(user: User): void {
    localStorage.setItem(this.userKey, JSON.stringify(user));
  }

  /**
   * Load from storage on init
   */
  private loadFromStorage(): void {
    const token = localStorage.getItem(this.tokenKey);
    const user = localStorage.getItem(this.userKey);

    if (token && user) {
    try {
     const parsedUser = JSON.parse(user);
    this.currentUser$.next(parsedUser);
  this.isAuthenticated$.next(true);
    } catch (e) {
        localStorage.removeItem(this.userKey);
      }
    }
  }
}
```

**Command:**

```powershell
ng generate service core/services/auth
```

---

## 🎯 **STEP 4: SETUP HTTP INTERCEPTORS**

### **Why Interceptors?**

Interceptors automatically add auth tokens to requests and handle errors globally.

### **4.1 Auth Interceptor**

**File**: `src/app/core/interceptors/auth.interceptor.ts`

```typescript
import { Injectable } from '@angular/core';
import { HttpInterceptor, HttpRequest, HttpHandler, HttpEvent } from '@angular/common/http';
import { Observable } from 'rxjs';
import { AuthService } from '../services/auth.service';

@Injectable()
export class AuthInterceptor implements HttpInterceptor {
  constructor(private authService: AuthService) { }

intercept(req: HttpRequest<any>, next: HttpHandler): Observable<HttpEvent<any>> {
    const token = this.authService.getToken();

    if (token) {
   // Clone request and add authorization header
      req = req.clone({
      setHeaders: {
          Authorization: `Bearer ${token}`,
          'Content-Type': 'application/json'
   }
      });
    }

    return next.handle(req);
  }
}
```

**Command:**

```powershell
ng generate interceptor core/interceptors/auth
```

### **4.2 Error Interceptor**

**File**: `src/app/core/interceptors/error.interceptor.ts`

```typescript
import { Injectable, inject } from '@angular/core';
import { HttpInterceptor, HttpRequest, HttpHandler, HttpErrorResponse, HttpEvent } from '@angular/common/http';
import { Observable, throwError } from 'rxjs';
import { catchError } from 'rxjs/operators';
import { NzMessageService } from 'ng-zorro-antd/message';
import { AuthService } from '../services/auth.service';

@Injectable()
export class ErrorInterceptor implements HttpInterceptor {
  private message = inject(NzMessageService);
  private authService = inject(AuthService);

  intercept(req: HttpRequest<any>, next: HttpHandler): Observable<HttpEvent<any>> {
    return next.handle(req).pipe(
      catchError((error: HttpErrorResponse) => {
        let errorMessage = 'An error occurred';

        if (error.error instanceof ErrorEvent) {
   // Client-side error
    errorMessage = `Error: ${error.error.message}`;
        } else {
          // Server-side error
          errorMessage = error.error?.message || `Error Code: ${error.status}\nMessage: ${error.statusText}`;

        // Handle specific status codes
    switch (error.status) {
 case 401:
              // Unauthorized - redirect to login
       this.message.error('Session expired. Please login again.');
   this.authService.logout();
       break;

            case 403:
              this.message.error('You do not have permission to access this resource.');
   break;

            case 404:
       this.message.error('Resource not found.');
      break;

            case 500:
 this.message.error('Server error. Please try again later.');
          break;

   default:
            this.message.error(errorMessage);
      }
        }

      console.error('HTTP Error:', {
      status: error.status,
          message: errorMessage,
        error: error.error
        });

        return throwError(() => new Error(errorMessage));
      })
    );
  }
}
```

**Command:**

```powershell
ng generate interceptor core/interceptors/error
```

---

## 🎯 **STEP 5: CREATE ROUTE GUARDS**

### **5.1 Auth Guard**

**File**: `src/app/core/guards/auth.guard.ts`

```typescript
import { Injectable, inject } from '@angular/core';
import { CanActivate, Router, ActivatedRouteSnapshot, RouterStateSnapshot } from '@angular/router';
import { Observable } from 'rxjs';
import { AuthService } from '../services/auth.service';
import { map, take } from 'rxjs/operators';

@Injectable({
  providedIn: 'root'
})
export class AuthGuard implements CanActivate {
  private authService = inject(AuthService);
  private router = inject(Router);

  canActivate(
    route: ActivatedRouteSnapshot,
    state: RouterStateSnapshot
  ): Observable<boolean> {
    return this.authService.isAuthenticated().pipe(
      take(1),
      map(isAuthenticated => {
        if (isAuthenticated) {
    return true;
        } else {
          this.router.navigate(['/login'], { queryParams: { returnUrl: state.url } });
          return false;
        }
      })
    );
  }
}
```

**Command:**

```powershell
ng generate guard core/guards/auth
```

### **5.2 Role Guard (Optional)**

**File**: `src/app/core/guards/role.guard.ts`

```typescript
import { Injectable, inject } from '@angular/core';
import { CanActivate, Router, ActivatedRouteSnapshot } from '@angular/router';
import { Observable } from 'rxjs';
import { AuthService } from '../services/auth.service';
import { map, take } from 'rxjs/operators';
import { NzMessageService } from 'ng-zorro-antd/message';

@Injectable({
  providedIn: 'root'
})
export class RoleGuard implements CanActivate {
  private authService = inject(AuthService);
  private router = inject(Router);
  private message = inject(NzMessageService);

  canActivate(route: ActivatedRouteSnapshot): Observable<boolean> {
    return this.authService.getCurrentUser().pipe(
      take(1),
      map(user => {
        if (!user) {
       this.router.navigate(['/login']);
          return false;
        }

        const requiredRole = route.data['role'];
        if (requiredRole && user.role !== requiredRole) {
          this.message.error('You do not have permission to access this page.');
          this.router.navigate(['/dashboard']);
          return false;
        }

        return true;
      })
  );
  }
}
```

---

## 🎯 **STEP 6: UPDATE APP MODULE & ROUTING**

### **6.1 Update App Module**

**File**: `src/app/app.module.ts`

```typescript
import { NgModule } from '@angular/core';
import { BrowserModule } from '@angular/platform-browser';
import { BrowserAnimationsModule } from '@angular/platform-browser/animations';
import { HttpClientModule, HTTP_INTERCEPTORS } from '@angular/common/http';
import { CommonModule } from '@angular/common';

// Ant Design
import { NzMessageModule } from 'ng-zorro-antd/message';
import { NzNotificationModule } from 'ng-zorro-antd/notification';

// App
import { AppRoutingModule } from './app-routing.module';
import { AppComponent } from './app.component';

// Core
import { CoreModule } from './core/core.module';
import { AuthInterceptor } from './core/interceptors/auth.interceptor';
import { ErrorInterceptor } from './core/interceptors/error.interceptor';

// Shared
import { SharedModule } from './shared/shared.module';

@NgModule({
  declarations: [
 AppComponent
  ],
  imports: [
    BrowserModule,
    BrowserAnimationsModule,
  CommonModule,
    HttpClientModule,
    NzMessageModule,
    NzNotificationModule,
    AppRoutingModule,
    CoreModule,
    SharedModule
  ],
  providers: [
    { provide: HTTP_INTERCEPTORS, useClass: AuthInterceptor, multi: true },
    { provide: HTTP_INTERCEPTORS, useClass: ErrorInterceptor, multi: true }
  ],
  bootstrap: [AppComponent]
})
export class AppModule { }
```

### **6.2 Update Routing**

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
    path: 'login',
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
    canActivate: [AuthGuard]
  },
  {
    path: 'eligibility',
    loadChildren: () => import('./features/eligibility/eligibility.module').then(m => m.EligibilityModule),
    canActivate: [AuthGuard]
  },
  {
    path: 'pre-auth',
    loadChildren: () => import('./features/pre-auth/pre-auth.module').then(m => m.PreAuthModule),
    canActivate: [AuthGuard]
  },
  {
    path: 'reports',
    loadChildren: () => import('./features/reports/reports.module').then(m => m.ReportsModule),
    canActivate: [AuthGuard]
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

### **6.3 Create Core Module**

**File**: `src/app/core/core.module.ts`

```typescript
import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';

// Services
import { ApiService } from './services/api.service';
import { AuthService } from './services/auth.service';
import { ClaimsService } from './services/claims.service';
import { EligibilityService } from './services/eligibility.service';

// Guards
import { AuthGuard } from './guards/auth.guard';
import { RoleGuard } from './guards/role.guard';

@NgModule({
  declarations: [],
  imports: [CommonModule],
  providers: [
    ApiService,
    AuthService,
    ClaimsService,
    EligibilityService,
    AuthGuard,
RoleGuard
  ]
})
export class CoreModule { }
```

**Command:**

```powershell
ng generate module core
```

---

## 📋 **QUICK SETUP COMMANDS**

Run these commands in order:

```powershell
# Navigate to project
cd C:\Users\sibtain.alimadad\source\repos\rcm-portal-antd

# Create models
ng generate interface core/models/user
ng generate interface core/models/claim
ng generate interface core/models/eligibility
ng generate interface core/models/api-response

# Create services
ng generate service core/services/api
ng generate service core/services/auth
ng generate service core/services/claims
ng generate service core/services/eligibility

# Create interceptors
ng generate interceptor core/interceptors/auth
ng generate interceptor core/interceptors/error

# Create guards
ng generate guard core/guards/auth
ng generate guard core/guards/role

# Create core module
ng generate module core

# Create shared module
ng generate module shared
```

---

## ✅ **PHASE 2 COMPLETION CHECKLIST**

- [ ] TypeScript models created
  - [ ] User model
  - [ ] Claim model
  - [ ] Eligibility model
  - [ ] API response model

- [ ] API service layer built
  - [ ] Base API service
  - [ ] Claims service
  - [ ] Eligibility service

- [ ] Authentication implemented
  - [ ] Auth service
  - [ ] Login functionality
  - [ ] Token management

- [ ] Interceptors setup
  - [ ] Auth interceptor
  - [ ] Error interceptor

- [ ] Route guards created
  - [ ] Auth guard
  - [ ] Role guard (optional)

- [ ] App module updated
  - [ ] Providers added
  - [ ] Interceptors registered

- [ ] Routing configured
  - [ ] Routes defined
  - [ ] Guards applied

- [ ] Core module created
  - [ ] Services provided
  - [ ] Guards provided

---

## 🚀 **NEXT: START DEVELOPMENT SERVER**

Once all the above is complete:

```powershell
# Start dev server
npm start

# Or use batch file
.\start-server.bat

# App will be at http://localhost:4300
```

---

## 📊 **WHAT'S NEXT (Phase 3)**

After Phase 2 is complete:

✅ Phase 3: Build Feature Modules
- Create Dashboard
- Create Claims Module
- Create Eligibility Module
- Create Pre-Auth Module
- Create Reports Module

**Estimated Time**: 2-3 days

---

## 🎯 **SUCCESS CRITERIA**

Phase 2 is complete when:

✅ All models are created and typed  
✅ API service can make HTTP calls  
✅ Auth service handles login/logout  
✅ Interceptors add tokens to requests  
✅ Guards protect routes  
✅ App compiles without errors  
✅ No console errors when running  

---

## 💡 **BEST PRACTICES**

✅ Keep services focused and single-responsibility  
✅ Use typed interfaces for all data
✅ Handle errors globally via interceptors  
✅ Use guards to protect routes  
✅ Store sensitive data in services, not components  
✅ Use RxJS operators for async operations  
✅ Follow Angular naming conventions  
✅ Organize code by feature/domain  

---

## 🎊 **YOU'RE READY FOR PHASE 2!**

All your environment is set up and ready.

**Next Step**: Follow the 6 steps above to build Phase 2!

**Time Estimate**: 3-4 hours to complete all steps

**Result**: Fully functional authentication and service layer!

---

**Happy coding! 🚀**

In any component
constructor(
  private authService: AuthService,
  private claimsService: ClaimsService,
  private eligibilityService: EligibilityService
) { }

// Login
this.authService.login(email, password).subscribe(...)

// Get claims
this.claimsService.getAll({pageNumber: 1, pageSize: 10}).subscribe(...)

// Check eligibility
this.eligibilityService.check(request).subscribe(...)

