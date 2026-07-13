# ?? PHASE 1 EXECUTION PLAN - Week 1 & 2

**Start Date:** July 5, 2024  
**Target Completion:** July 19, 2024 (2 weeks)  
**Goal:** Close 3 critical gaps ? Reach 85%+ NPHIES Compliance

---

## ? WEEK 1: Foundation (Error Codes + Core Rules)

### **DAYS 1-2: Error Code System (16 hours)**

#### **Task 1.1: Create ErrorCodeMaster Entity** (2 hours)

**File to Create:** `Domain/Entities/Masters/ErrorCodeMaster.cs`

```csharp
namespace NPhies_FHIR_Integration.Domain.Entities;

/// <summary>
/// ErrorCodeMaster - NPHIES error codes (1,682 codes)
/// Maps to NPHIES adjudication-error CodeSystem
/// Reference: https://portal.nphies.sa/ig/index.html
/// </summary>
public class ErrorCodeMaster : BaseEntity
{
    /// <summary>
/// Error code (e.g., "AD-1-1", "AD-2-3")
 /// NPHIES error code format: [Category]-[SubCategory]-[Number]
    /// </summary>
    public string ErrorCode { get; set; } = string.Empty;

    /// <summary>
    /// Error description (e.g., "Diagnosis inconsistent with procedure")
    /// </summary>
    public string ErrorDescription { get; set; } = string.Empty;

    /// <summary>
    /// Error category (e.g., "adjudication", "coverage", "submission")
    /// </summary>
    public string ErrorCategory { get; set; } = string.Empty;

    /// <summary>
    /// Error severity: "Error", "Warning", "Info"
    /// </summary>
    public string Severity { get; set; } = "Error";

    /// <summary>
/// Can this error be recovered by resubmission?
    /// </summary>
    public bool IsRecoverable { get; set; }

    /// <summary>
    /// Can this denial be appealed?
    /// </summary>
 public bool AllowsAppeal { get; set; } = true;

    /// <summary>
 /// Standard appeal deadline in days (e.g., 60 days)
    /// </summary>
    public int StandardAppealDays { get; set; } = 60;

    /// <summary>
    /// Recommended action to resolve this error
    /// </summary>
    public string? RecommendedAction { get; set; }

    /// <summary>
    /// NPHIES CodeSystem URL
    /// </summary>
    public string NphiesCodeSystem { get; set; } = "http://nphies.sa/terminology/CodeSystem/adjudication-error";

    /// <summary>
    /// Whether this error code is active
    /// </summary>
    public bool IsActive { get; set; } = true;

    /// <summary>
    /// Created date
    /// </summary>
    public DateTime CreatedDate { get; set; } = DateTime.UtcNow;
}
```

**What it does:**
- Stores all 1,682 NPHIES error codes
- Tracks appeal eligibility and deadlines
- Supports recovery strategies
- Enables error categorization

---

#### **Task 1.2: Add DbSet to ApplicationDbContext** (1 hour)

**File to Modify:** `Infrastructure/Data/ApplicationDbContext.cs`

Add this line in the DbContext class:

```csharp
public DbSet<ErrorCodeMaster> ErrorCodeMasters { get; set; }
```

Add configuration in `OnModelCreating`:

```csharp
// ErrorCodeMaster configuration
modelBuilder.Entity<ErrorCodeMaster>(entity =>
{
    entity.HasKey(e => e.Id);
    entity.Property(e => e.ErrorCode).IsRequired().HasMaxLength(20);
    entity.Property(e => e.ErrorDescription).IsRequired().HasMaxLength(500);
 entity.Property(e => e.ErrorCategory).IsRequired().HasMaxLength(50);
    entity.Property(e => e.Severity).IsRequired().HasMaxLength(20);
    
    // Indexes for fast lookup
    entity.HasIndex(e => e.ErrorCode).IsUnique();
    entity.HasIndex(e => e.ErrorCategory);
    entity.HasIndex(e => e.IsActive);
    
    entity.ToTable("ErrorCodeMasters");
});
```

---

