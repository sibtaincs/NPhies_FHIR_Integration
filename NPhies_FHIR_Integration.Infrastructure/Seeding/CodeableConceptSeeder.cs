using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using NPhies_FHIR_Integration.Infrastructure.Data;
using NPhies_FHIR_Integration.Domain.CodeableConcept.Models;

namespace NPhies_FHIR_Integration.Infrastructure.Seeding;

/// <summary>
/// Seeder for NPHIES CodeableConcept database initial data
/// </summary>
public class CodeableConceptSeeder
{
    private readonly ApplicationDbContext _context;
    private readonly ILogger<CodeableConceptSeeder> _logger;

    public CodeableConceptSeeder(
        ApplicationDbContext context,
        ILogger<CodeableConceptSeeder> logger)
    {
        _context = context ?? throw new ArgumentNullException(nameof(context));
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
    }

    /// <summary>
    /// Seed all CodeableConcept data
    /// </summary>
    public async Task SeedAllAsync()
    {
        try
        {
            _logger.LogInformation("Starting CodeableConcept database seeding...");

            // Check if database already has data
            var hasData = await _context.CodeSystems.AnyAsync();
            if (hasData)
            {
                _logger.LogInformation("CodeableConcept database already seeded. Skipping...");
                return;
            }

            await SeedCodeSystemsAsync();
            await SeedValueSetsAsync();
            await SeedMessageTypesAsync();
            
            await _context.SaveChangesAsync();

            _logger.LogInformation("CodeableConcept database seeding completed successfully!");
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error seeding CodeableConcept database");
            throw;
        }
    }

    private async Task SeedCodeSystemsAsync()
    {
        _logger.LogInformation("Seeding CodeSystems...");

        var codeSystems = new List<CodeSystemEntity>
        {
            // HL7 FHIR CodeSystems
            new() { Url = "http://terminology.hl7.org/CodeSystem/claim-type", Name = "claim-type", Title = "Claim Type Codes", IsActive = true },
            new() { Url = "http://terminology.hl7.org/CodeSystem/ex-claimsubtype", Name = "ex-claimsubtype", Title = "Example Claim SubType Codes", IsActive = true },
            new() { Url = "http://terminology.hl7.org/CodeSystem/ex-diagnosistype", Name = "ex-diagnosistype", Title = "Example Diagnosis Type Codes", IsActive = true },
            new() { Url = "http://terminology.hl7.org/CodeSystem/ex-benefitcategory", Name = "ex-benefitcategory", Title = "Benefit Category Codes", IsActive = true },
            
            // NPHIES CodeSystems
            new() { Url = "http://nphies.sa/terminology/CodeSystem/claim-subtype", Name = "claim-subtype", Title = "NPHIES Claim SubType", IsActive = true },
            new() { Url = "http://nphies.sa/terminology/CodeSystem/diagnosis-type", Name = "diagnosis-type", Title = "NPHIES Diagnosis Type", IsActive = true },
            new() { Url = "http://nphies.sa/terminology/CodeSystem/procedures", Name = "procedures", Title = "NPHIES Procedure Codes", IsActive = true },
            new() { Url = "http://nphies.sa/terminology/CodeSystem/services", Name = "services", Title = "NPHIES Service Codes", IsActive = true },
            new() { Url = "http://nphies.sa/terminology/CodeSystem/medication-codes", Name = "medication-codes", Title = "NPHIES Medication Codes", IsActive = true },
            new() { Url = "http://nphies.sa/terminology/CodeSystem/medical-devices", Name = "medical-devices", Title = "NPHIES Medical Device Codes", IsActive = true },
            new() { Url = "http://nphies.sa/terminology/CodeSystem/adjudication-error", Name = "adjudication-error", Title = "NPHIES Adjudication Error Codes", IsActive = true },
            new() { Url = "http://nphies.sa/terminology/CodeSystem/fdi-tooth-surface", Name = "fdi-tooth-surface", Title = "FDI Tooth Surface Codes", IsActive = true },
            new() { Url = "http://nphies.sa/terminology/CodeSystem/fdi-oral-region", Name = "fdi-oral-region", Title = "FDI Oral Region Codes", IsActive = true },
            new() { Url = "http://nphies.sa/terminology/CodeSystem/bodysite", Name = "bodysite", Title = "Body Site Codes", IsActive = true },
        };

        _context.CodeSystems.AddRange(codeSystems);
        _logger.LogInformation("Added {Count} CodeSystems", codeSystems.Count);
    }

