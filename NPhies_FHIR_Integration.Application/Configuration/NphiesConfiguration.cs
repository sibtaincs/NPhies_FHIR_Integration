namespace NPhies_FHIR_Integration.Application.Configuration;

/// <summary>
/// NPHIES API configuration settings
/// </summary>
public class NphiesConfiguration
{
  /// <summary>
    /// Base URL for NPHIES API
    /// </summary>
    public string BaseUrl { get; set; } = "https://hsb.nphies.sa/";

    /// <summary>
    /// API endpoint configurations
    /// </summary>
    public EndpointsConfiguration Endpoints { get; set; } = new();

    /// <summary>
    /// Authentication configuration
    /// </summary>
    public AuthConfiguration Auth { get; set; } = new();

    /// <summary>
    /// Polling configuration
    /// </summary>
    public PollingConfiguration Polling { get; set; } = new();

    /// <summary>
    /// Request timeout in seconds
    /// </summary>
public int Timeout { get; set; } = 30;

    /// <summary>
    /// Number of retry attempts
    /// </summary>
    public int RetryCount { get; set; } = 3;

    /// <summary>
    /// Delay between retries in seconds
    /// </summary>
    public int RetryDelaySeconds { get; set; } = 2;

    /// <summary>
    /// Enable detailed logging
    /// </summary>
 public bool EnableDetailedLogging { get; set; } = true;
}

/// <summary>
/// NPHIES API endpoints
/// </summary>
public class EndpointsConfiguration
{
    /// <summary>
    /// Eligibility request endpoint
    /// </summary>
    public string Eligibility { get; set; } = "/EligibilityRequest/$submit";

    /// <summary>
    /// Claim submission endpoint
    /// </summary>
    public string Claim { get; set; } = "/Claim/$submit";

    /// <summary>
    /// Pre-authorization endpoint
    /// </summary>
    public string PreAuth { get; set; } = "/Claim/$submit";

    /// <summary>
    /// Polling endpoint (Task resource)
    /// </summary>
    public string Poll { get; set; } = "/Task/";

    /// <summary>
    /// Health check endpoint
    /// </summary>
 public string HealthCheck { get; set; } = "/metadata";
}

/// <summary>
/// Authentication configuration
/// </summary>
public class AuthConfiguration
{
    /// <summary>
    /// OAuth2 authority URL
    /// </summary>
    public string Authority { get; set; } = string.Empty;

    /// <summary>
    /// OAuth2 client ID
  /// </summary>
    public string ClientId { get; set; } = string.Empty;

    /// <summary>
    /// OAuth2 client secret
    /// </summary>
    public string ClientSecret { get; set; } = string.Empty;

    /// <summary>
    /// OAuth2 scope
    /// </summary>
    public string Scope { get; set; } = "nphies.api";

    /// <summary>
    /// Token endpoint
    /// </summary>
    public string TokenEndpoint { get; set; } = "/token";

    /// <summary>
    /// Token cache duration in minutes
    /// </summary>
    public int TokenCacheDurationMinutes { get; set; } = 55; // Refresh before 60min expiry
}

/// <summary>
/// Polling configuration for async responses
/// </summary>
public class PollingConfiguration
{
    /// <summary>
    /// Polling interval in seconds
    /// </summary>
    public int IntervalSeconds { get; set; } = 3;

    /// <summary>
    /// Maximum polling duration in minutes
    /// </summary>
    public int MaxDurationMinutes { get; set; } = 5;

    /// <summary>
  /// Use exponential backoff for polling
    /// </summary>
    public bool ExponentialBackoff { get; set; } = true;

    /// <summary>
    /// Maximum number of polling attempts
    /// </summary>
    public int MaxRetries { get; set; } = 100;

    /// <summary>
    /// Initial backoff delay in seconds (for exponential backoff)
 /// </summary>
    public int InitialBackoffSeconds { get; set; } = 2;

    /// <summary>
    /// Maximum backoff delay in seconds
    /// </summary>
    public int MaxBackoffSeconds { get; set; } = 30;
}
