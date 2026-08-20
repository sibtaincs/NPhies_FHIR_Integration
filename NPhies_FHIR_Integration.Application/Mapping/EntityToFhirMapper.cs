using Hl7.Fhir.Model;
using Microsoft.Extensions.Logging;
using DomainPatient = NPhies_FHIR_Integration.Domain.Entities.Patient;
using DomainOrganization = NPhies_FHIR_Integration.Domain.Entities.Organization;
using DomainCoverage = NPhies_FHIR_Integration.Domain.Entities.Coverage;
using DomainClaim = NPhies_FHIR_Integration.Domain.Entities.Claim;
using DomainPractitioner = NPhies_FHIR_Integration.Domain.Entities.Practitioner;
using DomainEncounter = NPhies_FHIR_Integration.Domain.Entities.Encounter;
using DomainCoverageEligibilityRequest = NPhies_FHIR_Integration.Domain.Entities.CoverageEligibilityRequest;
using FhirPatient = Hl7.Fhir.Model.Patient;
using FhirOrganization = Hl7.Fhir.Model.Organization;
using FhirCoverage = Hl7.Fhir.Model.Coverage;
using FhirClaim = Hl7.Fhir.Model.Claim;
using FhirPractitioner = Hl7.Fhir.Model.Practitioner;
using FhirEncounter = Hl7.Fhir.Model.Encounter;

namespace NPhies_FHIR_Integration.Application.Mapping;

/// <summary>
/// Maps domain entities to FHIR R4 resources for NPHIES integration
/// </summary>
public interface IEntityToFhirMapper
{
    // Basic Resources
    FhirPatient MapToFhirPatient(DomainPatient patient);
    FhirCoverage MapToFhirCoverage(DomainCoverage coverage);
    FhirOrganization MapToFhirOrganization(DomainOrganization organization);
    FhirPractitioner MapToFhirPractitioner(DomainPractitioner practitioner);
    FhirEncounter MapToFhirEncounter(DomainEncounter encounter);
    MessageHeader CreateMessageHeader(string eventCode, DomainOrganization source);

    // Request Resources
    CoverageEligibilityRequest MapToFhirEligibilityRequest(
        DomainCoverageEligibilityRequest request,
        string patientRef,
        string coverageRef,
        string providerRef,
        string insurerRef);

    FhirClaim MapToFhirClaim(
    DomainClaim claim,
        string patientRef,
        string providerRef,
        string insurerRef,
        string coverageRef,
        string? encounterRef = null);

    // Extensions & Identifiers
    Extension CreateNphiesExtension(string url, DataType value);
    Identifier CreateNphiesIdentifier(string system, string value);
}

/// <summary>
/// Implementation of Entity to FHIR mapper
/// </summary>
public class EntityToFhirMapper : IEntityToFhirMapper
{
    private readonly ILogger<EntityToFhirMapper> _logger;

    public EntityToFhirMapper(ILogger<EntityToFhirMapper> logger)
    {
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
    }

