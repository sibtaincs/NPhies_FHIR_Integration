namespace NPhies_FHIR_Integration.Domain.Entities;

/// <summary>
/// ClaimItem entity - represents line items in a claim
/// FHIR Resource: Claim.item
/// </summary>
public class ClaimItem : BaseEntity
{
    /// <summary>
    /// Claim ID this item belongs to
    /// </summary>
    public string ClaimId { get; set; } = string.Empty;

    /// <summary>
    /// Claim
    /// </summary>
    public Claim? Claim { get; set; }

    /// <summary>
    /// Item sequence number
    /// </summary>
    public int Sequence { get; set; }

    /// <summary>
    /// Care team sequence reference
    /// </summary>
    public int? CareTeamSequence { get; set; }

    /// <summary>
    /// Product or service code
    /// </summary>
    public string ProductOrServiceCode { get; set; } = string.Empty;

    /// <summary>
    /// Product or service system (e.g., "http://nphies.sa/terminology/CodeSystem/services")
    /// </summary>
    public string? ProductOrServiceSystem { get; set; }

    /// <summary>
    /// Product or service display text
    /// </summary>
    public string? ProductOrServiceDisplay { get; set; }

    /// <summary>
    /// Alternative coding system (e.g., provider's own system)
    /// </summary>
    public string? AltProductOrServiceCode { get; set; }

    /// <summary>
/// Alternative coding system URL
/// </summary>
    public string? AltProductOrServiceSystem { get; set; }

    /// <summary>
    /// Service date
    /// </summary>
    public DateTime? ServicedDate { get; set; }

    /// <summary>
    /// Service period start date
    /// </summary>
    public DateTime? ServicedPeriodStart { get; set; }

    /// <summary>
    /// Service period end date
    /// </summary>
  public DateTime? ServicedPeriodEnd { get; set; }

    /// <summary>
    /// Quantity of service
    /// </summary>
    public decimal? Quantity { get; set; }

    /// <summary>
    /// Unit price
    /// </summary>
  public decimal? UnitPrice { get; set; }

    /// <summary>
    /// Total net amount for this item
    /// </summary>
    public decimal? Net { get; set; }

    /// <summary>
    /// Amount patient responsible for paying (extension)
    /// </summary>
    public decimal? PatientShare { get; set; }

    /// <summary>
    /// Patient share currency
    /// </summary>
 public string? PatientShareCurrency { get; set; }

  /// <summary>
    /// Is this item part of a package (extension)
    /// </summary>
    public bool IsPackage { get; set; } = false;

    /// <summary>
    /// Is this a maternity service (extension)
    /// </summary>
    public bool IsMaternity { get; set; } = false;

 /// <summary>
    /// Item notes/remarks
    /// </summary>
    public string? Notes { get; set; }

    /// <summary>
    /// Patient invoice reference system (e.g., "http://sgh.com/patientInvoice")
    /// System URL for the patient invoice identifier
    /// </summary>
    public string? PatientInvoiceSystem { get; set; }

    /// <summary>
    /// Patient invoice reference value (e.g., "Invc-20220120/IP-1110987")
    /// Links to provider's patient invoice
    /// </summary>
    public string? PatientInvoiceValue { get; set; }

    // Navigation Properties
    /// <summary>
    /// Collection of claim item details (sub-items)
    /// </summary>
    public ICollection<ClaimItemDetail> Details { get; set; } = new List<ClaimItemDetail>();
}
