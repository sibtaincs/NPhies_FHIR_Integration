# NPHIES RCM Project - Developer Implementation Guide

## ?? Table of Contents
- [Project Overview](#project-overview)
- [Current Implementation Status](#current-implementation-status)
- [Critical Gaps & Priorities](#critical-gaps--priorities)
- [Development Roadmap](#development-roadmap)
- [Module-by-Module Implementation Plan](#module-by-module-implementation-plan)
- [NPHIES Compliance Requirements](#nphies-compliance-requirements)
- [Technical Architecture](#technical-architecture)
- [Development Standards](#development-standards)
- [Testing Strategy](#testing-strategy)

---

## ?? Project Overview

This project implements a complete Revenue Cycle Management (RCM) system compliant with NPHIES (National Platform for Health Insurance Exchange Services) guidelines for Saudi Arabia's healthcare system.

**Technology Stack:**
- .NET 9
- C# 13.0
- Entity Framework Core 9
- SQL Server
- FHIR R4 (HL7.Fhir.R4)
- ASP.NET Core Web API

**NPHIES References:**
- Official Portal: https://nphies.sa/
- Implementation Guide: https://portal.nphies.sa/ig/index.html

---

## ?? Current Implementation Status

### ? Completed Modules (50-80% Complete)

| Module | Status | Completion | Notes |
|--------|--------|------------|-------|
| **Eligibility Validation** | ?? Good | 80% | Service layer complete, FHIR bundling missing |
| **Pre-Authorization** | ?? Fair | 70% | Core workflow done, FHIR generation needed |
| **Claims Processing** | ?? Fair | 65% | Domain models exist, FHIR serialization missing |
| **Adjudication Engine** | ?? Good | 75% | 10 rules implemented, medical necessity AI pending |
| **Payment Reconciliation** | ?? Fair | 60% | Basic matching done, advanced features needed |
| **Domain Entities** | ?? Good | 85% | Most FHIR resources modeled |
| **CodeableConcept System** | ?? Excellent | 90% | Comprehensive terminology database |
| **Validation Services** | ?? Fair | 70% | Patient/message validation exists |

### ? Missing Critical Components (0-30% Complete)

| Component | Status | Completion | Priority |
|-----------|--------|------------|----------|
| **FHIR Bundle Generation** | ?? Critical | 10% | **P0 - BLOCKER** |
| **NPHIES API Client** | ?? Critical | 0% | **P0 - BLOCKER** |
| **Encounter Management** | ?? Critical | 0% | **P0 - BLOCKER** |
| **Task & Polling** | ?? Needs Work | 30% | **P1 - High** |
| **Extensions & Profiles** | ?? Needs Work | 20% | **P1 - High** |
| **Communication Resource** | ?? Needs Work | 10% | **P2 - Medium** |
| **Document/Attachment** | ?? Needs Work | 10% | **P2 - Medium** |
| **Prescription Management** | ?? Missing | 0% | **P2 - Medium** |
| **Error Handling & Retry** | ?? Needs Work | 20% | **P1 - High** |
| **Integration Tests** | ?? Missing | 0% | **P1 - High** |

---

## ?? Critical Gaps & Priorities

### Priority 0 (BLOCKERS) - Must Complete First

#### 1. FHIR Bundle Generation Service
**Impact:** Cannot submit ANY requests to NPHIES without proper FHIR bundles

**What's Missing:**
- No FHIR R4 serialization/deserialization
- Bundle structure not compliant with NPHIES IG
- MessageHeader not included in bundles
- No resource bundling (Patient, Organization, Practitioner)

**Implementation Required:**
```csharp
// File: NPhies_FHIR_Integration.Application/Services/FHIR/FhirBundleService.cs

public interface IFhirBundleService
{
    // Eligibility
    Task<Bundle> CreateEligibilityRequestBundleAsync(CoverageEligibilityRequest request);
    Task<CoverageEligibilityResponse> ParseEligibilityResponseAsync(Bundle responseBundle);
    
    // Claims
    Task<Bundle> CreateClaimRequestBundleAsync(Claim claim);
 Task<ClaimResponse> ParseClaimResponseAsync(Bundle responseBundle);
    
    // Pre-Authorization
    Task<Bundle> CreatePreAuthRequestBundleAsync(Claim preAuth);
    Task<ClaimResponse> ParsePreAuthResponseAsync(Bundle responseBundle);
    
    // Communication
    Task<Bundle> CreateCommunicationRequestBundleAsync(CommunicationRequest commRequest);
    
    // Polling
    Task<Bundle> CreatePollRequestBundleAsync(string taskId);
    
    // Serialization
    Task<string> SerializeToJsonAsync(Bundle bundle);
    Task<Bundle> DeserializeFromJsonAsync(string json);
    Task<T> ParseResourceFromBundleAsync<T>(Bundle bundle) where T : Resource;
}
```

**Dependencies to Install:**
```xml
<PackageReference Include="Hl7.Fhir.R4" Version="5.8.0" />
<PackageReference Include="Hl7.Fhir.Serialization" Version="5.8.0" />
<PackageReference Include="Hl7.Fhir.Support" Version="5.8.0" />
```

---

#### 2. NPHIES API Client
**Impact:** Cannot communicate with NPHIES platform

**What's Missing:**
- No HTTP client configured for NPHIES endpoints
- No OAuth2 authentication flow
- No request/response handling
- No retry logic or circuit breaker

**Implementation Required:**
```csharp
// File: NPhies_FHIR_Integration.Infrastructure/Integration/NphiesApiClient.cs

public interface INphiesApiClient
{
    // Authentication
    Task<string> GetAuthTokenAsync();
    Task RefreshTokenAsync();
    
    // Eligibility
    Task<HttpResponseMessage> SubmitEligibilityRequestAsync(Bundle bundle);
    
    // Claims
    Task<HttpResponseMessage> SubmitClaimAsync(Bundle bundle);
    
    // Pre-Authorization
    Task<HttpResponseMessage> SubmitPreAuthAsync(Bundle bundle);
    
    // Polling
    Task<HttpResponseMessage> PollResponseAsync(string taskId);
    
    // Communication
    Task<HttpResponseMessage> SendCommunicationAsync(Bundle bundle);
    
    // Cancel
    Task<HttpResponseMessage> CancelRequestAsync(Bundle bundle);
}

public class NphiesApiClientConfiguration
{
    public string BaseUrl { get; set; } = "https://api.nphies.sa"; // Production
    public string SandboxUrl { get; set; } = "https://sandbox.nphies.sa"; // Testing
    public string ClientId { get; set; }
    public string ClientSecret { get; set; }
    public string ProviderLicense { get; set; }
    public int TimeoutSeconds { get; set; } = 30;
    public int MaxRetries { get; set; } = 3;
  public bool UseSandbox { get; set; } = true;
}
```

**Configuration Required:**
```json
// appsettings.json
{
  "NphiesApi": {
    "BaseUrl": "https://sandbox.nphies.sa/fhir",
    "ClientId": "your-provider-client-id",
    "ClientSecret": "your-client-secret",
    "ProviderLicense": "your-license-number",
    "TimeoutSeconds": 30,
    "MaxRetries": 3,
    "UseSandbox": true
  }
}
```

---

#### 3. Encounter Entity & Service
**Impact:** Claims without encounters will be REJECTED by NPHIES

**What's Missing:**
- No Encounter entity exists
- No encounter creation service
- No encounter-claim linkage
- No diagnosis/procedure linking to encounters

**Implementation Required:**
```csharp
// File: NPhies_FHIR_Integration.Domain/Entities/Encounter.cs

public class Encounter : BaseEntity
{
    public string EncounterId { get; set; } = string.Empty;
    
    // Status: planned | arrived | triaged | in-progress | onleave | finished | cancelled
    public string Status { get; set; } = "in-progress";
    
    // Class: AMB (ambulatory), EMER (emergency), IMP (inpatient), HH (home health)
    public string Class { get; set; } = "AMB";
    public string? ClassSystem { get; set; }
    
    // Type of encounter
  public string? Type { get; set; }
    public string? TypeSystem { get; set; }
    
    // Service type
    public string? ServiceType { get; set; }
    public string? ServiceTypeSystem { get; set; }
 
    // Priority: routine | urgent | emergency
public string? Priority { get; set; }
    public string? PrioritySystem { get; set; }
    
    // Period
  public DateTime PeriodStart { get; set; }
    public DateTime? PeriodEnd { get; set; }
    
    // References
public string PatientId { get; set; } = string.Empty;
    public virtual Patient? Patient { get; set; }
    
    public string? LocationId { get; set; }
    public virtual Location? Location { get; set; }
    
    public string? ServiceProviderId { get; set; }
    public virtual Organization? ServiceProvider { get; set; }
    
    // Participant (doctors, nurses)
    public virtual ICollection<EncounterParticipant> Participants { get; set; } = new List<EncounterParticipant>();
  
    // Diagnoses
    public virtual ICollection<EncounterDiagnosis> Diagnoses { get; set; } = new List<EncounterDiagnosis>();
    
    // Related claims
    public virtual ICollection<Claim> Claims { get; set; } = new List<Claim>();
    
    // Hospitalization details (if inpatient)
    public string? AdmitSource { get; set; }
    public string? DischargeDisposition { get; set; }
    public DateTime? AdmitDate { get; set; }
    public DateTime? DischargeDate { get; set; }
}

public class EncounterParticipant : BaseEntity
{
    public string EncounterId { get; set; } = string.Empty;
    public virtual Encounter? Encounter { get; set; }
    
    public string? Type { get; set; } // ATND (attender), PPRF (primary performer)
    public string? TypeSystem { get; set; }
    
    public string PractitionerId { get; set; } = string.Empty;
    public virtual Practitioner? Practitioner { get; set; }
    
    public DateTime? PeriodStart { get; set; }
    public DateTime? PeriodEnd { get; set; }
}

public class EncounterDiagnosis : BaseEntity
{
    public string EncounterId { get; set; } = string.Empty;
    public virtual Encounter? Encounter { get; set; }
    
    public int Rank { get; set; } // 1 = principal diagnosis

    public string DiagnosisCode { get; set; } = string.Empty;
    public string? DiagnosisSystem { get; set; }
    public string? DiagnosisDisplay { get; set; }
    
    // Use: billing | admission | discharge
    public string? Use { get; set; }
    public string? UseSystem { get; set; }
}
```

**Service Required:**
```csharp
// File: NPhies_FHIR_Integration.Application/Services/EncounterService.cs

public interface IEncounterService
{
    Task<EncounterDto> CreateEncounterAsync(CreateEncounterDto dto);
    Task<EncounterDto> GetEncounterAsync(string encounterId);
    Task<EncounterDto> UpdateEncounterAsync(string encounterId, UpdateEncounterDto dto);
    Task<EncounterDto> CompleteEncounterAsync(string encounterId);
    Task<List<EncounterDto>> GetPatientEncountersAsync(string patientId);
Task<EncounterDto> AddDiagnosisAsync(string encounterId, EncounterDiagnosisDto diagnosis);
    Task<EncounterDto> AddParticipantAsync(string encounterId, EncounterParticipantDto participant);
}
```

---

### Priority 1 (HIGH) - Complete After P0

#### 4. Task & Polling Service
**NPHIES Workflow:**
```
1. Submit Request ? Receive HTTP 202 + Task Resource
2. Poll Task.status every 5-30 seconds
3. When Task.status = "completed" ? Fetch response from Task.output
4. Process ClaimResponse/EligibilityResponse
```

**Implementation Required:**
```csharp
// File: NPhies_FHIR_Integration.Application/Services/Polling/NphiesPollingService.cs

public interface INphiesPollingService
{
    Task<TaskResource> SubmitAndPollAsync(Bundle requestBundle, int maxAttempts = 60, int intervalSeconds = 5);
    Task<TaskResource> PollTaskStatusAsync(string taskId);
    Task<Bundle> GetTaskOutputAsync(TaskResource task);
    Task<List<TaskResource>> GetPendingTasksAsync();
}

public class TaskResource : BaseEntity
{
    public string TaskId { get; set; } = string.Empty;
    
    // Status: draft | requested | received | accepted | rejected | 
    //         ready | cancelled | in-progress | on-hold | failed | completed
    public string Status { get; set; } = "received";
    
    // Intent: unknown | proposal | plan | order | original-order | reflex-order | 
    //   filler-order | instance-order | option
    public string Intent { get; set; } = "order";
    
  // Code: What task is trying to accomplish
    public string? Code { get; set; }
    public string? CodeSystem { get; set; }
    
    // Focus: What resource task is acting on (Claim, EligibilityRequest)
    public string? FocusReference { get; set; }
    
    // For: Patient reference
  public string? PatientReference { get; set; }
    
    // Requester: Who requested task
    public string? RequesterReference { get; set; }
    
    // Owner: Who is responsible
    public string? OwnerReference { get; set; }
    
    // Times
    public DateTime? AuthoredOn { get; set; }
    public DateTime? LastModified { get; set; }
    
    // Business status: waiting | processing | completed | error
    public string? BusinessStatus { get; set; }
    
    // Output: Result of task (ClaimResponse bundle reference)
    public string? OutputReference { get; set; }
    public string? OutputJson { get; set; }
}
```

---

#### 5. NPHIES Extensions Implementation
**Required Extensions:**

```csharp
// File: NPhies_FHIR_Integration.Application/Services/FHIR/NphiesExtensions.cs

public static class NphiesExtensionUrls
{
    // Claim Extensions
    public const string AuthorizationType = 
      "http://nphies.sa/fhir/ksa/nphies-fs/StructureDefinition/extension-authorization-type";
    public const string Episode = 
        "http://nphies.sa/fhir/ksa/nphies-fs/StructureDefinition/extension-episode";
    public const string PatientShare = 
        "http://nphies.sa/fhir/ksa/nphies-fs/StructureDefinition/extension-patient-share";
    public const string PatientInvoice = 
        "http://nphies.sa/fhir/ksa/nphies-fs/StructureDefinition/extension-patient-invoice";
  public const string Tax = 
      "http://nphies.sa/fhir/ksa/nphies-fs/StructureDefinition/extension-tax";
    
    // ClaimItem Extensions
    public const string MedicationType = 
        "http://nphies.sa/fhir/ksa/nphies-fs/StructureDefinition/extension-medication-type";
    public const string PrescribedMedication = 
        "http://nphies.sa/fhir/ksa/nphies-fs/StructureDefinition/extension-prescribed-medication";
 public const string PharmaceuticalDosageForm = 
        "http://nphies.sa/fhir/ksa/nphies-fs/StructureDefinition/extension-pharmaceutical-dosage-form";
    
  // Coverage Extensions
    public const string MaxLimit = 
        "http://nphies.sa/fhir/ksa/nphies-fs/StructureDefinition/extension-max-limit";
    
    // Patient Extensions
    public const string Nationality = 
        "http://nphies.sa/fhir/ksa/nphies-fs/StructureDefinition/extension-nationality";
    public const string Religion = 
  "http://nphies.sa/fhir/ksa/nphies-fs/StructureDefinition/extension-religion";
}

public static class NphiesExtensionHelper
{
    public static void AddAuthorizationType(this Claim claim, string authType)
    {
   claim.Extension = claim.Extension ?? new List<Extension>();
      claim.Extension.Add(new Extension
        {
       Url = NphiesExtensionUrls.AuthorizationType,
          Value = new Code(authType) // "initial" | "reissue" | "extension"
        });
    }
    
    public static void AddEpisode(this Claim claim, string episodeSystem, string episodeValue)
    {
     claim.Extension = claim.Extension ?? new List<Extension>();
        claim.Extension.Add(new Extension
        {
            Url = NphiesExtensionUrls.Episode,
 Value = new Identifier
      {
      System = episodeSystem,
      Value = episodeValue
  }
   });
    }
    
    public static void AddPatientShare(this Claim claim, decimal amount, string currency = "SAR")
    {
        claim.Extension = claim.Extension ?? new List<Extension>();
        claim.Extension.Add(new Extension
     {
     Url = NphiesExtensionUrls.PatientShare,
            Value = new Money
        {
  Value = amount,
         Currency = currency
      }
        });
    }
}
```

---

#### 6. Error Handling & Retry Logic

**Implementation Required:**
```csharp
// File: NPhies_FHIR_Integration.Infrastructure/Resilience/ResiliencePolicy.cs

using Polly;
using Polly.CircuitBreaker;
using Polly.Retry;

public class NphiesResiliencePolicy
{
    private readonly ILogger _logger;
    
    public AsyncRetryPolicy<HttpResponseMessage> RetryPolicy { get; }
    public AsyncCircuitBreakerPolicy<HttpResponseMessage> CircuitBreakerPolicy { get; }
    
    public NphiesResiliencePolicy(ILogger logger)
    {
        _logger = logger;
        
        // Retry policy: 3 attempts with exponential backoff
   RetryPolicy = Policy<HttpResponseMessage>
            .Handle<HttpRequestException>()
            .OrResult(r => (int)r.StatusCode >= 500 || r.StatusCode == HttpStatusCode.RequestTimeout)
      .WaitAndRetryAsync(
    retryCount: 3,
      sleepDurationProvider: attempt => TimeSpan.FromSeconds(Math.Pow(2, attempt)),
   onRetry: (outcome, timespan, retryCount, context) =>
        {
       _logger.LogWarning(
             "NPHIES API call failed. Retry {RetryCount} after {Delay}ms. Status: {Status}",
        retryCount, timespan.TotalMilliseconds, outcome.Result?.StatusCode);
  });
        
        // Circuit breaker: Open after 5 consecutive failures, reset after 30 seconds
 CircuitBreakerPolicy = Policy<HttpResponseMessage>
   .Handle<HttpRequestException>()
      .OrResult(r => (int)r.StatusCode >= 500)
      .CircuitBreakerAsync(
  handledEventsAllowedBeforeBreaking: 5,
          durationOfBreak: TimeSpan.FromSeconds(30),
         onBreak: (outcome, duration) =>
            {
        _logger.LogError("NPHIES API circuit breaker opened for {Duration}s", duration.TotalSeconds);
          },
             onReset: () =>
             {
 _logger.LogInformation("NPHIES API circuit breaker reset");
                });
    }
}

// Install Polly packages:
// Install-Package Polly
// Install-Package Polly.Extensions.Http
```

---

### Priority 2 (MEDIUM) - Complete After P1

#### 7. Communication Resource

```csharp
// File: NPhies_FHIR_Integration.Domain/Entities/Communication.cs

public class Communication : BaseEntity
{
    public string CommunicationId { get; set; } = string.Empty;
    
    // Status: preparation | in-progress | not-done | on-hold | stopped | completed | entered-in-error | unknown
    public string Status { get; set; } = "completed";
    
  // Category: alert | notification | reminder | instruction
    public string? Category { get; set; }
    public string? CategorySystem { get; set; }
    
    // Priority: routine | urgent | asap | stat
    public string? Priority { get; set; }
    
    // Subject: Patient reference
    public string? PatientId { get; set; }
    public virtual Patient? Patient { get; set; }
    
    // About: Claim/Authorization reference
    public string? AboutClaimId { get; set; }
 public virtual Claim? AboutClaim { get; set; }
    
    // Sender & Recipient
    public string? SenderId { get; set; } // Organization
    public virtual Organization? Sender { get; set; }
    
    public string? RecipientId { get; set; } // Organization
    public virtual Organization? Recipient { get; set; }
    
    // Payload: The message content
    public string? ContentString { get; set; }
    public string? ContentAttachmentUrl { get; set; }
    
    // Sent & Received
    public DateTime? Sent { get; set; }
    public DateTime? Received { get; set; }
  
    // Reason: Why communication occurred
    public string? ReasonCode { get; set; }
 public string? ReasonSystem { get; set; }
}

public class CommunicationRequest : BaseEntity
{
    public string CommunicationRequestId { get; set; } = string.Empty;
    
    // Status: draft | active | on-hold | revoked | completed | entered-in-error | unknown
    public string Status { get; set; } = "active";
    
    // Priority: routine | urgent | asap | stat
    public string? Priority { get; set; }
    
  // Subject: Patient reference
    public string? PatientId { get; set; }
    public virtual Patient? Patient { get; set; }
    
    // About: Claim/Authorization reference
    public string? AboutClaimId { get; set; }
 public virtual Claim? AboutClaim { get; set; }
  
    // Requested communication content
    public string? RequestedContent { get; set; }
    
 // Authored on
    public DateTime? AuthoredOn { get; set; }
  
    // Requester
    public string? RequesterId { get; set; }
    public virtual Organization? Requester { get; set; }
    
    // Recipient
  public string? RecipientId { get; set; }
    public virtual Organization? Recipient { get; set; }
}
```

---

#### 8. Document & Attachment Management

```csharp
// File: NPhies_FHIR_Integration.Application/Services/Attachment/AttachmentService.cs

public interface IAttachmentService
{
    Task<AttachmentDto> UploadAttachmentAsync(IFormFile file, string claimId, string documentType);
    Task<AttachmentDto> GetAttachmentAsync(string attachmentId);
    Task<byte[]> DownloadAttachmentAsync(string attachmentId);
 Task<List<AttachmentDto>> GetClaimAttachmentsAsync(string claimId);
    Task<bool> DeleteAttachmentAsync(string attachmentId);
    Task<bool> ValidateAttachmentAsync(IFormFile file);
}

// File: NPhies_FHIR_Integration.Domain/Entities/DocumentReference.cs

public class DocumentReference : BaseEntity
{
    public string DocumentReferenceId { get; set; } = string.Empty;
    
    // Status: current | superseded | entered-in-error
    public string Status { get; set; } = "current";
    
    // Doc Status: preliminary | final | amended
    public string? DocStatus { get; set; }
    
    // Type: Lab report, prescription, discharge summary, etc.
    public string? Type { get; set; }
  public string? TypeSystem { get; set; }
    
    // Category: clinical-note | imaging | laboratory | pathology
    public string? Category { get; set; }
    public string? CategorySystem { get; set; }
    
    // Subject: Patient reference
    public string PatientId { get; set; } = string.Empty;
    public virtual Patient? Patient { get; set; }
    
    // Date
    public DateTime Date { get; set; }
    
    // Author
    public string? AuthorPractitionerId { get; set; }
    public virtual Practitioner? Author { get; set; }
    
    // Custodian
 public string? CustodianOrganizationId { get; set; }
    public virtual Organization? Custodian { get; set; }
    
    // Related to Claim
    public string? RelatedClaimId { get; set; }
    public virtual Claim? RelatedClaim { get; set; }
    
    // Content
    public virtual ICollection<DocumentContent> Contents { get; set; } = new List<DocumentContent>();
}

public class DocumentContent : BaseEntity
{
    public string DocumentReferenceId { get; set; } = string.Empty;
    public virtual DocumentReference? DocumentReference { get; set; }
    
    // Attachment
    public string? ContentType { get; set; } // application/pdf, image/jpeg
    public string? Title { get; set; }
    public string? Url { get; set; } // Storage URL
    public long? Size { get; set; }
    public string? Hash { get; set; } // SHA-256 hash
    
    // Or embedded data (Base64)
    public string? DataBase64 { get; set; }
    
    // Creation date
    public DateTime? CreationDate { get; set; }
}

// Validation rules
public class AttachmentValidationRules
{
    public static readonly List<string> AllowedContentTypes = new()
    {
        "application/pdf",
        "image/jpeg",
        "image/jpg",
  "image/png",
        "image/tiff"
    };
    
    public const long MaxFileSizeBytes = 5 * 1024 * 1024; // 5 MB
    public const long MaxTotalAttachmentsPerClaim = 10;
}
```

---

#### 9. Prescription & Medication Management

```csharp
// File: NPhies_FHIR_Integration.Domain/Entities/MedicationRequest.cs

public class MedicationRequest : BaseEntity
{
    public string MedicationRequestId { get; set; } = string.Empty;
    
  // Status: active | on-hold | cancelled | completed | entered-in-error | stopped | draft | unknown
    public string Status { get; set; } = "active";
    
    // Intent: proposal | plan | order | original-order | reflex-order | filler-order | instance-order | option
    public string Intent { get; set; } = "order";
    
  // Medication code
  public string MedicationCode { get; set; } = string.Empty;
    public string? MedicationSystem { get; set; }
    public string? MedicationDisplay { get; set; }
    
    // Patient
    public string PatientId { get; set; } = string.Empty;
    public virtual Patient? Patient { get; set; }
    
    // Encounter
    public string? EncounterId { get; set; }
    public virtual Encounter? Encounter { get; set; }
    
    // Authored on
    public DateTime AuthoredOn { get; set; }
    
    // Requester (Prescriber)
 public string? PrescriberId { get; set; }
    public virtual Practitioner? Prescriber { get; set; }
    
    // Reason
 public string? ReasonCode { get; set; }
    public string? ReasonSystem { get; set; }
    
    // Dosage instructions
    public string? DosageInstructions { get; set; }
 public decimal? DoseQuantity { get; set; }
    public string? DoseUnit { get; set; }
    
    // Dispense request
    public int? DispenseQuantity { get; set; }
    public string? DispenseUnit { get; set; }
    public int? ExpectedSupplyDuration { get; set; }
    public int? NumberOfRepeatsAllowed { get; set; }
    
    // Substitution
 public bool SubstitutionAllowed { get; set; } = true;
    
    // Related claim
    public virtual ICollection<Claim> Claims { get; set; } = new List<Claim>();
}

// File: NPhies_FHIR_Integration.Domain/Entities/VisionPrescription.cs

public class VisionPrescription : BaseEntity
{
    public string VisionPrescriptionId { get; set; } = string.Empty;
    
    // Status: active | cancelled | draft | entered-in-error
    public string Status { get; set; } = "active";
    
    // Patient
    public string PatientId { get; set; } = string.Empty;
    public virtual Patient? Patient { get; set; }
    
    // Encounter
    public string? EncounterId { get; set; }
    public virtual Encounter? Encounter { get; set; }
    
    // Created date
    public DateTime DateWritten { get; set; }
    
    // Prescriber
    public string PrescriberId { get; set; } = string.Empty;
    public virtual Practitioner? Prescriber { get; set; }
    
    // Lens specifications
  public virtual ICollection<VisionPrescriptionLensSpecification> LensSpecifications { get; set; } 
    = new List<VisionPrescriptionLensSpecification>();
}

public class VisionPrescriptionLensSpecification : BaseEntity
{
    public string VisionPrescriptionId { get; set; } = string.Empty;
    public virtual VisionPrescription? VisionPrescription { get; set; }
    
    // Product: lens | contact
    public string Product { get; set; } = string.Empty;
    public string? ProductSystem { get; set; }
    
    // Eye: right | left
    public string Eye { get; set; } = string.Empty;
    
    // Sphere
    public decimal? Sphere { get; set; }
    
    // Cylinder
    public decimal? Cylinder { get; set; }
    
    // Axis
    public int? Axis { get; set; }
    
    // Add
    public decimal? Add { get; set; }
    
    // Power
    public decimal? Power { get; set; }
    
    // Prism
    public decimal? PrismAmount { get; set; }
    public string? PrismBase { get; set; } // up | down | in | out
    
    // Duration
    public decimal? Duration { get; set; }
 public string? DurationUnit { get; set; }
}
```

---

## ??? Development Roadmap

### Phase 1: FHIR Foundation (Weeks 1-3)
**Goal:** Establish FHIR communication capability

| Week | Tasks | Owner | Status |
|------|-------|-------|--------|
| 1 | Install Firely.NET SDK | Dev Team | ? Pending |
| 1 | Implement FhirBundleService interface | Dev Team | ? Pending |
| 1 | Create MessageHeader builder | Dev Team | ? Pending |
| 2 | Implement eligibility bundle generation | Dev Team | ? Pending |
| 2 | Implement claim bundle generation | Dev Team | ? Pending |
| 2 | Implement pre-auth bundle generation | Dev Team | ? Pending |
| 3 | Bundle serialization/deserialization | Dev Team | ? Pending |
| 3 | Unit tests for FHIR services | QA Team | ? Pending |
| 3 | FHIR validator integration | Dev Team | ? Pending |

**Deliverables:**
- ? Working FHIR bundle generation for all message types
- ? Serialization to NPHIES-compliant JSON
- ? Deserialization of NPHIES responses
- ? Unit tests with 80%+ coverage

---

### Phase 2: NPHIES Integration (Weeks 4-6)
**Goal:** Connect to NPHIES platform

| Week | Tasks | Owner | Status |
|------|-------|-------|--------|
| 4 | Implement NphiesApiClient | Dev Team | ? Pending |
| 4 | OAuth2 authentication flow | Dev Team | ? Pending |
| 4 | Configuration management | Dev Team | ? Pending |
| 5 | Implement polling service | Dev Team | ? Pending |
| 5 | Task resource handling | Dev Team | ? Pending |
| 5 | Resilience policies (Polly) | Dev Team | ? Pending |
| 6 | Error handling & retry logic | Dev Team | ? Pending |
| 6 | Integration tests with sandbox | QA Team | ? Pending |
| 6 | API monitoring & logging | DevOps | ? Pending |

**Deliverables:**
- ? Working API client with authentication
- ? Successful sandbox eligibility check
- ? Successful sandbox claim submission
- ? Polling workflow implementation
- ? Error handling with 3 retry attempts

---

### Phase 3: Encounter & Clinical Data (Weeks 7-8)
**Goal:** Complete clinical data model

| Week | Tasks | Owner | Status |
|------|-------|-------|--------|
| 7 | Implement Encounter entity | Dev Team | ? Pending |
| 7 | Implement EncounterService | Dev Team | ? Pending |
| 7 | Encounter-Claim linking | Dev Team | ? Pending |
| 7 | Database migration | DevOps | ? Pending |
| 8 | Implement MedicationRequest | Dev Team | ? Pending |
| 8 | Implement VisionPrescription | Dev Team | ? Pending |
| 8 | Implement ServiceRequest | Dev Team | ? Pending |
| 8 | Update claim bundle to include encounters | Dev Team | ? Pending |

**Deliverables:**
- ? Encounter management API
- ? Prescription management API
- ? Complete clinical data in claim bundles

---

### Phase 4: Extensions & Compliance (Weeks 9-10)
**Goal:** NPHIES-specific features

| Week | Tasks | Owner | Status |
|------|-------|-------|--------|
| 9 | Implement all NPHIES extensions | Dev Team | ? Pending |
| 9 | Extension helper methods | Dev Team | ? Pending |
| 9 | Profile validation | Dev Team | ? Pending |
| 10 | Identifier system standardization | Dev Team | ? Pending |
| 10 | Code system validation | Dev Team | ? Pending |
| 10 | NPHIES IG compliance audit | QA Team | ? Pending |

**Deliverables:**
- ? All required extensions implemented
- ? Profile validation passing
- ? 100% NPHIES IG compliance

---

### Phase 5: Communication & Attachments (Weeks 11-12)
**Goal:** Document exchange capability

| Week | Tasks | Owner | Status |
|------|-------|-------|--------|
| 11 | Implement Communication entity | Dev Team | ? Pending |
| 11 | Implement CommunicationRequest | Dev Team | ? Pending |
| 11 | Communication workflow | Dev Team | ? Pending |
| 12 | Implement DocumentReference | Dev Team | ? Pending |
| 12 | File upload/download service | Dev Team | ? Pending |
| 12 | Attachment validation | Dev Team | ? Pending |
| 12 | Azure Blob Storage integration | DevOps | ? Pending |

**Deliverables:**
- ? Payer-provider messaging
- ? Attachment upload/download
- ? Document management system

---

### Phase 6: Testing & Production (Weeks 13-16)
**Goal:** Production readiness

| Week | Tasks | Owner | Status |
|------|-------|-------|--------|
| 13 | Integration test suite | QA Team | ? Pending |
| 13 | Performance testing | QA Team | ? Pending |
| 13 | Load testing | QA Team | ? Pending |
| 14 | Security audit | Security Team | ? Pending |
| 14 | Penetration testing | Security Team | ? Pending |
| 14 | NPHIES certification prep | Project Manager | ? Pending |
| 15 | Production deployment | DevOps | ? Pending |
| 15 | Monitoring & alerting setup | DevOps | ? Pending |
| 16 | Pilot payer integration | Business Team | ? Pending |
| 16 | Production validation | QA Team | ? Pending |

**Deliverables:**
- ? Complete test coverage
- ? Security clearance
- ? Production deployment
- ? Live payer integration

---

## ?? Technical Architecture

### Solution Structure
```
NPhies_FHIR_Integration/
??? NPhies_FHIR_Integration.Domain/
?   ??? Entities/
?   ?   ??? Patient.cs
?   ? ??? Organization.cs
?   ?   ??? Practitioner.cs
?   ? ??? Coverage.cs
?   ?   ??? Claim.cs
?   ?   ??? ClaimResponse.cs
?   ?   ??? Encounter.cs ?? TO ADD
?   ?   ??? MedicationRequest.cs ?? TO ADD
?   ?   ??? VisionPrescription.cs ?? TO ADD
?   ?   ??? Communication.cs ?? TO ADD
?   ?   ??? DocumentReference.cs ?? TO ADD
?   ?   ??? TaskResource.cs ?? TO ADD
?   ??? Interfaces/
?
??? NPhies_FHIR_Integration.Application/
?   ??? Services/
?   ?   ??? FHIR/
? ?   ?   ??? FhirBundleService.cs ?? TO ADD
?   ?   ?   ??? FhirSerializationService.cs ?? TO ADD
?   ?   ?   ??? NphiesExtensions.cs ?? TO ADD
? ?   ?   ??? FhirValidationService.cs ?? TO ADD
?   ?   ??? Integration/
?   ?   ?   ??? NphiesPollingService.cs ?? TO ADD
?   ?   ?   ??? TaskManagementService.cs ?? TO ADD
?   ?   ??? Clinical/
?   ?   ?   ??? EncounterService.cs ?? TO ADD
?   ?   ?   ??? MedicationService.cs ?? TO ADD
?   ?   ?   ??? VisionService.cs ?? TO ADD
?   ?   ??? Communication/
?   ? ?   ??? CommunicationService.cs ?? TO ADD
?   ?   ?   ??? AttachmentService.cs ?? TO ADD
?   ?   ??? (Existing services)
?   ??? DTOs/
?
??? NPhies_FHIR_Integration.Infrastructure/
?   ??? Integration/
?   ?   ??? NphiesApiClient.cs ?? TO ADD
?   ?   ??? NphiesAuthService.cs ?? TO ADD
?   ?   ??? ResiliencePolicy.cs ?? TO ADD
?   ??? Persistence/
?   ?   ??? Configurations/
?   ?   ?   ??? EncounterConfiguration.cs ?? TO ADD
?   ?   ?   ??? CommunicationConfiguration.cs ?? TO ADD
?   ?   ?   ??? TaskConfiguration.cs ?? TO ADD
?   ?   ??? ApplicationDbContext.cs (Update)
?   ??? Storage/
?       ??? BlobStorageService.cs ?? TO ADD
?
??? NPhies_FHIR_Integration.ApiService/
    ??? Controllers/
    ?   ??? EncounterController.cs ?? TO ADD
    ?   ??? MedicationController.cs ?? TO ADD
    ?   ??? CommunicationController.cs ?? TO ADD
  ?   ??? AttachmentController.cs ?? TO ADD
    ??? Program.cs (Update registrations)
```

---

## ?? Development Standards

### 1. FHIR Resource Naming
- Use FHIR R4 resource names exactly (e.g., `CoverageEligibilityRequest`, not `EligibilityRequest`)
- Use FHIR property names (e.g., `status`, not `Status`) in serialized JSON
- Entity classes can use C# conventions (PascalCase)

### 2. Identifier Systems
Always use official NPHIES identifiers:
```csharp
public static class NphiesIdentifierSystems
{
    // Patient
    public const string PatientIqama = "http://nphies.sa/identifier/iqama";
    public const string PatientNationalId = "http://nphies.sa/identifier/nationalid";
    public const string PatientBorderId = "http://nphies.sa/identifier/border";
    public const string PatientNewborn = "http://nphies.sa/identifier/newborn";
    
    // Organization
    public const string ProviderLicense = "http://nphies.sa/license/provider-license";
    public const string PayerLicense = "http://nphies.sa/license/payer-license";
    
    // Practitioner
    public const string PractitionerLicense = "http://nphies.sa/license/practitioner-license";
    
  // Claim
    public const string ClaimIdentifier = "http://provider.sa/claim";
    public const string AuthorizationNumber = "http://nphies.sa/identifier/authorization-number";
}
```

### 3. Code Systems
Use NPHIES-approved code systems:
```csharp
public static class NphiesCodeSystems
{
    // Diagnosis
    public const string ICD10 = "http://hl7.org/fhir/sid/icd-10";
    public const string ICD10AM = "http://hl7.org/fhir/sid/icd-10-am";
    
    // Procedures
    public const string ICD9CM = "http://terminology.hl7.org/CodeSystem/icd9cm";
    
    // Claim Type
    public const string ClaimType = "http://terminology.hl7.org/CodeSystem/claim-type";
    public const string ClaimSubType = "http://nphies.sa/terminology/CodeSystem/claim-subtype";
    
    // Services
    public const string NphiesServices = "http://nphies.sa/terminology/CodeSystem/services";
    
    // Medications
    public const string NphiesMedications = "http://nphies.sa/terminology/CodeSystem/medication-codes";
    
    // Adjudication Error
    public const string AdjudicationError = "http://nphies.sa/terminology/CodeSystem/adjudication-error";
}
```

### 4. Error Handling Pattern
```csharp
public async Task<Result<T>> MethodNameAsync()
{
    try
    {
        // Business logic
     _logger.LogInformation("Operation started: {OperationName}", nameof(MethodNameAsync));
        
      // Validate input
        if (!IsValid(input))
      {
            return Result<T>.Failure("Validation error message");
        }
   
        // Process
        var result = await ProcessAsync();
        
        _logger.LogInformation("Operation completed: {OperationName}", nameof(MethodNameAsync));
      return Result<T>.Success(result);
    }
    catch (NphiesApiException ex)
    {
      _logger.LogError(ex, "NPHIES API error in {OperationName}", nameof(MethodNameAsync));
        return Result<T>.Failure($"NPHIES error: {ex.Message}");
    }
    catch (Exception ex)
    {
        _logger.LogError(ex, "Unexpected error in {OperationName}", nameof(MethodNameAsync));
     return Result<T>.Failure("An unexpected error occurred");
    }
}
```

### 5. Logging Standards
```csharp
// Information logs
_logger.LogInformation(
    "Eligibility request submitted for patient {PatientId}, member {MemberId}", 
    patientId, memberId);

// Warning logs
_logger.LogWarning(
    "Coverage inactive for patient {PatientId}, status: {Status}", 
    patientId, coverage.Status);

// Error logs
_logger.LogError(ex, 
 "Failed to submit claim {ClaimId} to NPHIES. Error: {ErrorMessage}", 
    claimId, ex.Message);

// Debug logs (development only)
_logger.LogDebug(
    "FHIR Bundle generated: {BundleJson}", 
    bundleJson);
```

---

## ?? Testing Strategy

### Unit Tests
**Target Coverage:** 80%+

**Frameworks:**
- xUnit
- Moq
- FluentAssertions

**Example:**
```csharp
// File: NPhies_FHIR_Integration.Tests/Services/FhirBundleServiceTests.cs

public class FhirBundleServiceTests
{
    private readonly IFhirBundleService _service;
    private readonly Mock<ILogger<FhirBundleService>> _loggerMock;
    
    public FhirBundleServiceTests()
    {
        _loggerMock = new Mock<ILogger<FhirBundleService>>();
        _service = new FhirBundleService(_loggerMock.Object);
    }
    
    [Fact]
    public async Task CreateEligibilityBundleAsync_ValidRequest_ReturnsBundle()
    {
        // Arrange
        var request = new CoverageEligibilityRequest
        {
      PatientId = "patient-123",
          CoverageId = "coverage-456"
    };
        
 // Act
      var bundle = await _service.CreateEligibilityRequestBundleAsync(request);
        
        // Assert
   bundle.Should().NotBeNull();
    bundle.Type.Should().Be(Bundle.BundleType.Message);
 bundle.Entry.Should().HaveCountGreaterThan(0);
   
     // Verify MessageHeader exists
        var messageHeader = bundle.Entry
  .Select(e => e.Resource)
            .OfType<MessageHeader>()
            .FirstOrDefault();
    messageHeader.Should().NotBeNull();
    }
    
    [Fact]
 public async Task SerializeToJsonAsync_ValidBundle_ReturnsJson()
    {
        // Arrange
 var bundle = CreateTestBundle();
        
        // Act
        var json = await _service.SerializeToJsonAsync(bundle);
        
        // Assert
        json.Should().NotBeNullOrEmpty();
        json.Should().Contain("\"resourceType\": \"Bundle\"");
      json.Should().Contain("\"type\": \"message\"");
    }
}
```

### Integration Tests
**Target:** All API endpoints

**Example:**
```csharp
// File: NPhies_FHIR_Integration.Tests/Integration/NphiesApiClientTests.cs

public class NphiesApiClientIntegrationTests : IClassFixture<WebApplicationFactory<Program>>
{
    private readonly HttpClient _client;
    private readonly INphiesApiClient _nphiesClient;
    
    public NphiesApiClientIntegrationTests(WebApplicationFactory<Program> factory)
    {
 _client = factory.CreateClient();
        // Configure to use NPHIES sandbox
    }
    
    [Fact]
    public async Task SubmitEligibilityRequest_ValidRequest_ReturnsTask()
    {
        // Arrange
        var bundle = CreateEligibilityBundle();
   
        // Act
        var response = await _nphiesClient.SubmitEligibilityRequestAsync(bundle);
   
        // Assert
        response.IsSuccessStatusCode.Should().BeTrue();
        var taskResource = await ParseTaskFromResponse(response);
    taskResource.Status.Should().Be("received");
    }
}
```

### NPHIES Sandbox Testing
**Checklist:**
- [ ] Eligibility request/response
- [ ] Pre-authorization request/response
- [ ] Claim submission/response
- [ ] Communication exchange
- [ ] Document attachment
- [ ] Polling workflow
- [ ] Error handling (invalid data)
- [ ] Timeout scenarios

---

## ?? NPHIES Compliance Requirements

### Message Types to Implement

| Message Type | Request Resource | Response Resource | Status | Priority |
|--------------|------------------|-------------------|--------|----------|
| **eligibility-request** | CoverageEligibilityRequest | CoverageEligibilityResponse | ?? 70% | P0 |
| **priorauth-request** | Claim (use=preauthorization) | ClaimResponse | ?? 70% | P0 |
| **claim-request** | Claim (use=claim) | ClaimResponse | ?? 65% | P0 |
| **poll-request** | Parameters | Bundle (Task) | ? 10% | P1 |
| **cancel-request** | Task | Bundle | ?? 50% | P1 |
| **communication-request** | CommunicationRequest | Communication | ? 10% | P2 |
| **payment-notice** | PaymentNotice | - | ?? 60% | P2 |
| **payment-reconciliation** | PaymentReconciliation | - | ?? 60% | P2 |

### Required FHIR Profiles

**Must validate against:**
- http://nphies.sa/fhir/ksa/nphies-fs/StructureDefinition/patient
- http://nphies.sa/fhir/ksa/nphies-fs/StructureDefinition/organization
- http://nphies.sa/fhir/ksa/nphies-fs/StructureDefinition/practitioner
- http://nphies.sa/fhir/ksa/nphies-fs/StructureDefinition/coverage
- http://nphies.sa/fhir/ksa/nphies-fs/StructureDefinition/claim
- http://nphies.sa/fhir/ksa/nphies-fs/StructureDefinition/eligibility-request

### Business Rules to Enforce

1. **Eligibility Check:**
   - Must be performed within 24 hours before service
   - Member ID must be valid format (10 digits for Iqama)
   - Coverage must be active on service date

2. **Pre-Authorization:**
   - Required for services > SAR 10,000
   - Must include diagnosis and justification
   - Valid for 90 days from approval

3. **Claim Submission:**
   - Must be submitted within 90 days of service
   - Must include valid encounter reference
   - Diagnosis must support service codes
   - Patient share calculation must be accurate

4. **Encounter:**
   - Must have encounter for all institutional claims
   - Professional claims may omit encounter
   - Encounter dates must align with service dates

5. **Attachments:**
 - Maximum 5 MB per file
   - Allowed formats: PDF, JPEG, PNG, TIFF
   - Maximum 10 attachments per claim

---

## ?? Configuration & Setup

### appsettings.json
```json
{
  "ConnectionStrings": {
    "DefaultConnection": "Server=.;Database=NphiesFhirDb;Trusted_Connection=True;MultipleActiveResultSets=true;TrustServerCertificate=True",
    "CodeableConceptDb": "Server=.;Database=NphiesCodeableConcept;Trusted_Connection=True;MultipleActiveResultSets=true;TrustServerCertificate=True"
  },
  "NphiesApi": {
    "BaseUrl": "https://hsb.nphies.sa/fhir",
    "SandboxUrl": "https://sandbox.nphies.sa/fhir",
    "UseSandbox": true,
    "ClientId": "YOUR_CLIENT_ID",
    "ClientSecret": "YOUR_CLIENT_SECRET",
    "ProviderLicense": "YOUR_LICENSE",
    "TimeoutSeconds": 30,
    "MaxRetries": 3,
  "PollingIntervalSeconds": 5,
    "MaxPollingAttempts": 60
  },
  "BlobStorage": {
    "ConnectionString": "YOUR_AZURE_STORAGE_CONNECTION_STRING",
  "ContainerName": "claim-attachments"
  },
  "Logging": {
  "LogLevel": {
    "Default": "Information",
      "Microsoft.AspNetCore": "Warning",
      "NPhies_FHIR_Integration": "Debug"
    }
  }
}
```

### Package Dependencies
```xml
<!-- FHIR Packages -->
<PackageReference Include="Hl7.Fhir.R4" Version="5.8.0" />
<PackageReference Include="Hl7.Fhir.Serialization" Version="5.8.0" />
<PackageReference Include="Hl7.Fhir.Support" Version="5.8.0" />
<PackageReference Include="Hl7.Fhir.Validation" Version="5.8.0" />

<!-- Resilience -->
<PackageReference Include="Polly" Version="8.3.1" />
<PackageReference Include="Polly.Extensions.Http" Version="3.0.0" />

<!-- Azure Storage (for attachments) -->
<PackageReference Include="Azure.Storage.Blobs" Version="12.19.1" />

<!-- Testing -->
<PackageReference Include="xunit" Version="2.6.6" />
<PackageReference Include="Moq" Version="4.20.70" />
<PackageReference Include="FluentAssertions" Version="6.12.0" />
<PackageReference Include="Microsoft.AspNetCore.Mvc.Testing" Version="9.0.0" />
```

---

## ?? Support & Resources

### NPHIES Documentation
- **Official Portal:** https://nphies.sa/
- **Implementation Guide:** https://portal.nphies.sa/ig/index.html
- **Sandbox Environment:** https://sandbox.nphies.sa/
- **Technical Support:** support@nphies.sa

### HL7 FHIR Resources
- **FHIR R4 Specification:** https://hl7.org/fhir/R4/
- **Firely .NET SDK:** https://fire.ly/products/firely-net-sdk/
- **FHIR Server:** https://github.com/microsoft/fhir-server

### Saudi Arabia Healthcare
- **CCHI (Council of Cooperative Health Insurance):** https://cchi.gov.sa/
- **MOH (Ministry of Health):** https://www.moh.gov.sa/

---

## ?? Success Criteria

### Phase 1 Complete When:
- [ ] FHIR bundles generate successfully
- [ ] Bundles pass FHIR validator
- [ ] All unit tests pass (80%+ coverage)

### Phase 2 Complete When:
- [ ] Successfully submit eligibility request to sandbox
- [ ] Receive and parse eligibility response
- [ ] Successfully submit claim to sandbox
- [ ] Polling workflow retrieves response

### Phase 3 Complete When:
- [ ] Encounters created and linked to claims
- [ ] Prescriptions managed
- [ ] Clinical data complete in bundles

### Phase 4 Complete When:
- [ ] All NPHIES extensions implemented
- [ ] Profile validation passes
- [ ] 100% NPHIES IG compliance

### Production Ready When:
- [ ] All integration tests pass
- [ ] Security audit complete
- [ ] Performance benchmarks met
- [ ] Live payer integration successful

---

## ?? Timeline Summary

| Phase | Duration | Start Date | End Date | Status |
|-------|----------|------------|----------|--------|
| Phase 1: FHIR Foundation | 3 weeks | TBD | TBD | ? Not Started |
| Phase 2: NPHIES Integration | 3 weeks | TBD | TBD | ? Not Started |
| Phase 3: Clinical Data | 2 weeks | TBD | TBD | ? Not Started |
| Phase 4: Extensions & Compliance | 2 weeks | TBD | TBD | ? Not Started |
| Phase 5: Communication & Attachments | 2 weeks | TBD | TBD | ? Not Started |
| Phase 6: Testing & Production | 4 weeks | TBD | TBD | ? Not Started |
| **Total** | **16 weeks** | **TBD** | **TBD** | ? Not Started |

---

## ? Next Actions

### Immediate (This Week)
1. ? Review this development guide with team
2. ? Set up development environment
3. ? Install Firely.NET SDK
4. ? Create NPHIES sandbox account
5. ? Obtain sandbox credentials

### Week 1 Goals
1. ? Implement `IFhirBundleService` interface
2. ? Create first eligibility bundle
3. ? Serialize to JSON and validate
4. ? Write unit tests for bundle generation

### Month 1 Goals
1. ? Complete Phase 1 (FHIR Foundation)
2. ? Complete Phase 2 (NPHIES Integration)
3. ? Successful sandbox eligibility check
4. ? Successful sandbox claim submission

---

**Document Version:** 1.0  
**Last Updated:** 2024  
**Author:** Development Team  
**Status:** ?? In Progress

---

*This guide should be updated weekly as development progresses. Mark items as complete (?) and update status indicators.*
