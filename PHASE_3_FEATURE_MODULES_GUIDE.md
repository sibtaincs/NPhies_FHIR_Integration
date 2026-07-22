# ?? **PHASE 3: FEATURE MODULES IMPLEMENTATION**

**Status**: Ready to Implement  
**Backend Verified**: ? All APIs analyzed  
**Models Updated**: ? Claim models match backend  

---

## ?? **PHASE 3 MODULES BREAKDOWN**

### **Module 1: Dashboard Module**

**Components to Create:**
- Dashboard (main container)
- Statistics Cards (claims count, eligibility checks, etc.)
- Recent Claims Table
- Eligibility Status Overview
- Charts/Analytics

**Services Needed:**
- Statistics service (get counts and metrics)
- Chart data service

**Estimated Time**: 4-5 hours

---

### **Module 2: Claims Module** (Priority 1)

**Components to Create:**

1. **Claims List**
   - Data table with pagination
   - Status filter
   - Patient ID filter
   - Actions (view, edit, delete)

2. **Claims Detail**
   - Full claim view
   - Status timeline
   - Related items (diagnoses, care team, etc.)
   - Action buttons

3. **Claims Form**
   - Create new claim
   - Edit existing claim
   - Validation
   - Submit button

4. **Claims Routing**
   - List page: `/claims`
   - Detail page: `/claims/:id`
   - Create page: `/claims/create`
   - Edit page: `/claims/:id/edit`

**Endpoints Used:**
```
POST   /api/claims      - Create
GET    /api/claims/{id}       - Get detail
GET    /api/claims/{id}/details    - Get with relations
GET    /api/claims/patient/{id}    - Patient claims
GET    /api/claims/status/{status} - Status filter
PUT    /api/claims/{id}            - Update
```

**Estimated Time**: 6-8 hours

---

### **Module 3: Eligibility Module**

**Components to Create:**

1. **Eligibility Check Form**
   - Input: Member ID, DOB, Service Date
 - Submit button
   - Loading state

2. **Eligibility Result**
   - Display eligibility data
   - Coverage details
   - Benefits summary
   - Print option

3. **Eligibility History**
   - List of previous checks
   - Timestamps
   - Status indicators

4. **Eligibility Routing**
   - Check page: `/eligibility`
   - Results page: `/eligibility/results/:id`
   - History page: `/eligibility/history`

**Endpoints Used:**
```
POST   /api/v1/eligibility/check          - Check eligibility
GET    /api/v1/eligibility/requests       - Get requests
GET    /api/v1/eligibility/responses      - Get responses
GET    /api/v1/eligibility/requests/pending - Pending
```

**Estimated Time**: 5-6 hours

---

### **Module 4: Pre-Auth Module**

**Components to Create:**

1. **Pre-Auth Request Form**
   - Input fields
- Submit button

2. **Pre-Auth Status**
   - Request list
   - Status tracking
   - Approval/denial status

3. **Pre-Auth Details**
   - View request details
   - View response details

4. **Pre-Auth Routing**
   - Request page: `/pre-auth/create`
   - List page: `/pre-auth`
   - Status page: `/pre-auth/:id`

**Endpoints**: TBD (need to check backend)

**Estimated Time**: 4-5 hours

---

### **Module 5: Reports Module**

**Components to Create:**

1. **Reports Dashboard**
   - Report selection
   - Filter options
   - Generate button

2. **Report Viewer**
   - Display report data
   - Charts/graphs
   - Export options (PDF, Excel)

3. **Report Filters**
   - Date range
   - Status
   - Provider
- Payer

4. **Reports Routing**
   - Dashboard: `/reports`
   - View: `/reports/:id`

**Endpoints**: TBD (need to check backend)

**Estimated Time**: 5-6 hours

---

## ?? **IMPLEMENTATION PRIORITY**

