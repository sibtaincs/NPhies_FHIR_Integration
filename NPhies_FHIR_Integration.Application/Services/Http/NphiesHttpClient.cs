using Hl7.Fhir.Model;
using Hl7.Fhir.Serialization;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using NPhies_FHIR_Integration.Application.Configuration;
using NPhies_FHIR_Integration.Application.Exceptions;
using NPhies_FHIR_Integration.Application.Services.Auth;
using System.Net;
using System.Net.Http.Headers;
using System.Text;

namespace NPhies_FHIR_Integration.Application.Services.Http;

/// <summary>
/// HTTP client implementation for NPHIES API communication
/// </summary>
public class NphiesHttpClient : INphiesHttpClient
{
    private readonly HttpClient _httpClient;
    private readonly ILogger<NphiesHttpClient> _logger;
    private readonly IAuthenticationService _authService;
    private readonly NphiesConfiguration _config;
    private readonly FhirJsonSerializer _serializer;
    private readonly FhirJsonParser _parser;

    public NphiesHttpClient(
        HttpClient httpClient,
        ILogger<NphiesHttpClient> logger,
        IAuthenticationService authService,
        IOptions<NphiesConfiguration> config)
    {
        _httpClient = httpClient ?? throw new ArgumentNullException(nameof(httpClient));
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        _authService = authService ?? throw new ArgumentNullException(nameof(authService));
        _config = config?.Value ?? throw new ArgumentNullException(nameof(config));

        // Configure base address
   _httpClient.BaseAddress = new Uri(_config.BaseUrl);
  _httpClient.Timeout = TimeSpan.FromSeconds(_config.Timeout);

   // Initialize FHIR serializer/parser
   _serializer = new FhirJsonSerializer(new SerializerSettings { Pretty = false });
        _parser = new FhirJsonParser();
    }

  /// <summary>
    /// Submit a FHIR bundle to NPHIES
  /// </summary>
    public async Task<Bundle> SubmitBundleAsync(
     Bundle requestBundle,
        string endpoint,
        CancellationToken cancellationToken = default)
    {
  try
    {
 _logger.LogInformation("Submitting bundle to NPHIES endpoint: {Endpoint}", endpoint);

       // Validate bundle
        if (requestBundle == null)
           throw new ArgumentNullException(nameof(requestBundle));

        // Serialize bundle to JSON
            var json = _serializer.SerializeToString(requestBundle);
            _logger.LogDebug("Serialized bundle size: {Size} bytes", Encoding.UTF8.GetByteCount(json));

            // Get access token
            var token = await _authService.GetAccessTokenAsync(cancellationToken);

            // Create HTTP request
            using var request = new HttpRequestMessage(HttpMethod.Post, endpoint);
            request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", token);
       request.Headers.Accept.Add(new MediaTypeWithQualityHeaderValue("application/fhir+json"));
            request.Content = new StringContent(json, Encoding.UTF8, "application/fhir+json");

   // Log request (without sensitive data)
            if (_config.EnableDetailedLogging)
   {
          _logger.LogDebug("Request: POST {BaseUrl}{Endpoint}", _config.BaseUrl, endpoint);
            }

            // Send request with retry logic
            var response = await SendWithRetryAsync(request, cancellationToken);

            // Read response
     var responseJson = await response.Content.ReadAsStringAsync(cancellationToken);
     _logger.LogDebug("Response size: {Size} bytes", Encoding.UTF8.GetByteCount(responseJson));

            // Parse response bundle
  var responseBundle = _parser.Parse<Bundle>(responseJson);

 _logger.LogInformation("Successfully received response bundle with {Count} entries",
        responseBundle.Entry?.Count ?? 0);

         return responseBundle;
      }
        catch (HttpRequestException ex)
        {
          _logger.LogError(ex, "HTTP error while submitting bundle to {Endpoint}", endpoint);
            throw new NphiesException($"Failed to submit bundle to NPHIES: {ex.Message}", ex);
     }
   catch (TaskCanceledException ex)
   {
  _logger.LogError(ex, "Request timeout while submitting bundle to {Endpoint}", endpoint);
      throw new NphiesTimeoutException(
    $"Request to NPHIES timed out after {_config.Timeout} seconds",
              _config.Timeout);
        }
        catch (Exception ex)
        {
         _logger.LogError(ex, "Unexpected error while submitting bundle to {Endpoint}", endpoint);
throw new NphiesException($"Unexpected error communicating with NPHIES: {ex.Message}", ex);
      }
 }

