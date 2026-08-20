using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Microsoft.Extensions.Diagnostics.HealthChecks;
using NPhies_FHIR_Integration.Application.Configuration;
using NPhies_FHIR_Integration.Application.Services.Auth;
using NPhies_FHIR_Integration.Application.Services.FHIR;
using NPhies_FHIR_Integration.Application.Services.Http;
using NPhies_FHIR_Integration.Application.Services.Orchestration;
using NPhies_FHIR_Integration.Application.Services.Polling;
using NPhies_FHIR_Integration.Application.HealthChecks;
using Polly;
using Polly.Extensions.Http;
using System.Net;

namespace NPhies_FHIR_Integration.Application.Extensions;

/// <summary>
/// Extension methods for configuring NPHIES services in the DI container
/// </summary>
public static class ServiceCollectionExtensions
{
    /// <summary>
    /// Adds all NPHIES integration services to the service collection
    /// </summary>
    /// <param name="services">The service collection</param>
    /// <param name="configuration">The configuration</param>
    /// <returns>The service collection for chaining</returns>
    public static IServiceCollection AddNphiesIntegration(
        this IServiceCollection services,
        IConfiguration configuration)
    {
        // 1. Register Configuration
        services.AddNphiesConfiguration(configuration);

        // 2. Register HTTP Client with Polly policies
        services.AddNphiesHttpClient();

        // 3. Register Core Services
        services.AddNphiesServices();

        // 4. Register Memory Cache for token caching
        services.AddMemoryCache();

        // 5. Register Health Checks
        services.AddNphiesHealthChecks();

        return services;
    }

    /// <summary>
    /// Registers NPHIES configuration
    /// </summary>
    private static IServiceCollection AddNphiesConfiguration(
        this IServiceCollection services,
        IConfiguration configuration)
    {
        // Bind configuration
        services.Configure<NphiesConfiguration>(
        configuration.GetSection("Nphies"));

        // Validate configuration on startup
        services.AddSingleton<IValidateOptions<NphiesConfiguration>, NphiesConfigurationValidator>();

        return services;
    }

    /// <summary>
    /// Registers HTTP Client with resilience policies
    /// </summary>
    private static IServiceCollection AddNphiesHttpClient(this IServiceCollection services)
    {
        services.AddHttpClient<INphiesHttpClient, NphiesHttpClient>("NphiesHttpClient")
         .ConfigureHttpClient((sp, client) =>
            {
                var config = sp.GetRequiredService<IOptions<NphiesConfiguration>>().Value;
                client.BaseAddress = new Uri(config.BaseUrl);
                client.Timeout = TimeSpan.FromSeconds(config.Timeout);
                client.DefaultRequestHeaders.Add("Accept", "application/fhir+json");
                client.DefaultRequestHeaders.Add("User-Agent", "NPHIES-FHIR-Integration/1.0");
            })
  .AddPolicyHandler(GetRetryPolicy())
  .AddPolicyHandler(GetCircuitBreakerPolicy())
       .AddPolicyHandler(GetTimeoutPolicy());

        return services;
    }

    /// <summary>
    /// Registers NPHIES services
    /// </summary>
    private static IServiceCollection AddNphiesServices(this IServiceCollection services)
    {
        // Authentication Service (Singleton for token caching)
        services.AddSingleton<IAuthenticationService, NphiesAuthenticationService>();

        // FHIR Bundle Service (Scoped)
        services.AddScoped<IFhirBundleService, FhirBundleService>();

        // Polling Service (Scoped)
        services.AddScoped<INphiesPollingService, NphiesPollingService>();

        // Orchestration Service (Scoped)
        services.AddScoped<INphiesOrchestrationService, NphiesOrchestrationService>();

        return services;
    }

