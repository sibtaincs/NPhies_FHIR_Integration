# ?? **PHASE 3 - PRE-AUTH MODULE BUILD PLAN**

**Status**: Ready to Start  
**Timeline**: 4-5 hours  
**Target**: 80% Phase 3 completion  
**Components**: 3 major components  

---

## ?? **PRE-AUTH MODULE OVERVIEW**

### **Purpose**
Manage pre-authorization requests for healthcare procedures and services.

### **Key Workflows**
```
1. Create Request
   - Patient selects service/procedure
   - Enters procedure details
   - Submits for pre-auth
   
2. Track Status
   - View pending requests
   - Check approval status
   - View comments/notes
 
3. View History
   - List all pre-auth requests
   - Filter by status
   - Search by patient ID
```

---

## ?? **ARCHITECTURE**

### **Components to Build**

```
pre-auth/
??? pre-auth-list.component
?   ??? .ts (List with filters & pagination)
?   ??? .html (Table & search)
?   ??? .less (Styling)
?
??? pre-auth-form.component
?   ??? .ts (Form validation & submission)
?   ??? .html (Form fields)
?   ??? .less (Styling)
?
??? pre-auth-status.component
 ??? .ts (Status display & tracking)
 ??? .html (Details & timeline)
 ??? .less (Styling)
```

### **Service**
```
pre-auth.service.ts
??? Methods:
?   ??? getAll() - Fetch all requests
?   ??? getById() - Get request details
? ??? create() - Create new request
?   ??? update() - Update request
?   ??? delete() - Delete request
?
??? Models:
    ??? PreAuthRequestDto
    ??? PreAuthStatusEnum
    ??? PreAuthResponseDto
```

### **Models/DTOs**

```typescript
PreAuthRequestDto {
  id: number;
  patientId: string;
  serviceType: string;
  procedureCode: string;
  requestedAmount: number;
  status: PreAuthStatus;
  createdAt: Date;
  updatedAt: Date;
  notes: string;
  comments: PreAuthComment[];
}

PreAuthStatus:
  - PENDING (Waiting for review)
  - APPROVED (Request approved)
  - DENIED (Request denied)
  - EXPIRED (Request expired)
  - UNDER_REVIEW (Being reviewed)

PreAuthComment {
  id: number;
  author: string;
  message: string;
  createdAt: Date;
  type: 'internal' | 'external';
}
```

---

## ?? **COMPONENT BREAKDOWN**

### **1. Pre-Auth List Component** (45 min)

**Purpose**: Display all pre-authorization requests in a table with filters

**Features**:
```typescript
? Table with pagination
? Search by patient ID
? Filter by status
? Sort by date
? View button (navigate to detail)
? Create button (navigate to form)
? Delete action (with confirmation)
? Responsive design
```

**Data Displayed**:
```
| Request # | Patient ID | Service Type | Status | Amount | Date | Actions |
|-----------|------------|--------------|--------|--------|------|---------|
| PA-001    | P12345     | Diagnostic   | Pending| 2,500  | 1/15 | View... |
```

**Filters**:
- Status dropdown (Pending, Approved, Denied, Expired)
- Date range picker
- Service type dropdown

---

### **2. Pre-Auth Form Component** (1 hour)

**Purpose**: Create and edit pre-authorization requests

**Features**:
```typescript
? Reactive form with validation
? Field validation (required, min/max)
? Success/error messages
? Submit button
? Cancel button
? Reset button
```

**Form Fields**:
```typescript
- Patient ID (required, search)
- Service Type (dropdown: Diagnostic, Therapeutic, Surgical, etc.)
- Procedure Code (required, code format)
- Procedure Description (textarea)
- Service Location (dropdown)
- Requested Amount (required, currency)
- Notes (textarea, optional)
```

**Validation Rules**:
```
? Patient ID: required, min 3 characters
? Service Type: required, must be valid
? Procedure Code: required, format: XX-XXXXX
? Amount: required, > 0, < 1,000,000 SAR
? Notes: optional, max 500 characters
```

---

### **3. Pre-Auth Status Component** (1 hour)

**Purpose**: Display detailed status and timeline of a pre-auth request

**Features**:
```typescript
? Request details card
? Status timeline (created ? under review ? approved/denied)
? Comments section
? Add comment ability
? Update status (admin only)
? Approval letter (if approved)
```

**Sections**:
```
1. Request Header
   - PA Number
   - Status badge
   - Patient info
   - Service details
   
2. Timeline
   - Created: Jan 15, 2024
- Under Review: Jan 16, 2024
   - Approved: Jan 17, 2024
 
3. Comments Section
   - Internal notes
   - External messages
   - Add comment form
```

---

## ?? **IMPLEMENTATION STEPS**

### **Step 1: Create Module & Service (30 min)**

```bash
1. Create pre-auth.module.ts
   - Import all Ant Design modules
   - Declare components
   
2. Create pre-auth-routing.module.ts
   - Route: /pre-auth ? list
   - Route: /pre-auth/create ? form
   - Route: /pre-auth/:id ? detail
   
3. Create pre-auth.service.ts
   - Inject HttpClient
   - Implement 5 main methods
   - Error handling
```

### **Step 2: Create Pre-Auth List (45 min)**

```bash
1. pre-auth-list.component.ts
   - Fetch data in ngOnInit
   - Handle pagination
 - Implement filters
   - Navigate to detail/form
   
2. pre-auth-list.component.html
   - Search bar
   - Filter dropdowns
   - Table with data
   - Action buttons
   
3. pre-auth-list.component.less
   - Table styling
   - Button styling
   - Filter layout
   - Responsive design
```

### **Step 3: Create Pre-Auth Form (1 hour)**

