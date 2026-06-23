using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using NPhies_FHIR_Integration.Domain.Entities;

namespace NPhies_FHIR_Integration.Application.Services.RCM;

/// <summary>
/// Payment Reconciliation Service Interface
/// Handles payment matching, reconciliation, and discrepancy detection
/// </summary>
public interface IPaymentReconciliationService
{
    /// <summary>
 /// Reconcile payment from remittance advice
    /// </summary>
    /// <param name="response">Claim response</param>
/// <param name="paymentNotice">Payment notice</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>Reconciliation result</returns>
    Task<ReconciliationResult> ReconcilePaymentAsync(
        ClaimResponse response,
        PaymentNotice paymentNotice,
        CancellationToken cancellationToken = default);

    /// <summary>
    /// Match payment to claims
    /// </summary>
    /// <param name="payment">Payment details</param>
    /// <param name="potentialClaims">List of potential claims</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>Payment match result</returns>
  Task<PaymentMatchResult> MatchPaymentToClaimAsync(
        Payment payment,
        List<Claim> potentialClaims,
        CancellationToken cancellationToken = default);

    /// <summary>
    /// Identify payment discrepancies
    /// </summary>
    /// <param name="payments">List of payments</param>
    /// <param name="claims">List of claims</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>List of discrepancies found</returns>
    Task<List<PaymentDiscrepancy>> IdentifyDiscrepanciesAsync(
        List<Payment> payments,
        List<Claim> claims,
 CancellationToken cancellationToken = default);

    /// <summary>
    /// Generate reconciliation report
    /// </summary>
    /// <param name="fromDate">From date</param>
    /// <param name="toDate">To date</param>
  /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>Reconciliation report</returns>
    Task<ReconciliationReport> GenerateReconciliationReportAsync(
        DateTime fromDate,
        DateTime toDate,
        CancellationToken cancellationToken = default);

    /// <summary>
    /// Calculate payment ageing
    /// </summary>
    /// <param name="providerId">Provider ID</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>Payment ageing report</returns>
    Task<PaymentAgeingReport> CalculatePaymentAgeingAsync(
  string providerId,
        CancellationToken cancellationToken = default);

    /// <summary>
    /// Identify payment adjustments (overpayments/underpayments)
    /// </summary>
    /// <param name="payments">List of payments</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>Payment adjustment result</returns>
    Task<PaymentAdjustmentResult> IdentifyPaymentAdjustmentsAsync(
    List<Payment> payments,
      CancellationToken cancellationToken = default);
}

/// <summary>
/// Payment entity
/// </summary>
public class Payment
{
    /// <summary>
    /// Payment ID
    /// </summary>
    public int Id { get; set; }

    /// <summary>
    /// Claim ID
    /// </summary>
    public int? ClaimId { get; set; }

    /// <summary>
    /// Payment date
    /// </summary>
    public DateTime PaymentDate { get; set; }

    /// <summary>
    /// Payment amount
    /// </summary>
    public decimal Amount { get; set; }

    /// <summary>
    /// Payment method
    /// </summary>
    public string PaymentMethod { get; set; } = string.Empty;

    /// <summary>
    /// Reference number
    /// </summary>
    public string ReferenceNumber { get; set; } = string.Empty;

    /// <summary>
    /// Provider ID
    /// </summary>
  public string ProviderId { get; set; } = string.Empty;

  /// <summary>
    /// Insurer ID
    /// </summary>
    public string InsurerId { get; set; } = string.Empty;
}

/// <summary>
/// Payment notice entity
/// </summary>
public class PaymentNotice
{
    /// <summary>
    /// Notice ID
    /// </summary>
  public int Id { get; set; }

/// <summary>
    /// Notice date
    /// </summary>
    public DateTime NoticeDate { get; set; }

    /// <summary>
    /// Total amount
    /// </summary>
    public decimal TotalAmount { get; set; }

