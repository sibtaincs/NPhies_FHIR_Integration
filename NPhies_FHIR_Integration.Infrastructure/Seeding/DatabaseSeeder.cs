using NPhies_FHIR_Integration.Domain.Entities;
using NPhies_FHIR_Integration.Infrastructure.Data;

namespace NPhies_FHIR_Integration.Infrastructure.Seeding;

/// <summary>
/// Database seeder for populating test/dummy data
/// </summary>
public class DatabaseSeeder
{
    private readonly ApplicationDbContext _context;

    /// <summary>
    /// Constructor
    /// </summary>
    public DatabaseSeeder(ApplicationDbContext context)
    {
        _context = context ?? throw new ArgumentNullException(nameof(context));
    }

    /// <summary>
    /// Seed all test data to database
    /// </summary>
    public async Task SeedAsync()
    {
        try
        {
            // Only seed if database is empty
if (_context.Patients.Any())
            {
             return;
     }

  // Create organizations first
            var provider = CreateProvider();
       var insurer = CreateInsurer();

         await _context.AddAsync(provider);
      await _context.AddAsync(insurer);
await _context.SaveChangesAsync();

            // Create locations
            var locations = CreateLocations(provider);
 await _context.AddRangeAsync(locations);
            await _context.SaveChangesAsync();

        // Create practitioners
         var practitioners = CreatePractitioners(provider);
      await _context.AddRangeAsync(practitioners);
   await _context.SaveChangesAsync();

            // Create patients
            var patients = CreatePatients();
  await _context.AddRangeAsync(patients);
            await _context.SaveChangesAsync();

     // Create coverages
            var coverages = CreateCoverages(patients, insurer);
   await _context.AddRangeAsync(coverages);
            await _context.SaveChangesAsync();

            // Create message headers
   var messageHeaders = CreateMessageHeaders(provider, insurer);
            await _context.AddRangeAsync(messageHeaders);
       await _context.SaveChangesAsync();

            // Create eligibility requests and responses
            var eligibilityData = CreateEligibilityData(patients, coverages, provider, insurer, messageHeaders);
            await _context.AddRangeAsync(eligibilityData.Requests);
   await _context.SaveChangesAsync();

     await _context.AddRangeAsync(eligibilityData.Responses);
      await _context.SaveChangesAsync();
        }
        catch (Exception ex)
        {
    throw new InvalidOperationException("Error seeding database", ex);
        }
    }

    private Organization CreateProvider()
    {
  return new Organization
        {
            Id = Guid.NewGuid().ToString(),
      OrganizationName = "Al-Noor Medical Center",
            LicenseNumber = "MED-2024-001",
       LicenseSystem = "http://nphies.sa/license/provider-license",
            OrganizationType = "prov",
            SpecializationType = "General Hospital",
   Website = "https://alnoor.med.sa",
            Email = "info@alnoor.med.sa",
            PhoneNumber = "+966112341234",
   AddressLine1 = "123 Medical Street",
      AddressLine2 = "Building A",
            City = "Riyadh",
          State = "Riyadh",
        PostalCode = "11111",
            Status = "active",
            CreatedAt = DateTime.UtcNow,
  IsActive = true
        };
    }

    private Organization CreateInsurer()
    {
        return new Organization
        {
            Id = Guid.NewGuid().ToString(),
  OrganizationName = "Saudi Health Insurance",
   LicenseNumber = "INS-2024-001",
     LicenseSystem = "http://nphies.sa/license/payer-license",
            OrganizationType = "ins",
            Website = "https://insurance.sa",
            Email = "support@insurance.sa",
      PhoneNumber = "+966114441111",
 AddressLine1 = "456 Insurance Boulevard",
            City = "Riyadh",
      State = "Riyadh",
            PostalCode = "11222",
   Status = "active",
    CreatedAt = DateTime.UtcNow,
     IsActive = true
  };
    }

