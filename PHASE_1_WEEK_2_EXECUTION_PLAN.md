# ?? PHASE 1: WEEK 2 EXECUTION PLAN

**Continue from:** Week 1 completion (Error Codes + Rules)  
**Week 2 Focus:** Appeal Workflow + Payment Reconciliation + Integration  
**Days:** 1-10 (5 working days x 2 weeks)

---

## WEEK 2: DAYS 1-3: Appeal Workflow (18 hours)

### **Task 3.1: Create Appeal Entities** (4 hours)

#### **File 1: `Domain/Entities/Appeals/AppealRequest.cs`**

```csharp
namespace NPhies_FHIR_Integration.Domain.Entities;

/// <summary>
/// Appeal Request - Represents an appeal of a denial
/// FHIR Resource Reference: appeal-related messaging
/// </summary>
public class AppealRequest : BaseEntity
{
    /// <summary>
    /// Original claim ID being appealed
    /// </summary>
    public string ClaimId { get; set; } = string.Empty;

    /// <summary>
    /// Claim Response ID (denial) being appealed
    /// </summary>
    public string ClaimResponseId { get; set; } = string.Empty;

    /// <summary>
    /// Patient ID (person appealing)
    /// </summary>
public string PatientId { get; set; } = string.Empty;

    /// <summary>
    /// Denied item sequence (which line item is appealed)
    /// </summary>
    public int DeniedItemSequence { get; set; }

    /// <summary>
    /// Error code being appealed (e.g., "AD-1-1")
    /// </summary>
    public string DenialErrorCode { get; set; } = string.Empty;

    /// <summary>
  /// Appeal level: "First", "Second", "External"
    /// </summary>
    public string AppealLevel { get; set; } = "First";

    /// <summary>
    /// Deadline for submitting appeal
    /// </summary>
    public DateTime AppealDeadline { get; set; }

    /// <summary>
  /// Date appeal was submitted
    /// </summary>
  public DateTime? SubmittedDate { get; set; }

    /// <summary>
    /// Appeal status: "Draft", "Submitted", "Under Review", "Approved", "Denied", "Withdrawn"
    /// </summary>
    public string Status { get; set; } = "Draft";

    /// <summary>
    /// Reason for appeal (free text)
    /// </summary>
    public string AppealReason { get; set; } = string.Empty;

    /// <summary>
    /// Supporting documents (JSON array of URLs/references)
    /// </summary>
    public string? SupportingDocuments { get; set; }

    /// <summary>
    /// Appeal letter content (generated)
    /// </summary>
    public string? AppealLetterContent { get; set; }

    /// <summary>
    /// Appeal decision when received
    /// </summary>
    public string? AppealDecision { get; set; } // "Approved", "Denied", "Pending"

    /// <summary>
    /// Decision date
    /// </summary>
    public DateTime? DecisionDate { get; set; }

    /// <summary>
/// Decision notes/explanation
    /// </summary>
    public string? DecisionNotes { get; set; }

    /// <summary>
    /// Can this appeal be escalated to next level?
    /// </summary>
    public bool CanEscalate { get; set; }

    /// <summary>
    /// Tracking reference (from payer system)
    /// </summary>
    public string? PayerAppealReference { get; set; }
}
```

#### **File 2: `Domain/Entities/Appeals/AppealTracking.cs`**

```csharp
namespace NPhies_FHIR_Integration.Domain.Entities;

/// <summary>
/// Appeal Tracking - Track appeal status/history
/// </summary>
public class AppealTracking : BaseEntity
{
    /// <summary>
 /// Appeal Request ID this tracking record belongs to
    /// </summary>
    public string AppealRequestId { get; set; } = string.Empty;

    /// <summary>
    /// Appeal Request
/// </summary>
    public AppealRequest? AppealRequest { get; set; }

    /// <summary>
    /// Status at this point: "Created", "Submitted", "Acknowledged", "Under Review", "Decision"
    /// </summary>
    public string TrackingStatus { get; set; } = string.Empty;

    /// <summary>
    /// Event date/time
    /// </summary>
    public DateTime EventDate { get; set; } = DateTime.UtcNow;

    /// <summary>
    /// Notes about this event
    /// </summary>
    public string? Notes { get; set; }

    /// <summary>
    /// User/system that created this event
    /// </summary>
    public string CreatedBy { get; set; } = "SYSTEM";
}
```

#### **File 3: `Domain/Entities/Appeals/AppealDeadlineRule.cs`**

