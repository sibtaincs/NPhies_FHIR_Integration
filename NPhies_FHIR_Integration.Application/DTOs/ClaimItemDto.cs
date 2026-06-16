namespace NPhies_FHIR_Integration.Application.DTOs;

/// <summary>
/// DTO for ClaimItem entity - for read operations
/// </summary>
public class ClaimItemDto
{
 public int Id { get; set; }

    /// <summary>
    /// Claim ID
    /// </summary>
 public string ClaimId { get; set; } = string.Empty;

    /// <summary>
    /// Sequence number
  /// </summary>
    public int Sequence { get; set; }

  /// <summary>
  /// Product or service code
    /// </summary>
    public string ProductOrServiceCode { get; set; } = string.Empty;

    /// <summary>
    /// Product or service system
    /// </summary>
    public string? ProductOrServiceSystem { get; set; }

    /// <summary>
    /// Alternate product or service code
 /// </summary>
    public string? AltProductOrServiceCode { get; set; }

    /// <summary>
    /// Alternate product or service system
    /// </summary>
    public string? AltProductOrServiceSystem { get; set; }

    /// <summary>
    /// Quantity
    /// </summary>
    public decimal? Quantity { get; set; }

    /// <summary>
    /// Unit price
    /// </summary>
    public decimal? UnitPrice { get; set; }

    /// <summary>
    /// Net amount
    /// </summary>
    public decimal? Net { get; set; }

    /// <summary>
    /// Notes
    /// </summary>
    public string? Notes { get; set; }

 /// <summary>
    /// Patient invoice reference system
    /// </summary>
    public string? PatientInvoiceSystem { get; set; }

    /// <summary>
    /// Patient invoice reference value
    /// </summary>
    public string? PatientInvoiceValue { get; set; }
}

/// <summary>
/// DTO for creating a ClaimItem
/// </summary>
public class CreateClaimItemDto
{
    /// <summary>
    /// Claim ID
    /// </summary>
    public string ClaimId { get; set; } = string.Empty;

    /// <summary>
    /// Sequence number
    /// </summary>
    public int Sequence { get; set; }

    /// <summary>
 /// Product or service code
    /// </summary>
    public string ProductOrServiceCode { get; set; } = string.Empty;

    /// <summary>
    /// Product or service system
    /// </summary>
public string? ProductOrServiceSystem { get; set; }

    /// <summary>
    /// Alternate product or service code
    /// </summary>
    public string? AltProductOrServiceCode { get; set; }

    /// <summary>
    /// Alternate product or service system
    /// </summary>
    public string? AltProductOrServiceSystem { get; set; }

 /// <summary>
    /// Quantity
 /// </summary>
    public decimal? Quantity { get; set; }

    /// <summary>
  /// Unit price
    /// </summary>
    public decimal? UnitPrice { get; set; }

    /// <summary>
    /// Net amount
    /// </summary>
    public decimal? Net { get; set; }

    /// <summary>
    /// Notes
    /// </summary>
 public string? Notes { get; set; }

    /// <summary>
 /// Patient invoice reference system
    /// </summary>
    public string? PatientInvoiceSystem { get; set; }

    /// <summary>
    /// Patient invoice reference value
    /// </summary>
    public string? PatientInvoiceValue { get; set; }
}

/// <summary>
/// DTO for updating a ClaimItem
/// </summary>
public class UpdateClaimItemDto
{
    /// <summary>
    /// Sequence number
    /// </summary>
    public int? Sequence { get; set; }

  /// <summary>
 /// Product or service code
    /// </summary>
    public string? ProductOrServiceCode { get; set; }

    /// <summary>
    /// Quantity
    /// </summary>
    public decimal? Quantity { get; set; }

    /// <summary>
  /// Unit price
    /// </summary>
    public decimal? UnitPrice { get; set; }

    /// <summary>
    /// Net amount
    /// </summary>
    public decimal? Net { get; set; }

    /// <summary>
    /// Patient invoice reference value
    /// </summary>
    public string? PatientInvoiceValue { get; set; }
}