    /// <summary>
    /// Map Patient entity to FHIR Patient resource
    /// </summary>
    public FhirPatient MapToFhirPatient(DomainPatient patient)
    {
        _logger.LogDebug("Mapping patient {PatientId} to FHIR resource", patient.Id);

        var fhirPatient = new FhirPatient
        {
            Id = patient.Id,
            Identifier = new List<Identifier>
            {
         new Identifier
      {
             System = "urn:oid:2.16.840.1.113883.3.571.1.1", // National ID
   Value = patient.NationalId
      },
       new Identifier
       {
               System = "http://nphies.sa/identifier/member-id", // Member ID
      Value = patient.MRN
        }
       },
            Name = new List<HumanName>
     {
                new HumanName
  {
        Family = patient.LastName,
        Given = new[] { patient.FirstName },
         Text = patient.FullName
    }
        },
            BirthDate = patient.DateOfBirth.ToString("yyyy-MM-dd"),
            Gender = patient.Gender?.ToUpper() switch
            {
                "M" => AdministrativeGender.Male,
                "F" => AdministrativeGender.Female,
                "O" => AdministrativeGender.Other,
                _ => AdministrativeGender.Unknown
            },
            Active = patient.IsActive,
            Meta = new Meta
            {
                Profile = new[] { "http://nphies.sa/fhir/StructureDefinition/Patient" }
            }
        };

        // Add telecom if available
        var telecom = new List<ContactPoint>();
        if (!string.IsNullOrWhiteSpace(patient.Phone))
        {
            telecom.Add(new ContactPoint
            {
                System = ContactPoint.ContactPointSystem.Phone,
                Value = patient.Phone,
                Use = ContactPoint.ContactPointUse.Mobile
            });
        }
        if (!string.IsNullOrWhiteSpace(patient.Email))
        {
            telecom.Add(new ContactPoint
            {
                System = ContactPoint.ContactPointSystem.Email,
                Value = patient.Email
            });
        }
        if (telecom.Any())
            fhirPatient.Telecom = telecom;

        // Add address if available
        if (!string.IsNullOrWhiteSpace(patient.City))
        {
            fhirPatient.Address = new List<Address>
   {
       new Address
                {
             Line = new[] { patient.AddressLine1, patient.AddressLine2 }
      .Where(l => !string.IsNullOrWhiteSpace(l))
   .ToArray(),
    City = patient.City,
           State = patient.State,
          PostalCode = patient.PostalCode,
          Country = patient.Country ?? "SA"
       }
            };
        }

        return fhirPatient;
    }

    /// <summary>
    /// Map Coverage entity to FHIR Coverage resource
    /// </summary>
    public FhirCoverage MapToFhirCoverage(DomainCoverage coverage)
    {
        _logger.LogDebug("Mapping coverage {CoverageId} to FHIR resource", coverage.Id);

        var fhirCoverage = new FhirCoverage
        {
Id = coverage.Id,
            Identifier = new List<Identifier>
            {
    new Identifier
     {
          System = "http://nphies.sa/identifier/policy-number",
      Value = coverage.PolicyNumber
       },
        new Identifier
        {
     System = "http://nphies.sa/license/payer-member-id",
    Value = coverage.MemberID
       }
         },
     Status = FinancialResourceStatusCodes.Active,
    Type = new CodeableConcept
     {
          Coding = new List<Coding>
      {
    new Coding
               {
   System = "http://terminology.hl7.org/CodeSystem/v3-ActCode",
  Code = coverage.CoverageType ?? "EHCPOL",
Display = "Extended Healthcare"
        }
     }
     },
            Beneficiary = new ResourceReference($"Patient/{coverage.PatientId}"),
   Subscriber = new ResourceReference($"Patient/{coverage.SubscriberPatientId ?? coverage.PatientId}"),
  Relationship = new CodeableConcept
    {
      Coding = new List<Coding>
       {
       new Coding
     {
      System = "http://terminology.hl7.org/CodeSystem/subscriber-relationship",
     Code = coverage.SubscriberRelationship?.ToLower() ?? "self",
             Display = coverage.SubscriberRelationship ?? "Self"
  }
    }
         },
          Period = new Period
      {
             Start = coverage.CoverageStartDate.ToString("yyyy-MM-dd"),
          End = coverage.CoverageEndDate.ToString("yyyy-MM-dd") // Not nullable in entity
       },
          Payor = new List<ResourceReference>
         {
        new ResourceReference($"Organization/{coverage.InsurerId}") // Use InsurerId
  },
         Meta = new Meta
          {
      Profile = new[] { "http://nphies.sa/fhir/StructureDefinition/Coverage" }
            }
        };

        return fhirCoverage;
    }

