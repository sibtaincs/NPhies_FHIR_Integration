# ?? WEEK 1 DAYS 3-5: ADJUDICATION RULES IMPLEMENTATION PLAN

**Duration:** 3 days (Days 3, 4, 5 of Week 1)  
**Focus:** Implement 10+ adjudication rules + execution engine  
**Estimated Effort:** 24 hours (8 hours per day)  
**Status:** Ready to start immediately

---

## ?? OVERVIEW

### **What We're Building**

1. **Rule Framework** (Day 3 - 3 hours)
   - `IAdjudicationRule` interface
   - `AdjudicationContext` class
   - `RuleResult` class
   - `AdjudicationRuleEngine` implementation

2. **10+ Adjudication Rules** (Days 3-5 - 15 hours)
   - Each rule is a class implementing IAdjudicationRule
   - Includes business logic for calculations
   - Proper logging and error handling

3. **Integration** (Days 4-5 - 6 hours)
   - Register rules in DI
   - Integrate with ErrorCodeService
   - Create rule tests

---

## ??? DAY 3: RULE FRAMEWORK (3 hours)

### **Task 3.1: Create IAdjudicationRule Interface** (1 hour)

**File:** `Application/Services/RCM/Rules/IAdjudicationRule.cs`

```csharp
namespace NPhies_FHIR_Integration.Application.Services.RCM.Rules;

/// <summary>
/// Base interface for adjudication rules
/// Each rule applies specific business logic to claim adjudication
/// </summary>
public interface IAdjudicationRule
{
    /// <summary>
    /// Unique rule identifier (e.g., "DEDUCTIBLE", "COPAY", "COINSURANCE")
    /// </summary>
    string RuleId { get; }

    /// <summary>
    /// Human-readable rule name
    /// </summary>
    string RuleName { get; }

    /// <summary>
    /// Execution priority (lower = higher priority)
    /// Example: Deductible(10) ? Copay(20) ? Coinsurance(30) ? OOP(40)
    /// </summary>
    int Priority { get; }

    /// <summary>
    /// Check if this rule applies to the given context
    /// </summary>
    Task<bool> IsApplicableAsync(AdjudicationContext context);

    /// <summary>
    /// Evaluate and apply the rule
    /// </summary>
    Task<RuleResult> EvaluateAsync(AdjudicationContext context);
}

/// <summary>
/// Context passed to all adjudication rules
/// Contains all claim and coverage information needed for adjudication
/// </summary>
public class AdjudicationContext
{
// Claim Item Information
 public int ItemSequence { get; set; }
    public string ServiceCode { get; set; } = string.Empty;
    public decimal SubmittedAmount { get; set; }
    public decimal AllowedAmount { get; set; }
    public decimal RemainingAmount { get; set; }
    
// Coverage Information
    public string NetworkStatus { get; set; } = "in-network"; // in-network, out-network
    public decimal CoveragePercentage { get; set; } = 80m;
    public string CoverageType { get; set; } = string.Empty;
    
    // Deductible Information
    public decimal AnnualDeductible { get; set; }
    public decimal DeductibleMet { get; set; }
    public decimal RemainingDeductible => Math.Max(0, AnnualDeductible - DeductibleMet);
    
    // Copay Information
    public decimal CopayAmount { get; set; }
    public bool CopayApplied { get; set; }
    
    // Coinsurance Information
    public decimal CoinsurancePercentage { get; set; } = 20m; // Patient portion
 public bool CoinsuranceApplied { get; set; }
    
    // Out-of-Pocket Information
    public decimal OutOfPocketMax { get; set; }
    public decimal OutOfPocketMet { get; set; }
    public decimal RemainingOOP => Math.Max(0, OutOfPocketMax - OutOfPocketMet);
    
    // Benefit Limits
    public decimal AnnualBenefitLimit { get; set; }
    public decimal BenefitUsed { get; set; }
    public decimal RemainingBenefit => Math.Max(0, AnnualBenefitLimit - BenefitUsed);
    
 // Service Type
    public string ServiceType { get; set; } = string.Empty; // inpatient, outpatient, etc.
    public string DiagnosisCode { get; set; } = string.Empty;
}

/// <summary>
/// Result of a rule evaluation
/// </summary>
public class RuleResult
{
    /// <summary>
    /// Was the rule applied?
    /// </summary>
    public bool IsApplied { get; set; }

 /// <summary>
    /// Rule ID that produced this result
    /// </summary>
    public string RuleId { get; set; } = string.Empty;

    /// <summary>
 /// Amount applied to patient responsibility
    /// </summary>
    public decimal? PatientResponsibilityApplied { get; set; }

    /// <summary>
    /// Amount remaining for insurance
    /// </summary>
    public decimal? RemainingAmount { get; set; }

    /// <summary>
    /// Message explaining the result
    /// </summary>
    public string? Message { get; set; }

    /// <summary>
    /// Error if rule failed
    /// </summary>
    public string? Error { get; set; }

    /// <summary>
 /// Create a skip result (rule not applicable)
    /// </summary>
    public static RuleResult Skip(string ruleId) =>
        new() { IsApplied = false, RuleId = ruleId };

    /// <summary>
    /// Create an applied result
    /// </summary>
    public static RuleResult Applied(
        string ruleId,
        decimal patientResponsibility,
  decimal? remaining = null,
    string? message = null) =>
        new()
        {
      IsApplied = true,
   RuleId = ruleId,
            PatientResponsibilityApplied = patientResponsibility,
     RemainingAmount = remaining,
            Message = message
        };

    /// <summary>
  /// Create a failure result
    /// </summary>
    public static RuleResult Failed(string ruleId, string error) =>
        new() { IsApplied = false, RuleId = ruleId, Error = error };
}
```

