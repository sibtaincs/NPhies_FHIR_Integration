using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using NPhies_FHIR_Integration.Domain.Entities;

namespace NPhies_FHIR_Integration.Application.Services.RCM;

/// <summary>
/// ClaimResponse Processing Service Interface
/// Handles processing of claim responses from payers
/// </summary>
public interface IClaimResponseProcessingService
{
    /// <summary>
    /// Process incoming claim response
    /// </summary>
    /// <param name="response">ClaimResponse entity</param>
    /// <param name="originalClaim">Original submitted claim</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>Processing result with details</returns>
    Task<ClaimResponseProcessingResult> ProcessClaimResponseAsync(
        ClaimResponse response,
 Claim originalClaim,
  CancellationToken cancellationToken = default);

    /// <summary>
    /// Extract adjudication details from response
    /// </summary>
    /// <param name="response">ClaimResponse entity</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>List of adjudication details</returns>
    Task<List<AdjudicationDetailDto>> ExtractAdjudicationDetailsAsync(
        ClaimResponse response,
        CancellationToken cancellationToken = default);

  /// <summary>
    /// Calculate patient responsibility from response
  /// </summary>
    /// <param name="response">ClaimResponse entity</param>
    /// <param name="coverage">Coverage information</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>Patient responsibility calculation</returns>
    Task<PatientResponsibilityResult> CalculatePatientResponsibilityAsync(
    ClaimResponse response,
        Coverage coverage,
        CancellationToken cancellationToken = default);

    /// <summary>
    /// Identify denied claim items
    /// </summary>
    /// <param name="response">ClaimResponse entity</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>List of denied items</returns>
    Task<List<DeniedItemDetail>> IdentifyDeniedItemsAsync(
  ClaimResponse response,
        CancellationToken cancellationToken = default);

  /// <summary>
    /// Generate RCM summary from response
    /// </summary>
    /// <param name="response">ClaimResponse entity</param>
    /// <param name="originalClaim">Original claim</param>
  /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>RCM summary</returns>
    Task<RCMSummary> GenerateRCMSummaryAsync(
        ClaimResponse response,
        Claim originalClaim,
        CancellationToken cancellationToken = default);
}

/// <summary>
/// Result from processing a claim response
/// </summary>
public class ClaimResponseProcessingResult
{
    /// <summary>
/// Claim ID
    /// </summary>
    public string ClaimId { get; set; } = string.Empty;

    /// <summary>
/// Response ID
    /// </summary>
    public string ResponseId { get; set; } = string.Empty;

    /// <summary>
    /// Whether processing was successful
    /// </summary>
    public bool IsSuccessful { get; set; }

    /// <summary>
    /// Processing status message
    /// </summary>
    public string StatusMessage { get; set; } = string.Empty;

    /// <summary>
  /// Total approved amount
    /// </summary>
    public decimal TotalApprovedAmount { get; set; }

    /// <summary>
    /// Total denied amount
    /// </summary>
    public decimal TotalDeniedAmount { get; set; }

    /// <summary>
    /// Total patient responsibility
    /// </summary>
    public decimal TotalPatientResponsibility { get; set; }

    /// <summary>
    /// Number of approved items
    /// </summary>
    public int ApprovedItemCount { get; set; }

    /// <summary>
    /// Number of denied items
    /// </summary>
    public int DeniedItemCount { get; set; }

    /// <summary>
    /// Number of pending items
    /// </summary>
    public int PendingItemCount { get; set; }

    /// <summary>
    /// Processing timestamp
    /// </summary>
    public DateTime ProcessedAt { get; set; } = DateTime.UtcNow;

    /// <summary>
    /// Any processing errors
    /// </summary>
    public List<string> Errors { get; set; } = new();
}

/// <summary>
/// Adjudication detail DTO
/// </summary>
public class AdjudicationDetailDto
{
    /// <summary>
    /// Item sequence number
    /// </summary>
    public int ItemSequence { get; set; }

  /// <summary>
    /// Service description
    /// </summary>
 public string ServiceDescription { get; set; } = string.Empty;

    /// <summary>
    /// Adjudication status (approved, denied, pending, pended)
    /// </summary>
    public string Status { get; set; } = string.Empty;

