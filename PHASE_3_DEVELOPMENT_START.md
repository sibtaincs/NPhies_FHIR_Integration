# ?? **PHASE 3: FEATURE DEVELOPMENT - DASHBOARD, CLAIMS, ELIGIBILITY**

**Status**: Starting Phase 3  
**Based On**: Backend API Analysis  
**Integrated With**: .NET 9 Backend (All DTOs matched)

---

## ?? **PHASE 3 OVERVIEW**

### **What We'll Build**

Phase 3 consists of implementing 5 feature modules:

1. **Dashboard Module** - Overview & metrics
2. **Claims Module** - CRUD operations with full details
3. **Eligibility Module** - Eligibility verification
4. **Pre-Auth Module** - Pre-authorization requests
5. **Reports Module** - Analytics & reporting

### **Timeline**: 2-3 days
### **Dependencies**: Phase 2 (? Complete)

---

## ?? **BACKEND ANALYSIS RESULTS**

I've analyzed the backend and found the following:

### **Endpoints Available**

#### **Claims API**
```
POST/api/claims        Create claim
GET    /api/claims/{id}               Get claim by ID
GET    /api/claims/{id}/details       Get claim with details
GET    /api/claims/patient/{patientId}  Get patient's claims
GET    /api/claims/status/{status}    Get claims by status
PUT    /api/claims/{id}       Update claim
```

#### **Valid Claim Statuses**
```
- active
- submitted
- processed
- denied
- cancelled
```

### **Backend Models Found**

**ClaimDto** (Read):
```csharp
Id, ClaimNumber, ClaimIdentifierSystem, ClaimIdentifierValue,
Status, ClaimType, ClaimTypeSystem, ClaimSubType,
Use, Priority, PayeeType, Total, TotalCurrency,
PatientId, ProviderId, InsurerId, CoverageId,
MessageHeaderId, EpisodeIdentifierSystem, EpisodeIdentifierValue,
EligibilityOfflineReference, EligibilityOfflineDate,
AuthorizationOfflineDate, CreatedAt, UpdatedAt
```

**CreateClaimDto** (Write):
```csharp
ClaimNumber*, ClaimIdentifierSystem, ClaimIdentifierValue,
ClaimType*, ClaimTypeSystem, ClaimSubType,
Use, Priority, PrioritySystem, PayeeType, PayeeTypeSystem,
Total, TotalCurrency,
PatientId*, ProviderId, InsurerId, CoverageId,
MessageHeaderId, EpisodeIdentifierSystem, EpisodeIdentifierValue,
EligibilityOfflineReference, EligibilityOfflineDate,
AuthorizationOfflineDate

(* = Required)
```

**UpdateClaimDto** (Partial):
```csharp
Status?, Total?, EpisodeIdentifierValue?,
AuthorizationOfflineDate?
```

---

## ? **UPDATE FRONTEND MODELS TO MATCH BACKEND**

Now let's update the frontend Claim model to match the backend exactly:
