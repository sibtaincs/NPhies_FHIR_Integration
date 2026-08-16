using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using NPhies_FHIR_Integration.Domain.Entities;
using NPhies_FHIR_Integration.Infrastructure.Data;

namespace NPhies_FHIR_Integration.Infrastructure.Seeding;

/// <summary>
/// Comprehensive Database Seeder - All sample data seeding in one place
/// Seeds data in the correct dependency order: Master/Reference tables first, then child tables
/// </summary>
public class ComprehensiveDatabaseSeeder
{
    private readonly ApplicationDbContext _context;
    private readonly ILogger<ComprehensiveDatabaseSeeder> _logger;

 public ComprehensiveDatabaseSeeder(
        ApplicationDbContext context,
  ILogger<ComprehensiveDatabaseSeeder> logger)
    {
     _context = context ?? throw new ArgumentNullException(nameof(context));
     _logger = logger ?? throw new ArgumentNullException(nameof(logger));
    }

  /// <summary>
    /// Seeds all database data in the correct dependency order
    /// </summary>
    public async Task SeedAllAsync()
  {
      try
      {
 _logger.LogInformation("?? Starting comprehensive database seeding...");
            _logger.LogInformation("???????????????????????????????????????????????????");

        // Check if already seeded
            if (await IsAlreadySeededAsync())
            {
  _logger.LogInformation("??  Database already contains data. Skipping seeding.");
         await LogSeedingSummaryAsync();
     return;
        }

    // === PHASE 1: MASTER/REFERENCE TABLES (No dependencies) ===
       _logger.LogInformation("?? PHASE 1: Seeding Master/Reference Tables...");
            
         await SeedServiceCodeMastersAsync();
         await SeedMedicationCodeMastersAsync();
   await SeedMedicalDeviceCodeMastersAsync();
            await SeedDiagnosisCodeMastersAsync();
      await SeedModifierCodeMastersAsync();
      await SeedBenefitCodeMastersAsync();
            await SeedNphiesCodeMappingAsync();
   await SeedClaimSubmissionRulesAsync();
    
    _logger.LogInformation("? Phase 1 complete: Master/Reference tables seeded");

   // === PHASE 2: ORGANIZATIONS (Required for everything else) ===
_logger.LogInformation("?? PHASE 2: Seeding Organizations...");
    
            var organizations = await SeedOrganizationsAsync();
        
          _logger.LogInformation("? Phase 2 complete: Organizations seeded");

            // === PHASE 3: LOCATION & PRACTITIONER DATA (Depends on Organizations) ===
            _logger.LogInformation("?? PHASE 3: Seeding Locations and Practitioners...");
     
 await SeedLocationsAsync(organizations);
var practitioners = await SeedPractitionersAsync(organizations);
 
 _logger.LogInformation("? Phase 3 complete: Locations and Practitioners seeded");

  // === PHASE 4: PAYER & POLICY DATA (Depends on Organizations) ===
 _logger.LogInformation("?? PHASE 4: Seeding Payers and Policies...");
         
         await SeedPayerMastersAsync();
         await SeedPayerPolicyMastersAsync();
            await SeedPolicyBenefitCoverageAsync();
         
            _logger.LogInformation("? Phase 4 complete: Payers and Policies seeded");

    // === PHASE 5: CLINIC & DOCTOR DATA (Depends on Organizations) ===
   _logger.LogInformation("?? PHASE 5: Seeding Clinics and Doctors...");
        
          await SeedClinicMastersAsync(organizations);
await SeedDoctorMastersAsync(practitioners);
  await SeedDoctorQualificationsAsync();
   
      _logger.LogInformation("? Phase 5 complete: Clinics and Doctors seeded");

      // === PHASE 6: PATIENT & COVERAGE DATA (Depends on Organizations) ===
        _logger.LogInformation("?? PHASE 6: Seeding Patients and Coverages...");
  
            var patients = await SeedPatientsAsync();
 var coverages = await SeedCoveragesAsync(patients, organizations);
            
            _logger.LogInformation("? Phase 6 complete: Patients and Coverages seeded");

       // === PHASE 7: MESSAGING & ELIGIBILITY DATA (Depends on everything) ===
            _logger.LogInformation("?? PHASE 7: Seeding Message Headers and Eligibility Data...");
            
            var messageHeaders = await SeedMessageHeadersAsync(organizations);
            await SeedEligibilityDataAsync(patients, coverages, organizations, messageHeaders);
          
            _logger.LogInformation("? Phase 7 complete: Message Headers and Eligibility Data seeded");

     _logger.LogInformation("???????????????????????????????????????????????????");
         _logger.LogInformation("? Comprehensive database seeding completed successfully!");
            
  await LogSeedingSummaryAsync();
        }
      catch (Exception ex)
        {
_logger.LogError(ex, "? Error during comprehensive database seeding");
       throw;
    }
    }

