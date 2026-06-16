using NPhies_FHIR_Integration.Common.Constants;
using NPhies_FHIR_Integration.Infrastructure.Repositories;
using NPhies_FHIR_Integration.Infrastructure.Data;
using NPhies_FHIR_Integration.Infrastructure.Seeding;
using NPhies_FHIR_Integration.Domain.Interfaces;
using NPhies_FHIR_Integration.Application.Services;
using NPhies_FHIR_Integration.Application.Mapping;
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
builder.Services.AddAutoMapper(typeof(EligibilityMappingProfile));

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

// Register services
builder.Services.AddScoped<IEligibilityService, EligibilityService>();
builder.Services.AddScoped<IFhirToEntityMapper, FhirToEntityMapper>();
builder.Services.AddScoped<DatabaseSeeder>();

// Register Claim services
builder.Services.AddScoped<IClaimService, ClaimService>();
builder.Services.AddScoped<IClaimItemService, ClaimItemService>();
builder.Services.AddScoped<IClaimDiagnosisService, ClaimDiagnosisService>();
builder.Services.AddScoped<IClaimResponseService, ClaimResponseService>();

// Add controllers
builder.Services.AddControllers();

var app = builder.Build();

// Configure the HTTP request pipeline.
app.UseExceptionHandler();

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