```csharp
namespace NPhies_FHIR_Integration.Domain.Entities;

/// <summary>
/// Appeal Deadline Rules - Define appeal deadlines by error type/scenario
/// </summary>
public class AppealDeadlineRule : BaseEntity
{
    /// <summary>
    /// Error code this rule applies to (e.g., "AD-1-1")
    /// </summary>
    public string ErrorCode { get; set; } = string.Empty;

    /// <summary>
    /// Appeal level: "First", "Second", "External"
    /// </summary>
    public string AppealLevel { get; set; } = string.Empty;

    /// <summary>
    /// Deadline in calendar days (e.g., 60)
    /// </summary>
    public int DeadlineDays { get; set; } = 60;

    /// <summary>
    /// Is this the standard deadline?
    /// </summary>
    public bool IsStandard { get; set; } = true;

    /// <summary>
    /// Special conditions for this deadline
    /// </summary>
    public string? Conditions { get; set; }

    /// <summary>
    /// Is this rule active?
    /// </summary>
    public bool IsActive { get; set; } = true;
}
```

---

### **Task 3.2: Add DbSets and Migrations** (2 hours)

**File to Modify:** `Infrastructure/Data/ApplicationDbContext.cs`

Add:
```csharp
public DbSet<AppealRequest> AppealRequests { get; set; }
public DbSet<AppealTracking> AppealTrackings { get; set; }
public DbSet<AppealDeadlineRule> AppealDeadlineRules { get; set; }
```

Add configuration:
```csharp
// AppealRequest
modelBuilder.Entity<AppealRequest>(entity =>
{
    entity.HasKey(e => e.Id);
    entity.Property(e => e.ClaimId).IsRequired();
    entity.Property(e => e.PatientId).IsRequired();
    entity.HasIndex(e => e.Status);
    entity.HasIndex(e => e.AppealDeadline);
    entity.ToTable("AppealRequests");
});

// AppealTracking
modelBuilder.Entity<AppealTracking>(entity =>
{
  entity.HasKey(e => e.Id);
    entity.HasOne(e => e.AppealRequest)
     .WithMany()
        .HasForeignKey(e => e.AppealRequestId)
        .OnDelete(DeleteBehavior.Cascade);
    entity.ToTable("AppealTrackings");
});

// AppealDeadlineRule
modelBuilder.Entity<AppealDeadlineRule>(entity =>
{
    entity.HasKey(e => e.Id);
  entity.HasIndex(e => e.ErrorCode);
    entity.ToTable("AppealDeadlineRules");
});
```

**Create Migration:**
```bash
dotnet ef migrations add AddAppealEntities --project NPhies_FHIR_Integration.Infrastructure --startup-project NPhies_FHIR_Integration.ApiService
```

---

### **Task 3.3: Create Appeal Service** (12 hours)

**File to Create:** `Application/Services/RCM/IAppealWorkflowService.cs`

```csharp
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using NPhies_FHIR_Integration.Domain.Entities;

namespace NPhies_FHIR_Integration.Application.Services.RCM;

public interface IAppealWorkflowService
{
    /// <summary>
    /// Create appeal for a denied claim item
    /// </summary>
    Task<AppealRequest> CreateAppealAsync(
        string claimId,
        string claimResponseId,
        int itemSequence,
      string errorCode,
   string patientId,
      CancellationToken cancellationToken = default);

    /// <summary>
    /// Get appeal by ID
    /// </summary>
    Task<AppealRequest?> GetAppealAsync(string appealId, CancellationToken cancellationToken = default);

    /// <summary>
    /// Get appeals for a claim
    /// </summary>
    Task<List<AppealRequest>> GetAppealsByClaimAsync(string claimId, CancellationToken cancellationToken = default);

    /// <summary>
    /// Calculate appeal deadline for error code
    /// </summary>
    Task<DateTime> CalculateAppealDeadlineAsync(
        string errorCode,
 string appealLevel = "First",
        CancellationToken cancellationToken = default);

    /// <summary>
    /// Generate appeal letter
    /// </summary>
    Task<string> GenerateAppealLetterAsync(
     AppealRequest appeal,
        Claim originalClaim,
    ClaimResponse response,
      ErrorCodeMaster errorCode,
CancellationToken cancellationToken = default);

    /// <summary>
    /// Submit appeal (change status to submitted)
    /// </summary>
    Task<bool> SubmitAppealAsync(
   string appealId,
        string appealReason,
      string? supportingDocuments = null,
        CancellationToken cancellationToken = default);

    /// <summary>
    /// Update appeal status
    /// </summary>
    Task<bool> UpdateAppealStatusAsync(
        string appealId,
        string status,
        string? notes = null,
  CancellationToken cancellationToken = default);

    /// <summary>
    /// Check if appeal is still within deadline
    /// </summary>
    Task<bool> IsWithinDeadlineAsync(
        string appealId,
        CancellationToken cancellationToken = default);

    /// <summary>
    /// Escalate appeal to next level
    /// </summary>
    Task<AppealRequest?> EscalateAppealAsync(
      string appealId,
        CancellationToken cancellationToken = default);
}
```

