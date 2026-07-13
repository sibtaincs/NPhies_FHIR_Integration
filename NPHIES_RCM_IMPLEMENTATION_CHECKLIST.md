# ?? NPHIES RCM Implementation Checklist & Action Plan

**Last Updated:** July 5, 2024  
**Overall Completion:** 71%  
**Next Phase Target:** 85%

---

## ?? IMMEDIATE ACTIONS (Next 2 Weeks)

### **Week 1: Critical Error Code Implementation**

#### **Task 1.1: Create ErrorCodeMaster Table**
- [ ] Add ErrorCodeMaster entity to Domain
- [ ] Add ErrorCodeMaster DbSet to ApplicationDbContext
- [ ] Create migration for ErrorCodeMaster table
- [ ] Add indexes on ErrorCode, ErrorCategory
- **Owner:** Backend Team
- **Effort:** 2-3 hours
- **Files to Create/Modify:**
  - `Domain/Entities/Masters/ErrorCodeMaster.cs`
  - `Infrastructure/Data/ApplicationDbContext.cs`

#### **Task 1.2: Bulk Import 1,682 NPHIES Error Codes**
- [ ] Download NPHIES error code appendix (XML/JSON)
- [ ] Create bulk import script
- [ ] Parse and validate error codes
- [ ] Load into database
- [ ] Verify import (should have 1,682 records)
- **Owner:** Data Team
- **Effort:** 4-6 hours
- **Files to Create:**
  - `Infrastructure/Seeding/ErrorCodeMasterSeeder.cs`
  - SQL bulk insert script

#### **Task 1.3: Create Error Code Service**
- [ ] Create IErrorCodeService interface
- [ ] Implement ErrorCodeService
- [ ] Add GetErrorCodeAsync(code)
- [ ] Add GetErrorCodesByCategory(category)
- [ ] Add SearchErrorCodes(searchTerm)
- **Owner:** Backend Team
- **Effort:** 2-3 hours
- **Files to Create:**
  - `Application/Services/Masters/IErrorCodeService.cs`
  - `Application/Services/Masters/ErrorCodeService.cs`

#### **Task 1.4: Integrate Error Codes into Adjudication Processing**
- [ ] Modify ClaimResponseProcessingService to use ErrorCodeService
- [ ] Update denial reason extraction to map to ErrorCodeMaster
- [ ] Add error code validation in IdentifyDeniedItemsAsync
- [ ] Test with sample NPHIES error codes
- **Owner:** Backend Team
- **Effort:** 3-4 hours
- **Files to Modify:**
  - `Application/Services/RCM/ClaimResponseProcessingService.cs`

---

### **Week 2: Core Adjudication Rules**

#### **Task 2.1: Extend Validation Rule Engine**
- [ ] Create AdjudicationRule entity/interface
- [ ] Add priority sequencing for rule execution
- [ ] Implement rule context evaluation
- [ ] Add rule result tracking
- **Owner:** Backend Team
- **Effort:** 4-5 hours
- **Files to Modify:**
  - `Application/Services/RCM/NphiesValidationRuleEngine.cs`
  - `Domain/Entities/ValidationRule.cs`

#### **Task 2.2: Implement Top 50 Adjudication Rules**
Critical rules to implement:
- [ ] Copay rule: "$X per visit/claim"
- [ ] Deductible rule: "Apply annual deductible first"
- [ ] Coinsurance rule: "Apply X% after deductible"
- [ ] Network status rule: "In-network: X%, Out-of-network: Y%"
- [ ] Service limit rule: "Maximum Z visits per year"
- [ ] Benefit exclusion rule: "Service not covered"
- [ ] Prior auth rule: "Prior auth required"
- [ ] Waiting period rule: "Waiting period applies"
- (40 more rules based on your business)

- **Owner:** Business Analyst + Backend Team
- **Effort:** 10-12 hours
- **Files to Create:**
  - `Application/Services/RCM/AdjudicationRules/` (multiple files)

#### **Task 2.3: Create Rule Execution Engine**
- [ ] Implement rule evaluation in sequence
- [ ] Handle rule dependencies
- [ ] Track rule application results
- [ ] Generate rule audit trail
- **Owner:** Backend Team
- **Effort:** 3-4 hours
- **Files to Modify:**
  - `Application/Services/RCM/ClaimResponseProcessingService.cs`

#### **Task 2.4: Test Against NPHIES Scenarios**
- [ ] Get NPHIES test claim scenarios
- [ ] Create test cases for each rule
- [ ] Validate rule outputs match expected results
- [ ] Fix any discrepancies
- **Owner:** QA Team
- **Effort:** 5-6 hours

---

## ?? PHASE 2 ACTIONS (Weeks 3-4)

### **Task 3: Complete Appeal Workflow**

#### **3.1 Create Appeal Entities**
- [ ] AppealRequest entity
- [ ] AppealResponse entity
- [ ] AppealTracking entity
- [ ] AppealDeadline entity
- **Files:**
  - `Domain/Entities/Appeal*.cs` (4 files)

