using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using NPhies_FHIR_Integration.Domain.Entities;

namespace NPhies_FHIR_Integration.Application.Services.RCM;

/// <summary>
/// Payment Reconciliation Service Implementation
/// Handles payment matching, reconciliation, and discrepancy detection
/// </summary>
public class PaymentReconciliationService : IPaymentReconciliationService
{
 private readonly ILogger<PaymentReconciliationService> _logger;

    public PaymentReconciliationService(ILogger<PaymentReconciliationService> logger)
    {
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
    }

    /// <summary>
    /// Reconcile payment from remittance advice
    /// </summary>
public async Task<ReconciliationResult> ReconcilePaymentAsync(
     ClaimResponse response,
        PaymentNotice paymentNotice,
        CancellationToken cancellationToken = default)
    {
        _logger.LogInformation("Reconciling payment for claim ID {ClaimId}", response.ClaimId);

        var result = new ReconciliationResult
        {
   ReconciliationId = GenerateReconciliationId(),
       ClaimId = response.ClaimId,
IsSuccessful = true,
            ReconciliationDate = DateTime.UtcNow
      };

        try
        {
      // Calculate expected payment from ClaimResponse
            var expectedAmount = response.Totals?.Sum(t => t.Amount) ?? 0;

            // Get actual payment from PaymentNotice
            var actualAmount = paymentNotice?.TotalAmount ?? 0;

            // Calculate variance
       var variance = actualAmount - expectedAmount;
            var variancePercentage = expectedAmount > 0 ? (variance / expectedAmount) * 100 : 0;

     result.ExpectedAmount = expectedAmount;
    result.ActualAmount = actualAmount;
  result.Variance = variance;
       result.VariancePercentage = variancePercentage;

            // Determine status
if (Math.Abs(variance) < 0.01m)  // Within $0.01 tolerance
            {
     result.Status = "Reconciled";
     }
    else if (variance > 0)
   {
result.Status = "Overpayment";
      result.Discrepancies.Add($"Overpayment of ${Math.Abs(variance):F2}");
            }
       else
            {
     result.Status = "Underpayment";
        result.Discrepancies.Add($"Underpayment of ${Math.Abs(variance):F2}");
      }

          _logger.LogInformation("Payment reconciled for claim {ClaimId}. Expected: {Expected}, Actual: {Actual}, Variance: {Variance}",
      response.ClaimId, expectedAmount, actualAmount, variance);

            return result;
        }
    catch (Exception ex)
      {
     _logger.LogError(ex, "Error reconciling payment");
       result.IsSuccessful = false;
     result.Status = "Error";
   result.Discrepancies.Add(ex.Message);
            return result;
        }
    }