    private async Task<bool> IsAlreadySeededAsync()
    {
     var existingServices = await _context.ServiceCodeMasters.CountAsync();
        var existingPayers = await _context.PayerMasters.CountAsync();
   var existingPatients = await _context.Patients.CountAsync();

        if (existingServices > 0 || existingPayers > 0 || existingPatients > 0)
  {
            _logger.LogInformation("   - Service Codes: {Count}", existingServices);
       _logger.LogInformation("   - Payers: {Count}", existingPayers);
            _logger.LogInformation("   - Patients: {Count}", existingPatients);
    return true;
        }

  return false;
    }

    #region PHASE 1: Master/Reference Tables

    private async Task SeedServiceCodeMastersAsync()
    {
        _logger.LogInformation("   ? Seeding Service Code Masters...");

        var serviceCodes = new List<ServiceCodeMaster>
   {
 // Office Visits
        new ServiceCodeMaster { ServiceCode = "99201", ServiceName = "Office Visit - New Patient (Brief)", ServiceCategory = "Office Visits", DefaultPrice = 100m, CurrencyCode = "SAR", IsNphiesMapped = true, IsActive = true, CreatedBy = "System", ModifiedBy = "System" },
            new ServiceCodeMaster { ServiceCode = "99213", ServiceName = "Office Visit - Established Patient", ServiceCategory = "Office Visits", DefaultPrice = 150m, CurrencyCode = "SAR", IsNphiesMapped = true, IsActive = true, CreatedBy = "System", ModifiedBy = "System" },
    
            // Emergency
          new ServiceCodeMaster { ServiceCode = "99281", ServiceName = "Emergency Department Visit - Level 1", ServiceCategory = "Emergency", DefaultPrice = 300m, CurrencyCode = "SAR", IsNphiesMapped = true, IsActive = true, CreatedBy = "System", ModifiedBy = "System" },
   new ServiceCodeMaster { ServiceCode = "99285", ServiceName = "Emergency Department Visit - Level 5", ServiceCategory = "Emergency", DefaultPrice = 1000m, CurrencyCode = "SAR", IsNphiesMapped = true, IsActive = true, CreatedBy = "System", ModifiedBy = "System" },
            
            // Laboratory
            new ServiceCodeMaster { ServiceCode = "85025", ServiceName = "Complete Blood Count (CBC)", ServiceCategory = "Laboratory", DefaultPrice = 50m, CurrencyCode = "SAR", IsNphiesMapped = true, IsActive = true, CreatedBy = "System", ModifiedBy = "System" },
     new ServiceCodeMaster { ServiceCode = "80053", ServiceName = "Comprehensive Metabolic Panel", ServiceCategory = "Laboratory", DefaultPrice = 75m, CurrencyCode = "SAR", IsNphiesMapped = true, IsActive = true, CreatedBy = "System", ModifiedBy = "System" },
        };

      _context.ServiceCodeMasters.AddRange(serviceCodes);
        await _context.SaveChangesAsync();
   _logger.LogInformation("   ? Service codes seeded: {Count}", serviceCodes.Count);
    }

    private async Task SeedMedicationCodeMastersAsync()
    {
        _logger.LogInformation("   ? Seeding Medication Code Masters...");

        var medications = new List<MedicationCodeMaster>
        {
   new MedicationCodeMaster { MedicationCode = "AMX-500", MedicationName = "Amoxicillin 500mg", ActiveIngredient = "Amoxicillin", Strength = "500", Unit = "mg", Form = "Capsule", UnitPrice = 5m, CurrencyCode = "SAR", IsActive = true, CreatedBy = "System", ModifiedBy = "System" },
   new MedicationCodeMaster { MedicationCode = "IBU-400", MedicationName = "Ibuprofen 400mg", ActiveIngredient = "Ibuprofen", Strength = "400", Unit = "mg", Form = "Tablet", UnitPrice = 2m, CurrencyCode = "SAR", IsActive = true, CreatedBy = "System", ModifiedBy = "System" },
            new MedicationCodeMaster { MedicationCode = "MET-500", MedicationName = "Metformin 500mg", ActiveIngredient = "Metformin HCl", Strength = "500", Unit = "mg", Form = "Tablet", UnitPrice = 3m, CurrencyCode = "SAR", IsActive = true, CreatedBy = "System", ModifiedBy = "System" },
 };

        _context.MedicationCodeMasters.AddRange(medications);
      await _context.SaveChangesAsync();
        _logger.LogInformation("   ? Medications seeded: {Count}", medications.Count);
    }