    private List<Location> CreateLocations(Organization provider)
    {
        return new List<Location>
        {
       new Location
  {
 Id = Guid.NewGuid().ToString(),
           LocationName = "Emergency Department",
      LocationLicense = "LOC-ED-001",
      LicenseSystem = "http://nphies.sa/license/location-license",
            OrganizationId = provider.Id,
         FacilityType = "hospital",
    FacilityTypeDescription = "Emergency Department",
      AddressLine1 = "123 Medical Street, Building A",
    City = "Riyadh",
  State = "Riyadh",
     PostalCode = "11111",
    Country = "SA",
      Phone = "+966112341234",
 Email = "ed@alnoor.med.sa",
 Status = "active",
        CreatedAt = DateTime.UtcNow
      },
      new Location
  {
        Id = Guid.NewGuid().ToString(),
       LocationName = "Outpatient Clinic",
       LocationLicense = "LOC-OPC-001",
    LicenseSystem = "http://nphies.sa/license/location-license",
     OrganizationId = provider.Id,
  FacilityType = "clinic",
  FacilityTypeDescription = "Outpatient Clinic",
       AddressLine1 = "123 Medical Street, Building B",
      City = "Riyadh",
  State = "Riyadh",
       PostalCode = "11111",
   Country = "SA",
         Phone = "+966112341235",
      Email = "opc@alnoor.med.sa",
     Status = "active",
     CreatedAt = DateTime.UtcNow
   }
        };
  }

    private List<Practitioner> CreatePractitioners(Organization provider)
    {
    return new List<Practitioner>
    {
        new Practitioner
            {
Id = Guid.NewGuid().ToString(),
 FirstName = "Ahmed",
       LastName = "Al-Rashid",
      LicenseNumber = "DOC-2024-001",
  LicenseSystem = "http://nphies.sa/license/practitioner-license",
        Specialization = "General Practitioner",
  Qualification = "MD, Internal Medicine",
 Title = "Dr",
        Email = "ahmed.rashid@alnoor.med.sa",
          Phone = "+966501234567",
                OrganizationId = provider.Id,
     Status = "active",
  CreatedAt = DateTime.UtcNow
    },
      new Practitioner
       {
     Id = Guid.NewGuid().ToString(),
        FirstName = "Fatima",
     LastName = "Al-Sulaiman",
        LicenseNumber = "DOC-2024-002",
       LicenseSystem = "http://nphies.sa/license/practitioner-license",
   Specialization = "Pediatrics",
             Qualification = "MD, Pediatrics",
     Title = "Dr",
      Email = "fatima.sulaiman@alnoor.med.sa",
     Phone = "+966501234568",
   OrganizationId = provider.Id,
        Status = "active",
    CreatedAt = DateTime.UtcNow
       }
        };
    }

  private List<Patient> CreatePatients()
    {
        return new List<Patient>
        {
       new Patient
   {
 Id = Guid.NewGuid().ToString(),
  MRN = "MRN-2024-001",
                IdentifierSystem = "http://nphies.sa/identifier/member-id",
 NationalId = "1234567890",
     FirstName = "Mohammed",
     LastName = "Al-Mutairi",
      DateOfBirth = new DateTime(1985, 5, 15),
       Gender = "M",
       Email = "mohammed@example.com",
           Phone = "+966541234567",
     AddressLine1 = "123 Main Street",
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
         MRN = "MRN-2024-002",
    IdentifierSystem = "http://nphies.sa/identifier/member-id",
   NationalId = "1234567891",
     FirstName = "Sarah",
                LastName = "Al-Dosari",
           DateOfBirth = new DateTime(1990, 8, 22),
Gender = "F",
  Email = "sarah@example.com",
         Phone = "+966541234568",
     AddressLine1 = "456 Oak Avenue",
       City = "Riyadh",
        State = "Riyadh",
     PostalCode = "11222",
          Country = "SA",
    Status = "active",
                CreatedAt = DateTime.UtcNow,
             IsActive = true
       },
    new Patient
            {
    Id = Guid.NewGuid().ToString(),
     MRN = "MRN-2024-003",
        IdentifierSystem = "http://nphies.sa/identifier/member-id",
                NationalId = "1234567892",
                FirstName = "Ali",
LastName = "Al-Qahtani",
       DateOfBirth = new DateTime(1988, 3, 10),
   Gender = "M",
        Email = "ali@example.com",
      Phone = "+966541234569",
      AddressLine1 = "789 Pine Road",
         City = "Jeddah",
                State = "Makkah",
         PostalCode = "21111",
      Country = "SA",
     Status = "active",
       CreatedAt = DateTime.UtcNow,
     IsActive = true
        }
     };
    }