    /// <summary>
    /// Submitted amount
  /// </summary>
    public decimal SubmittedAmount { get; set; }

  /// <summary>
    /// Allowed amount
    /// </summary>
    public decimal AllowedAmount { get; set; }

    /// <summary>
    /// Insurance responsibility
    /// </summary>
    public decimal InsuranceResponsibility { get; set; }

    /// <summary>
    /// Patient responsibility
    /// </summary>
    public decimal PatientResponsibility { get; set; }

    /// <summary>
    /// Deductible applied
    /// </summary>
    public decimal DeductibleApplied { get; set; }

  /// <summary>
    /// Coinsurance applied
    /// </summary>
    public decimal CoinsuranceApplied { get; set; }

    /// <summary>
    /// Out-of-pocket applied
    /// </summary>
    public decimal OutOfPocketApplied { get; set; }
}

/// <summary>
/// Patient responsibility result
/// </summary>
public class PatientResponsibilityResult
{
    /// <summary>
    /// Total patient responsibility
    /// </summary>
    public decimal TotalResponsibility { get; set; }

    /// <summary>
    /// Breakdown by type
    /// </summary>
    public decimal DeductibleAmount { get; set; }

    /// <summary>
    /// Coinsurance amount
    /// </summary>
    public decimal CoinsuranceAmount { get; set; }

    /// <summary>
    /// Out-of-pocket amount
    /// </summary>
    public decimal OutOfPocketAmount { get; set; }

    /// <summary>
    /// Other patient costs
    /// </summary>
public decimal OtherAmount { get; set; }

  /// <summary>
    /// Whether responsibility has been met
 /// </summary>
    public bool IsResponsibilityMet { get; set; }

    /// <summary>
    /// Notes about calculation
    /// </summary>
    public string Notes { get; set; } = string.Empty;
}

/// <summary>
/// Denied item detail
/// </summary>
public class DeniedItemDetail
{
    /// <summary>
    /// Item sequence number
  /// </summary>
    public int ItemSequence { get; set; }

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
/// Submitted amount that was denied
  /// </summary>
    public decimal DeniedAmount { get; set; }

    /// <summary>
    /// Whether this denial is recoverable
    /// </summary>
    public bool IsRecoverable { get; set; }

    /// <summary>
    /// Whether patient can appeal
    /// </summary>
  public bool CanAppeal { get; set; }

    /// <summary>
    /// Appeal deadline if applicable
    /// </summary>
    public DateTime? AppealDeadline { get; set; }
}

/// <summary>
/// RCM summary
/// </summary>
public class RCMSummary
{
    /// <summary>
    /// Claim ID
    /// </summary>
    public string ClaimId { get; set; } = string.Empty;

    /// <summary>
    /// Response ID
    /// </summary>
public string ResponseId { get; set; } = string.Empty;

/// <summary>
    /// Total submitted amount
    /// </summary>
    public decimal TotalSubmittedAmount { get; set; }

    /// <summary>
    /// Total allowed amount
    /// </summary>
  public decimal TotalAllowedAmount { get; set; }

    /// <summary>
    /// Total approved amount
    /// </summary>
    public decimal TotalApprovedAmount { get; set; }

    /// <summary>
    /// Total denied amount
    /// </summary>
    public decimal TotalDeniedAmount { get; set; }

    /// <summary>
    /// Total insurance responsibility
    /// </summary>
    public decimal TotalInsuranceResponsibility { get; set; }

    /// <summary>
  /// Total patient responsibility
    /// </summary>
    public decimal TotalPatientResponsibility { get; set; }

    /// <summary>
 /// Number of items approved
    /// </summary>
  public int ApprovedItemCount { get; set; }

    /// <summary>
    /// Number of items denied
    /// </summary>
    public int DeniedItemCount { get; set; }

    /// <summary>
    /// Number of items pending
    /// </summary>
    public int PendingItemCount { get; set; }

 /// <summary>
    /// Processing status
    /// </summary>
    public string ProcessingStatus { get; set; } = "completed";

    /// <summary>
    /// Generated timestamp
    /// </summary>
    public DateTime GeneratedAt { get; set; } = DateTime.UtcNow;
}