**File to Create:** `Application/Services/RCM/AppealWorkflowService.cs`

```csharp
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using NPhies_FHIR_Integration.Application.Services.Masters;
using NPhies_FHIR_Integration.Domain.Entities;
using NPhies_FHIR_Integration.Infrastructure.Data;
using NPhies_FHIR_Integration.Infrastructure.Repositories;

namespace NPhies_FHIR_Integration.Application.Services.RCM;

public class AppealWorkflowService : IAppealWorkflowService
{
    private readonly ApplicationDbContext _context;
    private readonly IErrorCodeService _errorCodeService;
    private readonly ILogger<AppealWorkflowService> _logger;

    public AppealWorkflowService(
        ApplicationDbContext context,
        IErrorCodeService errorCodeService,
        ILogger<AppealWorkflowService> logger)
    {
    _context = context ?? throw new ArgumentNullException(nameof(context));
        _errorCodeService = errorCodeService ?? throw new ArgumentNullException(nameof(errorCodeService));
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
    }

    public async Task<AppealRequest> CreateAppealAsync(
        string claimId,
      string claimResponseId,
        int itemSequence,
        string errorCode,
  string patientId,
        CancellationToken cancellationToken = default)
    {
        _logger.LogInformation("Creating appeal for claim {ClaimId}, error {ErrorCode}", claimId, errorCode);

     // Calculate deadline
        var deadline = await CalculateAppealDeadlineAsync(errorCode, "First", cancellationToken);

        // Check if error code allows appeal
        var canAppeal = await _errorCodeService.AllowsAppealAsync(errorCode);
        if (!canAppeal)
      {
            _logger.LogWarning("Error code {ErrorCode} does not allow appeals", errorCode);
  throw new InvalidOperationException($"Error code {errorCode} does not allow appeals");
    }

      var appeal = new AppealRequest
        {
  ClaimId = claimId,
            ClaimResponseId = claimResponseId,
         PatientId = patientId,
      DeniedItemSequence = itemSequence,
  DenialErrorCode = errorCode,
            AppealLevel = "First",
        AppealDeadline = deadline,
      Status = "Draft"
        };

     _context.AppealRequests.Add(appeal);

        // Create tracking record
     var tracking = new AppealTracking
        {
  AppealRequestId = appeal.Id,
    TrackingStatus = "Created",
 EventDate = DateTime.UtcNow,
 Notes = $"Appeal created for error {errorCode}",
 CreatedBy = "SYSTEM"
        };

        _context.AppealTrackings.Add(tracking);

        await _context.SaveChangesAsync(cancellationToken);

   _logger.LogInformation("Appeal created with ID {AppealId}, deadline {Deadline}", appeal.Id, deadline);

      return appeal;
    }

    public async Task<AppealRequest?> GetAppealAsync(
        string appealId,
        CancellationToken cancellationToken = default)
    {
        return await _context.AppealRequests
            .AsNoTracking()
   .FirstOrDefaultAsync(a => a.Id == appealId, cancellationToken);
    }

    public async Task<List<AppealRequest>> GetAppealsByClaimAsync(
        string claimId,
      CancellationToken cancellationToken = default)
    {
      return await _context.AppealRequests
         .AsNoTracking()
            .Where(a => a.ClaimId == claimId)
        .OrderByDescending(a => a.CreatedAt)
            .ToListAsync(cancellationToken);
    }

    public async Task<DateTime> CalculateAppealDeadlineAsync(
    string errorCode,
        string appealLevel = "First",
        CancellationToken cancellationToken = default)
    {
        // Get deadline rule for this error code
        var rule = await _context.AppealDeadlineRules
.AsNoTracking()
            .FirstOrDefaultAsync(
              r => r.ErrorCode == errorCode && r.AppealLevel == appealLevel && r.IsActive,
        cancellationToken);

        int deadlineDays = rule?.DeadlineDays ?? 60; // Default 60 days

        return DateTime.UtcNow.AddDays(deadlineDays);
    }

    public async Task<string> GenerateAppealLetterAsync(
        AppealRequest appeal,
        Claim originalClaim,
        ClaimResponse response,
        ErrorCodeMaster errorCode,
        CancellationToken cancellationToken = default)
    {
        _logger.LogInformation("Generating appeal letter for appeal {AppealId}", appeal.Id);

        var letter = $@"
APPEAL LETTER

Date: {DateTime.UtcNow:yyyy-MM-dd}
Appeal ID: {appeal.Id}
Claim ID: {appeal.ClaimId}
Patient ID: {appeal.PatientId}

=== APPEAL DETAILS ===

Item Sequence: {appeal.DeniedItemSequence}
Error Code: {appeal.DenialErrorCode}
Error Description: {errorCode?.ErrorDescription}

=== ORIGINAL CLAIM ===

Submitted Amount: ${originalClaim?.Total:F2}
Service Date: {originalClaim?.CreatedAt:yyyy-MM-dd}

=== CLAIM RESPONSE ===

Response Status: {response?.Status}
Response Date: {response?.CreatedAt:yyyy-MM-dd}

=== APPEAL DEADLINE ===

Deadline: {appeal.AppealDeadline:yyyy-MM-dd}
Days Remaining: {(appeal.AppealDeadline - DateTime.UtcNow).Days}

=== INSTRUCTIONS ===

Please submit this appeal with any supporting documentation to the address below.
Include this reference number with your submission: {appeal.PayerAppealReference ?? appeal.Id}

This appeal must be received by the deadline indicated above.

=== APPEAL REASON ===

{appeal.AppealReason}

Submitted by: {appeal.CreatedAt:yyyy-MM-dd}
";

        return await Task.FromResult(letter);
    }

    public async Task<bool> SubmitAppealAsync(
        string appealId,
        string appealReason,
 string? supportingDocuments = null,
        CancellationToken cancellationToken = default)
    {
        _logger.LogInformation("Submitting appeal {AppealId}", appealId);

        var appeal = await GetAppealAsync(appealId, cancellationToken);
   if (appeal == null)
        {
          _logger.LogWarning("Appeal {AppealId} not found", appealId);
  return false;
        }

        // Check deadline
        if (DateTime.UtcNow > appeal.AppealDeadline)
        {
            _logger.LogWarning("Appeal {AppealId} past deadline", appealId);
            throw new InvalidOperationException("Appeal is past deadline");
        }

        appeal.Status = "Submitted";
        appeal.SubmittedDate = DateTime.UtcNow;
     appeal.AppealReason = appealReason;
   appeal.SupportingDocuments = supportingDocuments;

        // Create tracking record
        var tracking = new AppealTracking
        {
            AppealRequestId = appealId,
            TrackingStatus = "Submitted",
EventDate = DateTime.UtcNow,
    Notes = "Appeal submitted by provider",
      CreatedBy = "PROVIDER"
  };

        _context.AppealTrackings.Add(tracking);
        await _context.SaveChangesAsync(cancellationToken);

        _logger.LogInformation("Appeal {AppealId} submitted successfully", appealId);

        return true;
    }

    public async Task<bool> UpdateAppealStatusAsync(
        string appealId,
 string status,
     string? notes = null,
  CancellationToken cancellationToken = default)
    {
        var appeal = await GetAppealAsync(appealId, cancellationToken);
  if (appeal == null)
       return false;

        appeal.Status = status;

        var tracking = new AppealTracking
        {
         AppealRequestId = appealId,
 TrackingStatus = status,
          EventDate = DateTime.UtcNow,
         Notes = notes,
            CreatedBy = "SYSTEM"
};

        _context.AppealTrackings.Add(tracking);
    await _context.SaveChangesAsync(cancellationToken);

      return true;
    }

    public async Task<bool> IsWithinDeadlineAsync(
        string appealId,
   CancellationToken cancellationToken = default)
 {
        var appeal = await GetAppealAsync(appealId, cancellationToken);
        if (appeal == null)
         return false;

        return DateTime.UtcNow <= appeal.AppealDeadline;
    }

    public async Task<AppealRequest?> EscalateAppealAsync(
        string appealId,
  CancellationToken cancellationToken = default)
    {
        var appeal = await GetAppealAsync(appealId, cancellationToken);
     if (appeal == null)
   return null;

  // Determine next appeal level
        string nextLevel = appeal.AppealLevel switch
        {
            "First" => "Second",
     "Second" => "External",
      _ => appeal.AppealLevel
        };

     // Calculate new deadline
  var newDeadline = await CalculateAppealDeadlineAsync(
            appeal.DenialErrorCode,
      nextLevel,
   cancellationToken);

        // Create new appeal at next level
     var escalatedAppeal = new AppealRequest
        {
  ClaimId = appeal.ClaimId,
            ClaimResponseId = appeal.ClaimResponseId,
   PatientId = appeal.PatientId,
            DeniedItemSequence = appeal.DeniedItemSequence,
        DenialErrorCode = appeal.DenialErrorCode,
          AppealLevel = nextLevel,
 AppealDeadline = newDeadline,
 Status = "Draft",
  AppealReason = appeal.AppealReason
        };

        _context.AppealRequests.Add(escalatedAppeal);
        await _context.SaveChangesAsync(cancellationToken);

        _logger.LogInformation("Appeal escalated from {FromLevel} to {ToLevel}", 
      appeal.AppealLevel, nextLevel);

        return escalatedAppeal;
    }
}
```