### **Task 3.2: Create AdjudicationRuleEngine** (1.5 hours)

**File:** `Application/Services/RCM/Rules/AdjudicationRuleEngine.cs`

```csharp
namespace NPhies_FHIR_Integration.Application.Services.RCM.Rules;

using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

/// <summary>
/// Engine that executes adjudication rules in proper sequence
/// </summary>
public class AdjudicationRuleEngine
{
    private readonly ILogger<AdjudicationRuleEngine> _logger;
    private readonly List<IAdjudicationRule> _rules = new();

 public AdjudicationRuleEngine(ILogger<AdjudicationRuleEngine> logger)
    {
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
    }

    /// <summary>
    /// Register rules (will be sorted by priority)
    /// </summary>
    public void RegisterRules(params IAdjudicationRule[] rules)
    {
        _rules.AddRange(rules);
    _rules.Sort((a, b) => a.Priority.CompareTo(b.Priority));
 
        _logger.LogInformation("Registered {Count} rules in order: {Rules}",
      _rules.Count,
            string.Join(" ? ", _rules.Select(r => r.RuleId)));
    }

    /// <summary>
    /// Execute all applicable rules in priority order
    /// </summary>
    public async Task<AdjudicationExecutionResult> ExecuteAsync(AdjudicationContext context)
    {
        _logger.LogInformation("Starting adjudication for item {ItemSequence}", context.ItemSequence);

        var result = new AdjudicationExecutionResult
     {
   ItemSequence = context.ItemSequence,
  SubmittedAmount = context.SubmittedAmount,
       AllowedAmount = context.AllowedAmount,
            ExecutedRules = new List<ExecutedRuleDetail>()
        };

 try
        {
    // Execute each rule in priority order
  foreach (var rule in _rules)
   {
       // Check if rule is applicable
                var applicable = await rule.IsApplicableAsync(context);
              if (!applicable)
        {
   _logger.LogDebug("Rule {RuleId} not applicable for item {ItemSequence}",
   rule.RuleId, context.ItemSequence);
  continue;
         }

          // Execute rule
           _logger.LogInformation("Applying rule {RuleId} ({RuleName}) for item {ItemSequence}",
   rule.RuleId, rule.RuleName, context.ItemSequence);

 var ruleResult = await rule.EvaluateAsync(context);

             // Record execution
          result.ExecutedRules.Add(new ExecutedRuleDetail
     {
RuleId = rule.RuleId,
           RuleName = rule.RuleName,
     Priority = rule.Priority,
             IsApplied = ruleResult.IsApplied,
 PatientResponsibilityApplied = ruleResult.PatientResponsibilityApplied,
     RemainingAmount = ruleResult.RemainingAmount,
      Message = ruleResult.Message,
         Error = ruleResult.Error
       });

             if (ruleResult.IsApplied)
  {
      // Update context for next rule
      context.RemainingAmount = ruleResult.RemainingAmount ?? context.RemainingAmount;
  
   _logger.LogInformation("Rule {RuleId} applied: {Message}",
          rule.RuleId, ruleResult.Message);
    }
            }

            // Calculate final result
         result.InsuranceResponsibility = context.RemainingAmount;
     result.PatientResponsibility = context.AllowedAmount - result.InsuranceResponsibility;
  result.IsSuccessful = true;

      _logger.LogInformation(
    "Adjudication completed for item {ItemSequence}: Insurance=${Insurance:F2}, Patient=${Patient:F2}",
 context.ItemSequence,
        result.InsuranceResponsibility,
        result.PatientResponsibility);

            return result;
        }
        catch (Exception ex)
        {
 _logger.LogError(ex, "Error executing adjudication rules for item {ItemSequence}",
         context.ItemSequence);

       result.IsSuccessful = false;
            result.ErrorMessage = ex.Message;
 return result;
  }
    }
}

/// <summary>
/// Result of adjudication execution
/// </summary>
public class AdjudicationExecutionResult
{
    public int ItemSequence { get; set; }
    public decimal SubmittedAmount { get; set; }
    public decimal AllowedAmount { get; set; }
 public decimal InsuranceResponsibility { get; set; }
    public decimal PatientResponsibility { get; set; }
    public bool IsSuccessful { get; set; }
 public string? ErrorMessage { get; set; }
    public List<ExecutedRuleDetail> ExecutedRules { get; set; } = new();
}

/// <summary>
/// Details of a rule that was executed
/// </summary>
public class ExecutedRuleDetail
{
    public string RuleId { get; set; } = string.Empty;
    public string RuleName { get; set; } = string.Empty;
    public int Priority { get; set; }
    public bool IsApplied { get; set; }
    public decimal? PatientResponsibilityApplied { get; set; }
    public decimal? RemainingAmount { get; set; }
    public string? Message { get; set; }
    public string? Error { get; set; }
}
```

