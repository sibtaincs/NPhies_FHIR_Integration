using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace NPhies_FHIR_Integration.Application.Services.RCM;

/// <summary>
/// Denial Management Service Interface
/// Handles denial analysis, categorization, and bulk resubmission
/// </summary>
public interface IDenialManagementService
{
    /// <summary>
    /// Get all denials with optional filtering
    /// </summary>
    /// <param name="filter">Denial filter criteria</param>
/// <param name="cancellationToken">Cancellation token</param>
    /// <returns>List of denial details</returns>
    Task<List<DenialDetail>> GetDenialsAsync(
  DenialFilter filter,
 CancellationToken cancellationToken = default);

    /// <summary>
    /// Categorize denials by type
    /// </summary>
    /// <param name="denials">List of denials</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>Categorized denials</returns>
    Task<DenialCategorization> CategorizeDenialsAsync(
  List<DenialDetail> denials,
        CancellationToken cancellationToken = default);

 /// <summary>
    /// Generate denial report
    /// </summary>
    /// <param name="fromDate">From date</param>
    /// <param name="toDate">To date</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>Denial report</returns>
    Task<DenialReport> GenerateDenialReportAsync(
        DateTime fromDate,
        DateTime toDate,
    CancellationToken cancellationToken = default);

    /// <summary>
    /// Get high-value denials
    /// </summary>
    /// <param name="threshold">Amount threshold</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>List of high-value denials</returns>
    Task<List<DenialDetail>> GetHighValueDenialsAsync(
        decimal threshold,
    CancellationToken cancellationToken = default);

    /// <summary>
    /// Calculate denial metrics
    /// </summary>
    /// <param name="providerId">Provider ID</param>
    /// <param name="fromDate">From date</param>
    /// <param name="toDate">To date</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>Denial metrics</returns>
    Task<DenialMetrics> CalculateDenialMetricsAsync(
        string providerId,
        DateTime fromDate,
        DateTime toDate,
        CancellationToken cancellationToken = default);

    /// <summary>
    /// Bulk resubmit denied claims
    /// </summary>
    /// <param name="claimIds">List of claim IDs to resubmit</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>Bulk resubmission result</returns>
    Task<BulkResubmissionResult> BulkResubmitDeniedClaimsAsync(
   List<int> claimIds,
        CancellationToken cancellationToken = default);
}

/// <summary>
/// Denial filter criteria
/// </summary>
public class DenialFilter
{
    /// <summary>
    /// Provider ID (optional)
    /// </summary>
    public string ProviderId { get; set; } = string.Empty;

    /// <summary>
    /// Insurer ID (optional)
    /// </summary>
    public string InsurerId { get; set; } = string.Empty;

    /// <summary>
/// From date (optional)
/// </summary>
    public DateTime? FromDate { get; set; }

    /// <summary>
    /// To date (optional)
    /// </summary>
    public DateTime? ToDate { get; set; }

    /// <summary>
 /// Denial reason code (optional)
  /// </summary>
    public string DenialReasonCode { get; set; } = string.Empty;

  /// <summary>
    /// Minimum amount (optional)
    /// </summary>
  public decimal? MinAmount { get; set; }

    /// <summary>
    /// Maximum amount (optional)
    /// </summary>
    public decimal? MaxAmount { get; set; }

    /// <summary>
    /// Is recoverable only
    /// </summary>
    public bool RecoverableOnly { get; set; }

    /// <summary>
    /// Page number
    /// </summary>
    public int PageNumber { get; set; } = 1;

    /// <summary>
    /// Page size
    /// </summary>
    public int PageSize { get; set; } = 50;
}

/// <summary>
/// Denial detail
/// </summary>
public class DenialDetail
{
    /// <summary>
  /// Claim ID
    /// </summary>
    public string ClaimId { get; set; } = string.Empty;

    /// <summary>
 /// Item sequence
    /// </summary>
    public int ItemSequence { get; set; }

    /// <summary>
    /// Service code
    /// </summary>
 public string ServiceCode { get; set; } = string.Empty;

    /// <summary>
    /// Service description
    /// </summary>
 public string ServiceDescription { get; set; } = string.Empty;

    /// <summary>
    /// Denial reason code
    /// </summary>
    public string DenialReasonCode { get; set; } = string.Empty;

    /// <summary>
  /// Denial reason description
    /// </summary>
    public string DenialReason { get; set; } = string.Empty;

    /// <summary>
 /// Denied amount
    /// </summary>
    public decimal DeniedAmount { get; set; }

    /// <summary>
    /// Denial date
    /// </summary>
 public DateTime DenialDate { get; set; }

  /// <summary>
    /// Is recoverable
    /// </summary>
    public bool IsRecoverable { get; set; }