    /// <summary>
    /// Map Organization entity to FHIR Organization resource
    /// </summary>
    public FhirOrganization MapToFhirOrganization(DomainOrganization organization)
    {
        _logger.LogDebug("Mapping organization {OrgId} to FHIR resource", organization.Id);

        var isProvider = organization.OrganizationType?.ToLower() == "provider" ||
  organization.OrganizationType?.ToLower() == "prov";

        var identifierSystem = isProvider
        ? "http://nphies.sa/license/provider-license"
             : "http://nphies.sa/license/payer-license";

        var fhirOrg = new FhirOrganization
        {
            Id = organization.Id,
            Identifier = new List<Identifier>
  {
    new Identifier
           {
     System = identifierSystem,
    Value = organization.LicenseNumber
        }
    },
            Name = organization.OrganizationName,
            Active = organization.Status?.ToLower() == "active",
            Type = new List<CodeableConcept>
      {
  new CodeableConcept
       {
  Coding = new List<Coding>
      {
      new Coding
          {
    System = "http://terminology.hl7.org/CodeSystem/organization-type",
        Code = isProvider ? "prov" : "ins",
       Display = isProvider ? "Healthcare Provider" : "Insurance Company"
         }
  }
      }
 },
            Meta = new Meta
            {
                Profile = new[] { "http://nphies.sa/fhir/StructureDefinition/Organization" }
            }
        };

        // Add telecom if available
        var telecom = new List<ContactPoint>();
        if (!string.IsNullOrWhiteSpace(organization.PhoneNumber))
        {
            telecom.Add(new ContactPoint
            {
                System = ContactPoint.ContactPointSystem.Phone,
                Value = organization.PhoneNumber
            });
        }
        if (!string.IsNullOrWhiteSpace(organization.Email))
        {
            telecom.Add(new ContactPoint
            {
                System = ContactPoint.ContactPointSystem.Email,
                Value = organization.Email
            });
        }
        if (telecom.Any())
            fhirOrg.Telecom = telecom;

        return fhirOrg;
    }

    /// <summary>
    /// Map Practitioner entity to FHIR Practitioner resource
    /// </summary>
    public FhirPractitioner MapToFhirPractitioner(DomainPractitioner practitioner)
    {
        _logger.LogDebug("Mapping practitioner {PractitionerId} to FHIR resource", practitioner.Id);

        return new FhirPractitioner
        {
            Id = practitioner.Id,
            Identifier = new List<Identifier>
      {
     new Identifier
    {
   System = "http://nphies.sa/license/practitioner-license",
             Value = practitioner.LicenseNumber
                }
   },
            Name = new List<HumanName>
       {
 new HumanName
             {
      Family = practitioner.LastName,
    Given = new[] { practitioner.FirstName },
 Text = $"{practitioner.FirstName} {practitioner.LastName}"
      }
            },
            Active = practitioner.Status?.ToLower() == "active",
            Qualification = new List<Practitioner.QualificationComponent>
    {
                new Practitioner.QualificationComponent
                {
       Code = new CodeableConcept
               {
            Coding = new List<Coding>
                 {
         new Coding
       {
         System = "http://nphies.sa/terminology/CodeSystem/practice-codes",
     Code = practitioner.Specialization,
    Display = practitioner.Specialization
           }
  }
     }
        }
        },
            Meta = new Meta
            {
                Profile = new[] { "http://nphies.sa/fhir/StructureDefinition/Practitioner" }
            }
        };
    }

