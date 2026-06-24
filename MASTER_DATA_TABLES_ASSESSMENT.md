```
?????????????????????????????????????????????????????????????????????????????
?  ?
?  ?? MASTER DATA TABLES ASSESSMENT & IMPLEMENTATION PLAN        ?
?     Before Unit Testing - Complete Analysis?
?           ?
?????????????????????????????????????????????????????????????????????????????


????????????????????????????????????????????????????????????????????????
?? CURRENT STATE ANALYSIS
????????????????????????????????????????????????????????????????????????

## WHAT YOU ALREADY HAVE ?

### Excellent Foundation:
1. ? CodeableConcept Infrastructure (9 tables)
   - CodeSystem table
   - Concept table
   - ValueSet table
   - ValidationRule table
- NphiesMessageType table
   - Complete terminology management

2. ? Complete Domain Entities (42 tables)
   - Patient, Coverage, Organization
   - Eligibility Request/Response
   - Claim & ClaimResponse
   - Communication
   - Task Request/Response
   - Polling Records

3. ? DbContext Configuration
   - All entities mapped
   - Relationships configured
   - Indexes defined

---

## WHAT'S MISSING ? (CRITICAL FOR PRODUCTION)

These master/reference tables are NOT yet created but are ESSENTIAL:

1. ? **Service/Procedure Code Mappings**
   - Medical services codes
   - Procedure codes
   - Medication codes
   - Medical device codes

2. ? **Provider/Clinic Masters**
   - Clinic/Facility details
   - Doctor/Practitioner extended details
   - Provider specializations
   - Provider qualifications

3. ? **Payer/Insurance Masters**
   - Payer policies & rules
   - Coverage types
   - Authorization rules
   - Claim submission rules

4. ? **NPHIES Specific Mappings**
   - NPHIES service codes
   - NPHIES diagnosis codes
   - NPHIES procedure codes
   - NPHIES claim type mappings

5. ? **Clinical Masters**
   - Diagnosis codes (ICD codes)
   - Service categories
   - Modifier codes
   - Benefit type codes

6. ? **Business Rules Engine**
   - Validation rules
   - Authorization rules
   - Claim processing rules

---

????????????????????????????????????????????????????????????????????????
??? MASTER TABLES TO CREATE - DETAILED SPECIFICATIONS
????????????????????????????????????????????????????????????????????????

## 1?? SERVICE CODE MAPPINGS

### Table: ServiceCodeMaster
Purpose: Store all medical service codes with NPHIES mappings

| Column | Type | Constraints | Notes |
|--------|------|-------------|-------|
| Id | int | PK, Identity | Primary key |
| ServiceCode | nvarchar(50) | NOT NULL, UNIQUE | Local service code |
| ServiceName | nvarchar(255) | NOT NULL | Display name |
| ServiceDescription | nvarchar(1000) | | Detailed description |
| ServiceCategory | nvarchar(100) | NOT NULL | Category (e.g., Diagnostic, Surgical) |
| NphiesServiceCode | nvarchar(50) | | NPHIES equivalent code |
| NphiesServiceName | nvarchar(255) | | NPHIES display name |
| NphiesCategoryCode | nvarchar(50) | | NPHIES category |
| IsNphiesMapped | bit | | Flag: Is mapped to NPHIES? |
| MappingValidationStatus | nvarchar(50) | | Status: Valid, Invalid, Pending |
| DefaultPrice | decimal(18,2) | | Standard pricing |
| CurrencyCode | nvarchar(3) | DEFAULT 'SAR' | Currency |
| IsRequiresAuthorization | bit | | Authorization required? |
| DefaultAuthorizationDays | int | | Pre-auth validity (days) |
| IsActive | bit | NOT NULL, DEFAULT 1 | Active flag |
| CreatedBy | nvarchar(100) | | User who created |
| CreatedDate | datetime2 | NOT NULL, DEFAULT GETUTCDATE() | Creation timestamp |
| ModifiedBy | nvarchar(100) | | User who modified |
| ModifiedDate | datetime2 | | Modification timestamp |

**Indexes:**
- PK: Id
- UNIQUE: ServiceCode
- INDEX: ServiceCategory
- INDEX: NphiesServiceCode
- INDEX: IsActive
- INDEX: CreatedDate

**Sample Data:**
```
ServiceCode='SRV001', ServiceName='General Consultation',
ServiceCategory='Consultation', NphiesServiceCode='H1234',
DefaultPrice=150.00, IsRequiresAuthorization=0
```

---

### Table: MedicationCodeMaster
Purpose: Store medication codes with NPHIES mappings

| Column | Type | Constraints | Notes |
|--------|------|-------------|-------|
| Id | int | PK, Identity | Primary key |
| MedicationCode | nvarchar(50) | NOT NULL, UNIQUE | Local medication code |
| MedicationName | nvarchar(255) | NOT NULL | Drug name |
| ActiveIngredient | nvarchar(255) | | Active component |
| Strength | nvarchar(100) | | Dosage strength |
| Unit | nvarchar(50) | | Unit (mg, ml, etc.) |
| Form | nvarchar(50) | | Form (Tablet, Injection, etc.) |
| Manufacturer | nvarchar(255) | | Manufacturer name |
| NphiesMedicationCode | nvarchar(50) | | NPHIES code |
| IsNphiesMapped | bit | | Mapped to NPHIES? |
| UnitPrice | decimal(18,2) | | Cost per unit |
| CurrencyCode | nvarchar(3) | DEFAULT 'SAR' | Currency |
| IsControlledSubstance | bit | | Requires special tracking? |
| IsActive | bit | NOT NULL, DEFAULT 1 | Active flag |
| CreatedDate | datetime2 | NOT NULL, DEFAULT GETUTCDATE() | |
| ModifiedDate | datetime2 | | |

**Sample Data:**
```
MedicationCode='MED001', MedicationName='Aspirin',
Strength='500', Unit='mg', Form='Tablet',
NphiesMedicationCode='NPHMED001', UnitPrice=25.50
```

---

### Table: MedicalDeviceCodeMaster
Purpose: Store medical device codes

| Column | Type | Constraints | Notes |
|--------|------|-------------|-------|
| Id | int | PK, Identity | Primary key |
| DeviceCode | nvarchar(50) | NOT NULL, UNIQUE | Local device code |
| DeviceName | nvarchar(255) | NOT NULL | Device name |
| DeviceDescription | nvarchar(1000) | | Details |
| DeviceType | nvarchar(100) | | Type (Implant, Equipment, etc.) |
| Manufacturer | nvarchar(255) | | Manufacturer |
| NphiesDeviceCode | nvarchar(50) | | NPHIES equivalent |
| IsNphiesMapped | bit | | Mapped? |
| UnitPrice | decimal(18,2) | | Cost per unit |
| CurrencyCode | nvarchar(3) | DEFAULT 'SAR' | |
| IsImplantable | bit | | Implantable device? |
| IsReusable | bit | | Reusable? |
| IsActive | bit | NOT NULL, DEFAULT 1 | |
| CreatedDate | datetime2 | NOT NULL, DEFAULT GETUTCDATE() | |
| ModifiedDate | datetime2 | | |

---

## 2?? PROVIDER/CLINIC MASTERS

### Table: ClinicMaster
Purpose: Extended clinic/facility information (complements Organization table)

| Column | Type | Constraints | Notes |
|--------|------|-------------|-------|
| Id | int | PK, Identity | Primary key |
| OrganizationId | nvarchar(450) | FK to Organization | Link to org |
| ClinicName | nvarchar(255) | NOT NULL | Clinic name |
| ClinicCode | nvarchar(50) | NOT NULL, UNIQUE | Internal code |
| ClinicType | nvarchar(100) | | Type (Hospital, Clinic, Lab, etc.) |
| SpecializedServices | nvarchar(500) | | Comma-separated service types |
| NumberOfBeds | int | | Bed count |
| NumberOfDoctors | int | | Doctor count |
| NumberOfNurses | int | | Nurse count |
| IsCertifiedBy | nvarchar(255) | | Certifications |
| AccreditationLevel | nvarchar(100) | | Accreditation (Gold, Silver, etc.) |
| WorkingHoursFrom | time | | Opening time |
| WorkingHoursTo | time | | Closing time |
| IsEmergencyAvailable | bit | | 24/7 emergency? |
| PharmacyAvailable | bit | | In-house pharmacy? |
| LabAvailable | bit | | In-house lab? |
| ImagingAvailable | bit | | In-house imaging? |
| AcceptsCashPayment | bit | | Payment methods |
| AcceptsInsurance | bit | | |
| AcceptsCardPayment | bit | | |
| AvailableBeds | int | | Current available |
| IsActive | bit | NOT NULL, DEFAULT 1 | |
| CreatedDate | datetime2 | NOT NULL, DEFAULT GETUTCDATE() | |
| ModifiedDate | datetime2 | | |

**Relationship:** FK to Organization table

**Indexes:**
- PK: Id
- FK: OrganizationId
- INDEX: ClinicCode
- INDEX: ClinicType
- INDEX: IsActive

---

### Table: DoctorMaster
Purpose: Extended doctor/practitioner information

| Column | Type | Constraints | Notes |
|--------|------|-------------|-------|
| Id | int | PK, Identity | Primary key |
| PractitionerId | nvarchar(450) | FK to Practitioner | Link to practitioner |
| DoctorCode | nvarchar(50) | NOT NULL, UNIQUE | Internal code |
| DoctorName | nvarchar(255) | NOT NULL | Full name |
| Specialization | nvarchar(100) | | Primary specialization |
| SubSpecialization | nvarchar(100) | | Sub-specialty |
| QualificationDegree | nvarchar(50) | | MD, MD (specialization) |
| UniversityName | nvarchar(255) | | Where qualified |
| YearsOfExperience | int | | Experience in years |
| IsConsultant | bit | | Is consultant? |
| ConsultationFee | decimal(18,2) | | Fee for consultation |
| FollowupFee | decimal(18,2) | | Fee for follow-up |
| CurrencyCode | nvarchar(3) | DEFAULT 'SAR' | |
| ClinicMasterId | int | FK to ClinicMaster | Assigned clinic |
| IsAvailableForAppointments | bit | | Can take appointments? |
| AvailableSlotsPerDay | int | | Appointment slots |
| Board | nvarchar(100) | | Board registration |
| BoardLicenseNumber | nvarchar(100) | | License number |
| BoardLicenseExpiry | date | | Expiry date |
| ResearchPapers | int | | Publications count |
| IsTeachingMember | bit | | Teaches? |
| IsActive | bit | NOT NULL, DEFAULT 1 | |
| CreatedDate | datetime2 | NOT NULL, DEFAULT GETUTCDATE() | |
| ModifiedDate | datetime2 | | |

**Relationships:**
- FK to Practitioner
- FK to ClinicMaster

**Indexes:**
- PK: Id
- FK: PractitionerId
- FK: ClinicMasterId
- INDEX: DoctorCode
- INDEX: Specialization
- INDEX: BoardLicenseNumber
- INDEX: IsActive

---

### Table: DoctorQualifications
Purpose: Multiple qualifications per doctor

| Column | Type | Constraints | Notes |
|--------|------|-------------|-------|
| Id | int | PK, Identity | Primary key |
| DoctorMasterId | int | FK to DoctorMaster | Doctor reference |
| QualificationType | nvarchar(100) | | MBBS, MD, Fellowship, etc. |
| QualificationName | nvarchar(255) | NOT NULL | Full name |
| UniversityName | nvarchar(255) | NOT NULL | Issuing university |
| IssuedDate | date | NOT NULL | When issued |
| IsExpiring | bit | | Has expiry? |
| ExpiryDate | date | | Expiry date |
| CertificateNumber | nvarchar(100) | | Certificate/License number |
| VerificationStatus | nvarchar(50) | | Verified, Pending, Failed |
| CreatedDate | datetime2 | NOT NULL, DEFAULT GETUTCDATE() | |

**Relationship:** FK to DoctorMaster

---

## 3?? PAYER/INSURANCE MASTERS

### Table: PayerMaster
Purpose: Insurance company/payer information

| Column | Type | Constraints | Notes |
|--------|------|-------------|-------|
| Id | int | PK, Identity | Primary key |
| PayerId | nvarchar(100) | NOT NULL, UNIQUE | NPHIES payer ID |
| PayerName | nvarchar(255) | NOT NULL | Insurance company name |
| PayerType | nvarchar(50) | | GOV, PRIVATE, etc. |
| NphiesConnectionStatus | nvarchar(50) | | Active, Testing, Inactive |
| NphiesApiEndpoint | nvarchar(500) | | API endpoint URL |
| IsNphiesMember | bit | | Member of NPHIES? |
| SupportedClaimTypes | nvarchar(500) | | Comma-separated types |
| SupportedEligibilityTypes | nvarchar(500) | | Supported eligibility |
| MaxClaimsPerDay | int | | Rate limit |
| MaxClaimAmount | decimal(18,2) | | Maximum claim value |
| CurrencyCode | nvarchar(3) | DEFAULT 'SAR' | |
| IsActive | bit | NOT NULL, DEFAULT 1 | |
| CreatedDate | datetime2 | NOT NULL, DEFAULT GETUTCDATE() | |
| ModifiedDate | datetime2 | | |

**Indexes:**
- PK: Id
- UNIQUE: PayerId
- INDEX: PayerType
- INDEX: IsActive

---

### Table: PayerPolicyMaster
Purpose: Insurance policies offered by payer

| Column | Type | Constraints | Notes |
|--------|------|-------------|-------|
| Id | int | PK, Identity | Primary key |
| PayerMasterId | int | FK to PayerMaster | Payer reference |
| PolicyCode | nvarchar(100) | NOT NULL, UNIQUE | Policy code |
| PolicyName | nvarchar(255) | NOT NULL | Policy name |
| PolicyType | nvarchar(100) | | Family, Individual, Corporate |
| CoverageType | nvarchar(100) | | Medical, Dental, Vision, Comprehensive |
| AnnualPremium | decimal(18,2) | | Yearly cost |
| CurrencyCode | nvarchar(3) | DEFAULT 'SAR' | |
| AnnualDeductible | decimal(18,2) | | Deductible amount |
| MaxOutOfPocket | decimal(18,2) | | OOP maximum |
| Copay | decimal(18,2) | | Per-visit copay |
| CoinsurancePercentage | decimal(5,2) | | Coinsurance % |
| CoverageLimitPerVisit | decimal(18,2) | | Max per visit |
| CoverageLimitPerYear | decimal(18,2) | | Max per year |
| PreAuthRequiredForAmount | decimal(18,2) | | Pre-auth threshold |
| EffectiveFromDate | date | NOT NULL | Policy effective date |
| EffectiveToDate | date | | Policy expiry |
| IsPolicyActive | bit | NOT NULL, DEFAULT 1 | Currently active? |
| Notes | nvarchar(1000) | | Additional info |
| CreatedDate | datetime2 | NOT NULL, DEFAULT GETUTCDATE() | |
| ModifiedDate | datetime2 | | |

**Relationships:**
- FK to PayerMaster

**Indexes:**
- PK: Id
- FK: PayerMasterId
- UNIQUE: PolicyCode
- INDEX: PolicyType
- INDEX: CoverageType
- INDEX: EffectiveFromDate
- INDEX: IsPolicyActive

---

### Table: PolicyBenefitCoverage
Purpose: Benefits covered under each policy

| Column | Type | Constraints | Notes |
|--------|------|-------------|-------|
| Id | int | PK, Identity | Primary key |
| PolicyMasterId | int | FK to PayerPolicyMaster | Policy reference |
| ServiceCodeId | int | FK to ServiceCodeMaster | Service reference |
| BenefitType | nvarchar(100) | | Consultation, Procedure, Medication |
| CoveragePercentage | decimal(5,2) | | % covered (0-100) |
| MaxCoverageAmount | decimal(18,2) | | Max amount covered |
| RequiresPreAuth | bit | | Pre-authorization needed? |
| RequiresReferral | bit | | Referral needed? |
| PreAuthValidityDays | int | | How long is pre-auth valid |
| CoverageLimitPerYear | int | | How many times per year |
| CoverageLimitPerLifetime | int | | Lifetime limit |
| IsExcluded | bit | | Explicitly excluded? |
| ExclusionReason | nvarchar(500) | | Why excluded |
| IsWaitingPeriodApplicable | bit | | Waiting period? |
| WaitingPeriodDays | int | | Days of waiting |
| Notes | nvarchar(1000) | | Additional info |
| CreatedDate | datetime2 | NOT NULL, DEFAULT GETUTCDATE() | |
| ModifiedDate | datetime2 | | |

**Relationships:**
- FK to PayerPolicyMaster
- FK to ServiceCodeMaster

---

### Table: ClaimSubmissionRules
Purpose: Rules for claim submission per payer/policy

| Column | Type | Constraints | Notes |
|--------|------|-------------|-------|
| Id | int | PK, Identity | Primary key |
| PayerMasterId | int | FK to PayerMaster | Payer reference |
| PolicyMasterId | int | FK to PayerPolicyMaster | Policy (optional) |
| RuleName | nvarchar(255) | NOT NULL | Rule name |
| RuleType | nvarchar(100) | | Validation, Requirement, Limit |
| RuleCondition | nvarchar(1000) | | Rule logic/condition |
| MaxClaimAmount | decimal(18,2) | | Max claim value |
| MaxItemsPerClaim | int | | Max items |
| RequiresInvoice | bit | | Invoice required? |
| RequiresMedicalReport | bit | | Medical report required? |
| RequiresPhotos | bit | | Photos/scans required? |
| MaxDaysForSubmission | int | | Days after service to submit |
| IsActive | bit | NOT NULL, DEFAULT 1 | Active? |
| Priority | int | | Execution priority |
| CreatedDate | datetime2 | NOT NULL, DEFAULT GETUTCDATE() | |
| ModifiedDate | datetime2 | | |

---

## 4?? NPHIES SPECIFIC MAPPINGS

### Table: NphiesCodeMapping
Purpose: Centralized NPHIES code mappings

| Column | Type | Constraints | Notes |
|--------|------|-------------|-------|
| Id | int | PK, Identity | Primary key |
| LocalCode | nvarchar(50) | NOT NULL | Your local code |
| LocalCodeSystem | nvarchar(500) | | Your code system |
| LocalDescription | nvarchar(255) | | Description |
| NphiesCode | nvarchar(50) | NOT NULL | NPHIES code |
| NphiesCodeSystem | nvarchar(500) | NOT NULL | NPHIES system |
| NphiesDescription | nvarchar(255) | | NPHIES description |
| CodeType | nvarchar(50) | | Service, Diagnosis, Procedure, Medication, Device |
| IsMappingValid | bit | NOT NULL | Valid mapping? |
| MappingValidationDate | datetime2 | | Last validated |
| Notes | nvarchar(1000) | | Comments |
| CreatedDate | datetime2 | NOT NULL, DEFAULT GETUTCDATE() | |
| ModifiedDate | datetime2 | | |

**Indexes:**
- PK: Id
- UNIQUE: (LocalCode + LocalCodeSystem)
- INDEX: NphiesCode
- INDEX: CodeType
- INDEX: IsMappingValid

---

## 5?? CLINICAL MASTERS

### Table: DiagnosisCodeMaster
Purpose: ICD diagnosis codes

| Column | Type | Constraints | Notes |
|--------|------|-------------|-------|
| Id | int | PK, Identity | Primary key |
| DiagnosisCode | nvarchar(20) | NOT NULL, UNIQUE | ICD code (e.g., A00) |
| DiagnosisName | nvarchar(255) | NOT NULL | Diagnosis name |
| DiagnosisCategory | nvarchar(100) | | Category |
| DiagnosisType | nvarchar(50) | | Primary, Secondary, Comorbidity |
| IsOnAdmission | bit | | Present on admission? |
| NphiesDiagnosisCode | nvarchar(20) | | NPHIES equivalent |
| IsNphiesMapped | bit | | Mapped to NPHIES? |
| Severity | nvarchar(50) | | Mild, Moderate, Severe |
| RequiresDocumentation | bit | | Documentation needed? |
| IsActive | bit | NOT NULL, DEFAULT 1 | |
| CreatedDate | datetime2 | NOT NULL, DEFAULT GETUTCDATE() | |

**Indexes:**
- PK: Id
- UNIQUE: DiagnosisCode
- INDEX: DiagnosisCategory
- INDEX: NphiesDiagnosisCode

---

### Table: ModifierCodeMaster
Purpose: Procedure modifiers

| Column | Type | Constraints | Notes |
|--------|------|-------------|-------|
| Id | int | PK, Identity | Primary key |
| ModifierCode | nvarchar(10) | NOT NULL, UNIQUE | Modifier code |
| ModifierName | nvarchar(255) | NOT NULL | Name |
| ModifierDescription | nvarchar(500) | | Description |
| ModifierType | nvarchar(50) | | Anatomical, Procedural, etc. |
| ImpactOnCharges | nvarchar(100) | | Increases, Decreases, None |
| ChargePercentage | decimal(5,2) | | % impact |
| NphiesModifierCode | nvarchar(10) | | NPHIES code |
| IsActive | bit | NOT NULL, DEFAULT 1 | |
| CreatedDate | datetime2 | NOT NULL, DEFAULT GETUTCDATE() | |

---

### Table: BenefitCodeMaster
Purpose: Benefit category codes

| Column | Type | Constraints | Notes |
|--------|------|-------------|-------|
| Id | int | PK, Identity | Primary key |
| BenefitCode | nvarchar(50) | NOT NULL, UNIQUE | Code |
| BenefitName | nvarchar(255) | NOT NULL | Benefit name |
| BenefitCategory | nvarchar(100) | | Medical, Dental, Vision, Mental |
| IsActive | bit | NOT NULL, DEFAULT 1 | |
| CreatedDate | datetime2 | NOT NULL, DEFAULT GETUTCDATE() | |

---

????????????????????????????????????????????????????????????????????????
?? IMPLEMENTATION ROADMAP
????????????????????????????????????????????????????????????????????????

## PHASE 1: CREATE ENTITIES (Week 1)

Priority 1 (MUST HAVE):
- [ ] ServiceCodeMaster
- [ ] MedicationCodeMaster
- [ ] PayerMaster
- [ ] PayerPolicyMaster
- [ ] DiagnosisCodeMaster

Priority 2 (SHOULD HAVE):
- [ ] ClinicMaster
- [ ] DoctorMaster
- [ ] MedicalDeviceCodeMaster
- [ ] ModifierCodeMaster
- [ ] NphiesCodeMapping

Priority 3 (NICE TO HAVE):
- [ ] DoctorQualifications
- [ ] PolicyBenefitCoverage
- [ ] ClaimSubmissionRules
- [ ] BenefitCodeMaster

## PHASE 2: CREATE MIGRATIONS & SEEDING (Week 1)

- [ ] Generate EF Core migrations
- [ ] Create seed data scripts
- [ ] Load NPHIES code mappings
- [ ] Load local service codes
- [ ] Load payer/policy data

## PHASE 3: CREATE DATA ACCESS LAYER (Week 2)

- [ ] Create repositories for each master table
- [ ] Create generic master data repository
- [ ] Implement caching for master data
- [ ] Create bulk import utilities

## PHASE 4: CREATE SERVICE LAYER (Week 2)

- [ ] MasterDataService
- [ ] CodeMappingService
- [ ] ValidationService (using master data)
- [ ] PolicyRulesService

## PHASE 5: CREATE API ENDPOINTS (Week 2)

- [ ] MasterDataController
- [ ] CodeMappingController
- [ ] ValidationController
- [ ] GET endpoints for lookups
- [ ] POST/PUT for admin operations

## PHASE 6: TESTING (Week 3)

- [ ] Unit tests for services
- [ ] Integration tests with database
- [ ] Validation tests
- [ ] Load tests for lookup queries

---

????????????????????????????????????????????????????????????????????????
?? IMPACT ON EXISTING CODE
????????????????????????????????????????????????????????????????????????

## How These Master Tables Will Be Used:

### 1. In Claim Submission
```csharp
// Before: Hard-coded values
var claim = new Claim { 
    ClaimType = "institutional",
    Items = new[] { 
    new ClaimItem { ProductOrServiceCode = "12345" }
    }
};

