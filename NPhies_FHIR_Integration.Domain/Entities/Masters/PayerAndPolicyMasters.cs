using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace NPhies_FHIR_Integration.Domain.Entities;

/// <summary>
/// PayerMaster - Insurance companies/payers
/// </summary>
[Table("PayerMaster")]
public class PayerMaster : BaseEntity
{
    [Required]
    [StringLength(100)]
    public string PayerId { get; set; } = string.Empty;

    [Required]
    [StringLength(255)]
    public string PayerName { get; set; } = string.Empty;

    [StringLength(50)]
    public string? PayerType { get; set; }

    [StringLength(50)]
  public string? NphiesConnectionStatus { get; set; }

    [StringLength(500)]
    public string? NphiesApiEndpoint { get; set; }

    public bool IsNphiesMember { get; set; }

    [StringLength(500)]
  public string? SupportedClaimTypes { get; set; }

    [StringLength(500)]
    public string? SupportedEligibilityTypes { get; set; }

    public int MaxClaimsPerDay { get; set; }

    [Column(TypeName = "decimal(18,2)")]
 public decimal? MaxClaimAmount { get; set; }

    [StringLength(3)]
    public string CurrencyCode { get; set; } = "SAR";

    public bool IsActive { get; set; } = true;

    [StringLength(100)]
    public string? CreatedBy { get; set; }

    [StringLength(100)]
    public string? ModifiedBy { get; set; }

  // Navigation
    public virtual ICollection<PayerPolicyMaster> Policies { get; set; } = new List<PayerPolicyMaster>();
}

/// <summary>
/// PayerPolicyMaster - Insurance policies offered by payer
/// </summary>
[Table("PayerPolicyMaster")]
public class PayerPolicyMaster : BaseEntity
{
    [Required]
    [ForeignKey("Payer")]
    public int PayerMasterId { get; set; }

  [Required]
    [StringLength(100)]
    public string PolicyCode { get; set; } = string.Empty;

    [Required]
    [StringLength(255)]
    public string PolicyName { get; set; } = string.Empty;

    [StringLength(100)]
public string? PolicyType { get; set; }

  [StringLength(100)]
    public string? CoverageType { get; set; }

    [Column(TypeName = "decimal(18,2)")]
    public decimal AnnualPremium { get; set; }

 [StringLength(3)]
    public string CurrencyCode { get; set; } = "SAR";

    [Column(TypeName = "decimal(18,2)")]
    public decimal AnnualDeductible { get; set; }

[Column(TypeName = "decimal(18,2)")]
    public decimal? MaxOutOfPocket { get; set; }

    [Column(TypeName = "decimal(18,2)")]
    public decimal? Copay { get; set; }

    [Column(TypeName = "decimal(5,2)")]
    public decimal? CoinsurancePercentage { get; set; }

    [Column(TypeName = "decimal(18,2)")]
    public decimal? CoverageLimitPerVisit { get; set; }

 [Column(TypeName = "decimal(18,2)")]
    public decimal? CoverageLimitPerYear { get; set; }

    [Column(TypeName = "decimal(18,2)")]
    public decimal? PreAuthRequiredForAmount { get; set; }

    [Required]
    public DateTime EffectiveFromDate { get; set; }

  public DateTime? EffectiveToDate { get; set; }

    public bool IsPolicyActive { get; set; } = true;

    [StringLength(1000)]
    public string? Notes { get; set; }

    [StringLength(100)]
    public string? CreatedBy { get; set; }

    [StringLength(100)]
    public string? ModifiedBy { get; set; }

    // Navigation
  [ForeignKey("PayerMasterId")]
    public virtual PayerMaster? Payer { get; set; }

    public virtual ICollection<PolicyBenefitCoverage> BenefitCoverages { get; set; } = new List<PolicyBenefitCoverage>();
}

/// <summary>
/// PolicyBenefitCoverage - Benefits covered under each policy
/// </summary>
[Table("PolicyBenefitCoverage")]
public class PolicyBenefitCoverage : BaseEntity
{
 [Required]
    [ForeignKey("Policy")]
    public int PolicyMasterId { get; set; }

    [ForeignKey("ServiceCode")]
    public int? ServiceCodeMasterId { get; set; }

    [StringLength(100)]
    public string? ServiceCategory { get; set; }

    [StringLength(100)]
    public string? BenefitType { get; set; }

    [Column(TypeName = "decimal(5,2)")]
 public decimal CoveragePercentage { get; set; } = 100;

 [Column(TypeName = "decimal(18,2)")]
    public decimal? MaxCoverageAmount { get; set; }

    public bool RequiresPreAuth { get; set; }

    public bool RequiresReferral { get; set; }

    public int? PreAuthValidityDays { get; set; }

    public int? CoverageLimitPerYear { get; set; }

 public int? CoverageLimitPerLifetime { get; set; }

    public bool IsExcluded { get; set; }

    [StringLength(500)]
    public string? ExclusionReason { get; set; }

    public bool IsWaitingPeriodApplicable { get; set; }

 public int? WaitingPeriodDays { get; set; }

    [StringLength(1000)]
    public string? Notes { get; set; }

    [StringLength(100)]
    public string? CreatedBy { get; set; }

[StringLength(100)]
    public string? ModifiedBy { get; set; }

    // Navigation
    [ForeignKey("PolicyMasterId")]
    public virtual PayerPolicyMaster? Policy { get; set; }

    [ForeignKey("ServiceCodeMasterId")]
 public virtual ServiceCodeMaster? ServiceCode { get; set; }
}

/// <summary>
/// ClaimSubmissionRules - Rules for claim submission per payer/policy
/// </summary>
[Table("ClaimSubmissionRules")]
public class ClaimSubmissionRules : BaseEntity
{
    [ForeignKey("Payer")]
    public int? PayerMasterId { get; set; }

    [ForeignKey("Policy")]
  public int? PolicyMasterId { get; set; }

    [Required]
    [StringLength(255)]
    public string RuleName { get; set; } = string.Empty;

    [StringLength(100)]
    public string? RuleType { get; set; }

    [StringLength(1000)]
    public string? RuleCondition { get; set; }

    [Column(TypeName = "decimal(18,2)")]
    public decimal? MaxClaimAmount { get; set; }

    public int? MaxItemsPerClaim { get; set; }

    public bool RequiresInvoice { get; set; }

 public bool RequiresMedicalReport { get; set; }

    public bool RequiresPhotos { get; set; }

    public int? MaxDaysForSubmission { get; set; }

    public bool IsActive { get; set; } = true;

    public int Priority { get; set; } = 100;

  [StringLength(100)]
    public string? CreatedBy { get; set; }

    [StringLength(100)]
  public string? ModifiedBy { get; set; }

 // Navigation
  [ForeignKey("PayerMasterId")]
    public virtual PayerMaster? Payer { get; set; }

    [ForeignKey("PolicyMasterId")]
    public virtual PayerPolicyMaster? Policy { get; set; }
}
