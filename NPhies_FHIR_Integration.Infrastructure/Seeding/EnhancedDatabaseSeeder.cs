using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using NPhies_FHIR_Integration.Domain.Entities;
using NPhies_FHIR_Integration.Infrastructure.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace NPhies_FHIR_Integration.Infrastructure.Seeding;

/// <summary>
/// Complete Database Seeder for Master Data
/// Seeds all master data tables with NPHIES-compliant data
/// </summary>
public class EnhancedDatabaseSeeder : IDatabaseSeeder
{
    private readonly ApplicationDbContext _context;
    private readonly ILogger<EnhancedDatabaseSeeder> _logger;

    public EnhancedDatabaseSeeder(
   ApplicationDbContext context,
        ILogger<EnhancedDatabaseSeeder> logger)
    {
        _context = context ?? throw new ArgumentNullException(nameof(context));
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
    }

    /// <summary>
    /// Seed all master data
    /// </summary>
    public async Task SeedAllMasterDataAsync()
    {
        _logger.LogInformation("Starting comprehensive master data seeding...");

        try
        {
            // Check if already seeded
            if (await _context.ServiceCodeMasters.AnyAsync())
            {
                _logger.LogWarning("Master data already seeded. Skipping.");
                return;
            }

            // Seed in dependency order
            await SeedServiceCodeMastersAsync();
            await SeedMedicationCodeMastersAsync();
            await SeedMedicalDeviceCodeMastersAsync();
            await SeedDiagnosisCodeMastersAsync();
            await SeedModifierCodeMastersAsync();
            await SeedBenefitCodeMastersAsync();
            await SeedPayerMastersAsync();
            await SeedPayerPolicyMastersAsync();
            await SeedPolicyBenefitCoverageAsync();
            await SeedClaimSubmissionRulesAsync();
            await SeedNphiesCodeMappingAsync();
            await SeedClinicMastersAsync();
            await SeedDoctorMastersAsync();
            await SeedDoctorQualificationsAsync();

            await _context.SaveChangesAsync();
            _logger.LogInformation("? All master data seeded successfully!");
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "? Error seeding master data");
            throw;
        }
    }

    /// <summary>
    /// Seed service code masters (procedures, services)
    /// </summary>
    private async Task SeedServiceCodeMastersAsync()
    {
        _logger.LogInformation("Seeding Service Code Masters...");

        var serviceCodes = new List<ServiceCodeMaster>
        {
      new ServiceCodeMaster { ServiceCode = "99213", ServiceName = "Office Visit - Established Patient", ServiceCategory = "Office Visits", DefaultPrice = 150m, IsNphiesMapped = true, IsActive = true },
            new ServiceCodeMaster { ServiceCode = "99214", ServiceName = "Office Visit - Established Patient (Extended)", ServiceCategory = "Office Visits", DefaultPrice = 200m, IsNphiesMapped = true, IsActive = true },
            new ServiceCodeMaster { ServiceCode = "99215", ServiceName = "Office Visit - Established Patient (Complex)", ServiceCategory = "Office Visits", DefaultPrice = 250m, IsNphiesMapped = true, IsActive = true },
      new ServiceCodeMaster { ServiceCode = "70450", ServiceName = "CT Head", ServiceCategory = "Radiology", DefaultPrice = 800m, IsNphiesMapped = true, IsActive = true },
     new ServiceCodeMaster { ServiceCode = "71020", ServiceName = "Chest X-Ray", ServiceCategory = "Radiology", DefaultPrice = 150m, IsNphiesMapped = true, IsActive = true },
            new ServiceCodeMaster { ServiceCode = "27447", ServiceName = "Total Knee Replacement", ServiceCategory = "Surgery", DefaultPrice = 45000m, IsNphiesMapped = true, IsActive = true },
  new ServiceCodeMaster { ServiceCode = "29881", ServiceName = "Knee Arthroscopy", ServiceCategory = "Surgery", DefaultPrice = 8000m, IsNphiesMapped = true, IsActive = true },
          new ServiceCodeMaster { ServiceCode = "80053", ServiceName = "Comprehensive Metabolic Panel", ServiceCategory = "Laboratory", DefaultPrice = 75m, IsNphiesMapped = true, IsActive = true },
            new ServiceCodeMaster { ServiceCode = "85025", ServiceName = "Complete Blood Count", ServiceCategory = "Laboratory", DefaultPrice = 50m, IsNphiesMapped = true, IsActive = true },
            new ServiceCodeMaster { ServiceCode = "90834", ServiceName = "Psychotherapy - 45 minutes", ServiceCategory = "Mental Health", DefaultPrice = 200m, IsNphiesMapped = true, IsActive = true },
        };

        _context.ServiceCodeMasters.AddRange(serviceCodes);
        _logger.LogInformation("Added {Count} service codes", serviceCodes.Count);
    }

    /// <summary>
    /// Seed medication code masters
    /// </summary>
    private async Task SeedMedicationCodeMastersAsync()
    {
        _logger.LogInformation("Seeding Medication Code Masters...");

        var medications = new List<MedicationCodeMaster>
        {
            new MedicationCodeMaster { MedicationCode = "00378-0033", MedicationName = "Amoxicillin 500mg", UnitPrice = 5m, IsNphiesMapped = true, IsActive = true },
            new MedicationCodeMaster { MedicationCode = "00378-1010", MedicationName = "Ibuprofen 400mg", UnitPrice = 2m, IsNphiesMapped = true, IsActive = true },
   new MedicationCodeMaster { MedicationCode = "00069-2500", MedicationName = "Metformin 500mg", UnitPrice = 3m, IsNphiesMapped = true, IsActive = true },
   new MedicationCodeMaster { MedicationCode = "00597-0050", MedicationName = "Lisinopril 10mg", UnitPrice = 4m, IsNphiesMapped = true, IsActive = true },
    new MedicationCodeMaster { MedicationCode = "00228-2919", MedicationName = "Simvastatin 20mg", UnitPrice = 6m, IsNphiesMapped = true, IsActive = true },
  new MedicationCodeMaster { MedicationCode = "00054-0162", MedicationName = "Levothyroxine 75mcg", UnitPrice = 8m, IsNphiesMapped = true, IsActive = true },
            new MedicationCodeMaster { MedicationCode = "00078-0024", MedicationName = "Sertraline 50mg", UnitPrice = 10m, IsNphiesMapped = true, IsActive = true },
       new MedicationCodeMaster { MedicationCode = "00093-5113", MedicationName = "Omeprazole 20mg", UnitPrice = 4m, IsNphiesMapped = true, IsActive = true },
        };

        _context.MedicationCodeMasters.AddRange(medications);
        _logger.LogInformation("Added {Count} medications", medications.Count);
    }

    /// <summary>
    /// Seed medical device code masters
    /// </summary>
    private async Task SeedMedicalDeviceCodeMastersAsync()
    {
        _logger.LogInformation("Seeding Medical Device Code Masters...");

        var devices = new List<MedicalDeviceCodeMaster>
        {
            new MedicalDeviceCodeMaster { DeviceCode = "K061053", DeviceName = "Artificial Hip Joint", UnitPrice = 8000m, IsNphiesMapped = true, IsActive = true },
    new MedicalDeviceCodeMaster { DeviceCode = "K040968", DeviceName = "Cardiac Stent", UnitPrice = 1500m, IsNphiesMapped = true, IsActive = true },
          new MedicalDeviceCodeMaster { DeviceCode = "K082571", DeviceName = "Dialysis Filter", UnitPrice = 300m, IsNphiesMapped = true, IsActive = true },
            new MedicalDeviceCodeMaster { DeviceCode = "K070570", DeviceName = "Contact Lens", UnitPrice = 50m, IsNphiesMapped = true, IsActive = true },
  new MedicalDeviceCodeMaster { DeviceCode = "K183657", DeviceName = "Surgical Implant Screw", UnitPrice = 200m, IsNphiesMapped = true, IsActive = true },
        };

        _context.MedicalDeviceCodeMasters.AddRange(devices);
        _logger.LogInformation("Added {Count} devices", devices.Count);
    }

    /// <summary>
    /// Seed diagnosis code masters (ICD-10 sample)
    /// </summary>
    private async Task SeedDiagnosisCodeMastersAsync()
    {
        _logger.LogInformation("Seeding Diagnosis Code Masters...");

        var diagnoses = new List<DiagnosisCodeMaster>
      {
      new DiagnosisCodeMaster { DiagnosisCode = "I10", DiagnosisName = "Essential (primary) hypertension", DiagnosisCategory = "Circulatory", IsNphiesMapped = true, IsActive = true },
            new DiagnosisCodeMaster { DiagnosisCode = "E11", DiagnosisName = "Type 2 diabetes mellitus", DiagnosisCategory = "Endocrine", IsNphiesMapped = true, IsActive = true },
    new DiagnosisCodeMaster { DiagnosisCode = "E66", DiagnosisName = "Overweight and obesity", DiagnosisCategory = "Nutritional", IsNphiesMapped = true, IsActive = true },
       new DiagnosisCodeMaster { DiagnosisCode = "I50", DiagnosisName = "Heart failure", DiagnosisCategory = "Circulatory", IsNphiesMapped = true, IsActive = true },
            new DiagnosisCodeMaster { DiagnosisCode = "J44", DiagnosisName = "Chronic obstructive pulmonary disease", DiagnosisCategory = "Respiratory", IsNphiesMapped = true, IsActive = true },
 new DiagnosisCodeMaster { DiagnosisCode = "M19", DiagnosisName = "Unspecified osteoarthritis", DiagnosisCategory = "Musculoskeletal", IsNphiesMapped = true, IsActive = true },
      new DiagnosisCodeMaster { DiagnosisCode = "F41", DiagnosisName = "Anxiety disorders", DiagnosisCategory = "Mental", IsNphiesMapped = true, IsActive = true },
    new DiagnosisCodeMaster { DiagnosisCode = "Z12", DiagnosisName = "Encounter for screening for malignant neoplasms", DiagnosisCategory = "Screening", IsNphiesMapped = true, IsActive = true },
        };

        _context.DiagnosisCodeMasters.AddRange(diagnoses);
        _logger.LogInformation("Added {Count} diagnosis codes", diagnoses.Count);
    }

    /// <summary>
    /// Seed modifier code masters
    /// </summary>
    private async Task SeedModifierCodeMastersAsync()
    {
        _logger.LogInformation("Seeding Modifier Code Masters...");

        var modifiers = new List<ModifierCodeMaster>
        {
       new ModifierCodeMaster { ModifierCode = "25", ModifierName = "Significant, Separately Identifiable Evaluation and Management Service", ModifierDescription = "For E&M services", IsActive = true },
       new ModifierCodeMaster { ModifierCode = "26", ModifierName = "Professional Component", ModifierDescription = "Professional component only", IsActive = true },
  new ModifierCodeMaster { ModifierCode = "27", ModifierName = "Technical Component", ModifierDescription = "Technical component only", IsActive = true },
       new ModifierCodeMaster { ModifierCode = "59", ModifierName = "Distinct Procedural Service", ModifierDescription = "Distinct procedural service", IsActive = true },
  new ModifierCodeMaster { ModifierCode = "76", ModifierName = "Repeat Procedure by Same Physician", ModifierDescription = "Repeat procedure", IsActive = true },
        new ModifierCodeMaster { ModifierCode = "77", ModifierName = "Repeat Procedure by Another Physician", ModifierDescription = "Repeat by another physician", IsActive = true },
   new ModifierCodeMaster { ModifierCode = "78", ModifierName = "Return to the Operating Room for a Related Procedure", ModifierDescription = "Return to OR", IsActive = true },
 new ModifierCodeMaster { ModifierCode = "91", ModifierName = "Repeat Clinical Diagnostic Laboratory Test", ModifierDescription = "Repeat lab test", IsActive = true },
      };

        _context.ModifierCodeMasters.AddRange(modifiers);
        _logger.LogInformation("Added {Count} modifiers", modifiers.Count);
    }

    /// <summary>
    /// Seed benefit code masters
    /// </summary>
    private async Task SeedBenefitCodeMastersAsync()
    {
        _logger.LogInformation("Seeding Benefit Code Masters...");

        var benefits = new List<BenefitCodeMaster>
        {
 new BenefitCodeMaster { BenefitCode = "INPATIENT", BenefitName = "Inpatient Hospital Benefits", IsActive = true },
            new BenefitCodeMaster { BenefitCode = "OUTPATIENT", BenefitName = "Outpatient Services", IsActive = true },
            new BenefitCodeMaster { BenefitCode = "EMERGENCY", BenefitName = "Emergency Services", IsActive = true },
    new BenefitCodeMaster { BenefitCode = "SURGERY", BenefitName = "Surgical Services", IsActive = true },
          new BenefitCodeMaster { BenefitCode = "PHARMACY", BenefitName = "Pharmacy/Prescription Drugs", IsActive = true },
   new BenefitCodeMaster { BenefitCode = "DENTAL", BenefitName = "Dental Services", IsActive = true },
  new BenefitCodeMaster { BenefitCode = "VISION", BenefitName = "Vision Services", IsActive = true },
            new BenefitCodeMaster { BenefitCode = "MENTAL", BenefitName = "Mental Health Services", IsActive = true },
new BenefitCodeMaster { BenefitCode = "REHABILITATION", BenefitName = "Rehabilitation Services", IsActive = true },
            new BenefitCodeMaster { BenefitCode = "MATERNITY", BenefitName = "Maternity Services", IsActive = true },
        };

        _context.BenefitCodeMasters.AddRange(benefits);
        _logger.LogInformation("Added {Count} benefit codes", benefits.Count);
    }

    /// <summary>
    /// Seed payer masters (insurance companies)
    /// </summary>
    private async Task SeedPayerMastersAsync()
    {
        _logger.LogInformation("Seeding Payer Masters...");

        var payers = new List<PayerMaster>
        {
 new PayerMaster { PayerId = "PAY-001", PayerName = "Gulf Healthcare Insurance", IsActive = true },
            new PayerMaster { PayerId = "PAY-002", PayerName = "National Health Insurance", IsActive = true },
            new PayerMaster { PayerId = "PAY-003", PayerName = "Private Medical Insurance", IsActive = true },
         new PayerMaster { PayerId = "PAY-004", PayerName = "Cooperative Insurance", IsActive = true },
   new PayerMaster { PayerId = "PAY-005", PayerName = "Government Health Services", IsActive = true },
        };

        _context.PayerMasters.AddRange(payers);
        _logger.LogInformation("Added {Count} payers", payers.Count);
    }

    /// <summary>
    /// Seed payer policy masters
    /// </summary>
    private async Task SeedPayerPolicyMastersAsync()
    {
        _logger.LogInformation("Seeding Payer Policy Masters...");

        var policies = new List<PayerPolicyMaster>
        {
         new PayerPolicyMaster
     {
            PolicyCode = "POL-GOLD-001",
      PolicyName = "Gold Premium Plan",
       PayerMasterId = (await _context.PayerMasters.FirstAsync(p => p.PayerId == "PAY-001")).Id,
   AnnualDeductible = 5000m,
        IsPolicyActive = true
            },
new PayerPolicyMaster
  {
                PolicyCode = "POL-SILVER-001",
    PolicyName = "Silver Standard Plan",
         PayerMasterId = (await _context.PayerMasters.FirstAsync(p => p.PayerId == "PAY-001")).Id,
     AnnualDeductible = 10000m,
     IsPolicyActive = true
   },
   new PayerPolicyMaster
            {
  PolicyCode = "POL-BASIC-001",
       PolicyName = "Basic Catastrophic Plan",
 PayerMasterId = (await _context.PayerMasters.FirstAsync(p => p.PayerId == "PAY-002")).Id,
     AnnualDeductible = 15000m,
      IsPolicyActive = true
         },
        };

        _context.PayerPolicyMasters.AddRange(policies);
        _logger.LogInformation("Added {Count} policies", policies.Count);
    }

    /// <summary>
    /// Seed policy benefit coverage
    /// </summary>
    private async Task SeedPolicyBenefitCoverageAsync()
    {
        _logger.LogInformation("Seeding Policy Benefit Coverage...");

        var policies = await _context.PayerPolicyMasters.ToListAsync();
        var benefits = await _context.BenefitCodeMasters.ToListAsync();

        var coverages = new List<PolicyBenefitCoverage>();
        foreach (var policy in policies)
        {
            foreach (var benefit in benefits)
            {
                coverages.Add(new PolicyBenefitCoverage
                {
                    PolicyMasterId = policy.Id,
                    ServiceCategory = benefit.BenefitCode,
                    BenefitType = benefit.BenefitName,
                    CoveragePercentage = 80m,
                    MaxCoverageAmount = 100000m,
                    RequiresPreAuth = false
                });
            }
        }

        _context.PolicyBenefitCoverages.AddRange(coverages);
        _logger.LogInformation("Added {Count} benefit coverage mappings", coverages.Count);
    }

    /// <summary>
    /// Seed claim submission rules
    /// </summary>
    private async Task SeedClaimSubmissionRulesAsync()
    {
        _logger.LogInformation("Seeding Claim Submission Rules...");

        var rules = new List<ClaimSubmissionRules>
    {
            new ClaimSubmissionRules { RuleName = "Max Claim Amount", MaxClaimAmount = 500000m, IsActive = true, RuleCondition = "Maximum allowed claim amount" },
       new ClaimSubmissionRules { RuleName = "Duplicate Check", MaxClaimAmount = null, IsActive = true, RuleCondition = "Check for duplicate claims within 24 hours" },
            new ClaimSubmissionRules { RuleName = "Service Date Validation", MaxClaimAmount = null, IsActive = true, RuleCondition = "Service date must be within past 90 days" },
      new ClaimSubmissionRules { RuleName = "Provider Network Check", MaxClaimAmount = null, IsActive = true, RuleCondition = "Provider must be in-network" },
            new ClaimSubmissionRules { RuleName = "Authorization Requirement", MaxClaimAmount = null, IsActive = true, RuleCondition = "Prior authorization required for certain services" },
        };

        _context.ClaimSubmissionRules.AddRange(rules);
        _logger.LogInformation("Added {Count} submission rules", rules.Count);
    }

    /// <summary>
    /// Seed NPHIES code mappings
    /// </summary>
    private async Task SeedNphiesCodeMappingAsync()
    {
        _logger.LogInformation("Seeding NPHIES Code Mappings...");

        var mappings = new List<NphiesCodeMapping>
        {
            new NphiesCodeMapping { LocalCode = "INPATIENT", NphiesCode = "inpatient", CodeType = "BenefitCategory", LocalCodeSystem = "LOCAL", NphiesCodeSystem = "http://nphies.sa/terminology/CodeSystem/benefit-category", IsMappingValid = true },
            new NphiesCodeMapping { LocalCode = "OUTPATIENT", NphiesCode = "outpatient", CodeType = "BenefitCategory", LocalCodeSystem = "LOCAL", NphiesCodeSystem = "http://nphies.sa/terminology/CodeSystem/benefit-category", IsMappingValid = true },
            new NphiesCodeMapping { LocalCode = "EMERGENCY", NphiesCode = "emergency", CodeType = "BenefitCategory", LocalCodeSystem = "LOCAL", NphiesCodeSystem = "http://nphies.sa/terminology/CodeSystem/benefit-category", IsMappingValid = true },
           new NphiesCodeMapping { LocalCode = "I10", NphiesCode = "I10", CodeType = "DiagnosisCode", LocalCodeSystem = "ICD-10", NphiesCodeSystem = "http://nphies.sa/terminology/CodeSystem/diagnosis-code", IsMappingValid = true },
           new NphiesCodeMapping { LocalCode = "E11", NphiesCode = "E11", CodeType = "DiagnosisCode", LocalCodeSystem = "ICD-10", NphiesCodeSystem = "http://nphies.sa/terminology/CodeSystem/diagnosis-code", IsMappingValid = true },
 };

        _context.NphiesCodeMappings.AddRange(mappings);
        _logger.LogInformation("Added {Count} NPHIES code mappings", mappings.Count);
    }

    /// <summary>
    /// Seed clinic masters
    /// </summary>
    private async Task SeedClinicMastersAsync()
    {
        _logger.LogInformation("Seeding Clinic Masters...");

        var clinics = new List<ClinicMaster>
   {
        new ClinicMaster { ClinicCode = "CLI-001", ClinicName = "Al-Noor Medical Center", IsActive = true },
            new ClinicMaster { ClinicCode = "CLI-002", ClinicName = "Gulf Health Clinic", IsActive = true },
            new ClinicMaster { ClinicCode = "CLI-003", ClinicName = "Premier Healthcare Services", IsActive = true },
      };

        _context.ClinicMasters.AddRange(clinics);
        _logger.LogInformation("Added {Count} clinics", clinics.Count);
    }

    /// <summary>
    /// Seed doctor masters
    /// </summary>
    private async Task SeedDoctorMastersAsync()
    {
        _logger.LogInformation("Seeding Doctor Masters...");

        var clinics = await _context.ClinicMasters.ToListAsync();

        var doctors = new List<DoctorMaster>
    {
    new DoctorMaster
  {
      DoctorCode = "DOC-001",
                DoctorName = "Dr. Ahmed AlKhaleej",
        BoardLicenseNumber = "LIC-12345",
          Specialization = "Cardiology",
            ConsultationFee = 300m,
         FollowupFee = 150m,
          ClinicMasterId = clinics.First().Id,
          IsActive = true
   },
            new DoctorMaster
    {
    DoctorCode = "DOC-002",
      DoctorName = "Dr. Fatima AlSuwaidi",
     BoardLicenseNumber = "LIC-12346",
     Specialization = "Pediatrics",
        ConsultationFee = 200m,
         FollowupFee = 100m,
        ClinicMasterId = clinics.First().Id,
         IsActive = true
         },
        };

        _context.DoctorMasters.AddRange(doctors);
        _logger.LogInformation("Added {Count} doctors", doctors.Count);
    }

    /// <summary>
    /// Seed doctor qualifications
    /// </summary>
    private async Task SeedDoctorQualificationsAsync()
    {
        _logger.LogInformation("Seeding Doctor Qualifications...");

        var doctors = await _context.DoctorMasters.ToListAsync();

        var qualifications = new List<DoctorQualification>
        {
            new DoctorQualification
            {
   DoctorMasterId = doctors.First().Id,
     QualificationType = "MD",
    QualificationName = "Doctor of Medicine",
    UniversityName = "King Saud University"
      },
      new DoctorQualification
  {
 DoctorMasterId = doctors.First().Id,
           QualificationType = "Board Certification",
                QualificationName = "American Board of Internal Medicine",
          UniversityName = "ABIM"
       },
        };

        _context.DoctorQualifications.AddRange(qualifications);
        _logger.LogInformation("Added {Count} qualifications", qualifications.Count);
    }
}

/// <summary>
/// Interface for database seeding
/// </summary>
public interface IDatabaseSeeder
{
    Task SeedAllMasterDataAsync();
}
