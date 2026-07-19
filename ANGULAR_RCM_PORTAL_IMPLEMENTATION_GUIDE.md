# ?? **RCM WEB PORTAL - ANGULAR IMPLEMENTATION GUIDE**

## ? **Angular RCM Portal - Comprehensive Plan**

**Status**: Ready to Implement  
**Technology**: Angular 18+ (Latest)  
**Backend**: .NET 9 API (Already Built)  
**Framework**: Angular with TypeScript  
**Database**: SQL Server (Existing)  

---

## ?? **PROJECT OVERVIEW**

### **What We're Building**

An enterprise-grade **Revenue Cycle Management (RCM) Web Portal** that provides:

? **Claims Management**
- Submit claims
- Track claim status
- View claim details
- Manage corrections & appeals

? **Eligibility Verification**
- Real-time member eligibility
- Coverage details
- Benefit information

? **Pre-Authorization**
- Submit pre-auth requests
- Track approvals
- Manage documentation

? **Reports & Analytics**
- Claims summary
- Financial reports
- Performance metrics
- Compliance tracking

? **Dashboard**
- KPI metrics
- Recent activity
- System health
- User alerts

---

## ??? **PROPOSED ARCHITECTURE**

```
???????????????????????????????????????????????????
?         Angular RCM Portal (Frontend)     ?
?  - Claims Management           ?
?  - Eligibility Verification   ?
?  - Pre-Authorization          ?
?  - Reports & Analytics  ?
???????????????????????????????????????????????????
  ? HTTP/REST API
???????????????????????????????????????????????????
?  .NET 9 API Service (Already Built)      ?
?  - 62 Business Services      ?
?  - Authentication & Authorization      ?
?  - Data Validation     ?
???????????????????????????????????????????????????
      ? EF Core
???????????????????????????????????????????????????
?      SQL Server Database   ?
?  - Claims Data      ?
?  - Eligibility Information           ?
?  - User Accounts   ?
???????????????????????????????????????????????????
```

---

## ?? **FOLDER STRUCTURE**

```
rcm-portal/ (Angular Project Root)
??? src/
?   ??? app/
?   ?   ??? core/               (Singleton services, guards)
?   ?   ?   ??? auth/
?   ?   ?   ?   ??? auth.service.ts
?   ?   ?   ?   ??? auth.guard.ts
?   ?   ?   ?   ??? jwt.interceptor.ts
? ?   ?   ??? api/
?   ?   ?   ?   ??? api.service.ts
?   ?   ?   ?   ??? http.interceptor.ts
?   ?   ?   ?   ??? error.interceptor.ts
?   ?   ?   ??? models/
?   ?   ?   ?   ??? claim.model.ts
?   ?   ?   ?   ??? eligibility.model.ts
?   ?   ?   ?   ??? user.model.ts
?   ?   ?   ??? services/
?   ?   ?       ??? claim.service.ts
?   ?   ?       ??? eligibility.service.ts
?   ?   ?     ??? auth.service.ts
?   ?   ?
?   ?   ??? shared/         (Reusable components)
?   ? ?   ??? components/
?   ?   ?   ?   ??? navbar/
?   ?   ?   ?   ??? sidebar/
?   ?   ?   ? ??? footer/
?   ?   ?   ?   ??? table/
?   ?   ?   ??? pipes/
?   ?   ?   ??? directives/
?   ?   ?   ??? shared.module.ts
?   ?   ?
?   ?   ??? features/   (Feature modules)
?   ?   ???? dashboard/
?   ?   ?   ?   ??? dashboard.component.ts
?   ?   ?   ?   ??? dashboard.component.html
?   ?   ? ?   ??? dashboard.module.ts
?   ?   ?   ??? claims/
?   ?   ?   ?   ??? claim-list/
?   ?   ?   ???? claim-detail/
?   ?   ?   ? ??? claim-submit/
?   ?   ?   ?   ??? claim-correction/
?   ? ?   ?   ??? claims.module.ts
?   ?   ?   ??? eligibility/
?   ?   ?   ?   ??? eligibility-check/
?   ?   ?   ?   ??? eligibility-detail/
?   ?   ?   ?   ??? eligibility.module.ts
?   ? ?   ??? pre-auth/
?   ?   ?   ?   ??? pre-auth-list/
?   ?   ?   ?   ??? pre-auth-submit/
?   ?   ?   ?   ??? pre-auth.module.ts
?   ?   ?   ??? reports/
?   ?   ?   ?   ??? reports-dashboard/
?   ?   ?   ?   ??? claims-report/
?   ?   ?   ?   ??? reports.module.ts
?   ?   ?   ??? settings/
?   ?   ?       ??? user-settings/
?   ?   ?       ??? settings.module.ts
?   ?   ?
?   ?   ??? auth/      (Auth module)
? ?   ?   ??? login/
?   ?   ?   ??? logout/
?   ?   ?   ??? auth.module.ts
? ?   ?
?   ?   ??? app.component.ts
?   ?   ??? app.component.html
?   ?   ??? app.routing.module.ts
?   ?   ??? app.module.ts
?   ?
?   ??? assets/
?   ?   ??? images/
?   ?   ??? styles/
?   ?   ??? data/
?   ?
?   ??? environments/
?   ?   ??? environment.ts
?   ?   ??? environment.prod.ts
?   ?
?   ??? index.html
?   ??? main.ts
?   ??? styles.scss
?   ??? styles.css
?
??? package.json
??? angular.json
??? tsconfig.json
??? README.md
??? .gitignore
```

