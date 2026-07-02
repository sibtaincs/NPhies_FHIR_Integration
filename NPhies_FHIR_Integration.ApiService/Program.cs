using NPhies_FHIR_Integration.Common.Constants;
using NPhies_FHIR_Integration.Infrastructure.Repositories;
using NPhies_FHIR_Integration.Infrastructure.Data;
using NPhies_FHIR_Integration.Infrastructure.Seeding;
using NPhies_FHIR_Integration.Domain.Interfaces;
using NPhies_FHIR_Integration.Application.Services;
using NPhies_FHIR_Integration.Application.Services.MasterDataServices;
using NPhies_FHIR_Integration.Application.Mapping;
using NPhies_FHIR_Integration.ApiService.Security.Extensions;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add service defaults & Aspire client integrations.
builder.AddServiceDefaults();

// Add services to the container.
builder.Services.AddProblemDetails();
builder.Services.AddOpenApi();

// Add DbContext
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection") ?? 
 "Server=(localdb)\\mssqllocaldb;Database=NPhiesDb;Trusted_Connection=true;"));

// Add AutoMapper
builder.Services.AddAutoMapper(typeof(ApplicationMappingProfile), typeof(EligibilityMappingProfile));

// ?? ADD COMPREHENSIVE SECURITY SYSTEM
builder.Services.AddComprehensiveSecurity(builder.Configuration);

// Add controllers
builder.Services.AddControllers();

// Register generic repository
builder.Services.AddScoped(typeof(IRepository<>), typeof(Repository<>));

// Register specialized repositories
builder.Services.AddScoped<ICoverageEligibilityRequestRepository, CoverageEligibilityRequestRepository>();
builder.Services.AddScoped<ICoverageEligibilityResponseRepository, CoverageEligibilityResponseRepository>();
builder.Services.AddScoped<IPatientRepository, PatientRepository>();
builder.Services.AddScoped<ICoverageRepository, CoverageRepository>();
builder.Services.AddScoped<IOrganizationRepository, OrganizationRepository>();
builder.Services.AddScoped<IBenefitBalanceRepository, BenefitBalanceRepository>();
builder.Services.AddScoped<IEligibilityErrorRepository, EligibilityErrorRepository>();
// Register Claim repositories
builder.Services.AddScoped<IClaimRepository, ClaimRepository>();
builder.Services.AddScoped<IClaimItemRepository, ClaimItemRepository>();
builder.Services.AddScoped<IClaimDiagnosisRepository, ClaimDiagnosisRepository>();
builder.Services.AddScoped<IClaimResponseRepository, ClaimResponseRepository>();

// Phase 2: Register Adjudication and Rejection repositories
builder.Services.AddScoped<IAdjudicationDetailRepository, AdjudicationDetailRepository>();
builder.Services.AddScoped<IRejectionReasonRepository, RejectionReasonRepository>();

// Register services
builder.Services.AddScoped<IEligibilityService, EligibilityService>();
builder.Services.AddScoped<IFhirToEntityMapper, FhirToEntityMapper>();
builder.Services.AddScoped<DatabaseSeeder>();

// Register Claim services
builder.Services.AddScoped<IClaimService, ClaimService>();
builder.Services.AddScoped<IClaimItemService, ClaimItemService>();
builder.Services.AddScoped<IClaimDiagnosisService, ClaimDiagnosisService>();
builder.Services.AddScoped<IClaimResponseService, ClaimResponseService>();

// Phase 2: Register Payment Calculation Engine
builder.Services.AddScoped<IPaymentCalculationEngine, PaymentCalculationEngine>();

// Phase 2: Register Payment Service
builder.Services.AddScoped<IPaymentService, PaymentService>();

// ?? MASTER DATA SERVICES - Phase 3
builder.Services.AddScoped<IServiceCodeMasterService, ServiceCodeMasterService>();
builder.Services.AddScoped<IMedicationCodeMasterService, MedicationCodeMasterService>();
builder.Services.AddScoped<IMedicalDeviceCodeMasterService, MedicalDeviceCodeMasterService>();
builder.Services.AddScoped<IDiagnosisCodeMasterService, DiagnosisCodeMasterService>();
builder.Services.AddScoped<IModifierCodeMasterService, ModifierCodeMasterService>();
builder.Services.AddScoped<IBenefitCodeMasterService, BenefitCodeMasterService>();
builder.Services.AddScoped<INphiesCodeMappingService, NphiesCodeMappingService>();
builder.Services.AddScoped<IPayerMasterService, PayerMasterService>();
builder.Services.AddScoped<IPayerPolicyMasterService, PayerPolicyMasterService>();
builder.Services.AddScoped<IPolicyBenefitCoverageService, PolicyBenefitCoverageService>();
builder.Services.AddScoped<IClinicMasterService, ClinicMasterService>();
builder.Services.AddScoped<IDoctorMasterService, DoctorMasterService>();
builder.Services.AddScoped<IDoctorQualificationService, DoctorQualificationService>();
builder.Services.AddScoped<IClaimSubmissionRulesService, ClaimSubmissionRulesService>();

// NOTE: Phase 3 RCM Services temporarily commented out - will be implemented later
// builder.Services.AddScoped<IClaimResponseProcessingService, ClaimResponseProcessingService>();
// builder.Services.AddScoped<IAdjudicationWorkflowService, AdjudicationWorkflowService>();
// builder.Services.AddScoped<IAppealWorkflowService, AppealWorkflowService>();
// builder.Services.AddScoped<IDenialManagementService, DenialManagementService>();
// builder.Services.AddScoped<IPaymentReconciliationService, PaymentReconciliationService>();

var app = builder.Build();

// Configure the HTTP request pipeline.
app.UseExceptionHandler();

// ?? USE COMPREHENSIVE SECURITY SYSTEM
app.UseComprehensiveSecurity(builder.Configuration);

if (app.Environment.IsDevelopment())
{
app.MapOpenApi();
  
  // Seed database in development
    using (var scope = app.Services.CreateScope())
    {
 var seeder = scope.ServiceProvider.GetRequiredService<DatabaseSeeder>();
        await seeder.SeedAsync();
    }
}

app.MapControllers();
app.MapDefaultEndpoints();

app.Run();

record WeatherForecast(DateOnly Date, int TemperatureC, string? Summary)
{
    public int TemperatureF => 32 + (int)(TemperatureC / 0.5556);
}
