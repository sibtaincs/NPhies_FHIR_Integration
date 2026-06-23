# ?? SERVICE IMPLEMENTATION GUIDE - REPLACE MOCK DATA

**Phase**: Database Integration - Week 1
**Objective**: Remove all mock data from services and implement real database queries
**Estimated Time**: 40 hours

---

## ?? OVERVIEW

Your services currently use **mock data** for testing. This guide walks you through **replacing mock data with real database queries**.

**Services to Update:**
1. ? ClaimResponseProcessingService
2. ? DenialManagementService
3. ? AdjudicationWorkflowService
4. ? AppealWorkflowService
5. ? PaymentReconciliationService

---

## ?? SERVICE 1: ClaimResponseProcessingService

### Current State (Mock Data)
```csharp
var originalClaim = new Claim { Id = claimId }; // Mock - NO database call
```

### Target State (Database Query)
```csharp
var originalClaim = await _claimRepository.GetWithDetailsAsync(claimId);
if (originalClaim == null)
{
  throw new InvalidOperationException($"Claim {claimId} not found");
}
```

### Implementation Steps

#### Step 1: Inject Repository

**File**: `NPhies_FHIR_Integration.Application/Services/RCM/ClaimResponseProcessingService.cs`

```csharp
public class ClaimResponseProcessingService : IClaimResponseProcessingService
{
    private readonly ILogger<ClaimResponseProcessingService> _logger;
    private readonly IClaimResponseRepository _responseRepository;
    private readonly IClaimRepository _claimRepository;
    // ? Already injected - repositories are available

    public ClaimResponseProcessingService(
        ILogger<ClaimResponseProcessingService> logger,
     IClaimResponseRepository responseRepository,
        IClaimRepository claimRepository)
    {
     _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        _responseRepository = responseRepository ?? throw new ArgumentNullException(nameof(responseRepository));
        _claimRepository = claimRepository ?? throw new ArgumentNullException(nameof(claimRepository));
    }
}
```

#### Step 2: Remove TODOs and Add Database Calls

Replace this:
```csharp
// TODO: Get original claim from database
var originalClaim = new Claim { Id = claimId }; // Mock for now
```

With this:
```csharp
// Get original claim from database
var originalClaim = await _claimRepository.GetWithDetailsAsync(claimId);
if (originalClaim == null)
{
    _logger.LogWarning("Original claim {ClaimId} not found in database", claimId);
    result.IsSuccessful = false;
    result.StatusMessage = $"Claim {claimId} not found";
    result.Errors.Add($"Claim not found in database");
    return result;
}
```

#### Step 3: Implement Transaction for Persistence

After processing:
```csharp
// Save response to database
using (var transaction = await _responseRepository.BeginTransactionAsync())
{
    try
    {
   response.ClaimResponseStatus = "processed";
        response.CreatedDate = DateTime.UtcNow;
   
 await _responseRepository.AddAsync(response);
        await _responseRepository.SaveChangesAsync();

        // Save adjudication details
        foreach (var detail in adjudicationDetails)
        {
         await _adjudicationRepository.AddAsync(detail);
        }
        await _adjudicationRepository.SaveChangesAsync();
        
        await transaction.CommitAsync();
        
  _logger.LogInformation("Claim response {ResponseId} persisted successfully", response.Id);
    }
 catch (Exception ex)
    {
        await transaction.RollbackAsync();
        _logger.LogError(ex, "Failed to persist claim response");
        throw;
    }
}
```

---

## ?? SERVICE 2: DenialManagementService

### Current State (Mock Data)
```csharp
var denials = new List<DenialDetail>
{
    new DenialDetail { ClaimId = "CLM-001", ... }  // Mock data hardcoded
};
```

### Target State (Database Query)
```csharp
var denials = await _claimResponseRepository.GetDeniedItemsAsync(filter.FromDate, filter.ToDate);
```

### Implementation Steps

#### Step 1: Add Repository Methods

**File**: `NPhies_FHIR_Integration.Infrastructure/Repositories/ClaimRepositories.cs`

Add to `IClaimResponseRepository` interface:

```csharp
/// <summary>
/// Get all denied items within date range
/// </summary>
Task<List<DenialDetail>> GetDeniedItemsAsync(DateTime? fromDate = null, DateTime? toDate = null);

/// <summary>
/// Get denials by provider
/// </summary>
Task<List<DenialDetail>> GetDenialsByProviderAsync(string providerId, DateTime fromDate, DateTime toDate);

/// <summary>
/// Get high-value denials above threshold
/// </summary>
Task<List<DenialDetail>> GetHighValueDenialsAsync(decimal threshold);
```

#### Step 2: Implement Repository Methods

