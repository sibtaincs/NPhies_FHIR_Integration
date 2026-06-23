using AutoMapper;
using NPhies_FHIR_Integration.Application.DTOs;
using NPhies_FHIR_Integration.Domain.Entities;

namespace NPhies_FHIR_Integration.Application.Mapping;

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
}