    private List<Coverage> CreateCoverages(List<Patient> patients, Organization insurer)
    {
        return new List<Coverage>
    {
new Coverage
         {
     Id = Guid.NewGuid().ToString(),
     PolicyNumber = "POL-2024-001",
  MemberID = "MEM-2024-001",
         CoverageType = "employee",
   Status = "active",
        PatientId = patients[0].Id,
    InsurerId = insurer.Id,
         CoverageStartDate = new DateTime(2024, 1, 1),
      CoverageEndDate = new DateTime(2024, 12, 31),
       RelationToSubscriber = "Self",
 AnnualDeductible = 500,
    DeductibleMet = 0,
       Copay = 50,
       CoinsurancePercent = 20,
    OutOfPocketMax = 2000,
        CreatedAt = DateTime.UtcNow
    },
            new Coverage
        {
       Id = Guid.NewGuid().ToString(),
        PolicyNumber = "POL-2024-002",
    MemberID = "MEM-2024-002",
            CoverageType = "family",
    Status = "active",
    PatientId = patients[1].Id,
        InsurerId = insurer.Id,
   CoverageStartDate = new DateTime(2024, 1, 1),
        CoverageEndDate = new DateTime(2024, 12, 31),
   RelationToSubscriber = "Self",
  AnnualDeductible = 1000,
  DeductibleMet = 250,
      Copay = 75,
  CoinsurancePercent = 15,
OutOfPocketMax = 3000,
CreatedAt = DateTime.UtcNow
 },
 new Coverage
      {
       Id = Guid.NewGuid().ToString(),
        PolicyNumber = "POL-2024-003",
         MemberID = "MEM-2024-003",
       CoverageType = "dependent",
   Status = "active",
       PatientId = patients[2].Id,
 InsurerId = insurer.Id,
      CoverageStartDate = new DateTime(2024, 1, 1),
    CoverageEndDate = new DateTime(2024, 12, 31),
   RelationToSubscriber = "Spouse",
         AnnualDeductible = 500,
     DeductibleMet = 0,
       Copay = 50,
      CoinsurancePercent = 20,
     OutOfPocketMax = 2000,
     CreatedAt = DateTime.UtcNow
  }
        };
    }

    private List<MessageHeader> CreateMessageHeaders(Organization provider, Organization insurer)
    {
        return new List<MessageHeader>
 {
     new MessageHeader
{
     Id = Guid.NewGuid().ToString(),
     MessageUUID = Guid.NewGuid().ToString(),
       CorrelationId = Guid.NewGuid().ToString(),
   EventCode = "coverage-eligibility-request",
      EventSystem = "http://nphies.sa/events",
      DestinationName = insurer.OrganizationName,
         DestinationEndpoint = "https://insurance.sa/fhir",
                FocusResourceType = "CoverageEligibilityRequest",
        SourceName = provider.OrganizationName,
  SourceEndpoint = "https://alnoor.med.sa/fhir",
Status = "received",
     MessageTimestamp = DateTime.UtcNow,
 CreatedAt = DateTime.UtcNow,
                IsActive = true
   },
       new MessageHeader
            {
        Id = Guid.NewGuid().ToString(),
           MessageUUID = Guid.NewGuid().ToString(),
  CorrelationId = Guid.NewGuid().ToString(),
                EventCode = "coverage-eligibility-response",
    EventSystem = "http://nphies.sa/events",
    DestinationName = provider.OrganizationName,
       DestinationEndpoint = "https://alnoor.med.sa/fhir",
         FocusResourceType = "CoverageEligibilityResponse",
                SourceName = insurer.OrganizationName,
   SourceEndpoint = "https://insurance.sa/fhir",
           Status = "received",
                MessageTimestamp = DateTime.UtcNow.AddSeconds(5),
      CreatedAt = DateTime.UtcNow.AddSeconds(5),
       IsActive = true
          }
    };
    }

