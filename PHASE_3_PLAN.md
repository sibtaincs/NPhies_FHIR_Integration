# ?? PHASE 3 IMPLEMENTATION PLAN - WORKFLOWS & ORCHESTRATION

**Current Status**: Phase 2 Complete (75% Compliance) ?  
**Phase 3 Target**: 80% NPHIES Compliance  
**Timeline**: 4-5 days  
**Effort**: 50-60 hours  
**Team Size**: 2-3 developers

---

## ?? PHASE 3 OVERVIEW

### What You'll Build

Phase 3 focuses on **workflow orchestration and API endpoints** - the critical layer that manages the flow of claims and eligibility requests through the system.

### Success Criteria

- [ ] 2 workflow services created
- [ ] 7+ API endpoints implemented
- [ ] Status tracking operational
- [ ] Polling mechanism functional
- [ ] 20+ tests passing
- [ ] 80% NPHIES compliance verified
- [ ] Build compiles (0 errors)

---

## ?? PHASE 3 ARCHITECTURE

### High-Level Flow

```
API Request
    ?
Workflow Service
    ?? Validate Input
    ?? Create Message
    ?? Submit to NPHIES
    ?? Track Status
    ?? Return Response
    ?
Database
    ?
Polling Task
    ?? Check Status
    ?? Retrieve Response
    ?? Process Results
    ?? Update Database
    ?
API Response
```

### Workflow Components

#### 1. ClaimWorkflowService

**Responsibilities**:
- Manage claim submission lifecycle
- Track claim status
- Handle claim responses
- Manage errors and retries
- Update database records

**Methods to Implement**:
- `SubmitClaimAsync()` - Submit claim to NPHIES
- `TrackClaimStatusAsync()` - Get current status
- `ProcessClaimResponseAsync()` - Handle response
- `HandleClaimErrorAsync()` - Error management
- `RetryClaimAsync()` - Retry failed submissions

#### 2. EligibilityWorkflowService

**Responsibilities**:
- Manage eligibility request lifecycle
- Track eligibility responses
- Determine benefits
- Update coverage information
- Handle eligibility errors

**Methods to Implement**:
- `CheckEligibilityAsync()` - Request eligibility
- `ProcessEligibilityResponseAsync()` - Handle response
- `DetermineBenefitsAsync()` - Extract benefits
- `UpdateCoverageAsync()` - Update coverage records
- `HandleEligibilityErrorAsync()` - Error handling

### New API Endpoints (7+)

#### Claims Endpoints

```
1. POST /api/claims/submit-batch
   - Description: Submit multiple claims at once
   - Request: List of claims
   - Response: Submission status for each
   - Use Case: Batch processing

2. GET /api/claims/{id}/status
   - Description: Check claim status
   - Request: Claim ID
   - Response: Current status, updates
   - Use Case: Real-time tracking

3. POST /api/claims/{id}/validate
   - Description: Validate claim before submission
   - Request: Claim details
   - Response: Validation errors/warnings
   - Use Case: Pre-submission validation

4. POST /api/claims/{id}/resubmit
   - Description: Resubmit failed claim
 - Request: Claim ID
   - Response: New submission status
 - Use Case: Error recovery
```

#### Eligibility Endpoints

```
5. POST /api/eligibility/check-batch
   - Description: Check multiple eligibilities
   - Request: List of check requests
   - Response: Eligibility status for each
   - Use Case: Batch eligibility checks

6. GET /api/eligibility/{id}/coverage
   - Description: Get coverage details
   - Request: Eligibility ID
   - Response: Coverage information, benefits
   - Use Case: Coverage lookup

7. GET /api/eligibility/{id}/benefits
   - Description: Get determined benefits
   - Request: Eligibility ID
   - Response: Benefit summary
   - Use Case: Benefit determination
```

#### Status Tracking Endpoints

```
8. GET /api/workflow/{id}/status
   - Description: Get workflow status
   - Request: Workflow ID
   - Response: Current status, timestamp
   - Use Case: Status polling

9. GET /api/workflow/{id}/history
   - Description: Get workflow history
   - Request: Workflow ID
   - Response: Status history, events
   - Use Case: Audit trail
```

