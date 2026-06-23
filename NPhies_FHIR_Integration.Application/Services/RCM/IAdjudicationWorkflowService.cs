using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using NPhies_FHIR_Integration.Domain.Entities;

namespace NPhies_FHIR_Integration.Application.Services.RCM;

/// <summary>
/// Adjudication Workflow Service Interface
/// Handles adjudication logic and remittance generation
/// </summary>
public interface IAdjudicationWorkflowService
{
    /// <summary>
    /// Process adjudication for claim items
    /// </summary>
    /// <param name="claim">Original claim</param>
    /// <param name="coverage">Patient coverage</param>
    /// <param name="response">Claim response from payer</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>Adjudication result</returns>
    Task<AdjudicationWorkflowResult> ProcessAdjudicationAsync(
Claim claim,
        Coverage coverage,
        ClaimResponse response,
        CancellationToken cancellationToken = default);

    /// <summary>
    /// Apply adjudication rules to a claim item
    /// </summary>
    /// <param name="item">Claim item</param>
    /// <param name="context">Adjudication context</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>Rule application result</returns>
    Task<AdjudicationRuleResult> ApplyAdjudicationRulesAsync(
 ClaimItem item,
        AdjudicationContext context,
        CancellationToken cancellationToken = default);

    /// <summary>
    /// Generate adjudication narrative/explanation
    /// </summary>
    /// <param name="response">Claim response</param>
    /// <param name="details">Adjudication details</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>Narrative text</returns>
    Task<string> GenerateAdjudicationNarrativeAsync(
  ClaimResponse response,
        List<AdjudicationDetailDto> details,
        CancellationToken cancellationToken = default);

    /// <summary>
    /// Calculate appeal deadlines
    /// </summary>
    /// <param name="response">Claim response</param>
  /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>Appeal deadlines</returns>
    Task<AppealDeadlines> CalculateAppealDeadlinesAsync(
        ClaimResponse response,
     CancellationToken cancellationToken = default);

    /// <summary>
    /// Generate remittance advice
    /// </summary>
    /// <param name="response">Claim response</param>
    /// <param name="claim">Original claim</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>Remittance advice</returns>
    Task<RemittanceAdvice> GenerateRemittanceAdviceAsync(
        ClaimResponse response,
  Claim claim,
        CancellationToken cancellationToken = default);
}

/// <summary>
/// Adjudication workflow result
/// </summary>
public class AdjudicationWorkflowResult
{
    /// <summary>
    /// Claim ID
    /// </summary>
    public string ClaimId { get; set; } = string.Empty;

  /// <summary>
    /// Whether adjudication was successful
    /// </summary>
    public bool IsSuccessful { get; set; }

    /// <summary>
  /// Overall adjudication status
    /// </summary>
    public string OverallStatus { get; set; } = string.Empty;

    /// <summary>
    /// Total approved amount
    /// </summary>
    public decimal TotalApprovedAmount { get; set; }

    /// <summary>
    /// Total denied amount
    /// </summary>
  public decimal TotalDeniedAmount { get; set; }

    /// <summary>
    /// Total pended amount
    /// </summary>
    public decimal TotalPendedAmount { get; set; }

    /// <summary>
    /// Adjudication narrative
    /// </summary>
public string Narrative { get; set; } = string.Empty;

    /// <summary>
    /// Appeal deadlines
    /// </summary>
  public AppealDeadlines AppealDeadlines { get; set; } = new();

    /// <summary>
    /// Any processing errors
  /// </summary>
    public List<string> Errors { get; set; } = new();
}

/// <summary>
/// Adjudication rule result
/// </summary>
public class AdjudicationRuleResult
{
    /// <summary>
    /// Whether rule was applied
/// </summary>
    public bool RuleApplied { get; set; }

    /// <summary>
    /// Adjudication status from rule
    /// </summary>
  public string AdjudicationStatus { get; set; } = string.Empty;

    /// <summary>
    /// Reason for adjudication decision
    /// </summary>
    public string DecisionReason { get; set; } = string.Empty;

    /// <summary>
    /// Adjusted amount after rule application
    /// </summary>
    public decimal AdjustedAmount { get; set; }

  /// <summary>
    /// Rules that were applied
    /// </summary>
    public List<string> AppliedRules { get; set; } = new();
}

/// <summary>
/// Adjudication context
/// </summary>
public class AdjudicationContext
{
    /// <summary>
    /// Coverage ID
 /// </summary>
    public string CoverageId { get; set; } = string.Empty;

