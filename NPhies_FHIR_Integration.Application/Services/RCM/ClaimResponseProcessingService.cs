using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using NPhies_FHIR_Integration.Domain.Entities;
using NPhies_FHIR_Integration.Infrastructure.Repositories;

namespace NPhies_FHIR_Integration.Application.Services.RCM;

/// <summary>
/// ClaimResponse Processing Service Implementation
/// Processes claim responses from payers
/// Handles adjudication, denials, and financial calculations
/// </summary>
public class ClaimResponseProcessingService : IClaimResponseProcessingService
{
    private readonly ILogger<ClaimResponseProcessingService> _logger;
    private readonly IClaimResponseRepository _claimResponseRepository;
    private readonly IClaimRepository _claimRepository;

    public ClaimResponseProcessingService(
        ILogger<ClaimResponseProcessingService> logger,
        IClaimResponseRepository claimResponseRepository = null,
      IClaimRepository claimRepository = null)
    {
    _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        _claimResponseRepository = claimResponseRepository;
        _claimRepository = claimRepository;
    }

    /// <summary>
    /// Process incoming claim response
    /// Handles adjudication, denial identification, and financial calculations
    /// </summary>
    public async Task<ClaimResponseProcessingResult> ProcessClaimResponseAsync(
 ClaimResponse response,
     Claim originalClaim,
      CancellationToken cancellationToken = default)
    {
        _logger.LogInformation("Processing claim response for claim ID {ClaimId}", response.ClaimId);

        var result = new ClaimResponseProcessingResult
        {
 ClaimId = response.ClaimId,
         ResponseId = response.Id,
      IsSuccessful = false,
            StatusMessage = "Processing started",
      ProcessedAt = DateTime.UtcNow
        };

    try
        {
            // Step 1: Get original claim from database if not provided
 if (originalClaim == null && _claimRepository != null)
 {
   originalClaim = await _claimRepository.GetWithDetailsAsync(response.ClaimId);
             if (originalClaim == null)
    {
        _logger.LogWarning("Original claim {ClaimId} not found in database", response.ClaimId);
        result.IsSuccessful = false;
              result.StatusMessage = "Original claim not found";
   result.Errors.Add($"Claim {response.ClaimId} not found");
   return result;
             }
            }

            // Step 2: Extract adjudication details
       var adjudicationDetails = await ExtractAdjudicationDetailsAsync(response, cancellationToken);
            _logger.LogInformation("Extracted {Count} adjudication details for claim {ClaimId}",
         adjudicationDetails.Count, response.ClaimId);

 // Step 3: Identify approved and denied items
      var approvedItems = adjudicationDetails.Where(d => d.Status == "approved").ToList();
            var deniedItems = await IdentifyDeniedItemsAsync(response, cancellationToken);
    var pendingItems = adjudicationDetails.Where(d => d.Status == "pending" || d.Status == "pended").ToList();

  result.ApprovedItemCount = approvedItems.Count;
     result.DeniedItemCount = deniedItems.Count;
            result.PendingItemCount = pendingItems.Count;

            _logger.LogInformation("Claim response breakdown - Approved: {Approved}, Denied: {Denied}, Pending: {Pending}",
                result.ApprovedItemCount, result.DeniedItemCount, result.PendingItemCount);

// Step 4: Calculate totals from ClaimResponseTotal entities
     if (response.Totals != null && response.Totals.Any())
            {
           var submittedTotal = response.Totals.FirstOrDefault(t => t.Category == "submitted");
    var approvedTotal = response.Totals.FirstOrDefault(t => t.Category == "benefit" || t.Category == "approved");
             var deniedTotal = response.Totals.FirstOrDefault(t => t.Category == "denied");

                result.TotalApprovedAmount = approvedTotal?.Amount ?? approvedItems.Sum(a => a.InsuranceResponsibility);
  result.TotalDeniedAmount = deniedTotal?.Amount ?? deniedItems.Sum(d => d.DeniedAmount);
            }
 else
 {
     // Calculate from adjudication details if totals not available
             result.TotalApprovedAmount = approvedItems.Sum(a => a.InsuranceResponsibility);
                result.TotalDeniedAmount = deniedItems.Sum(d => d.DeniedAmount);
  }

            // Step 5: Get coverage to calculate patient responsibility
          var coverage = originalClaim?.Coverage;
PatientResponsibilityResult patientResp = null;

            if (coverage != null)
       {
                patientResp = await CalculatePatientResponsibilityAsync(response, coverage, cancellationToken);
      result.TotalPatientResponsibility = patientResp.TotalResponsibility;
    }
   else
      {
// Fallback: use sum of patient responsibility from adjudication details
        result.TotalPatientResponsibility = adjudicationDetails.Sum(a => a.PatientResponsibility);
     }

  _logger.LogInformation("Totals calculated - Approved: {Approved}, Denied: {Denied}, Patient: {Patient}",
     result.TotalApprovedAmount, result.TotalDeniedAmount, result.TotalPatientResponsibility);

        // Step 6: Generate RCM summary
            var summary = await GenerateRCMSummaryAsync(response, originalClaim, cancellationToken);

  // Step 7: Save response to database
            if (_claimResponseRepository != null)
        {
                response.ClaimResponseStatus = "processed";
   await _claimResponseRepository.AddAsync(response);
       await _claimResponseRepository.SaveChangesAsync();
                _logger.LogInformation("Claim response saved to database for claim {ClaimId}", response.ClaimId);
            }

            // Step 8: Mark as successful
     result.IsSuccessful = true;
result.StatusMessage = "Processing completed successfully";

      _logger.LogInformation("Claim response processed successfully for claim ID {ClaimId}. " +
"Approved: {Approved}, Denied: {Denied}, Patient Responsibility: {Patient}",
      response.ClaimId, result.TotalApprovedAmount, result.TotalDeniedAmount, result.TotalPatientResponsibility);

     return result;
        }
    catch (Exception ex)
 {
        _logger.LogError(ex, "Error processing claim response for claim ID {ClaimId}", response.ClaimId);
  result.IsSuccessful = false;
            result.StatusMessage = $"Processing failed: {ex.Message}";
      result.Errors.Add(ex.Message);
   return result;
      }
    }

