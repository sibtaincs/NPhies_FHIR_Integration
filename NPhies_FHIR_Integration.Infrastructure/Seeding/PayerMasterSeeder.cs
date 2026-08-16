using NPhies_FHIR_Integration.Domain.Entities;
using NPhies_FHIR_Integration.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace NPhies_FHIR_Integration.Infrastructure.Seeding;

/// <summary>
/// Seeds Saudi Arabian Insurance Companies (Payers)
/// Based on CCHI (Council of Cooperative Health Insurance) registered insurers
/// </summary>
public class PayerMasterSeeder
{
    private readonly ApplicationDbContext _context;
    private readonly ILogger<PayerMasterSeeder> _logger;

    public PayerMasterSeeder(ApplicationDbContext context, ILogger<PayerMasterSeeder> logger)
    {
   _context = context ?? throw new ArgumentNullException(nameof(context));
   _logger = logger ?? throw new ArgumentNullException(nameof(logger));
 }

    /// <summary>
    /// Seed major Saudi insurance companies
    /// </summary>
    public async Task SeedPayersAsync()
    {
        _logger.LogInformation("Starting payer master data seeding...");

 var existingCount = await _context.PayerMasters.CountAsync();
        if (existingCount > 0)
        {
  _logger.LogInformation("Payers already seeded. Count: {Count}", existingCount);
          return;
     }

    var payers = GetSaudiInsuranceCompanies();
    
    await _context.PayerMasters.AddRangeAsync(payers);
    await _context.SaveChangesAsync();

   _logger.LogInformation("? Seeded {Count} insurance companies", payers.Count);
    }

/// <summary>
    /// Get major Saudi insurance companies
    /// </summary>
    private List<PayerMaster> GetSaudiInsuranceCompanies()
    {
        return new List<PayerMaster>
        {
  // Bupa Arabia
            new PayerMaster
            {
                PayerId = "INS-7001",
     PayerName = "Bupa Arabia for Cooperative Insurance Company",
                PayerNameArabic = "???? ??????? ??????? ????????",
      PayerType = "Insurance",
                LicenseNumber = "7001",
                NphiesPayerId = "7001",
     NphiesConnectionStatus = "Active",
 NphiesApiEndpoint = "https://api.nphies.sa/bupa",
     IsNphiesMember = true,
    ContactEmail = "info@bupaarabia.com",
          ContactPhone = "+966-11-4190909",
           Website = "https://www.bupaarabia.com",
    AddressLine1 = "King Fahd Road",
      City = "Riyadh",
                Country = "SA",
    PostalCode = "12271",
     ContractStartDate = new DateTime(2020, 1, 1),
    IsActive = true,
    IsNphiesIntegrated = true,
  SupportedClaimTypes = "institutional,professional,pharmacy,vision,dental",
      SupportedEligibilityTypes = "standard,preauthorization",
           PaymentCycle = "Monthly",
                AverageTurnaroundDays = 15,
           MaxClaimsPerDay = 1000,
    MaxClaimAmount = 500000m,
  CurrencyCode = "SAR",
         Notes = "One of the largest health insurance providers in Saudi Arabia",
           CreatedBy = "System",
         ModifiedBy = "System"
       },

            // Tawuniya
     new PayerMaster
          {
              PayerId = "INS-7002",
         PayerName = "The Company for Cooperative Insurance",
            PayerNameArabic = "?????? ????????? ??????? - ?????????",
PayerType = "Insurance",
       LicenseNumber = "7002",
        NphiesPayerId = "7002",
    NphiesConnectionStatus = "Active",
            NphiesApiEndpoint = "https://api.nphies.sa/tawuniya",
       IsNphiesMember = true,
         ContactEmail = "info@tawuniya.com.sa",
 ContactPhone = "+966-11-2187000",
     Website = "https://www.tawuniya.com.sa",
              AddressLine1 = "Al Olaya",
    City = "Riyadh",
          Country = "SA",
           PostalCode = "11411",
                ContractStartDate = new DateTime(2020, 1, 1),
      IsActive = true,
      IsNphiesIntegrated = true,
             SupportedClaimTypes = "institutional,professional,pharmacy,vision,dental",
            SupportedEligibilityTypes = "standard,preauthorization",
        PaymentCycle = "Bi-Weekly",
        AverageTurnaroundDays = 10,
             MaxClaimsPerDay = 1500,
        MaxClaimAmount = 750000m,
      CurrencyCode = "SAR",
           Notes = "Leading insurance company with extensive network",
       CreatedBy = "System",
        ModifiedBy = "System"
            },

          // MedGulf
      new PayerMaster
            {
        PayerId = "INS-7003",
     PayerName = "Mediterranean & Gulf Insurance",
        PayerNameArabic = "???? ?????? ??????? ???????? ?????? ?????? ???????",
   PayerType = "Insurance",
   LicenseNumber = "7003",
           NphiesPayerId = "7003",
             NphiesConnectionStatus = "Active",
       NphiesApiEndpoint = "https://api.nphies.sa/medgulf",
         IsNphiesMember = true,
      ContactEmail = "info@medgulfins.com",
          ContactPhone = "+966-11-2182222",
                Website = "https://www.medgulfins.com",
            AddressLine1 = "King Fahd Road",
     City = "Riyadh",
                Country = "SA",
     PostalCode = "11564",
  ContractStartDate = new DateTime(2020, 1, 1),
     IsActive = true,
          IsNphiesIntegrated = true,
                SupportedClaimTypes = "institutional,professional,pharmacy,vision",
    SupportedEligibilityTypes = "standard",
                PaymentCycle = "Monthly",
     AverageTurnaroundDays = 12,
     MaxClaimsPerDay = 800,
            MaxClaimAmount = 400000m,
     CurrencyCode = "SAR",
      Notes = "Well-established provider with competitive rates",
        CreatedBy = "System",
     ModifiedBy = "System"
 },

          // Allianz
        new PayerMaster
    {
      PayerId = "INS-7004",
                PayerName = "Allianz Saudi Fransi Cooperative Insurance Company",
              PayerNameArabic = "?????? ??????? ??????? ??????? ????????",
            PayerType = "Insurance",
          LicenseNumber = "7004",
   NphiesPayerId = "7004",
    NphiesConnectionStatus = "Active",
    NphiesApiEndpoint = "https://api.nphies.sa/allianz",
          IsNphiesMember = true,
    ContactEmail = "info@allianz.sa",
                ContactPhone = "+966-11-2185555",
       Website = "https://www.allianz.sa",
             AddressLine1 = "King Fahad Road",
 City = "Riyadh",
      Country = "SA",
     PostalCode = "11432",
  ContractStartDate = new DateTime(2020, 1, 1),
       IsActive = true,
    IsNphiesIntegrated = true,
                SupportedClaimTypes = "institutional,professional,pharmacy,vision,dental",
    SupportedEligibilityTypes = "standard,preauthorization",
   PaymentCycle = "Monthly",
                AverageTurnaroundDays = 14,
        MaxClaimsPerDay = 1200,
                MaxClaimAmount = 600000m,
      CurrencyCode = "SAR",
         Notes = "International insurance provider with local presence",
       CreatedBy = "System",
                ModifiedBy = "System"
       },

    // AXA
        new PayerMaster
         {
  PayerId = "INS-7005",
      PayerName = "AXA Cooperative Insurance Company",
                PayerNameArabic = "???? ??????? ????????",
     PayerType = "Insurance",
   LicenseNumber = "7005",
                NphiesPayerId = "7005",
     NphiesConnectionStatus = "Active",
     NphiesApiEndpoint = "https://api.nphies.sa/axa",
          IsNphiesMember = true,
         ContactEmail = "customerservice@axa-gulf.com",
 ContactPhone = "+966-11-2900000",
           Website = "https://www.axa-gulf.com/sa",
   AddressLine1 = "Al Mousa Tower, King Fahad Road",
     City = "Riyadh",
   Country = "SA",
  PostalCode = "11576",
             ContractStartDate = new DateTime(2020, 1, 1),
             IsActive = true,
 IsNphiesIntegrated = true,
      SupportedClaimTypes = "institutional,professional,pharmacy",
            SupportedEligibilityTypes = "standard",
     PaymentCycle = "Monthly",
   AverageTurnaroundDays = 15,
                MaxClaimsPerDay = 900,
     MaxClaimAmount = 450000m,
                CurrencyCode = "SAR",
              Notes = "Global insurance leader serving Saudi market",
   CreatedBy = "System",
    ModifiedBy = "System"
       },

        // Malath Insurance
     new PayerMaster
      {
        PayerId = "INS-7006",
    PayerName = "Malath Cooperative Insurance Company",
 PayerNameArabic = "???? ???? ?????????",
       PayerType = "Insurance",
  LicenseNumber = "7006",
   NphiesPayerId = "7006",
       NphiesConnectionStatus = "Active",
NphiesApiEndpoint = "https://api.nphies.sa/malath",
          IsNphiesMember = true,
    ContactEmail = "info@malath.com.sa",
       ContactPhone = "+966-11-2065555",
      Website = "https://www.malath.com.sa",
    AddressLine1 = "Olaya Street",
        City = "Riyadh",
   Country = "SA",
             PostalCode = "12211",
    ContractStartDate = new DateTime(2020, 1, 1),
         IsActive = true,
          IsNphiesIntegrated = true,
       SupportedClaimTypes = "institutional,professional,pharmacy,vision",
     SupportedEligibilityTypes = "standard,preauthorization",
      PaymentCycle = "Bi-Weekly",
           AverageTurnaroundDays = 10,
       MaxClaimsPerDay = 700,
                MaxClaimAmount = 350000m,
    CurrencyCode = "SAR",
  Notes = "Specialized in health and medical insurance",
 CreatedBy = "System",
  ModifiedBy = "System"
            },

            // SAICO
       new PayerMaster
  {
    PayerId = "INS-7007",
       PayerName = "Saudi Arabian Insurance Company (SAICO)",
        PayerNameArabic = "?????? ???????? ??????? ???????",
    PayerType = "Insurance",
         LicenseNumber = "7007",
            NphiesPayerId = "7007",
                NphiesConnectionStatus = "Active",
             NphiesApiEndpoint = "https://api.nphies.sa/saico",
    IsNphiesMember = true,
    ContactEmail = "info@saico.com.sa",
        ContactPhone = "+966-11-4777777",
                Website = "https://www.saico.com.sa",
            AddressLine1 = "Al Takhassousi Street",
     City = "Riyadh",
              Country = "SA",
        PostalCode = "11564",
   ContractStartDate = new DateTime(2020, 1, 1),
           IsActive = true,
      IsNphiesIntegrated = true,
      SupportedClaimTypes = "institutional,professional,pharmacy",
       SupportedEligibilityTypes = "standard",
     PaymentCycle = "Monthly",
       AverageTurnaroundDays = 12,
          MaxClaimsPerDay = 600,
                MaxClaimAmount = 300000m,
   CurrencyCode = "SAR",
    Notes = "One of the oldest insurance companies in Saudi Arabia",
          CreatedBy = "System",
      ModifiedBy = "System"
            },

            // Wataniya Insurance
            new PayerMaster
            {
    PayerId = "INS-7008",
      PayerName = "Al Watania Insurance Company",
         PayerNameArabic = "???? ??????? ???????",
                PayerType = "Insurance",
      LicenseNumber = "7008",
         NphiesPayerId = "7008",
       NphiesConnectionStatus = "Active",
NphiesApiEndpoint = "https://api.nphies.sa/watania",
 IsNphiesMember = true,
                ContactEmail = "info@watania.com",
        ContactPhone = "+966-11-2183838",
 Website = "https://www.watania.com",
        AddressLine1 = "King Fahd Road",
                City = "Riyadh",
      Country = "SA",
          PostalCode = "11564",
          ContractStartDate = new DateTime(2020, 1, 1),
IsActive = true,
   IsNphiesIntegrated = true,
SupportedClaimTypes = "institutional,professional,pharmacy,vision,dental",
 SupportedEligibilityTypes = "standard,preauthorization",
            PaymentCycle = "Monthly",
        AverageTurnaroundDays = 14,
      MaxClaimsPerDay = 1000,
                MaxClaimAmount = 500000m,
    CurrencyCode = "SAR",
                Notes = "Comprehensive health insurance solutions",
   CreatedBy = "System",
    ModifiedBy = "System"
   },

        // Salama Insurance
       new PayerMaster
  {
      PayerId = "INS-7009",
         PayerName = "Islamic Arab Insurance Company (Salama)",
     PayerNameArabic = "?????? ????????? ??????? ??????? - ?????",
          PayerType = "Insurance",
            LicenseNumber = "7009",
 NphiesPayerId = "7009",
      NphiesConnectionStatus = "Active",
         NphiesApiEndpoint = "https://api.nphies.sa/salama",
  IsNphiesMember = true,
     ContactEmail = "info@salama.com.sa",
 ContactPhone = "+966-11-4605666",
     Website = "https://www.salama.com.sa",
       AddressLine1 = "Olaya District",
                City = "Riyadh",
      Country = "SA",
    PostalCode = "12214",
         ContractStartDate = new DateTime(2020, 1, 1),
  IsActive = true,
         IsNphiesIntegrated = true,
 SupportedClaimTypes = "institutional,professional,pharmacy",
                SupportedEligibilityTypes = "standard",
    PaymentCycle = "Monthly",
        AverageTurnaroundDays = 15,
      MaxClaimsPerDay = 800,
     MaxClaimAmount = 400000m,
                CurrencyCode = "SAR",
         Notes = "Sharia-compliant insurance solutions",
    CreatedBy = "System",
         ModifiedBy = "System"
    },

         // MEDNET
  new PayerMaster
          {
      PayerId = "INS-7010",
      PayerName = "MEDNET (TPA)",
             PayerNameArabic = "????? - ???? ??????? ??????",
         PayerType = "TPA",
    LicenseNumber = "7010",
           NphiesPayerId = "7010",
          NphiesConnectionStatus = "Active",
   NphiesApiEndpoint = "https://api.nphies.sa/mednet",
    IsNphiesMember = true,
ContactEmail = "info@mednet.com.sa",
   ContactPhone = "+966-11-2303030",
      Website = "https://www.mednet.com.sa",
      AddressLine1 = "Al Olaya",
            City = "Riyadh",
        Country = "SA",
       PostalCode = "11311",
      ContractStartDate = new DateTime(2020, 1, 1),
 IsActive = true,
  IsNphiesIntegrated = true,
       SupportedClaimTypes = "institutional,professional,pharmacy,vision,dental",
 SupportedEligibilityTypes = "standard,preauthorization",
           PaymentCycle = "Bi-Weekly",
     AverageTurnaroundDays = 7,
     MaxClaimsPerDay = 2000,
  MaxClaimAmount = 1000000m,
 CurrencyCode = "SAR",
                Notes = "Third-party administrator with fast processing",
            CreatedBy = "System",
         ModifiedBy = "System"
 }
      };
    }