**Register in Program.cs:**
```csharp
builder.Services.AddScoped<IAppealWorkflowService, AppealWorkflowService>();
```

---

### **DAYS 4-5: Payment Reconciliation + Integration (12 hours)**

#### **Task 4.1: Enhance ClaimResponseProcessingService** (6 hours)

Modify existing: `Application/Services/RCM/ClaimResponseProcessingService.cs`

Add integration with error codes:

```csharp
// In ProcessClaimResponseAsync method, add:
if (!string.IsNullOrEmpty(denialReasonCode))
{
    var errorCode = await _errorCodeService.GetErrorCodeAsync(denialReasonCode);
    if (errorCode != null)
    {
        // Create appeal if recoverable
      if (errorCode.IsRecoverable && errorCode.AllowsAppeal)
        {
            var appeal = await _appealService.CreateAppealAsync(
    response.ClaimId,
    response.Id,
    item.Sequence,
             denialReasonCode,
        originalClaim.PatientId,
        cancellationToken);
          
            _logger.LogInformation("Appeal created for denial {ErrorCode}: {AppealId}",
                denialReasonCode, appeal.Id);
    }
 }
}
```

#### **Task 4.2: Testing** (6 hours)

Create test cases:
- Error code lookup performance
- Rule execution sequencing
- Appeal creation and deadline calculation
- End-to-end claim processing with rules