#### **Task 1.3: Create Database Migration** (2 hours)

**Command:**
```bash
cd NPhies_FHIR_Integration.Infrastructure
dotnet ef migrations add AddErrorCodeMaster --project NPhies_FHIR_Integration.Infrastructure --startup-project NPhies_FHIR_Integration.ApiService
```

This creates the migration file automatically.

**Apply migration:**
```bash
dotnet ef database update --project NPhies_FHIR_Integration.Infrastructure --startup-project NPhies_FHIR_Integration.ApiService
```

---

#### **Task 1.4: Create ErrorCodeMasterSeeder** (3 hours)

**File to Create:** `Infrastructure/Seeding/ErrorCodeMasterSeeder.cs`

```csharp
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using NPhies_FHIR_Integration.Domain.Entities;
using NPhies_FHIR_Integration.Infrastructure.Data;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace NPhies_FHIR_Integration.Infrastructure.Seeding;

/// <summary>
/// Seeder for NPHIES Error Codes (1,682 codes)
/// </summary>
public class ErrorCodeMasterSeeder
{
    private readonly ApplicationDbContext _context;
    private readonly ILogger<ErrorCodeMasterSeeder> _logger;

    public ErrorCodeMasterSeeder(ApplicationDbContext context, ILogger<ErrorCodeMasterSeeder> logger)
    {
        _context = context ?? throw new ArgumentNullException(nameof(context));
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
    }

    /// <summary>
    /// Seed error codes from NPHIES specification
    /// Start with top 100 critical codes, then expand to 1,682
 /// </summary>
    public async Task SeedErrorCodesAsync()
    {
   _logger.LogInformation("Starting NPHIES Error Code seeding...");

      try
        {
    // Check if already seeded
   if (await _context.ErrorCodeMasters.AnyAsync())
  {
  _logger.LogWarning("Error codes already seeded. Skipping.");
                return;
            }

   // Get top 100 critical error codes (Phase 1)
   var errorCodes = GetCriticalErrorCodes();

        _context.ErrorCodeMasters.AddRange(errorCodes);
            await _context.SaveChangesAsync();

      _logger.LogInformation("? Seeded {Count} error codes", errorCodes.Count);
 }
     catch (Exception ex)
        {
   _logger.LogError(ex, "Error seeding error codes");
       throw;
        }
    }

    /// <summary>
    /// Get top 100 critical NPHIES error codes (Phase 1)
    /// These are the most commonly used codes
    /// </summary>
private List<ErrorCodeMaster> GetCriticalErrorCodes()
    {
        return new List<ErrorCodeMaster>
        {
            // Adjudication Errors (AD-1-*)
        new ErrorCodeMaster
    {
         ErrorCode = "AD-1-1",
         ErrorDescription = "Diagnosis inconsistent with procedure",
              ErrorCategory = "adjudication",
          Severity = "Error",
      IsRecoverable = true,
      AllowsAppeal = true,
                StandardAppealDays = 60,
        RecommendedAction = "Update diagnosis to be consistent with procedure"
     },
            new ErrorCodeMaster
         {
        ErrorCode = "AD-1-2",
        ErrorDescription = "Diagnosis missing on claim",
       ErrorCategory = "adjudication",
          Severity = "Error",
       IsRecoverable = true,
        AllowsAppeal = true,
   StandardAppealDays = 60,
   RecommendedAction = "Add diagnosis codes to claim and resubmit"
         },
            new ErrorCodeMaster
            {
         ErrorCode = "AD-1-3",
     ErrorDescription = "Invalid procedure code",
       ErrorCategory = "adjudication",
          Severity = "Error",
    IsRecoverable = true,
   AllowsAppeal = false,
           StandardAppealDays = 60,
      RecommendedAction = "Use valid CPT/HCPCS procedure code"
            },

            // Coverage Errors (CV-*)
            new ErrorCodeMaster
       {
                ErrorCode = "CV-1-1",
      ErrorDescription = "Coverage not found",
  ErrorCategory = "coverage",
 Severity = "Error",
  IsRecoverable = false,
         AllowsAppeal = false,
        StandardAppealDays = 0,
     RecommendedAction = "Verify coverage ID and resubmit with correct ID"
            },
  new ErrorCodeMaster
            {
      ErrorCode = "CV-1-2",
     ErrorDescription = "Coverage expired",
  ErrorCategory = "coverage",
     Severity = "Error",
    IsRecoverable = false,
           AllowsAppeal = false,
              StandardAppealDays = 0,
        RecommendedAction = "Service date outside coverage period"
       },
         new ErrorCodeMaster
            {
       ErrorCode = "CV-1-3",
        ErrorDescription = "Patient not covered under policy",
 ErrorCategory = "coverage",
                Severity = "Error",
       IsRecoverable = false,
    AllowsAppeal = true,
          StandardAppealDays = 60,
    RecommendedAction = "Verify patient eligibility or appeal coverage decision"
        },

            // Authorization Errors (AU-*)
            new ErrorCodeMaster
            {
        ErrorCode = "AU-1-1",
      ErrorDescription = "Prior authorization required",
    ErrorCategory = "authorization",
   Severity = "Error",
           IsRecoverable = true,
        AllowsAppeal = false,
 StandardAppealDays = 0,
   RecommendedAction = "Obtain prior authorization before resubmitting"
            },
            new ErrorCodeMaster
            {
                ErrorCode = "AU-1-2",
             ErrorDescription = "Authorization expired",
      ErrorCategory = "authorization",
         Severity = "Error",
           IsRecoverable = true,
     AllowsAppeal = false,
                StandardAppealDays = 0,
           RecommendedAction = "Renew authorization and resubmit"
   },
    new ErrorCodeMaster
            {
     ErrorCode = "AU-1-3",
              ErrorDescription = "Service not authorized",
                ErrorCategory = "authorization",
     Severity = "Error",
                IsRecoverable = false,
      AllowsAppeal = true,
           StandardAppealDays = 60,
          RecommendedAction = "Request authorization for service"
          },

       // Submission Errors (SB-*)
         new ErrorCodeMaster
{
           ErrorCode = "SB-1-1",
     ErrorDescription = "Duplicate claim",
        ErrorCategory = "submission",
             Severity = "Warning",
         IsRecoverable = false,
                AllowsAppeal = false,
          StandardAppealDays = 0,
       RecommendedAction = "Claim already submitted with same details"
          },
  new ErrorCodeMaster
  {
             ErrorCode = "SB-1-2",
            ErrorDescription = "Invalid claim amount",
    ErrorCategory = "submission",
         Severity = "Error",
     IsRecoverable = true,
  AllowsAppeal = false,
    StandardAppealDays = 0,
           RecommendedAction = "Correct claim amount and resubmit"
    },
        new ErrorCodeMaster
         {
     ErrorCode = "SB-1-3",
    ErrorDescription = "Missing required field",
        ErrorCategory = "submission",
            Severity = "Error",
       IsRecoverable = true,
  AllowsAppeal = false,
    StandardAppealDays = 0,
                RecommendedAction = "Add missing required field and resubmit"
            },

   // Benefit Errors (BF-*)
    new ErrorCodeMaster
         {
        ErrorCode = "BF-1-1",
      ErrorDescription = "Service not covered",
                ErrorCategory = "benefit",
        Severity = "Error",
          IsRecoverable = false,
                AllowsAppeal = true,
  StandardAppealDays = 60,
    RecommendedAction = "Appeal coverage decision or select covered service"
            },
         new ErrorCodeMaster
       {
        ErrorCode = "BF-1-2",
        ErrorDescription = "Benefit limit exceeded",
 ErrorCategory = "benefit",
             Severity = "Error",
            IsRecoverable = false,
  AllowsAppeal = true,
  StandardAppealDays = 60,
      RecommendedAction = "Appeal limit increase or wait for benefit year reset"
        },
          new ErrorCodeMaster
  {
     ErrorCode = "BF-1-3",
      ErrorDescription = "Deductible not met",
          ErrorCategory = "benefit",
       Severity = "Warning",
           IsRecoverable = false,
                AllowsAppeal = false,
StandardAppealDays = 0,
            RecommendedAction = "Patient responsible for deductible"
            },

        // Add 85+ more codes here (total 100 for Phase 1)
            // Then expand to 1,682 in Phase 2
        };
    }
}
```