```
Priority 1 (Critical): Claims Module
?? Create maximum value
?? Used daily
?? Time: 6-8 hours

Priority 2 (High): Eligibility Module
?? Real-time requirement
?? Core functionality
?? Time: 5-6 hours

Priority 3 (Medium): Dashboard Module
?? Overview & metrics
?? Nice to have
?? Time: 4-5 hours

Priority 4 (Medium): Pre-Auth Module
?? Workflow process
?? Moderate complexity
?? Time: 4-5 hours

Priority 5 (Low): Reports Module
?? Analytics
?? Can be deferred
?? Time: 5-6 hours

Total Estimated Time: 24-30 hours (3-4 days)
```

---

## ?? **MODULE FILE STRUCTURE**

### **Claims Module**
```
src/app/features/claims/
??? claims.module.ts
??? claims-routing.module.ts
??? components/
?   ??? claims-list/
?   ?   ??? claims-list.component.ts
?   ?   ??? claims-list.component.html
?   ?   ??? claims-list.component.less
?   ?   ??? claims-list.component.spec.ts
?   ??? claims-detail/
?   ?   ??? ... (similar structure)
?   ??? claims-form/
?       ??? ... (similar structure)
??? services/
?   ??? claims.service.ts (already exists in core)
??? models/
    ??? claims.models.ts (in core, already created)
```

### **Eligibility Module**
```
src/app/features/eligibility/
??? eligibility.module.ts
??? eligibility-routing.module.ts
??? components/
?   ??? eligibility-check/
?   ??? eligibility-results/
?   ??? eligibility-history/
??? services/
?   ??? eligibility.service.ts (already in core)
??? models/
    ??? eligibility.models.ts (in core, already created)
```

### **Dashboard Module**
```
src/app/features/dashboard/
??? dashboard.module.ts
??? dashboard-routing.module.ts
??? components/
?   ??? dashboard/
?   ??? stats-cards/
?   ??? recent-claims-table/
?   ??? charts/
??? services/
    ??? dashboard.service.ts (to create)
```

---

## ??? **IMPLEMENTATION STEPS**

### **Step 1: Generate Module Files** (15 min)

```powershell
# Claims Module
ng generate module features/claims
ng generate module features/claims/claims-routing

# Eligibility Module
ng generate module features/eligibility
ng generate module features/eligibility/eligibility-routing

# Dashboard Module
ng generate module features/dashboard
ng generate module features/dashboard/dashboard-routing

# Pre-Auth Module
ng generate module features/pre-auth
ng generate module features/pre-auth/pre-auth-routing

# Reports Module
ng generate module features/reports
ng generate module features/reports/reports-routing
```

### **Step 2: Create Components** (30 min)

```powershell
# Claims Components
ng generate component features/claims/components/claims-list
ng generate component features/claims/components/claims-detail
ng generate component features/claims/components/claims-form

# Eligibility Components
ng generate component features/eligibility/components/eligibility-check
ng generate component features/eligibility/components/eligibility-results

# Dashboard Components
ng generate component features/dashboard/components/dashboard
ng generate component features/dashboard/components/stats-cards

# ... and so on
```

### **Step 3: Implement Services**

Use existing services from Phase 2:
- `ClaimsService` - Already created ?
- `EligibilityService` - Already created ?
- Create new: `DashboardService`, `PreAuthService`, `ReportsService`

### **Step 4: Build Components** (2-3 days)

Implement each component with:
- Template (HTML)
- Logic (TypeScript)
- Styles (LESS)
- Ant Design components

### **Step 5: Add Routing**

Configure lazy-loaded routes for each module

### **Step 6: Test**

Unit tests for components and services

---

## ?? **BACKEND ENDPOINTS CHECKLIST**

### **Claims** ?
- [x] POST /api/claims
- [x] GET /api/claims/{id}
- [x] GET /api/claims/{id}/details
- [x] GET /api/claims/patient/{id}
- [x] GET /api/claims/status/{status}
- [x] PUT /api/claims/{id}
- [ ] DELETE /api/claims/{id} (need to check)

