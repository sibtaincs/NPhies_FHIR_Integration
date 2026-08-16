# ?? Week 2 Detailed Plan - NPHIES HTTP Client & API Integration

## ?? Overview

**Status**: Ready to Start  
**Prerequisites**: ? Week 1 Complete (FhirBundleService ready)  
**Timeline**: 5-7 days  
**Risk Level**: ?? Medium (External API dependencies)  
**Target Framework**: .NET 9

---

## ?? Week 2 Goals (From Developer Guide)

Based on Phase 1 (Weeks 1-3), Week 2 focuses on:

### Primary Objectives
1. ? **Complete Eligibility Bundle Generation** (Already done in Week 1!)
2. ? **Complete Claim Bundle Generation** (Already done in Week 1!)
3. ? **Complete Pre-Auth Bundle Generation** (Already done in Week 1!)
4. ?? **Implement HTTP Client for NPHIES API**
5. ?? **Implement OAuth2/JWT Authentication**
6. ?? **Implement Polling Service for Async Responses**
7. ?? **Add Retry Logic and Resilience**
8. ?? **Create Unit Tests for Week 1 & Week 2**

### Bonus (If Time Permits)
- Configuration Management (appsettings.json)
- Integration Tests
- FHIR Validator Integration

---

## ?? What We Already Have (Week 1)

? **FhirBundleService** - Complete
- Creates eligibility, claim, pre-auth, and polling bundles
- Parses all response types
- Serializes/deserializes JSON
- Validates bundle structure

? **Domain Entities** - Complete
- Patient, Coverage, Claim, Organization, etc.
- All mapped correctly to FHIR resources

? **Documentation** - Complete
- 8 comprehensive documents
- Architecture patterns established

---

## ??? Week 2 Implementation Tasks

### Task 1: HTTP Client Service (Priority 0)

**Estimated Time**: 6-8 hours  
**Files to Create**:
- `NPhies_FHIR_Integration.Application/Services/Http/INphiesHttpClient.cs`
- `NPhies_FHIR_Integration.Application/Services/Http/NphiesHttpClient.cs`

**Requirements**:
```csharp
public interface INphiesHttpClient
{
    // Submit bundle and get response
    Task<Bundle> SubmitBundleAsync(Bundle requestBundle, CancellationToken cancellationToken = default);
    
    // Get bundle by URL (for polling)
    Task<Bundle> GetBundleAsync(string url, CancellationToken cancellationToken = default);
    
    // Health check
    Task<bool> HealthCheckAsync(CancellationToken cancellationToken = default);
    
    // Configuration
    void ConfigureEndpoint(string baseUrl, string apiKey);
}
```

**Implementation Details**:
- Use `HttpClientFactory` for connection pooling
- Add retry policies with Polly
- Implement timeout handling (30 seconds default)
- Add request/response logging
- Handle HTTP status codes properly
- Integrate with FhirBundleService for serialization

**NuGet Packages Needed**:
```xml
<PackageReference Include="Microsoft.Extensions.Http" Version="9.0.0" />
<PackageReference Include="Microsoft.Extensions.Http.Polly" Version="9.0.0" />
<PackageReference Include="Polly" Version="8.2.0" />
<PackageReference Include="Polly.Extensions.Http" Version="3.0.0" />
```

**Configuration** (appsettings.json):
```json
{
  "Nphies": {
    "BaseUrl": "https://hsb.nphies.sa/",
    "Endpoints": {
      "Eligibility": "/EligibilityRequest/$submit",
  "Claim": "/Claim/$submit",
      "PreAuth": "/Claim/$submit",
   "Poll": "/Task/"
    },
    "Timeout": 30,
  "RetryCount": 3,
    "RetryDelaySeconds": 2
  }
}
```

---

### Task 2: Authentication Service (Priority 0)

**Estimated Time**: 4-6 hours  
**Files to Create**:
- `NPhies_FHIR_Integration.Application/Services/Auth/IAuthenticationService.cs`
- `NPhies_FHIR_Integration.Application/Services/Auth/NphiesAuthenticationService.cs`

**Requirements**:
```csharp
public interface IAuthenticationService
{
    // Get access token (cached if valid)
    Task<string> GetAccessTokenAsync(CancellationToken cancellationToken = default);
    
    // Refresh token
    Task<string> RefreshTokenAsync(CancellationToken cancellationToken = default);
    
    // Validate token
    Task<bool> IsTokenValidAsync(string token);
    
    // Clear cached token
    void ClearToken();
}
```

