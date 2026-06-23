# ?? **MASTER CODE IMPLEMENTATION GUIDE**

## **STEP 1: UPDATE AutoMapper Profiles**

### **What Needs to Be Added to ApplicationMappingProfile.cs:**

The existing mapping profile is missing mappings for:
- [ ] Claims DTOs
- [ ] ClaimResponse DTOs  
- [ ] Eligibility DTOs
- [ ] Payment DTOs

---

## **CODE TO ADD: Eligibility Mappings**

```csharp
// Add this to ApplicationMappingProfile.cs

private void ApplyEligibilityMappings()
{
    // CoverageEligibilityRequest Mappings
    CreateMap<CoverageEligibilityRequest, CoverageEligibilityRequestDto>().ReverseMap();
    CreateMap<CreateCoverageEligibilityRequestDto, CoverageEligibilityRequest>()
  .ForMember(dest => dest.Id, opt => opt.MapFrom(src => Guid.NewGuid().ToString()))
        .ForMember(dest => dest.Status, opt => opt.MapFrom(src => "active"))
        .ForMember(dest => dest.MessageStatus, opt => opt.MapFrom(src => "sent"))
        .ForMember(dest => dest.CreatedAt, opt => opt.MapFrom(src => DateTime.UtcNow));

    // CoverageEligibilityResponse Mappings  
    CreateMap<CoverageEligibilityResponse, CoverageEligibilityResponseDto>().ReverseMap();
    
  // EligibilityItem Mappings
    CreateMap<EligibilityItem, EligibilityItemDto>().ReverseMap();
    
    // Benefit Mappings
    CreateMap<BenefitBalance, BenefitBalanceDto>().ReverseMap();
    CreateMap<Benefit, BenefitDto>().ReverseMap();
    
    // Error Mappings
    CreateMap<EligibilityError, EligibilityErrorDto>().ReverseMap();
}

// Add this call to the ApplicationMappingProfile constructor:
ApplyEligibilityMappings();
```

---

## **CODE TO ADD: Claims Mappings**

```csharp
// Add this to ApplicationMappingProfile.cs

private void ApplyClaimsMappings()
{
    // Claim Mappings
    CreateMap<Claim, ClaimDto>().ReverseMap();
    CreateMap<CreateClaimDto, Claim>()
        .ForMember(dest => dest.Id, opt => opt.MapFrom(src => Guid.NewGuid().ToString()))
        .ForMember(dest => dest.Status, opt => opt.MapFrom(src => "draft"))
    .ForMember(dest => dest.CreatedAt, opt => opt.MapFrom(src => DateTime.UtcNow));
    CreateMap<UpdateClaimDto, Claim>()
  .ForMember(dest => dest.UpdatedAt, opt => opt.MapFrom(src => DateTime.UtcNow))
        .ForAllMembers(opts => opts.Condition((src, dest, srcMember) => srcMember != null));
    
    // ClaimItem Mappings
    CreateMap<ClaimItem, ClaimItemDto>().ReverseMap();
 
    // ClaimDiagnosis Mappings
    CreateMap<ClaimDiagnosis, ClaimDiagnosisDto>().ReverseMap();
    
    // ClaimResponse Mappings
    CreateMap<ClaimResponse, ClaimResponseDto>().ReverseMap();
    CreateMap<ClaimResponseInsurance, ClaimResponseInsuranceDto>().ReverseMap();
}

// Add this call to the ApplicationMappingProfile constructor:
ApplyClaimsMappings();
```

---

## **CODE TO ADD: Payment Mappings**

```csharp
// Add this to ApplicationMappingProfile.cs

private void ApplyPaymentMappings()
{
    // PaymentNotice Mappings
    CreateMap<PaymentNotice, PaymentNoticeDto>().ReverseMap();
    CreateMap<CreatePaymentNoticeDto, PaymentNotice>()
        .ForMember(dest => dest.Id, opt => opt.MapFrom(src => Guid.NewGuid().ToString()))
        .ForMember(dest => dest.Status, opt => opt.MapFrom(src => "pending"))
        .ForMember(dest => dest.CreatedAt, opt => opt.MapFrom(src => DateTime.UtcNow));
    
    // PaymentReconciliation Mappings
    CreateMap<PaymentReconciliation, PaymentReconciliationDto>().ReverseMap();
    CreateMap<PaymentReconciliationDetail, PaymentReconciliationDetailDto>().ReverseMap();
}

// Add this call to the ApplicationMappingProfile constructor:
ApplyPaymentMappings();
```

---

## **STEP 2: Verify Existing Controllers**

All controllers appear to exist. Let's verify they have the necessary endpoints:

### **CheckList for Each Controller:**

- [ ] **PatientsController** - ? COMPLETE (6 endpoints)
- [ ] **OrganizationsController** - ? COMPLETE (8 endpoints)
- [ ] **CoverageController** - ? COMPLETE (7 endpoints)
- [ ] **EligibilityController** - Verify complete
- [ ] **ClaimsController** - Verify complete
- [ ] **ClaimResponsesController** - Verify complete  
- [ ] **PaymentsController** - ? COMPLETE (3 endpoints)
- [ ] **RCMController** - Verify complete
- [ ] **DiagnosesController** - Verify complete
- [ ] **ItemsController** - Verify complete

---

## **STEP 3: Missing DTOs to Create (if any)**

### **Check if these DTOs exist:**

- [ ] CreateCoverageEligibilityRequestDto
- [ ] UpdateCoverageEligibilityRequestDto
- [ ] CreateClaimDto
- [ ] UpdateClaimDto
- [ ] CreatePaymentNoticeDto
- [ ] PaymentReconciliationDetailDto
- [ ] CreateClaimResponseDto

If missing, we'll create them.

---

## **STEP 4: Implementation Order**

1. ? **Fix ApplicationMappingProfile** - Add all missing mappings
2. ? **Create Missing DTOs** - If any are missing  
3. ? **Verify All Controllers** - Ensure all endpoints present
4. ? **Add Advanced Endpoints** - Search, filter, export
5. ? **Build & Test** - Ensure no compilation errors

---

## **Summary of Changes Needed**

### **File: ApplicationMappingProfile.cs**

```csharp
// Update the ApplicationMappingProfile constructor to:
public ApplicationMappingProfile()
{
    ApplyPatientMappings();
    ApplyCoverageMappings();
    ApplyOrganizationMappings();
    ApplyEligibilityMappings();      // ADD THIS
    ApplyClaimsMappings();         // ADD THIS
    ApplyPaymentMappings();   // ADD THIS
}

// Add all three new mapping methods above
```

---

## **Build Status After Changes**

Once all mappings are added:
- [ ] Build should succeed (0 errors)
- [ ] All DTOs should be mapped
- [ ] All entities should be mappable
- [ ] No compilation warnings about unmapped members

---

**Ready to implement?** Let's start with the ApplicationMappingProfile updates! ??