    /// <summary>
    /// Registers NPHIES health checks
    /// </summary>
    private static IServiceCollection AddNphiesHealthChecks(this IServiceCollection services)
    {
        services.AddHealthChecks()
  // NPHIES API connectivity
       .AddCheck<NphiesHealthCheck>(
         "nphies_api",
         failureStatus: HealthStatus.Degraded,
   tags: new[] { "nphies", "api", "external", "ready" })
       
            // NPHIES Authentication
            .AddCheck<NphiesAuthenticationHealthCheck>(
 "nphies_auth",
  failureStatus: HealthStatus.Degraded,
            tags: new[] { "nphies", "auth", "security", "ready" })
            
    // Database connectivity
  .AddCheck<DatabaseHealthCheck>(
             "database",
        failureStatus: HealthStatus.Unhealthy,
            tags: new[] { "database", "sql", "data", "ready" });

      return services;
    }

    /// <summary>
    /// Gets the retry policy with exponential backoff
    /// </summary>
    private static IAsyncPolicy<HttpResponseMessage> GetRetryPolicy()
    {
        return HttpPolicyExtensions
  .HandleTransientHttpError() // 5xx and 408
       .OrResult(msg => msg.StatusCode == HttpStatusCode.TooManyRequests) // 429
       .WaitAndRetryAsync(
        retryCount: 3,
             sleepDurationProvider: retryAttempt =>
      TimeSpan.FromSeconds(Math.Pow(2, retryAttempt))); // Exponential backoff
    }

    /// <summary>
    /// Gets the circuit breaker policy
    /// </summary>
    private static IAsyncPolicy<HttpResponseMessage> GetCircuitBreakerPolicy()
    {
  return HttpPolicyExtensions
     .HandleTransientHttpError()
            .CircuitBreakerAsync(
      handledEventsAllowedBeforeBreaking: 5,
      durationOfBreak: TimeSpan.FromSeconds(30));
    }

 /// <summary>
    /// Gets the timeout policy
    /// </summary>
    private static IAsyncPolicy<HttpResponseMessage> GetTimeoutPolicy()
    {
      return Policy.TimeoutAsync<HttpResponseMessage>(TimeSpan.FromSeconds(30));
    }
}

/// <summary>
/// Validator for NPHIES configuration
/// </summary>
public class NphiesConfigurationValidator : IValidateOptions<NphiesConfiguration>
{
    public ValidateOptionsResult Validate(string? name, NphiesConfiguration options)
    {
        if (string.IsNullOrWhiteSpace(options.BaseUrl))
        {
            return ValidateOptionsResult.Fail("Nphies:BaseUrl is required");
        }

        if (!Uri.TryCreate(options.BaseUrl, UriKind.Absolute, out _))
        {
            return ValidateOptionsResult.Fail("Nphies:BaseUrl must be a valid URL");
        }

        if (options.Timeout <= 0)
        {
            return ValidateOptionsResult.Fail("Nphies:Timeout must be greater than 0");
        }

        if (options.RetryCount < 0)
        {
            return ValidateOptionsResult.Fail("Nphies:RetryCount must be greater than or equal to 0");
        }

        // Validate Auth configuration
        if (string.IsNullOrWhiteSpace(options.Auth.Authority))
        {
            return ValidateOptionsResult.Fail("Nphies:Auth:Authority is required");
        }

        if (string.IsNullOrWhiteSpace(options.Auth.ClientId))
        {
            return ValidateOptionsResult.Fail("Nphies:Auth:ClientId is required");
        }

        if (string.IsNullOrWhiteSpace(options.Auth.ClientSecret))
        {
            return ValidateOptionsResult.Fail("Nphies:Auth:ClientSecret is required");
        }

        // Validate Polling configuration
        if (options.Polling.IntervalSeconds <= 0)
        {
            return ValidateOptionsResult.Fail("Nphies:Polling:IntervalSeconds must be greater than 0");
        }

        if (options.Polling.MaxDurationMinutes <= 0)
        {
            return ValidateOptionsResult.Fail("Nphies:Polling:MaxDurationMinutes must be greater than 0");
        }

        return ValidateOptionsResult.Success;
    }
}