---

## ? COMPLETION CHECKLIST

**By End of Week 2:**

- [ ] Error codes table seeded (1,682 codes - Phase 1: 100)
- [ ] ErrorCodeService operational
- [ ] Adjudication rules engine working (10+ rules)
- [ ] Appeal entities created and migrated
- [ ] AppealWorkflowService implemented
- [ ] Appeal creation, submission, escalation working
- [ ] Integration between services complete
- [ ] All unit tests passing
- [ ] Integration tests passing
- [ ] Documentation updated

---

## ?? PROGRESS TRACKING

```
Week 1: Error Codes + Basic Rules
?? Days 1-2: ErrorCodeMaster entity & DB ?
?? Days 3-5: Top 10 adjudication rules ?
?? Status: ?????????? 80%

Week 2: Appeal Workflow + Integration
?? Days 1-3: Appeal entities & service ?
?? Days 4-5: Payment integration & testing ?
?? Status: ?????????? 0% (starting)

TOTAL PHASE 1: 70 hours, 2 weeks
```

---

## ?? FINAL DELIVERABLES

? **Error Code System:**
- 1,682 NPHIES codes in database
- Fast lookup API
- Code categorization

? **Adjudication Rules:**
- 10+ core rules implemented
- Priority-based execution
- Extensible framework

? **Appeal Workflow:**
- Complete appeal lifecycle
- Deadline management
- Escalation support

? **Production Ready:**
- 85%+ NPHIES compliance
- Full test coverage
- Ready for go-live

---

**Timeline:** Start Week 1 Monday ? Complete Friday of Week 2  
**Team:** 2-3 developers (40-80 hours each)  
**Result:** Production-ready NPHIES RCM API