  private (List<CoverageEligibilityRequest> Requests, List<CoverageEligibilityResponse> Responses) CreateEligibilityData(
        List<Patient> patients, List<Coverage> coverages, Organization provider, Organization insurer, List<MessageHeader> messageHeaders)
    {
        var requests = new List<CoverageEligibilityRequest>();
        var responses = new List<CoverageEligibilityResponse>();

        // Create request 1
 var request1 = new CoverageEligibilityRequest
{
        Id = Guid.NewGuid().ToString(),
 MessageUUID = Guid.NewGuid().ToString(),
    RequestId = $"REQ-{DateTime.UtcNow:yyyyMMddHHmmss}-001",
    MessageHeaderId = messageHeaders[0].Id,
       RequestType = "eligibility",
        Status = "active",
            Priority = "normal",
            PatientId = patients[0].Id,
        CoverageId = coverages[0].Id,
  ProviderId = provider.Id,
  InsurerId = insurer.Id,
            ServiceType = "medical",
       ServiceDate = DateTime.UtcNow.AddDays(7),
        EligibilityStatus = "pending",
  MessageStatus = "sent",
            RequestCreatedAt = DateTime.UtcNow,
        SubmittedAt = DateTime.UtcNow,
   CreatedAt = DateTime.UtcNow,
     IsActive = true
 };

     // Add items to request
        var items1 = new List<EligibilityItem>
        {
      new EligibilityItem
            {
      Id = Guid.NewGuid().ToString(),
     EligibilityRequestId = request1.Id,
                SequenceNumber = 1,
   Category = "medical",
       CategorySystem = "http://nphies.sa/services",
           CategoryDescription = "Medical Services",
       ProductOrServiceCode = "99213",
    ProductOrServiceSystem = "http://www.ama-assn.org/go/cpt",
        ProductOrServiceDescription = "Office/outpatient visit",
        CreatedAt = DateTime.UtcNow,
      IsActive = true
            }
        };

    request1.Items = items1;
  requests.Add(request1);

     // Create response 1
      var response1 = new CoverageEligibilityResponse
   {
          Id = Guid.NewGuid().ToString(),
   ResponseUUID = Guid.NewGuid().ToString(),
        RequestId = request1.RequestId,
   EligibilityRequestId = request1.Id,
 MessageHeaderId = messageHeaders[1].Id,
            Status = "active",
            Outcome = "complete",
       ProcessingStatus = "complete",
          EligibilityStatus = "active",
  IsInForce = true,
  NetworkStatus = "in-network",
NetworkName = "National Health Network",
    ResponseCreatedAt = DateTime.UtcNow.AddSeconds(5),
   ResponseReceivedAt = DateTime.UtcNow.AddSeconds(10),
            CreatedAt = DateTime.UtcNow.AddSeconds(5),
            IsActive = true
        };

      // Add benefit balances
     var benefitBalance1 = new BenefitBalance
        {
      Id = Guid.NewGuid().ToString(),
         EligibilityResponseId = response1.Id,
    SequenceNumber = 1,
        Category = "medical",
   CategorySystem = "http://nphies.sa/benefits",
     CategoryDescription = "Medical Services",
CreatedAt = DateTime.UtcNow,
            Benefits = new List<Benefit>
     {
    new Benefit
    {
          Id = Guid.NewGuid().ToString(),
         BenefitBalanceId = "",  // Will be set after balance is created
    BenefitType = "copay",
      BenefitTypeSystem = "http://nphies.sa/benefit-type",
          AllowedAmount = 50,
        AllowedCurrency = "SAR",
  UsedAmount = 0,
    CreatedAt = DateTime.UtcNow
     },
  new Benefit
 {
    Id = Guid.NewGuid().ToString(),
      BenefitBalanceId = "",  // Will be set after balance is created
   BenefitType = "coinsurance",
      BenefitTypeSystem = "http://nphies.sa/benefit-type",
        PercentageAmount = 20,
       CreatedAt = DateTime.UtcNow
    }
       }
     };

    // Set benefit balance IDs
 foreach (var benefit in benefitBalance1.Benefits)
        {
    benefit.BenefitBalanceId = benefitBalance1.Id;
      }

    response1.BenefitBalances = new List<BenefitBalance> { benefitBalance1 };
  responses.Add(response1);

        return (requests, responses);
    }
}