**Implementation Details**:
- OAuth2 client credentials flow
- JWT token handling
- Token caching with expiration
- Automatic refresh before expiry
- Thread-safe token management

**Configuration** (appsettings.json):
```json
{
  "Nphies": {
    "Auth": {
      "Authority": "https://nphies.sa/oauth2/",
      "ClientId": "YOUR_CLIENT_ID",
      "ClientSecret": "YOUR_CLIENT_SECRET",
  "Scope": "nphies.api",
      "TokenEndpoint": "/token"
    }
  }
}
```

**NuGet Packages Needed**:
```xml
<PackageReference Include="IdentityModel" Version="7.0.0" />
<PackageReference Include="System.IdentityModel.Tokens.Jwt" Version="7.1.2" />
```

---

### Task 3: Polling Service (Priority 1)

**Estimated Time**: 4-5 hours  
**Files to Create**:
- `NPhies_FHIR_Integration.Application/Services/Polling/IPollingService.cs`
- `NPhies_FHIR_Integration.Application/Services/Polling/NphiesPollingService.cs`

**Requirements**:
```csharp
public interface IPollingService
{
    // Poll for response using Task ID
    Task<Bundle?> PollForResponseAsync(
  string taskId, 
        TimeSpan timeout, 
        CancellationToken cancellationToken = default);
    
    // Check task status
    Task<TaskStatus> GetTaskStatusAsync(string taskId);
    
    // Cancel pending task
    Task<bool> CancelTaskAsync(string taskId);
}

public enum TaskStatus
{
    Requested,
    Received,
    Accepted,
    Rejected,
    Ready,
    Cancelled,
    InProgress,
    OnHold,
    Failed,
    Completed,
    EnteredInError
}
```

**Implementation Details**:
- Poll every 2-5 seconds
- Maximum polling duration configurable (default 5 minutes)
- Exponential backoff for retries
- Handle Task resource status changes
- Return response bundle when ready
- Integrate with FhirBundleService and HttpClient

**Configuration** (appsettings.json):
```json
{
  "Nphies": {
    "Polling": {
      "IntervalSeconds": 3,
      "MaxDurationMinutes": 5,
      "ExponentialBackoff": true,
      "MaxRetries": 100
    }
  }
}
```

---

### Task 4: Orchestration Service (Priority 1)

**Estimated Time**: 4-6 hours  
**Files to Create**:
- `NPhies_FHIR_Integration.Application/Services/Orchestration/INphiesOrchestrationService.cs`
- `NPhies_FHIR_Integration.Application/Services/Orchestration/NphiesOrchestrationService.cs`

**Purpose**: High-level service that coordinates bundle creation, submission, and response handling

**Requirements**:
```csharp
public interface INphiesOrchestrationService
{
    // Submit eligibility request and get response
    Task<CoverageEligibilityResponse> SubmitEligibilityRequestAsync(
        CoverageEligibilityRequest request,
        Patient patient,
        Coverage coverage,
        Organization provider,
        Organization insurer);
    
    // Submit claim and get response
    Task<ClaimResponse> SubmitClaimAsync(
  Claim claim,
        Patient patient,
        Organization provider,
        Organization insurer,
        Coverage coverage,
        Encounter? encounter = null);
    
    // Submit pre-auth and get response
    Task<ClaimResponse> SubmitPreAuthAsync(
        Claim preAuth,
        Patient patient,
        Organization provider,
        Organization insurer,
        Coverage coverage,
   Encounter? encounter = null);
}
```

**Flow**:
```
1. Create bundle using FhirBundleService
2. Authenticate using AuthenticationService
3. Submit bundle using HttpClient
4. If async (Task returned), start polling
5. Parse response bundle
6. Return domain entity
```

---

### Task 5: Unit Tests (Priority 1)

**Estimated Time**: 6-8 hours  
**Files to Create**:
- `NPhies_FHIR_Integration.Tests/FHIR/FhirBundleServiceTests.cs` (Week 1 catch-up)
- `NPhies_FHIR_Integration.Tests/Http/NphiesHttpClientTests.cs`
- `NPhies_FHIR_Integration.Tests/Auth/AuthenticationServiceTests.cs`
- `NPhies_FHIR_Integration.Tests/Polling/PollingServiceTests.cs`
- `NPhies_FHIR_Integration.Tests/Orchestration/OrchestrationServiceTests.cs`