    /// <summary>
    /// Provider ID
    /// </summary>
    public string ProviderId { get; set; } = string.Empty;

    /// <summary>
 /// Patient ID
    /// </summary>
    public string PatientId { get; set; } = string.Empty;
}

/// <summary>
/// Denial categorization
/// </summary>
public class DenialCategorization
{
    /// <summary>
    /// Total denials
    /// </summary>
    public int TotalDenials { get; set; }

    /// <summary>
    /// Medical necessity denials
    /// </summary>
    public int MedicalNecessityCount { get; set; }

    /// <summary>
    /// Authorization denials
    /// </summary>
    public int AuthorizationCount { get; set; }

  /// <summary>
    /// Coverage limitation denials
    /// </summary>
    public int CoverageLimitationCount { get; set; }

    /// <summary>
    /// Duplicate service denials
    /// </summary>
    public int DuplicateServiceCount { get; set; }

    /// <summary>
    /// Non-covered service denials
    /// </summary>
    public int NonCoveredServiceCount { get; set; }

    /// <summary>
    /// Other denials
    /// </summary>
    public int OtherCount { get; set; }

    /// <summary>
    /// Total denied amount
    /// </summary>
    public decimal TotalDeniedAmount { get; set; }

    /// <summary>
    /// Category breakdown
    /// </summary>
    public Dictionary<string, int> CategoryBreakdown { get; set; } = new();
}

/// <summary>
/// Denial report
/// </summary>
public class DenialReport
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
    /// Total denials in period
    /// </summary>
    public int TotalDenials { get; set; }

    /// <summary>
    /// Total denied amount
    /// </summary>
    public decimal TotalDeniedAmount { get; set; }

    /// <summary>
    /// Top denial reasons
    /// </summary>
    public List<DenialReasonSummary> TopReasons { get; set; } = new();

    /// <summary>
    /// Trends
    /// </summary>
    public string Trends { get; set; } = string.Empty;

    /// <summary>
    /// Recommendations
/// </summary>
    public List<string> Recommendations { get; set; } = new();

    /// <summary>
    /// Raw denial details
    /// </summary>
    public List<DenialDetail> DetailedDenials { get; set; } = new();
}

/// <summary>
/// Denial reason summary
/// </summary>
public class DenialReasonSummary
{
    /// <summary>
    /// Reason code
    /// </summary>
    public string ReasonCode { get; set; } = string.Empty;

    /// <summary>
    /// Reason description
    /// </summary>
    public string Reason { get; set; } = string.Empty;

    /// <summary>
    /// Count of denials with this reason
    /// </summary>
    public int Count { get; set; }

    /// <summary>
    /// Total amount denied with this reason
    /// </summary>
    public decimal TotalAmount { get; set; }

    /// <summary>
    /// Percentage of total denials
    /// </summary>
    public decimal PercentageOfTotal { get; set; }
}

/// <summary>
/// Denial metrics
/// </summary>
public class DenialMetrics
{
    /// <summary>
 /// Provider ID
    /// </summary>
    public string ProviderId { get; set; } = string.Empty;

    /// <summary>
    /// Total claims in period
    /// </summary>
    public int TotalClaims { get; set; }

    /// <summary>
    /// Total denied claims
    /// </summary>
    public int DeniedClaims { get; set; }

    /// <summary>
    /// Denial rate percentage
    /// </summary>
    public decimal DenialRate { get; set; }

    /// <summary>
    /// Total denied amount
    /// </summary>
    public decimal TotalDeniedAmount { get; set; }

    /// <summary>
    /// Average denial amount
    /// </summary>
    public decimal AverageDenialAmount { get; set; }

    /// <summary>
    /// Recoverable denials count
    /// </summary>
    public int RecoverableDenialsCount { get; set; }

    /// <summary>
    /// Potential recovery amount
    /// </summary>
    public decimal PotentialRecoveryAmount { get; set; }
}

/// <summary>
/// Bulk resubmission result
/// </summary>
public class BulkResubmissionResult
{
    /// <summary>
    /// Batch ID
    /// </summary>
    public int BatchId { get; set; }

    /// <summary>
    /// Total claims processed
    /// </summary>
    public int TotalProcessed { get; set; }

    /// <summary>
    /// Successfully resubmitted
    /// </summary>
    public int SuccessCount { get; set; }

 /// <summary>
    /// Failed resubmissions
    /// </summary>
    public int FailureCount { get; set; }

    /// <summary>
    /// Submission date
    /// </summary>
    public DateTime SubmittedAt { get; set; } = DateTime.UtcNow;

    /// <summary>
 /// Failed claim IDs
    /// </summary>
    public List<int> FailedClaimIds { get; set; } = new();

    /// <summary>
    /// Failure reasons
    /// </summary>
    public List<string> FailureReasons { get; set; } = new();
}