### **Eligibility** ??
- [x] POST /api/v1/eligibility/check
- [x] GET /api/v1/eligibility/requests
- [x] GET /api/v1/eligibility/responses
- [x] GET /api/v1/eligibility/requests/pending
- [ ] Need to verify exact endpoints

### **Pre-Auth** ?
- [ ] Need to check backend endpoints

### **Reports** ?
- [ ] Need to check backend endpoints

---

## ?? **UI/UX GUIDELINES**

### **Ant Design Components to Use**

**Tables**:
- `nz-table` - Data display
- Pagination
- Sorting
- Filtering

**Forms**:
- `nz-form`
- Input, Select, DatePicker
- Validation

**Modals**:
- Confirm dialogs
- Create/Edit forms
- View details

**Cards**:
- Statistics cards
- Detail cards
- Timeline

**Buttons**:
- Primary: Submit, Save
- Secondary: Edit, View
- Danger: Delete

**Notifications**:
- Success: "Claim created successfully"
- Error: "Failed to load claims"
- Info: "Loading..."

---

## ?? **SAMPLE CLAIMS LIST COMPONENT**

```typescript
import { Component, OnInit } from '@angular/core';
import { ClaimsService } from '../../../core/services/claims.service';
import { PageRequest } from '../../../core/models/api-response.model';
import { Claim, ClaimStatus } from '../../../core/models/claim.model';

@Component({
  selector: 'app-claims-list',
  templateUrl: './claims-list.component.html',
  styleUrls: ['./claims-list.component.less']
})
export class ClaimsListComponent implements OnInit {
  claims$ = this.claimsService.getAll({
    pageNumber: 1,
  pageSize: 10
  } as PageRequest);

  selectedStatus: ClaimStatus | null = null;
  loading = false;
  statuses: ClaimStatus[] = ['active', 'submitted', 'processed', 'denied', 'cancelled'];

constructor(private claimsService: ClaimsService) { }

  ngOnInit() {
    // Load claims on init
  }

  onStatusChange(status: ClaimStatus | null) {
    this.selectedStatus = status;
    if (status) {
      this.claims$ = this.claimsService.getByStatus(status);
    }
  }

  onViewDetail(claimId: string) {
    // Navigate to detail page
  }

  onEdit(claimId: string) {
    // Navigate to edit page
  }

  onDelete(claimId: string) {
    // Show confirm dialog and delete
  }
}
```

---

## ? **PHASE 3 CHECKLIST**

### **Dashboard Module**
- [ ] Generate module
- [ ] Create components
- [ ] Add services
- [ ] Implement templates
- [ ] Add routing
- [ ] Test

### **Claims Module** (Priority)
- [ ] Generate module
- [ ] Create list component
- [ ] Create detail component
- [ ] Create form component
- [ ] Implement templates
- [ ] Add routing
- [ ] Test

### **Eligibility Module**
- [ ] Generate module
- [ ] Create check component
- [ ] Create results component
- [ ] Create history component
- [ ] Implement templates
- [ ] Add routing
- [ ] Test

### **Pre-Auth Module**
- [ ] Generate module
- [ ] Check backend endpoints
- [ ] Create components
- [ ] Implement templates
- [ ] Add routing
- [ ] Test

### **Reports Module**
- [ ] Generate module
- [ ] Check backend endpoints
- [ ] Create components
- [ ] Implement templates
- [ ] Add routing
- [ ] Test

---

## ?? **READY TO START**

All analysis complete! ?

**Next Action**: Start implementing Claims Module (Priority 1)

**Command to Start**:
```powershell
cd C:\Users\sibtain.alimadad\source\repos\rcm-portal-antd

# Generate module and components
ng generate module features/claims
ng generate module features/claims/claims-routing
ng generate component features/claims/components/claims-list
ng generate component features/claims/components/claims-detail
ng generate component features/claims/components/claims-form
```

---

**Status**: ? Phase 3 Ready to Start  
**Backend Models**: ? Matched  
**Frontend Models**: ? Updated  
**Next**: Claims Module Implementation  

?? **Let's build Phase 3!** ??
