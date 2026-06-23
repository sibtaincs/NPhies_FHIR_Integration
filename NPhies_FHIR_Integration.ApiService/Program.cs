using NPhies_FHIR_Integration.Common.Constants;
using NPhies_FHIR_Integration.Infrastructure.Repositories;
using NPhies_FHIR_Integration.Infrastructure.Data;
using NPhies_FHIR_Integration.Infrastructure.Seeding;
using NPhies_FHIR_Integration.Domain.Interfaces;
using NPhies_FHIR_Integration.Application.Services;
using NPhies_FHIR_Integration.Application.Mapping;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;

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

// ? ADD JWT AUTHENTICATION
var jwtSettings = builder.Configuration.GetSection("JwtSettings");
var key = Encoding.ASCII.GetBytes(jwtSettings["Secret"] ?? "your-super-secret-key-that-is-at-least-32-characters-long-for-security");

builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
})
.AddJwtBearer(options =>
{
    options.RequireHttpsMetadata = !builder.Environment.IsDevelopment();
    options.SaveToken = true;
    options.TokenValidationParameters = new TokenValidationParameters
    {
      ValidateIssuerSigningKey = true,
        IssuerSigningKey = new SymmetricSecurityKey(key),
        ValidateIssuer = true,
      ValidIssuer = jwtSettings["Issuer"] ?? "NPhiesIssuer",
        ValidateAudience = true,
        ValidAudience = jwtSettings["Audience"] ?? "NPhiesAudience",
        ValidateLifetime = true,
        ClockSkew = TimeSpan.FromSeconds(10)
    };
});

// ? ADD AUTHORIZATION POLICIES
builder.Services.AddAuthorization(options =>
{
    // RCM Processor role - can process claims
    options.AddPolicy("RCMProcessor", policy =>
    policy.RequireRole("Admin", "RCMProcessor"));
    
    // RCM Viewer role - read-only access
    options.AddPolicy("RCMViewer", policy =>
  policy.RequireRole("Admin", "RCMProcessor", "RCMViewer"));
    
    // Admin role - full access
    options.AddPolicy("AdminOnly", policy =>
        policy.RequireRole("Admin"));
});

// Enable CORS for Angular frontend
builder.Services.AddCors(options =>
{
 options.AddPolicy("AllowAngular", policy =>
    {
        policy.AllowAnyOrigin()
         .AllowAnyMethod()
      .AllowAnyHeader();
    });
});

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

// NOTE: Phase 3 RCM Services temporarily commented out - will be implemented later
// builder.Services.AddScoped<IClaimResponseProcessingService, ClaimResponseProcessingService>();
// builder.Services.AddScoped<IAdjudicationWorkflowService, AdjudicationWorkflowService>();
// builder.Services.AddScoped<IAppealWorkflowService, AppealWorkflowService>();
// builder.Services.AddScoped<IDenialManagementService, DenialManagementService>();
// builder.Services.AddScoped<IPaymentReconciliationService, PaymentReconciliationService>();

// Add controllers
builder.Services.AddControllers();

var app = builder.Build();

// Configure the HTTP request pipeline.
app.UseExceptionHandler();

// ? USE AUTHENTICATION & AUTHORIZATION
app.UseAuthentication();
app.UseAuthorization();

// ? USE HTTPS REDIRECTION
app.UseHttpsRedirection();

// Enable CORS
app.UseCors("AllowAngular");

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