    private async Task SeedMedicalDeviceCodeMastersAsync()
    {
    _logger.LogInformation("   ? Seeding Medical Device Code Masters...");

   var devices = new List<MedicalDeviceCodeMaster>
      {
            new MedicalDeviceCodeMaster { DeviceCode = "HIP-001", DeviceName = "Total Hip Prosthesis", DeviceType = "Orthopedic Implant", UnitPrice = 8000m, CurrencyCode = "SAR", IsImplantable = true, IsActive = true, CreatedBy = "System", ModifiedBy = "System" },
  new MedicalDeviceCodeMaster { DeviceCode = "STENT-001", DeviceName = "Drug-Eluting Cardiac Stent", DeviceType = "Cardiac Device", UnitPrice = 1500m, CurrencyCode = "SAR", IsImplantable = true, IsActive = true, CreatedBy = "System", ModifiedBy = "System" },
     };

        _context.MedicalDeviceCodeMasters.AddRange(devices);
        await _context.SaveChangesAsync();
        _logger.LogInformation("   ? Devices seeded: {Count}", devices.Count);
    }

    private async Task SeedDiagnosisCodeMastersAsync()
    {
      _logger.LogInformation("   ? Seeding Diagnosis Code Masters...");

        var diagnoses = new List<DiagnosisCodeMaster>
        {
    new DiagnosisCodeMaster { DiagnosisCode = "I10", DiagnosisName = "Essential Hypertension", DiagnosisCategory = "Circulatory", DiagnosisType = "Chronic", Severity = "Moderate", IsActive = true, CreatedBy = "System", ModifiedBy = "System" },
        new DiagnosisCodeMaster { DiagnosisCode = "E11", DiagnosisName = "Type 2 Diabetes Mellitus", DiagnosisCategory = "Endocrine", DiagnosisType = "Chronic", Severity = "Moderate", IsActive = true, CreatedBy = "System", ModifiedBy = "System" },
 new DiagnosisCodeMaster { DiagnosisCode = "J45", DiagnosisName = "Asthma", DiagnosisCategory = "Respiratory", DiagnosisType = "Chronic", Severity = "Moderate", IsActive = true, CreatedBy = "System", ModifiedBy = "System" },
   };

        _context.DiagnosisCodeMasters.AddRange(diagnoses);
        await _context.SaveChangesAsync();
        _logger.LogInformation("   ? Diagnoses seeded: {Count}", diagnoses.Count);
    }

    private async Task SeedModifierCodeMastersAsync()
    {
 _logger.LogInformation("   ? Seeding Modifier Code Masters...");

        var modifiers = new List<ModifierCodeMaster>
   {
   new ModifierCodeMaster { ModifierCode = "25", ModifierName = "Significant E/M Service", ModifierType = "E/M Service", ImpactOnCharges = "No Change", ChargePercentage = 100m, IsActive = true, CreatedBy = "System", ModifiedBy = "System" },
      new ModifierCodeMaster { ModifierCode = "50", ModifierName = "Bilateral Procedure", ModifierType = "Bilateral", ImpactOnCharges = "Increase", ChargePercentage = 150m, IsActive = true, CreatedBy = "System", ModifiedBy = "System" },
        };

   _context.ModifierCodeMasters.AddRange(modifiers);
        await _context.SaveChangesAsync();
        _logger.LogInformation("   ? Modifiers seeded: {Count}", modifiers.Count);
    }

