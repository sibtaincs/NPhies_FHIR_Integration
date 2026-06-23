using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace NPhies_FHIR_Integration.Application.Services.RCM;

/// <summary>
/// Appeal Workflow Service Interface
/// Handles appeal submissions, tracking, and documentation
/// </summary>
public interface IAppealWorkflowService
{
    /// <summary>
    /// Submit appeal for denied claim
    /// </summary>
    /// <param name="claimId">Claim ID to appeal</param>
    /// <param name="denialReason">Original denial reason</param>
    /// <param name="appealReason">Why provider is appealing</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>Appeal submission result</returns>
    Task<AppealSubmissionResult> SubmitAppealAsync(
    string claimId,
    string denialReason,
        string appealReason,
        CancellationToken cancellationToken = default);

    /// <summary>
    /// Get appeal status
    /// </summary>
    /// <param name="appealId">Appeal ID</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>Appeal status information</returns>
    Task<AppealStatus> GetAppealStatusAsync(
    string appealId,
        CancellationToken cancellationToken = default);

    /// <summary>
    /// Add supporting documentation to appeal
    /// </summary>
    /// <param name="appealId">Appeal ID</param>
    /// <param name="document">Document bytes</param>
    /// <param name="documentType">Type of document (clinical note, lab report, etc.)</param>
  /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>Whether add was successful</returns>
 Task<bool> AddSupportingDocumentationAsync(
        string appealId,
        byte[] document,
   string documentType,
        CancellationToken cancellationToken = default);

    /// <summary>
/// Generate appeal letter
    /// </summary>
    /// <param name="appeal">Appeal information</param>
  /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>Appeal letter as PDF bytes</returns>
    Task<byte[]> GenerateAppealLetterAsync(
    Appeal appeal,
   CancellationToken cancellationToken = default);

    /// <summary>
    /// Get appeal deadline for a claim
    /// </summary>
 /// <param name="claimId">Claim ID</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>Appeal deadline date</returns>
    Task<DateTime> GetAppealDeadlineAsync(
      string claimId,
        CancellationToken cancellationToken = default);

    /// <summary>
    /// Calculate appeal metrics/statistics
    /// </summary>
    /// <param name="fromDate">From date</param>
    /// <param name="toDate">To date</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>Appeal metrics</returns>
    Task<AppealMetrics> GetAppealMetricsAsync(
DateTime fromDate,
 DateTime toDate,
    CancellationToken cancellationToken = default);
}

/// <summary>
/// Appeal submission result
/// </summary>
public class AppealSubmissionResult
{
    /// <summary>
    /// Appeal ID (generated upon submission)
    /// </summary>
    public string AppealId { get; set; } = string.Empty;

  /// <summary>
    /// Claim ID
    /// </summary>
  public string ClaimId { get; set; } = string.Empty;

    /// <summary>
    /// Whether submission was successful
    /// </summary>
    public bool IsSuccessful { get; set; }

    /// <summary>
    /// Submission status message
    /// </summary>
    public string StatusMessage { get; set; } = string.Empty;

    /// <summary>
    /// Submission timestamp
    /// </summary>
    public DateTime SubmittedAt { get; set; } = DateTime.UtcNow;

    /// <summary>
    /// Appeal deadline
    /// </summary>
    public DateTime AppealDeadline { get; set; }

    /// <summary>
    /// Confirmation number
    /// </summary>
    public string ConfirmationNumber { get; set; } = string.Empty;

    /// <summary>
    /// Any submission errors
    /// </summary>
    public List<string> Errors { get; set; } = new();
}

/// <summary>
/// Appeal status
/// </summary>
public class AppealStatus
{
    /// <summary>
    /// Appeal ID
    /// </summary>
    public string AppealId { get; set; } = string.Empty;

    /// <summary>
    /// Claim ID
    /// </summary>
    public string ClaimId { get; set; } = string.Empty;

    /// <summary>
    /// Appeal status (submitted, under review, approved, denied, withdrawn)
    /// </summary>
    public string Status { get; set; } = string.Empty;

    /// <summary>
    /// Current review level
    /// </summary>
    public int ReviewLevel { get; set; }

    /// <summary>
    /// Appeal reason
    /// </summary>
    public string AppealReason { get; set; } = string.Empty;

    /// <summary>
    /// Submission date
    /// </summary>
    public DateTime SubmittedDate { get; set; }

    /// <summary>
    /// Last updated
    /// </summary>
    public DateTime LastUpdated { get; set; }

    /// <summary>
    /// Appeal decision
    /// </summary>
    public string Decision { get; set; } = string.Empty;

    /// <summary>
    /// Days since submission
    /// </summary>
    public int DaysSinceSubmission { get; set; }

    /// <summary>
    /// Supporting documents attached
    /// </summary>
    public int DocumentCount { get; set; }

    /// <summary>
 /// Notes/comments
    /// </summary>
 public string Notes { get; set; } = string.Empty;
}

/// <summary>
/// Appeal entity
/// </summary>
public class Appeal
{
    /// <summary>
    /// Appeal ID
    /// </summary>
    public string Id { get; set; } = string.Empty;

    /// <summary>
    /// Claim ID
    /// </summary>
    public string ClaimId { get; set; } = string.Empty;

/// <summary>
    /// Appeal reason
    /// </summary>
    public string AppealReason { get; set; } = string.Empty;

    /// <summary>
    /// Original denial reason
    /// </summary>
    public string DenialReason { get; set; } = string.Empty;

    /// <summary>
    /// Submitted date
    /// </summary>
    public DateTime SubmittedDate { get; set; } = DateTime.UtcNow;

    /// <summary>
    /// Status
  /// </summary>
    public string Status { get; set; } = "submitted";

    /// <summary>
    /// Review level
    /// </summary>
public int ReviewLevel { get; set; } = 1;

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
/// Appeal metrics
/// </summary>
public class AppealMetrics
{
    /// <summary>
    /// Total appeals submitted
    /// </summary>
    public int TotalAppeals { get; set; }

    /// <summary>
    /// Appeals approved
    /// </summary>
    public int ApprovedAppeals { get; set; }

    /// <summary>
    /// Appeals denied
    /// </summary>
    public int DeniedAppeals { get; set; }

    /// <summary>
    /// Appeals pending
    /// </summary>
    public int PendingAppeals { get; set; }

/// <summary>
    /// Appeals withdrawn
    /// </summary>
    public int WithdrawnAppeals { get; set; }

    /// <summary>
    /// Average days to resolution
    /// </summary>
  public decimal AverageDaysToResolution { get; set; }

    /// <summary>
    /// Appeal approval rate
    /// </summary>
    public decimal ApprovalRate { get; set; }

    /// <summary>
    /// Total amount recovered through appeals
    /// </summary>
    public decimal TotalRecoveredAmount { get; set; }

    /// <summary>
    /// Most common appeal reasons
  /// </summary>
  public List<string> TopAppealReasons { get; set; } = new();

    /// <summary>
    /// Date range start
    /// </summary>
    public DateTime FromDate { get; set; }

    /// <summary>
    /// Date range end
    /// </summary>
    public DateTime ToDate { get; set; }
}
