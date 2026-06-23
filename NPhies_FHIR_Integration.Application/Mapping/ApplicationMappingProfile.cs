using AutoMapper;
using NPhies_FHIR_Integration.Application.DTOs;
using NPhies_FHIR_Integration.Domain.Entities;
using DomainDTOs = NPhies_FHIR_Integration.Domain.DTOs;

namespace NPhies_FHIR_Integration.Application.Mapping;

/// <summary>
/// Combined mapping profile for all DTOs
/// </summary>
public class ApplicationMappingProfile : Profile
{
  public ApplicationMappingProfile()
    {
  ApplyPatientMappings();
   ApplyCoverageMappings();
        ApplyOrganizationMappings();
     ApplyEligibilityMappings();
    ApplyClaimsMappings();
  ApplyPaymentMappings();
    ApplyCommunicationRequestMappings();
        ApplyCommunicationMappings();
    }

    private void ApplyPatientMappings()
    {
  CreateMap<Patient, PatientDto>().ReverseMap();
        CreateMap<CreatePatientDto, Patient>()
     .ForMember(dest => dest.Id, opt => opt.MapFrom(src => Guid.NewGuid().ToString()))
 .ForMember(dest => dest.CreatedAt, opt => opt.MapFrom(src => DateTime.UtcNow))
   .ForMember(dest => dest.IsActive, opt => opt.MapFrom(src => true));
        CreateMap<UpdatePatientDto, Patient>()
    .ForMember(dest => dest.UpdatedAt, opt => opt.MapFrom(src => DateTime.UtcNow))
     .ForAllMembers(opts => opts.Condition((src, dest, srcMember) => srcMember != null));
  }

    private void ApplyCoverageMappings()
    {
    CreateMap<Coverage, CoverageDto>().ReverseMap();
  CreateMap<CreateCoverageDto, Coverage>()
     .ForMember(dest => dest.Id, opt => opt.MapFrom(src => Guid.NewGuid().ToString()))
 .ForMember(dest => dest.Status, opt => opt.MapFrom(src => "active"))
.ForMember(dest => dest.CreatedAt, opt => opt.MapFrom(src => DateTime.UtcNow))
  .ForMember(dest => dest.DeductibleMet, opt => opt.MapFrom(src => 0m));
CreateMap<UpdateCoverageDto, Coverage>()
 .ForMember(dest => dest.UpdatedAt, opt => opt.MapFrom(src => DateTime.UtcNow))
        .ForAllMembers(opts => opts.Condition((src, dest, srcMember) => srcMember != null));
    }

private void ApplyOrganizationMappings()
    {
        CreateMap<Organization, OrganizationDto>().ReverseMap();
CreateMap<Organization, ProviderDto>();
        CreateMap<Organization, InsurerDto>();
  CreateMap<CreateOrganizationDto, Organization>()
      .ForMember(dest => dest.Id, opt => opt.MapFrom(src => Guid.NewGuid().ToString()))
 .ForMember(dest => dest.Status, opt => opt.MapFrom(src => "active"))
.ForMember(dest => dest.CreatedAt, opt => opt.MapFrom(src => DateTime.UtcNow));
  CreateMap<UpdateOrganizationDto, Organization>()
      .ForMember(dest => dest.UpdatedAt, opt => opt.MapFrom(src => DateTime.UtcNow))
    .ForAllMembers(opts => opts.Condition((src, dest, srcMember) => srcMember != null));
    }

    private void ApplyEligibilityMappings()
  {
 // CoverageEligibilityRequest Mappings
     CreateMap<CoverageEligibilityRequest, DomainDTOs.CoverageEligibilityRequestDto>().ReverseMap();

    // CoverageEligibilityResponse Mappings  
      CreateMap<CoverageEligibilityResponse, DomainDTOs.CoverageEligibilityResponseDto>().ReverseMap();
   
// EligibilityItem Mappings
 CreateMap<EligibilityItem, DomainDTOs.EligibilityItemDto>().ReverseMap();
  
   // Benefit Mappings
        CreateMap<BenefitBalance, DomainDTOs.BenefitBalanceDto>().ReverseMap();
      CreateMap<Benefit, DomainDTOs.BenefitDto>().ReverseMap();
  
  // Error Mappings
 CreateMap<EligibilityError, DomainDTOs.EligibilityErrorDto>().ReverseMap();
    }

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

  private void ApplyPaymentMappings()
 {
        // PaymentNotice Mappings
   CreateMap<PaymentNotice, PaymentNoticeDto>().ReverseMap();
 
   // PaymentReconciliation Mappings
 CreateMap<PaymentReconciliation, PaymentReconciliationDto>().ReverseMap();
      CreateMap<PaymentReconciliationDetail, PaymentReconciliationDetailDto>().ReverseMap();
    }

  private void ApplyCommunicationRequestMappings()
   {
        // CommunicationRequest Mappings
 CreateMap<CommunicationRequest, CommunicationRequestDto>().ReverseMap();
  CreateMap<CreateCommunicationRequestDto, CommunicationRequest>()
            .ForMember(dest => dest.Id, opt => opt.MapFrom(src => Guid.NewGuid().ToString()))
   .ForMember(dest => dest.Status, opt => opt.MapFrom(src => src.Status ?? "active"))
            .ForMember(dest => dest.CreatedAt, opt => opt.MapFrom(src => DateTime.UtcNow));
        CreateMap<UpdateCommunicationRequestDto, CommunicationRequest>()
        .ForMember(dest => dest.UpdatedAt, opt => opt.MapFrom(src => DateTime.UtcNow))
      .ForAllMembers(opts => opts.Condition((src, dest, srcMember) => srcMember != null));
    }