    /// <summary>
/// Match payment to claims
    /// </summary>
  public async Task<PaymentMatchResult> MatchPaymentToClaimAsync(
   Payment payment,
        List<Claim> potentialClaims,
        CancellationToken cancellationToken = default)
    {
   _logger.LogInformation("Matching payment ${Amount} to potential claims", payment.Amount);

        var result = new PaymentMatchResult
        {
            IsMatched = false,
  MatchConfidence = 0
        };

        try
        {
            if (potentialClaims == null || potentialClaims.Count == 0)
          return result;

    // Strategy 1: Exact match by ClaimId (convert to string for comparison)
            if (payment.ClaimId.HasValue)
            {
           var claimIdStr = payment.ClaimId.Value.ToString();
      var exactMatch = potentialClaims.FirstOrDefault(c => c.Id == claimIdStr);
         if (exactMatch != null)
        {
    result.IsMatched = true;
 result.MatchedClaimId = int.Parse(exactMatch.Id);
           result.MatchConfidence = 100m;
     result.MatchReason = "Exact match by Claim ID";
           _logger.LogInformation("Exact match found for payment ${Amount}: Claim {ClaimId}",
   payment.Amount, exactMatch.Id);
     return result;
         }
     }

    // Strategy 2: Fuzzy match by amount (within 1%)
 var amountMatches = potentialClaims
   .Where(c => Math.Abs(c.Total - payment.Amount) <= c.Total * 0.01m)
      .ToList();

       if (amountMatches.Count == 1)
   {
                var fuzzyMatch = amountMatches.First();
             result.IsMatched = true;
 result.MatchedClaimId = int.Parse(fuzzyMatch.Id);
        result.MatchConfidence = 85m;
      result.MatchReason = "Fuzzy match by amount (±1%)";
    _logger.LogInformation("Fuzzy match found for payment ${Amount}: Claim {ClaimId}",
        payment.Amount, fuzzyMatch.Id);
   return result;
            }
            else if (amountMatches.Count > 1)
     {
 // Multiple potential matches
                result.AlternativeMatches = amountMatches.Select(c => int.Parse(c.Id)).ToList();
             result.MatchConfidence = 70m;
       result.MatchReason = "Multiple potential matches found";
            }

   // Strategy 3: Reference match
 if (!string.IsNullOrEmpty(payment.ReferenceNumber))
            {
      var referenceMatch = potentialClaims.FirstOrDefault(c =>
    c.ClaimNumber.Contains(payment.ReferenceNumber) ||
                    payment.ReferenceNumber.Contains(c.ClaimNumber));

     if (referenceMatch != null)
    {
       result.IsMatched = true;
  result.MatchedClaimId = int.Parse(referenceMatch.Id);
         result.MatchConfidence = 95m;
                result.MatchReason = "Match by reference number";
         _logger.LogInformation("Reference match found for payment: Claim {ClaimId}", referenceMatch.Id);
         return result;
              }
        }

            // Strategy 4: Provider match
   var providerMatches = potentialClaims
  .Where(c => c.ProviderId == payment.ProviderId)
    .ToList();

          if (providerMatches.Count == 1 &&
      Math.Abs(providerMatches[0].Total - payment.Amount) <= 0.01m)
   {
     var providerMatch = providerMatches.First();
      result.IsMatched = true;
      result.MatchedClaimId = int.Parse(providerMatch.Id);
          result.MatchConfidence = 70m;
     result.MatchReason = "Match by provider and amount";
_logger.LogInformation("Provider match found for payment: Claim {ClaimId}", providerMatch.Id);
       return result;
            }

    return result;
      }
        catch (Exception ex)
        {
_logger.LogError(ex, "Error matching payment");
      throw;
        }
    }