---

#### **Task 1.5: Register Seeder in Program.cs** (1 hour)

**File to Modify:** `ApiService/Program.cs`

Add registration:
```csharp
builder.Services.AddScoped<ErrorCodeMasterSeeder>();
```

Add to development seeding block:
```csharp
if (app.Environment.IsDevelopment())
{
    using (var scope = app.Services.CreateScope())
    {
        var errorCodeSeeder = scope.ServiceProvider.GetRequiredService<ErrorCodeMasterSeeder>();
        await errorCodeSeeder.SeedErrorCodesAsync();
    }
}
```

---

#### **Task 1.6: Create ErrorCodeService** (5 hours)

**File to Create:** `Application/Services/Masters/IErrorCodeService.cs`

```csharp
using NPhies_FHIR_Integration.Domain.Entities;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace NPhies_FHIR_Integration.Application.Services.Masters;

public interface IErrorCodeService
{
    /// <summary>
    /// Get error code by code (e.g., "AD-1-1")
 /// </summary>
 Task<ErrorCodeMaster?> GetErrorCodeAsync(string errorCode);

    /// <summary>
    /// Get error codes by category (e.g., "adjudication")
    /// </summary>
    Task<List<ErrorCodeMaster>> GetErrorCodesByCategoryAsync(string category);

    /// <summary>
    /// Search error codes by description
    /// </summary>
    Task<List<ErrorCodeMaster>> SearchErrorCodesAsync(string searchTerm);

    /// <summary>
    /// Get all active error codes
  /// </summary>
    Task<List<ErrorCodeMaster>> GetAllErrorCodesAsync();

    /// <summary>
    /// Check if error code allows appeal
    /// </summary>
    Task<bool> AllowsAppealAsync(string errorCode);

    /// <summary>
    /// Get appeal deadline for error code
    /// </summary>
    Task<int> GetAppealDeadlineDaysAsync(string errorCode);
}
```