    private void ApplyCommunicationMappings()
 {
        // Communication Mappings
        CreateMap<Communication, CommunicationDto>()
        .ForMember(dest => dest.PayloadAttachmentSizeKB, opt => opt.MapFrom(src => 
                src.PayloadAttachmentData != null ? Math.Round((decimal)src.PayloadAttachmentData.Length / 1024, 2) : 0m))
      .ForMember(dest => dest.HasAttachment, opt => opt.MapFrom(src => src.PayloadAttachmentData != null && src.PayloadAttachmentData.Length > 0));
        
  CreateMap<CreateCommunicationDto, Communication>()
            .ForMember(dest => dest.Id, opt => opt.MapFrom(src => Guid.NewGuid().ToString()))
    .ForMember(dest => dest.Status, opt => opt.MapFrom(src => src.Status ?? "completed"))
 .ForMember(dest => dest.ProcessingStatus, opt => opt.MapFrom(src => "received"))
            .ForMember(dest => dest.PayloadAttachmentData, opt => opt.MapFrom(src => 
                !string.IsNullOrEmpty(src.PayloadAttachmentDataBase64) ? Convert.FromBase64String(src.PayloadAttachmentDataBase64) : null))
            .ForMember(dest => dest.CreatedAt, opt => opt.MapFrom(src => DateTime.UtcNow));
  
        CreateMap<UpdateCommunicationDto, Communication>()
       .ForMember(dest => dest.UpdatedAt, opt => opt.MapFrom(src => DateTime.UtcNow))
            .ForAllMembers(opts => opts.Condition((src, dest, srcMember) => srcMember != null));
    }
}

/// <summary>
/// AutoMapper profile for Patient entity mappings
/// </summary>
public class PatientMappingProfile : Profile
{
 public PatientMappingProfile()
    {
   // Patient -> PatientDto
CreateMap<Patient, PatientDto>()
     .ForMember(dest => dest.Gender, opt => opt.MapFrom(src => src.Gender))
   .ReverseMap();

      // CreatePatientDto -> Patient
    CreateMap<CreatePatientDto, Patient>()
 .ForMember(dest => dest.Id, opt => opt.MapFrom(src => Guid.NewGuid().ToString()))
  .ForMember(dest => dest.CreatedAt, opt => opt.MapFrom(src => DateTime.UtcNow))
      .ForMember(dest => dest.IsActive, opt => opt.MapFrom(src => true));

      // UpdatePatientDto -> Patient
    CreateMap<UpdatePatientDto, Patient>()
   .ForMember(dest => dest.UpdatedAt, opt => opt.MapFrom(src => DateTime.UtcNow))
       .ForAllMembers(opts => opts.Condition((src, dest, srcMember) => srcMember != null));
  }
}

/// <summary>
/// AutoMapper profile for Coverage entity mappings
/// </summary>
public class CoverageMappingProfile : Profile
{
    public CoverageMappingProfile()
 {
        // Coverage -> CoverageDto
   CreateMap<Coverage, CoverageDto>()
        .ReverseMap();

        // CreateCoverageDto -> Coverage
  CreateMap<CreateCoverageDto, Coverage>()
  .ForMember(dest => dest.Id, opt => opt.MapFrom(src => Guid.NewGuid().ToString()))
      .ForMember(dest => dest.Status, opt => opt.MapFrom(src => "active"))
     .ForMember(dest => dest.CreatedAt, opt => opt.MapFrom(src => DateTime.UtcNow))
       .ForMember(dest => dest.DeductibleMet, opt => opt.MapFrom(src => 0m));

 // UpdateCoverageDto -> Coverage
     CreateMap<UpdateCoverageDto, Coverage>()
        .ForMember(dest => dest.UpdatedAt, opt => opt.MapFrom(src => DateTime.UtcNow))
    .ForAllMembers(opts => opts.Condition((src, dest, srcMember) => srcMember != null));
    }
}

/// <summary>
/// AutoMapper profile for Organization entity mappings
/// </summary>
public class OrganizationMappingProfile : Profile
{
    public OrganizationMappingProfile()
    {
  // Organization -> OrganizationDto
        CreateMap<Organization, OrganizationDto>()
 .ReverseMap();

  // Organization -> ProviderDto
     CreateMap<Organization, ProviderDto>()
.ForMember(dest => dest.OrganizationName, opt => opt.MapFrom(src => src.OrganizationName));

        // Organization -> InsurerDto
     CreateMap<Organization, InsurerDto>()
      .ForMember(dest => dest.OrganizationName, opt => opt.MapFrom(src => src.OrganizationName));

        // CreateOrganizationDto -> Organization
    CreateMap<CreateOrganizationDto, Organization>()
     .ForMember(dest => dest.Id, opt => opt.MapFrom(src => Guid.NewGuid().ToString()))
    .ForMember(dest => dest.Status, opt => opt.MapFrom(src => "active"))
 .ForMember(dest => dest.CreatedAt, opt => opt.MapFrom(src => DateTime.UtcNow));

        // UpdateOrganizationDto -> Organization
    CreateMap<UpdateOrganizationDto, Organization>()
     .ForMember(dest => dest.UpdatedAt, opt => opt.MapFrom(src => DateTime.UtcNow))
   .ForAllMembers(opts => opts.Condition((src, dest, srcMember) => srcMember != null));
  }
}