    /// <summary>
/// Claim type
  /// </summary>
    public string ClaimType { get; set; } = string.Empty;

    /// <summary>
    /// Service date
    /// </summary>
    public DateTime ServiceDate { get; set; }

    /// <summary>
    /// Network status
    /// </summary>
    public bool IsNetworkProvider { get; set; }

    /// <summary>
    /// Pre-authorization status
    /// </summary>
    public bool HasPreAuthorization { get; set; }

    /// <summary>
    /// Pre-authorization number
  /// </summary>
    public string PreAuthNumber { get; set; } = string.Empty;
}

/// <summary>
/// Appeal deadlines
/// </summary>
public class AppealDeadlines
{
    /// <summary>
    /// Claim ID
    /// </summary>
    public string ClaimId { get; set; } = string.Empty;

  /// <summary>
    /// Appeal deadline (from denial date)
    /// </summary>
    public DateTime AppealDeadline { get; set; }

    /// <summary>
    /// Days remaining to appeal
    /// </summary>
    public int DaysRemainingToAppeal { get; set; }

    /// <summary>
    /// Whether appeal is still possible
    /// </summary>
    public bool CanAppeal { get; set; }

    /// <summary>
    /// First level appeal deadline
    /// </summary>
    public DateTime FirstLevelAppealDeadline { get; set; }

    /// <summary>
    /// Second level appeal deadline
    /// </summary>
    public DateTime SecondLevelAppealDeadline { get; set; }

    /// <summary>
    /// Appeal instructions
    /// </summary>
    public string AppealInstructions { get; set; } = string.Empty;
}

/// <summary>
/// Remittance advice
/// </summary>
public class RemittanceAdvice
{
    /// <summary>
    /// Claim ID
    /// </summary>
    public string ClaimId { get; set; } = string.Empty;

  /// <summary>
 /// Remittance number
    /// </summary>
    public string RemittanceNumber { get; set; } = string.Empty;

    /// <summary>
    /// Remittance date
    /// </summary>
    public DateTime RemittanceDate { get; set; }

    /// <summary>
    /// Insurer name
    /// </summary>
    public string InsurerName { get; set; } = string.Empty;

    /// <summary>
    /// Provider name
    /// </summary>
public string ProviderName { get; set; } = string.Empty;

    /// <summary>
    /// Patient name
    /// </summary>
    public string PatientName { get; set; } = string.Empty;

    /// <summary>
    /// Patient ID
    /// </summary>
  public string PatientId { get; set; } = string.Empty;

/// <summary>
    /// Service date
  /// </summary>
 public DateTime ServiceDate { get; set; }

    /// <summary>
    /// Total submitted
  /// </summary>
    public decimal TotalSubmitted { get; set; }

    /// <summary>
    /// Total allowed
    /// </summary>
    public decimal TotalAllowed { get; set; }

  /// <summary>
    /// Total insurance pays
    /// </summary>
  public decimal TotalInsurancePays { get; set; }

    /// <summary>
    /// Total patient responsibility
    /// </summary>
    public decimal TotalPatientResponsibility { get; set; }

    /// <summary>
    /// Item details
    /// </summary>
    public List<RemittanceLineItem> LineItems { get; set; } = new();

  /// <summary>
    /// Special notes
    /// </summary>
  public string Notes { get; set; } = string.Empty;
}

/// <summary>
/// Remittance line item
/// </summary>
public class RemittanceLineItem
{
    /// <summary>
 /// Item sequence
    /// </summary>
    public int Sequence { get; set; }

    /// <summary>
  /// Service code
    /// </summary>
    public string ServiceCode { get; set; } = string.Empty;

    /// <summary>
    /// Service description
    /// </summary>
    public string Description { get; set; } = string.Empty;

    /// <summary>
    /// Submitted amount
  /// </summary>
 public decimal SubmittedAmount { get; set; }

    /// <summary>
    /// Allowed amount
  /// </summary>
    public decimal AllowedAmount { get; set; }

    /// <summary>
    /// Insurance pays
    /// </summary>
    public decimal InsurancePays { get; set; }

    /// <summary>
    /// Patient responsibility
    /// </summary>
    public decimal PatientResponsibility { get; set; }

    /// <summary>
 /// Adjudication status
    /// </summary>
    public string Status { get; set; } = string.Empty;
}