**Test Categories**:

#### Week 1 Tests (Catch-up)
1. `SerializeToJsonAsync_ValidBundle_ReturnsJson`
2. `DeserializeFromJsonAsync_ValidJson_ReturnsBundle`
3. `ValidateBundleAsync_ValidBundle_ReturnsIsValid`
4. `ValidateBundleAsync_BundleWithoutMessageHeader_ReturnsErrors`
5. `ExtractResourcesAsync_BundleWithPatients_ReturnsPatientList`
6. `CreateEligibilityRequestBundleAsync_ValidData_ReturnsCompleteBundle`
7. `CreateClaimRequestBundleAsync_ValidData_ReturnsCompleteBundle`
8. `ParseEligibilityResponseAsync_ValidBundle_ReturnsResponse`
9. `ParseClaimResponseAsync_ValidBundle_ReturnsResponse`

#### Week 2 Tests
10. `HttpClient_SubmitBundle_ReturnsResponse`
11. `HttpClient_RetryOnFailure_EventuallySucceeds`
12. `HttpClient_Timeout_ThrowsException`
13. `Auth_GetToken_ReturnsValidToken`
14. `Auth_TokenExpired_RefreshesAutomatically`
15. `Polling_WaitForResponse_ReturnsWhenReady`
16. `Polling_Timeout_ThrowsException`
17. `Orchestration_SubmitEligibility_EndToEnd`

**NuGet Packages Needed**:
```xml
<PackageReference Include="xunit" Version="2.6.6" />
<PackageReference Include="xunit.runner.visualstudio" Version="2.5.6" />
<PackageReference Include="Moq" Version="4.20.70" />
<PackageReference Include="FluentAssertions" Version="6.12.0" />
<PackageReference Include="Microsoft.NET.Test.Sdk" Version="17.9.0" />
```

---

### Task 6: Configuration Management (Priority 2)

**Estimated Time**: 2-3 hours  
**Files to Create**:
- `NPhies_FHIR_Integration.Application/Configuration/NphiesConfiguration.cs`
- `NPhies_FHIR_Integration.Application/Configuration/NphiesConfigurationExtensions.cs`

**Configuration Classes**:
```csharp
public class NphiesConfiguration
{
    public string BaseUrl { get; set; } = string.Empty;
    public EndpointsConfiguration Endpoints { get; set; } = new();
    public AuthConfiguration Auth { get; set; } = new();
    public PollingConfiguration Polling { get; set; } = new();
    public int Timeout { get; set; } = 30;
    public int RetryCount { get; set; } = 3;
    public int RetryDelaySeconds { get; set; } = 2;
}

public class EndpointsConfiguration
{
    public string Eligibility { get; set; } = "/EligibilityRequest/$submit";
    public string Claim { get; set; } = "/Claim/$submit";
    public string PreAuth { get; set; } = "/Claim/$submit";
    public string Poll { get; set; } = "/Task/";
}

public class AuthConfiguration
{
    public string Authority { get; set; } = string.Empty;
    public string ClientId { get; set; } = string.Empty;
    public string ClientSecret { get; set; } = string.Empty;
    public string Scope { get; set; } = "nphies.api";
    public string TokenEndpoint { get; set; } = "/token";
}

public class PollingConfiguration
{
    public int IntervalSeconds { get; set; } = 3;
    public int MaxDurationMinutes { get; set; } = 5;
    public bool ExponentialBackoff { get; set; } = true;
    public int MaxRetries { get; set; } = 100;
}
```