```bash
1. pre-auth-form.component.ts
   - Build reactive form
   - Form validation
   - Submit logic
   - Error handling
   
2. pre-auth-form.component.html
   - Form fields
   - Validation messages
   - Submit/Cancel buttons

3. pre-auth-form.component.less
   - Form styling
   - Field layout
   - Error messages
```

### **Step 4: Create Pre-Auth Status (1 hour)**

```bash
1. pre-auth-status.component.ts
   - Load request by ID
   - Display timeline
   - Handle comments
   - Update status
   
2. pre-auth-status.component.html
   - Request details
   - Timeline component
   - Comments section
   
3. pre-auth-status.component.less
   - Card styling
   - Timeline styling
   - Comments layout
```

### **Step 5: Styling & Testing (30 min)**

```bash
1. Review all components
2. Test responsive design
3. Verify all routes work
4. Test error scenarios
5. Final styling touches
```

---

## ?? **MODULE STRUCTURE**

```typescript
// pre-auth.module.ts
@NgModule({
  declarations: [
    PreAuthListComponent,
 PreAuthFormComponent,
    PreAuthStatusComponent
  ],
  imports: [
    CommonModule,
    ReactiveFormsModule,
    FormsModule,
  PreAuthRoutingModule,
    // Ant Design
    NzCardModule,
    NzTableModule,
    NzButtonModule,
    NzFormModule,
 NzInputModule,
    NzSelectModule,
    NzDatePickerModule,
    NzBadgeModule,
    NzDividerModule,
    NzEmptyModule,
    NzSpinModule,
    NzTimelineModule,
    NzCommentModule,
    NzModalModule,
    NzMessageModule,
  ]
})
export class PreAuthModule { }
```

---

## ?? **UI DESIGN**

### **Pre-Auth List Page**
```
???????????????????????????????????????
? Pre-Authorization Requests?
???????????????????????????????????????
? [Search: _________] [Filter?] [Create?]?
???????????????????????????????????????
? PA# | Patient | Service | Status   ?
???????????????????????????????????????
? 001 | P12345  | Diag    | Pending  ?
? 002 | P12346  | Surg    | Approved ?
? 003 | P12347  | Ther    | Denied   ?
???????????????????????????????????????
? Page 1 of 5 | [< Prev] [Next >]    ?
???????????????????????????????????????
```

### **Pre-Auth Form Page**
```
????????????????????????????
? Create Request           ?
????????????????????????????
? Patient ID: [________]   ?
? Service:    [? Select]   ?
? Procedure:  [________]   ?
? Amount:     [________]   ?
? Notes:      [_______]    ?
?         [_______]    ?
????????????????????????????
? [Submit] [Cancel] [Reset]?
????????????????????????????
```

### **Pre-Auth Detail Page**
```
??????????????????????????????????????
? Request #PA-001 - PENDING      ?
??????????????????????????????????????
? Patient: P12345     ?
? Service: Diagnostic           ?
? Amount: SAR 2,500          ?
??????????????????????????????????????
? Timeline:              ?
? ? Created Jan 15         ?
? ? Under Review Jan 16              ?
?      ?
? Comments:         ?
? [Previous comments...]   ?
? [Add Comment Box]     ?
??????????????????????????????????????
? [Back] [Edit] [Print]           ?
??????????????????????????????????????
```

---

## ? **BUILD EXECUTION CHECKLIST**

### **Module Setup**
- [ ] Create pre-auth.module.ts
- [ ] Create pre-auth-routing.module.ts
- [ ] Import all Ant Design modules
- [ ] Setup routing with 3 routes

### **Service Layer**
- [ ] Create pre-auth.service.ts
- [ ] Implement getAll() method
- [ ] Implement getById() method
- [ ] Implement create() method
- [ ] Implement update() method
- [ ] Implement delete() method
- [ ] Add error handling

### **Pre-Auth List Component**
- [ ] Create component files (.ts, .html, .less)
- [ ] Implement table display
- [ ] Add search functionality
- [ ] Add filter dropdowns
- [ ] Implement pagination
- [ ] Add action buttons
- [ ] Add styling

### **Pre-Auth Form Component**
- [ ] Create component files
- [ ] Build reactive form
- [ ] Add validation
- [ ] Implement submit
- [ ] Add error messages
- [ ] Style form fields

### **Pre-Auth Status Component**
- [ ] Create component files
- [ ] Display request details
- [ ] Build timeline
- [ ] Add comments section
- [ ] Implement comment submission
- [ ] Add styling

### **Final Steps**
- [ ] Test all routes
- [ ] Verify responsive design
- [ ] Check error handling
- [ ] Review styling
- [ ] Commit to Git

---

## ?? **SUCCESS CRITERIA**

After completion, you should have:

? Working /pre-auth route with list  
? Working /pre-auth/create form  
? Working /pre-auth/:id detail page  
? Full CRUD operations  
? Search & filter functionality  
? Responsive design on all devices  
? Error handling  
? Loading states  
? Professional styling  
? Complete documentation  

---

## ?? **TIME ESTIMATE**

| Task | Time |
|------|------|
| Module Setup | 30min |
| Service | 30min |
| List Component | 45min |
| Form Component | 1h |
| Status Component | 1h |
| Styling & Testing | 30min |
| **TOTAL** | **4-5h** |

---

## ?? **READY TO BUILD**

Everything is planned and ready. Let's execute this Pre-Auth Module build step by step!

**Next Action**: Start with Module & Service creation

---

**Estimated Completion**: 4-5 hours  
**Next Phase Target**: 80% (4/5 modules)
**Final Target**: 100% Phase 3 (5/5 modules)  
