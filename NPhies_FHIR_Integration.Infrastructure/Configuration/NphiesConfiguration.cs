namespace NPhies_FHIR_Integration.Infrastructure.Configuration;

/// <summary>
/// NPHIES API configuration settings
/// </summary>
public class NphiesConfiguration
{
    public const string SectionName = "Nphies";

    /// <summary>
    /// Environment: Sandbox, Production
    /// </summary>
    public string Environment { get; set; } = "Sandbox";

 /// <summary>
  /// Base URL for NPHIES API
    /// </summary>
    public string BaseUrl { get; set; } = string.Empty;

    /// <summary>
  /// Authentication endpoint
    /// </summary>
    public string TokenUrl { get; set; } = string.Empty;

    /// <summary>
    /// Client ID for OAuth2
    /// </summary>
    public string ClientId { get; set; } = string.Empty;

    /// <summary>
    /// Client Secret for OAuth2
    /// </summary>
 public string ClientSecret { get; set; } = string.Empty;

    /// <summary>
    /// Organization license number (Provider ID)
    /// </summary>
    public string OrganizationLicense { get; set; } = string.Empty;

    /// <summary>
    /// Organization identifier system
    /// </summary>
    public string OrganizationSystem { get; set; } = "http://nphies.sa/license";

    /// <summary>
    /// Enable certificate-based authentication (mTLS)
    /// </summary>
    public bool UseCertificateAuth { get; set; } = false;

  /// <summary>
  /// Certificate path for mTLS
    /// </summary>
    public string CertificatePath { get; set; } = string.Empty;

    /// <summary>
    /// Certificate password
    /// </summary>
    public string CertificatePassword { get; set; } = string.Empty;

    /// <summary>
    /// Request timeout in seconds
    /// </summary>
    public int TimeoutSeconds { get; set; } = 30;

    /// <summary>
    /// Maximum retry attempts
 /// </summary>
    public int MaxRetryAttempts { get; set; } = 3;

    /// <summary>
    /// Enable request/response logging
    /// </summary>
    public bool EnableLogging { get; set; } = true;

    /// <summary>
    /// Sandbox mode (uses mock responses)
/// </summary>
    public bool IsSandbox => Environment.Equals("Sandbox", StringComparison.OrdinalIgnoreCase);

    /// <summary>
    /// API Endpoints
    /// </summary>
    public NphiesEndpoints Endpoints { get; set; } = new();
}

/// <summary>
/// NPHIES API endpoints
/// </summary>
public class NphiesEndpoints
{
    /// <summary>
    /// Eligibility check endpoint
    /// </summary>
    public string EligibilityCheck { get; set; } = "/eligibility/$submit";

    /// <summary>
    /// Claim submission endpoint
    /// </summary>
public string ClaimSubmission { get; set; } = "/claim/$submit";

/// <summary>
/// Pre-authorization endpoint
    /// </summary>
    public string PreAuthorization { get; set; } = "/claim/$submit";

    /// <summary>
    /// Claim inquiry (status check) endpoint
    /// </summary>
    public string ClaimInquiry { get; set; } = "/claim/$poll";

    /// <summary>
    /// Claim cancellation endpoint
    /// </summary>
    public string ClaimCancellation { get; set; } = "/task";

    /// <summary>
    /// Communication endpoint
    /// </summary>
    public string Communication { get; set; } = "/communication";

    /// <summary>
    /// Polling endpoint
    /// </summary>
    public string Polling { get; set; } = "/task/$poll";
}