**Extension Method for DI**:
```csharp
public static class NphiesConfigurationExtensions
{
    public static IServiceCollection AddNphiesServices(
    this IServiceCollection services,
        IConfiguration configuration)
    {
        // Bind configuration
        services.Configure<NphiesConfiguration>(
            configuration.GetSection("Nphies"));
   
        // Register services
        services.AddScoped<IFhirBundleService, FhirBundleService>();
     services.AddScoped<IAuthenticationService, NphiesAuthenticationService>();
        services.AddScoped<IPollingService, NphiesPollingService>();
        services.AddScoped<INphiesOrchestrationService, NphiesOrchestrationService>();
        
        // Register HTTP client with Polly
        services.AddHttpClient<INphiesHttpClient, NphiesHttpClient>()
            .AddPolicyHandler(GetRetryPolicy())
   .AddPolicyHandler(GetCircuitBreakerPolicy());
        
      return services;
  }
    
    private static IAsyncPolicy<HttpResponseMessage> GetRetryPolicy()
    {
        return HttpPolicyExtensions
            .HandleTransientHttpError()
   .WaitAndRetryAsync(3, retryAttempt => 
      TimeSpan.FromSeconds(Math.Pow(2, retryAttempt)));
    }
    
    private static IAsyncPolicy<HttpResponseMessage> GetCircuitBreakerPolicy()
    {
      return HttpPolicyExtensions
            .HandleTransientHttpError()
   .CircuitBreakerAsync(5, TimeSpan.FromSeconds(30));
    }
}
```

---

### Task 7: Error Handling & Resilience (Priority 1)

**Estimated Time**: 3-4 hours  
**Files to Create**:
- `NPhies_FHIR_Integration.Application/Exceptions/NphiesException.cs`
- `NPhies_FHIR_Integration.Application/Exceptions/NphiesAuthenticationException.cs`
- `NPhies_FHIR_Integration.Application/Exceptions/NphiesTimeoutException.cs`
- `NPhies_FHIR_Integration.Application/Exceptions/NphiesValidationException.cs`

**Custom Exceptions**:
```csharp
public class NphiesException : Exception
{
    public string ErrorCode { get; set; } = string.Empty;
    public HttpStatusCode? StatusCode { get; set; }
    
    public NphiesException(string message) : base(message) { }
    public NphiesException(string message, Exception innerException) 
      : base(message, innerException) { }
}

public class NphiesAuthenticationException : NphiesException
{
 public NphiesAuthenticationException(string message) : base(message) { }
}

public class NphiesTimeoutException : NphiesException
{
    public NphiesTimeoutException(string message) : base(message) { }
}

public class NphiesValidationException : NphiesException
{
    public List<string> ValidationErrors { get; set; } = new();
    
    public NphiesValidationException(string message, List<string> errors) 
        : base(message)
    {
  ValidationErrors = errors;
    }
}
```

---

## ?? Week 2 Task Breakdown by Day

### Day 1 (Monday): HTTP Client Foundation
- ? **2 hours**: Set up HTTP Client infrastructure
- ? **2 hours**: Implement basic submit/get methods
- ? **2 hours**: Add Polly retry policies
- ? **1 hour**: Configuration setup

### Day 2 (Tuesday): Authentication
- ? **3 hours**: Implement OAuth2 authentication
- ? **2 hours**: Token caching and refresh
- ? **2 hours**: Integration with HTTP Client

### Day 3 (Wednesday): Polling Service
- ? **3 hours**: Implement polling logic
- ? **2 hours**: Task status handling
- ? **2 hours**: Timeout and error handling

### Day 4 (Thursday): Orchestration Service
- ? **4 hours**: Implement orchestration service
- ? **3 hours**: End-to-end integration testing

### Day 5 (Friday): Unit Tests & Error Handling
- ? **4 hours**: Write Week 1 unit tests (catch-up)
- ? **3 hours**: Write Week 2 unit tests

### Weekend (Optional): Polish & Documentation
- ? **2 hours**: Code review and refactoring
- ? **2 hours**: Update documentation
- ? **1 hour**: Week 2 completion report

---

## ?? Success Metrics

### Must Have (100% Completion)
- ? HTTP Client can submit bundles to NPHIES
- ? Authentication service working
- ? Polling service can retrieve async responses
- ? Orchestration service coordinates all operations
- ? Zero compilation errors
- ? At least 10 unit tests passing

### Nice to Have (Bonus)
- ? 15+ unit tests
- ? Integration tests with mock server
- ? FHIR validator integration
- ? Performance benchmarks
- ? Load testing results

---

## ?? Testing Strategy

### Unit Tests (Target: 15 tests)
- Mock HttpClient responses
- Mock authentication tokens
- Test retry logic
- Test timeout handling
- Test bundle creation/parsing

### Integration Tests (Stretch Goal)
- Use WireMock.Net for mock NPHIES server
- Test full eligibility flow
- Test full claim submission flow
- Test polling mechanism

### Manual Testing Checklist
- [ ] Submit eligibility request to NPHIES sandbox
- [ ] Verify response is parsed correctly
- [ ] Submit claim to NPHIES sandbox
- [ ] Test polling for async responses
- [ ] Verify authentication token refresh

