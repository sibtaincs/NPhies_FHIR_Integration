# ?? DAYS 6-7 IMPLEMENTATION GUIDE - START HERE

**Current Status**: 77.5% compliance (Phase 3: 40% complete)  
**This Session Target**: Implement Appeal Workflow + RCM Controller  
**Expected Result**: 78.5% compliance, 700+ lines of code, 32+ tests  
**Timeline**: 8-16 hours  

---

## ?? WHAT TO BUILD IN DAYS 6-7

### The Two Main Components

```
1. AppealWorkflowService
   ?? Service class with 6 methods
   ?? 11+ unit tests
   ?? Database integration
   ?? 350+ lines of code

2. RCMController
   ?? API Controller with 7 endpoints
   ?? 21+ integration tests
   ?? Swagger documentation
   ?? 300+ lines of code
   
TOTAL: 700+ lines, 32+ tests
```

---

## ?? STEP-BY-STEP IMPLEMENTATION

### STEP 1: Implement AppealWorkflowService (6 methods)

**Location**: `NPhies_FHIR_Integration.Application/Services/RCM/AppealWorkflowService.cs`

**Method 1: SubmitAppealAsync()**
```csharp
public async Task<AppealSubmissionResult> SubmitAppealAsync(
    string claimId,
    string denialReason,
    string appealReason,
    CancellationToken cancellationToken = default)
{
    // 1. Validate claim exists and has denied items
    // 2. Check appeal deadline (60 days from denial)
    // 3. Create Appeal entity
    // 4. Save to database
    // 5. Generate confirmation number
    // 6. Return AppealSubmissionResult with deadline
    
    // Expected test: SubmitAppealAsync_WithValidClaim_ReturnsSuccess
    // Expected test: SubmitAppealAsync_WithExpiredDeadline_ReturnsFail
}
```

**Method 2: GetAppealStatusAsync()**
```csharp
public async Task<AppealStatus> GetAppealStatusAsync(
    string appealId,
  CancellationToken cancellationToken = default)
{
    // 1. Query Appeal from database
    // 2. Get current status (submitted ? under review ? approved/denied)
    // 3. Calculate days since submission
    // 4. Get supporting documents
    // 5. Return AppealStatus
 
    // Expected test: GetAppealStatusAsync_ReturnsCorrectStatus
}
```

**Method 3: AddSupportingDocumentationAsync()**
```csharp
public async Task<bool> AddSupportingDocumentationAsync(
    string appealId,
    byte[] document,
    string documentType,
    CancellationToken cancellationToken = default)
{
    // 1. Validate document (not null, not too large)
    // 2. Save document (database or blob storage)
    // 3. Add reference to Appeal
    // 4. Update timestamp
// 5. Return success
  
    // Expected test: AddSupportingDocumentationAsync_SavesDocument
}
```

**Method 4: GenerateAppealLetterAsync()**
```csharp
public async Task<byte[]> GenerateAppealLetterAsync(
    Appeal appeal,
    CancellationToken cancellationToken = default)
{
    // 1. Load letter template
    // 2. Fill with appeal details (claim ID, denial reason, appeal reason)
    // 3. Add appeal deadline info
    // 4. Generate PDF
    // 5. Return PDF bytes
  
    // Expected test: GenerateAppealLetterAsync_CreatesPDF
    // Note: Use iTextSharp or similar for PDF generation
}
```

**Method 5: GetAppealDeadlineAsync()**
```csharp
public async Task<DateTime> GetAppealDeadlineAsync(
    string claimId,
    CancellationToken cancellationToken = default)
{
    // 1. Get claim response date
    // 2. Add 60 days
    // 3. Adjust for weekends/holidays if needed
    // 4. Return deadline
    
    // Expected test: GetAppealDeadlineAsync_CalculatesCorrectly
}
```

**Method 6: GetAppealMetricsAsync()**
```csharp
public async Task<AppealMetrics> GetAppealMetricsAsync(
    DateTime fromDate,
    DateTime toDate,
    CancellationToken cancellationToken = default)
{
    // 1. Query appeals submitted in date range
    // 2. Count by status (submitted, approved, denied, pending)
    // 3. Calculate approval rate
    // 4. Sum recovered amounts
    // 5. Calculate average days to resolution
    // 6. Identify top appeal reasons
    
    // Expected test: GetAppealMetricsAsync_ReturnsMetrics
}
```