---

## ??? IMPLEMENTATION DETAILS

### 1. ClaimWorkflowService

```csharp
namespace NPhies_FHIR_Integration.Application.Services
{
public interface IClaimWorkflowService
    {
        Task<ClaimSubmissionResult> SubmitClaimAsync(
            int claimId, 
   CancellationToken cancellationToken = default);
        
        Task<ClaimStatusResult> TrackClaimStatusAsync(
            int claimId, 
            CancellationToken cancellationToken = default);
     
        Task<ProcessingResult> ProcessClaimResponseAsync(
        int claimResponseId, 
            CancellationToken cancellationToken = default);
        
    Task<RetryResult> RetryClaimAsync(
int claimId, 
    CancellationToken cancellationToken = default);
    }
    
    public class ClaimWorkflowService : IClaimWorkflowService
    {
        private readonly IClaimRepository _claimRepository;
        private readonly IClaimResponseRepository _claimResponseRepository;
        private readonly INphiesMessageService _messageService;
        private readonly ILogger<ClaimWorkflowService> _logger;
    
        // Implementation methods...
    }
}
```

**Key Methods**:

1. **SubmitClaimAsync**
   - Validate claim is ready
   - Create message envelope
   - Send to NPHIES
   - Create tracking task
   - Return submission status

2. **TrackClaimStatusAsync**
   - Query claim status
   - Check for responses
   - Update local status
   - Return current state

3. **ProcessClaimResponseAsync**
   - Parse NPHIES response
   - Extract adjudication
   - Update claim record
   - Calculate benefits
   - Send notification

4. **RetryClaimAsync**
   - Validate retry eligibility
   - Fix issues if needed
   - Resubmit to NPHIES
   - Reset tracking
   - Return new status

### 2. EligibilityWorkflowService

```csharp
namespace NPhies_FHIR_Integration.Application.Services
{
    public interface IEligibilityWorkflowService
    {
        Task<EligibilityCheckResult> CheckEligibilityAsync(
            int eligibilityRequestId, 
          CancellationToken cancellationToken = default);
        
        Task<BenefitDeterminationResult> DetermineBenefitsAsync(
      int eligibilityResponseId, 
CancellationToken cancellationToken = default);
    
Task<CoverageUpdateResult> UpdateCoverageAsync(
       int coverageId, 
            EligibilityInfo eligibilityInfo, 
       CancellationToken cancellationToken = default);
    }
    
    public class EligibilityWorkflowService : IEligibilityWorkflowService
    {
        private readonly ICoverageEligibilityRequestRepository _requestRepository;
        private readonly ICoverageEligibilityResponseRepository _responseRepository;
        private readonly ICoverageRepository _coverageRepository;
        private readonly INphiesMessageService _messageService;
        private readonly ILogger<EligibilityWorkflowService> _logger;
        
 // Implementation methods...
    }
}
```

**Key Methods**:

1. **CheckEligibilityAsync**
   - Validate request data
   - Create message
   - Send to NPHIES
   - Track request
   - Return check status

2. **DetermineBenefitsAsync**
   - Parse eligibility response
   - Extract benefits
   - Calculate summaries
   - Determine coverage gaps
   - Return benefit summary

3. **UpdateCoverageAsync**
   - Get eligibility info
   - Update coverage record
   - Update deductible tracking
   - Update benefits
   - Return update status

### 3. Status Tracking Service

```csharp
namespace NPhies_FHIR_Integration.Application.Services
{
    public interface IStatusTrackingService
    {
        Task<WorkflowStatus> GetStatusAsync(string workflowId);
Task<List<StatusHistory>> GetHistoryAsync(string workflowId);
        Task UpdateStatusAsync(string workflowId, string newStatus);
    Task<bool> IsCompleteAsync(string workflowId);
    }
}
```

---

## ?? API Controllers

### ClaimsController Enhancements

