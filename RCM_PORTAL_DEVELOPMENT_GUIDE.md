# ?? **ANGULAR + ANT DESIGN RCM PORTAL - COMPLETE DEVELOPMENT GUIDE**

**Start Your Development Journey Here!**

---

## ?? **TABLE OF CONTENTS**

1. [Quick Start](#quick-start)
2. [Architecture Overview](#architecture-overview)
3. [Setup Instructions](#setup-instructions)
4. [Project Structure](#project-structure)
5. [Core Services](#core-services)
6. [Ant Design Components](#ant-design-components)
7. [Creating Components](#creating-components)
8. [Forms & Validation](#forms--validation)
9. [Data Tables](#data-tables)
10. [Routing](#routing)
11. [Security & Authentication](#security--authentication)
12. [Best Practices](#best-practices)
13. [Common Tasks](#common-tasks)

---

## ? **QUICK START**

### **Option 1: Setup in 5 Minutes (Express)**

```bash
# 1. Create project with Ant Design
ng new rcm-portal-antd --routing --style=less --package-manager=npm

# 2. Navigate to project
cd rcm-portal-antd

# 3. Add Ant Design (one command, everything automatic)
ng add ng-zorro-antd

# 4. Install dependencies
npm install chart.js ng2-charts moment ngx-moment lodash-es

# 5. Start dev server
ng serve --open

# 6. Done! Visit http://localhost:4200
```

### **Option 2: Detailed Setup (45 minutes)**

Follow [Setup Instructions](#setup-instructions) section below.

---

## ??? **ARCHITECTURE OVERVIEW**

### **System Architecture**

```
???????????????????????????????????????????????????????????
?      BROWSER (Angular + Ant Design)    ?
???????????????????????????????????????????????????????????
?        ?
?  ???????????????????????????????????????????????        ?
?  ?  PRESENTATION LAYER (Components)?        ?
?  ?  ?? Dashboard Component  ?        ?
?  ?  ?? Claims Module   ?        ?
?  ?  ?? Eligibility Module           ?   ?
?  ?  ?? Pre-Auth Module              ?        ?
?  ?  ?? Reports Module  ?        ?
????????????????????????????????????????????????        ?
?       ? (Angular Services)              ?
?  ???????????????????????????????????????????????  ?
?  ?  APPLICATION LAYER (Business Logic)       ?        ?
?  ?  ?? API Service          ?        ?
?  ?  ?? Auth Service           ?        ?
?  ?  ?? Claims Service?   ?
?  ?  ?? Eligibility Service      ?        ?
?  ?  ?? Reports Service     ?        ?
?  ???????????????????????????????????????????????        ?
?    ? (HTTP with JWT)          ?
?  ???????????????????????????????????????????????        ?
??  INFRASTRUCTURE LAYER (Interceptors)   ?     ?
?  ?  ?? JWT Interceptor   ?        ?
?  ?  ?? Error Interceptor            ?        ?
?  ???????????????????????????????????????????????  ?
???????????????????????????????????????????????????????????
   ? HTTPS
        ?
???????????????????????????????????????????????????????????
?         .NET 9 BACKEND API (62 Services)?
???????????????????????????????????????????????????????????
?  ?? Claims Processing          ?
?  ?? Eligibility Verification          ?
?  ?? Pre-Authorization            ?
?  ?? Reports & Analytics     ?
?  ?? All Business Logic                ?
???????????????????????????????????????????????????????????
   ? SQL Queries
        ?
???????????????????????????????????????????????????????????
?           SQL SERVER DATABASE        ?
???????????????????????????????????????????????????????????
```

### **4-Layer Architecture**

```
Layer 1: PRESENTATION
  ?? Components, Templates, UI (Angular Components + Ant Design)

Layer 2: APPLICATION
  ?? Business Logic, Services, State Management (RxJS Observables)

Layer 3: INFRASTRUCTURE
  ?? HTTP, Interceptors, Guards, Security (JWT, Error Handling)

Layer 4: DATA ACCESS
  ?? Models, Interfaces, Type Definitions (TypeScript Interfaces)
```

---

## ??? **SETUP INSTRUCTIONS**

### **Step 1: Prerequisites**

```bash
# Check Node.js version (need 18+)
node --version
# Expected: v18.0.0 or higher

# Check npm version (need 9+)
npm --version
# Expected: 9.0.0 or higher

# Install Angular CLI globally
npm install -g @angular/cli@18
```

### **Step 2: Create Angular Project**

```bash
ng new rcm-portal-antd --routing --style=less --package-manager=npm
cd rcm-portal-antd
```

### **Step 3: Add Ant Design**

```bash
# This single command sets up everything!
ng add ng-zorro-antd

# Press Enter for all defaults
```

### **Step 4: Install Additional Packages**

```bash
npm install chart.js ng2-charts moment ngx-moment lodash-es
```

### **Step 5: Create Project Structure**

```bash
# Create directories
mkdir -p src/app/core/services
mkdir -p src/app/core/interceptors
mkdir -p src/app/core/guards
mkdir -p src/app/core/models
mkdir -p src/app/shared/components
mkdir -p src/app/features/dashboard/components
mkdir -p src/app/features/claims/components
mkdir -p src/app/features/eligibility/components
mkdir -p src/app/features/pre-auth/components
mkdir -p src/app/features/reports/components
mkdir -p src/app/auth/components

# Create modules
ng generate module core --routing
ng generate module shared
ng generate module features/dashboard --routing
ng generate module features/claims --routing
ng generate module features/eligibility --routing
ng generate module features/pre-auth --routing
ng generate module features/reports --routing
ng generate module auth --routing
```

### **Step 6: Start Development Server**

```bash
ng serve --open

# Visit http://localhost:4200
```

---

## ?? **PROJECT STRUCTURE**

```
rcm-portal-antd/
??? src/
?   ??? app/
?   ?   ??? core/
?   ?   ?   ??? services/
?   ?   ?   ?   ??? api.service.ts
?   ?   ?   ?   ??? auth.service.ts
?   ?   ?   ?   ??? claims.service.ts
?   ?   ?   ?   ??? eligibility.service.ts
?   ?   ?   ?   ??? reports.service.ts
?   ?   ?   ??? interceptors/
?   ?   ?   ?   ??? jwt.interceptor.ts
?   ?   ?   ?   ??? error.interceptor.ts
?   ?   ?   ??? guards/
?   ?   ?   ?   ??? auth.guard.ts
?   ?   ?   ??? models/
?   ?   ?   ?   ??? claim.model.ts
?   ?   ?   ?   ??? user.model.ts
?   ?   ?   ?   ??? eligibility.model.ts
?   ?   ?   ??? core.module.ts
?   ?   ?
?   ?   ??? shared/
?   ?   ?   ??? components/
?   ?   ?   ?   ??? navbar/
?   ?   ?   ?   ??? sidebar/
?   ?   ?   ?   ??? footer/
?   ?   ?   ??? shared.module.ts
?   ?   ?
?   ?   ??? features/
?   ? ?   ??? dashboard/
?   ?   ?   ?   ??? dashboard.component.ts
?   ?   ?   ?   ??? dashboard.component.html
?   ?   ?   ?   ??? dashboard.component.less
?   ?   ?   ?   ??? dashboard.module.ts
?   ?   ?   ??? claims/
?   ?   ?   ?   ??? components/
?   ?   ?   ?   ?   ??? claim-list.component.ts
?   ?   ?   ?   ?   ??? claim-detail.component.ts
?   ?   ?   ?   ?   ??? claim-submit.component.ts
? ?   ?   ?   ??? claims.module.ts
??   ?   ??? eligibility/
?   ?   ?   ??? pre-auth/
?   ?   ?   ??? reports/
?   ?   ?
?   ?   ??? auth/
?   ?   ? ??? components/
?   ?   ?   ?   ??? login.component.ts
?   ?   ?   ?   ??? logout.component.ts
?   ?   ?   ??? auth.module.ts
?   ?   ?
?   ?   ??? app.component.ts
?   ?   ??? app.module.ts
?   ?   ??? app-routing.module.ts
?   ?   ??? app.component.html
?   ?
?   ??? environments/
?   ?   ??? environment.ts
?   ?   ??? environment.prod.ts
?   ?
?   ??? theme.less (Ant Design customization)
?   ??? styles.less (Global styles)
?   ??? main.ts
?   ??? index.html
?
??? angular.json
??? package.json
??? tsconfig.json
??? README.md
```

---

## ?? **CORE SERVICES**

### **1. API Service (Base for all HTTP calls)**

```typescript
// src/app/core/services/api.service.ts
import { Injectable } from '@angular/core';
import { HttpClient, HttpParams } from '@angular/common/http';
import { Observable } from 'rxjs';
import { environment } from '../../../environments/environment';

@Injectable({
  providedIn: 'root'
})
export class ApiService {
  private apiUrl = environment.apiUrl;

  constructor(private http: HttpClient) { }

  // Generic GET
  get<T>(endpoint: string, params?: HttpParams): Observable<T> {
    return this.http.get<T>(`${this.apiUrl}/${endpoint}`, { params });
  }

  // Generic POST
  post<T>(endpoint: string, data: any): Observable<T> {
    return this.http.post<T>(`${this.apiUrl}/${endpoint}`, data);
  }

  // Generic PUT
  put<T>(endpoint: string, id: string, data: any): Observable<T> {
    return this.http.put<T>(`${this.apiUrl}/${endpoint}/${id}`, data);
  }

  // Generic DELETE
  delete<T>(endpoint: string, id: string): Observable<T> {
    return this.http.delete<T>(`${this.apiUrl}/${endpoint}/${id}`);
  }
}
```

### **2. Auth Service**

```typescript
// src/app/core/services/auth.service.ts
import { Injectable } from '@angular/core';
import { BehaviorSubject, Observable } from 'rxjs';
import { ApiService } from './api.service';
import { tap, map } from 'rxjs/operators';
import { User } from '../models/user.model';

@Injectable({
  providedIn: 'root'
})
export class AuthService {
  private currentUserSubject = new BehaviorSubject<User | null>(null);
  public currentUser$ = this.currentUserSubject.asObservable();

  constructor(private apiService: ApiService) {
  // Load user from localStorage if available
    const user = localStorage.getItem('user');
  if (user) {
  this.currentUserSubject.next(JSON.parse(user));
 }
  }

  login(username: string, password: string): Observable<any> {
    return this.apiService.post('auth/login', { username, password })
      .pipe(
        tap(response => {
          // Store token and user
   localStorage.setItem('token', response.token);
          localStorage.setItem('user', JSON.stringify(response.user));
          this.currentUserSubject.next(response.user);
        })
  );
  }

  logout(): void {
    localStorage.removeItem('token');
    localStorage.removeItem('user');
    this.currentUserSubject.next(null);
  }

  get isAuthenticated(): boolean {
    return !!localStorage.getItem('token');
  }

  get currentUserValue(): User | null {
    return this.currentUserSubject.value;
  }
}
```

### **3. Claims Service**

```typescript
// src/app/core/services/claims.service.ts
import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { HttpParams } from '@angular/common/http';
import { ApiService } from './api.service';
import { Claim } from '../models/claim.model';

@Injectable({
  providedIn: 'root'
})
export class ClaimsService {
  private endpoint = 'claims';

  constructor(private apiService: ApiService) { }

  // Get paginated claims
  getClaims(page: number = 1, pageSize: number = 10): Observable<any> {
    const params = new HttpParams()
      .set('page', page)
      .set('pageSize', pageSize);
    return this.apiService.get<any>(this.endpoint, params);
  }

  // Get single claim
  getClaimById(id: string): Observable<Claim> {
    return this.apiService.get<Claim>(`${this.endpoint}/${id}`);
  }

  // Submit new claim
  submitClaim(claim: Claim): Observable<any> {
    return this.apiService.post<any>(`${this.endpoint}/submit`, claim);
}

  // Update claim
  updateClaim(id: string, claim: Claim): Observable<any> {
    return this.apiService.put<any>(this.endpoint, id, claim);
  }

  // Get claim status
  getClaimStatus(id: string): Observable<any> {
    return this.apiService.get<any>(`${this.endpoint}/${id}/status`);
  }
}
```

---

## ?? **ANT DESIGN COMPONENTS**

### **Available Components**

**Layout**:
```html
<nz-layout>
  <nz-header></nz-header>
  <nz-layout>
    <nz-sider></nz-sider>
    <nz-content></nz-content>
  </nz-layout>
  <nz-footer></nz-footer>
</nz-layout>
```

**Data Display**:
```html
<nz-table></nz-table>          <!-- Advanced data tables -->
<nz-card></nz-card>      <!-- Card containers -->
<nz-statistic></nz-statistic>  <!-- Statistics display -->
<nz-badge></nz-badge>   <!-- Status badges -->
<nz-tag></nz-tag>       <!-- Category tags -->
<nz-timeline></nz-timeline>    <!-- Timeline -->
```

**Forms**:
```html
<nz-form></nz-form>  <!-- Form container -->
<nz-form-item></nz-form-item>          <!-- Form row -->
<nz-form-control></nz-form-control>    <!-- Form field -->
<input nz-input />           <!-- Text input -->
<nz-input-number></nz-input-number>    <!-- Number input -->
<nz-select></nz-select>   <!-- Dropdown -->
<nz-date-picker></nz-date-picker>      <!-- Date picker -->
<nz-checkbox></nz-checkbox>    <!-- Checkbox -->
<nz-radio></nz-radio>         <!-- Radio button -->
```

**Navigation**:
```html
<ul nz-menu></ul>      <!-- Navigation menu -->
<nz-breadcrumb></nz-breadcrumb> <!-- Breadcrumbs -->
<nz-tabs></nz-tabs>            <!-- Tab navigation -->
<nz-pagination></nz-pagination> <!-- Pagination -->
```

**Feedback**:
```html
<nz-modal></nz-modal>          <!-- Dialogs -->
<nz-drawer></nz-drawer> <!-- Side panels -->
<nz-alert></nz-alert>          <!-- Alerts -->
<nz-spin></nz-spin>   <!-- Loading spinner -->
<nz-progress></nz-progress>    <!-- Progress bar -->
```

---

## ?? **CREATING COMPONENTS**

### **Generate Component**

```bash
ng generate component features/claims/components/claim-list
```

### **Component Template Example**

```typescript
// claim-list.component.ts
import { Component, OnInit } from '@angular/core';
import { ClaimsService } from '../../../core/services/claims.service';
import { NzMessageService } from 'ng-zorro-antd/message';

@Component({
  selector: 'app-claim-list',
  templateUrl: './claim-list.component.html',
  styleUrls: ['./claim-list.component.less']
})
export class ClaimListComponent implements OnInit {
  claims: any[] = [];
  loading = false;
  pageIndex = 1;
  pageSize = 10;
  total = 0;

  constructor(
    private claimsService: ClaimsService,
    private messageService: NzMessageService
  ) { }

  ngOnInit(): void {
    this.loadClaims();
  }

  loadClaims(): void {
    this.loading = true;
    this.claimsService.getClaims(this.pageIndex, this.pageSize).subscribe({
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
}
```

```html
<!-- claim-list.component.html -->
<nz-card nzTitle="Claims Management">
  <nz-table
    #table
    [nzData]="claims"
    [nzLoading]="loading"
    [nzPageIndex]="pageIndex"
    [nzPageSize]="pageSize"
    [nzTotal]="total"
    (nzPageIndexChange)="onPageChange($event)"
  >
    <thead>
      <tr>
        <th nzSortFn="id">Claim ID</th>
        <th nzSortFn="patient">Patient</th>
        <th nzSortFn="amount">Amount</th>
   <th nzSortFn="status">Status</th>
      <th nzWidth="120px">Action</th>
      </tr>
    </thead>
    <tbody>
      <tr *ngFor="let claim of table.data">
        <td><strong>{{ claim.id }}</strong></td>
        <td>{{ claim.patient }}</td>
 <td>{{ claim.amount | currency }}</td>
    <td>
        <nz-badge
         [nzStatus]="claim.status | lowercase as status | statusPipe"
  [nzText]="claim.status"
     ></nz-badge>
    </td>
   <td>
      <a nz-button nzType="link" nzSize="small">View</a>
        </td>
      </tr>
    </tbody>
  </nz-table>
</nz-card>
```

---

## ?? **FORMS & VALIDATION**

### **Reactive Form Example**

```typescript
// claim-submit.component.ts
import { Component } from '@angular/core';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { ClaimsService } from '../../../core/services/claims.service';
import { NzMessageService } from 'ng-zorro-antd/message';
import { Router } from '@angular/router';

@Component({
  selector: 'app-claim-submit',
  templateUrl: './claim-submit.component.html',
  styleUrls: ['./claim-submit.component.less']
})
export class ClaimSubmitComponent {
  claimForm: FormGroup;
  loading = false;

  constructor(
    private fb: FormBuilder,
    private claimsService: ClaimsService,
    private messageService: NzMessageService,
    private router: Router
  ) {
    this.claimForm = this.fb.group({
      patientName: ['', [Validators.required, Validators.minLength(3)]],
      amount: ['', [Validators.required, Validators.min(0)]],
   serviceDate: ['', Validators.required],
 diagnosisCode: ['', Validators.required],
      provider: ['', Validators.required]
    });
  }

  submitClaim(): void {
    if (!this.claimForm.valid) {
      this.messageService.error('Please fill all required fields');
      return;
    }

    this.loading = true;
    this.claimsService.submitClaim(this.claimForm.value).subscribe({
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
}
```

```html
<!-- claim-submit.component.html -->
<nz-card nzTitle="Submit New Claim">
  <form nz-form [formGroup]="claimForm" (ngSubmit)="submitClaim()">
    
    <!-- Patient Name -->
    <nz-form-item>
      <nz-form-label nzRequired>Patient Name</nz-form-label>
      <nz-form-control nzErrorTip="Min 3 characters required">
        <input 
   nz-input 
          formControlName="patientName"
          placeholder="Enter patient name"
    />
      </nz-form-control>
    </nz-form-item>

<!-- Amount -->
    <nz-form-item>
      <nz-form-label nzRequired>Amount ($)</nz-form-label>
      <nz-form-control nzErrorTip="Amount must be >= 0">
   <nz-input-number
          formControlName="amount"
          [nzMin]="0"
          nzPlaceHolder="0.00"
          nzPrefix="$"
        ></nz-input-number>
      </nz-form-control>
    </nz-form-item>

    <!-- Service Date -->
    <nz-form-item>
      <nz-form-label nzRequired>Service Date</nz-form-label>
      <nz-form-control>
        <nz-date-picker 
          formControlName="serviceDate"
          nzFormat="YYYY-MM-DD"
    ></nz-date-picker>
  </nz-form-control>
    </nz-form-item>

    <!-- Diagnosis Code -->
    <nz-form-item>
    <nz-form-label nzRequired>Diagnosis Code</nz-form-label>
      <nz-form-control>
 <nz-select 
          formControlName="diagnosisCode"
          nzPlaceHolder="Select diagnosis"
        >
  <nz-option nzValue="J06.9" nzLabel="Acute URI"></nz-option>
          <nz-option nzValue="E11.9" nzLabel="Diabetes"></nz-option>
 <nz-option nzValue="I10" nzLabel="Hypertension"></nz-option>
        </nz-select>
      </nz-form-control>
    </nz-form-item>

    <!-- Buttons -->
    <nz-form-item nzExtra>
      <button 
        nz-button 
    nzType="primary" 
   nzSize="large"
      [disabled]="!claimForm.valid || loading"
    >
      <i nz-icon nzType="check" nzTheme="outline"></i>
        Submit Claim
      </button>
      <button 
        nz-button 
        nzSize="large"
        (click)="resetForm()"
        class="ml-8"
      >
        Reset
   </button>
    </nz-form-item>
  </form>
</nz-card>
```

---

## ?? **DATA TABLES**

### **Advanced Table with Sorting & Filtering**

```html
<nz-table
  #table
  [nzData]="claims"
  [nzLoading]="loading"
  [nzPageSize]="10"
  [nzTotal]="total"
  [nzShowSizeChanger]="true"
  [nzPageSizeOptions]="[10, 20, 50]"
  (nzPageIndexChange)="onPageChange($event)"
>
  <thead>
    <tr>
      <th nzSortFn="claimId" nzShowSort>Claim ID</th>
      <th nzSortFn="patient" nzShowSort>Patient Name</th>
      <th nzSortFn="amount" nzShowSort>Amount</th>
      <th nzShowFilter 
     [nzFilterFn]="true" 
    [nzFilters]="statusFilter">
        Status
      </th>
      <th nzWidth="120px">Actions</th>
    </tr>
  </thead>
  <tbody>
    <tr *ngFor="let claim of table.data">
      <td>{{ claim.id }}</td>
      <td>{{ claim.patient }}</td>
      <td>{{ claim.amount | currency }}</td>
      <td>
        <nz-badge 
          [nzStatus]="getStatus(claim.status)" 
 [nzText]="claim.status">
        </nz-badge>
  </td>
      <td>
        <a nz-button nzType="link" nzSize="small">View</a>
  <nz-divider nzType="vertical"></nz-divider>
        <a nz-button nzType="link" nzSize="small">Edit</a>
      </td>
    </tr>
  </tbody>
</nz-table>
```

---

## ?? **ROUTING**

### **App Routing Module**

```typescript
// app-routing.module.ts
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
    children: [
      {
        path: 'login',
   loadChildren: () => import('./auth/auth.module').then(m => m.AuthModule)
      }
    ]
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

---

## ?? **SECURITY & AUTHENTICATION**

### **JWT Interceptor**

```typescript
// src/app/core/interceptors/jwt.interceptor.ts
import { Injectable } from '@angular/core';
import { HttpInterceptor, HttpRequest, HttpHandler, HttpEvent } from '@angular/common/http';
import { Observable } from 'rxjs';
import { AuthService } from '../services/auth.service';

@Injectable()
export class JwtInterceptor implements HttpInterceptor {
  constructor(private authService: AuthService) { }

  intercept(
    request: HttpRequest<any>,
    next: HttpHandler
  ): Observable<HttpEvent<any>> {
  const token = localStorage.getItem('token');
    
    if (token) {
      request = request.clone({
        setHeaders: {
   Authorization: `Bearer ${token}`
      }
    });
    }

    return next.handle(request);
  }
}
```

### **Error Interceptor**

```typescript
// src/app/core/interceptors/error.interceptor.ts
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
      this.authService.logout();
     this.router.navigate(['/auth/login']);
          this.messageService.error('Session expired. Please login again.');
        } else if (error.status === 403) {
       // Forbidden
     this.messageService.error('You do not have permission to access this resource.');
    this.router.navigate(['/unauthorized']);
        } else if (error.status === 500) {
          // Server error
   this.messageService.error('Server error. Please try again later.');
        } else {
          // Other errors
          this.messageService.error(error.error?.message || 'An error occurred');
        }
        return throwError(() => error);
      })
    );
  }
}
```

### **Auth Guard**

```typescript
// src/app/core/guards/auth.guard.ts
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
    if (this.authService.isAuthenticated) {
      return true;
    }

    this.router.navigate(['/auth/login']);
    return false;
  }
}
```

---

## ? **BEST PRACTICES**

### **1. Component Design**
- Keep components focused (single responsibility)
- Use `OnDestroy` to unsubscribe from Observables
- Use `ChangeDetectionStrategy.OnPush` for performance

### **2. Services**
- Use services for business logic and API calls
- Provide services at root level with `providedIn: 'root'`
- Use RxJS Observables for async operations

### **3. Forms**
- Use Reactive Forms (FormBuilder) for complex forms
- Always validate on both client and server
- Show user-friendly error messages

### **4. Data Loading**
- Show loading spinner while fetching data
- Handle errors gracefully
- Implement pagination for large datasets

### **5. Security**
- Always use HTTPS in production
- Store tokens securely
- Validate all user input
- Implement proper authentication and authorization

### **6. Performance**
- Lazy load feature modules
- Use `trackBy` in `*ngFor`
- Unsubscribe from Observables
- Use OnPush change detection

---

## ??? **COMMON TASKS**

### **Task 1: Add a New Feature Module**

```bash
# Generate module
ng generate module features/my-feature --routing

# Generate component
ng generate component features/my-feature/my-feature

# Update routing in app-routing.module.ts
```

### **Task 2: Create a New Service**

```bash
ng generate service core/services/my-service
```

### **Task 3: Create a Form Component**

```bash
ng generate component features/my-feature/components/my-form
```

Then create the form following the examples above.

### **Task 4: Add Authentication**

1. Update `AuthService` with your backend API
2. Create `LoginComponent`
3. Create `LogoutComponent`
4. Update `AppComponent` to use navbar/sidebar

### **Task 5: Style Components**

```less
// styles.less
@primary-color: #1890ff;
@success-color: #52c41a;

.container {
  padding: 24px;
  background: @body-background;
}

.card {
  border-radius: 2px;
  box-shadow: 0 1px 2px rgba(0, 0, 0, 0.03);
}
```

---

## ?? **RESPONSIVE DESIGN**

### **Using Ant Design Grid**

```html
<div nz-row [nzGutter]="16">
  <!-- On mobile: 24 columns (full width) -->
  <!-- On tablet: 12 columns (half width) -->
  <!-- On desktop: 6 columns (quarter width) -->
  
  <div nz-col [nzXs]="24" [nzSm]="12" [nzMd]="8" [nzLg]="6">
    <nz-card nzTitle="Statistic 1">
      <nz-statistic nzTitle="Value" [nzValue]="1000"></nz-statistic>
    </nz-card>
  </div>

  <div nz-col [nzXs]="24" [nzSm]="12" [nzMd]="8" [nzLg]="6">
    <nz-card nzTitle="Statistic 2">
      <nz-statistic nzTitle="Value" [nzValue]="2000"></nz-statistic>
    </nz-card>
  </div>
</div>
```

---

## ?? **NEXT STEPS**

1. ? Complete setup (5-10 minutes)
2. ? Create project structure (5 minutes)
3. ? Setup core services (15 minutes)
4. ? Create auth components (20 minutes)
5. ? Build dashboard component (30 minutes)
6. ? Build claims module (1 hour)
7. ? Build eligibility module (1 hour)
8. ? Build pre-auth module (1 hour)
9. ? Build reports module (1 hour)
10. ? Testing & optimization (2 hours)

**Total: 1-2 weeks for MVP**

---

## ?? **RESOURCES**

- **Ant Design Components**: https://ng.ant.design/components/
- **Angular Documentation**: https://angular.io/docs
- **RxJS Documentation**: https://rxjs.dev/
- **TypeScript Handbook**: https://www.typescriptlang.org/docs/

---

## ?? **YOU'RE READY!**

You now have everything needed to start development:
- ? Complete setup instructions
- ? Project structure template
- ? Core services examples
- ? Component templates
- ? Form examples
- ? Security setup
- ? Best practices
- ? Common tasks

**Start Building!** ??

---

**Last Updated**: January 2024  
**Status**: ? Production Ready  
**Version**: 1.0