```csharp
public async Task<List<DenialDetail>> GetDeniedItemsAsync(DateTime? fromDate = null, DateTime? toDate = null)
{
    var deniedItems = new List<DenialDetail>();

    var query = _context.ClaimResponseAddItems.AsNoTracking();

    if (fromDate.HasValue)
      query = query.Where(ai => ai.ClaimResponse.CreatedAt >= fromDate.Value);

    if (toDate.HasValue)
        query = query.Where(ai => ai.ClaimResponse.CreatedAt <= toDate.Value);

    var addItems = await query
        .Include(ai => ai.ClaimResponse)
        .Include(ai => ai.Adjudications)
    .ToListAsync();

    foreach (var addItem in addItems)
    {
        var hasDenial = addItem.Adjudications?.Any(a => 
            a.AdjudicationCategory?.ToLower().Contains("deny") == true) ?? false;

        if (hasDenial)
        {
       deniedItems.Add(new DenialDetail
       {
       ItemSequence = addItem.Sequence,
            ServiceDescription = addItem.ProductOrServiceDisplay ?? "Unknown",
       DeniedAmount = (addItem.SubmittedAmount ?? 0) - (addItem.BenefitAmount ?? 0),
                CanAppeal = true,
         AppealDeadline = addItem.ClaimResponse.CreatedAt.AddDays(60),
       IsRecoverable = true,
          ProviderId = addItem.ClaimResponse.Requestor?.Id ?? "Unknown",
    PatientId = addItem.ClaimResponse.PatientId
          });
     }
    }

    return deniedItems;
}
```

#### Step 3: Update Service to Use Database

```csharp
public async Task<List<DenialDetail>> GetDenialsAsync(
    DenialFilter filter,
    CancellationToken cancellationToken = default)
{
    _logger.LogInformation("Getting denials from database for period {From} to {To}", 
        filter?.FromDate, filter?.ToDate);

    try
    {
        // Query from database
    var denials = await _claimResponseRepository.GetDeniedItemsAsync(
   filter?.FromDate, 
          filter?.ToDate);

    // Apply client-side filters
      var filtered = denials.AsEnumerable();

        if (!string.IsNullOrEmpty(filter?.ProviderId))
         filtered = filtered.Where(d => d.ProviderId == filter.ProviderId);

        if (filter?.MinAmount.HasValue == true)
         filtered = filtered.Where(d => d.DeniedAmount >= filter.MinAmount.Value);

        if (filter?.RecoverableOnly == true)
        filtered = filtered.Where(d => d.IsRecoverable);

        // Apply pagination
        var skip = ((filter?.PageNumber ?? 1) - 1) * (filter?.PageSize ?? 10);
var paginatedDenials = filtered.Skip(skip).Take(filter?.PageSize ?? 10).ToList();

        _logger.LogInformation("Retrieved {Count} denials from database", paginatedDenials.Count);
        return paginatedDenials;
    }
    catch (Exception ex)
    {
    _logger.LogError(ex, "Error retrieving denials from database");
        throw;
    }
}
```

---

## ?? SERVICE 3: AdjudicationWorkflowService

### Implementation Steps

#### Step 1: Inject Repositories

```csharp
public class AdjudicationWorkflowService : IAdjudicationWorkflowService
{
    private readonly IClaimRepository _claimRepository;
  private readonly IClaimResponseRepository _responseRepository;
    private readonly IAdjudicationDetailRepository _adjudicationRepository;
    private readonly ILogger<AdjudicationWorkflowService> _logger;

    public AdjudicationWorkflowService(
IClaimRepository claimRepository,
   IClaimResponseRepository responseRepository,
        IAdjudicationDetailRepository adjudicationRepository,
    ILogger<AdjudicationWorkflowService> logger)
    {
        _claimRepository = claimRepository ?? throw new ArgumentNullException(nameof(claimRepository));
      _responseRepository = responseRepository ?? throw new ArgumentNullException(nameof(responseRepository));
     _adjudicationRepository = adjudicationRepository ?? throw new ArgumentNullException(nameof(adjudicationRepository));
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
    }
}
```

#### Step 2: Implement Adjudication Logic with Database