    /// <summary>
  /// Extract adjudication details from response
    /// </summary>
    public async Task<List<AdjudicationDetailDto>> ExtractAdjudicationDetailsAsync(
  ClaimResponse response,
  CancellationToken cancellationToken = default)
    {
        _logger.LogInformation("Extracting adjudication details for response ID {ResponseId}", response.Id);

        var details = new List<AdjudicationDetailDto>();

   try
        {
            // Extract from ClaimResponseAddItems which contain adjudication information
    if (response.AddItems == null || !response.AddItems.Any())
            {
           _logger.LogWarning("No add items found in response ID {ResponseId}", response.Id);
                return details;
      }

            foreach (var addItem in response.AddItems)
        {
           var detail = new AdjudicationDetailDto
    {
ItemSequence = addItem.Sequence,
            ServiceDescription = addItem.ProductOrServiceDisplay ?? addItem.ProductOrServiceCode ?? "Unknown Service",
           SubmittedAmount = addItem.SubmittedAmount ?? 0m,
     AllowedAmount = addItem.BenefitAmount ?? addItem.SubmittedAmount ?? 0m,
       InsuranceResponsibility = addItem.BenefitAmount ?? 0m
              };

       // Determine status based on adjudications
 if (addItem.Adjudications != null && addItem.Adjudications.Any())
    {
         // Check for denial
          var denialAdj = addItem.Adjudications.FirstOrDefault(a =>
     a.AdjudicationCategory?.ToLower().Contains("deny") == true ||
            a.AdjudicationCategory?.ToLower().Contains("denied") == true);

      if (denialAdj != null)
    {
    detail.Status = "denied";
        }
            else
        {
  // Check for pending
            var pendingAdj = addItem.Adjudications.FirstOrDefault(a =>
          a.AdjudicationCategory?.ToLower().Contains("pend") == true);

     detail.Status = pendingAdj != null ? "pending" : "approved";
            }

          // Extract amounts from adjudications
       foreach (var adj in addItem.Adjudications)
      {
      if (adj.AdjudicationCategory?.ToLower().Contains("deductible") == true)
         {
  detail.DeductibleApplied = adj.Amount ?? 0m;
         }
        else if (adj.AdjudicationCategory?.ToLower().Contains("coinsurance") == true)
             {
                  detail.CoinsuranceApplied = adj.Amount ?? 0m;
}
      else if (adj.AdjudicationCategory?.ToLower().Contains("copay") == true)
       {
         detail.OutOfPocketApplied = adj.Amount ?? 0m;
                 }
     }
         }
        else
       {
             detail.Status = "approved"; // Default to approved if no adjudications
    }

      // Calculate patient responsibility
                detail.PatientResponsibility = detail.DeductibleApplied + detail.CoinsuranceApplied + detail.OutOfPocketApplied;

       details.Add(detail);
   }

      _logger.LogInformation("Extracted {Count} adjudication details from {ItemCount} items",
   details.Count, response.AddItems.Count);

         return details;
        }
     catch (Exception ex)
        {
 _logger.LogError(ex, "Error extracting adjudication details from response ID {ResponseId}", response.Id);
            throw;
        }
    }

