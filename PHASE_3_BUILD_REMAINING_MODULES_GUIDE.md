# ?? **PHASE 3 - BUILD REMAINING MODULES GUIDE**

**Status**: Starting Remaining Modules  
**Completed**: Claims Module ?  
**Next**: Eligibility, Dashboard, Pre-Auth, Reports  
**Timeline**: 18-22 hours total for remaining modules  

---

## ?? **PHASE 3 ROADMAP - REMAINING MODULES**

### **Priority Breakdown**

```
Priority 1 (HIGH):    Eligibility Module      5-6 hours
Priority 2 (MEDIUM):Dashboard Module        4-5 hours
Priority 3 (MEDIUM):  Pre-Auth Module         4-5 hours
Priority 4 (LOW):     Reports Module          5-6 hours

Total: 18-22 hours (3-4 days)
```

---

## ?? **MODULE 2: ELIGIBILITY MODULE**

### **Overview**
Check real-time eligibility for members against insurance plans.

### **Components to Build**

#### **1. Eligibility Check Component**
- Input form with:
  - Member ID
  - Date of Birth
  - Service Date
- Submit button
- Loading state

#### **2. Eligibility Results Component**
- Display eligibility response
- Member information
- Coverage details
- Benefits breakdown
- Print option

#### **3. Eligibility History Component**
- List of previous checks
- Timestamps
- Status (Verified, Pending, Failed)
- Re-check button

### **Routing**
```
/eligibility           ? Eligibility Check Form
/eligibility/results   ? Results Display
/eligibility/history   ? History List
```

### **Backend Integration**
```
POST /api/v1/eligibility/check      ? Check eligibility
GET  /api/v1/eligibility/requests   ? Get requests
GET  /api/v1/eligibility/responses  ? Get responses
```

---

## ?? **MODULE 3: DASHBOARD MODULE**

### **Overview**
Overview and key metrics for the RCM system.

### **Components to Build**

#### **1. Dashboard Component (Main)**
- Statistics cards
- Recent claims table
- Eligibility status chart
- Quick actions

#### **2. Statistics Cards Component**
- Total claims
- Pending claims
- Processed claims
- Denied claims
- Approved claims

#### **3. Charts Component**
- Claims by status (Pie chart)
- Claims trend (Line chart)
- Amount breakdown (Bar chart)

#### **4. Recent Activity Component**
- Recent claims
- Recent eligibility checks
- Recent approvals

### **Routing**
```
/dashboard    ? Main Dashboard
```

### **No new backend calls** (uses existing services)

---

## ?? **MODULE 4: PRE-AUTH MODULE**

### **Overview**
Pre-authorization request management and tracking.

### **Components to Build**

#### **1. Pre-Auth Request Form**
- Inputs:
  - Patient ID
  - Service Type
  - Procedure Code
  - Requested Amount
- Submit button

#### **2. Pre-Auth List Component**
- Table with all requests
- Filter by status
- Search by patient ID
- View & Track buttons

#### **3. Pre-Auth Status Component**
- Request details
- Approval status
- Timeline
- Comments section

### **Routing**
```
/pre-auth           ? Pre-Auth List
/pre-auth/create  ? Create Request
/pre-auth/:id       ? View Status
```

### **Backend Endpoints** (To be checked)
```
POST /api/pre-auth        ? Create request
GET  /api/pre-auth      ? List requests
GET  /api/pre-auth/:id    ? Get details
PUT  /api/pre-auth/:id    ? Update status
```

---

## ?? **MODULE 5: REPORTS MODULE**

### **Overview**
Analytics, reports, and data export.

### **Components to Build**

#### **1. Reports Dashboard**
- Report templates
- Filter options
- Generate button

#### **2. Reports View Component**
- Display report data
- Tables with data
- Charts/Graphs
- Export buttons

#### **3. Report Filters**
- Date range picker
- Status filter
- Provider filter
- Payer filter

### **Routing**
```
/reports           ? Reports Dashboard
/reports/:id       ? View Report
```

### **Backend Endpoints** (To be checked)
```
GET /api/reports  ? List reports
GET /api/reports/:id       ? Get report
POST /api/reports/generate ? Generate report
GET /api/reports/export/:id ? Export report
```

---

## ?? **FILE STRUCTURE FOR ALL MODULES**

### **Eligibility Module**
```
src/app/features/eligibility/
??? eligibility.module.ts
??? eligibility-routing.module.ts
??? components/
?   ??? eligibility-check/
?   ?   ??? eligibility-check.component.ts
?   ?   ??? eligibility-check.component.html
?   ?   ??? eligibility-check.component.less
?   ??? eligibility-results/
?   ?   ??? eligibility-results.component.ts
?   ?   ??? eligibility-results.component.html
?   ?   ??? eligibility-results.component.less
?   ??? eligibility-history/
?       ??? eligibility-history.component.ts
?   ??? eligibility-history.component.html
?       ??? eligibility-history.component.less
??? services/
    ??? eligibility-local.service.ts (if needed)
```

