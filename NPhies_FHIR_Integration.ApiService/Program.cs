using NPhies_FHIR_Integration.Common.Constants;
using NPhies_FHIR_Integration.Infrastructure.Repositories;
using NPhies_FHIR_Integration.Infrastructure.Data;
using NPhies_FHIR_Integration.Infrastructure.Seeding;
using NPhies_FHIR_Integration.Domain.Interfaces;
using NPhies_FHIR_Integration.Application.Services;
using NPhies_FHIR_Integration.Application.Services.MasterDataServices;
using NPhies_FHIR_Integration.Application.Mapping;
using NPhies_FHIR_Integration.ApiService.Security.Extensions;
using NPhies_FHIR_Integration.ApiService.Security.Services;
using NPhies_FHIR_Integration.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using NPhies_FHIR_Integration.Application.Services.Masters;

var builder = WebApplication.CreateBuilder(args);

// Add service defaults & Aspire client integrations.
builder.AddServiceDefaults();

// Add services to the container.
builder.Services.AddProblemDetails();
builder.Services.AddOpenApi();

// Add DbContext - Use In-Memory Database for now (NuGet server unavailable)
// TODO: Change to UseSqlServer when NuGet is available
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseInMemoryDatabase("NPhiesDb_Development"));

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
builder.Services.AddScoped<EnhancedDatabaseSeeder>();
builder.Services.AddScoped<ErrorCodeMasterSeeder>();

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

// Register Error Code Service for NPHIES error code management
builder.Services.AddScoped<IErrorCodeService, ErrorCodeService>();

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

// Seed test data for in-memory database
using (var scope = app.Services.CreateScope())
{
    var context = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
    var passwordService = scope.ServiceProvider.GetRequiredService<IPasswordHashingService>();
    
    // Create test users for login testing
    var testUser1 = new User
    {
        Id = Guid.NewGuid().ToString(),
      Username = "test.reviewer",
  Email = "test.reviewer@example.com",
        FirstName = "Test",
        LastName = "Reviewer",
 IsActive = true,
  IsEmailVerified = true,
        Roles = new List<string> { "TECHNICAL_REVIEWER" },
    CreatedAt = DateTime.UtcNow
    };

    var testUser2 = new User
    {
        Id = Guid.NewGuid().ToString(),
        Username = "admin.manager",
  Email = "admin.manager@example.com",
        FirstName = "Admin",
        LastName = "Manager",
        IsActive = true,
        IsEmailVerified = true,
        Roles = new List<string> { "TECHNICAL_REVIEW_MANAGER" },
   CreatedAt = DateTime.UtcNow
    };

    var testUser3 = new User
    {
        Id = Guid.NewGuid().ToString(),
        Username = "john.reviewer",
        Email = "john.reviewer@example.com",
        FirstName = "John",
     LastName = "Reviewer",
        IsActive = true,
        IsEmailVerified = true,
        Roles = new List<string> { "TECHNICAL_REVIEWER" },
 CreatedAt = DateTime.UtcNow
    };

    // Hash passwords
  var password = "TestPassword123!";
    var (hash1, salt1) = passwordService.HashPassword(password);
    testUser1.PasswordHash = hash1;
    testUser1.PasswordSalt = salt1;

    var (hash2, salt2) = passwordService.HashPassword("AdminPassword123!");
    testUser2.PasswordHash = hash2;
    testUser2.PasswordSalt = salt2;

    var (hash3, salt3) = passwordService.HashPassword(password);
    testUser3.PasswordHash = hash3;
    testUser3.PasswordSalt = salt3;

    // Add users to database
    context.Users.AddRange(testUser1, testUser2, testUser3);
    await context.SaveChangesAsync();
    
    Console.WriteLine("? Test users created successfully!");
    Console.WriteLine("   - test.reviewer / TestPassword123!");
    Console.WriteLine("   - admin.manager / AdminPassword123!");
    Console.WriteLine("   - john.reviewer / TestPassword123!");
}

if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

app.MapControllers();
app.MapDefaultEndpoints();

app.Run();

record WeatherForecast(DateOnly Date, int TemperatureC, string? Summary)
{
    public int TemperatureF => 32 + (int)(TemperatureC / 0.5556);
}