    /// <summary>
    /// Identify payment discrepancies
    /// </summary>
    public async Task<List<PaymentDiscrepancy>> IdentifyDiscrepanciesAsync(
        List<Payment> payments,
        List<Claim> claims,
        CancellationToken cancellationToken = default)
    {
        _logger.LogInformation("Identifying discrepancies between {PaymentCount} payments and {ClaimCount} claims",
        payments?.Count ?? 0, claims?.Count ?? 0);

        var discrepancies = new List<PaymentDiscrepancy>();

        try
 {
            if (payments == null || claims == null || payments.Count == 0 || claims.Count == 0)
        return discrepancies;

         // Match all payments to claims
    foreach (var payment in payments)
     {
    var matchResult = await MatchPaymentToClaimAsync(payment, claims, cancellationToken);

            if (!matchResult.IsMatched)
              {
   // Unmatched payment
           discrepancies.Add(new PaymentDiscrepancy
           {
 ClaimId = "UNMATCHED",
          DiscrepancyType = "Unmatched Payment",
    ExpectedAmount = 0,
  ActualAmount = payment.Amount,
    Variance = payment.Amount,
              Description = $"Payment of ${payment.Amount:F2} could not be matched to any claim",
       Severity = "High",
             RecommendedAction = "Investigate payment and contact payer if necessary"
        });
 continue;
                }

          // Get matched claim
 var matchedClaim = claims.FirstOrDefault(c => int.Parse(c.Id) == matchResult.MatchedClaimId);
      if (matchedClaim == null)
         continue;

              var expectedAmount = matchedClaim.Total;
    var actualAmount = payment.Amount;
                var variance = actualAmount - expectedAmount;

     // Check for overpayment
            if (variance > 0.01m)
                {
        discrepancies.Add(new PaymentDiscrepancy
{
            ClaimId = matchedClaim.ClaimNumber,
       DiscrepancyType = "Overpayment",
              ExpectedAmount = expectedAmount,
 ActualAmount = actualAmount,
    Variance = variance,
      Description = $"Overpayment of ${variance:F2} (Expected: ${expectedAmount:F2}, Received: ${actualAmount:F2})",
  Severity = "Medium",
        RecommendedAction = "Review and file for return of overpayment"
   });
      }
  // Check for underpayment
           else if (variance < -0.01m)
            {
       discrepancies.Add(new PaymentDiscrepancy
  {
      ClaimId = matchedClaim.ClaimNumber,
         DiscrepancyType = "Underpayment",
    ExpectedAmount = expectedAmount,
   ActualAmount = actualAmount,
        Variance = variance,
                Description = $"Underpayment of ${Math.Abs(variance):F2} (Expected: ${expectedAmount:F2}, Received: ${actualAmount:F2})",
             Severity = "High",
         RecommendedAction = "File appeal or request for additional payment"
       });
    }
      }

            _logger.LogInformation("Found {Count} discrepancies", discrepancies.Count);
     return discrepancies;
        }
        catch (Exception ex)
  {
            _logger.LogError(ex, "Error identifying discrepancies");
            throw;
        }
    }

    /// <summary>
    /// Generate reconciliation report
    /// </summary>
    public async Task<ReconciliationReport> GenerateReconciliationReportAsync(
        DateTime fromDate,
  DateTime toDate,
  CancellationToken cancellationToken = default)
  {
        _logger.LogInformation("Generating reconciliation report for period {FromDate} to {ToDate}",
          fromDate.Date, toDate.Date);

        var report = new ReconciliationReport
        {
     ReportId = GenerateReportId(),
            FromDate = fromDate,
            ToDate = toDate,
    ReportDate = DateTime.UtcNow
        };

        try
        {
          // Mock data for now
       var mockPayments = new List<Payment>
        {
         new Payment { Id = 1, ClaimId = 1, Amount = 100m, PaymentDate = DateTime.UtcNow.AddDays(-5), ReferenceNumber = "REM-001", ProviderId = "PROV-001", InsurerId = "INS-001" },
        new Payment { Id = 2, ClaimId = 2, Amount = 150m, PaymentDate = DateTime.UtcNow.AddDays(-3), ReferenceNumber = "REM-002", ProviderId = "PROV-001", InsurerId = "INS-001" },
         new Payment { Id = 3, ClaimId = null, Amount = 75m, PaymentDate = DateTime.UtcNow.AddDays(-1), ReferenceNumber = "REM-003", ProviderId = "PROV-002", InsurerId = "INS-001" }
            };

var mockClaims = new List<Claim>
 {
        new Claim { Id = "1", Total = 100m, ProviderId = "PROV-001", ClaimNumber = "CLM-001" },
  new Claim { Id = "2", Total = 150m, ProviderId = "PROV-001", ClaimNumber = "CLM-002" },
     new Claim { Id = "3", Total = 75m, ProviderId = "PROV-002", ClaimNumber = "CLM-003" }
    };

            // Identify discrepancies
         var discrepancies = await IdentifyDiscrepanciesAsync(mockPayments, mockClaims, cancellationToken);

    report.TotalClaims = mockClaims.Count;
         report.TotalExpectedPayments = mockClaims.Sum(c => c.Total);
            report.TotalActualPayments = mockPayments.Sum(p => p.Amount);
    report.TotalVariance = report.TotalActualPayments - report.TotalExpectedPayments;
        report.SuccessfullyReconciled = report.TotalClaims - discrepancies.Count;
            report.DiscrepanciesFound = discrepancies.Count;
          report.Discrepancies = discrepancies;

     _logger.LogInformation("Reconciliation report generated: Total: {Total}, Variance: {Variance}",
     report.TotalExpectedPayments, report.TotalVariance);

       return report;
   }
        catch (Exception ex)
        {
     _logger.LogError(ex, "Error generating reconciliation report");
       throw;
        }
    }