#### **3.2 Implement Appeal Service**
```
Required Methods:
- CreateAppealAsync(denialDetail, coverage)
- GetAppealDeadlineAsync(denialType)
- SubmitAppealAsync(appealRequest)
- TrackAppealStatusAsync(appealId)
- GenerateAppealLetterAsync(appealRequest)
- CheckAppealEligibilityAsync(denialDetail)
```

#### **3.3 Appeal Deadline Rules**
- [ ] Standard appeal: 60 days from denial
- [ ] Expedited appeal: 30 days (if requested)
- [ ] Second level: 60 days after first appeal decision
- [ ] External review: Based on plan rules
- **Files:**
  - `Application/Services/RCM/AppealDeadlineCalculator.cs`

---

### **Task 4: Payment Reconciliation**

#### **4.1 Implement Payment Matching**
- [ ] Match payment notice to claims
- [ ] Track partial payments
- [ ] Identify overpayments/underpayments
- [ ] Create reconciliation report
- **Files:**
  - `Application/Services/RCM/PaymentReconciliationService.cs`

#### **4.2 Payment Dispute Handling**
- [ ] Create DisputeRequest entity
- [ ] Implement dispute creation logic
- [ ] Track dispute resolution
- **Files:**
  - `Domain/Entities/PaymentDispute*.cs`

---

### **Task 5: Authorization Workflows**

#### **5.1 Complete Auth Service**
```
Required Methods:
- CheckAuthRequirementAsync(serviceType, diagnosis)
- IsAuthorizationRequiredAsync(claim)
- ValidateAuthorizationAsync(claimItem, auth)
- CheckAuthExpirationAsync(authorizationId)
- GetAuthStatusAsync(authorizationId)
```

---

## ?? Detailed Feature Implementation

### **Feature 1: Error Code System**

**Current State:** None  
**Target State:** Full 1,682 codes integrated  
**Effort:** 16 hours

```csharp
// Example: ErrorCodeMaster entity structure
public class ErrorCodeMaster : BaseEntity
{
 public string ErrorCode { get; set; } // e.g., "AD-1-1"
    public string ErrorDescription { get; set; }
    public string ErrorCategory { get; set; } // Adjudication, Coverage, etc.
    public string Severity { get; set; } // Error, Warning, Info
 public bool IsRecoverable { get; set; }
    public bool AllowsAppeal { get; set; }
    public int StandardAppealDays { get; set; }
    public string RecommendedAction { get; set; }
}
```

### **Feature 2: Adjudication Rules**

**Current State:** 25 basic rules  
**Target State:** 200+ business rules  
**Effort:** 40+ hours

```csharp
// Example: Rule structure
public interface IAdjudicationRule
{
    string RuleId { get; }
    int Priority { get; } // Lower number = higher priority
    Task<RuleResult> EvaluateAsync(AdjudicationContext context);
}

public class CopayRule : IAdjudicationRule
{
    public string RuleId => "COPAY-CALC";
    public int Priority => 10;
    
    public async Task<RuleResult> EvaluateAsync(AdjudicationContext context)
    {
    // Copay = min(actualCost, coverageAmount)
        // Apply copay first, then deductible, then coinsurance
    }
}
```

### **Feature 3: Appeal Management**

**Current State:** Scaffolding only  
**Target State:** Complete workflow  
**Effort:** 30+ hours

```csharp
public class AppealRequest : BaseEntity
{
    public string DenialId { get; set; }
    public string PatientId { get; set; }
    public string ClaimId { get; set; }
    public string AppealLevel { get; set; } // First, Second, External
    public DateTime DeadlineDate { get; set; }
    public string AppealReason { get; set; }
    public string SupportingDocuments { get; set; }
    public DateTime SubmittedDate { get; set; }
    public string Status { get; set; } // Pending, Approved, Denied
    public string AppealDecision { get; set; }
    public DateTime DecisionDate { get; set; }
}
```

---

## ?? Development Process

### **For Each Task:**

1. **Planning**
   - [ ] Define requirements
   - [ ] Identify entities/DTOs
   - [ ] Design database schema
   - Effort: 0.5-1 hour

2. **Development**
   - [ ] Create entities
   - [ ] Create services/interfaces
   - [ ] Create repository/queries
   - [ ] Add API endpoints
   - Effort: 4-8 hours

3. **Testing**
   - [ ] Unit tests
   - [ ] Integration tests
   - [ ] End-to-end tests
   - Effort: 3-5 hours

4. **Documentation**
   - [ ] Code comments
   - [ ] API documentation
   - [ ] User guide
   - Effort: 1-2 hours

5. **Code Review**
   - [ ] Peer review
   - [ ] Architecture review
   - [ ] Security review
   - Effort: 1-2 hours

---

## ?? Files to Create