    /// <summary>
  /// Get a bundle by URL (for polling)
    /// </summary>
    public async Task<Bundle> GetBundleAsync(
        string url,
        CancellationToken cancellationToken = default)
    {
   try
        {
 _logger.LogInformation("Getting bundle from: {Url}", url);

    // Get access token
   var token = await _authService.GetAccessTokenAsync(cancellationToken);

     // Create HTTP request
            using var request = new HttpRequestMessage(HttpMethod.Get, url);
        request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", token);
        request.Headers.Accept.Add(new MediaTypeWithQualityHeaderValue("application/fhir+json"));

      // Send request
         var response = await SendWithRetryAsync(request, cancellationToken);

    // Read response
      var responseJson = await response.Content.ReadAsStringAsync(cancellationToken);

          // Parse bundle
    var bundle = _parser.Parse<Bundle>(responseJson);

            _logger.LogInformation("Successfully retrieved bundle with {Count} entries",
     bundle.Entry?.Count ?? 0);

            return bundle;
        }
        catch (HttpRequestException ex)
        {
     _logger.LogError(ex, "HTTP error while getting bundle from {Url}", url);
    throw new NphiesException($"Failed to get bundle from NPHIES: {ex.Message}", ex);
    }
        catch (TaskCanceledException ex)
        {
            _logger.LogError(ex, "Request timeout while getting bundle from {Url}", url);
            throw new NphiesTimeoutException(
          $"Request to NPHIES timed out after {_config.Timeout} seconds",
            _config.Timeout);
        }
        catch (Exception ex)
        {
   _logger.LogError(ex, "Unexpected error while getting bundle from {Url}", url);
throw new NphiesException($"Unexpected error communicating with NPHIES: {ex.Message}", ex);
  }
    }

    /// <summary>
    /// Get a Task resource by ID
    /// </summary>
    public async Task<Hl7.Fhir.Model.Task> GetTaskAsync(
        string taskId,
        CancellationToken cancellationToken = default)
    {
        try
        {
          _logger.LogInformation("Getting Task resource: {TaskId}", taskId);

         // Build Task URL
            var taskUrl = $"{_config.Endpoints.Poll}{taskId}";

            // Get access token
     var token = await _authService.GetAccessTokenAsync(cancellationToken);

       // Create HTTP request
     using var request = new HttpRequestMessage(HttpMethod.Get, taskUrl);
            request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", token);
            request.Headers.Accept.Add(new MediaTypeWithQualityHeaderValue("application/fhir+json"));

      // Send request
    var response = await SendWithRetryAsync(request, cancellationToken);

            // Read response
  var responseJson = await response.Content.ReadAsStringAsync(cancellationToken);

            // Parse Task resource
          var task = _parser.Parse<Hl7.Fhir.Model.Task>(responseJson);

 _logger.LogInformation("Task {TaskId} status: {Status}", taskId, task.Status);

         return task;
   }
        catch (HttpRequestException ex)
        {
         _logger.LogError(ex, "HTTP error while getting Task {TaskId}", taskId);
  throw new NphiesException($"Failed to get Task from NPHIES: {ex.Message}", ex);
        }
        catch (TaskCanceledException ex)
        {
 _logger.LogError(ex, "Request timeout while getting Task {TaskId}", taskId);
    throw new NphiesTimeoutException(
   $"Request to NPHIES timed out after {_config.Timeout} seconds",
         _config.Timeout);
        }
    catch (Exception ex)
        {
   _logger.LogError(ex, "Unexpected error while getting Task {TaskId}", taskId);
      throw new NphiesException($"Unexpected error communicating with NPHIES: {ex.Message}", ex);
        }
  }

    /// <summary>
    /// Health check
    /// </summary>
    public async Task<bool> HealthCheckAsync(CancellationToken cancellationToken = default)
    {
        try
      {
            _logger.LogInformation("Performing NPHIES health check");

            // Try to get capability statement
       var capability = await GetCapabilityStatementAsync(cancellationToken);

            var isHealthy = capability != null && capability.Status == PublicationStatus.Active;

            _logger.LogInformation("NPHIES health check result: {IsHealthy}", isHealthy);

   return isHealthy;
  }
        catch (Exception ex)
   {
            _logger.LogWarning(ex, "NPHIES health check failed");
         return false;
        }
    }

    /// <summary>
    /// Get capability statement
    /// </summary>
    public async Task<CapabilityStatement> GetCapabilityStatementAsync(
        CancellationToken cancellationToken = default)
    {
 try
    {
            _logger.LogDebug("Getting NPHIES capability statement");

          using var request = new HttpRequestMessage(HttpMethod.Get, _config.Endpoints.HealthCheck);
     request.Headers.Accept.Add(new MediaTypeWithQualityHeaderValue("application/fhir+json"));

       var response = await _httpClient.SendAsync(request, cancellationToken);
      response.EnsureSuccessStatusCode();

 var json = await response.Content.ReadAsStringAsync(cancellationToken);
            var capability = _parser.Parse<CapabilityStatement>(json);

     return capability;
        }
   catch (Exception ex)
        {
         _logger.LogError(ex, "Failed to get capability statement");
       throw new NphiesException("Failed to get NPHIES capability statement", ex);
 }
    }