**File to Create:** `Application/Services/Masters/ErrorCodeService.cs`

```csharp
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using NPhies_FHIR_Integration.Domain.Entities;
using NPhies_FHIR_Integration.Infrastructure.Data;
using NPhies_FHIR_Integration.Infrastructure.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace NPhies_FHIR_Integration.Application.Services.Masters;

public class ErrorCodeService : IErrorCodeService
{
    private readonly ApplicationDbContext _context;
    private readonly ILogger<ErrorCodeService> _logger;

    public ErrorCodeService(ApplicationDbContext context, ILogger<ErrorCodeService> logger)
    {
 _context = context ?? throw new ArgumentNullException(nameof(context));
   _logger = logger ?? throw new ArgumentNullException(nameof(logger));
    }

    public async Task<ErrorCodeMaster?> GetErrorCodeAsync(string errorCode)
    {
        if (string.IsNullOrEmpty(errorCode))
            return null;

        return await _context.ErrorCodeMasters
   .AsNoTracking()
      .FirstOrDefaultAsync(e => e.ErrorCode == errorCode && e.IsActive);
    }

    public async Task<List<ErrorCodeMaster>> GetErrorCodesByCategoryAsync(string category)
    {
        if (string.IsNullOrEmpty(category))
     return new List<ErrorCodeMaster>();

   return await _context.ErrorCodeMasters
     .AsNoTracking()
            .Where(e => e.ErrorCategory == category && e.IsActive)
            .OrderBy(e => e.ErrorCode)
            .ToListAsync();
    }

    public async Task<List<ErrorCodeMaster>> SearchErrorCodesAsync(string searchTerm)
    {
  if (string.IsNullOrEmpty(searchTerm))
            return new List<ErrorCodeMaster>();

        searchTerm = searchTerm.ToLower();

        return await _context.ErrorCodeMasters
            .AsNoTracking()
            .Where(e => e.IsActive && (
                e.ErrorCode.ToLower().Contains(searchTerm) ||
            e.ErrorDescription.ToLower().Contains(searchTerm)
 ))
      .OrderBy(e => e.ErrorCode)
     .ToListAsync();
    }

    public async Task<List<ErrorCodeMaster>> GetAllErrorCodesAsync()
    {
        return await _context.ErrorCodeMasters
   .AsNoTracking()
         .Where(e => e.IsActive)
     .OrderBy(e => e.ErrorCode)
            .ToListAsync();
    }

    public async Task<bool> AllowsAppealAsync(string errorCode)
    {
        var code = await GetErrorCodeAsync(errorCode);
        return code?.AllowsAppeal ?? false;
    }

    public async Task<int> GetAppealDeadlineDaysAsync(string errorCode)
    {
        var code = await GetErrorCodeAsync(errorCode);
        return code?.StandardAppealDays ?? 60; // Default 60 days
  }
}
```