---

## ?? **IMPLEMENTATION STEPS**

### **Step 1: Create Angular Project**

```bash
# Install Angular CLI
npm install -g @angular/cli

# Create new Angular project
ng new rcm-portal

# Navigate to project
cd rcm-portal

# Generate routing module
ng generate module app-routing --flat
```

### **Step 2: Install Dependencies**

```bash
npm install --save \
  @angular/common \
  @angular/platform-browser \
  @angular/platform-browser-dynamic \
  @angular/compiler \
  @angular/forms \
  @angular/http \
  @angular/router \
  rxjs \
  bootstrap \
  ng-bootstrap \
  chart.js \
  ngx-bootstrap \
  moment \
  lodash

# Install Bootstrap
npm install bootstrap --save

# Install ng-bootstrap
npm install ng-bootstrap --save
```

### **Step 3: Project Structure Creation**

```bash
# Core modules
ng generate module core
ng generate service core/auth/auth
ng generate guard core/auth/auth
ng generate service core/api/api
ng generate interceptor core/http/http

# Shared modules
ng generate module shared
ng generate component shared/navbar
ng generate component shared/sidebar
ng generate component shared/footer

# Feature modules
ng generate module features/dashboard
ng generate component features/dashboard/dashboard

ng generate module features/claims
ng generate component features/claims/claim-list
ng generate component features/claims/claim-detail
ng generate component features/claims/claim-submit

ng generate module features/eligibility
ng generate component features/eligibility/eligibility-check

ng generate module features/pre-auth
ng generate component features/pre-auth/pre-auth-list

ng generate module features/reports
ng generate component features/reports/reports-dashboard

# Models
ng generate interface core/models/claim
ng generate interface core/models/user
ng generate interface core/models/eligibility
```

---

## ?? **KEY SERVICE IMPLEMENTATIONS**

### **1. Authentication Service**