    /// <summary>
    /// Payment details
    /// </summary>
    public List<Payment> Payments { get; set; } = new();
}

/// <summary>
/// Reconciliation result
/// </summary>
public class ReconciliationResult
{
    /// <summary>
    /// Reconciliation ID
    /// </summary>
    public string ReconciliationId { get; set; } = string.Empty;

    /// <summary>
    /// Claim ID
    /// </summary>
 public string ClaimId { get; set; } = string.Empty;

    /// <summary>
    /// Whether reconciliation was successful
    /// </summary>
    public bool IsSuccessful { get; set; }

    /// <summary>
    /// Reconciliation status
    /// </summary>
    public string Status { get; set; } = string.Empty;

    /// <summary>
 /// Expected payment amount
    /// </summary>
    public decimal ExpectedAmount { get; set; }

    /// <summary>
    /// Actual payment amount
    /// </summary>
    public decimal ActualAmount { get; set; }

    /// <summary>
    /// Variance
    /// </summary>
    public decimal Variance { get; set; }

    /// <summary>
    /// Variance percentage
    /// </summary>
    public decimal VariancePercentage { get; set; }

    /// <summary>
  /// Any discrepancies found
    /// </summary>
    public List<string> Discrepancies { get; set; } = new();

    /// <summary>
    /// Reconciliation date
    /// </summary>
    public DateTime ReconciliationDate { get; set; } = DateTime.UtcNow;
}

/// <summary>
/// Payment match result
/// </summary>
public class PaymentMatchResult
{
    /// <summary>
    /// Whether match was successful
    /// </summary>
    public bool IsMatched { get; set; }

    /// <summary>
    /// Matched claim ID
    /// </summary>
    public int MatchedClaimId { get; set; }

    /// <summary>
    /// Match confidence (0-100)
    /// </summary>
    public decimal MatchConfidence { get; set; }

    /// <summary>
    /// Match reason
    /// </summary>
 public string MatchReason { get; set; } = string.Empty;

    /// <summary>
    /// Potential alternative matches
    /// </summary>
    public List<int> AlternativeMatches { get; set; } = new();
}

/// <summary>
/// Payment discrepancy
/// </summary>
public class PaymentDiscrepancy
{
    /// <summary>
    /// Claim ID
    /// </summary>
    public string ClaimId { get; set; } = string.Empty;

    /// <summary>
    /// Discrepancy type (overpayment, underpayment, unmatched, etc.)
    /// </summary>
    public string DiscrepancyType { get; set; } = string.Empty;

    /// <summary>
    /// Expected amount
    /// </summary>
    public decimal ExpectedAmount { get; set; }

    /// <summary>
    /// Actual amount
    /// </summary>
    public decimal ActualAmount { get; set; }

    /// <summary>
    /// Variance
    /// </summary>
    public decimal Variance { get; set; }

    /// <summary>
    /// Description
    /// </summary>
 public string Description { get; set; } = string.Empty;

    /// <summary>
    /// Severity (low, medium, high)
    /// </summary>
    public string Severity { get; set; } = string.Empty;

    /// <summary>
/// Recommended action
    /// </summary>
public string RecommendedAction { get; set; } = string.Empty;

    /// <summary>
    /// Discovery date
    /// </summary>
    public DateTime DiscoveryDate { get; set; } = DateTime.UtcNow;
}

/// <summary>
/// Reconciliation report
/// </summary>
public class ReconciliationReport
{
    /// <summary>
  /// Report ID
    /// </summary>
    public int ReportId { get; set; }

    /// <summary>
    /// Report date
    /// </summary>
    public DateTime ReportDate { get; set; } = DateTime.UtcNow;

    /// <summary>
    /// Period from
    /// </summary>
    public DateTime FromDate { get; set; }

    /// <summary>
    /// Period to
    /// </summary>
 public DateTime ToDate { get; set; }

    /// <summary>
    /// Total claims processed
    /// </summary>
    public int TotalClaims { get; set; }