    private async Task SeedBenefitCodeMastersAsync()
    {
     _logger.LogInformation(" ? Seeding Benefit Code Masters...");

        var benefits = new List<BenefitCodeMaster>
        {
   new BenefitCodeMaster { BenefitCode = "INPATIENT", BenefitName = "Inpatient Hospital Services", BenefitCategory = "Hospital", IsActive = true, CreatedBy = "System", ModifiedBy = "System" },
         new BenefitCodeMaster { BenefitCode = "OUTPATIENT", BenefitName = "Outpatient Services", BenefitCategory = "Ambulatory", IsActive = true, CreatedBy = "System", ModifiedBy = "System" },
            new BenefitCodeMaster { BenefitCode = "EMERGENCY", BenefitName = "Emergency Services", BenefitCategory = "Emergency", IsActive = true, CreatedBy = "System", ModifiedBy = "System" },
        };

  _context.BenefitCodeMasters.AddRange(benefits);
  await _context.SaveChangesAsync();
   _logger.LogInformation("   ? Benefits seeded: {Count}", benefits.Count);
    }

    private async Task SeedNphiesCodeMappingAsync()
  {
        _logger.LogInformation("   ? Seeding NPHIES Code Mappings...");

 var mappings = new List<NphiesCodeMapping>
        {
            new NphiesCodeMapping { LocalCode = "INPATIENT", LocalCodeSystem = "LOCAL", NphiesCode = "inpatient", NphiesCodeSystem = "http://nphies.sa", CodeType = "BenefitCategory", IsMappingValid = true, MappingValidationDate = DateTime.UtcNow, CreatedBy = "System", ModifiedBy = "System" },
   new NphiesCodeMapping { LocalCode = "I10", LocalCodeSystem = "ICD-10", NphiesCode = "I10", NphiesCodeSystem = "http://hl7.org/fhir/sid/icd-10", CodeType = "DiagnosisCode", IsMappingValid = true, MappingValidationDate = DateTime.UtcNow, CreatedBy = "System", ModifiedBy = "System" },
};

 _context.NphiesCodeMappings.AddRange(mappings);
     await _context.SaveChangesAsync();
    _logger.LogInformation("   ? Mappings seeded: {Count}", mappings.Count);
    }

    private async Task SeedClaimSubmissionRulesAsync()
    {
        _logger.LogInformation("   ? Seeding Claim Submission Rules...");

        var rules = new List<ClaimSubmissionRules>
      {
 new ClaimSubmissionRules { RuleName = "Maximum Claim Amount", RuleType = "Financial", RuleCondition = "Total claim amount must not exceed maximum limit", MaxClaimAmount = 500000m, IsActive = true, Priority = 100, CreatedBy = "System", ModifiedBy = "System" },
            new ClaimSubmissionRules { RuleName = "Service Date Validation", RuleType = "Date", RuleCondition = "Service date must be within past 90 days", MaxDaysForSubmission = 90, IsActive = true, Priority = 95, CreatedBy = "System", ModifiedBy = "System" },
        };

        _context.ClaimSubmissionRules.AddRange(rules);
     await _context.SaveChangesAsync();
        _logger.LogInformation("   ? Rules seeded: {Count}", rules.Count);
    }

    #endregion

    #region PHASE 2: Organizations

    private async Task<List<Organization>> SeedOrganizationsAsync()
    {
        _logger.LogInformation("   ? Seeding Organizations...");

        var organizations = new List<Organization>
        {
      // Provider Organizations
  new Organization
          {
        Id = Guid.NewGuid().ToString(),
           OrganizationName = "Al-Noor Medical Center",
     LicenseNumber = "PROV-001-2024",
    LicenseSystem = "http://nphies.sa/license/provider-license",
         OrganizationType = "prov",
       Website = "https://alnoor.med.sa",
          Email = "info@alnoor.med.sa",
      PhoneNumber = "+966112341234",
 AddressLine1 = "123 Medical Street",
           City = "Riyadh",
          State = "Riyadh",
                PostalCode = "11111",
        Status = "active",
      CreatedAt = DateTime.UtcNow,
         IsActive = true
    },
        new Organization
            {
         Id = Guid.NewGuid().ToString(),
        OrganizationName = "Gulf Health Specialty Clinic",
       LicenseNumber = "PROV-002-2024",
   LicenseSystem = "http://nphies.sa/license/provider-license",
      OrganizationType = "prov",
                Email = "info@gulfhealth.sa",
                PhoneNumber = "+966112341235",
                AddressLine1 = "456 Medical Boulevard",
    City = "Riyadh",
           State = "Riyadh",
              PostalCode = "11222",
                Status = "active",
     CreatedAt = DateTime.UtcNow,
     IsActive = true
         },
            // Insurer Organization
            new Organization
            {
   Id = Guid.NewGuid().ToString(),
     OrganizationName = "Saudi Health Insurance",
 LicenseNumber = "INS-001-2024",
     LicenseSystem = "http://nphies.sa/license/payer-license",
OrganizationType = "ins",
Email = "support@insurance.sa",
              PhoneNumber = "+966114441111",
         AddressLine1 = "789 Insurance Boulevard",
     City = "Riyadh",
        State = "Riyadh",
          PostalCode = "11333",
    Status = "active",
         CreatedAt = DateTime.UtcNow,
   IsActive = true
         }
        };

      _context.Organizations.AddRange(organizations);
     await _context.SaveChangesAsync();
        _logger.LogInformation(" ? Organizations seeded: {Count}", organizations.Count);

      return organizations;
    }

