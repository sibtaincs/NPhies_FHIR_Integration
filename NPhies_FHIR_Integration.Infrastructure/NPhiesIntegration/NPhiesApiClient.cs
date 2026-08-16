using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace NPhies_FHIR_Integration.Infrastructure.NPhiesIntegration;

/// <summary>
/// Implementation of NPHIES API client
/// </summary>
public class NPhiesApiClient : INPhiesApiClient
{
    private readonly HttpClient _httpClient;
    private readonly ILogger<NPhiesApiClient> _logger;
    private readonly NPhiesApiConfiguration _config;
    private string? _cachedAccessToken;
    private DateTime _tokenExpiryTime;

    public NPhiesApiClient(
        HttpClient httpClient,
        IOptions<NPhiesApiConfiguration> config,
        ILogger<NPhiesApiClient> logger)
    {
        _httpClient = httpClient ?? throw new ArgumentNullException(nameof(httpClient));
        _config = config?.Value ?? throw new ArgumentNullException(nameof(config));
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));

        _httpClient.BaseAddress = new Uri(_config.BaseUrl);
        _httpClient.Timeout = TimeSpan.FromSeconds(_config.TimeoutSeconds);
    }

    // ========== ELIGIBILITY OPERATIONS ==========

    public async Task<NPhiesApiResponse<string>> SubmitEligibilityRequestAsync(
        string fhirBundle,
        CancellationToken cancellationToken = default)
    {
        return await PostFhirBundleAsync(
            "/CoverageEligibilityRequest/$submit",
            fhirBundle,
            cancellationToken);
    }

    public async Task<NPhiesApiResponse<string>> PollEligibilityResponseAsync(
        string requestId,
        CancellationToken cancellationToken = default)
    {
        return await GetFhirResourceAsync(
            $"/CoverageEligibilityResponse/{requestId}",
            cancellationToken);
    }

    // ========== CLAIM OPERATIONS ==========

    public async Task<NPhiesApiResponse<string>> SubmitClaimAsync(
        string fhirBundle,
        string claimType,
        CancellationToken cancellationToken = default)
    {
        return await PostFhirBundleAsync(
            "/Claim/$submit",
            fhirBundle,
            cancellationToken);
    }

    public async Task<NPhiesApiResponse<string>> PollClaimResponseAsync(
        string claimId,
        CancellationToken cancellationToken = default)
    {
        return await GetFhirResourceAsync(
            $"/ClaimResponse/{claimId}",
            cancellationToken);
    }

    // ========== PRIOR AUTHORIZATION OPERATIONS ==========

    public async Task<NPhiesApiResponse<string>> SubmitPriorAuthorizationAsync(
        string fhirBundle,
        CancellationToken cancellationToken = default)
    {
        return await PostFhirBundleAsync(
            "/Claim/$submit", // Prior auth uses same endpoint with type=preauthorization
            fhirBundle,
            cancellationToken);
    }

    public async Task<NPhiesApiResponse<string>> PollPriorAuthorizationResponseAsync(
        string authorizationId,
        CancellationToken cancellationToken = default)
    {
        return await GetFhirResourceAsync(
            $"/ClaimResponse/{authorizationId}",
            cancellationToken);
    }

    // ========== CANCELLATION OPERATIONS ==========

    public async Task<NPhiesApiResponse<string>> SubmitCancellationAsync(
        string taskBundle,
        string resourceIdToCancel,
        CancellationToken cancellationToken = default)
    {
        return await PostFhirBundleAsync(
            "/Task/$submit",
            taskBundle,
            cancellationToken);
    }

    // ========== COMMUNICATION OPERATIONS ==========

    public async Task<NPhiesApiResponse<string>> SubmitCommunicationRequestAsync(
        string communicationBundle,
        CancellationToken cancellationToken = default)
    {
        return await PostFhirBundleAsync(
            "/CommunicationRequest/$submit",
            communicationBundle,
            cancellationToken);
    }

    public async Task<NPhiesApiResponse<string>> PollCommunicationResponseAsync(
        string communicationId,
        CancellationToken cancellationToken = default)
    {
        return await GetFhirResourceAsync(
            $"/Communication/{communicationId}",
            cancellationToken);
    }

    // ========== PAYMENT OPERATIONS ==========

    public async Task<NPhiesApiResponse<string>> PollPaymentNoticeAsync(
        string paymentNoticeId,
        CancellationToken cancellationToken = default)
    {
        return await GetFhirResourceAsync(
            $"/PaymentNotice/{paymentNoticeId}",
            cancellationToken);
    }

    public async Task<NPhiesApiResponse<string>> PollPaymentReconciliationAsync(
        string reconciliationId,
        CancellationToken cancellationToken = default)
    {
        return await GetFhirResourceAsync(
            $"/PaymentReconciliation/{reconciliationId}",
            cancellationToken);
    }

    // ========== STATUS CHECK OPERATIONS ==========

    public async Task<NPhiesApiResponse<string>> CheckTransactionStatusAsync(
        string transactionId,
        string resourceType,
        CancellationToken cancellationToken = default)
    {
        return await GetFhirResourceAsync(
            $"/{resourceType}/{transactionId}",
            cancellationToken);
    }

    // ========== TOKEN MANAGEMENT ==========

    public async Task<string> GetAccessTokenAsync(CancellationToken cancellationToken = default)
    {
        // Check if cached token is still valid
        if (!string.IsNullOrEmpty(_cachedAccessToken) && DateTime.UtcNow < _tokenExpiryTime)
        {
            _logger.LogDebug("Using cached access token");
            return _cachedAccessToken;
        }

        _logger.LogInformation("Requesting new access token from NPHIES");

        var tokenRequest = new Dictionary<string, string>
        {
            { "grant_type", "client_credentials" },
            { "client_id", _config.ClientId },
            { "client_secret", _config.ClientSecret },
            { "scope", _config.Scope }
        };

        var content = new FormUrlEncodedContent(tokenRequest);
        var response = await _httpClient.PostAsync(_config.TokenEndpoint, content, cancellationToken);

        response.EnsureSuccessStatusCode();

        var responseContent = await response.Content.ReadAsStringAsync(cancellationToken);
        var tokenResponse = JsonSerializer.Deserialize<TokenResponse>(responseContent);

        _cachedAccessToken = tokenResponse!.AccessToken;
        _tokenExpiryTime = DateTime.UtcNow.AddSeconds(tokenResponse.ExpiresIn - 60); // Refresh 60s before expiry

        _logger.LogInformation("Successfully obtained access token (expires in {Seconds}s)", tokenResponse.ExpiresIn);

        return _cachedAccessToken;
    }

    public async Task<string> RefreshAccessTokenAsync(string refreshToken, CancellationToken cancellationToken = default)
    {
        _logger.LogInformation("Refreshing access token");

        var tokenRequest = new Dictionary<string, string>
        {
            { "grant_type", "refresh_token" },
            { "refresh_token", refreshToken },
            { "client_id", _config.ClientId },
            { "client_secret", _config.ClientSecret }
        };

        var content = new FormUrlEncodedContent(tokenRequest);
        var response = await _httpClient.PostAsync(_config.TokenEndpoint, content, cancellationToken);

        response.EnsureSuccessStatusCode();

        var responseContent = await response.Content.ReadAsStringAsync(cancellationToken);
        var tokenResponse = JsonSerializer.Deserialize<TokenResponse>(responseContent);

        _cachedAccessToken = tokenResponse!.AccessToken;
        _tokenExpiryTime = DateTime.UtcNow.AddSeconds(tokenResponse.ExpiresIn - 60);

        return _cachedAccessToken;
    }

    // ========== HEALTH CHECK ==========

    public async Task<bool> IsNPhiesAvailableAsync(CancellationToken cancellationToken = default)
    {
        try
        {
            _logger.LogInformation("Checking NPHIES availability");

            var response = await _httpClient.GetAsync("/metadata", cancellationToken);
            var isAvailable = response.IsSuccessStatusCode;

            _logger.LogInformation("NPHIES availability check: {Status}", 
                isAvailable ? "Available" : "Unavailable");

            return isAvailable;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error checking NPHIES availability");
            return false;
        }
    }

    // ========== PRIVATE HELPER METHODS ==========

    private async Task<NPhiesApiResponse<string>> PostFhirBundleAsync(
        string endpoint,
        string fhirBundle,
        CancellationToken cancellationToken)
    {
        var stopwatch = Stopwatch.StartNew();
        
        try
        {
            _logger.LogInformation("Posting FHIR bundle to NPHIES endpoint: {Endpoint}", endpoint);

            var accessToken = await GetAccessTokenAsync(cancellationToken);

            using var request = new HttpRequestMessage(HttpMethod.Post, endpoint);
            request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);
            request.Headers.Accept.Add(new MediaTypeWithQualityHeaderValue("application/fhir+json"));
            request.Content = new StringContent(fhirBundle, Encoding.UTF8, "application/fhir+json");

            var response = await _httpClient.SendAsync(request, cancellationToken);
            stopwatch.Stop();

            var responseContent = await response.Content.ReadAsStringAsync(cancellationToken);

            if (response.IsSuccessStatusCode)
            {
                _logger.LogInformation("Successfully submitted to NPHIES (Status: {Status}, Duration: {Duration}ms)",
                    response.StatusCode, stopwatch.ElapsedMilliseconds);

                return new NPhiesApiResponse<string>
                {
                    IsSuccess = true,
                    Data = responseContent,
                    StatusCode = (int)response.StatusCode,
                    Duration = stopwatch.Elapsed,
                    Headers = GetResponseHeaders(response)
                };
            }
            else
            {
                _logger.LogError("NPHIES API error (Status: {Status}, Response: {Response})",
                    response.StatusCode, responseContent);

                return new NPhiesApiResponse<string>
                {
                    IsSuccess = false,
                    StatusCode = (int)response.StatusCode,
                    ErrorMessage = responseContent,
                    Duration = stopwatch.Elapsed
                };
            }
        }
        catch (Exception ex)
        {
            stopwatch.Stop();
            _logger.LogError(ex, "Exception calling NPHIES API endpoint: {Endpoint}", endpoint);

            return new NPhiesApiResponse<string>
            {
                IsSuccess = false,
                StatusCode = 500,
                ErrorMessage = ex.Message,
                Duration = stopwatch.Elapsed
            };
        }
    }

    private async Task<NPhiesApiResponse<string>> GetFhirResourceAsync(
        string endpoint,
        CancellationToken cancellationToken)
    {
        var stopwatch = Stopwatch.StartNew();

        try
        {
            _logger.LogInformation("Getting FHIR resource from NPHIES endpoint: {Endpoint}", endpoint);

            var accessToken = await GetAccessTokenAsync(cancellationToken);

            using var request = new HttpRequestMessage(HttpMethod.Get, endpoint);
            request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);
            request.Headers.Accept.Add(new MediaTypeWithQualityHeaderValue("application/fhir+json"));

            var response = await _httpClient.SendAsync(request, cancellationToken);
            stopwatch.Stop();

            var responseContent = await response.Content.ReadAsStringAsync(cancellationToken);

            if (response.IsSuccessStatusCode)
            {
                _logger.LogInformation("Successfully retrieved from NPHIES (Status: {Status}, Duration: {Duration}ms)",
                    response.StatusCode, stopwatch.ElapsedMilliseconds);

                return new NPhiesApiResponse<string>
                {
                    IsSuccess = true,
                    Data = responseContent,
                    StatusCode = (int)response.StatusCode,
                    Duration = stopwatch.Elapsed,
                    Headers = GetResponseHeaders(response)
                };
            }
            else
            {
                _logger.LogError("NPHIES API error (Status: {Status}, Response: {Response})",
                    response.StatusCode, responseContent);

                return new NPhiesApiResponse<string>
                {
                    IsSuccess = false,
                    StatusCode = (int)response.StatusCode,
                    ErrorMessage = responseContent,
                    Duration = stopwatch.Elapsed
                };
            }
        }
        catch (Exception ex)
        {
            stopwatch.Stop();
            _logger.LogError(ex, "Exception calling NPHIES API endpoint: {Endpoint}", endpoint);

            return new NPhiesApiResponse<string>
            {
                IsSuccess = false,
                StatusCode = 500,
                ErrorMessage = ex.Message,
                Duration = stopwatch.Elapsed
            };
        }
    }

    private Dictionary<string, string> GetResponseHeaders(HttpResponseMessage response)
    {
        var headers = new Dictionary<string, string>();
        
        foreach (var header in response.Headers)
        {
            headers[header.Key] = string.Join(", ", header.Value);
        }

        return headers;
    }

    // ========== INTERNAL MODELS ==========

    private class TokenResponse
    {
        [System.Text.Json.Serialization.JsonPropertyName("access_token")]
        public string AccessToken { get; set; } = string.Empty;

        [System.Text.Json.Serialization.JsonPropertyName("expires_in")]
        public int ExpiresIn { get; set; }

        [System.Text.Json.Serialization.JsonPropertyName("token_type")]
        public string TokenType { get; set; } = "Bearer";

        [System.Text.Json.Serialization.JsonPropertyName("refresh_token")]
        public string? RefreshToken { get; set; }
    }
}