### **Domain Layer** (7 files)
```
Domain/Entities/Masters/
??? ErrorCodeMaster.cs
??? AppealRequest.cs
??? AppealResponse.cs
??? AppealTracking.cs
??? PaymentDispute.cs
??? AuthorizationRequest.cs
??? AuthorizationResponse.cs
```

### **Application Layer** (8 files)
```
Application/Services/RCM/
??? IErrorCodeService.cs
??? ErrorCodeService.cs
??? IAdjudicationRuleService.cs
??? AdjudicationRuleService.cs
??? IAppealWorkflowService.cs (update)
??? AppealWorkflowService.cs (update)
??? PaymentReconciliationService.cs (update)
??? AdjudicationRules/
    ??? CopayRule.cs
    ??? DeductibleRule.cs
    ??? CoinsuranceRule.cs
  ??? ... (50+ rule files)
```

### **Infrastructure Layer** (3 files)
```
Infrastructure/
??? Seeding/ErrorCodeMasterSeeder.cs
??? Repositories/IErrorCodeRepository.cs
??? Repositories/ErrorCodeRepository.cs
```

---

## ?? Testing Strategy

### **Unit Tests**
```bash
# For each new service/rule
dotnet test --filter "ErrorCodeService"
dotnet test --filter "AdjudicationRules"
```

### **Integration Tests**
```bash
# Test full workflows
dotnet test --filter "ClaimProcessing"
dotnet test --filter "AppealWorkflow"
```

### **NPHIES Compliance Tests**
```bash
# Test against NPHIES scenarios
dotnet test --filter "NphiesScenarios"
```

---

## ?? Success Metrics

### **Week 1 Targets**
- [ ] 1,682 error codes in database
- [ ] ErrorCodeService fully functional
- [ ] Integration into adjudication processing
- **Success Criteria:** All tests passing, error codes accessible via API

### **Week 2 Targets**
- [ ] 50 adjudication rules implemented
- [ ] Rule execution engine working
- [ ] NPHIES test scenarios passing
- **Success Criteria:** >95% test pass rate, accurate adjudication results

### **Week 3-4 Targets**
- [ ] Appeal workflow complete
- [ ] Payment reconciliation working
- [ ] Authorization service complete
- **Success Criteria:** All features tested and documented

---

## ?? Risk Mitigation

### **Risk 1: NPHIES Error Code Accuracy**
- **Impact:** HIGH
- **Mitigation:** 
  - [ ] Validate error codes against official NPHIES documentation
  - [ ] Create validation suite to check all 1,682 codes
  - [ ] Get sign-off from business team

### **Risk 2: Rule Complexity**
- **Impact:** HIGH
- **Mitigation:**
- [ ] Start with top 20 rules
  - [ ] Add incrementally
  - [ ] Get business owner sign-off for each rule

### **Risk 3: Performance Impact**
- **Impact:** MEDIUM
- **Mitigation:**
  - [ ] Add caching for error codes
  - [ ] Optimize rule execution order (priority)
  - [ ] Performance test with 1000+ claims

### **Risk 4: Data Integrity**
- **Impact:** MEDIUM
- **Mitigation:**
  - [ ] Add audit trails for all adjudication decisions
  - [ ] Implement validation at each step
  - [ ] Create reconciliation reports

---

## ?? Resource Requirements

### **Team Composition**
- Backend Developer(s): 2-3 FTE
- Database Developer: 1 FTE
- QA Engineer: 1 FTE
- Business Analyst: 0.5 FTE (for rule definition)
- Technical Lead: 0.5 FTE (architecture review)

### **Tools Required**
- Visual Studio 2022 / VS Code
- SQL Server 2019+
- GitHub (already configured)
- JIRA/Azure DevOps for tracking
- Postman for API testing

### **Knowledge Required**
- NPHIES business rules
- Healthcare claims processing
- FHIR standards
- .NET/C# development
- SQL database design

---

## ?? Timeline Summary

| Phase | Duration | Effort | Status |
|-------|----------|--------|--------|
| Error Code System | 2 days | 16 hours | ?? NOT STARTED |
| Adjudication Rules (50) | 3 days | 40 hours | ?? NOT STARTED |
| Appeal Workflow | 2 days | 30 hours | ?? IN PROGRESS |
| Payment Reconciliation | 2 days | 25 hours | ?? IN PROGRESS |
| Authorization | 1.5 days | 20 hours | ?? NOT STARTED |
| **TOTAL** | **~2 weeks** | **~131 hours** | **~14 FTE days** |

---

## ? Sign-Off Checklist

- [ ] Requirements reviewed and approved
- [ ] Resource allocation confirmed
- [ ] Timeline agreed upon
- [ ] Success metrics defined
- [ ] Risk mitigation plan accepted
- [ ] Technical approach approved

**Approvals:**
- Architect: _________________
- Product Owner: _________________
- Tech Lead: _________________
- Date: _________________

---

**Document Version:** 1.0  
**Status:** Ready for Implementation  
**Next Review:** After Week 1 completion