### **Dashboard Module**
```
src/app/features/dashboard/
??? dashboard.module.ts
??? dashboard-routing.module.ts
??? components/
?   ??? dashboard/
?   ?   ??? dashboard.component.ts
?   ?   ??? dashboard.component.html
?   ?   ??? dashboard.component.less
?   ??? stats-cards/
?   ?   ??? stats-cards.component.ts
?   ?   ??? stats-cards.component.html
?   ?   ??? stats-cards.component.less
? ??? charts/
?   ?   ??? charts.component.ts
?   ?   ??? charts.component.html
?   ?   ??? charts.component.less
?   ??? recent-activity/
?   ??? recent-activity.component.ts
?       ??? recent-activity.component.html
?       ??? recent-activity.component.less
??? services/
    ??? dashboard.service.ts
```

### **Pre-Auth Module**
```
src/app/features/pre-auth/
??? pre-auth.module.ts
??? pre-auth-routing.module.ts
??? components/
? ??? pre-auth-list/
?   ?   ??? pre-auth-list.component.ts
?   ?   ??? pre-auth-list.component.html
?   ?   ??? pre-auth-list.component.less
?   ??? pre-auth-form/
?   ?   ??? pre-auth-form.component.ts
?   ?   ??? pre-auth-form.component.html
?   ?   ??? pre-auth-form.component.less
?   ??? pre-auth-status/
?       ??? pre-auth-status.component.ts
?       ??? pre-auth-status.component.html
?    ??? pre-auth-status.component.less
??? models/
?   ??? pre-auth.model.ts
??? services/
 ??? pre-auth.service.ts
```

### **Reports Module**
```
src/app/features/reports/
??? reports.module.ts
??? reports-routing.module.ts
??? components/
?   ??? reports-dashboard/
?   ?   ??? reports-dashboard.component.ts
?   ?   ??? reports-dashboard.component.html
?   ?   ??? reports-dashboard.component.less
?   ??? reports-view/
?   ?   ??? reports-view.component.ts
?   ?   ??? reports-view.component.html
?   ?   ??? reports-view.component.less
?   ??? report-filters/
?       ??? report-filters.component.ts
?       ??? report-filters.component.html
?       ??? report-filters.component.less
??? models/
?   ??? report.model.ts
??? services/
    ??? reports.service.ts
```

---

## ??? **IMPLEMENTATION STEPS FOR EACH MODULE**

### **Step 1: Generate Module**
```powershell
# Eligibility
ng generate module features/eligibility
ng generate module features/eligibility/eligibility-routing

# Dashboard
ng generate module features/dashboard
ng generate module features/dashboard/dashboard-routing

# Pre-Auth
ng generate module features/pre-auth
ng generate module features/pre-auth/pre-auth-routing

# Reports
ng generate module features/reports
ng generate module features/reports/reports-routing
```

### **Step 2: Generate Components**
```powershell
# Eligibility Components
ng generate component features/eligibility/components/eligibility-check
ng generate component features/eligibility/components/eligibility-results
ng generate component features/eligibility/components/eligibility-history

# Dashboard Components
ng generate component features/dashboard/components/dashboard
ng generate component features/dashboard/components/stats-cards
ng generate component features/dashboard/components/charts
ng generate component features/dashboard/components/recent-activity

# Pre-Auth Components
ng generate component features/pre-auth/components/pre-auth-list
ng generate component features/pre-auth/components/pre-auth-form
ng generate component features/pre-auth/components/pre-auth-status

# Reports Components
ng generate component features/reports/components/reports-dashboard
ng generate component features/reports/components/reports-view
ng generate component features/reports/components/report-filters
```

### **Step 3: Add Ant Design Modules to Each Module**
Same as Claims module - add all required Ant Design components

### **Step 4: Implement Components**
- TypeScript logic
- HTML templates
- LESS styles
- Service integration

### **Step 5: Configure Routing**
```typescript
const routes: Routes = [
  { path: '', component: ListComponent },
  { path: 'create', component: FormComponent },
  { path: ':id', component: DetailComponent },
  { path: ':id/edit', component: FormComponent }
];
```

### **Step 6: Test**
- Load module
- Test all features
- Responsive design
- Error handling

---

## ?? **ELIGIBILITY MODULE - QUICK BUILD TEMPLATE**