    /// <summary>
 /// Calculate patient responsibility from response
 /// </summary>
    public async Task<PatientResponsibilityResult> CalculatePatientResponsibilityAsync(
        ClaimResponse response,
    Coverage coverage,
     CancellationToken cancellationToken = default)
    {
        _logger.LogInformation("Calculating patient responsibility for claim ID {ClaimId}", response.ClaimId);

        var result = new PatientResponsibilityResult
        {
            TotalResponsibility = 0,
        DeductibleAmount = 0,
   CoinsuranceAmount = 0,
  OutOfPocketAmount = 0,
         OtherAmount = 0,
          IsResponsibilityMet = false,
            Notes = string.Empty
    };

        try
        {
            // Get adjudication details
            var adjDetails = await ExtractAdjudicationDetailsAsync(response, cancellationToken);

    // Sum patient responsibility components
            result.DeductibleAmount = adjDetails.Sum(d => d.DeductibleApplied);
     result.CoinsuranceAmount = adjDetails.Sum(d => d.CoinsuranceApplied);
   result.OutOfPocketAmount = adjDetails.Sum(d => d.OutOfPocketApplied);

            // Total patient responsibility
  result.TotalResponsibility = result.DeductibleAmount + result.CoinsuranceAmount + result.OutOfPocketAmount;

  // Check if patient responsibility has been met based on coverage
            if (coverage != null)
          {
    var remainingDeductible = Math.Max(0, coverage.AnnualDeductible - coverage.DeductibleMet);
     var remainingOOP = Math.Max(0, coverage.OutOfPocketMax - coverage.DeductibleMet); // Simplified - would need actual OOP met

      result.IsResponsibilityMet = result.TotalResponsibility >= (remainingDeductible + remainingOOP);

       _logger.LogInformation("Patient responsibility: Deductible={Ded}, Coinsurance={Coin}, OOP={OOP}, Total={Total}. Met={IsMet}",
          result.DeductibleAmount, result.CoinsuranceAmount, result.OutOfPocketAmount,
         result.TotalResponsibility, result.IsResponsibilityMet);
     }

         return result;
        }
  catch (Exception ex)
        {
            _logger.LogError(ex, "Error calculating patient responsibility for claim ID {ClaimId}", response.ClaimId);
    throw;
        }
    }