    #endregion

#region PHASE 3: Locations & Practitioners

    private async Task SeedLocationsAsync(List<Organization> organizations)
    {
 _logger.LogInformation("   ? Seeding Locations...");

var provider = organizations.First(o => o.OrganizationType == "prov");

        var locations = new List<Location>
        {
      new Location
       {
     Id = Guid.NewGuid().ToString(),
        LocationName = "Emergency Department",
    OrganizationId = provider.Id,
         FacilityType = "hospital",
            AddressLine1 = "123 Medical Street, Building A",
         City = "Riyadh",
    State = "Riyadh",
            PostalCode = "11111",
         Country = "SA",
   Phone = "+966112341234",
     Status = "active",
         CreatedAt = DateTime.UtcNow
          }
        };

        _context.Locations.AddRange(locations);
    await _context.SaveChangesAsync();
   _logger.LogInformation("   ? Locations seeded: {Count}", locations.Count);
    }

    private async Task<List<Practitioner>> SeedPractitionersAsync(List<Organization> organizations)
    {
   _logger.LogInformation("   ? Seeding Practitioners...");

        var provider = organizations.First(o => o.OrganizationType == "prov");

        var practitioners = new List<Practitioner>
   {
     new Practitioner
            {
        Id = Guid.NewGuid().ToString(),
   FirstName = "Ahmed",
   LastName = "Al-Rashid",
                LicenseNumber = "DOC-001-2024",
          Specialization = "General Practitioner",
     Qualification = "MD",
OrganizationId = provider.Id,
     Status = "active",
                CreatedAt = DateTime.UtcNow
},
       new Practitioner
    {
           Id = Guid.NewGuid().ToString(),
         FirstName = "Fatima",
     LastName = "Al-Sulaiman",
       LicenseNumber = "DOC-002-2024",
              Specialization = "Pediatrics",
            Qualification = "MD",
             OrganizationId = provider.Id,
     Status = "active",
                CreatedAt = DateTime.UtcNow
    }
        };

      _context.Practitioners.AddRange(practitioners);
        await _context.SaveChangesAsync();
        _logger.LogInformation("   ? Practitioners seeded: {Count}", practitioners.Count);

        return practitioners;
    }

  #endregion

    #region PHASE 4: Payers & Policies

    private async Task SeedPayerMastersAsync()
    {
        _logger.LogInformation("   ? Seeding Payer Masters...");

        var payers = new List<PayerMaster>
  {
            new PayerMaster { PayerId = "7001", PayerName = "Bupa Arabia", PayerNameArabic = "???? ???????", PayerType = "Insurance", City = "Riyadh", Country = "SA", IsActive = true, CreatedBy = "System", ModifiedBy = "System" },
   new PayerMaster { PayerId = "7002", PayerName = "Tawuniya", PayerNameArabic = "?????????", PayerType = "Insurance", City = "Riyadh", Country = "SA", IsActive = true, CreatedBy = "System", ModifiedBy = "System" },
     };

  _context.PayerMasters.AddRange(payers);
        await _context.SaveChangesAsync();
        _logger.LogInformation("   ? Payers seeded: {Count}", payers.Count);
    }

