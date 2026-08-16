namespace NPhies_FHIR_Integration.Application.DTOs;

/// <summary>
/// DTO for ClaimResponse entity - for read operations
/// </summary>
public class ClaimResponseDto
{
    public int Id { get; set; }
    public string ClaimId { get; set; } = string.Empty;
    public string? ResponseIdentifierSystem { get; set; }
    public string? ResponseIdentifierValue { get; set; }
    public string? ClaimResponseStatus { get; set; }
    public string? ClaimType { get; set; }
    public string? ClaimSubType { get; set; }
    public string? Use { get; set; }
    public string? PreAuthRef { get; set; }
    public string? PatientId { get; set; }
    public string? InsurerId { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime? UpdatedAt { get; set; }
    
    /// <summary>
    /// Alias for ClaimResponseStatus for compatibility
    /// </summary>
    public string? Outcome => ClaimResponseStatus;
    
    /// <summary>
    /// Alias for CreatedAt for compatibility
    /// </summary>
    public DateTime Created => CreatedAt;
}

/// <summary>
/// DTO for creating a ClaimResponse
/// </summary>
public class CreateClaimResponseDto
{
    public string ClaimId { get; set; } = string.Empty;
    public string? ResponseIdentifierSystem { get; set; }
    public string? ResponseIdentifierValue { get; set; }
    public string? ClaimResponseStatus { get; set; }
    public string? ClaimType { get; set; }
    public string? ClaimTypeSystem { get; set; }
    public string? ClaimSubType { get; set; }
    public string? Use { get; set; }
    public string? RequestIdentifierSystem { get; set; }
    public string? RequestIdentifierValue { get; set; }
    public string? PreAuthRef { get; set; }
    public string? PatientId { get; set; }
  public string? InsurerId { get; set; }
    public string? RequestorId { get; set; }
}

/// <summary>
/// DTO for updating a ClaimResponse
/// </summary>
public class UpdateClaimResponseDto
{
    public string? ClaimResponseStatus { get; set; }
    public string? PreAuthRef { get; set; }
}