    private async Task SeedValueSetsAsync()
    {
        _logger.LogInformation("Seeding ValueSets...");

        var valueSets = new List<ValueSetEntity>
        {
            new() { Url = "http://nphies.sa/terminology/ValueSet/claim-type", Name = "claim-type", Title = "Claim Type ValueSet", IsActive = true },
            new() { Url = "http://nphies.sa/terminology/ValueSet/claim-subtype", Name = "claim-subtype", Title = "Claim SubType ValueSet", IsActive = true },
            new() { Url = "http://nphies.sa/terminology/ValueSet/diagnosis-type", Name = "diagnosis-type", Title = "Diagnosis Type ValueSet", IsActive = true },
            new() { Url = "http://nphies.sa/terminology/ValueSet/institutional-billing", Name = "institutional-billing", Title = "Institutional Billing Codes", IsActive = true },
            new() { Url = "http://nphies.sa/terminology/ValueSet/professional-billing", Name = "professional-billing", Title = "Professional Billing Codes", IsActive = true },
            new() { Url = "http://nphies.sa/terminology/ValueSet/pharmacy-billing", Name = "pharmacy-billing", Title = "Pharmacy Billing Codes", IsActive = true },
            new() { Url = "http://nphies.sa/terminology/ValueSet/oral-billing", Name = "oral-billing", Title = "Oral/Dental Billing Codes", IsActive = true },
        };

        _context.ValueSets.AddRange(valueSets);
        _logger.LogInformation("Added {Count} ValueSets", valueSets.Count);
    }

    private async Task SeedMessageTypesAsync()
    {
        _logger.LogInformation("Seeding NPHIES Message Types...");

        var messageTypes = new List<NphiesMessageTypeEntity>
        {
            new() { MessageType = "eligibility-request", FhirResourceType = "CoverageEligibilityRequest", Description = "Coverage eligibility request", IsActive = true },
            new() { MessageType = "eligibility-response", FhirResourceType = "CoverageEligibilityResponse", Description = "Coverage eligibility response", IsActive = true },
            new() { MessageType = "claim-request", FhirResourceType = "Claim", Description = "Claim submission request", IsActive = true },
            new() { MessageType = "claim-response", FhirResourceType = "ClaimResponse", Description = "Claim adjudication response", IsActive = true },
            new() { MessageType = "priorauth-request", FhirResourceType = "Claim", Description = "Prior authorization request", IsActive = true },
            new() { MessageType = "priorauth-response", FhirResourceType = "ClaimResponse", Description = "Prior authorization response", IsActive = true },
            new() { MessageType = "cancel-request", FhirResourceType = "Task", Description = "Cancellation request", IsActive = true },
            new() { MessageType = "cancel-response", FhirResourceType = "Task", Description = "Cancellation response", IsActive = true },
            new() { MessageType = "communication-request", FhirResourceType = "CommunicationRequest", Description = "Communication request", IsActive = true },
            new() { MessageType = "communication", FhirResourceType = "Communication", Description = "Communication response", IsActive = true },
            new() { MessageType = "payment-notice", FhirResourceType = "PaymentNotice", Description = "Payment notice", IsActive = true },
            new() { MessageType = "payment-reconciliation", FhirResourceType = "PaymentReconciliation", Description = "Payment reconciliation", IsActive = true },
            new() { MessageType = "status-check", FhirResourceType = "Task", Description = "Status check request", IsActive = true },
            new() { MessageType = "status-response", FhirResourceType = "Task", Description = "Status check response", IsActive = true },
        };

        _context.NphiesMessageTypes.AddRange(messageTypes);
        _logger.LogInformation("Added {Count} NPHIES Message Types", messageTypes.Count);
    }
}