    /// <summary>
    /// Calculate payment ageing
    /// </summary>
    public async Task<PaymentAgeingReport> CalculatePaymentAgeingAsync(
        string providerId,
        CancellationToken cancellationToken = default)
    {
      _logger.LogInformation("Calculating payment ageing for provider {ProviderId}", providerId);

    var report = new PaymentAgeingReport
        {
   ProviderId = providerId,
    AsOfDate = DateTime.UtcNow
        };

        try
        {
            // Mock data
    var mockPayments = new List<Payment>
         {
          new Payment { Id = 1, PaymentDate = DateTime.UtcNow.AddDays(-5), Amount = 1000m, ProviderId = providerId },
                new Payment { Id = 2, PaymentDate = DateTime.UtcNow.AddDays(-15), Amount = 1500m, ProviderId = providerId },
    new Payment { Id = 3, PaymentDate = DateTime.UtcNow.AddDays(-45), Amount = 2000m, ProviderId = providerId },
    new Payment { Id = 4, PaymentDate = DateTime.UtcNow.AddDays(-75), Amount = 1200m, ProviderId = providerId },
                new Payment { Id = 5, PaymentDate = DateTime.UtcNow.AddDays(-120), Amount = 800m, ProviderId = providerId }
            };

    var filteredPayments = mockPayments.Where(p => p.ProviderId == providerId).ToList();

            if (filteredPayments.Count == 0)
   return report;

            var today = DateTime.UtcNow;

      // Not yet due
            var notYetDue = filteredPayments.Where(p => (today - p.PaymentDate).Days <= 30).ToList();
            report.NotYetDue = new PaymentAgeingBucket
{
  Count = notYetDue.Count,
        Amount = notYetDue.Sum(p => p.Amount)
     };

 // 1-30 days
          var pastDue1To30 = filteredPayments.Where(p =>
                (today - p.PaymentDate).Days > 30 && (today - p.PaymentDate).Days <= 60).ToList();
            report.PastDue1To30 = new PaymentAgeingBucket
            {
      Count = pastDue1To30.Count,
                Amount = pastDue1To30.Sum(p => p.Amount)
       };

          // 31-60 days
    var pastDue31To60 = filteredPayments.Where(p =>
     (today - p.PaymentDate).Days > 60 && (today - p.PaymentDate).Days <= 90).ToList();
    report.PastDue31To60 = new PaymentAgeingBucket
      {
                Count = pastDue31To60.Count,
             Amount = pastDue31To60.Sum(p => p.Amount)
    };

  // 61-90 days
  var pastDue61To90 = filteredPayments.Where(p =>
     (today - p.PaymentDate).Days > 90 && (today - p.PaymentDate).Days <= 120).ToList();
     report.PastDue61To90 = new PaymentAgeingBucket
  {
             Count = pastDue61To90.Count,
          Amount = pastDue61To90.Sum(p => p.Amount)
            };

     // Over 90 days
var pastDueOver90 = filteredPayments.Where(p => (today - p.PaymentDate).Days > 120).ToList();
   report.PastDueOver90 = new PaymentAgeingBucket
            {
    Count = pastDueOver90.Count,
        Amount = pastDueOver90.Sum(p => p.Amount)
};

  report.TotalReceivable = filteredPayments.Sum(p => p.Amount);

            // Calculate percentages
  if (report.TotalReceivable > 0)
       {
        report.NotYetDue.Percentage = (report.NotYetDue.Amount / report.TotalReceivable) * 100;
    report.PastDue1To30.Percentage = (report.PastDue1To30.Amount / report.TotalReceivable) * 100;
report.PastDue31To60.Percentage = (report.PastDue31To60.Amount / report.TotalReceivable) * 100;
     report.PastDue61To90.Percentage = (report.PastDue61To90.Amount / report.TotalReceivable) * 100;
         report.PastDueOver90.Percentage = (report.PastDueOver90.Amount / report.TotalReceivable) * 100;
      }

       _logger.LogInformation("Payment ageing calculated: Total: {Total}, Over90: {Over90}",
        report.TotalReceivable, report.PastDueOver90.Amount);

            return report;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error calculating payment ageing");
            throw;
        }
    }