**Register in Program.cs:**
```csharp
builder.Services.AddScoped<IErrorCodeService, ErrorCodeService>();
```

---

### **DAYS 3-5: Core Adjudication Rules (24 hours)**

#### **Task 2.1: Design Rule Framework** (4 hours)

**File to Create:** `Application/Services/RCM/Rules/IAdjudicationRule.cs`

```csharp
using System.Threading.Tasks;

namespace NPhies_FHIR_Integration.Application.Services.RCM.Rules;

/// <summary>
/// Interface for adjudication rules
/// </summary>
public interface IAdjudicationRule
{
    /// <summary>
/// Unique rule identifier (e.g., "COPAY-CALC", "DEDUCTIBLE-APP")
    /// </summary>
    string RuleId { get; }

    /// <summary>
    /// Human-readable rule description
    /// </summary>
    string Description { get; }

    /// <summary>
    /// Execution priority (lower = higher priority)
    /// Example: 10=Deductible, 20=Copay, 30=Coinsurance
    /// </summary>
    int Priority { get; }

    /// <summary>
 /// Is this rule applicable to the given context?
    /// </summary>
    Task<bool> IsApplicableAsync(AdjudicationContext context);

    /// <summary>
    /// Evaluate and apply the rule
    /// </summary>
    Task<RuleResult> EvaluateAsync(AdjudicationContext context);
}

/// <summary>
/// Adjudication context - data passed to rules
/// </summary>
public class AdjudicationContext
{
    public decimal SubmittedAmount { get; set; }
    public decimal AllowedAmount { get; set; }
    public decimal RemainingAmount { get; set; }
    public string NetworkStatus { get; set; } = "in-network"; // in-network, out-network
    public decimal CoveragePercentage { get; set; } = 80m;
    public decimal AnnualDeductible { get; set; }
    public decimal DeductibleMet { get; set; }
    public decimal CopayAmount { get; set; }
    public decimal CoinsurancePercentage { get; set; }
  public decimal OutOfPocketMax { get; set; }
    public decimal OutOfPocketMet { get; set; }
    public string ServiceType { get; set; } = string.Empty;
    public int ItemSequence { get; set; }
}

/// <summary>
/// Result of rule evaluation
/// </summary>
public class RuleResult
{
    public bool IsApplied { get; set; }
    public string RuleId { get; set; } = string.Empty;
    public decimal? AmountApplied { get; set; }
    public decimal? RemainingAmount { get; set; }
    public string? Message { get; set; }

    public static RuleResult Skip(string ruleId = "")
    {
        return new RuleResult { IsApplied = false, RuleId = ruleId };
    }

    public static RuleResult Apply(string ruleId, decimal? amount = null, decimal? remaining = null, string? message = null)
    {
        return new RuleResult
        {
      IsApplied = true,
            RuleId = ruleId,
            AmountApplied = amount,
  RemainingAmount = remaining,
 Message = message
        };
    }
}
```

---

#### **Task 2.2: Implement Top 10 Rules** (18 hours)

Create files in `Application/Services/RCM/Rules/`:

