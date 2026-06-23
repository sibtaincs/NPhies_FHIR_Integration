# ?? **PHASE 2 SESSION 2: DEVELOPMENT PLAN**

## **Current Progress Summary**

### **Phase 2 Status: 35% Complete** ??

```
? DTOs               100%  (20+ classes)
? AutoMapper   100%  (Mappings configured)
? Patients API        100%  (7 endpoints working)
? Coverage API        100%  (8 endpoints working)
? Error Handling 100%  (Global exception handling)
? Organizations API     0%  (8 endpoints to build)
? Eligibility API       0%  (6 endpoints to build)
? Claims API       0%  (8+ endpoints to build)
? Testing           0%  (Integration tests)
```

---

## **Session 2 Objectives**

### **?? Target: 70% Phase 2 Completion**

We will build 3 new API controllers:
1. **OrganizationController** (8 endpoints)
2. **EligibilityController** (6 endpoints)
3. **ClaimsController** (8+ endpoints)

Plus create corresponding DTOs and testing.

---

## **?? Development Roadmap: Session 2**

### **Part 1: Organizations API** (45 minutes)

#### **Endpoints to Create**
```
GET    /api/organizations              (List all - paginated)
GET    /api/organizations/{id}         (Get by ID)
GET    /api/organizations/providers    (Get all providers)
GET    /api/organizations/insurers     (Get all insurers)
GET    /api/organizations/search       (Search by name)
POST   /api/organizations      (Create new)
PUT    /api/organizations/{id}         (Update)
DELETE /api/organizations/{id}     (Delete)
```

#### **DTOs Needed**
- ? Already created: `OrganizationDto`, `CreateOrganizationDto`, `UpdateOrganizationDto`
- ? Already created: `ProviderDto`, `InsurerDto`

#### **Repository Methods Available**
```csharp
? GetByLicenseNumberAsync(licenseNumber)
? GetProvidersAsync()
? GetInsurersAsync()
? GetWithDetailsAsync(organizationId)
? GetByEmailAsync(email)
? SearchByNameAsync(name)
```

---

### **Part 2: Eligibility API** (60 minutes)

#### **Endpoints to Create**
```
POST   /api/eligibility/request       (Submit eligibility request)
GET    /api/eligibility/request/{id}      (Get request details)
GET    /api/eligibility/requests         (List all requests - paginated)
GET    /api/eligibility/response/{id}          (Get response details)
GET    /api/eligibility/pending      (Get pending requests)
GET    /api/eligibility/patient/{patientId}    (Get patient eligibility history)
```

#### **DTOs Needed** (To Create)
- `EligibilityRequestDto`
- `CreateEligibilityRequestDto`
- `EligibilityResponseDto`
- `EligibilityRequestListDto`
- `PendingEligibilityDto`

#### **Repository Methods Available**
```csharp
? GetWithDetailsAsync(requestId)
? GetByPatientIdAsync(patientId)
? GetByCoverageIdAsync(coverageId)
? GetByProviderIdAsync(providerId)
? GetByStatusAsync(status)
? GetPendingRequestsAsync()
```

---

### **Part 3: Claims API** (60 minutes)

#### **Endpoints to Create**
```
POST   /api/claims              (Submit claim)
GET    /api/claims/{id}      (Get claim details)
GET    /api/claims      (List all - paginated)
GET    /api/claims/patient/{patientId}  (Get patient claims)
GET    /api/claims/status/{status}      (Get by status)
PUT    /api/claims/{id}     (Update claim)
GET    /api/claims/{id}/response        (Get claim response)
DELETE /api/claims/{id}        (Delete claim)
```

#### **DTOs Needed** (To Create)
- `ClaimDto`
- `CreateClaimDto`
- `ClaimListDto`
- `ClaimResponseDto`
- `ClaimDetailDto`

#### **Repository Methods Available**
```csharp
? IClaimRepository interface exists
? Methods to be implemented
```

---

## **?? Detailed Implementation Order**

### **Step 1: Create OrganizationController** (30 minutes)

**File**: `NPhies_FHIR_Integration.ApiService/Controllers/OrganizationController.cs`