### **Task 3.3: Build & Verify** (0.5 hours)

---

## ?? DAYS 4-5: IMPLEMENT 10+ RULES (15 hours)

### **Core Rules (Priority 1-5) - 10 hours**

**Rule 1: DeductibleRule (Priority 10)**
- Apply annual deductible first
- Deduct from allowed amount
- Track remaining deductible
- File: `Application/Services/RCM/Rules/DeductibleRule.cs`

**Rule 2: CopayRule (Priority 20)**
- Apply copay (fixed amount per visit)
- Only applies if deductible met
- Apply to remaining amount
- File: `Application/Services/RCM/Rules/CopayRule.cs`

**Rule 3: CoinsuranceRule (Priority 30)**
- Apply coinsurance percentage
- Only applies after deductible + copay
- Calculate patient portion (20% or plan rate)
- File: `Application/Services/RCM/Rules/CoinsuranceRule.cs`

**Rule 4: NetworkStatusRule (Priority 5)**
- Adjust rates based on in-network vs out-of-network
- Out-of-network: Higher coinsurance
- May reduce allowed amount
- File: `Application/Services/RCM/Rules/NetworkStatusRule.cs`

**Rule 5: ServiceExclusionRule (Priority 1)**
- Check if service is excluded
- Deny entire claim if excluded
- No appeal if explicitly excluded
- File: `Application/Services/RCM/Rules/ServiceExclusionRule.cs`

**Rule 6: OutOfPocketRule (Priority 40)**
- Enforce out-of-pocket maximum
- After patient responsibility reaches limit, insurance pays 100%
- File: `Application/Services/RCM/Rules/OutOfPocketRule.cs`

**Rule 7: BenefitLimitRule (Priority 50)**
- Check annual benefit limits
- Check per-service limits
- Reduce or deny if limit exceeded
- File: `Application/Services/RCM/Rules/BenefitLimitRule.cs`

**Rule 8: PriorAuthRule (Priority 2)**
- Check if prior authorization required
- Check authorization validity
- Deny if missing/expired
- File: `Application/Services/RCM/Rules/PriorAuthRule.cs`

**Rule 9: WaitingPeriodRule (Priority 3)**
- Check waiting period
- Deny if service date within waiting period
- File: `Application/Services/RCM/Rules/WaitingPeriodRule.cs`

**Rule 10: AgeQualificationRule (Priority 4)**
- Check age-based eligibility
- Some services have age limits
- File: `Application/Services/RCM/Rules/AgeQualificationRule.cs`

### **Extended Rules (Priority 6-8) - 5 hours**

**Rule 11: DiagnosisValidationRule** - Validate diagnosis codes  
**Rule 12: QuantityLimitRule** - Check quantity limits  
**Rule 13: FrequencyLimitRule** - Check service frequency limits  

---

## ?? IMPLEMENTATION TEMPLATE

Each rule follows this pattern:

```csharp
namespace NPhies_FHIR_Integration.Application.Services.RCM.Rules;

using Microsoft.Extensions.Logging;
using System;
using System.Threading.Tasks;

/// <summary>
/// [Rule Description]
/// Priority: [Number] ([Execution Order])
/// </summary>
public class [RuleName]Rule : IAdjudicationRule
{
    private readonly ILogger<[RuleName]Rule> _logger;

    public string RuleId => "[RULE_ID]";
    public string RuleName => "[Human Readable Name]";
    public int Priority => [Priority Number];

    public [RuleName]Rule(ILogger<[RuleName]Rule> logger)
    {
        _logger = logger;
    }

    /// <summary>
    /// Check if rule is applicable
    /// </summary>
    public async Task<bool> IsApplicableAsync(AdjudicationContext context)
    {
   // Check conditions when this rule applies
        return await Task.FromResult([condition]);
    }

    /// <summary>
    /// Apply rule logic
    /// </summary>
    public async Task<RuleResult> EvaluateAsync(AdjudicationContext context)
    {
  try
        {
            // Rule logic here
            
     return RuleResult.Applied(
     ruleId: RuleId,
                patientResponsibility: [calculated],
        remaining: [remaining_amount],
                message: $"[Description of what was applied]");
        }
        catch (Exception ex)
   {
  _logger.LogError(ex, "Error in {RuleId}", RuleId);
            return RuleResult.Failed(RuleId, ex.Message);
        }
    }
}
```

---

## ?? TESTING STRATEGY

### **Unit Tests (Create alongside rules)**

For each rule, create:
- `[RuleName]RuleTests.cs`
- Test IsApplicableAsync returns correct boolean
- Test EvaluateAsync with various amounts
- Test error conditions

### **Integration Tests**

- `AdjudicationRuleEngineTests.cs`
- Test rule execution sequence
- Test rule priority ordering
- Test financial calculations
- Test edge cases (0 amounts, negative calculations)

---

## ?? DAY-BY-DAY EXECUTION

### **Day 3 (8 hours)**
- Hours 1-1.5: Create IAdjudicationRule interface
- Hours 1.5-3: Create AdjudicationRuleEngine
- Hours 3-3.5: Build & verify
- Hours 3.5-8: Start implementing core rules
  - ServiceExclusionRule (30 min)
  - PriorAuthRule (30 min)
  - WaitingPeriodRule (30 min)
  - AgeQualificationRule (30 min)
  - NetworkStatusRule (1 hour)

### **Day 4 (8 hours)**
- Hours 0-2: Continue implementing core rules
  - DeductibleRule (1 hour)
  - CopayRule (1 hour)
- Hours 2-5: Implement essential rules
  - CoinsuranceRule (1 hour)
  - OutOfPocketRule (1 hour)
  - BenefitLimitRule (1 hour)
- Hours 5-8: Register in DI + Basic testing

### **Day 5 (8 hours)**
- Hours 0-3: Remaining rules
  - DiagnosisValidationRule (30 min)
  - QuantityLimitRule (30 min)
  - FrequencyLimitRule (30 min)
  - Unit tests (1.5 hours)
- Hours 3-6: Integration testing
  - Create AdjudicationRuleEngineTests
  - Test rule execution
  - Test financial calculations
- Hours 6-8: Documentation & preparation for Week 2

---

## ? SUCCESS CRITERIA

**By End of Day 5:**
- [ ] IAdjudicationRule interface created
- [ ] AdjudicationRuleEngine implemented
- [ ] 10+ rules implemented and tested
- [ ] Rules registered in DI
- [ ] Unit tests passing (>95%)
- [ ] Integration tests passing
- [ ] Build succeeding (0 errors)
- [ ] Ready to integrate with ClaimResponseProcessingService

---

## ?? DELIVERABLES

**Code:**
- 1 interface file
- 1 engine file
- 10+ rule files
- 12+ test files

**Tests:**
- Unit tests for each rule
- Integration tests for engine
- Edge case testing

**Documentation:**
- XML comments on all public methods
- Rule priority documentation
- Integration guide

---

## ?? ESTIMATED METRICS

| Metric | Estimate |
|--------|----------|
| New Files | 13 |
| New Classes | 13 |
| Lines of Code | 1,500+ |
| Test Methods | 40+ |
| Build Time | < 30 seconds |
| Test Run Time | < 5 seconds |

---

**Next:** After Days 3-5, move to Week 2 - Appeal Workflow Implementation