    private async Task SeedPayerPolicyMastersAsync()
    {
    _logger.LogInformation(" ? Seeding Payer Policy Masters...");

        var payers = await _context.PayerMasters.ToListAsync();
        var policies = new List<PayerPolicyMaster>();

        foreach (var payer in payers)
        {
   policies.Add(new PayerPolicyMaster
            {
                PayerMasterId = payer.Id,
                PolicyCode = $"{payer.PayerId}-STD",
                PolicyName = $"{payer.PayerName} Standard Plan",
      PolicyType = "Individual",
      CoverageLevel = "Standard",
     AnnualPremium = 4000m,
           CurrencyCode = "SAR",
   PolicyStartDate = new DateTime(2024, 1, 1),
  PolicyEndDate = new DateTime(2024, 12, 31),
        IsPolicyActive = true,
    CreatedBy = "System",
            ModifiedBy = "System"
    });
   }

  _context.PayerPolicyMasters.AddRange(policies);
      await _context.SaveChangesAsync();
        _logger.LogInformation("   ? Policies seeded: {Count}", policies.Count);
    }

    private async Task SeedPolicyBenefitCoverageAsync()
    {
        _logger.LogInformation("   ? Seeding Policy Benefit Coverage...");

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
             MaxCoverageAmount = 50000m,
           RequiresPreAuth = false,
    CreatedBy = "System",
     ModifiedBy = "System"
     });
      }
        }

        _context.PolicyBenefitCoverages.AddRange(coverages);
        await _context.SaveChangesAsync();
     _logger.LogInformation("   ? Coverage mappings seeded: {Count}", coverages.Count);
    }

    #endregion

    #region PHASE 5: Clinics & Doctors

    private async Task SeedClinicMastersAsync(List<Organization> organizations)
    {
        _logger.LogInformation("   ? Seeding Clinic Masters...");

        var providers = organizations.Where(o => o.OrganizationType == "prov").ToList();

        var clinics = new List<ClinicMaster>
        {
            new ClinicMaster
      {
                OrganizationId = providers[0].Id,
      ClinicCode = "CLI-001",
                ClinicName = "Al-Noor Medical Center",
   ClinicType = "General Hospital",
    NumberOfBeds = 150,
              NumberOfDoctors = 45,
    NumberOfNurses = 120,
         AccreditationLevel = "Platinum",
     IsEmergencyAvailable = true,
      PharmacyAvailable = true,
      LabAvailable = true,
       ImagingAvailable = true,
  IsActive = true,
       CreatedBy = "System",
          ModifiedBy = "System"
            }
        };

        _context.ClinicMasters.AddRange(clinics);
        await _context.SaveChangesAsync();
     _logger.LogInformation(" ? Clinics seeded: {Count}", clinics.Count);
    }

    private async Task SeedDoctorMastersAsync(List<Practitioner> practitioners)
 {
  _logger.LogInformation("   ? Seeding Doctor Masters...");

 var clinics = await _context.ClinicMasters.ToListAsync();
        if (!clinics.Any())
        {
            _logger.LogWarning("   ??  No clinics found. Skipping doctor seeding.");
            return;
}

        var doctors = new List<DoctorMaster>
        {
            new DoctorMaster
            {
     PractitionerId = practitioners[0].Id,
    DoctorCode = "DOC-001",
             DoctorName = "Dr. Ahmed Al-Rashid",
 Specialization = "General Practice",
    QualificationDegree = "MD",
         YearsOfExperience = 10,
  ConsultationFee = 200m,
            CurrencyCode = "SAR",
        ClinicMasterId = clinics[0].Id,
         IsActive = true,
    CreatedBy = "System",
       ModifiedBy = "System"
            },
            new DoctorMaster
            {
      PractitionerId = practitioners[1].Id,
       DoctorCode = "DOC-002",
  DoctorName = "Dr. Fatima Al-Sulaiman",
     Specialization = "Pediatrics",
   QualificationDegree = "MD",
      YearsOfExperience = 8,
    ConsultationFee = 250m,
                CurrencyCode = "SAR",
     ClinicMasterId = clinics[0].Id,
     IsActive = true,
   CreatedBy = "System",
  ModifiedBy = "System"
            }
        };

        _context.DoctorMasters.AddRange(doctors);
        await _context.SaveChangesAsync();
    _logger.LogInformation(" ? Doctors seeded: {Count}", doctors.Count);
    }

    private async Task SeedDoctorQualificationsAsync()
    {
        _logger.LogInformation("   ? Seeding Doctor Qualifications...");

        var doctors = await _context.DoctorMasters.ToListAsync();
   if (!doctors.Any())
        {
 _logger.LogWarning("   ??  No doctors found. Skipping qualifications seeding.");
            return;
  }

        var qualifications = new List<DoctorQualification>();

 foreach (var doctor in doctors)
        {
 qualifications.Add(new DoctorQualification
          {
       DoctorMasterId = doctor.Id,
             QualificationType = "Medical Degree",
   QualificationName = "Doctor of Medicine (MD)",
     UniversityName = "King Saud University",
          IssuedDate = DateTime.UtcNow.AddYears(-doctor.YearsOfExperience ?? -10),
                VerificationStatus = "Verified",
      CreatedBy = "System"
  });
      }

        _context.DoctorQualifications.AddRange(qualifications);
   await _context.SaveChangesAsync();
      _logger.LogInformation("   ? Qualifications seeded: {Count}", qualifications.Count);
    }

    #endregion

    #region PHASE 6: Patients & Coverages

    private async Task<List<Patient>> SeedPatientsAsync()
    {
        _logger.LogInformation("   ? Seeding Patients...");

        var patients = new List<Patient>
        {
            new Patient
      {
       Id = Guid.NewGuid().ToString(),
      MRN = "MRN-001-2024",
                NationalId = "1234567890",
       FirstName = "Mohammed",
      LastName = "Al-Mutairi",
 DateOfBirth = new DateTime(1985, 5, 15),
        Gender = "M",
         Phone = "+966541234567",
                City = "Riyadh",
      State = "Riyadh",
       PostalCode = "11111",
  Country = "SA",
                Status = "active",
     CreatedAt = DateTime.UtcNow,
     IsActive = true
            },
     new Patient
       {
  Id = Guid.NewGuid().ToString(),
   MRN = "MRN-002-2024",
     NationalId = "1234567891",
        FirstName = "Sarah",
        LastName = "Al-Dosari",
 DateOfBirth = new DateTime(1990, 8, 22),
 Gender = "F",
      Phone = "+966541234568",
      City = "Riyadh",
         State = "Riyadh",
          PostalCode = "11222",
        Country = "SA",
         Status = "active",
                CreatedAt = DateTime.UtcNow,
   IsActive = true
            }
        };

     _context.Patients.AddRange(patients);
        await _context.SaveChangesAsync();
        _logger.LogInformation("   ? Patients seeded: {Count}", patients.Count);

        return patients;
    }

    private async Task<List<Coverage>> SeedCoveragesAsync(List<Patient> patients, List<Organization> organizations)
    {
     _logger.LogInformation("   ? Seeding Coverages...");

        var insurer = organizations.FirstOrDefault(o => o.OrganizationType == "ins");
        if (insurer == null)
        {
            _logger.LogWarning("   ??  No insurer found. Skipping coverage seeding.");
         return new List<Coverage>();
   }

        var coverages = new List<Coverage>
        {
    new Coverage
     {
     Id = Guid.NewGuid().ToString(),
        PolicyNumber = "POL-001-2024",
                MemberID = "MEM-001-2024",
      PatientId = patients[0].Id,
              InsurerId = insurer.Id,
     Status = "active",
                CoverageStartDate = new DateTime(2024, 1, 1),
       CoverageEndDate = new DateTime(2024, 12, 31),
    CreatedAt = DateTime.UtcNow,
             IsActive = true
   },
       new Coverage
            {
   Id = Guid.NewGuid().ToString(),
  PolicyNumber = "POL-002-2024",
     MemberID = "MEM-002-2024",
      PatientId = patients[1].Id,
   InsurerId = insurer.Id,
        Status = "active",
 CoverageStartDate = new DateTime(2024, 1, 1),
      CoverageEndDate = new DateTime(2024, 12, 31),
      CreatedAt = DateTime.UtcNow,
         IsActive = true
}
        };

        _context.Coverages.AddRange(coverages);
    await _context.SaveChangesAsync();
        _logger.LogInformation("   ? Coverages seeded: {Count}", coverages.Count);

        return coverages;
    }

    #endregion

    #region PHASE 7: Messaging & Eligibility

    private async Task<List<MessageHeader>> SeedMessageHeadersAsync(List<Organization> organizations)
    {
      _logger.LogInformation("   ? Seeding Message Headers...");

        var provider = organizations.First(o => o.OrganizationType == "prov");
        var insurer = organizations.First(o => o.OrganizationType == "ins");

      var messageHeaders = new List<MessageHeader>
 {
  new MessageHeader
   {
      Id = Guid.NewGuid().ToString(),
           MessageUUID = Guid.NewGuid().ToString(),
        EventCode = "coverage-eligibility-request",
       SenderOrganizationId = provider.Id,
        DestinationOrganizationId = insurer.Id,
      Status = "received",
                MessageTimestamp = DateTime.UtcNow,
                CreatedAt = DateTime.UtcNow,
         IsActive = true
            }
        };

        _context.MessageHeaders.AddRange(messageHeaders);
        await _context.SaveChangesAsync();
   _logger.LogInformation("   ? Message headers seeded: {Count}", messageHeaders.Count);

        return messageHeaders;
    }

    private async Task SeedEligibilityDataAsync(
        List<Patient> patients,
  List<Coverage> coverages,
        List<Organization> organizations,
        List<MessageHeader> messageHeaders)
    {
 _logger.LogInformation("   ? Seeding Eligibility Data...");

        var provider = organizations.First(o => o.OrganizationType == "prov");
        var insurer = organizations.First(o => o.OrganizationType == "ins");

        var request = new CoverageEligibilityRequest
        {
       Id = Guid.NewGuid().ToString(),
        RequestId = $"REQ-{DateTime.UtcNow:yyyyMMddHHmmss}",
            MessageHeaderId = messageHeaders[0].Id,
  Status = "active",
 PatientId = patients[0].Id,
            CoverageId = coverages[0].Id,
        ProviderId = provider.Id,
    InsurerId = insurer.Id,
ServiceDate = DateTime.UtcNow.AddDays(7),
     RequestCreatedAt = DateTime.UtcNow,
     CreatedAt = DateTime.UtcNow,
     IsActive = true
        };

        _context.CoverageEligibilityRequests.Add(request);
     await _context.SaveChangesAsync();
        _logger.LogInformation("   ? Eligibility data seeded: 1 request");
    }

    #endregion

    #region Summary Logging

    private async Task LogSeedingSummaryAsync()
    {
        try
        {
          var serviceCodes = await _context.ServiceCodeMasters.CountAsync();
          var medications = await _context.MedicationCodeMasters.CountAsync();
         var devices = await _context.MedicalDeviceCodeMasters.CountAsync();
      var diagnoses = await _context.DiagnosisCodeMasters.CountAsync();
  var payers = await _context.PayerMasters.CountAsync();
            var policies = await _context.PayerPolicyMasters.CountAsync();
  var doctors = await _context.DoctorMasters.CountAsync();
            var clinics = await _context.ClinicMasters.CountAsync();
       var organizations = await _context.Organizations.CountAsync();
            var practitioners = await _context.Practitioners.CountAsync();
    var patients = await _context.Patients.CountAsync();
      var coverages = await _context.Coverages.CountAsync();

          _logger.LogInformation("?? Database Seeding Summary:");
    _logger.LogInformation("???????????????????????????????????????????????????");
            _logger.LogInformation("MASTER DATA:");
 _logger.LogInformation("   ? Service Codes: {Count}", serviceCodes);
            _logger.LogInformation("   ? Medications: {Count}", medications);
    _logger.LogInformation("   ? Medical Devices: {Count}", devices);
  _logger.LogInformation(" ? Diagnoses: {Count}", diagnoses);
       _logger.LogInformation("   ? Payers: {Count}", payers);
   _logger.LogInformation("   ? Policies: {Count}", policies);
      _logger.LogInformation("? Doctors: {Count}", doctors);
    _logger.LogInformation("   ? Clinics: {Count}", clinics);
        _logger.LogInformation("");
            _logger.LogInformation("OPERATIONAL DATA:");
 _logger.LogInformation("   ? Organizations: {Count}", organizations);
  _logger.LogInformation("   ? Practitioners: {Count}", practitioners);
    _logger.LogInformation("   ? Patients: {Count}", patients);
      _logger.LogInformation(" ? Coverages: {Count}", coverages);
            _logger.LogInformation("???????????????????????????????????????????????????");

  var totalRecords = serviceCodes + medications + devices + diagnoses + payers +
     policies + doctors + clinics + organizations + practitioners +
     patients + coverages;

    _logger.LogInformation("?? Total Records Seeded: {Count}", totalRecords);
       _logger.LogInformation("?? Database is ready for use!");
        }
        catch (Exception ex)
     {
            _logger.LogWarning(ex, "Could not generate seeding summary");
      }
    }

    #endregion
}