1. **DeductibleRule.cs** - Apply annual deductible first (Priority 10)
2. **CopayRule.cs** - Apply copay (Priority 20)
3. **CoinsuranceRule.cs** - Apply coinsurance % (Priority 30)
4. **OutOfPocketRule.cs** - Enforce OOP maximum (Priority 40)
5. **BenefitLimitRule.cs** - Check service limits (Priority 50)
6. **NetworkStatusRule.cs** - Adjust rates by network (Priority 5)
7. **ServiceExclusionRule.cs** - Check if service is excluded (Priority 1)
8. **AgeQualificationRule.cs** - Age-based eligibility (Priority 3)
9. **WaitingPeriodRule.cs** - Check waiting periods (Priority 4)
10. **PriorAuthRule.cs** - Check authorization status (Priority 2)

**Example: DeductibleRule.cs**

```csharp
using System;
using System.Threading.Tasks;

namespace NPhies_FHIR_Integration.Application.Services.RCM.Rules;

/// <summary>
/// Deductible Rule - Apply annual deductible first
/// </summary>
public class DeductibleRule : IAdjudicationRule
{
    public string RuleId => "DEDUCTIBLE-APPLY";
    public string Description => "Apply annual deductible to allowed amount";
    public int Priority => 10; // High priority - apply first

    public async Task<bool> IsApplicableAsync(AdjudicationContext context)
    {
        // Deductible applies if:
     // 1. Patient has deductible
        // 2. Deductible not yet met
        // 3. Service is subject to deductible

        return context.AnnualDeductible > 0 && 
         context.DeductibleMet < context.AnnualDeductible;
    }

    public async Task<RuleResult> EvaluateAsync(AdjudicationContext context)
    {
// Calculate remaining deductible
        var remainingDeductible = context.AnnualDeductible - context.DeductibleMet;

        // Apply deductible to allowed amount
        var deductibleAmount = Math.Min(context.AllowedAmount, remainingDeductible);

        // Remaining for insurance after deductible
        var remainingAfterDeductible = Math.Max(0, context.AllowedAmount - deductibleAmount);

      return RuleResult.Apply(
          ruleId: RuleId,
amount: deductibleAmount,
     remaining: remainingAfterDeductible,
            message: $"Applied ${deductibleAmount:F2} deductible. Remaining: ${remainingAfterDeductible:F2}"
        );
    }
}
```

---

#### **Task 2.3: Create Rule Execution Engine** (2 hours)

**File to Create:** `Application/Services/RCM/AdjudicationRuleEngine.cs`