```typescript
// auth.service.ts
import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { BehaviorSubject, Observable } from 'rxjs';
import { map } from 'rxjs/operators';

@Injectable({
  providedIn: 'root'
})
export class AuthService {
  private currentUserSubject: BehaviorSubject<any>;
  public currentUser: Observable<any>;
  private apiUrl = 'https://api.nphies-rcm.health/api/auth';

  constructor(private http: HttpClient) {
    this.currentUserSubject = new BehaviorSubject<any>(
    JSON.parse(localStorage.getItem('currentUser') || '{}')
    );
    this.currentUser = this.currentUserSubject.asObservable();
  }

  public get currentUserValue(): any {
  return this.currentUserSubject.value;
  }

  login(username: string, password: string): Observable<any> {
    return this.http.post<any>(`${this.apiUrl}/login`, { username, password })
      .pipe(map(response => {
      if (response && response.token) {
          localStorage.setItem('currentUser', JSON.stringify(response));
          this.currentUserSubject.next(response);
        }
        return response;
      }));
  }

  logout() {
    localStorage.removeItem('currentUser');
    this.currentUserSubject.next(null);
  }

  register(user: any): Observable<any> {
    return this.http.post<any>(`${this.apiUrl}/register`, user);
  }
}
```

### **2. Claims Service**

```typescript
// claims.service.ts
import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';
import { ClaimModel } from '../models/claim.model';

@Injectable({
  providedIn: 'root'
})
export class ClaimsService {
  private apiUrl = 'https://api.nphies-rcm.health/api/claims';

  constructor(private http: HttpClient) {}

  // Get all claims
  getClaims(page: number = 1, pageSize: number = 10): Observable<any> {
    return this.http.get<any>(
      `${this.apiUrl}?page=${page}&pageSize=${pageSize}`
 );
  }

  // Get claim by ID
  getClaimById(id: string): Observable<ClaimModel> {
    return this.http.get<ClaimModel>(`${this.apiUrl}/${id}`);
  }

  // Submit new claim
  submitClaim(claim: ClaimModel): Observable<any> {
    return this.http.post<any>(`${this.apiUrl}/submit`, claim);
  }

  // Update claim
  updateClaim(id: string, claim: ClaimModel): Observable<any> {
    return this.http.put<any>(`${this.apiUrl}/${id}`, claim);
  }

  // Get claim status
  getClaimStatus(id: string): Observable<any> {
    return this.http.get<any>(`${this.apiUrl}/${id}/status`);
  }

  // Submit correction
  submitCorrection(id: string, correction: any): Observable<any> {
    return this.http.post<any>(`${this.apiUrl}/${id}/correction`, correction);
  }

  // Get claim history
  getClaimHistory(id: string): Observable<any> {
    return this.http.get<any>(`${this.apiUrl}/${id}/history`);
  }
}
```

### **3. Eligibility Service**

```typescript
// eligibility.service.ts
import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';

@Injectable({
  providedIn: 'root'
})
export class EligibilityService {
  private apiUrl = 'https://api.nphies-rcm.health/api/eligibility';

  constructor(private http: HttpClient) {}

  // Check eligibility
  checkEligibility(memberId: string, serviceDate?: string): Observable<any> {
    const params = serviceDate ? `?serviceDate=${serviceDate}` : '';
    return this.http.get<any>(`${this.apiUrl}/check/${memberId}${params}`);
  }

  // Get detailed eligibility
  getDetailedEligibility(memberId: string, serviceCode: string): Observable<any> {
    return this.http.get<any>(
      `${this.apiUrl}/details/${memberId}/${serviceCode}`
    );
  }

  // Verify coverage
  verifyCoverage(memberId: string, serviceCode: string): Observable<any> {
    return this.http.get<any>(
    `${this.apiUrl}/verify/${memberId}/${serviceCode}`
    );
  }
}
```

---

## ?? **UI COMPONENTS STRUCTURE**

### **Dashboard Component**

