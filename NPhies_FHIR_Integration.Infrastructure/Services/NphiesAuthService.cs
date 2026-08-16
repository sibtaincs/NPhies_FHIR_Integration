using Microsoft.Extensions.Options;
using Microsoft.Extensions.Logging;
using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;
using NPhies_FHIR_Integration.Infrastructure.Configuration;

namespace NPhies_FHIR_Integration.Infrastructure.Services;

/// <summary>
/// NPHIES authentication service
/// Handles OAuth2 token acquisition and management
/// </summary>
public interface INphiesAuthService
{
    Task<string> GetAccessTokenAsync();
    Task<bool> ValidateTokenAsync(string token);
    Task RefreshTokenAsync();
}

public class NphiesAuthService : INphiesAuthService
{
    private readonly HttpClient _httpClient;
    private readonly NphiesConfiguration _config;
    private readonly ILogger<NphiesAuthService> _logger;
    private string _cachedToken = string.Empty;
    private DateTime _tokenExpiry = DateTime.MinValue;

    public NphiesAuthService(
        HttpClient httpClient,
      IOptions<NphiesConfiguration> config,
        ILogger<NphiesAuthService> logger)
    {
        _httpClient = httpClient ?? throw new ArgumentNullException(nameof(httpClient));
     _config = config?.Value ?? throw new ArgumentNullException(nameof(config));
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
    }

    /// <summary>
    /// Get valid access token (cached or new)
    /// </summary>
    public async Task<string> GetAccessTokenAsync()
    {
        // Return cached token if still valid
if (!string.IsNullOrEmpty(_cachedToken) && DateTime.UtcNow < _tokenExpiry.AddMinutes(-5))
        {
      _logger.LogDebug("Using cached access token");
            return _cachedToken;
        }

        _logger.LogInformation("Acquiring new NPHIES access token...");

     try
        {
// Sandbox mode - return mock token
    if (_config.IsSandbox)
     {
             _logger.LogInformation("Sandbox mode: Using mock token");
           _cachedToken = "SANDBOX_MOCK_TOKEN_" + Guid.NewGuid().ToString("N");
                _tokenExpiry = DateTime.UtcNow.AddHours(1);
           return _cachedToken;
            }

   // Production mode - acquire real token
   var tokenResponse = await RequestTokenAsync();
         
            _cachedToken = tokenResponse.AccessToken;
    _tokenExpiry = DateTime.UtcNow.AddSeconds(tokenResponse.ExpiresIn);

  _logger.LogInformation("? Access token acquired successfully. Expires at: {Expiry}", _tokenExpiry);

   return _cachedToken;
        }
        catch (Exception ex)
        {
  _logger.LogError(ex, "Failed to acquire NPHIES access token");
            throw new InvalidOperationException("Failed to authenticate with NPHIES", ex);
        }
    }

    /// <summary>
    /// Validate token with NPHIES
    /// </summary>
    public async Task<bool> ValidateTokenAsync(string token)
    {
    if (string.IsNullOrEmpty(token))
       return false;

      // Sandbox mode - always valid
     if (_config.IsSandbox)
            return true;

   // Production mode - validate with NPHIES
        try
     {
  var request = new HttpRequestMessage(HttpMethod.Get, $"{_config.BaseUrl}/userinfo");
      request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", token);

      var response = await _httpClient.SendAsync(request);
    return response.IsSuccessStatusCode;
        }
        catch (Exception ex)
        {
        _logger.LogError(ex, "Token validation failed");
       return false;
}
    }

    /// <summary>
    /// Force token refresh
    /// </summary>
    public async Task RefreshTokenAsync()
    {
        _logger.LogInformation("Forcing token refresh...");
        _cachedToken = string.Empty;
        _tokenExpiry = DateTime.MinValue;
  await GetAccessTokenAsync();
    }

    /// <summary>
    /// Request OAuth2 token from NPHIES
    /// </summary>
    private async Task<TokenResponse> RequestTokenAsync()
    {
        var requestContent = new FormUrlEncodedContent(new Dictionary<string, string>
        {
  ["grant_type"] = "client_credentials",
       ["client_id"] = _config.ClientId,
 ["client_secret"] = _config.ClientSecret,
          ["scope"] = "system/*.read system/*.write"
   });

        var response = await _httpClient.PostAsync(_config.TokenUrl, requestContent);
        response.EnsureSuccessStatusCode();

        var responseContent = await response.Content.ReadAsStringAsync();
        var tokenResponse = JsonSerializer.Deserialize<TokenResponse>(responseContent);

        if (tokenResponse == null || string.IsNullOrEmpty(tokenResponse.AccessToken))
     throw new InvalidOperationException("Invalid token response from NPHIES");

      return tokenResponse;
    }

    /// <summary>
    /// OAuth2 token response
    /// </summary>
    private class TokenResponse
    {
        [JsonPropertyName("access_token")]
        public string AccessToken { get; set; } = string.Empty;

        [JsonPropertyName("token_type")]
     public string TokenType { get; set; } = "Bearer";

        [JsonPropertyName("expires_in")]
        public int ExpiresIn { get; set; } = 3600;

     [JsonPropertyName("scope")]
     public string Scope { get; set; } = string.Empty;
    }
}