    /// <summary>
 /// Send HTTP request with retry logic
    /// </summary>
    private async Task<HttpResponseMessage> SendWithRetryAsync(
        HttpRequestMessage request,
        CancellationToken cancellationToken)
    {
   HttpResponseMessage? response = null;
   Exception? lastException = null;

   for (int attempt = 1; attempt <= _config.RetryCount; attempt++)
   {
         try
    {
           // Clone request for retries (HttpRequestMessage can only be sent once)
       using var requestClone = await CloneRequestAsync(request);

  response = await _httpClient.SendAsync(requestClone, cancellationToken);

       // If successful, return
            if (response.IsSuccessStatusCode)
          {
          if (attempt > 1)
           {
             _logger.LogInformation("Request succeeded on attempt {Attempt}", attempt);
            }
            return response;
                }

      // Handle specific status codes
            if (response.StatusCode == HttpStatusCode.Unauthorized)
     {
    _logger.LogWarning("Unauthorized response, clearing token cache");
              _authService.ClearToken();

        if (attempt < _config.RetryCount)
     {
            // Retry with new token
        continue;
          }

     throw new NphiesAuthenticationException("Authentication failed with NPHIES");
      }

       // Log non-success status
    var errorContent = await response.Content.ReadAsStringAsync(cancellationToken);
             _logger.LogWarning("Request failed with status {StatusCode}: {Error}",
         response.StatusCode, errorContent);

                // Don't retry on client errors (4xx except 401)
    if ((int)response.StatusCode >= 400 && (int)response.StatusCode < 500)
      {
 throw new NphiesException(
       $"NPHIES returned error: {response.StatusCode}",
         response.StatusCode.ToString(),
      response.StatusCode);
                }

          // Retry on server errors (5xx)
         if (attempt < _config.RetryCount)
    {
         var delay = TimeSpan.FromSeconds(_config.RetryDelaySeconds * attempt);
   _logger.LogInformation("Retrying in {Delay} seconds (attempt {Attempt}/{Max})",
     delay.TotalSeconds, attempt, _config.RetryCount);

      await System.Threading.Tasks.Task.Delay(delay, cancellationToken);
 continue;
     }

   // Final attempt failed
     throw new NphiesException(
    $"NPHIES request failed after {_config.RetryCount} attempts: {response.StatusCode}",
     response.StatusCode.ToString(),
       response.StatusCode);
    }
      catch (Exception ex) when (ex is not NphiesException)
 {
      lastException = ex;

    if (attempt < _config.RetryCount)
   {
        var delay = TimeSpan.FromSeconds(_config.RetryDelaySeconds * attempt);
         _logger.LogWarning(ex, "Request attempt {Attempt} failed, retrying in {Delay} seconds",
   attempt, delay.TotalSeconds);

           await System.Threading.Tasks.Task.Delay(delay, cancellationToken);
     continue;
      }

       // All retries exhausted
      throw;
        }
        }

        // Should not reach here, but just in case
     if (lastException != null)
     throw lastException;

        if (response != null && !response.IsSuccessStatusCode)
        {
            throw new NphiesException(
     $"NPHIES request failed: {response.StatusCode}",
        response.StatusCode.ToString(),
        response.StatusCode);
        }

        throw new NphiesException("Request failed for unknown reason");
    }

    /// <summary>
    /// Clone HTTP request for retry
    /// </summary>
    private async Task<HttpRequestMessage> CloneRequestAsync(HttpRequestMessage request)
    {
    var clone = new HttpRequestMessage(request.Method, request.RequestUri);

  // Copy headers
        foreach (var header in request.Headers)
    {
         clone.Headers.TryAddWithoutValidation(header.Key, header.Value);
     }

        // Copy content if present
        if (request.Content != null)
 {
 var content = await request.Content.ReadAsStringAsync();
            clone.Content = new StringContent(
      content,
     Encoding.UTF8,
              request.Content.Headers.ContentType?.MediaType ?? "application/fhir+json");

            // Copy content headers
 foreach (var header in request.Content.Headers)
        {
    clone.Content.Headers.TryAddWithoutValidation(header.Key, header.Value);
       }
        }

        return clone;
    }
}