```typescript
// dashboard.component.ts
import { Component, OnInit } from '@angular/core';
import { ClaimsService } from '../../core/api/claims.service';

@Component({
  selector: 'app-dashboard',
  templateUrl: './dashboard.component.html',
  styleUrls: ['./dashboard.component.scss']
})
export class DashboardComponent implements OnInit {
  claimsCount = 0;
  approvedCount = 0;
  deniedCount = 0;
  pendingCount = 0;
  recentClaims: any[] = [];
  successRate = 0;

  constructor(private claimsService: ClaimsService) {}

  ngOnInit(): void {
    this.loadDashboardData();
  }

  loadDashboardData(): void {
    this.claimsService.getClaims(1, 5).subscribe({
      next: (data) => {
        this.recentClaims = data.claims;
        this.claimsCount = data.total;
        this.approvedCount = data.approved;
        this.deniedCount = data.denied;
   this.pendingCount = data.pending;
        this.successRate = ((this.approvedCount / this.claimsCount) * 100).toFixed(2);
      },
      error: (err) => console.error('Error loading dashboard', err)
    });
  }
}
```

---

## ?? **KEY FEATURES**

### **1. Claims Management**
- ? View all claims (paginated, searchable)
- ? Submit new claims (wizard-based)
- ? Track claim status real-time
- ? Manage corrections
- ? Submit appeals
- ? View claim history

### **2. Eligibility Verification**
- ? Real-time member eligibility check
- ? Service-specific coverage details
- ? Benefit information
- ? Deductible/OOP tracking
- ? Prior authorization requirements

### **3. Pre-Authorization**
- ? Submit pre-auth requests
- ? Track approval status
- ? Manage documentation
- ? View authorization details
- ? Monitor expiration dates

### **4. Reports & Analytics**
- ? Claims summary report
- ? Financial report
- ? Performance metrics
- ? Compliance report
- ? Export to PDF/Excel

### **5. Dashboard**
- ? Key metrics (counts, rates)
- ? Recent activity log
- ? System health status
- ? User alerts
- ? Quick actions

---

## ?? **SECURITY IMPLEMENTATION**

### **JWT Token Handling**

```typescript
// jwt.interceptor.ts
import { Injectable } from '@angular/core';
import { HttpInterceptor, HttpRequest, HttpHandler, HttpEvent } from '@angular/common/http';
import { Observable } from 'rxjs';
import { AuthService } from '../auth/auth.service';

@Injectable()
export class JwtInterceptor implements HttpInterceptor {
  constructor(private authService: AuthService) {}

  intercept(request: HttpRequest<any>, next: HttpHandler): Observable<HttpEvent<any>> {
    const currentUser = this.authService.currentUserValue;
    
    if (currentUser && currentUser.token) {
   request = request.clone({
        setHeaders: {
Authorization: `Bearer ${currentUser.token}`
        }
      });
    }

    return next.handle(request);
  }
}
```

### **Error Handling**

```typescript
// error.interceptor.ts
import { Injectable } from '@angular/core';
import { HttpInterceptor, HttpRequest, HttpHandler, HttpEvent, HttpErrorResponse } from '@angular/common/http';
import { Observable, throwError } from 'rxjs';
import { catchError } from 'rxjs/operators';
import { AuthService } from '../auth/auth.service';
import { Router } from '@angular/router';

@Injectable()
export class ErrorInterceptor implements HttpInterceptor {
  constructor(
    private authService: AuthService,
    private router: Router
  ) {}

  intercept(request: HttpRequest<any>, next: HttpHandler): Observable<HttpEvent<any>> {
  return next.handle(request).pipe(catchError(error => {
    if (error.status === 401) {
        // Unauthorized - logout user
     this.authService.logout();
        this.router.navigate(['/login']);
  } else if (error.status === 403) {
   // Forbidden
        this.router.navigate(['/unauthorized']);
  } else if (error.status === 500) {
        // Server error
 console.error('Server error:', error);
      }
return throwError(error);
    }));
  }
}
```

---

## ?? **DATABASE INTEGRATION**

Your existing .NET API already has:
- ? 62 business services
- ? SQL Server database
- ? Authentication system
- ? Data validation