    /// <summary>
    /// Map Encounter entity to FHIR Encounter resource
    /// </summary>
    public FhirEncounter MapToFhirEncounter(DomainEncounter encounter)
    {
        _logger.LogDebug("Mapping encounter {EncounterId} to FHIR resource", encounter.Id);

        var fhirEncounter = new FhirEncounter
        {
            Id = encounter.Id,
            Identifier = new List<Identifier>
      {
         new Identifier
        {
 System = "http://nphies.sa/identifier/encounter-id",
   Value = encounter.Id // Use Id since EncounterNumber doesn't exist
        }
      },
    Status = Encounter.EncounterStatus.Finished,
    Class = new Coding
        {
       System = "http://terminology.hl7.org/CodeSystem/v3-ActCode",
 Code = "AMB" // Default to ambulatory
 },
        Subject = new ResourceReference($"Patient/{encounter.PatientId}"),
    Period = new Period
            {
         Start = encounter.CreatedAt.ToString("yyyy-MM-ddTHH:mm:ssZ"),
   End = encounter.CreatedAt.ToString("yyyy-MM-ddTHH:mm:ssZ") // Use CreatedAt as fallback
  },
      Meta = new Meta
      {
  Profile = new[] { "http://nphies.sa/fhir/StructureDefinition/Encounter" }
        }
        };

        return fhirEncounter;
    }

    /// <summary>
    /// Create MessageHeader for NPHIES bundle
    /// </summary>
    public MessageHeader CreateMessageHeader(string eventCode, DomainOrganization source)
    {
        _logger.LogDebug("Creating MessageHeader with event code {EventCode}", eventCode);

        return new MessageHeader
   {
 Id = Guid.NewGuid().ToString(),
    Event = new Coding // Use Event not EventCoding for FHIR R4
        {
  System = "http://nphies.sa/terminology/CodeSystem/ksa-message-events",
   Code = eventCode
     },
      Source = new MessageHeader.MessageSourceComponent
    {
       Name = source.OrganizationName,
             Endpoint = source.LicenseNumber // Using license as endpoint identifier
            },
 Destination = new List<MessageHeader.MessageDestinationComponent>
  {
  new MessageHeader.MessageDestinationComponent
          {
   Name = "NPHIES",
   Endpoint = "http://nphies.sa/fhir"
    }
        },
            Meta = new Meta
    {
     Profile = new[] { "http://nphies.sa/fhir/StructureDefinition/MessageHeader" }
 }
        };
    }

    /// <summary>
    /// Map CoverageEligibilityRequest entity to FHIR resource
    /// </summary>
    public CoverageEligibilityRequest MapToFhirEligibilityRequest(
        DomainCoverageEligibilityRequest request,
      string patientRef,
        string coverageRef,
  string providerRef,
        string insurerRef)
    {
        _logger.LogDebug("Mapping eligibility request {RequestId} to FHIR resource", request.RequestId);

        return new CoverageEligibilityRequest
        {
            Id = request.Id,
            Identifier = new List<Identifier>
            {
            new Identifier
         {
        System = "http://nphies.sa/identifier/request-id",
     Value = request.RequestId
       }
   },
  Status = FinancialResourceStatusCodes.Active,
     Priority = new CodeableConcept
     {
 Coding = new List<Coding>
     {
   new Coding
     {
  System = "http://terminology.hl7.org/CodeSystem/processpriority",
  Code = request.Priority?.ToLower() ?? "normal",
      Display = request.Priority ?? "Normal"
    }
         }
      },
       Purpose = new List<CoverageEligibilityRequest.EligibilityRequestPurpose?>
      {
 CoverageEligibilityRequest.EligibilityRequestPurpose.Discovery,
    CoverageEligibilityRequest.EligibilityRequestPurpose.Benefits,
      CoverageEligibilityRequest.EligibilityRequestPurpose.Validation
            },
         Patient = new ResourceReference(patientRef),
          Serviced = new FhirDateTime(request.ServiceDate.ToString("yyyy-MM-dd")), // Use Serviced property
   Created = DateTime.UtcNow.ToString("yyyy-MM-ddTHH:mm:ssZ"),
            Enterer = new ResourceReference(providerRef),
            Provider = new ResourceReference(providerRef),
   Insurer = new ResourceReference(insurerRef),
      Insurance = new List<CoverageEligibilityRequest.InsuranceComponent>
   {
 new CoverageEligibilityRequest.InsuranceComponent
    {
    Coverage = new ResourceReference(coverageRef),
   Focal = true
          }
    },
        Meta = new Meta
     {
                Profile = new[] { "http://nphies.sa/fhir/StructureDefinition/CoverageEligibilityRequest" }
}
        };
    }