```csharp
public async Task<AdjudicationWorkflowResult> ProcessAdjudicationAsync(
    Claim claim,
    Coverage coverage,
    ClaimResponse response,
    CancellationToken cancellationToken = default)
{
    _logger.LogInformation("Processing adjudication for claim {ClaimId}", claim.Id);

    var result = new AdjudicationWorkflowResult
    {
        ClaimId = claim.Id,
        OverallStatus = "pending"
    };

    try
    {
        // Get claim from database with all details
        var fullClaim = await _claimRepository.GetWithDetailsAsync(claim.Id);
   if (fullClaim == null)
            throw new InvalidOperationException($"Claim {claim.Id} not found");

   // Apply adjudication rules
     var adjudications = await ApplyAdjudicationRulesAsync(fullClaim, coverage);

        // Save adjudications to database
      using (var transaction = await _adjudicationRepository.BeginTransactionAsync())
        {
  try
   {
  foreach (var adjudication in adjudications)
                {
         await _adjudicationRepository.AddAsync(adjudication);
     }
          await _adjudicationRepository.SaveChangesAsync();

// Update claim status
        fullClaim.Status = "adjudicated";
    await _claimRepository.UpdateAsync(fullClaim);
       await _claimRepository.SaveChangesAsync();

         await transaction.CommitAsync();

  result.OverallStatus = "approved";
              result.IsSuccessful = true;
       }
      catch
       {
      await transaction.RollbackAsync();
           throw;
          }
        }

        return result;
    }
    catch (Exception ex)
    {
        _logger.LogError(ex, "Adjudication processing failed for claim {ClaimId}", claim.Id);
     result.IsSuccessful = false;
        result.OverallStatus = "failed";
        result.Errors.Add(ex.Message);
        return result;
    }
}

private async Task<List<ClaimResponseAdjudication>> ApplyAdjudicationRulesAsync(
    Claim claim,
    Coverage coverage)
{
    var adjudications = new List<ClaimResponseAdjudication>();

    foreach (var item in claim.Items)
    {
     // Apply deductible rule
        if (coverage.DeductibleMet < coverage.AnnualDeductible)
        {
            var deductibleOwed = Math.Min(
                item.Net ?? 0,
       coverage.AnnualDeductible - coverage.DeductibleMet);

      adjudications.Add(new ClaimResponseAdjudication
          {
                Id = Guid.NewGuid().ToString(),
    AdjudicationCategory = "deductible",
             Amount = deductibleOwed,
        CreatedAt = DateTime.UtcNow
    });
        }

     // Apply coinsurance rule
 var coinsuranceAmount = (item.Net ?? 0) * (coverage.CoinsurancePercent / 100m);
   adjudications.Add(new ClaimResponseAdjudication
   {
            Id = Guid.NewGuid().ToString(),
            AdjudicationCategory = "coinsurance",
            Amount = coinsuranceAmount,
    CreatedAt = DateTime.UtcNow
        });
    }

    return adjudications;
}
```

---

## ?? SERVICE 4: AppealWorkflowService

### Implementation Steps

#### Step 1: Create Appeal Repository

**File**: `NPhies_FHIR_Integration.Infrastructure/Repositories/AppealRepository.cs`

```csharp
public interface IAppealRepository : IRepository<Appeal>
{
    Task<Appeal> GetByIdWithDetailsAsync(string appealId);
    Task<List<Appeal>> GetByClaimIdAsync(string claimId);
    Task<List<Appeal>> GetByStatusAsync(string status);
    Task<List<Appeal>> GetPendingAppealsAsync();
}

public class AppealRepository : Repository<Appeal>, IAppealRepository
{
    private readonly ApplicationDbContext _context;

    public AppealRepository(ApplicationDbContext context) : base(context)
    {
   _context = context;
    }

  public async Task<Appeal> GetByIdWithDetailsAsync(string appealId)
    {
    return await _context.Appeals
            .AsNoTracking()
          .Include(a => a.Claim)
       .Include(a => a.DeniedItem)
    .FirstOrDefaultAsync(a => a.Id == appealId);
    }

    public async Task<List<Appeal>> GetByClaimIdAsync(string claimId)
    {
        return await _context.Appeals
  .AsNoTracking()
            .Where(a => a.ClaimId == claimId)
      .OrderByDescending(a => a.CreatedAt)
      .ToListAsync();
    }

    public async Task<List<Appeal>> GetByStatusAsync(string status)
    {
    return await _context.Appeals
         .AsNoTracking()
   .Where(a => a.Status == status)
     .OrderByDescending(a => a.CreatedAt)
     .ToListAsync();
    }

    public async Task<List<Appeal>> GetPendingAppealsAsync()
    {
        return await _context.Appeals
     .AsNoTracking()
   .Where(a => a.Status == "pending" || a.Status == "submitted")
      .OrderBy(a => a.AppealDeadline)
          .ToListAsync();
    }
}
```

#### Step 2: Update Appeal Service

