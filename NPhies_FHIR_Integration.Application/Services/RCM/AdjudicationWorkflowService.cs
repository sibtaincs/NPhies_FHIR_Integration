using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using NPhies_FHIR_Integration.Domain.Entities;

namespace NPhies_FHIR_Integration.Application.Services.RCM;

/// <summary>
/// Adjudication Workflow Service Implementation
/// Handles adjudication logic, rule application, and document generation
/// </summary>
public class AdjudicationWorkflowService : IAdjudicationWorkflowService
{
    private readonly ILogger<AdjudicationWorkflowService> _logger;
    private readonly IClaimResponseProcessingService _claimResponseProcessor;

    public AdjudicationWorkflowService(
        ILogger<AdjudicationWorkflowService> logger,
        IClaimResponseProcessingService claimResponseProcessor)
  {
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        _claimResponseProcessor = claimResponseProcessor ?? throw new ArgumentNullException(nameof(claimResponseProcessor));
    }

    /// <summary>
    /// Process adjudication for claim items
 /// </summary>
 public async Task<AdjudicationWorkflowResult> ProcessAdjudicationAsync(
        Claim claim,
        Coverage coverage,
ClaimResponse response,
  CancellationToken cancellationToken = default)
    {
        _logger.LogInformation("Processing adjudication for claim ID {ClaimId}", claim.Id);

        var result = new AdjudicationWorkflowResult
      {
            ClaimId = claim.Id,
     IsSuccessful = false,
   OverallStatus = "processing"
        };

try
        {
            // Step 1: Extract adjudication details from response
        var adjDetails = await _claimResponseProcessor.ExtractAdjudicationDetailsAsync(response, cancellationToken);
 _logger.LogInformation("Extracted {Count} adjudication details for claim {ClaimId}", adjDetails.Count, claim.Id);

            // Step 2: Apply adjudication rules to each claim item
   var context = new AdjudicationContext
            {
     CoverageId = coverage?.Id ?? string.Empty,
     ClaimType = claim.ClaimType ?? "professional",
       ServiceDate = claim.ServicedPeriodStart ?? claim.CreatedDate,
   IsNetworkProvider = IsNetworkProvider(claim),
   HasPreAuthorization = HasPreAuthorization(response),
      PreAuthNumber = ExtractPreAuthNumber(response)
            };

            var ruleResults = new List<AdjudicationRuleResult>();
  if (claim.Items != null)
    {
                foreach (var item in claim.Items)
      {
 var ruleResult = await ApplyAdjudicationRulesAsync(item, context, cancellationToken);
     ruleResults.Add(ruleResult);
       }
            }

_logger.LogInformation("Applied adjudication rules to {Count} items for claim {ClaimId}",
        ruleResults.Count, claim.Id);

  // Step 3: Calculate financial totals
    var approvedItems = adjDetails.Where(d => d.Status == "approved").ToList();
      var deniedItems = adjDetails.Where(d => d.Status == "denied").ToList();
    var pendingItems = adjDetails.Where(d => d.Status == "pending" || d.Status == "pended").ToList();

  result.TotalApprovedAmount = approvedItems.Sum(a => a.InsuranceResponsibility);
          result.TotalDeniedAmount = deniedItems.Sum(d => d.SubmittedAmount - d.AllowedAmount);
     result.TotalPendedAmount = pendingItems.Sum(p => p.InsuranceResponsibility);

            // Step 4: Determine overall status
            if (deniedItems.Any())
  result.OverallStatus = "partial-approval";
            else if (pendingItems.Any())
    result.OverallStatus = "pended";
     else
              result.OverallStatus = "approved";

  // Step 5: Generate adjudication narrative
       result.Narrative = await GenerateAdjudicationNarrativeAsync(response, adjDetails, cancellationToken);

 // Step 6: Calculate appeal deadlines
     result.AppealDeadlines = await CalculateAppealDeadlinesAsync(response, cancellationToken);

       // Step 7: Mark as successful
         result.IsSuccessful = true;

          _logger.LogInformation("Adjudication processed successfully for claim ID {ClaimId}. " +
       "Approved: {Approved}, Denied: {Denied}, Pended: {Pended}. Status: {Status}",
      claim.Id, result.TotalApprovedAmount, result.TotalDeniedAmount, result.TotalPendedAmount,
     result.OverallStatus);

            return result;
     }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error processing adjudication for claim ID {ClaimId}", claim.Id);
            result.IsSuccessful = false;
        result.OverallStatus = "error";
 result.Errors.Add(ex.Message);
     return result;
   }
    }

    /// <summary>
    /// Apply adjudication rules to a claim item
    /// </summary>
    public async Task<AdjudicationRuleResult> ApplyAdjudicationRulesAsync(
        ClaimItem item,
     AdjudicationContext context,
        CancellationToken cancellationToken = default)
    {
        _logger.LogInformation("Applying adjudication rules to item sequence {ItemSequence}", item.Sequence);

        var result = new AdjudicationRuleResult
        {
            RuleApplied = true,
    AdjudicationStatus = "approved",
       AdjustedAmount = item.Net ?? item.UnitPrice ?? 0m,
    DecisionReason = "Item approved after rule evaluation",
     AppliedRules = new List<string>()
        };

        try
     {
     // Rule 1: Check if service is covered
         if (!IsServiceCovered(item))
      {
   result.AdjudicationStatus = "denied";
   result.DecisionReason = "Service is not covered under plan";
            result.AdjustedAmount = 0m;
      result.AppliedRules.Add("SERVICE_NOT_COVERED");

     _logger.LogWarning("Service not covered for item sequence {ItemSequence}", item.Sequence);
        return result;
            }

 result.AppliedRules.Add("SERVICE_COVERAGE_VERIFIED");

      // Rule 2: Check pre-authorization if required
      if (RequiresPreAuthorization(item) && !context.HasPreAuthorization)
   {
 result.AdjudicationStatus = "denied";
   result.DecisionReason = "Pre-authorization required but not provided";
        result.AdjustedAmount = 0m;
       result.AppliedRules.Add("PRE_AUTH_REQUIRED");

  _logger.LogWarning("Pre-authorization required for item sequence {ItemSequence}", item.Sequence);
  return result;
}

 if (context.HasPreAuthorization)
       result.AppliedRules.Add("PRE_AUTH_VERIFIED");

  // Rule 3: Check network status
    if (!context.IsNetworkProvider)
   {
   var outOfNetworkPercent = 0.80m; // 80% of allowed
 result.AdjustedAmount = (item.Net ?? item.UnitPrice ?? 0m) * outOfNetworkPercent;
        result.DecisionReason = "Approved at out-of-network rate (80%)";
     result.AppliedRules.Add("OUT_OF_NETWORK_APPLIED");

     _logger.LogInformation("Out-of-network adjustment applied for item sequence {ItemSequence}. " +
        "Original: {Original}, Adjusted: {Adjusted}",
       item.Sequence, item.Net ?? item.UnitPrice, result.AdjustedAmount);
        }
        else
{
     result.AppliedRules.Add("IN_NETWORK_VERIFIED");
  }

          // Rule 4: Check frequency limitations
  if (ExceedsFrequencyLimit(item))
     {
   result.AdjudicationStatus = "denied";
         result.DecisionReason = "Annual frequency limit exceeded";
          result.AdjustedAmount = 0m;
      result.AppliedRules.Add("FREQUENCY_LIMIT_EXCEEDED");

      _logger.LogWarning("Frequency limit exceeded for item sequence {ItemSequence}", item.Sequence);
        return result;
 }

  result.AppliedRules.Add("FREQUENCY_LIMIT_VERIFIED");

      // Rule 5: Check for bundled services
   if (IsBundledService(item))
   {
        var bundlePercent = 0.50m; // Bundled items get 50% payment
  result.AdjustedAmount = (item.Net ?? item.UnitPrice ?? 0m) * bundlePercent;
           result.DecisionReason = "Approved at bundled rate (50%)";
    result.AppliedRules.Add("BUNDLED_SERVICE_APPLIED");

          _logger.LogInformation("Bundled service adjustment applied for item sequence {ItemSequence}. " +
          "Adjusted to: {Adjusted}",
             item.Sequence, result.AdjustedAmount);
        }
else
            {
    result.AppliedRules.Add("STANDALONE_SERVICE_VERIFIED");
       }

       _logger.LogInformation("Adjudication rules applied for item sequence {ItemSequence}. " +
     "Status: {Status}, Adjusted Amount: {Amount}, Rules Applied: {RuleCount}",
item.Sequence, result.AdjudicationStatus, result.AdjustedAmount, result.AppliedRules.Count);

   return result;
        }
catch (Exception ex)
        {
 _logger.LogError(ex, "Error applying adjudication rules to item sequence {ItemSequence}", item.Sequence);
    throw;
        }
  }

    /// <summary>
    /// Generate adjudication narrative/explanation
    /// </summary>
    public async Task<string> GenerateAdjudicationNarrativeAsync(
        ClaimResponse response,
  List<AdjudicationDetailDto> details,
        CancellationToken cancellationToken = default)
    {
        _logger.LogInformation("Generating adjudication narrative for response ID {ResponseId}", response.Id);

    try
     {
            var narrative = new System.Text.StringBuilder();

            // Header
    narrative.AppendLine($"CLAIM ADJUDICATION NARRATIVE");
            narrative.AppendLine($"Claim ID: {response.ClaimId}");
  narrative.AppendLine($"Response ID: {response.Id}");
          narrative.AppendLine($"Adjudication Date: {DateTime.UtcNow:yyyy-MM-dd}");
            narrative.AppendLine();

  // Summary
   var approved = details.Count(d => d.Status == "approved");
         var denied = details.Count(d => d.Status == "denied");
            var pending = details.Count(d => d.Status == "pending" || d.Status == "pended");

            narrative.AppendLine("SUMMARY:");
   narrative.AppendLine($"  Total Items: {details.Count}");
            narrative.AppendLine($"  Approved: {approved} items");
    narrative.AppendLine($"  Denied: {denied} items");
 narrative.AppendLine($"  Pending: {pending} items");
       narrative.AppendLine();

            // Financial Summary
          var totalApproved = details.Where(d => d.Status == "approved").Sum(d => d.InsuranceResponsibility);
     var totalDenied = details.Where(d => d.Status == "denied").Sum(d => d.SubmittedAmount);
      var totalPatient = details.Sum(d => d.PatientResponsibility);

narrative.AppendLine("FINANCIAL SUMMARY:");
            narrative.AppendLine($"  Total Approved Amount: {totalApproved:C}");
narrative.AppendLine($"  Total Denied Amount: {totalDenied:C}");
            narrative.AppendLine($"  Total Patient Responsibility: {totalPatient:C}");
            narrative.AppendLine();

  // Itemized Details
       narrative.AppendLine("ITEMIZED ADJUDICATIONS:");
            foreach (var detail in details)
            {
             narrative.AppendLine($"  Item {detail.ItemSequence}: {detail.ServiceDescription}");
          narrative.AppendLine($"    Submitted: {detail.SubmittedAmount:C}");
      narrative.AppendLine($"    Allowed: {detail.AllowedAmount:C}");
  narrative.AppendLine($"    Status: {detail.Status.ToUpper()}");
         narrative.AppendLine($"    Insurance Responsibility: {detail.InsuranceResponsibility:C}");
                narrative.AppendLine($"    Patient Responsibility: {detail.PatientResponsibility:C}");

                if (detail.DeductibleApplied > 0)
         narrative.AppendLine($"    Deductible Applied: {detail.DeductibleApplied:C}");
             if (detail.CoinsuranceApplied > 0)
    narrative.AppendLine($"    Coinsurance Applied: {detail.CoinsuranceApplied:C}");
      if (detail.OutOfPocketApplied > 0)
        narrative.AppendLine($"    Out-of-Pocket Applied: {detail.OutOfPocketApplied:C}");

                narrative.AppendLine();
            }

   narrative.AppendLine("END OF ADJUDICATION NARRATIVE");

    var narrativeText = narrative.ToString();

       _logger.LogInformation("Adjudication narrative generated with {Length} characters", narrativeText.Length);
            return narrativeText;
     }
        catch (Exception ex)
    {
   _logger.LogError(ex, "Error generating adjudication narrative");
     throw;
   }
    }

    /// <summary>
    /// Calculate appeal deadlines
    /// </summary>
    public async Task<AppealDeadlines> CalculateAppealDeadlinesAsync(
        ClaimResponse response,
     CancellationToken cancellationToken = default)
    {
        _logger.LogInformation("Calculating appeal deadlines for response ID {ResponseId}", response.Id);

     try
        {
            var responseDate = response.ClaimResponseStatus == "active" ? DateTime.UtcNow : DateTime.UtcNow;
     var standardAppealWindow = 60; // days
        var secondAppealWindow = 30; // days after first appeal

            var deadlines = new AppealDeadlines
            {
       ClaimId = response.ClaimId,
          AppealDeadline = responseDate.AddDays(standardAppealWindow),
                FirstLevelAppealDeadline = responseDate.AddDays(standardAppealWindow),
    SecondLevelAppealDeadline = responseDate.AddDays(standardAppealWindow + secondAppealWindow),
       DaysRemainingToAppeal = standardAppealWindow,
  CanAppeal = true,
                AppealInstructions = GenerateAppealInstructions()
       };

       _logger.LogInformation("Appeal deadlines calculated for claim ID {ClaimId}. " +
                "First Level Deadline: {FirstLevel}, Second Level Deadline: {SecondLevel}",
       response.ClaimId, deadlines.FirstLevelAppealDeadline.Date, deadlines.SecondLevelAppealDeadline.Date);

            return deadlines;
}
        catch (Exception ex)
 {
            _logger.LogError(ex, "Error calculating appeal deadlines");
          throw;
        }
    }

 /// <summary>
  /// Generate remittance advice
    /// </summary>
  public async Task<RemittanceAdvice> GenerateRemittanceAdviceAsync(
  ClaimResponse response,
        Claim claim,
        CancellationToken cancellationToken = default)
    {
    _logger.LogInformation("Generating remittance advice for claim ID {ClaimId}", claim.Id);

        try
        {
  // Extract adjudication details
            var adjDetails = await _claimResponseProcessor.ExtractAdjudicationDetailsAsync(response, cancellationToken);

            // Create remittance advice
            var remittance = new RemittanceAdvice
     {
        ClaimId = claim.Id,
                RemittanceNumber = GenerateRemittanceNumber(),
    RemittanceDate = DateTime.UtcNow,
      InsurerName = response.InsurerId ?? "Unknown Insurer",
  ProviderName = claim.ProviderId ?? "Unknown Provider",
       PatientName = response.PatientId ?? "Unknown Patient",
        PatientId = response.PatientId ?? string.Empty,
                ServiceDate = claim.ServicedPeriodStart ?? claim.CreatedDate,
             TotalSubmitted = adjDetails.Sum(d => d.SubmittedAmount),
                TotalAllowed = adjDetails.Sum(d => d.AllowedAmount),
                TotalInsurancePays = adjDetails.Where(d => d.Status == "approved").Sum(d => d.InsuranceResponsibility),
           TotalPatientResponsibility = adjDetails.Sum(d => d.PatientResponsibility),
    LineItems = new List<RemittanceLineItem>()
          };

  // Billable and non-billable services
  var billableServices = new[] { "99213", "99214", "99215", "90834" }; // Example codes

  // Build line items
foreach (var detail in adjDetails.OrderBy(d => d.ItemSequence))
   {
 remittance.LineItems.Add(new RemittanceLineItem
       {
   Sequence = detail.ItemSequence,
   ServiceCode = "SERVICE",
  Description = detail.ServiceDescription,
     SubmittedAmount = detail.SubmittedAmount,
         AllowedAmount = detail.AllowedAmount,
  InsurancePays = detail.Status == "approved" ? detail.InsuranceResponsibility : 0m,
    PatientResponsibility = detail.PatientResponsibility,
       Status = detail.Status.ToUpper()
     });
     }

          // Add notes
   var deniedCount = adjDetails.Count(d => d.Status == "denied");
            var pendingCount = adjDetails.Count(d => d.Status == "pending");

            var notes = new System.Text.StringBuilder();
      notes.AppendLine("Thank you for submitting this claim.");
            if (deniedCount > 0)
                notes.AppendLine($"{deniedCount} item(s) were denied. Please review the detailed explanations below.");
     if (pendingCount > 0)
                notes.AppendLine($"{pendingCount} item(s) are pending further review.");
    notes.AppendLine("If you have any questions, please contact us at 1-800-INSURANCE or visit our website.");

     remittance.Notes = notes.ToString();

  _logger.LogInformation("Remittance advice generated for claim ID {ClaimId}. " +
              "Items: {ItemCount}, Total Insurance Pays: {TotalPays}",
claim.Id, remittance.LineItems.Count, remittance.TotalInsurancePays);

   return remittance;
      }
catch (Exception ex)
     {
      _logger.LogError(ex, "Error generating remittance advice");
        throw;
     }
    }

    #region Helper Methods

    /// <summary>
    /// Check if provider is in-network
    /// </summary>
    private bool IsNetworkProvider(Claim claim)
    {
        // This would typically check a provider network database
        // For now, assume all providers in the system are in-network
        return claim.ProviderId != null && claim.ProviderId.StartsWith("PROV");
    }

    /// <summary>
    /// Check if response has pre-authorization
    /// </summary>
    private bool HasPreAuthorization(ClaimResponse response)
    {
        return !string.IsNullOrEmpty(response.PreAuthRef);
    }

    /// <summary>
    /// Extract pre-auth number from response
    /// </summary>
    private string ExtractPreAuthNumber(ClaimResponse response)
    {
  return response.PreAuthRef ?? string.Empty;
    }

    /// <summary>