    /// <summary>
    /// Seed sample policy data for testing
    /// </summary>
    public async Task SeedSamplePoliciesAsync()
    {
        _logger.LogInformation("Starting sample policy seeding...");

        var existingCount = await _context.PayerPolicyMasters.CountAsync();
        if (existingCount > 0)
        {
     _logger.LogInformation("Policies already seeded. Count: {Count}", existingCount);
            return;
        }

        // Get Bupa as example payer
        var bupa = await _context.PayerMasters.FirstOrDefaultAsync(p => p.PayerId == "INS-7001");
 if (bupa == null)
        {
        _logger.LogWarning("Bupa payer not found. Seed payers first.");
            return;
  }

        var policies = new List<PayerPolicyMaster>
      {
            new PayerPolicyMaster
   {
      PayerMasterId = bupa.Id,
       PolicyCode = "BUPA-ESSENTIAL",
         PolicyName = "Bupa Essential Plan",
                PolicyNameArabic = "??? ???? ????????",
          PolicyDescription = "Essential health coverage with comprehensive benefits",
           PolicyType = "Individual",
           CoverageType = "Comprehensive",
                CoverageLevel = "Comprehensive",
   NetworkType = "PPO",
                AnnualPremium = 3500m,
 PremiumAmount = 3500m,
         PremiumFrequency = "Annual",
      CurrencyCode = "SAR",
            AnnualDeductible = 500m,
     Copay = 50m,
  CopaymentAmount = 50m,
  CoinsurancePercentage = 20m,
   MaxOutOfPocket = 5000m,
         OutOfPocketMax = 5000m,
    EffectiveFromDate = new DateTime(2024, 1, 1),
      EffectiveToDate = new DateTime(2024, 12, 31),
       PolicyStartDate = new DateTime(2024, 1, 1),
    PolicyEndDate = new DateTime(2024, 12, 31),
    IsPolicyActive = true,
RequiresPriorAuth = true,
       PreAuthRequiredForAmount = 10000m,
                Notes = "Popular plan for individuals and families",
     CreatedBy = "System",
  ModifiedBy = "System"
      },
       new PayerPolicyMaster
   {
       PayerMasterId = bupa.Id,
    PolicyCode = "BUPA-PREMIUM",
      PolicyName = "Bupa Premium Plan",
     PolicyNameArabic = "??? ???? ???????",
          PolicyDescription = "Premium health coverage with minimal restrictions",
    PolicyType = "Corporate",
         CoverageType = "Platinum",
 CoverageLevel = "Platinum",
   NetworkType = "PPO",
  AnnualPremium = 7500m,
    PremiumAmount = 7500m,
      PremiumFrequency = "Annual",
        CurrencyCode = "SAR",
         AnnualDeductible = 0m,
        Copay = 0m,
 CopaymentAmount = 0m,
     CoinsurancePercentage = 10m,
       MaxOutOfPocket = 2000m,
     OutOfPocketMax = 2000m,
   EffectiveFromDate = new DateTime(2024, 1, 1),
  EffectiveToDate = new DateTime(2024, 12, 31),
              PolicyStartDate = new DateTime(2024, 1, 1),
             PolicyEndDate = new DateTime(2024, 12, 31),
  IsPolicyActive = true,
           RequiresPriorAuth = false,
            PreAuthRequiredForAmount = 50000m,
      Notes = "Executive plan with comprehensive coverage",
      CreatedBy = "System",
     ModifiedBy = "System"
       }
    };

        await _context.PayerPolicyMasters.AddRangeAsync(policies);
        await _context.SaveChangesAsync();

    _logger.LogInformation("? Seeded {Count} sample policies", policies.Count);
    }
}