    /// <summary>
    /// Identify payment adjustments
    /// </summary>
    public async Task<PaymentAdjustmentResult> IdentifyPaymentAdjustmentsAsync(
    List<Payment> payments,
        CancellationToken cancellationToken = default)
    {
        _logger.LogInformation("Identifying payment adjustments for {Count} payments", payments?.Count ?? 0);

 var result = new PaymentAdjustmentResult();

 try
        {
      if (payments == null || payments.Count == 0)
           return result;

     // Mock claim lookup
  var mockClaims = new List<Claim>
            {
  new Claim { Id = "1", Total = 100m, ProviderId = "PROV-001", ClaimNumber = "CLM-001" },
      new Claim { Id = "2", Total = 150m, ProviderId = "PROV-001", ClaimNumber = "CLM-002" },
       new Claim { Id = "3", Total = 75m, ProviderId = "PROV-002", ClaimNumber = "CLM-003" }
         };

     foreach (var payment in payments)
  {
         if (!payment.ClaimId.HasValue)
              continue;

 var claim = mockClaims.FirstOrDefault(c => int.Parse(c.Id) == payment.ClaimId.Value);
     if (claim == null)
           continue;

                var adjustment = payment.Amount - claim.Total;

            if (adjustment > 0.01m)
          {
      // Overpayment
        result.TotalOverpayments += adjustment;
      result.OverpaymentCount++;
   result.Overpayments.Add(new PaymentAdjustmentDetail
       {
                  ClaimId = claim.ClaimNumber,
         ExpectedAmount = claim.Total,
       ActualAmount = payment.Amount,
    AdjustmentNeeded = -adjustment
     });
        }
     else if (adjustment < -0.01m)
  {
        // Underpayment
          result.TotalUnderpayments += Math.Abs(adjustment);
      result.UnderpaymentCount++;
          result.Underpayments.Add(new PaymentAdjustmentDetail
               {
             ClaimId = claim.ClaimNumber,
             ExpectedAmount = claim.Total,
                ActualAmount = payment.Amount,
            AdjustmentNeeded = Math.Abs(adjustment)
        });
      }
  }

     result.NetAdjustmentNeeded = result.TotalUnderpayments - result.TotalOverpayments;

    _logger.LogInformation("Payment adjustments identified: Overpayments: {Over}, Underpayments: {Under}",
   result.TotalOverpayments, result.TotalUnderpayments);

    return result;
    }
        catch (Exception ex)
   {
_logger.LogError(ex, "Error identifying payment adjustments");
    throw;
  }
    }

    #region Helper Methods

    private string GenerateReconciliationId()
    {
     return $"RECON-{DateTime.UtcNow:yyyyMMddHHmmss}-{Guid.NewGuid().ToString("N").Substring(0, 8)}";
    }

    private int GenerateReportId()
    {
   return (int)(DateTime.UtcNow.Ticks % int.MaxValue);
    }

    #endregion
}