/// Check if service is covered
    /// </summary>
    private bool IsServiceCovered(ClaimItem item)
    {
        // This would typically check a coverage ruleset
        // For now, assume all services are covered unless flagged
 return item.ProductOrServiceCode != null;
    }

    /// <summary>
    /// Check if service requires pre-authorization
    /// </summary>
    private bool RequiresPreAuthorization(ClaimItem item)
    {
        // Major procedures, specialty care, etc.
 var preAuthCodes = new[] { "99213", "99214", "99215", "90834" }; // Example codes
     return preAuthCodes.Contains(item.ProductOrServiceCode ?? string.Empty);
    }

    /// <summary>
    /// Check if item exceeds frequency limit
    /// </summary>
    private bool ExceedsFrequencyLimit(ClaimItem item)
    {
        // This would check against plan rules
        // For now, assume no frequency limit exceeded
        return false;
    }

    /// <summary>
    /// Check if service is bundled
    /// </summary>
    private bool IsBundledService(ClaimItem item)
{
        // Check if service is part of a bundle
        var bundledCodes = new[] { "99211", "99212" }; // Example bundled codes
        return bundledCodes.Contains(item.ProductOrServiceCode ?? string.Empty);
    }

    /// <summary>
    /// Generate appeal instructions
    /// </summary>
    private string GenerateAppealInstructions()
    {
        return @"APPEAL INSTRUCTIONS:
If you disagree with this adjudication decision, you have the right to appeal.

To appeal:
1. Contact us within 60 days of receiving this notice
2. Provide the claim number and specific reason for appeal
3. Include any supporting documentation
4. Submit to: Appeals Department, [Address], [Phone], [Email]

Appeal levels:
- First Level Appeal: 60 days from receipt
- Second Level Appeal: 30 days after first level decision
- External Review: Available for certain claim types";
    }

 /// <summary>
    /// Generate unique remittance number
    /// </summary>
    private string GenerateRemittanceNumber()
    {
        return $"RA-{DateTime.UtcNow:yyyyMMddHHmmss}-{Guid.NewGuid().ToString("N").Substring(0, 8)}";
    }

    #endregion
}