// After: Validated with master data
var service = await masterDataService.GetServiceCodeAsync("SRV001");
var claim = new Claim {
    ClaimType = service.ClaimType,  // From ServiceCodeMaster
    Items = new[] {
        new ClaimItem { 
     ProductOrServiceCode = service.NphiesServiceCode  // NPHIES mapped
        }
    }
};
```

### 2. In Eligibility Checks
```csharp
// Validate requested services against policy
var policy = await policyService.GetPolicyAsync(policyId);
var benefitCoverage = await policyService.GetBenefitCoverageAsync(
    policy.Id, 
    serviceCode
);

if (benefitCoverage.RequiresPreAuth) {
    // Request pre-authorization
}
```

### 3. In Claim Processing
```csharp
// Apply business rules from master data
var rules = await claimRulesService.GetApplicableRulesAsync(
    payerId, 
policyId, 
    claimType
);

foreach (var rule in rules) {
    ValidateClaimAgainstRule(claim, rule);
}
```

### 4. In Authorization
```csharp
// Check pre-auth requirements
var authRule = await authorizationService.GetAuthorizationRuleAsync(
    serviceCode,
    patientAge,
    diagnosisCode
);

if (authRule.RequiresPreAuth) {
    // Submit pre-auth request
}
```

---

????????????????????????????????????????????????????????????????????????
? WHAT YOU DO NEXT - ACTION PLAN
????????????????????????????????????????????????????????????????????????

## IMMEDIATE NEXT STEPS (This Week):

1. **Review & Confirm Master Tables**
   - Review the tables above
   - Adjust based on your specific NPHIES requirements
   - Add any missing columns specific to Saudi Arabia healthcare

2. **Create Entity Classes**
   - Implement all entities in Domain project
   - Add proper attributes and validations
   - Add relationships between master tables

3. **Update DbContext**
- Add DbSet<> for each master table
   - Configure relationships
   - Add indexes

4. **Create Migrations**
   - Generate EF Core migrations
   - Apply to database

5. **Seed Initial Data**
   - Load NPHIES codes
   - Load your local service codes
   - Load payer/policy data

6. **Create Repository Layer**
   - Implement repositories
   - Add filtering/searching
   - Add caching

7. **Create Service Layer**
 - Implement business logic
   - Add validation logic
   - Add code mapping logic

8. **Create API Endpoints**
   - Lookup endpoints (for dropdowns)
   - Admin endpoints (for maintenance)
   - Validation endpoints

---

## SAMPLE DATA TO PREPARE:

### You Need to Provide:

1. **ServiceCodeMaster** - All medical services you provide
   - Your service codes
   - Service names & descriptions
   - NPHIES mapping (you get from NPHIES or use their codes directly)
   - Pricing information

2. **MedicationCodeMaster** - All medications
   - Medicine codes
   - Generic names
   - Brand names
   - NPHIES mappings

3. **PayerMaster & PolicyMaster** - Insurance companies and policies
   - Payer codes
   - Policy types & rules
   - Benefits & coverage limits
   - Authorization rules

4. **ClinicMaster** - Your clinic information
   - Clinic details
   - Available services
   - Doctor allocations
   - Working hours

5. **DoctorMaster** - Your doctors
   - License information
   - Specializations
   - Qualifications

---

????????????????????????????????????????????????????????????????????????
?? SUMMARY - DECISION TIME
????????????????????????????????????????????????????????????????????????

## DO YOU WANT ME TO:

1. **Option A: Create All Master Tables** (Recommended)
   - I create all entity classes
   - I update DbContext
   - I create migrations
   - I provide seed data scripts
   - Timeline: 2-3 hours

2. **Option B: Create Priority 1 Tables Only**
   - ServiceCodeMaster
   - MedicationCodeMaster
   - PayerMaster
   - PayerPolicyMaster
   - DiagnosisCodeMaster
   - Timeline: 1 hour

3. **Option C: Create Specific Tables**
   - You tell me which ones
   - Timeline: 30 mins per table

---

## MY RECOMMENDATION:

? **Go with Option A** (All Tables)

**Why?**
- You need comprehensive master data for proper NPHIES integration
- All tables are interdependent
- Once created, testing becomes much easier
- Without these, unit tests will fail on validation
- Production deployment requires this data

**Risk of NOT doing this:**
- ? Claims will be rejected due to invalid codes
- ? Eligibility checks will fail without policy rules
- ? No validation of provider/doctor data
- ? Manual intervention required for each transaction
- ? Cannot scale to production

---

## DECISION:

Should I proceed with:
- [ ] Option A: All Master Tables
- [ ] Option B: Priority 1 Only
- [ ] Option C: Specific Tables (please list)

```