---

### STEP 2: Create RCMController (7 endpoints)

**Location**: `NPhies_FHIR_Integration.ApiService/Controllers/RCMController.cs`

**Create the controller file:**

```csharp
[ApiController]
[Route("api/[controller]")]
[Authorize]  // Require authentication
public class RCMController : ControllerBase
{
    private readonly IClaimResponseProcessingService _claimProcessor;
  private readonly IAdjudicationWorkflowService _adjudication;
    private readonly IAppealWorkflowService _appeals;
    private readonly IDenialManagementService _denials;
    private readonly IPaymentReconciliationService _reconciliation;
    private readonly ILogger<RCMController> _logger;

    public RCMController(
        IClaimResponseProcessingService claimProcessor,
IAdjudicationWorkflowService adjudication,
        IAppealWorkflowService appeals,
        IDenialManagementService denials,
        IPaymentReconciliationService reconciliation,
        ILogger<RCMController> logger)
  {
        _claimProcessor = claimProcessor;
   _adjudication = adjudication;
        _appeals = appeals;
   _denials = denials;
        _reconciliation = reconciliation;
        _logger = logger;
    }

    // 7 endpoints to implement...
}
```

**Endpoint 1: POST /api/rcm/process-response**
```csharp
/// <summary>
/// Process claim response from payer
/// </summary>
[HttpPost("process-response")]
public async Task<ActionResult<ClaimResponseProcessingResult>> ProcessClaimResponse(
    [FromQuery] string claimId,
    [FromBody] ClaimResponse response)
{
    try
  {
        // 1. Validate inputs
        if (string.IsNullOrEmpty(claimId)) return BadRequest("ClaimId required");
        if (response == null) return BadRequest("Response required");
        
        // 2. Get original claim from database
        var claim = await /* get claim from repository */;
        if (claim == null) return NotFound("Claim not found");
        
        // 3. Process response
        var result = await _claimProcessor.ProcessClaimResponseAsync(
    response, claim);
        
        // 4. Return result
        return Ok(result);
    }
    catch (Exception ex)
  {
    _logger.LogError(ex, "Error processing claim response");
        return StatusCode(500, "Error processing claim response");
    }
}
```

**Endpoint 2: POST /api/rcm/adjudicate**
```csharp
/// <summary>
/// Run adjudication on claim
/// </summary>
[HttpPost("adjudicate")]
public async Task<ActionResult<AdjudicationWorkflowResult>> ProcessAdjudication(
    [FromQuery] string claimId)
{
    // Similar pattern: validate ? get data ? process ? return
}
```

**Endpoint 3: POST /api/rcm/appeals**
```csharp
/// <summary>
/// Submit appeal for denied claim
/// </summary>
[HttpPost("appeals")]
public async Task<ActionResult<AppealSubmissionResult>> SubmitAppeal(
    [FromQuery] string claimId,
    [FromBody] AppealRequest request)
{
    // Input validation ? call service ? return result
}
```

**Endpoint 4: GET /api/rcm/appeals/{appealId}**
```csharp
/// <summary>
/// Get appeal status
/// </summary>
[HttpGet("appeals/{appealId}")]
public async Task<ActionResult<AppealStatus>> GetAppealStatus(string appealId)
{
    // Validate ID ? call service ? return status
}
```

**Endpoint 5: GET /api/rcm/denials**
```csharp
/// <summary>
/// Get denials with optional filtering
/// </summary>
[HttpGet("denials")]
public async Task<ActionResult<List<DenialDetail>>> GetDenials(
    [FromQuery] DenialFilter filter)
{
    // Call denial service ? return filtered denials
}
```

**Endpoint 6: GET /api/rcm/reconciliation**
```csharp
/// <summary>
/// Get payment reconciliation report
/// </summary>
[HttpGet("reconciliation")]
public async Task<ActionResult<ReconciliationReport>> GetReconciliation(
    [FromQuery] DateTime fromDate,
    [FromQuery] DateTime toDate)
{
// Call reconciliation service ? return report
}
```