**We just need to**:
1. Create Angular UI to consume these APIs
2. Add proper error handling
3. Implement responsive design
4. Add real-time updates

---

## ?? **IMPLEMENTATION ROADMAP**

### **Phase 1: Setup (Week 1)**
- [ ] Create Angular project
- [ ] Setup project structure
- [ ] Configure environment
- [ ] Setup CI/CD pipeline

### **Phase 2: Core (Week 2-3)**
- [ ] Authentication & authorization
- [ ] API integration
- [ ] HTTP interceptors
- [ ] Error handling

### **Phase 3: Features (Week 4-6)**
- [ ] Dashboard component
- [ ] Claims management
- [ ] Eligibility verification
- [ ] Pre-auth management

### **Phase 4: Reports (Week 7)**
- [ ] Reports dashboard
- [ ] Analytics
- [ ] Export functionality

### **Phase 5: Testing & Deployment (Week 8)**
- [ ] Unit testing
- [ ] Integration testing
- [ ] Performance optimization
- [ ] Production deployment

---

## ?? **TECHNOLOGY STACK**

| Layer | Technology | Version |
|-------|-----------|---------|
| **Framework** | Angular | 18+ |
| **Language** | TypeScript | 5.2+ |
| **Styling** | SCSS/Bootstrap | 5.3+ |
| **HTTP** | HttpClient | Angular built-in |
| **State** | RxJS | 7.8+ |
| **UI Library** | ng-bootstrap | Latest |
| **Charts** | Chart.js | 4+ |
| **Build** | Webpack | Angular default |

---

## ?? **PACKAGE.JSON DEPENDENCIES**

```json
{
  "dependencies": {
  "@angular/animations": "^18.0.0",
    "@angular/common": "^18.0.0",
"@angular/compiler": "^18.0.0",
    "@angular/core": "^18.0.0",
  "@angular/forms": "^18.0.0",
    "@angular/platform-browser": "^18.0.0",
"@angular/platform-browser-dynamic": "^18.0.0",
    "@angular/router": "^18.0.0",
    "@ng-bootstrap/ng-bootstrap": "^14.0.0",
    "bootstrap": "^5.3.0",
    "chart.js": "^4.4.0",
    "moment": "^2.29.0",
    "ngx-bootstrap": "^12.0.0",
    "rxjs": "^7.8.0",
    "tslib": "^2.6.0",
    "zone.js": "^0.14.0"
  },
  "devDependencies": {
    "@angular-devkit/build-angular": "^18.0.0",
    "@angular/cli": "^18.0.0",
    "@angular/compiler-cli": "^18.0.0",
    "@types/node": "^20.0.0",
    "typescript": "~5.2.0"
  }
}
```

---

## ? **RECOMMENDATION**

### **This is an EXCELLENT idea because:**

? **Leverages Existing Backend**
- Use your 62 already-built services
- No backend changes needed
- APIs are production-ready

? **Completes the Solution**
- Users now have web UI
- No more manual workarounds
- Professional interface

? **Improves User Experience**
- Responsive design
- Real-time updates
- Intuitive workflows

? **Scalable Architecture**
- Component-based
- Feature modules
- Easy to extend

? **Enterprise-Grade**
- JWT authentication
- Error handling
- Performance optimization
- Security best practices

---

## ?? **NEXT STEPS**

Would you like me to:

1. ? **Create the complete Angular project setup** with all folders & files?
2. ? **Generate individual components** (Dashboard, Claims, Eligibility, etc.)?
3. ? **Write complete service implementations** with full API integration?
4. ? **Create responsive HTML/SCSS templates** for all pages?
5. ? **Setup authentication & routing** configuration?
6. ? **Create deployment configuration** (Docker, CI/CD)?

---

**Decision**: Should I proceed with creating the complete Angular RCM Portal?

**Estimated Timeline**: 2-3 weeks for full production-ready implementation

**Recommendation**: ? **GO AHEAD - This will make your solution complete!**