    /// <summary>
    /// Total expected payments
    /// </summary>
    public decimal TotalExpectedPayments { get; set; }

    /// <summary>
    /// Total actual payments
    /// </summary>
    public decimal TotalActualPayments { get; set; }

    /// <summary>
    /// Total variance
    /// </summary>
    public decimal TotalVariance { get; set; }

    /// <summary>
  /// Successfully reconciled
    /// </summary>
    public int SuccessfullyReconciled { get; set; }

    /// <summary>
    /// Discrepancies found
    /// </summary>
  public int DiscrepanciesFound { get; set; }

    /// <summary>
    /// Discrepancy details
    /// </summary>
    public List<PaymentDiscrepancy> Discrepancies { get; set; } = new();
}

/// <summary>
/// Payment ageing report
/// </summary>
public class PaymentAgeingReport
{
    /// <summary>
 /// Provider ID
    /// </summary>
 public string ProviderId { get; set; } = string.Empty;

    /// <summary>
    /// Current date
    /// </summary>
    public DateTime AsOfDate { get; set; } = DateTime.UtcNow;

    /// <summary>
    /// Not yet due
    /// </summary>
    public PaymentAgeingBucket NotYetDue { get; set; } = new();

    /// <summary>
    /// 1-30 days past due
    /// </summary>
    public PaymentAgeingBucket PastDue1To30 { get; set; } = new();

    /// <summary>
    /// 31-60 days past due
  /// </summary>
    public PaymentAgeingBucket PastDue31To60 { get; set; } = new();

    /// <summary>
    /// 61-90 days past due
    /// </summary>
    public PaymentAgeingBucket PastDue61To90 { get; set; } = new();

    /// <summary>
    /// Over 90 days past due
  /// </summary>
 public PaymentAgeingBucket PastDueOver90 { get; set; } = new();

    /// <summary>
    /// Total receivable
    /// </summary>
    public decimal TotalReceivable { get; set; }
}

/// <summary>
/// Payment ageing bucket
/// </summary>
public class PaymentAgeingBucket
{
    /// <summary>
    /// Count of payments
    /// </summary>
    public int Count { get; set; }

    /// <summary>
    /// Total amount
    /// </summary>
    public decimal Amount { get; set; }

    /// <summary>
    /// Percentage of total
    /// </summary>
    public decimal Percentage { get; set; }
}

/// <summary>
/// Payment adjustment result
/// </summary>
public class PaymentAdjustmentResult
{
    /// <summary>
    /// Total overpayments
    /// </summary>
  public decimal TotalOverpayments { get; set; }

    /// <summary>
    /// Overpayment count
    /// </summary>
 public int OverpaymentCount { get; set; }

    /// <summary>
    /// Total underpayments
    /// </summary>
    public decimal TotalUnderpayments { get; set; }

    /// <summary>
    /// Underpayment count
    /// </summary>
    public int UnderpaymentCount { get; set; }

    /// <summary>
    /// Net adjustment needed
    /// </summary>
    public decimal NetAdjustmentNeeded { get; set; }

    /// <summary>
    /// Overpayment details
    /// </summary>
    public List<PaymentAdjustmentDetail> Overpayments { get; set; } = new();

    /// <summary>
    /// Underpayment details
    /// </summary>
    public List<PaymentAdjustmentDetail> Underpayments { get; set; } = new();
}

/// <summary>
/// Payment adjustment detail
/// </summary>
public class PaymentAdjustmentDetail
{
    /// <summary>
    /// Claim ID
    /// </summary>
    public string ClaimId { get; set; } = string.Empty;

    /// <summary>
    /// Expected amount
    /// </summary>
public decimal ExpectedAmount { get; set; }

  /// <summary>
  /// Actual amount
    /// </summary>
    public decimal ActualAmount { get; set; }

    /// <summary>
    /// Adjustment needed
    /// </summary>
    public decimal AdjustmentNeeded { get; set; }
}