**Endpoint 7: GET /api/rcm/summary/{claimId}**
```csharp
/// <summary>
/// Get RCM summary for a claim
/// </summary>
[HttpGet("summary/{claimId}")]
public async Task<ActionResult<RCMSummary>> GetClaimSummary(string claimId)
{
    // Get claim and response ? call processor ? return summary
}
```

---

### STEP 3: Write Tests

**Location**: `NPhies_FHIR_Integration.Tests/RCM/AppealWorkflowServiceTests.cs`

**Example Test Structure:**

```csharp
[Fact]
public async Task SubmitAppealAsync_WithValidClaim_ReturnsSuccess()
{
    // Arrange
    var claimId = "CLM-001";
    var denialReason = "Not covered";
    var appealReason = "Medical necessity documented";
    
    // Act
    var result = await _service.SubmitAppealAsync(
        claimId, denialReason, appealReason);
    
    // Assert
    Assert.NotNull(result);
Assert.True(result.IsSuccessful);
    Assert.NotEmpty(result.ConfirmationNumber);
    Assert.NotEqual(default, result.AppealDeadline);
}
```

**Tests to Write (11+):**
1. ? SubmitAppealAsync_WithValidClaim_ReturnsSuccess
2. ? SubmitAppealAsync_WithExpiredDeadline_ReturnsFail
3. ? GetAppealStatusAsync_ReturnsCorrectStatus
4. ? AddSupportingDocumentationAsync_SavesDocument
5. ? GenerateAppealLetterAsync_CreatesPDF
6. ? GetAppealDeadlineAsync_CalculatesCorrectly
7. ? GetAppealMetricsAsync_ReturnsMetrics
8. ? Multiple status values (submitted, under review, approved, denied)
9. ? Error handling (invalid IDs, null parameters)
10. ? Deadline calculations
11. ? Metrics calculations

**RCMController Tests (21+):**
- 3 tests per endpoint (success, bad request, not found)
- Total: 7 endpoints × 3 tests = 21 tests

---

## ?? SUCCESS CRITERIA

### Code Completeness
- [x] AppealWorkflowService: 6 methods, 350+ lines
- [x] RCMController: 7 endpoints, 300+ lines
- [x] All methods implement full business logic
- [x] Comprehensive error handling
- [x] Professional logging

### Testing
- [x] 11+ service tests
- [x] 21+ endpoint tests
- [x] All tests passing
- [x] Good coverage of happy path and error cases

### Quality
- [x] Build clean (0 errors, 0 warnings)
- [x] All code documented (XML comments)
- [x] Follows existing code patterns
- [x] Proper error responses
- [x] Logging on all operations

### Integration
- [x] Services properly injected in controller
- [x] Database calls work correctly
- [x] Request/response mapping correct
- [x] Swagger documentation included
- [x] Authorization checks in place

---

## ?? IMPLEMENTATION ORDER

**Day 1 of this phase (4 hours):**
1. Implement AppealWorkflowService (6 methods)
2. Run build to verify compilation
3. Create test file structure

**Day 2 of this phase (4 hours):**
1. Write 11+ tests for AppealWorkflowService
2. Get all tests passing

**Day 3 of this phase (4 hours):**
1. Create RCMController
2. Implement all 7 endpoints
3. Run build verification

**Day 4 of this phase (4 hours):**
1. Write 21+ tests for RCMController
2. Get all tests passing
3. Add Swagger documentation
4. Final build verification (0 errors)

---

## ? READY TO START?

You have everything you need:
? Clear requirements  
? Working foundation  
? Example patterns to follow  
? Test structure defined  
? Success criteria  

**Next: Start implementing AppealWorkflowService!**

Would you like me to:
1. **Start coding**: Create the implementation files now
2. **More details**: Show specific code for each method
3. **Database setup**: Create Appeal tables & migrations
4. **Something else**: Ask a question

---

**Status**: Ready to implement Days 6-7  
**Effort**: 8-16 hours of focused work  
**Target**: 700+ lines of code, 32+ tests, 78.5% compliance  
**Next Milestone**: Day 8 - DenialManagementService  

# ?? LET'S BUILD DAYS 6-7!
