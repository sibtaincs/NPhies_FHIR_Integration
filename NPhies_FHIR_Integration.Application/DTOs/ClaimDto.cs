namespace NPhies_FHIR_Integration.Application.DTOs;

/// <summary>
/// DTO for Claim entity - for read operations
/// </summary>
public class ClaimDto
{
    public int Id { get; set; }
    public string ClaimNumber { get; set; } = string.Empty;
    public string? ClaimIdentifierSystem { get; set; }
    public string? ClaimIdentifierValue { get; set; }
    public string Status { get; set; } = string.Empty;
    public string ClaimType { get; set; } = string.Empty;
 public string? ClaimTypeSystem { get; set; }
    public string? ClaimSubType { get; set; }
    public string Use { get; set; } = string.Empty;
    public string? Priority { get; set; }
    public string? PayeeType { get; set; }
    public decimal? Total { get; set; }
    public string? TotalCurrency { get; set; }
    public string PatientId { get; set; } = string.Empty;
    public string? ProviderId { get; set; }
    public string? InsurerId { get; set; }
    public string? CoverageId { get; set; }
    public string? MessageHeaderId { get; set; }
    public string? EpisodeIdentifierSystem { get; set; }
    public string? EpisodeIdentifierValue { get; set; }
    public string? EligibilityOfflineReference { get; set; }
    public DateTime? EligibilityOfflineDate { get; set; }
    public DateTime? AuthorizationOfflineDate { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime? UpdatedAt { get; set; }
}

/// <summary>
/// DTO for creating a Claim
/// </summary>
public class CreateClaimDto
{
    public string ClaimNumber { get; set; } = string.Empty;
    public string? ClaimIdentifierSystem { get; set; }
    public string? ClaimIdentifierValue { get; set; }
    public string ClaimType { get; set; } = string.Empty;
    public string? ClaimTypeSystem { get; set; }
    public string? ClaimSubType { get; set; }
    public string Use { get; set; } = "claim";
    public string? Priority { get; set; }
    public string? PrioritySystem { get; set; }
    public string? PayeeType { get; set; }
    public string? PayeeTypeSystem { get; set; }
    public decimal? Total { get; set; }
    public string? TotalCurrency { get; set; }
    public string PatientId { get; set; } = string.Empty;
    public string? ProviderId { get; set; }
    public string? InsurerId { get; set; }
    public string? CoverageId { get; set; }
    public string? MessageHeaderId { get; set; }
    public string? EpisodeIdentifierSystem { get; set; }
    public string? EpisodeIdentifierValue { get; set; }
    public string? EligibilityOfflineReference { get; set; }
    public DateTime? EligibilityOfflineDate { get; set; }
    public DateTime? AuthorizationOfflineDate { get; set; }
}

/// <summary>
/// DTO for updating a Claim
/// </summary>
public class UpdateClaimDto
{
public string? Status { get; set; }
    public decimal? Total { get; set; }
    public string? EpisodeIdentifierValue { get; set; }
public DateTime? AuthorizationOfflineDate { get; set; }
}