    /// <summary>
    /// Map Claim entity to FHIR Claim resource
    /// </summary>
    public FhirClaim MapToFhirClaim(
        DomainClaim claim,
     string patientRef,
        string providerRef,
        string insurerRef,
      string coverageRef,
        string? encounterRef = null)
    {
        _logger.LogDebug("Mapping claim {ClaimNumber} to FHIR resource", claim.ClaimNumber);

    var fhirClaim = new FhirClaim
   {
    Id = claim.Id,
   Identifier = new List<Identifier>
     {
         new Identifier
        {
     System = "http://nphies.sa/identifier/claim-id",
Value = claim.ClaimNumber
   }
     },
   Status = FinancialResourceStatusCodes.Active,
        Type = new CodeableConcept
 {
    Coding = new List<Coding>
  {
             new Coding
      {
        System = "http://terminology.hl7.org/CodeSystem/claim-type",
 Code = claim.ClaimType?.ToLower() ?? "institutional",
  Display = claim.ClaimType ?? "Institutional"
  }
          }
          },
        SubType = new CodeableConcept
   {
   Coding = new List<Coding>
        {
    new Coding
  {
System = "http://nphies.sa/terminology/CodeSystem/claim-subtype",
Code = claim.ClaimSubType?.ToLower() ?? "ip",
             Display = claim.ClaimSubType ?? "Inpatient"
     }
    }
            },
     Use = claim.Use?.ToLower() == "preauthorization" 
      ? ClaimUseCode.Preauthorization 
  : ClaimUseCode.Claim,
    Patient = new ResourceReference(patientRef),
  BillablePeriod = new Period
  {
       Start = claim.CreatedAt.ToString("yyyy-MM-dd"), // Use CreatedAt as fallback
   End = claim.CreatedAt.ToString("yyyy-MM-dd")
            },
  Created = DateTime.UtcNow.ToString("yyyy-MM-ddTHH:mm:ssZ"),
Provider = new ResourceReference(providerRef),
          Insurer = new ResourceReference(insurerRef),
         Priority = new CodeableConcept
   {
       Coding = new List<Coding>
          {
     new Coding
 {
         System = "http://terminology.hl7.org/CodeSystem/processpriority",
          Code = claim.Priority?.ToLower() ?? "normal",
 Display = claim.Priority ?? "Normal"
      }
  }
  },
     Insurance = new List<Claim.InsuranceComponent>
         {
         new Claim.InsuranceComponent
         {
      Sequence = 1,
      Focal = true,
      Coverage = new ResourceReference(coverageRef)
     }
       },
   Total = new Money
      {
          Value = claim.Total, // Use Total property
      Currency = Money.Currencies.SAR // Use enum
        },
       Meta = new Meta
  {
        Profile = new[] { "http://nphies.sa/fhir/StructureDefinition/Claim" }
    }
        };

     // Add encounter reference if provided
        if (!string.IsNullOrWhiteSpace(encounterRef))
        {
fhirClaim.Item = new List<Claim.ItemComponent>
  {
   new Claim.ItemComponent
       {
   Sequence = 1,
       ProductOrService = new CodeableConcept
      {
      Text = "Service"
 },
   Encounter = new List<ResourceReference> { new ResourceReference(encounterRef) } // Fixed property name
 }
       };
        }

     return fhirClaim;
    }

    /// <summary>
    /// Create NPHIES-specific extension
    /// </summary>
    public Extension CreateNphiesExtension(string url, DataType value)
    {
        return new Extension
        {
            Url = url,
            Value = value
        };
    }

    /// <summary>
    /// Create NPHIES identifier
    /// </summary>
    public Identifier CreateNphiesIdentifier(string system, string value)
    {
        return new Identifier
        {
            System = system,
            Value = value
        };
    }
}