### **eligibility.module.ts**
```typescript
import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { ReactiveFormsModule, FormsModule } from '@angular/forms';

// Ant Design
import { NzFormModule } from 'ng-zorro-antd/form';
import { NzInputModule } from 'ng-zorro-antd/input';
import { NzButtonModule } from 'ng-zorro-antd/button';
import { NzCardModule } from 'ng-zorro-antd/card';
import { NzTableModule } from 'ng-zorro-antd/table';
import { NzDatePickerModule } from 'ng-zorro-antd/date-picker';
import { NzSelectModule } from 'ng-zorro-antd/select';
import { NzSpinModule } from 'ng-zorro-antd/spin';
import { NzEmptyModule } from 'ng-zorro-antd/empty';
import { NzGridModule } from 'ng-zorro-antd/grid';
import { NzTabsModule } from 'ng-zorro-antd/tabs';
import { NzBadgeModule } from 'ng-zorro-antd/badge';
import { NzDividerModule } from 'ng-zorro-antd/divider';

// Routing & Components
import { EligibilityRoutingModule } from './eligibility-routing.module';

@NgModule({
  declarations: [],
  imports: [
    CommonModule,
    ReactiveFormsModule,
    FormsModule,
    EligibilityRoutingModule,
    // Ant Design Modules
    NzFormModule,
    NzInputModule,
    NzButtonModule,
    NzCardModule,
    NzTableModule,
    NzDatePickerModule,
    NzSelectModule,
    NzSpinModule,
NzEmptyModule,
NzGridModule,
    NzTabsModule,
    NzBadgeModule,
    NzDividerModule
  ]
})
export class EligibilityModule { }
```

### **eligibility-check.component.ts**
```typescript
import { Component, OnInit, inject } from '@angular/core';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { Router } from '@angular/router';
import { NzMessageService } from 'ng-zorro-antd/message';
import { EligibilityService } from '../../../../core/services/eligibility.service';
import { CheckEligibilityRequest } from '../../../../core/models/eligibility.model';

@Component({
  selector: 'app-eligibility-check',
  templateUrl: './eligibility-check.component.html',
  styleUrls: ['./eligibility-check.component.less']
})
export class EligibilityCheckComponent implements OnInit {
  private fb = inject(FormBuilder);
  private eligibilityService = inject(EligibilityService);
  private router = inject(Router);
  private message = inject(NzMessageService);

  checkForm!: FormGroup;
  loading = false;

  ngOnInit(): void {
    this.initForm();
  }

  private initForm(): void {
    this.checkForm = this.fb.group({
      memberId: ['', [Validators.required, Validators.minLength(3)]],
      dateOfService: ['', Validators.required],
      procedureCode: ['']
    });
  }

  onSubmit(): void {
    if (!this.checkForm.valid) {
      this.message.error('Please fill in required fields');
 return;
    }

    this.loading = true;
    const request: CheckEligibilityRequest = this.checkForm.value;

    this.eligibilityService.check(request).subscribe({
      next: (response) => {
        this.loading = false;
   this.message.success('Eligibility verified successfully');
        this.router.navigate(['/eligibility/results'], {
  state: { data: response.data }
        });
      },
      error: (error) => {
        this.loading = false;
        this.message.error('Failed to check eligibility');
      }
  });
  }
}
```

---

## ? **RECOMMENDED BUILD ORDER**

1. **Start with Eligibility Module** (5-6 hours)
   - Most important for RCM workflow
   - Simpler than others
   - Uses existing service

2. **Then Dashboard Module** (4-5 hours)
   - Uses existing services
   - No new backend calls
   - Charts & statistics

3. **Then Pre-Auth Module** (4-5 hours)
   - Requires new backend endpoints
   - Similar to Claims

4. **Finally Reports Module** (5-6 hours)
   - Complex data visualization
   - Export functionality

---

## ?? **QUICK COMPARISON - TIME ESTIMATE**

| Module | Components | Est. Time | Complexity |
|--------|-----------|-----------|-----------|
| Eligibility | 3 | 5-6h | Medium |
| Dashboard | 4 | 4-5h | Low |
| Pre-Auth | 3 | 4-5h | High |
| Reports | 3 | 5-6h | High |

---

## ?? **WHICH MODULE TO BUILD FIRST?**

### **Option A: Eligibility First** (RECOMMENDED)
- ? Simpler logic
- ? Quick to build
- ? Core business need
- ? Good momentum

### **Option B: Dashboard First**
- ? Overview feature
- ? Motivating visuals
- ? No backend changes needed

### **Option C: Pre-Auth First**
- ?? Most complex
- ?? New backend work
- ? Critical workflow

### **Option D: Reports First**
- ?? Data visualization
- ?? New backend work
- ? Analytics important

---

## ?? **LET'S START!**

**Ready to build the next module?**

Tell me which module you want to build first and I'll:
1. Create all module files
2. Implement components
3. Add styling
4. Configure routing
5. Integrate services
6. Test everything

**Recommendations:**
- **Start with Eligibility** for logical flow
- **Then Dashboard** for quick wins
- **Then Pre-Auth** for workflow
- **Finally Reports** for analytics

---

**Choose a module and let's build!** ??
