# NPHIES FHIR Integration - Developer Guide

## ?? Table of Contents
1. [Architecture Overview](#architecture-overview)
2. [Project Structure](#project-structure)
3. [Technology Stack](#technology-stack)
4. [Installation & Setup](#installation--setup)
5. [Development Guide](#development-guide)
6. [API Reference](#api-reference)
7. [Database](#database)
8. [Testing](#testing)
9. [Deployment](#deployment)
10. [Performance & Monitoring](#performance--monitoring)
11. [Security](#security)
12. [Troubleshooting](#troubleshooting)

---

## Architecture Overview

### System Architecture

```
???????????????????????????????????????????????????????????
?   API Gateway      ?
?          (Rate Limiting, Auth, Routing)                 ?
???????????????????????????????????????????????????????????
      ?
        ???????????????????????????
        ?  ?            ?
    ?????????  ???????????  ??????????
    ? Cache ?  ? Database ?  ? Queue  ?
    ?????????  ???????????  ??????????
    ?   ?   ?
        ???????????????????????????
           ?
    ????????????????????????????????????
    ?      Application Services     ?
    ?  ???????????????????????????????  ?
    ?  ? Validation Services (Phase1)?  ?
    ?  ? RCM Services (Phase2A)      ?  ?
?  ? Analytics Services (Phase2B)?  ?
    ?  ? Infrastructure (Phase2C)    ?  ?
    ?  ???????????????????????????????  ?
    ???????????????????????????????????????
     ?
      ???? External Systems (FHIR, NPHIES)
```

### Key Design Patterns

- **Service-Based Architecture**: 43 independent services
- **Dependency Injection**: Loose coupling, easy testing
- **Async/Await**: All I/O operations are asynchronous
- **Comprehensive Logging**: ILogger throughout
- **Error Handling**: Try-catch on all operations
- **Repository Pattern**: Data access abstraction

---

## Project Structure

```
NPhies_FHIR_Integration/
??? NPhies_FHIR_Integration.Application/
? ??? Services/
?       ??? RCM/
?       ?   ??? AdjudicationRulesEngine.cs
?       ?   ??? BenefitDeterminationEngine.cs
?  ?   ??? EnhancedPaymentCalculationEngine.cs
?   ?   ??? DenialManagementService.cs
?       ???? AppealWorkflowService.cs
??   ??? ClaimResponseProcessingService.cs
?       ?   ??? StatusTrackingAndBatchServices.cs
?  ?   ??? AdvancedRulesServices.cs
?       ?   ??? Analytics/
?       ?       ??? ComplianceAndPerformanceServices.cs
?       ? ??? ErrorAndDemographicsAnalyticsServices.cs
?       ?     ??? NetworkAndFinancialAnalyticsServices.cs
?       ?       ??? CustomReportBuilderService.cs
?       ??? Validation/
?           ??? ClaimValidationService.cs
?        ??? EligibilityValidationService.cs
?   ??? MessageFormatComplianceService.cs
?  ??? ErrorCodeStandardizationService.cs
? ??? ProviderCredentialService.cs
?           ??? PatientDemographicsService.cs
??? NPhies_FHIR_Integration.Domain/
?   ??? Entities/
?   ??? ValueObjects/
?   ??? Interfaces/
?   ??? Constants/
??? NPhies_FHIR_Integration.Infrastructure/
?   ??? Persistence/
?   ??? ExternalServices/
?   ??? Utilities/
??? NPhies_FHIR_Integration.ApiService/
?   ??? Controllers/
?   ??? Middleware/
?   ??? Program.cs
??? README.md
```

---

## Technology Stack

### Core Technologies
- **Language**: C# 12
- **Framework**: .NET 9
- **Architecture**: Clean Architecture / Microservices

### Data & Storage
- **Primary DB**: SQL Server 2022+
- **Caching**: Distributed Cache (Redis compatible)
- **Message Queue**: RabbitMQ / Azure Service Bus

### API & Communication
- **API Style**: RESTful with FHIR
- **Serialization**: JSON
- **Authentication**: OAuth 2.0 / API Keys

### Monitoring & Logging
- **Logging**: ILogger / Serilog
- **Monitoring**: Application Insights / Prometheus
- **Distributed Tracing**: Application Insights

### Testing
- **Unit Tests**: xUnit
- **Integration Tests**: TestContainers
- **Mocking**: Moq

### DevOps
- **CI/CD**: GitHub Actions
- **Containerization**: Docker
- **Orchestration**: Kubernetes
- **Cloud**: Azure / AWS

---

## Installation & Setup

### Prerequisites
- .NET 9 SDK or later
- Visual Studio 2022 / VS Code
- SQL Server 2022+ or LocalDB
- Git
- Docker (optional, for local development)

### Step 1: Clone Repository
```bash
git clone https://github.com/sibtaincs/NPhies_FHIR_Integration.git
cd NPhies_FHIR_Integration
git checkout phase-2/advanced-features
```

### Step 2: Install Dependencies
```bash
# Restore NuGet packages
dotnet restore

# Build solution
dotnet build
```

### Step 3: Configure Database

**Option A: Using LocalDB**
```bash
# Create migrations
dotnet ef migrations add Initial -p NPhies_FHIR_Integration.Infrastructure -s NPhies_FHIR_Integration.ApiService

# Update database
dotnet ef database update -p NPhies_FHIR_Integration.Infrastructure -s NPhies_FHIR_Integration.ApiService
```

**Option B: Using SQL Server**
```
Connection String: "Server=YOUR_SERVER;Database=NPhiesDB;Trusted_Connection=true;"
```

### Step 4: Configure Secrets
```bash
# Initialize user secrets
dotnet user-secrets init

# Add secrets
dotnet user-secrets set "Jwt:Key" "your-secret-key"
dotnet user-secrets set "Jwt:Issuer" "nphies-issuer"
dotnet user-secrets set "Database:ConnectionString" "your-connection-string"
```

### Step 5: Run Application
```bash
# Development
dotnet run --project NPhies_FHIR_Integration.ApiService

# Production
dotnet run --project NPhies_FHIR_Integration.ApiService --configuration Release
```

**API will be available at**: https://localhost:5001

---

## Development Guide

### Adding a New Service

#### Step 1: Create Interface
```csharp
namespace NPhies_FHIR_Integration.Application.Services.RCM
{
    public interface IMyNewService
    {
        Task<MyResult> ProcessAsync(MyRequest request);
    }
}
```

#### Step 2: Create Implementation
```csharp
public class MyNewService : IMyNewService
{
    private readonly ILogger<MyNewService> _logger;

    public MyNewService(ILogger<MyNewService> logger)
    {
      _logger = logger;
    }

    public async Task<MyResult> ProcessAsync(MyRequest request)
 {
      try
      {
   _logger.LogInformation("Processing request");
      
    // Your implementation here
    
       return result;
        }
        catch (Exception ex)
        {
  _logger.LogError(ex, "Error processing request");
            throw;
        }
    }
}
```

#### Step 3: Register in DI Container
```csharp
// In Program.cs or Startup.cs
builder.Services.AddScoped<IMyNewService, MyNewService>();
```

#### Step 4: Use in Controller/Service
```csharp
[ApiController]
[Route("api/[controller]")]
public class MyController : ControllerBase
{
    private readonly IMyNewService _service;

    public MyController(IMyNewService service)
    {
        _service = service;
    }

 [HttpPost]
    public async Task<IActionResult> Process([FromBody] MyRequest request)
    {
    var result = await _service.ProcessAsync(request);
        return Ok(result);
    }
}
```

### Coding Standards

#### Naming Conventions
- Classes: PascalCase (MyClassName)
- Methods: PascalCase (MyMethodName)
- Properties: PascalCase (MyProperty)
- Fields: camelCase (_myField)
- Constants: UPPER_CASE (MY_CONSTANT)

#### Code Style
```csharp
// Always use async/await
public async Task<Result> ProcessAsync()
{
 // Implementation
}

// Use null-coalescing operator
var value = nullableValue ?? defaultValue;

// Use null-conditional operator
var length = obj?.Property?.Length ?? 0;

// Use expression-bodied members where appropriate
public bool IsValid => !string.IsNullOrWhiteSpace(Value);
```

#### Documentation
```csharp
/// <summary>
/// Brief description of what this method does
/// </summary>
/// <param name="parameter1">Description of parameter1</param>
/// <returns>Description of return value</returns>
public async Task<Result> MethodNameAsync(string parameter1)
{
    // Implementation
}
```

### Error Handling

#### Standard Pattern
```csharp
try
{
    _logger.LogInformation("Starting operation");
    
    // Do work
    
    return result;
}
catch (ValidationException ex)
{
    _logger.LogWarning(ex, "Validation failed");
    return new Result { Success = false, Errors = ex.Errors };
}
catch (Exception ex)
{
    _logger.LogError(ex, "Unexpected error");
    throw;
}
```

#### Custom Exceptions
```csharp
public class DomainException : Exception
{
    public DomainException(string message) : base(message) { }
}

public class ValidationException : DomainException
{
    public List<string> Errors { get; set; }
}
```

---

## API Reference

### Base URL
```
https://api.nphies-fhir.health/api
```

### Authentication
```
Authorization: Bearer {token}
or
X-API-Key: {api-key}
```

### Common Response Format
```json
{
"success": true,
  "data": {},
  "errors": [],
  "timestamp": "2024-01-15T10:30:00Z"
}
```

### Key Endpoints

#### Claims
```
POST   /claims       - Submit claim
GET    /claims/{id}         - Get claim details
GET    /claims         - List claims (paginated)
PUT    /claims/{id}     - Update claim
DELETE /claims/{id}         - Cancel claim
```

#### Adjudication
```
POST   /adjudication - Adjudicate claim
GET    /adjudication/{id}   - Get adjudication result
```

#### Analytics
```
GET    /analytics/dashboard - Get dashboard metrics
GET    /analytics/reports   - Get available reports
POST   /analytics/reports   - Generate custom report
```

### Pagination
```
GET /claims?page=1&pageSize=50&sort=createdDate&order=desc
```

### Filtering
```
GET /claims?status=approved&providerId=P001&dateFrom=2024-01-01&dateTo=2024-01-31
```

---

## Database

### Schema Overview

#### Tables
- Claims
- ClaimItems
- Eligibility
- Benefits
- Adjudication
- Appeals
- Payments
- AuditLogs
- Providers
- Patients

### Database Scripts

#### Backup
```sql
BACKUP DATABASE [NPhiesDB] 
TO DISK = 'C:\Backups\NPhiesDB.bak';
```

#### Restore
```sql
RESTORE DATABASE [NPhiesDB] 
FROM DISK = 'C:\Backups\NPhiesDB.bak';
```

#### Optimization
```sql
-- Rebuild indexes
ALTER INDEX ALL ON Claims REBUILD;

-- Update statistics
UPDATE STATISTICS Claims;

-- Check fragmentation
SELECT * FROM sys.dm_db_index_physical_stats(...);
```

---

## Testing

### Unit Tests
```bash
# Run all tests
dotnet test

# Run specific test class
dotnet test --filter "ClassName=MyServiceTests"

# Run with coverage
dotnet test /p:CollectCoverage=true
```

### Sample Unit Test
```csharp
[Fact]
public async Task ProcessAsync_WithValidRequest_ReturnsSuccess()
{
    // Arrange
    var service = new MyService(mockLogger);
    var request = new MyRequest { /* ... */ };

    // Act
    var result = await service.ProcessAsync(request);

    // Assert
Assert.True(result.Success);
    Assert.Empty(result.Errors);
}
```

### Integration Tests
```bash
# Run integration tests only
dotnet test --filter "Category=Integration"
```

### Performance Tests
```bash
# Run load tests
dotnet test --filter "Category=Performance"
```

---

## Deployment

### Docker Deployment

#### Build Image
```bash
docker build -t nphies-fhir:latest .
```

#### Run Container
```bash
docker run -p 5001:5001 \
  -e "Database__ConnectionString=..." \
  -e "Jwt__Key=..." \
  nphies-fhir:latest
```

### Kubernetes Deployment

#### Deploy
```bash
kubectl apply -f k8s/deployment.yaml
kubectl apply -f k8s/service.yaml
```

#### Check Status
```bash
kubectl get pods
kubectl logs pod-name
```

### Azure Deployment

#### Using Azure CLI
```bash
# Create resource group
az group create -n nphies-rg -l eastus

# Create app service
az appservice plan create -n nphies-plan -g nphies-rg

# Deploy
az webapp up -n nphies-app -g nphies-rg --sku B1
```

### GitHub Actions CI/CD

See `.github/workflows/` for automated build and deployment pipelines.

---

## Performance & Monitoring

### Performance Metrics

Target performance levels:
- **Average Response Time**: < 200ms
- **P95 Response Time**: < 500ms
- **P99 Response Time**: < 1000ms
- **Requests Per Second**: 500+
- **Error Rate**: < 0.1%

### Monitoring Tools

#### Application Insights
```csharp
builder.Services.AddApplicationInsightsTelemetry();
```

#### Prometheus Metrics
```bash
# Access metrics at http://localhost:5001/metrics
```

#### Health Checks
```bash
GET /health         - System health
GET /health/live               - Liveness check
GET /health/ready              - Readiness check
```

### Performance Optimization

#### Caching Strategy
```csharp
// Set cache with expiration
await _cachingService.SetCacheAsync(
    "claim:" + claimId, 
    claim,
    TimeSpan.FromMinutes(30)
);
```

#### Database Optimization
```sql
-- Create indexes for frequently queried columns
CREATE INDEX IX_Claims_ProviderId ON Claims(ProviderId);
CREATE INDEX IX_Claims_PatientId ON Claims(PatientId);
CREATE INDEX IX_Claims_Status ON Claims(Status);
```

#### Async/Await Best Practices
```csharp
// Good: Async all the way
public async Task<Result> ProcessAsync()
{
    var data = await _repository.GetAsync(id);
    var processed = await _processor.ProcessAsync(data);
    return processed;
}

// Bad: Blocking async
public async Task<Result> ProcessAsync()
{
    var data = _repository.GetAsync(id).Result;  // Don't do this
    return data;
}
```

---

## Security

### Authentication & Authorization

#### JWT Bearer Token
```csharp
builder.Services
    .AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        options.TokenValidationParameters = new TokenValidationParameters
     {
       ValidateIssuer = true,
       ValidateAudience = true,
   ValidateLifetime = true,
            ValidateIssuerSigningKey = true
        };
    });
```

#### API Key Authentication
```csharp
[ApiKey]
[HttpGet("secured-endpoint")]
public IActionResult SecuredEndpoint()
{
    return Ok("Secure data");
}
```

### Data Encryption

#### In Transit
- All APIs use HTTPS/TLS 1.3+
- Certificate pinning for sensitive endpoints

#### At Rest
```csharp
public class EncryptionService : IEncryptionService
{
    public async Task<string> EncryptAsync(string plaintext)
    {
        // AES-256 encryption
    }

    public async Task<string> DecryptAsync(string ciphertext)
    {
        // AES-256 decryption
    }
}
```

### Security Best Practices

1. **Never Log Sensitive Data**
```csharp
// Bad
_logger.LogInformation($"Processing SSN: {ssn}");

// Good
_logger.LogInformation("Processing patient record");
```

2. **Validate All Inputs**
```csharp
if (string.IsNullOrWhiteSpace(input))
    throw new ValidationException("Input required");
```

3. **Use Parameterized Queries**
```csharp
// Safe - parameterized query
var patients = await _context.Patients
    .Where(p => p.Id == patientId)
    .ToListAsync();
```

4. **Handle Secrets Securely**
- Use Azure Key Vault
- Use User Secrets for development
- Never commit secrets to repository

---

## Troubleshooting

### Build Issues

#### Issue: NuGet Restore Fails
```bash
# Clear NuGet cache
nuget locals all -clear

# Restore again
dotnet restore
```

#### Issue: Database Connection
```
Ensure connection string is correct in appsettings.json
Or use User Secrets for sensitive data
```

### Runtime Issues

#### Issue: Service Not Responding
1. Check service logs: `dotnet run > log.txt`
2. Verify database connection
3. Check firewall rules
4. Review error in Application Insights

#### Issue: High Memory Usage
```csharp
// Check for memory leaks
// Monitor cache usage
// Review large object heap usage
```

#### Issue: Slow Queries
```bash
# Enable query logging
SET STATISTICS IO ON;
SET STATISTICS TIME ON;

# Analyze execution plan
```

### Common Errors

| Error | Cause | Solution |
|-------|-------|----------|
| 401 Unauthorized | Invalid token | Regenerate JWT token |
| 404 Not Found | Endpoint/resource missing | Verify URL, check logs |
| 500 Internal Server | Unhandled exception | Check application logs |
| TimeoutException | Query too slow | Optimize query, add index |

---

## Contributing

### Pull Request Process
1. Create feature branch: `git checkout -b feature/my-feature`
2. Make changes following coding standards
3. Write tests for new functionality
4. Update documentation
5. Create pull request with description
6. Pass code review
7. Merge to main/develop

### Code Review Checklist
- [ ] Code follows naming conventions
- [ ] Tests written and passing
- [ ] Documentation updated
- [ ] No breaking changes
- [ ] Performance impact considered
- [ ] Security reviewed

---

## Additional Resources

### Documentation
- [FHIR R4 Specification](http://hl7.org/fhir/R4/)
- [NPHIES Documentation](https://nphies.sa/)
- [.NET 9 Documentation](https://learn.microsoft.com/en-us/dotnet/)

### Tools & Utilities
- [Swagger UI](https://localhost:5001/swagger)
- [API Documentation](https://localhost:5001/api-docs)
- [Health Checks](https://localhost:5001/health)

---

## Version Information

**Project Version**: 1.0.0  
**.NET Version**: 9.0  
**Last Updated**: January 2024  
**Status**: Production Ready

---

**For questions or issues, please open a GitHub issue or contact the development team.**
