using NPhies_FHIR_Integration.Common.Constants;
using NPhies_FHIR_Integration.Infrastructure.Repositories;
using NPhies_FHIR_Integration.Infrastructure.Data;
using NPhies_FHIR_Integration.Domain.Interfaces;
using NPhies_FHIR_Integration.Application.Services;
using NPhies_FHIR_Integration.Application.Services.MasterDataServices;
using NPhies_FHIR_Integration.Application.Mapping;
using NPhies_FHIR_Integration.ApiService.Security.Extensions;
using NPhies_FHIR_Integration.ApiService.Security.Services;
using NPhies_FHIR_Integration.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using NPhies_FHIR_Integration.Application.Services.Masters;
using NPhies_FHIR_Integration.Domain.CodeableConcept.Services;
using NPhies_FHIR_Integration.Infrastructure.Seeding;

var builder = WebApplication.CreateBuilder(args);

// Add service defaults & Aspire client integrations.
builder.AddServiceDefaults();

// Add services to the container.
builder.Services.AddProblemDetails();
builder.Services.AddOpenApi();

// Add DbContext - Use SQL Server with default connection string
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection") ?? 
"Server=(localdb)\\mssqllocaldb;Database=NPhiesDb;Trusted_Connection=true;"));

// Add AutoMapper
builder.Services.AddAutoMapper(typeof(ApplicationMappingProfile), typeof(EligibilityMappingProfile));

// 🔒 ADD COMPREHENSIVE SECURITY SYSTEM
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

// Register Seeder services
builder.Services.AddScoped<ComprehensiveDatabaseSeeder>(); // NEW: Single comprehensive seeder
builder.Services.AddScoped<ErrorCodeMasterSeeder>(); // Kept separate (optional large dataset)
builder.Services.AddScoped<CodeableConceptSeeder>(); // Kept separate (optional large dataset)
builder.Services.AddScoped<PayerMasterSeeder>(); // Kept separate (additional payers)

// Register Claim services
builder.Services.AddScoped<IClaimService, ClaimService>();
builder.Services.AddScoped<IClaimItemService, ClaimItemService>();
builder.Services.AddScoped<IClaimDiagnosisService, ClaimDiagnosisService>();
builder.Services.AddScoped<IClaimResponseService, ClaimResponseService>();

// Phase 2: Register Payment Calculation Engine
builder.Services.AddScoped<IPaymentCalculationEngine, PaymentCalculationEngine>();

// Phase 2: Register Payment Service
builder.Services.AddScoped<IPaymentService, PaymentService>();

// 📚 MASTER DATA SERVICES - Phase 3
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

// ✅ CODEABLE CONCEPT SERVICE - Phase 4
builder.Services.AddScoped<ICodeableConceptService, CodeableConceptService>();

// ✅ RCM SERVICES - Status Tracking
// builder.Services.AddScoped<IClaimStatusTracker, ClaimStatusTracker>();

// NOTE: Phase 3 RCM Services temporarily commented out - will be implemented later
// builder.Services.AddScoped<IClaimResponseProcessingService, ClaimResponseProcessingService>();
// builder.Services.AddScoped<IAdjudicationWorkflowService, AdjudicationWorkflowService>();
// builder.Services.AddScoped<IAppealWorkflowService, AppealWorkflowService>();
// builder.Services.AddScoped<IDenialManagementService, DenialManagementService>();
// builder.Services.AddScoped<IPaymentReconciliationService, PaymentReconciliationService>();

var app = builder.Build();

// Configure the HTTP request pipeline.
app.UseExceptionHandler();

// 🔒 USE COMPREHENSIVE SECURITY SYSTEM
app.UseComprehensiveSecurity(builder.Configuration);

if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
    
  // Apply migrations and seed database in development
    using (var scope = app.Services.CreateScope())
 {
        var logger = scope.ServiceProvider.GetRequiredService<ILogger<Program>>();
        
        try
        {
      // ✅ STEP 1: Apply pending migrations
       logger.LogInformation("🔍 Checking database connection and pending migrations...");
            var context = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
            
    // Test connection first
            var canConnect = await context.Database.CanConnectAsync();
          if (!canConnect)
            {
        logger.LogError("❌ Cannot connect to database. Please check:");
            logger.LogError("   1. SQL Server is running");
       logger.LogError("   2. Connection string is correct");
        logger.LogError("   3. Database permissions are set");
          throw new Exception("Database connection failed");
}
            
      logger.LogInformation("✅ Database connection successful");

     var pendingMigrations = await context.Database.GetPendingMigrationsAsync();
            if (pendingMigrations.Any())
         {
           logger.LogInformation("⏳ Applying {Count} pending migrations: {Migrations}", 
          pendingMigrations.Count(), 
 string.Join(", ", pendingMigrations));
             await context.Database.MigrateAsync();
   logger.LogInformation("✅ Migrations applied successfully!");
       }
            else
     {
                logger.LogInformation("✅ Database is up to date. No pending migrations.");
 }
            
        // ✅ STEP 2: Seed data
          logger.LogInformation("🌱 Starting database seeding...");
 logger.LogInformation("═══════════════════════════════════════════════════");

   // Use Comprehensive Seeder (all sample data in dependency order)
       logger.LogInformation("📋 Step 1/3: Seeding All Sample Data...");
  var comprehensiveSeeder = scope.ServiceProvider.GetRequiredService<ComprehensiveDatabaseSeeder>();
            await comprehensiveSeeder.SeedAllAsync();

     // Optional: Seed Additional Payers
            logger.LogInformation("📋 Step 2/3: Seeding Additional Payers (Optional)...");
var payerSeeder = scope.ServiceProvider.GetRequiredService<PayerMasterSeeder>();
     await payerSeeder.SeedPayersAsync();
  await payerSeeder.SeedSamplePoliciesAsync();
            logger.LogInformation("✅ Additional payers seeded successfully");

// Optional: Seed Error Codes and Codeable Concepts (large datasets)
     logger.LogInformation("📋 Step 3/3: Seeding NPHIES Error Codes and Codeable Concepts (Optional)...");
       var errorCodeSeeder = scope.ServiceProvider.GetRequiredService<ErrorCodeMasterSeeder>();
     await errorCodeSeeder.SeedCriticalErrorCodesAsync();
            
       var codeableConceptSeeder = scope.ServiceProvider.GetRequiredService<CodeableConceptSeeder>();
       await codeableConceptSeeder.SeedAllAsync();
            logger.LogInformation("✅ Error codes and codeable concepts seeded successfully");

         logger.LogInformation("═══════════════════════════════════════════════════");
 logger.LogInformation("✅ All database seeding completed successfully!");
     }
   catch (Exception ex)
        {
      logger.LogError(ex, "❌ An error occurred during migration or seeding");
          logger.LogError("Error Type: {Type}", ex.GetType().Name);
        logger.LogError("Error Message: {Message}", ex.Message);
  
            if (ex.InnerException != null)
        {
    logger.LogError("Inner Error: {InnerMessage}", ex.InnerException.Message);
            }
      
         // Don't throw - allow app to start even if seeding fails
    logger.LogWarning("⚠️ Application will continue, but seeding may be incomplete");
  logger.LogWarning("⚠️ Please check the logs above for specific error details");
      }
    }
}

app.MapControllers();
app.MapDefaultEndpoints();

app.Run();