```csharp
[ApiController]
[Route("api/[controller]")]
public class ClaimsController : BaseController
{
    private readonly IClaimWorkflowService _workflowService;
    
    [HttpPost("submit-batch")]
    public async Task<IActionResult> SubmitBatch(
        [FromBody] List<int> claimIds)
    {
        // Implementation
    }
    
    [HttpGet("{id}/status")]
    public async Task<IActionResult> GetStatus(int id)
    {
        // Implementation
    }
    
    [HttpPost("{id}/validate")]
    public async Task<IActionResult> ValidateClaim(int id)
    {
     // Implementation
    }
    
    [HttpPost("{id}/resubmit")]
    public async Task<IActionResult> ResubmitClaim(int id)
    {
        // Implementation
    }
}
```

### EligibilityController Enhancements

```csharp
[ApiController]
[Route("api/[controller]")]
public class EligibilityController : BaseController
{
    private readonly IEligibilityWorkflowService _workflowService;
    
    [HttpPost("check-batch")]
    public async Task<IActionResult> CheckBatch(
    [FromBody] List<int> requestIds)
    {
    // Implementation
    }
    
    [HttpGet("{id}/coverage")]
    public async Task<IActionResult> GetCoverage(int id)
    {
   // Implementation
    }
    
 [HttpGet("{id}/benefits")]
    public async Task<IActionResult> GetBenefits(int id)
    {
        // Implementation
    }
}
```

### WorkflowController (New)

```csharp
[ApiController]
[Route("api/[controller]")]
public class WorkflowController : BaseController
{
    private readonly IStatusTrackingService _statusService;
    
    [HttpGet("{id}/status")]
    public async Task<IActionResult> GetStatus(string id)
    {
        // Implementation
    }
    
    [HttpGet("{id}/history")]
    public async Task<IActionResult> GetHistory(string id)
    {
        // Implementation
    }
    
 [HttpPost("{id}/poll")]
    public async Task<IActionResult> PollStatus(string id)
    {
        // Implementation
    }
}
```

---

## ?? Result Classes

### Submission Results

```csharp
public class ClaimSubmissionResult
{
    public int ClaimId { get; set; }
    public string SubmissionId { get; set; }
    public string Status { get; set; } // submitted, queued, processing
    public DateTime SubmittedAt { get; set; }
    public string MessageId { get; set; }
    public List<string> ValidationWarnings { get; set; }
 public bool IsSuccessful { get; set; }
}

public class EligibilityCheckResult
{
    public int RequestId { get; set; }
    public string Status { get; set; }
    public DateTime CheckedAt { get; set; }
    public string MessageId { get; set; }
    public bool IsQueued { get; set; }
}
```

### Status Results

```csharp
public class ClaimStatusResult
{
    public int ClaimId { get; set; }
    public string CurrentStatus { get; set; } // submitted, processing, approved, denied
    public DateTime LastUpdate { get; set; }
    public int? ClaimResponseId { get; set; }
    public List<StatusEvent> Events { get; set; }
    public SubmissionMetrics Metrics { get; set; }
}

public class StatusEvent
{
    public string Status { get; set; }
 public DateTime Timestamp { get; set; }
    public string Details { get; set; }
}
```

---

## ?? Test Strategy

### Unit Tests (20+)

#### ClaimWorkflowService Tests (8)
- [ ] Submit valid claim
- [ ] Validate claim before submission
- [ ] Handle invalid claim
- [ ] Track claim status
- [ ] Process claim response
- [ ] Handle claim error
- [ ] Retry failed claim
- [ ] Batch submission

#### EligibilityWorkflowService Tests (7)
- [ ] Check eligibility request
- [ ] Process eligibility response
- [ ] Determine benefits
- [ ] Update coverage
- [ ] Handle eligibility error
- [ ] Batch eligibility check
- [ ] Invalid eligibility data

#### Status Tracking Tests (5)
- [ ] Get workflow status
- [ ] Get workflow history
- [ ] Update status
- [ ] Status transitions
- [ ] Complete detection