```csharp
[ApiController]
[Route("api/[controller]")]
public class OrganizationsController : ControllerBase
{
    private readonly IOrganizationRepository _organizationRepository;
    private readonly IMapper _mapper;
    private readonly ILogger<OrganizationsController> _logger;

    // Constructor + 8 endpoints
}
```

**Endpoints Pattern** (Same as Patients/Coverage):
- ? Pagination support
- ? Error handling
- ? Logging
- ? ProducesResponseType attributes

---

### **Step 2: Create Eligibility DTOs** (15 minutes)

**File**: `NPhies_FHIR_Integration.Application/DTOs/EligibilityDtos.cs`

```csharp
public class EligibilityRequestDto : BaseDto { ... }
public class CreateEligibilityRequestDto { ... }
public class EligibilityResponseDto : BaseDto { ... }
```

---

### **Step 3: Create EligibilityController** (45 minutes)

**File**: `NPhies_FHIR_Integration.ApiService/Controllers/EligibilityController.cs`

```csharp
[ApiController]
[Route("api/[controller]")]
public class EligibilityController : ControllerBase
{
    // 6 endpoints for eligibility management
}
```

---

### **Step 4: Create Claims DTOs** (15 minutes)

**File**: `NPhies_FHIR_Integration.Application/DTOs/ClaimsDtos.cs`

```csharp
public class ClaimDto : BaseDto { ... }
public class CreateClaimDto { ... }
```

---

### **Step 5: Create ClaimsController** (45 minutes)

**File**: `NPhies_FHIR_Integration.ApiService/Controllers/ClaimsController.cs`

```csharp
[ApiController]
[Route("api/[controller]")]
public class ClaimsController : ControllerBase
{
    // 8+ endpoints for claims management
}
```

---

### **Step 6: Update AutoMapper Profiles** (15 minutes)

**File**: `NPhies_FHIR_Integration.Application/Mapping/ApplicationMappingProfile.cs`

Add mappings for:
- ? Organization DTOs
- ? Eligibility DTOs
- ? Claims DTOs

---

### **Step 7: Update Program.cs** (5 minutes)

Register new controllers and services:
```csharp
// No new registrations needed - repositories already registered
```

---

### **Step 8: Testing & Verification** (30 minutes)

- ? Build solution
- ? Run application
- ? Test all new endpoints in Swagger
- ? Verify error handling
- ? Test with Postman

---

## **?? Success Criteria**

- [ ] 3 new controllers created (Org, Eligibility, Claims)
- [ ] 22+ new endpoints implemented
- [ ] All DTOs created
- [ ] AutoMapper updated
- [ ] Build successful (0 errors)
- [ ] All endpoints tested in Swagger
- [ ] Error handling verified
- [ ] Logging working
- [ ] Git committed

---

## **?? Estimated Timeline**

| Task | Duration | Cumulative |
|------|----------|-----------|
| Organizations API | 45 min | 45 min |
| Eligibility API | 60 min | 105 min |
| Claims API | 60 min | 165 min |
| Testing & Fixes | 30 min | 195 min |
| **Total** | **~3.5 hours** | **Complete** |

---

## **?? Resources Available**

### **Controller Pattern** (Reference)
- `PatientsController.cs` - Full CRUD pattern
- `CoverageController.cs` - Pagination pattern

### **DTO Pattern** (Reference)
- `PatientDtos.cs` - DTO structure
- `CommonDtos.cs` - Response wrappers

### **Repository Methods** (Available)
- All repository methods already implemented
- See: `EligibilityRepositories.cs`, `ClaimRepositories.cs`

---

## **?? Ready to Start?**

### **Quick Checklist Before Starting**

- [ ] Application has been tested and working
- [ ] All endpoints from Phase 2 Session 1 verified
- [ ] Database has seeded data
- [ ] Build is successful
- [ ] Git branch is main
- [ ] Ready for development

---

## **Next Action: START DEVELOPMENT**

Ready to begin implementing Session 2? Let me know and I'll:

1. ? Create OrganizationsController
2. ? Create EligibilityDtos
3. ? Create EligibilityController
4. ? Create ClaimsDtos
5. ? Create ClaimsController
6. ? Update mappings
7. ? Test everything
8. ? Commit to Git

---

**Decision:** Ready to start Session 2 development?

Answer: **YES** ? Start building controllers
