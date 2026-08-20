using Microsoft.Extensions.Diagnostics.HealthChecks;
using Microsoft.Extensions.Logging;
using NPhies_FHIR_Integration.Application.Services.Auth;

namespace NPhies_FHIR_Integration.Application.HealthChecks;

/// <summary>
/// Health check for NPHIES authentication system
/// Verifies that authentication service can obtain and validate tokens
/// </summary>
public class NphiesAuthenticationHealthCheck : IHealthCheck
{
    private readonly IAuthenticationService _authService;
    private readonly ILogger<NphiesAuthenticationHealthCheck> _logger;

    public NphiesAuthenticationHealthCheck(
  IAuthenticationService authService,
      ILogger<NphiesAuthenticationHealthCheck> logger)
    {
  _authService = authService ?? throw new ArgumentNullException(nameof(authService));
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
    }

    public async Task<HealthCheckResult> CheckHealthAsync(
        HealthCheckContext context,
        CancellationToken cancellationToken = default)
    {
        try
  {
            _logger.LogDebug("Performing NPHIES authentication health check");

         // Try to get an access token
            var token = await _authService.GetAccessTokenAsync(cancellationToken);

            if (string.IsNullOrWhiteSpace(token))
  {
                var data = new Dictionary<string, object>
   {
    { "Status", "Failed" },
          { "Reason", "Token is null or empty" },
  { "CheckedAt", DateTime.UtcNow }
        };

            return HealthCheckResult.Unhealthy(
     "Authentication service returned empty token",
     null,
               data);
  }

       // Validate the token
      var isValid = await _authService.IsTokenValidAsync(token);

       if (isValid)
 {
          var data = new Dictionary<string, object>
         {
{ "Status", "Authenticated" },
            { "TokenLength", token.Length },
    { "CheckedAt", DateTime.UtcNow }
     };

      return HealthCheckResult.Healthy(
       "Authentication service is working correctly",
        data);
         }
 else
     {
      var data = new Dictionary<string, object>
 {
             { "Status", "TokenInvalid" },
   { "CheckedAt", DateTime.UtcNow }
         };

     return HealthCheckResult.Degraded(
     "Authentication service returned invalid token",
          null,
             data);
            }
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "NPHIES authentication health check failed");

  var data = new Dictionary<string, object>
 {
           { "Status", "Error" },
        { "Error", ex.Message },
          { "ErrorType", ex.GetType().Name },
     { "CheckedAt", DateTime.UtcNow }
   };

            return HealthCheckResult.Unhealthy(
 "Authentication health check failed with exception",
      ex,
     data);
      }
    }
}
