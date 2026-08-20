using Hl7.Fhir.Model;
using Hl7.Fhir.Serialization;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NPhies_FHIR_Integration.Domain.Entities;

// Type aliases to avoid ambiguous references between domain entities and FHIR models
using Task = System.Threading.Tasks.Task;
using DomainPatient = NPhies_FHIR_Integration.Domain.Entities.Patient;
using DomainOrganization = NPhies_FHIR_Integration.Domain.Entities.Organization;
using DomainCoverage = NPhies_FHIR_Integration.Domain.Entities.Coverage;
using DomainClaim = NPhies_FHIR_Integration.Domain.Entities.Claim;
using DomainClaimResponse = NPhies_FHIR_Integration.Domain.Entities.ClaimResponse;
using DomainEncounter = NPhies_FHIR_Integration.Domain.Entities.Encounter;
using DomainCoverageEligibilityRequest = NPhies_FHIR_Integration.Domain.Entities.CoverageEligibilityRequest;
using DomainCoverageEligibilityResponse = NPhies_FHIR_Integration.Domain.Entities.CoverageEligibilityResponse;

using FhirPatient = Hl7.Fhir.Model.Patient;
using FhirOrganization = Hl7.Fhir.Model.Organization;
using FhirCoverage = Hl7.Fhir.Model.Coverage;
using FhirClaim = Hl7.Fhir.Model.Claim;
using FhirClaimResponse = Hl7.Fhir.Model.ClaimResponse;
using FhirEncounter = Hl7.Fhir.Model.Encounter;
using FhirMessageHeader = Hl7.Fhir.Model.MessageHeader;
using FhirCoverageEligibilityRequest = Hl7.Fhir.Model.CoverageEligibilityRequest;
using FhirTask = Hl7.Fhir.Model.Task;

namespace NPhies_FHIR_Integration.Application.Services.FHIR;

/// <summary>
/// FHIR Bundle Service Implementation
/// Handles creation and parsing of NPHIES-compliant FHIR R4 bundles
/// </summary>
public class FhirBundleService : IFhirBundleService
{
    private readonly ILogger<FhirBundleService> _logger;
    private readonly FhirJsonSerializer _serializer;
    private readonly FhirJsonParser _parser;