---

## ?? Known Challenges & Mitigations

### Challenge 1: NPHIES Sandbox Access
**Risk**: May not have sandbox credentials  
**Mitigation**: 
- Use mock server (WireMock.Net) for development
- Create sample responses based on NPHIES spec
- Plan for sandbox testing once credentials available

### Challenge 2: OAuth2 Configuration
**Risk**: Complex authentication setup  
**Mitigation**:
- Start with basic auth if OAuth2 not ready
- Use IdentityModel library (well-documented)
- Have fallback to API key authentication

### Challenge 3: Async Polling Complexity
**Risk**: Polling logic can be tricky  
**Mitigation**:
- Start with simple polling (fixed interval)
- Add exponential backoff later if needed
- Use CancellationToken for timeout

### Challenge 4: Time Management
**Risk**: 7 tasks in 5 days is aggressive  
**Mitigation**:
- Focus on must-haves first (HTTP Client, Auth, Polling)
- Defer nice-to-haves to Week 3 if needed
- Unit tests can extend into weekend if necessary

---

## ?? Resources & Documentation

### NPHIES Documentation
- NPHIES Implementation Guide: https://nphies.sa/ig/
- FHIR R4 Specification: https://hl7.org/fhir/R4/
- OAuth2 Spec: https://oauth.net/2/

### NuGet Packages
- **Polly**: https://github.com/App-vNext/Polly
- **IdentityModel**: https://identitymodel.readthedocs.io/
- **Moq**: https://github.com/moq/moq4
- **FluentAssertions**: https://fluentassertions.com/

### Code Examples
- HttpClientFactory: https://learn.microsoft.com/en-us/dotnet/core/extensions/httpclient-factory
- Polly with HttpClient: https://learn.microsoft.com/en-us/dotnet/architecture/microservices/implement-resilient-applications/use-httpclientfactory-to-implement-resilient-http-requests

---

## ?? Week 2 Deliverables Checklist

### Code
- [ ] INphiesHttpClient interface
- [ ] NphiesHttpClient implementation
- [ ] IAuthenticationService interface
- [ ] NphiesAuthenticationService implementation
- [ ] IPollingService interface
- [ ] NphiesPollingService implementation
- [ ] INphiesOrchestrationService interface
- [ ] NphiesOrchestrationService implementation
- [ ] Custom exception classes
- [ ] Configuration classes

### Tests
- [ ] 9 FhirBundleService tests (Week 1 catch-up)
- [ ] 3 HttpClient tests
- [ ] 2 Authentication tests
- [ ] 2 Polling tests
- [ ] 1 Orchestration test
- [ ] All tests passing

### Documentation
- [ ] Week 2 progress summary
- [ ] API documentation for new services
- [ ] Configuration guide
- [ ] Testing guide

### Configuration
- [ ] appsettings.json structure
- [ ] appsettings.Development.json
- [ ] Environment variables documented

---

## ?? Week 2 Completion Criteria

? **Code Quality**
- Zero compilation errors
- Zero critical warnings
- All new services have XML documentation
- Follows established patterns from Week 1

? **Functionality**
- Can submit bundles to NPHIES (or mock server)
- Authentication working
- Polling logic functional
- Orchestration service coordinates operations

? **Testing**
- Minimum 15 unit tests
- All tests passing
- Code coverage > 60%

? **Documentation**
- All new interfaces documented
- Configuration guide complete
- Week 2 summary created

---

## ?? Ready to Start!

**Prerequisites**: ? All met (Week 1 complete)  
**Blockers**: None  
**Risk**: Medium (manageable)  
**Confidence**: High

**Next Step**: Begin Day 1 - HTTP Client Foundation

---

**Created**: January 2025  
**Status**: Ready for Implementation  
**Estimated Completion**: End of Week 2  
**Last Updated**: Now

---

## ?? Quick Help

**Stuck on HTTP Client?** ? Check Microsoft docs on HttpClientFactory  
**Stuck on Auth?** ? Review IdentityModel examples  
**Stuck on Polling?** ? Use simple loop first, optimize later  
**Stuck on Tests?** ? Review Moq documentation  

**Questions?** ? Review DEVELOPER_GUIDE.md or Week 1 docs for patterns

---

**Let's build this! ??**