### Integration Tests (5+)
- [ ] End-to-end claim submission
- [ ] End-to-end eligibility check
- [ ] Status polling workflow
- [ ] Error recovery workflow
- [ ] Batch operations

---

## ?? Database Changes

### New Entities Needed

1. **WorkflowStatus** (Tracking table)
   - WorkflowId
 - WorkflowType (Claim, Eligibility)
   - CurrentStatus
   - LastUpdated
   - RetryCount
   - ErrorMessage

2. **StatusHistory** (Audit table)
   - WorkflowId
   - OldStatus
   - NewStatus
   - Timestamp
   - ChangedBy
   - Details

### Modified Entities

1. **Claim**
   - Add: SubmissionCount
   - Add: LastSubmittedAt
   - Add: WorkflowStatusId

2. **CoverageEligibilityRequest**
   - Add: CheckCount
   - Add: LastCheckedAt
   - Add: WorkflowStatusId

---

## ?? Implementation Timeline

### Day 1-2: Core Services
- [ ] Create ClaimWorkflowService
- [ ] Create EligibilityWorkflowService
- [ ] Create StatusTrackingService
- [ ] Create result classes
- [ ] Unit tests for services

### Day 2-3: API Controllers
- [ ] Implement ClaimsController endpoints
- [ ] Implement EligibilityController endpoints
- [ ] Create WorkflowController
- [ ] Input validation
- [ ] Error handling

### Day 3-4: Integration & Testing
- [ ] Integration tests
- [ ] End-to-end testing
- [ ] Polling mechanism
- [ ] Error scenarios
- [ ] Performance optimization

### Day 4-5: Polish & Documentation
- [ ] Final testing
- [ ] Bug fixes
- [ ] Documentation
- [ ] Code review
- [ ] Compliance verification (80%)

---

## ? Checklist

### Core Implementation
- [ ] ClaimWorkflowService created
- [ ] EligibilityWorkflowService created
- [ ] StatusTrackingService created
- [ ] Result classes defined
- [ ] WorkflowStatus entity
- [ ] StatusHistory entity

### API Endpoints
- [ ] POST /claims/submit-batch
- [ ] GET /claims/{id}/status
- [ ] POST /claims/{id}/validate
- [ ] POST /claims/{id}/resubmit
- [ ] POST /eligibility/check-batch
- [ ] GET /eligibility/{id}/coverage
- [ ] GET /eligibility/{id}/benefits
- [ ] GET /workflow/{id}/status
- [ ] GET /workflow/{id}/history

### Testing
- [ ] Unit tests (20+)
- [ ] Integration tests (5+)
- [ ] All tests passing
- [ ] Error scenarios covered
- [ ] Performance acceptable

### Quality
- [ ] Build successful (0 errors)
- [ ] Build warnings (0)
- [ ] Code reviewed
- [ ] Documentation complete
- [ ] 80% compliance verified

---

## ?? SUCCESS CRITERIA

### Must Have
? 2 workflow services operational  
? 7+ API endpoints working  
? Status tracking functional  
? 20+ tests passing  
? 80% compliance achieved  

### Should Have
? Polling mechanism
? Batch operations
? Error recovery
? Comprehensive logging
? Performance optimized

### Nice to Have
? Webhook callbacks
? Real-time updates
? Advanced filtering
? Analytics integration

---

## ?? References

- Previous Phase 2 entities: AdjudicationDetailEntity, RejectionReasonEntity
- Payment Calculation Engine: For benefit determination
- Existing Repositories: For data access patterns
- NPHIES Documentation: For workflow requirements
- FHIR Specification: For resource handling

---

## ?? PHASE 3 READY?

**Status**: Ready to start ?
**Prerequisites met**: Phase 2 complete ?
**Team prepared**: Yes ?
**Timeline realistic**: Yes (4-5 days) ?
**Compliance target achievable**: 80% ?

---

**Next Step**: Start Phase 3 implementation immediately!

**Let's achieve 80% compliance! ??**