    /// <summary>
    /// Identify denied claim items
    /// </summary>
    public async Task<List<DeniedItemDetail>> IdentifyDeniedItemsAsync(
        ClaimResponse response,
        CancellationToken cancellationToken = default)
    {
        _logger.LogInformation("Identifying denied items for response ID {ResponseId}", response.Id);

        var deniedItems = new List<DeniedItemDetail>();

     try
{
            // Get adjudication details
  var adjDetails = await ExtractAdjudicationDetailsAsync(response, cancellationToken);

            // Filter for denied items
      var deniedDetails = adjDetails.Where(d => d.Status == "denied").ToList();

       if (!deniedDetails.Any())
 {
   _logger.LogInformation("No denied items found for response ID {ResponseId}", response.Id);
    return deniedItems;
          }

         // Map to DeniedItemDetail
        foreach (var deniedDetail in deniedDetails)
  {
    var addItem = response.AddItems?.FirstOrDefault(ai => ai.Sequence == deniedDetail.ItemSequence);

      var denialDetail = new DeniedItemDetail
      {
          ItemSequence = deniedDetail.ItemSequence,
         ServiceDescription = deniedDetail.ServiceDescription,
          DeniedAmount = deniedDetail.SubmittedAmount - deniedDetail.AllowedAmount,
    CanAppeal = true, // Most denials can be appealed
    AppealDeadline = DateTime.UtcNow.AddDays(60), // Standard 60-day appeal window
              IsRecoverable = true // Assume recoverable unless marked otherwise
    };

          // Extract denial reason from adjudications if available
     if (addItem?.Adjudications != null)
           {
      var denialAdj = addItem.Adjudications.FirstOrDefault(a =>
    a.AdjudicationCategory?.ToLower().Contains("deny") == true);

        if (denialAdj != null)
           {
    denialDetail.DenialReasonCode = denialAdj.AdjudicationCategory ?? "UNKNOWN";
    denialDetail.DenialReason = denialAdj.Notes ?? denialAdj.AdjudicationDisplay ?? "No reason provided";
             }
             }

      deniedItems.Add(denialDetail);
   }

            _logger.LogInformation("Found {Count} denied items for response ID {ResponseId}. Total denied amount: {TotalDenied}",
           deniedItems.Count, response.Id, deniedItems.Sum(d => d.DeniedAmount));

            return deniedItems;
        }
        catch (Exception ex)
     {
            _logger.LogError(ex, "Error identifying denied items for response ID {ResponseId}", response.Id);
            throw;
        }
    }

    /// <summary>
    /// Generate RCM summary from response
    /// </summary>
    public async Task<RCMSummary> GenerateRCMSummaryAsync(
      ClaimResponse response,
        Claim originalClaim,
        CancellationToken cancellationToken = default)
    {
   _logger.LogInformation("Generating RCM summary for claim ID {ClaimId}", response.ClaimId);

        var summary = new RCMSummary
        {
  ClaimId = response.ClaimId,
       ResponseId = response.Id,
  ProcessingStatus = "generated",
         GeneratedAt = DateTime.UtcNow
        };

        try
        {
      // Get adjudication details
            var adjDetails = await ExtractAdjudicationDetailsAsync(response, cancellationToken);
     var deniedItems = await IdentifyDeniedItemsAsync(response, cancellationToken);

     // Count items by status
       var approvedItems = adjDetails.Where(d => d.Status == "approved").ToList();
    var pendingItems = adjDetails.Where(d => d.Status == "pending" || d.Status == "pended").ToList();

  summary.ApprovedItemCount = approvedItems.Count;
            summary.DeniedItemCount = deniedItems.Count;
 summary.PendingItemCount = pendingItems.Count;

            // Calculate financial totals
 summary.TotalSubmittedAmount = adjDetails.Sum(d => d.SubmittedAmount);
            summary.TotalAllowedAmount = adjDetails.Sum(d => d.AllowedAmount);
    summary.TotalApprovedAmount = approvedItems.Sum(d => d.InsuranceResponsibility);
       summary.TotalDeniedAmount = deniedItems.Sum(d => d.DeniedAmount);
summary.TotalInsuranceResponsibility = approvedItems.Sum(d => d.InsuranceResponsibility);
            summary.TotalPatientResponsibility = adjDetails.Sum(d => d.PatientResponsibility);

     _logger.LogInformation("RCM summary generated for claim ID {ClaimId}. " +
                "Submitted: {Submitted}, Allowed: {Allowed}, Approved: {Approved}, Denied: {Denied}, " +
   "Insurance: {Insurance}, Patient: {Patient}",
  response.ClaimId,
        summary.TotalSubmittedAmount,
   summary.TotalAllowedAmount,
          summary.TotalApprovedAmount,
     summary.TotalDeniedAmount,
     summary.TotalInsuranceResponsibility,
  summary.TotalPatientResponsibility);

     return summary;
     }
        catch (Exception ex)
     {
            _logger.LogError(ex, "Error generating RCM summary for claim ID {ClaimId}", response.ClaimId);
    throw;
  }
    }
}
