using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace NPhies_FHIR_Integration.Infrastructure.NPhiesIntegration;

/// <summary>
/// Interface for NPHIES API client - handles all communication with NPHIES endpoints
/// </summary>
public interface INPhiesApiClient
{
    // ========== ELIGIBILITY OPERATIONS ==========
    /// <summary>
    /// Submit eligibility request to NPHIES
    /// </summary>
    Task<NPhiesApiResponse<string>> SubmitEligibilityRequestAsync(
        string fhirBundle,
        CancellationToken cancellationToken = default);

    /// <summary>
    /// Poll eligibility response from NPHIES
    /// </summary>
    Task<NPhiesApiResponse<string>> PollEligibilityResponseAsync(
        string requestId,
        CancellationToken cancellationToken = default);

    // ========== CLAIM OPERATIONS ==========
    /// <summary>
    /// Submit claim to NPHIES
    /// </summary>
    Task<NPhiesApiResponse<string>> SubmitClaimAsync(
        string fhirBundle,
        string claimType, // institutional, professional, pharmacy, oral
        CancellationToken cancellationToken = default);

    /// <summary>
    /// Poll claim response from NPHIES
    /// </summary>
    Task<NPhiesApiResponse<string>> PollClaimResponseAsync(
        string claimId,
        CancellationToken cancellationToken = default);

    // ========== PRIOR AUTHORIZATION OPERATIONS ==========
    /// <summary>
    /// Submit prior authorization request to NPHIES
    /// </summary>
    Task<NPhiesApiResponse<string>> SubmitPriorAuthorizationAsync(
        string fhirBundle,
        CancellationToken cancellationToken = default);

    /// <summary>
    /// Poll prior authorization response from NPHIES
    /// </summary>
    Task<NPhiesApiResponse<string>> PollPriorAuthorizationResponseAsync(
        string authorizationId,
        CancellationToken cancellationToken = default);

    // ========== CANCELLATION OPERATIONS ==========
    /// <summary>
    /// Submit cancellation request to NPHIES
    /// </summary>
    Task<NPhiesApiResponse<string>> SubmitCancellationAsync(
        string taskBundle,
        string resourceIdToCancel,
        CancellationToken cancellationToken = default);

    // ========== COMMUNICATION OPERATIONS ==========
    /// <summary>
    /// Submit communication request to NPHIES
    /// </summary>
    Task<NPhiesApiResponse<string>> SubmitCommunicationRequestAsync(
        string communicationBundle,
        CancellationToken cancellationToken = default);

    /// <summary>
    /// Poll communication response from NPHIES
    /// </summary>
    Task<NPhiesApiResponse<string>> PollCommunicationResponseAsync(
        string communicationId,
        CancellationToken cancellationToken = default);

    // ========== PAYMENT OPERATIONS ==========
    /// <summary>
    /// Poll payment notice from NPHIES
    /// </summary>
    Task<NPhiesApiResponse<string>> PollPaymentNoticeAsync(
        string paymentNoticeId,
        CancellationToken cancellationToken = default);

    /// <summary>
    /// Poll payment reconciliation from NPHIES
    /// </summary>
    Task<NPhiesApiResponse<string>> PollPaymentReconciliationAsync(
        string reconciliationId,
        CancellationToken cancellationToken = default);

    // ========== STATUS CHECK OPERATIONS ==========
    /// <summary>
    /// Check status of any NPHIES transaction
    /// </summary>
    Task<NPhiesApiResponse<string>> CheckTransactionStatusAsync(
        string transactionId,
        string resourceType, // Claim, CoverageEligibilityRequest, etc.
        CancellationToken cancellationToken = default);

    // ========== TOKEN MANAGEMENT ==========
    /// <summary>
    /// Get OAuth2 access token from NPHIES
    /// </summary>
    Task<string> GetAccessTokenAsync(CancellationToken cancellationToken = default);

    /// <summary>
    /// Refresh OAuth2 access token
    /// </summary>
    Task<string> RefreshAccessTokenAsync(string refreshToken, CancellationToken cancellationToken = default);

    // ========== HEALTH CHECK ==========
    /// <summary>
    /// Check if NPHIES API is available
    /// </summary>
    Task<bool> IsNPhiesAvailableAsync(CancellationToken cancellationToken = default);
}

/// <summary>
/// NPHIES API response wrapper
/// </summary>
public class NPhiesApiResponse<T>
{
    public bool IsSuccess { get; set; }
    public T? Data { get; set; }
    public int StatusCode { get; set; }
    public string? ErrorMessage { get; set; }
    public string? ErrorCode { get; set; }
    public Dictionary<string, string>? Headers { get; set; }
    public DateTime ResponseTimestamp { get; set; } = DateTime.UtcNow;
    public TimeSpan Duration { get; set; }
}

/// <summary>
/// NPHIES API configuration
/// </summary>
public class NPhiesApiConfiguration
{
    public string BaseUrl { get; set; } = string.Empty;
    public string TokenEndpoint { get; set; } = string.Empty;
    public string ClientId { get; set; } = string.Empty;
    public string ClientSecret { get; set; } = string.Empty;
    public string Scope { get; set; } = "openid profile";
    public int TimeoutSeconds { get; set; } = 30;
    public int MaxRetryAttempts { get; set; } = 3;
    public bool EnableLogging { get; set; } = true;
    public string Environment { get; set; } = "staging"; // staging or production
}