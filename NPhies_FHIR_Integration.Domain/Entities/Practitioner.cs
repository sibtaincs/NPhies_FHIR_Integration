namespace NPhies_FHIR_Integration.Domain.Entities;

/// <summary>
/// Practitioner entity - represents a healthcare professional (doctor, nurse, etc.)
/// FHIR Resource: Practitioner
/// </summary>
public class Practitioner : BaseEntity
{
    /// <summary>
    /// Practitioner's first name
    /// </summary>
    public string FirstName { get; set; } = string.Empty;

    /// <summary>
  /// Practitioner's last name/family name
    /// </summary>
    public string LastName { get; set; } = string.Empty;

 /// <summary>
    /// Professional license number
    /// </summary>
    public string LicenseNumber { get; set; } = string.Empty;

    /// <summary>
    /// License system identifier
    /// Typically: http://nphies.sa/license/practitioner-license
    /// </summary>
    public string LicenseSystem { get; set; } = string.Empty;

    // Professional Details
    /// <summary>
    /// Medical specialization (Cardiologist, Dentist, Pediatrician, etc.)
    /// </summary>
    public string Specialization { get; set; } = string.Empty;

    /// <summary>
    /// Professional qualification/credentials
    /// </summary>
    public string Qualification { get; set; } = string.Empty;

    /// <summary>
 /// Professional title (Dr., Prof., Nurse, etc.)
    /// </summary>
    public string Title { get; set; } = string.Empty;

    // Contact Information
    /// <summary>
    /// Practitioner email address
    /// </summary>
    public string Email { get; set; } = string.Empty;

  /// <summary>
    /// Practitioner phone number
    /// </summary>
    public string Phone { get; set; } = string.Empty;

    // Organization Information
    /// <summary>
    /// Reference to the organization where practitioner works
    /// </summary>
    public string OrganizationId { get; set; } = string.Empty;

    /// <summary>
    /// The organization where this practitioner is employed
    /// </summary>
    public Organization? Organization { get; set; }

    // Status
    /// <summary>
    /// Practitioner status: active, inactive, retired, on-leave
    /// </summary>
    public string Status { get; set; } = "active";

    // Navigation Properties
    /// <summary>
    /// Collection of claims where this practitioner is the provider
    /// </summary>
    public ICollection<Claim> Claims { get; set; } = new List<Claim>();

    /// <summary>
    /// Get full name of practitioner
    /// </summary>
    public string GetFullName() => $"{FirstName} {LastName}".Trim();

    /// <summary>
    /// Get display name with title (e.g., "Dr. Mohammed Abdullah")
    /// </summary>
    public string GetDisplayName()
    {
        var nameWithTitle = string.IsNullOrEmpty(Title) 
       ? GetFullName() 
    : $"{Title} {GetFullName()}";
        
        return nameWithTitle.Trim();
    }

    /// <summary>
    /// Check if practitioner is active
    /// </summary>
    public bool IsActive => Status == "active";

    /// <summary>
  /// Check if practitioner has the required specialization
    /// </summary>
    public bool HasSpecialization(string specialization)
    {
     return !string.IsNullOrEmpty(Specialization) && 
    Specialization.Equals(specialization, StringComparison.OrdinalIgnoreCase);
  }
}
