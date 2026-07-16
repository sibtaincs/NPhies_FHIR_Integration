using NPhies_FHIR_Integration.Domain.Entities;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace NPhies_FHIR_Integration.Application.Services.RCM
{
    /// <summary>
    /// NPHIES Adjudication Rules Engine Service Interface
    /// Manages claim adjudication based on NPHIES rules and plan design
    /// </summary>
    public interface IAdjudicationRulesEngine
    {
  /// <summary>
      /// Execute adjudication on a claim
        /// </summary>
 Task<AdjudicationResult> AdjudicateClaimAsync(AdjudicationRequest request);

  /// <summary>
        /// Execute adjudication on a claim item
        /// </summary>
        Task<ItemAdjudicationResult> AdjudicateClaimItemAsync(ItemAdjudicationRequest request);

     /// <summary>
        /// Get all applicable rules for a claim
        /// </summary>
        Task<List<AdjudicationRule>> GetApplicableRulesAsync(AdjudicationContext context);

        /// <summary>
      /// Evaluate a specific rule
        /// </summary>
   Task<RuleEvaluationResult> EvaluateRuleAsync(AdjudicationRule rule, AdjudicationContext context);

        /// <summary>
   /// Get adjudication decision explanation
        /// </summary>
        Task<string> GetAdjudicationReasonAsync(AdjudicationResult result);

   /// <summary>
  /// Check if claim is eligible for appeal
        /// </summary>
Task<bool> IsAppealEligibleAsync(AdjudicationResult result);

        /// <summary>
      /// Get detailed adjudication breakdown
        /// </summary>
        Task<AdjudicationBreakdown> GetAdjudicationBreakdownAsync(string claimId);

        /// <summary>
        /// Validate adjudication results
        /// </summary>
        Task<List<AdjudicationValidationMessage>> ValidateAdjudicationAsync(AdjudicationResult result);

  /// <summary>
  /// Get adjudication rule statistics
        /// </summary>
     Task<RuleStatistics> GetRuleStatisticsAsync(string ruleId);

        /// <summary>
        /// Execute batch adjudication
    /// </summary>
     Task<BatchAdjudicationResult> AdjudicateBatchAsync(List<AdjudicationRequest> requests);
    }

    /// <summary>
    /// Adjudication request
/// </summary>
    public class AdjudicationRequest
    {
   public string ClaimId { get; set; } = string.Empty;
        public string PatientId { get; set; } = string.Empty;
     public string ProviderId { get; set; } = string.Empty;
        public string InsurerId { get; set; } = string.Empty;
  public string CoverageId { get; set; } = string.Empty;
        public DateTime ServiceDate { get; set; }
        public decimal ClaimAmount { get; set; }
        public List<AdjudicationClaimItem> ClaimItems { get; set; } = new();
        public List<string> DiagnosisCodes { get; set; } = new();
    public string ClaimType { get; set; } = string.Empty;
        public bool IsNetworkProvider { get; set; }
        public Dictionary<string, object> AdditionalContext { get; set; } = new();
    }

    /// <summary>
    /// Adjudication claim item
  /// </summary>
    public class AdjudicationClaimItem
    {
 public int SequenceNumber { get; set; }
        public string ServiceCode { get; set; } = string.Empty;
        public string ServiceDescription { get; set; } = string.Empty;
        public decimal Quantity { get; set; }
        public decimal UnitAmount { get; set; }
        public decimal LineAmount { get; set; }
        public List<string> Modifiers { get; set; } = new();
    }

    /// <summary>
    /// Item adjudication request
    /// </summary>
    public class ItemAdjudicationRequest
    {
        public string ClaimId { get; set; } = string.Empty;
        public int ItemSequence { get; set; }
        public AdjudicationClaimItem Item { get; set; }
        public string ServiceCode { get; set; } = string.Empty;
        public decimal RequestedAmount { get; set; }
        public AdjudicationContext Context { get; set; }
    }

    /// <summary>
    /// Adjudication result
    /// </summary>
    public class AdjudicationResult
    {
   public string ClaimId { get; set; } = string.Empty;
        public AdjudicationStatus Status { get; set; } = AdjudicationStatus.Pending;
        public decimal AllowedAmount { get; set; }
   public decimal PatientResponsibility { get; set; }
        public decimal InsurancePayment { get; set; }
        public decimal DenialAmount { get; set; }
        public DateTime AdjudicationDate { get; set; } = DateTime.UtcNow;
        public List<string> AppliedRules { get; set; } = new();
        public List<AdjudicationMessage> Messages { get; set; } = new();
        public List<ItemAdjudicationResult> ItemResults { get; set; } = new();
        public bool IsAppealEligible { get; set; }
        public string DenialReason { get; set; } = string.Empty;
  public int DaysForAppeal { get; set; } = 30;
public Dictionary<string, object> Details { get; set; } = new();
    }

    /// <summary>
    /// Item adjudication result
    /// </summary>
    public class ItemAdjudicationResult
    {
      public int ItemSequence { get; set; }
    public string ServiceCode { get; set; } = string.Empty;
        public decimal RequestedAmount { get; set; }
        public decimal AllowedAmount { get; set; }
        public decimal PatientResponsibility { get; set; }
      public decimal InsurancePayment { get; set; }
        public ItemAdjudicationStatus Status { get; set; } = ItemAdjudicationStatus.Approved;
   public List<string> AppliedRules { get; set; } = new();
        public string DenialReason { get; set; } = string.Empty;
    }

    /// <summary>
    /// Adjudication status
    /// </summary>
    public enum AdjudicationStatus
    {
        Pending = 0,
    Approved = 1,
      PartiallyApproved = 2,
      Denied = 3,
        PendingReview = 4,
        AppealPending = 5
    }

    /// <summary>
    /// Item adjudication status
  /// </summary>
    public enum ItemAdjudicationStatus
    {
        Approved = 1,
        Denied = 2,
        PartiallyApproved = 3,
        PendingReview = 4
    }

    /// <summary>
    /// Adjudication message
    /// </summary>
    public class AdjudicationMessage
    {
   public string Code { get; set; } = string.Empty;
        public string Text { get; set; } = string.Empty;
        public AdjudicationMessageSeverity Severity { get; set; } = AdjudicationMessageSeverity.Info;
   public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    }

    /// <summary>
    /// Adjudication message severity
    /// </summary>
  public enum AdjudicationMessageSeverity
    {
  Info = 0,
        Warning = 1,
        Error = 2,
        Critical = 3
    }

    /// <summary>
    /// Adjudication rule
    /// </summary>
    public class AdjudicationRule
    {
        public string RuleId { get; set; } = string.Empty;
        public string RuleName { get; set; } = string.Empty;
        public string RuleDescription { get; set; } = string.Empty;
      public RuleCategory Category { get; set; }
        public int Priority { get; set; }
        public bool IsActive { get; set; } = true;
        public string ConditionJson { get; set; } = string.Empty;
        public string ActionJson { get; set; } = string.Empty;
  public string NphiesReference { get; set; } = string.Empty;
    }

    /// <summary>
    /// Rule category
    /// </summary>
    public enum RuleCategory
    {
    Coverage = 1,
        Benefit = 2,
        Network = 3,
 Medical = 4,
        Frequency = 5,
        Duration = 6,
        Amount = 7,
        Authorization = 8,
        Exclusion = 9,
        Limitation = 10
    }

    /// <summary>
    /// Rule evaluation result
    /// </summary>
public class RuleEvaluationResult
 {
        public string RuleId { get; set; } = string.Empty;
        public bool RuleMatched { get; set; }
    public RuleAction Action { get; set; }
        public List<string> Messages { get; set; } = new();
    }

    /// <summary>
    /// Rule action
    /// </summary>
    public class RuleAction
    {
        public ActionType Type { get; set; }
        public decimal? PercentageAdjustment { get; set; }
 public decimal? AmountAdjustment { get; set; }
  public string DenialReason { get; set; } = string.Empty;
public bool BlockClaim { get; set; }
    }

/// <summary>
    /// Action type
    /// </summary>
    public enum ActionType
    {
        Approve = 1,
        PartialApprove = 2,
     Deny = 3,
        AdjustAmount = 4,
   RequireReview = 5,
        RequireAuthorization = 6
    }

    /// <summary>
    /// Adjudication breakdown
    /// </summary>
    public class AdjudicationBreakdown
    {
    public string ClaimId { get; set; } = string.Empty;
     public decimal OriginalAmount { get; set; }
        public decimal AllowedAmount { get; set; }
    public decimal Copay { get; set; }
        public decimal Coinsurance { get; set; }
        public decimal Deductible { get; set; }
      public decimal OutOfPocketMax { get; set; }
        public decimal InsurancePayment { get; set; }
        public decimal PatientResponsibility { get; set; }
        public List<BreakdownLine> BreakdownLines { get; set; } = new();
    }

    /// <summary>
    /// Breakdown line
    /// </summary>
    public class BreakdownLine
    {
 public string Description { get; set; } = string.Empty;
        public decimal Amount { get; set; }
public string Category { get; set; } = string.Empty;
    }

    /// <summary>
    /// Adjudication validation message
    /// </summary>
    public class AdjudicationValidationMessage
    {
     public string Code { get; set; } = string.Empty;
        public string Message { get; set; } = string.Empty;
        public ValidationSeverity Severity { get; set; } = ValidationSeverity.Info;
    }

 /// <summary>
    /// Validation severity
    /// </summary>
    public enum ValidationSeverity
    {
      Info = 0,
        Warning = 1,
   Error = 2
    }

    /// <summary>
    /// Rule statistics
    /// </summary>
    public class RuleStatistics
    {
        public string RuleId { get; set; } = string.Empty;
        public int TimesApplied { get; set; }
    public int TimesMatched { get; set; }
        public int TimesDenied { get; set; }
    public decimal AverageAdjustment { get; set; }
        public DateTime FirstApplied { get; set; }
 public DateTime LastApplied { get; set; }
    }

    /// <summary>
    /// Batch adjudication result
    /// </summary>
    public class BatchAdjudicationResult
    {
   public int TotalClaims { get; set; }
        public int ApprovedCount { get; set; }
public int DeniedCount { get; set; }
        public int PartialCount { get; set; }
        public decimal TotalClaimAmount { get; set; }
     public decimal TotalApprovedAmount { get; set; }
  public decimal TotalDeniedAmount { get; set; }
        public List<AdjudicationResult> Results { get; set; } = new();
        public DateTime ProcessedAt { get; set; } = DateTime.UtcNow;
        public string BatchId { get; set; } = string.Empty;
    }

    /// <summary>
    /// NPHIES Adjudication Rules Engine Implementation
    /// </summary>
    public class AdjudicationRulesEngine : IAdjudicationRulesEngine
    {
      private readonly ILogger<AdjudicationRulesEngine> _logger;
        private readonly List<AdjudicationRule> _adjudicationRules = new();

        public AdjudicationRulesEngine(ILogger<AdjudicationRulesEngine> logger)
        {
            _logger = logger;
            InitializeDefaultRules();
        }

        /// <summary>
        /// Initialize default adjudication rules
        /// </summary>
        private void InitializeDefaultRules()
        {
        try
  {
       _logger.LogInformation("Initializing adjudication rules");

      // 10 core adjudication rules
          for (int i = 1; i <= 10; i++)
    {
       _adjudicationRules.Add(new AdjudicationRule
           {
             RuleId = $"ADJ-{i:D3}",
    RuleName = GetRuleName(i),
    RuleDescription = GetRuleDescription(i),
          Category = GetRuleCategory(i),
Priority = i,
            IsActive = true,
      NphiesReference = $"NPHIES Adjudication - {GetRuleName(i)}"
         });
 }

         _logger.LogInformation($"Initialized {_adjudicationRules.Count} adjudication rules");
            }
       catch (Exception ex)
            {
     _logger.LogError(ex, "Error initializing adjudication rules");
 }
        }

        private string GetRuleName(int ruleNumber) => ruleNumber switch
        {
        1 => "Coverage Verification",
  2 => "Network Provider Verification",
         3 => "Benefit Determination",
         4 => "Deductible Application",
      5 => "Copay Application",
   6 => "Coinsurance Application",
      7 => "Frequency Limit Checking",
8 => "Duration Limit Checking",
9 => "Medical Necessity Checking",
  10 => "Authorization Verification",
            _ => "Unknown Rule"
  };

     private string GetRuleDescription(int ruleNumber) => ruleNumber switch
  {
     1 => "Verify patient has active coverage",
         2 => "Verify provider is in network",
    3 => "Determine benefit for service",
         4 => "Apply deductible to claim",
   5 => "Apply copay to claim",
 6 => "Apply coinsurance percentage",
            7 => "Check service frequency limits",
         8 => "Check service duration limits",
  9 => "Verify service medical necessity",
   10 => "Verify required authorization",
       _ => "Unknown Rule"
        };

        private RuleCategory GetRuleCategory(int ruleNumber) => ruleNumber switch
    {
      1 => RuleCategory.Coverage,
     2 => RuleCategory.Network,
         3 => RuleCategory.Benefit,
         4 or 5 or 6 => RuleCategory.Amount,
    7 => RuleCategory.Frequency,
      8 => RuleCategory.Duration,
     9 => RuleCategory.Medical,
            10 => RuleCategory.Authorization,
            _ => RuleCategory.Coverage
        };

   public async Task<AdjudicationResult> AdjudicateClaimAsync(AdjudicationRequest request)
        {
            var result = new AdjudicationResult { ClaimId = request.ClaimId };

            try
      {
              _logger.LogInformation($"Starting adjudication for claim: {request.ClaimId}");

          var context = new AdjudicationContext
      {
           CoverageId = request.CoverageId,
         ClaimType = request.ClaimType,
             ServiceDate = request.ServiceDate,
            IsNetworkProvider = request.IsNetworkProvider
    };

 var applicableRules = await GetApplicableRulesAsync(context);

   result.AllowedAmount = request.ClaimAmount;
      result.InsurancePayment = request.ClaimAmount;
     result.PatientResponsibility = 0;

     foreach (var rule in applicableRules.OrderBy(r => r.Priority))
           {
     var ruleResult = await EvaluateRuleAsync(rule, context);

            if (ruleResult.RuleMatched)
        {
  result.AppliedRules.Add(rule.RuleId);

            if (ruleResult.Action != null)
     {
  if (ruleResult.Action.Type == ActionType.Deny)
     {
    result.Status = AdjudicationStatus.Denied;
       result.DenialAmount = result.AllowedAmount;
  result.AllowedAmount = 0;
        result.DenialReason = ruleResult.Action.DenialReason;
 result.Messages.Add(new AdjudicationMessage
      {
 Code = rule.RuleId,
  Text = ruleResult.Action.DenialReason,
    Severity = AdjudicationMessageSeverity.Critical
            });
               break;
       }
  else if (ruleResult.Action.Type == ActionType.AdjustAmount)
           {
    if (ruleResult.Action.PercentageAdjustment.HasValue)
        result.AllowedAmount *= (1 - ruleResult.Action.PercentageAdjustment.Value / 100);
    if (ruleResult.Action.AmountAdjustment.HasValue)
     result.AllowedAmount -= ruleResult.Action.AmountAdjustment.Value;
          }
   }
        }
 }

        if (result.Status != AdjudicationStatus.Denied)
       {
      result.Status = result.AllowedAmount == request.ClaimAmount ? 
                  AdjudicationStatus.Approved : AdjudicationStatus.PartiallyApproved;
  }

    result.InsurancePayment = result.AllowedAmount;
      result.PatientResponsibility = result.AllowedAmount > 0 ? 
       (request.ClaimAmount - result.AllowedAmount) : 0;

        result.ItemResults = new List<ItemAdjudicationResult>();
          foreach (var item in request.ClaimItems)
          {
         var itemRequest = new ItemAdjudicationRequest
        {
       ClaimId = request.ClaimId,
            ItemSequence = item.SequenceNumber,
       Item = item,
        ServiceCode = item.ServiceCode,
          RequestedAmount = item.LineAmount,
      Context = context
     };

      var itemResult = await AdjudicateClaimItemAsync(itemRequest);
    result.ItemResults.Add(itemResult);
       }

      result.AdjudicationDate = DateTime.UtcNow;
  result.IsAppealEligible = result.Status != AdjudicationStatus.Approved;

  _logger.LogInformation($"Adjudication completed for claim {request.ClaimId}: Status={result.Status}");
            }
      catch (Exception ex)
         {
   _logger.LogError(ex, $"Error adjudicating claim: {request.ClaimId}");
        result.Status = AdjudicationStatus.PendingReview;
              result.Messages.Add(new AdjudicationMessage
          {
         Code = "ADJ-ERR-001",
   Text = $"Error during adjudication: {ex.Message}",
   Severity = AdjudicationMessageSeverity.Error
   });
        }

            return result;
        }

      public async Task<ItemAdjudicationResult> AdjudicateClaimItemAsync(ItemAdjudicationRequest request)
        {
     var result = new ItemAdjudicationResult
            {
        ItemSequence = request.ItemSequence,
                ServiceCode = request.ServiceCode,
     RequestedAmount = request.RequestedAmount,
            AllowedAmount = request.RequestedAmount
     };

            try
       {
  _logger.LogInformation($"Adjudicating item {request.ItemSequence} of claim {request.ClaimId}");

     result.AllowedAmount = request.RequestedAmount;
                result.InsurancePayment = result.AllowedAmount;
                result.Status = ItemAdjudicationStatus.Approved;
  }
         catch (Exception ex)
            {
                _logger.LogError(ex, $"Error adjudicating claim item {request.ItemSequence}");
            result.Status = ItemAdjudicationStatus.PendingReview;
   }

            return result;
  }

  public async Task<List<AdjudicationRule>> GetApplicableRulesAsync(AdjudicationContext context)
    {
            try
            {
  _logger.LogInformation($"Getting applicable rules for context");
 return _adjudicationRules.Where(r => r.IsActive).ToList();
  }
catch (Exception ex)
{
  _logger.LogError(ex, "Error getting applicable rules");
                return new List<AdjudicationRule>();
        }
    }

  public async Task<RuleEvaluationResult> EvaluateRuleAsync(AdjudicationRule rule, AdjudicationContext context)
        {
        var result = new RuleEvaluationResult { RuleId = rule.RuleId };

        try
      {
   _logger.LogInformation($"Evaluating rule {rule.RuleId}");
            result.RuleMatched = true;
        result.Action = new RuleAction { Type = ActionType.Approve };
         return result;
      }
            catch (Exception ex)
  {
          _logger.LogError(ex, $"Error evaluating rule {rule.RuleId}");
   return result;
  }
        }

        public async Task<string> GetAdjudicationReasonAsync(AdjudicationResult result)
     {
      try
 {
        return result.Status switch
          {
  AdjudicationStatus.Approved => "Claim approved for payment",
          AdjudicationStatus.Denied => result.DenialReason,
     AdjudicationStatus.PartiallyApproved => $"Claim partially approved: ${result.AllowedAmount} allowed",
         AdjudicationStatus.PendingReview => "Claim pending manual review",
       _ => "Unknown adjudication status"
      };
            }
   catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting adjudication reason");
         return "Unable to determine adjudication reason";
     }
        }

        public async Task<bool> IsAppealEligibleAsync(AdjudicationResult result)
        {
         return result.Status != AdjudicationStatus.Approved;
        }

        public async Task<AdjudicationBreakdown> GetAdjudicationBreakdownAsync(string claimId)
  {
   try
      {
     _logger.LogInformation($"Getting adjudication breakdown for claim {claimId}");
  return new AdjudicationBreakdown { ClaimId = claimId };
  }
          catch (Exception ex)
          {
            _logger.LogError(ex, $"Error getting adjudication breakdown for claim {claimId}");
     return null;
  }
     }

        public async Task<List<AdjudicationValidationMessage>> ValidateAdjudicationAsync(AdjudicationResult result)
        {
          var messages = new List<AdjudicationValidationMessage>();

            try
   {
         if (result.AllowedAmount < 0)
     {
             messages.Add(new AdjudicationValidationMessage
  {
    Code = "ADJ-VAL-001",
           Message = "Allowed amount cannot be negative",
   Severity = ValidationSeverity.Error
           });
             }

    if (result.InsurancePayment + result.PatientResponsibility != result.AllowedAmount)
  {
               messages.Add(new AdjudicationValidationMessage
            {
                 Code = "ADJ-VAL-002",
        Message = "Payment amounts do not sum correctly",
      Severity = ValidationSeverity.Error
      });
      }
         }
            catch (Exception ex)
      {
    _logger.LogError(ex, "Error validating adjudication");
            }

        return messages;
     }

        public async Task<RuleStatistics> GetRuleStatisticsAsync(string ruleId)
        {
    try
 {
         return new RuleStatistics { RuleId = ruleId };
}
     catch (Exception ex)
            {
 _logger.LogError(ex, $"Error getting rule statistics for {ruleId}");
           return null;
            }
  }

   public async Task<BatchAdjudicationResult> AdjudicateBatchAsync(List<AdjudicationRequest> requests)
    {
            var batchResult = new BatchAdjudicationResult
         {
         BatchId = Guid.NewGuid().ToString(),
  TotalClaims = requests.Count
 };

            try
 {
         _logger.LogInformation($"Starting batch adjudication of {requests.Count} claims");

    batchResult.Results = new List<AdjudicationResult>();

           foreach (var request in requests)
           {
      var result = await AdjudicateClaimAsync(request);
       batchResult.Results.Add(result);

    if (result.Status == AdjudicationStatus.Approved)
         batchResult.ApprovedCount++;
      else if (result.Status == AdjudicationStatus.Denied)
        batchResult.DeniedCount++;
       else if (result.Status == AdjudicationStatus.PartiallyApproved)
       batchResult.PartialCount++;

          batchResult.TotalClaimAmount += request.ClaimAmount;
                batchResult.TotalApprovedAmount += result.AllowedAmount;
 batchResult.TotalDeniedAmount += result.DenialAmount;
         }

         _logger.LogInformation($"Batch adjudication completed: {batchResult.ApprovedCount} approved, {batchResult.DeniedCount} denied");
          }
            catch (Exception ex)
   {
      _logger.LogError(ex, "Error in batch adjudication");
            }

   return batchResult;
        }
    }
}