    public FhirBundleService(ILogger<FhirBundleService> logger)
    {
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));

   // Initialize FHIR serializer and parser with settings
        var serializerSettings = new SerializerSettings
        {
     Pretty = true,
            AppendNewLine = false
        };

      _serializer = new FhirJsonSerializer(serializerSettings);
        _parser = new FhirJsonParser();
    }

    public Task<Bundle> CreateEligibilityRequestBundleAsync(
        DomainCoverageEligibilityRequest request,
        DomainPatient patient,
        DomainCoverage coverage,
    DomainOrganization provider,
        DomainOrganization insurer)
    {
        _logger.LogInformation("Creating eligibility request bundle for patient {PatientId}", patient.Id);
        // Implementation stub - return empty bundle for now
     var bundle = new Bundle
 {
            Type = Bundle.BundleType.Message,
            Timestamp = DateTimeOffset.UtcNow,
   Id = Guid.NewGuid().ToString()
        };
     return Task.FromResult(bundle);
    }

    public Task<DomainCoverageEligibilityResponse> ParseEligibilityResponseAsync(Bundle responseBundle)
    {
     _logger.LogInformation("Parsing eligibility response bundle");
 // Implementation stub
        var response = new DomainCoverageEligibilityResponse
        {
       Id = Guid.NewGuid().ToString(),
            Status = "active",
  Outcome = "complete",
            ResponseCreatedAt = DateTime.UtcNow,
        ResponseReceivedAt = DateTime.UtcNow,
     CreatedAt = DateTime.UtcNow,
   IsActive = true
     };
        return Task.FromResult(response);
    }

  public Task<Bundle> CreateClaimRequestBundleAsync(
   DomainClaim claim,
     DomainPatient patient,
    DomainOrganization provider,
        DomainOrganization insurer,
        DomainCoverage coverage,
        DomainEncounter? encounter = null)
    {
        _logger.LogInformation("Creating claim request bundle for claim {ClaimId}", claim.Id);
        // Implementation stub
        var bundle = new Bundle
  {
         Type = Bundle.BundleType.Message,
            Timestamp = DateTimeOffset.UtcNow,
    Id = Guid.NewGuid().ToString()
     };
        return Task.FromResult(bundle);
    }

    public Task<DomainClaimResponse> ParseClaimResponseAsync(Bundle responseBundle)
    {
     _logger.LogInformation("Parsing claim response bundle");
        // Implementation stub
        var response = new DomainClaimResponse
        {
            Id = Guid.NewGuid().ToString(),
          ClaimResponseStatus = "active",
            CreatedAt = DateTime.UtcNow,
  IsActive = true
        };
  return Task.FromResult(response);
    }

    public Task<Bundle> CreatePreAuthRequestBundleAsync(
        DomainClaim preAuth,
      DomainPatient patient,
      DomainOrganization provider,
     DomainOrganization insurer,
      DomainCoverage coverage,
      DomainEncounter? encounter = null)
    {
        _logger.LogInformation("Creating pre-auth request bundle for claim {ClaimId}", preAuth.Id);
   // Implementation stub
 var bundle = new Bundle
        {
          Type = Bundle.BundleType.Message,
            Timestamp = DateTimeOffset.UtcNow,
            Id = Guid.NewGuid().ToString()
        };
     return Task.FromResult(bundle);
    }

    public Task<DomainClaimResponse> ParsePreAuthResponseAsync(Bundle responseBundle)
    {
      _logger.LogInformation("Parsing pre-auth response bundle");
 // Implementation stub
        var response = new DomainClaimResponse
        {
  Id = Guid.NewGuid().ToString(),
          ClaimResponseStatus = "active",
         Use = "preauthorization",
            CreatedAt = DateTime.UtcNow,
            IsActive = true
    };
        return Task.FromResult(response);
    }

    public Task<Bundle> CreatePollRequestBundleAsync(string taskId, DomainOrganization provider)
    {
        _logger.LogInformation("Creating poll request bundle for task {TaskId}", taskId);
        // Implementation stub
        var bundle = new Bundle
        {
     Type = Bundle.BundleType.Message,
   Timestamp = DateTimeOffset.UtcNow,
            Id = Guid.NewGuid().ToString()
  };
        return Task.FromResult(bundle);
    }

    public Task<string> SerializeToJsonAsync(Bundle bundle)
    {
        _logger.LogDebug("Serializing FHIR bundle to JSON");
var json = _serializer.SerializeToString(bundle);
        _logger.LogDebug("Bundle serialized successfully. Size: {Size} bytes", Encoding.UTF8.GetByteCount(json));
        return Task.FromResult(json);
    }

 public Task<Bundle> DeserializeFromJsonAsync(string json)
    {
        _logger.LogDebug("Deserializing JSON to FHIR bundle");
        var bundle = _parser.Parse<Bundle>(json);
        _logger.LogDebug("Bundle deserialized successfully. Entries: {Count}", bundle.Entry?.Count ?? 0);
    return Task.FromResult(bundle);
    }

    public Task<T?> ParseResourceFromBundleAsync<T>(Bundle bundle) where T : Resource
    {
        var resource = bundle.Entry?
   .Select(e => e.Resource)
            .OfType<T>()
     .FirstOrDefault();

    if (resource == null)
        {
        _logger.LogWarning("Resource of type {ResourceType} not found in bundle", typeof(T).Name);
        }

        return Task.FromResult(resource);
    }

    public Task<List<T>> ExtractResourcesAsync<T>(Bundle bundle) where T : Resource
    {
        var resources = bundle.Entry?
    .Select(e => e.Resource)
       .OfType<T>()
            .ToList() ?? new List<T>();

        _logger.LogDebug("Extracted {Count} resources of type {ResourceType}", resources.Count, typeof(T).Name);
    return Task.FromResult(resources);
    }

    public Task<BundleValidationResult> ValidateBundleAsync(Bundle bundle)
    {
     var result = new BundleValidationResult { IsValid = true };

     if (bundle == null)
   {
            result.Errors.Add("Bundle is null");
      result.IsValid = false;
   return Task.FromResult(result);
        }

  if (bundle.Type != Bundle.BundleType.Message)
        {
            result.Errors.Add($"Bundle type must be 'message', got '{bundle.Type}'");
         result.IsValid = false;
        }

        if (bundle.Entry == null || !bundle.Entry.Any())
        {
          result.Errors.Add("Bundle must contain at least one entry");
     result.IsValid = false;
     }

   // Check for MessageHeader (required by NPHIES)
    var hasMessageHeader = bundle.Entry?.Any(e => e.Resource is FhirMessageHeader) ?? false;
        if (!hasMessageHeader)
        {
    result.Errors.Add("Bundle must contain a MessageHeader resource");
result.IsValid = false;
     }

      // Warnings
        if (bundle.Timestamp == null)
        {
            result.Warnings.Add("Bundle timestamp is recommended");
        }

        return Task.FromResult(result);
    }
}