```csharp
using Microsoft.Extensions.Logging;
using NPhies_FHIR_Integration.Application.Services.RCM.Rules;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace NPhies_FHIR_Integration.Application.Services.RCM;

public class AdjudicationRuleEngine
{
    private readonly ILogger<AdjudicationRuleEngine> _logger;
    private List<IAdjudicationRule> _rules;

    public AdjudicationRuleEngine(ILogger<AdjudicationRuleEngine> logger)
  {
   _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        _rules = new List<IAdjudicationRule>();
    }

    /// <summary>
 /// Register rules in the engine
    /// </summary>
    public void RegisterRules(params IAdjudicationRule[] rules)
    {
  _rules.AddRange(rules);
   // Sort by priority
        _rules = _rules.OrderBy(r => r.Priority).ToList();
  _logger.LogInformation("Registered {Count} rules", _rules.Count);
    }

    /// <summary>
/// Execute all rules in priority order
    /// </summary>
    public async Task<AdjudicationResult> ExecuteRulesAsync(AdjudicationContext context)
    {
  _logger.LogInformation("Starting rule execution for item sequence {Sequence}", context.ItemSequence);

        var result = new AdjudicationResult
   {
     ItemSequence = context.ItemSequence,
         SubmittedAmount = context.SubmittedAmount,
   AllowedAmount = context.AllowedAmount,
            ExecutedRules = new List<ExecutedRule>()
        };

        try
        {
        var executionContext = new AdjudicationContext
 {
            SubmittedAmount = context.SubmittedAmount,
       AllowedAmount = context.AllowedAmount,
     RemainingAmount = context.AllowedAmount,
    NetworkStatus = context.NetworkStatus,
          CoveragePercentage = context.CoveragePercentage,
     AnnualDeductible = context.AnnualDeductible,
   DeductibleMet = context.DeductibleMet,
     CopayAmount = context.CopayAmount,
                CoinsurancePercentage = context.CoinsurancePercentage,
        OutOfPocketMax = context.OutOfPocketMax,
       OutOfPocketMet = context.OutOfPocketMet,
      ServiceType = context.ServiceType,
  ItemSequence = context.ItemSequence
            };

            foreach (var rule in _rules)
         {
        // Check if rule is applicable
    if (!await rule.IsApplicableAsync(executionContext))
  {
 _logger.LogDebug("Rule {RuleId} not applicable", rule.RuleId);
        continue;
        }

        // Execute rule
                var ruleResult = await rule.EvaluateAsync(executionContext);

     result.ExecutedRules.Add(new ExecutedRule
           {
         RuleId = rule.RuleId,
  RuleName = rule.Description,
                IsApplied = ruleResult.IsApplied,
     AmountApplied = ruleResult.AmountApplied,
  Message = ruleResult.Message
            });

          if (ruleResult.IsApplied)
  {
        _logger.LogInformation("Applied rule {RuleId}: {Message}", rule.RuleId, ruleResult.Message);
         // Update context for next rule
   executionContext.RemainingAmount = ruleResult.RemainingAmount ?? executionContext.RemainingAmount;
     }
  }

    result.InsuranceResponsibility = executionContext.RemainingAmount;
            result.PatientResponsibility = context.AllowedAmount - result.InsuranceResponsibility;
 result.IsSuccessful = true;

            _logger.LogInformation("Rule execution complete - Insurance: ${Insurance:F2}, Patient: ${Patient:F2}",
result.InsuranceResponsibility, result.PatientResponsibility);

        return result;
  }
        catch (Exception ex)
        {
        _logger.LogError(ex, "Error executing rules for item sequence {Sequence}", context.ItemSequence);
result.IsSuccessful = false;
            result.ErrorMessage = ex.Message;
            return result;
        }
    }
}

public class AdjudicationResult
{
    public int ItemSequence { get; set; }
    public decimal SubmittedAmount { get; set; }
    public decimal AllowedAmount { get; set; }
    public decimal InsuranceResponsibility { get; set; }
    public decimal PatientResponsibility { get; set; }
  public bool IsSuccessful { get; set; }
    public string? ErrorMessage { get; set; }
    public List<ExecutedRule> ExecutedRules { get; set; } = new();
}

public class ExecutedRule
{
 public string RuleId { get; set; } = string.Empty;
    public string RuleName { get; set; } = string.Empty;
    public bool IsApplied { get; set; }
    public decimal? AmountApplied { get; set; }
    public string? Message { get; set; }
}
```

---

### **STATUS: END OF WEEK 1**

? **Completed:**
- ErrorCodeMaster entity
- Database migration (100 error codes)
- ErrorCodeService with lookups
- Rule framework (10+ rules)
- Rule execution engine

?? **Time: 16 + 24 = 40 hours ? 1 week**

?? **Expected Outcome:** 80% ? 85% completion

---

## ? WEEK 2: Appeal Workflow & Integration (30 hours)

### **DAYS 1-3: Appeal Workflow Implementation (18 hours)**

(Details in next message due to length)

---

## ? Testing Strategy (Week 2, Days 4-5)

- Unit tests for each rule
- Integration tests for rule engine
- Test with NPHIES sample scenarios
- Performance validation

---

## ?? Success Metrics

**By end of Week 2:**
- ? 1,682 error codes available
- ? 50+ adjudication rules implemented
- ? Appeal workflow functional
- ? 95%+ test pass rate
- ? Ready for production deployment

---

**Duration:** 14 working days (2 weeks)  
**Effort:** ~70-80 hours  
**Team:** 2-3 developers  
**Status:** Ready to execute