```csharp
public class AppealWorkflowService : IAppealWorkflowService
{
    private readonly IAppealRepository _appealRepository;
    private readonly IClaimRepository _claimRepository;
    private readonly ILogger<AppealWorkflowService> _logger;

    public AppealWorkflowService(
        IAppealRepository appealRepository,
        IClaimRepository claimRepository,
        ILogger<AppealWorkflowService> logger)
    {
        _appealRepository = appealRepository ?? throw new ArgumentNullException(nameof(appealRepository));
        _claimRepository = claimRepository ?? throw new ArgumentNullException(nameof(claimRepository));
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
    }

    public async Task<AppealSubmissionResult> SubmitAppealAsync(
        string claimId,
        string denialReason,
        string appealReason,
        CancellationToken cancellationToken = default)
    {
        _logger.LogInformation("Submitting appeal for claim {ClaimId}", claimId);

   var result = new AppealSubmissionResult
  {
    IsSuccessful = false
        };

        try
        {
    // Get claim from database
            var claim = await _claimRepository.GetWithDetailsAsync(claimId);
  if (claim == null)
                throw new InvalidOperationException($"Claim {claimId} not found");

   // Create appeal
            var appeal = new Appeal
{
                Id = Guid.NewGuid().ToString(),
   ClaimId = claimId,
    Status = "submitted",
    AppealReason = appealReason,
    DenialReason = denialReason,
  AppealDate = DateTime.UtcNow,
  AppealDeadline = DateTime.UtcNow.AddDays(60),
         CreatedAt = DateTime.UtcNow,
   IsActive = true
      };

      // Save to database
            await _appealRepository.AddAsync(appeal);
        await _appealRepository.SaveChangesAsync();

            result.IsSuccessful = true;
       result.AppealId = appeal.Id;
      result.StatusMessage = "Appeal submitted successfully";

            _logger.LogInformation("Appeal {AppealId} submitted for claim {ClaimId}", appeal.Id, claimId);

         return result;
        }
        catch (Exception ex)
        {
     _logger.LogError(ex, "Error submitting appeal for claim {ClaimId}", claimId);
  result.StatusMessage = ex.Message;
            result.Errors.Add(ex.Message);
   return result;
      }
    }

    public async Task<AppealStatus> GetAppealStatusAsync(
        string appealId,
      CancellationToken cancellationToken = default)
    {
        var appeal = await _appealRepository.GetByIdWithDetailsAsync(appealId);
 if (appeal == null)
            throw new InvalidOperationException($"Appeal {appealId} not found");

        return new AppealStatus
      {
       AppealId = appeal.Id,
  Status = appeal.Status,
            AppealDate = appeal.AppealDate,
            AppealDeadline = appeal.AppealDeadline,
       ClaimId = appeal.ClaimId,
     Reason = appeal.AppealReason
     };
    }
}
```

---

## ?? IMPLEMENTATION CHECKLIST

### Services to Update

- [ ] **ClaimResponseProcessingService**
  - [ ] Inject repositories
  - [ ] Remove mock Claim creation
  - [ ] Query claim from database
  - [ ] Save response to database
  - [ ] Save adjudications
  - [ ] Use transactions

- [ ] **DenialManagementService**
  - [ ] Inject repository
  - [ ] Remove mock denial list
  - [ ] Query denials from database
  - [ ] Apply filters client-side
  - [ ] Implement pagination

- [ ] **AdjudicationWorkflowService**
  - [ ] Inject claim and adjudication repositories
  - [ ] Query claim from database
  - [ ] Apply rules to adjudicate
  - [ ] Save adjudications
  - [ ] Update claim status

- [ ] **AppealWorkflowService**
  - [ ] Create Appeal entity (if missing)
  - [ ] Create AppealRepository
  - [ ] Register in DI
  - [ ] Implement submit appeal
  - [ ] Implement get status

- [ ] **PaymentReconciliationService**
  - [ ] Inject payment repository
  - [ ] Query reconciliation data
  - [ ] Match payments to claims
  - [ ] Calculate variances
  - [ ] Generate reports from data

---

## ?? TESTING IMPLEMENTATION

### Unit Test Template

```csharp
[Fact]
public async Task ProcessAdjudication_WithValidClaim_ShouldPersistToDB()
{
    // Arrange
    var claim = new Claim 
    { 
  Id = "CLM-001",
        Status = "pending"
    };
    
    var mockClaimRepository = new Mock<IClaimRepository>();
    mockClaimRepository
    .Setup(r => r.GetWithDetailsAsync("CLM-001"))
        .ReturnsAsync(claim);

    var service = new AdjudicationWorkflowService(
        mockClaimRepository.Object,
  _responseRepository,
        _logger);

    // Act
    var result = await service.ProcessAdjudicationAsync(claim, coverage, response);

    // Assert
    Assert.True(result.IsSuccessful);
    mockClaimRepository.Verify(r => r.GetWithDetailsAsync("CLM-001"), Times.Once);
}
```

---

## ?? EXECUTION ORDER

**Week 1:**
1. Day 1: Update ClaimResponseProcessingService
2. Day 2: Update DenialManagementService
3. Day 3: Update AdjudicationWorkflowService
4. Day 4: Implement AppealWorkflowService
5. Day 5: Update PaymentReconciliationService

**Week 2:**
1. Write unit tests
2. Perform integration tests
3. Performance tune queries
4. Add caching where needed

---

**Next Document**: Testing Implementation Guide

