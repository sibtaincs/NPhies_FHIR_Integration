using Hl7.Fhir.Model;
using Hl7.Fhir.Serialization;
using Microsoft.Extensions.Logging;
using System.Text;
using NPhies_FHIR_Integration.Domain.Entities;

// Type aliases to avoid ambiguous references between domain entities and FHIR models
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

    // ==========================================
    // Eligibility Bundle Creation
    // ==========================================

    public async System.Threading.Tasks.Task<Bundle> CreateEligibilityRequestBundleAsync(
                            DomainCoverageEligibilityRequest request,
                            DomainPatient patient,
                            DomainCoverage coverage,
                            DomainOrganization provider,
                             DomainOrganization insurer)
    {
        try
        {
            _logger.LogInformation("Creating eligibility request bundle for patient {PatientId}", patient.Id);

            var bundle = new Bundle
            {
                Type = Bundle.BundleType.Message,
                Timestamp = DateTimeOffset.UtcNow,
                Id = Guid.NewGuid().ToString()
            };

            // 1. Add MessageHeader (REQUIRED for NPHIES)
            var messageHeader = CreateMessageHeader(
                   eventCode: "eligibility-request",
                  focusReference: $"CoverageEligibilityRequest/{request.RequestId}",
                  source: provider,
                  destination: insurer
             );
            bundle.Entry.Add(CreateBundleEntry(messageHeader, $"MessageHeader/{messageHeader.Id}"));

            // 2. Add CoverageEligibilityRequest
            var fhirRequest = MapToFhirEligibilityRequest(request, patient, coverage, provider, insurer);
            bundle.Entry.Add(CreateBundleEntry(fhirRequest, $"CoverageEligibilityRequest/{request.RequestId}"));

            // 3. Add Patient
            var fhirPatient = MapToFhirPatient(patient);
            bundle.Entry.Add(CreateBundleEntry(fhirPatient, $"Patient/{patient.Id}"));

            // 4. Add Coverage
            var fhirCoverage = MapToFhirCoverage(coverage, patient, insurer);
            bundle.Entry.Add(CreateBundleEntry(fhirCoverage, $"Coverage/{coverage.Id}"));

            // 5. Add Provider Organization
            var fhirProvider = MapToFhirOrganization(provider);
            bundle.Entry.Add(CreateBundleEntry(fhirProvider, $"Organization/{provider.Id}"));

            // 6. Add Insurer Organization
            var fhirInsurer = MapToFhirOrganization(insurer);
            bundle.Entry.Add(CreateBundleEntry(fhirInsurer, $"Organization/{insurer.Id}"));

            _logger.LogInformation("Eligibility bundle created successfully with {Count} entries", bundle.Entry.Count);
            return bundle;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error creating eligibility request bundle");
            throw;
        }
    }

    // ==========================================
    // Claim Bundle Creation
    // ==========================================

    public async System.Threading.Tasks.Task<Bundle> CreateClaimRequestBundleAsync(
        DomainClaim claim,
      DomainPatient patient,
        DomainOrganization provider,
     DomainOrganization insurer,
     DomainCoverage coverage,
    DomainEncounter? encounter = null)
    {
        try
        {
            _logger.LogInformation("Creating claim request bundle for claim {ClaimId}", claim.Id);

            var bundle = new Bundle
            {
                Type = Bundle.BundleType.Message,
                Timestamp = DateTimeOffset.UtcNow,
                Id = Guid.NewGuid().ToString()
            };

            // 1. Add MessageHeader
            var messageHeader = CreateMessageHeader(
                          eventCode: "claim-request",
                   focusReference: $"Claim/{claim.ClaimNumber}",
                 source: provider,
              destination: insurer
           );
            bundle.Entry.Add(CreateBundleEntry(messageHeader, $"MessageHeader/{messageHeader.Id}"));

            // 2. Add Claim
            var fhirClaim = MapToFhirClaim(claim, patient, provider, insurer, coverage, encounter);
            bundle.Entry.Add(CreateBundleEntry(fhirClaim, $"Claim/{claim.ClaimNumber}"));

            // 3. Add Patient
            var fhirPatient = MapToFhirPatient(patient);
            bundle.Entry.Add(CreateBundleEntry(fhirPatient, $"Patient/{patient.Id}"));

            // 4. Add Provider Organization
            var fhirProvider = MapToFhirOrganization(provider);
            bundle.Entry.Add(CreateBundleEntry(fhirProvider, $"Organization/{provider.Id}"));

            // 5. Add Insurer Organization
            var fhirInsurer = MapToFhirOrganization(insurer);
            bundle.Entry.Add(CreateBundleEntry(fhirInsurer, $"Organization/{insurer.Id}"));

            // 6. Add Coverage
            var fhirCoverage = MapToFhirCoverage(coverage, patient, insurer);
            bundle.Entry.Add(CreateBundleEntry(fhirCoverage, $"Coverage/{coverage.Id}"));

            // 7. Add Encounter (if provided and claim is institutional)
            if (encounter != null)
            {
                var fhirEncounter = MapToFhirEncounter(encounter, patient, provider);
                bundle.Entry.Add(CreateBundleEntry(fhirEncounter, $"Encounter/{encounter.Id}"));
            }

            _logger.LogInformation("Claim bundle created successfully with {Count} entries", bundle.Entry.Count);
            return bundle;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error creating claim request bundle");
            throw;
        }
    }

    // ==========================================
    // Pre-Authorization Bundle Creation
    // ==========================================

    public async System.Threading.Tasks.Task<Bundle> CreatePreAuthRequestBundleAsync(
   DomainClaim preAuth,
        DomainPatient patient,
        DomainOrganization provider,
     DomainOrganization insurer,
        DomainCoverage coverage,
        DomainEncounter? encounter = null)
    {
        try
        {
            _logger.LogInformation("Creating pre-auth request bundle for claim {ClaimId}", preAuth.Id);

            // Pre-auth is same as claim but with use=preauthorization
            // We can reuse claim bundle creation logic
            var bundle = await CreateClaimRequestBundleAsync(
                preAuth, patient, provider, insurer, coverage, encounter);

            // Update MessageHeader event code
            var messageHeader = bundle.Entry
              .Select(e => e.Resource as FhirMessageHeader)
              .FirstOrDefault(r => r != null);

            if (messageHeader != null)
            {
                messageHeader.Event = new Coding(
               "http://nphies.sa/terminology/CodeSystem/ksa-message-events",
               "priorauth-request"
                        );
            }

            _logger.LogInformation("Pre-auth bundle created successfully");
            return bundle;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error creating pre-auth request bundle");
            throw;
        }
    }

    // ==========================================
    // Polling Bundle Creation
    // ==========================================

    public async System.Threading.Tasks.Task<Bundle> CreatePollRequestBundleAsync(string taskId, DomainOrganization provider)
    {
        try
        {
            _logger.LogInformation("Creating poll request bundle for task {TaskId}", taskId);

            var bundle = new Bundle
            {
                Type = Bundle.BundleType.Message,
                Timestamp = DateTimeOffset.UtcNow,
                Id = Guid.NewGuid().ToString()
            };

            // 1. Add MessageHeader
            var messageHeader = CreateMessageHeader(
                     eventCode: "poll-request",
                focusReference: $"Task/{taskId}",
              source: provider,
         destination: null // NPHIES
            );
            bundle.Entry.Add(CreateBundleEntry(messageHeader, $"MessageHeader/{messageHeader.Id}"));

            // 2. Add Parameters resource with task ID
            var parameters = new Parameters();
            parameters.Parameter.Add(new Parameters.ParameterComponent
            {
                Name = "task-id",
                Value = new FhirString(taskId)
            });
            bundle.Entry.Add(CreateBundleEntry(parameters, $"Parameters/{Guid.NewGuid()}"));

            _logger.LogInformation("Poll bundle created successfully");
            return bundle;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error creating poll request bundle");
            throw;
        }
    }

    // ==========================================
    // Serialization
    // ==========================================

    public async System.Threading.Tasks.Task<string> SerializeToJsonAsync(Bundle bundle)
    {
        try
        {
            _logger.LogDebug("Serializing FHIR bundle to JSON");
            var json = await System.Threading.Tasks.Task.Run(() => _serializer.SerializeToString(bundle));
            _logger.LogDebug("Bundle serialized successfully. Size: {Size} bytes", Encoding.UTF8.GetByteCount(json));
            return json;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error serializing bundle to JSON");
            throw;
        }
    }

    public async System.Threading.Tasks.Task<Bundle> DeserializeFromJsonAsync(string json)
    {
        try
        {
            _logger.LogDebug("Deserializing JSON to FHIR bundle");
            var bundle = await System.Threading.Tasks.Task.Run(() => _parser.Parse<Bundle>(json));
            _logger.LogDebug("Bundle deserialized successfully. Entries: {Count}", bundle.Entry?.Count ?? 0);
            return bundle;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error deserializing JSON to bundle");
            throw;
        }
    }

    public async System.Threading.Tasks.Task<T?> ParseResourceFromBundleAsync<T>(Bundle bundle) where T : Resource
    {
        try
        {
            var resource = bundle.Entry?
         .Select(e => e.Resource)
             .OfType<T>()
         .FirstOrDefault();

            if (resource == null)
            {
                _logger.LogWarning("Resource of type {ResourceType} not found in bundle", typeof(T).Name);
            }

            return await System.Threading.Tasks.Task.FromResult(resource);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error parsing resource from bundle");
            throw;
        }
    }

    public async System.Threading.Tasks.Task<List<T>> ExtractResourcesAsync<T>(Bundle bundle) where T : Resource
    {
        try
        {
            var resources = bundle.Entry?
   .Select(e => e.Resource)
  .OfType<T>()
      .ToList() ?? new List<T>();

            _logger.LogDebug("Extracted {Count} resources of type {ResourceType}", resources.Count, typeof(T).Name);
            return await System.Threading.Tasks.Task.FromResult(resources);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error extracting resources from bundle");
            throw;
        }
    }

    // ==========================================
    // Response Parsing
    // ==========================================

    public async System.Threading.Tasks.Task<DomainCoverageEligibilityResponse> ParseEligibilityResponseAsync(Bundle responseBundle)
    {
        try
      {
    _logger.LogInformation("Parsing eligibility response bundle");

            if (responseBundle == null)
  throw new ArgumentNullException(nameof(responseBundle));

  // Extract MessageHeader
       var messageHeader = await ParseResourceFromBundleAsync<FhirMessageHeader>(responseBundle);
   
            // Extract CoverageEligibilityResponse
            var fhirResponse = await ParseResourceFromBundleAsync<Hl7.Fhir.Model.CoverageEligibilityResponse>(responseBundle);
          if (fhirResponse == null)
           throw new InvalidOperationException("CoverageEligibilityResponse not found in bundle");

      // Map to domain entity
            var domainResponse = new DomainCoverageEligibilityResponse
    {
         Id = fhirResponse.Id ?? Guid.NewGuid().ToString(),
            ResponseUUID = responseBundle.Id ?? Guid.NewGuid().ToString(),
       RequestId = fhirResponse.Request?.Reference ?? string.Empty,
   Status = fhirResponse.Status?.ToString()?.ToLower() ?? "active",
   Outcome = fhirResponse.Outcome?.ToString()?.ToLower() ?? "complete",
          Disposition = fhirResponse.Disposition,
     ResponseCreatedAt = DateTime.TryParse(fhirResponse.Created, out var created) ? created : DateTime.UtcNow,
    ResponseReceivedAt = DateTime.UtcNow,
     FhirResponseContent = await SerializeToJsonAsync(responseBundle),
    CreatedAt = DateTime.UtcNow,
     IsActive = true
    };

     // Extract Patient reference
    if (fhirResponse.Patient?.Reference != null)
            {
     domainResponse.PatientId = ExtractIdFromReference(fhirResponse.Patient.Reference);
            }

            // Extract Insurer reference
if (fhirResponse.Insurer?.Reference != null)
     {
       domainResponse.InsurerId = ExtractIdFromReference(fhirResponse.Insurer.Reference);
            }

      // Extract serviced date
            if (fhirResponse.Serviced is FhirDateTime servicedDate)
 {
       if (DateTime.TryParse(servicedDate.Value, out var serviceDate))
domainResponse.ServicedDate = serviceDate;
            }

            // Parse Insurance items (benefit information)
        if (fhirResponse.Insurance?.Any() == true)
   {
    foreach (var insurance in fhirResponse.Insurance)
           {
      // Extract coverage reference
           if (insurance.Coverage?.Reference != null)
           {
      domainResponse.CoverageId = ExtractIdFromReference(insurance.Coverage.Reference);
 }

     // Check if coverage is in-force
                domainResponse.IsInForce = insurance.Inforce ?? false;

  // Parse benefit period
if (insurance.BenefitPeriod != null)
   {
    if (DateTime.TryParse(insurance.BenefitPeriod.Start, out var periodStart))
       domainResponse.BenefitPeriodStart = periodStart;
       if (DateTime.TryParse(insurance.BenefitPeriod.End, out var periodEnd))
     domainResponse.BenefitPeriodEnd = periodEnd;
    }

 // Parse benefit items (deductible, copay, etc.)
    if (insurance.Item?.Any() == true)
       {
   foreach (var item in insurance.Item)
          {
        var benefitBalance = new BenefitBalance
    {
     Id = Guid.NewGuid().ToString(),
         EligibilityResponseId = domainResponse.Id,
         Category = item.Category?.Coding?.FirstOrDefault()?.Code ?? "general",
  CategorySystem = item.Category?.Coding?.FirstOrDefault()?.System ?? "http://terminology.hl7.org/CodeSystem/ex-benefitcategory",
   CategoryDescription = item.Category?.Coding?.FirstOrDefault()?.Display ?? string.Empty,
   CreatedAt = DateTime.UtcNow,
    IsActive = true
  };

      // Parse individual benefits within this category
   if (item.Benefit?.Any() == true)
            {
      foreach (var benefit in item.Benefit)
     {
     var domainBenefit = new Benefit
         {
      Id = Guid.NewGuid().ToString(),
         BenefitBalanceId = benefitBalance.Id,
     BenefitType = benefit.Type?.Coding?.FirstOrDefault()?.Code ?? "unknown",
   BenefitTypeSystem = benefit.Type?.Coding?.FirstOrDefault()?.System,
           BenefitTypeDescription = benefit.Type?.Coding?.FirstOrDefault()?.Display ?? string.Empty,
    CreatedAt = DateTime.UtcNow,
 IsActive = true
  };

          // Extract allowed money
       if (benefit.Allowed is Money allowedMoney)
          {
        domainBenefit.AllowedAmount = allowedMoney.Value ?? 0;
         domainBenefit.AllowedCurrency = allowedMoney.Currency?.ToString() ?? "SAR";
    }

    // Extract used money
     if (benefit.Used is Money usedMoney)
 {
        domainBenefit.UsedAmount = usedMoney.Value ?? 0;
    }

 benefitBalance.Benefits.Add(domainBenefit);
    }
         }

            domainResponse.BenefitBalances.Add(benefitBalance);
         }
  }
    }
}

        // Parse errors if outcome is error
     if (fhirResponse.Error?.Any() == true)
   {
        foreach (var error in fhirResponse.Error)
{
         var domainError = new EligibilityError
    {
     Id = Guid.NewGuid().ToString(),
      EligibilityResponseId = domainResponse.Id,
       ErrorCode = error.Code?.Coding?.FirstOrDefault()?.Code ?? "unknown",
  ErrorCodeSystem = error.Code?.Coding?.FirstOrDefault()?.System ?? "http://nphies.sa/terminology/CodeSystem/error-code",
     ErrorMessage = error.Code?.Coding?.FirstOrDefault()?.Display ?? error.Code?.Text ?? string.Empty,
   Severity = "error", // Default severity
    CreatedAt = DateTime.UtcNow,
           IsActive = true
     };

     domainResponse.Errors.Add(domainError);
     }
      }

    _logger.LogInformation("Eligibility response parsed successfully. Outcome: {Outcome}", domainResponse.Outcome);
            return domainResponse;
  }
        catch (Exception ex)
        {
   _logger.LogError(ex, "Error parsing eligibility response bundle");
          throw;
     }
    }

    public async System.Threading.Tasks.Task<DomainClaimResponse> ParseClaimResponseAsync(Bundle responseBundle)
    {
        try
     {
      _logger.LogInformation("Parsing claim response bundle");

            if (responseBundle == null)
                throw new ArgumentNullException(nameof(responseBundle));

        // Extract ClaimResponse
 var fhirClaimResponse = await ParseResourceFromBundleAsync<FhirClaimResponse>(responseBundle);
            if (fhirClaimResponse == null)
    throw new InvalidOperationException("ClaimResponse not found in bundle");

         // Map to domain entity
     var domainResponse = new DomainClaimResponse
    {
                Id = fhirClaimResponse.Id ?? Guid.NewGuid().ToString(),
  ClaimResponseStatus = fhirClaimResponse.Status?.ToString()?.ToLower() ?? "active",
       Use = fhirClaimResponse.Use?.ToString()?.ToLower(),
   ClaimType = fhirClaimResponse.Type?.Coding?.FirstOrDefault()?.Code,
ClaimTypeSystem = fhirClaimResponse.Type?.Coding?.FirstOrDefault()?.System,
        FhirClaimResponseJson = await SerializeToJsonAsync(responseBundle),
    CreatedAt = DateTime.UtcNow,
          IsActive = true
            };

   // Extract claim reference
       if (fhirClaimResponse.Request?.Reference != null)
{
          domainResponse.ClaimId = ExtractIdFromReference(fhirClaimResponse.Request.Reference);
            }

  // Extract patient reference
            if (fhirClaimResponse.Patient?.Reference != null)
            {
    domainResponse.PatientId = ExtractIdFromReference(fhirClaimResponse.Patient.Reference);
         }

         // Extract insurer reference
            if (fhirClaimResponse.Insurer?.Reference != null)
            {
           domainResponse.InsurerId = ExtractIdFromReference(fhirClaimResponse.Insurer.Reference);
            }

            // Extract requestor reference
   if (fhirClaimResponse.Requestor?.Reference != null)
            {
   domainResponse.RequestorId = ExtractIdFromReference(fhirClaimResponse.Requestor.Reference);
            }

// Extract pre-authorization reference
      if (fhirClaimResponse.PreAuthRef != null)
        {
    domainResponse.PreAuthRef = fhirClaimResponse.PreAuthRef;
 }

          // Extract pre-authorization period
            if (fhirClaimResponse.PreAuthPeriod != null)
 {
          if (DateTime.TryParse(fhirClaimResponse.PreAuthPeriod.Start, out var preAuthStart))
        domainResponse.PreAuthPeriodStart = preAuthStart;
          if (DateTime.TryParse(fhirClaimResponse.PreAuthPeriod.End, out var preAuthEnd))
      domainResponse.PreAuthPeriodEnd = preAuthEnd;
     }

            // Parse insurance information
    if (fhirClaimResponse.Insurance?.Any() == true)
            {
        foreach (var insurance in fhirClaimResponse.Insurance)
                {
        var domainInsurance = new ClaimResponseInsurance
{
    Id = Guid.NewGuid().ToString(),
       ClaimResponseId = domainResponse.Id,
   Sequence = insurance.Sequence ?? 1,
     CoverageId = insurance.Coverage?.Reference != null 
  ? ExtractIdFromReference(insurance.Coverage.Reference) 
     : null,
     PreAuthReferences = string.Empty, // Note: PreAuthRef is at ClaimResponse level, not Insurance level
       CreatedAt = DateTime.UtcNow,
IsActive = true
        };

    domainResponse.Insurance.Add(domainInsurance);
          }
 }

    // Parse totals
  if (fhirClaimResponse.Total?.Any() == true)
            {
     foreach (var total in fhirClaimResponse.Total)
         {
 var domainTotal = new ClaimResponseTotal
      {
    Id = Guid.NewGuid().ToString(),
   ClaimResponseId = domainResponse.Id,
 Category = total.Category?.Coding?.FirstOrDefault()?.Code ?? "submitted",
  CategorySystem = total.Category?.Coding?.FirstOrDefault()?.System,
  CategoryDisplay = total.Category?.Coding?.FirstOrDefault()?.Display,
      Amount = total.Amount?.Value ?? 0,
    Currency = total.Amount?.Currency?.ToString() ?? "SAR",
     CreatedAt = DateTime.UtcNow,
      IsActive = true
  };

domainResponse.Totals.Add(domainTotal);
             }
   }

          // Parse add items (for pre-authorization responses)
     if (fhirClaimResponse.AddItem?.Any() == true)
     {
           foreach (var addItem in fhirClaimResponse.AddItem)
           {
        var domainAddItem = new ClaimResponseAddItem
       {
   Id = Guid.NewGuid().ToString(),
         ClaimResponseId = domainResponse.Id,
     Sequence = addItem.ItemSequence?.FirstOrDefault() ?? 0,
       ProductOrServiceCode = addItem.ProductOrService?.Coding?.FirstOrDefault()?.Code,
        ProductOrServiceSystem = addItem.ProductOrService?.Coding?.FirstOrDefault()?.System,
ProductOrServiceDisplay = addItem.ProductOrService?.Coding?.FirstOrDefault()?.Display,
 ApprovedQuantity = (int?)(addItem.Quantity?.Value ?? 0),
     BenefitAmount = addItem.Net?.Value,
  BenefitCurrency = addItem.Net?.Currency?.ToString() ?? "SAR",
    CreatedAt = DateTime.UtcNow,
      IsActive = true
       };

       domainResponse.AddItems.Add(domainAddItem);
     }
            }

            _logger.LogInformation("Claim response parsed successfully. Status: {Status}", domainResponse.ClaimResponseStatus);
            return domainResponse;
        }
   catch (Exception ex)
        {
          _logger.LogError(ex, "Error parsing claim response bundle");
    throw;
        }
    }

    public async System.Threading.Tasks.Task<DomainClaimResponse> ParsePreAuthResponseAsync(Bundle responseBundle)
    {
        try
        {
          _logger.LogInformation("Parsing pre-authorization response bundle");

            // Pre-auth response is the same structure as claim response, just with use=preauthorization
  var domainResponse = await ParseClaimResponseAsync(responseBundle);

   // Ensure the use is set to preauthorization
   domainResponse.Use = "preauthorization";

            _logger.LogInformation("Pre-authorization response parsed successfully");
       return domainResponse;
}
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error parsing pre-authorization response bundle");
       throw;
        }
    }

    // Helper method to extract resource ID from reference string
    private string ExtractIdFromReference(string reference)
    {
        if (string.IsNullOrEmpty(reference))
    return string.Empty;

        // Reference format is typically "ResourceType/ID"
        var parts = reference.Split('/');
        return parts.Length > 1 ? parts[^1] : reference;
    }

    // ==========================================
    // Validation
    // ==========================================

    public async System.Threading.Tasks.Task<BundleValidationResult> ValidateBundleAsync(Bundle bundle)
    {
        var result = new BundleValidationResult { IsValid = true };

        try
        {
            // Basic validation checks
            if (bundle == null)
            {
                result.Errors.Add("Bundle is null");
                result.IsValid = false;
                return result;
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

            return await System.Threading.Tasks.Task.FromResult(result);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error validating bundle");
            result.Errors.Add($"Validation error: {ex.Message}");
            result.IsValid = false;
            return result;
        }
    }

    // ==========================================
    // Helper Methods - MessageHeader
    // ==========================================

    private FhirMessageHeader CreateMessageHeader(
        string eventCode,
        string focusReference,
      DomainOrganization source,
        DomainOrganization? destination)
    {
        var messageHeader = new FhirMessageHeader
        {
            Id = Guid.NewGuid().ToString(),
            Event = new Coding(
       "http://nphies.sa/terminology/CodeSystem/ksa-message-events",
     eventCode
   ),
            Source = new FhirMessageHeader.MessageSourceComponent
            {
                Endpoint = $"http://provider.sa/{source.Id}",
                Name = source.OrganizationName
            },
            Focus = new List<ResourceReference>
       {
     new ResourceReference(focusReference)
       }
        };

        // Add destination (NPHIES or specific payer)
        if (destination != null)
        {
            messageHeader.Destination.Add(new FhirMessageHeader.MessageDestinationComponent
            {
                Endpoint = "http://nphies.sa/fhir",
                Receiver = new ResourceReference($"Organization/{destination.Id}")
            });
        }
        else
        {
            messageHeader.Destination.Add(new FhirMessageHeader.MessageDestinationComponent
            {
                Endpoint = "http://nphies.sa/fhir",
                Name = "NPHIES"
            });
        }

        return messageHeader;
    }

    private Bundle.EntryComponent CreateBundleEntry(Resource resource, string fullUrl)
    {
        return new Bundle.EntryComponent
        {
            FullUrl = fullUrl,
            Resource = resource
        };
    }

    // ==========================================
    // Helper Methods - Resource Mapping (Simplified)
    // ==========================================

    private FhirCoverageEligibilityRequest MapToFhirEligibilityRequest(
      DomainCoverageEligibilityRequest request,
        DomainPatient patient,
        DomainCoverage coverage,
        DomainOrganization provider,
        DomainOrganization insurer)
    {
        return new FhirCoverageEligibilityRequest
        {
            Id = request.RequestId,
            Status = FinancialResourceStatusCodes.Active,
            Purpose = new List<FhirCoverageEligibilityRequest.EligibilityRequestPurpose?>
        {
            FhirCoverageEligibilityRequest.EligibilityRequestPurpose.Benefits
        },
            Patient = new ResourceReference($"Patient/{patient.Id}"),
            Created = request.RequestCreatedAt.ToString("yyyy-MM-dd"),
            Provider = new ResourceReference($"Organization/{provider.Id}"),
            Insurer = new ResourceReference($"Organization/{insurer.Id}"),
            Insurance = new List<FhirCoverageEligibilityRequest.InsuranceComponent>
        {
            new FhirCoverageEligibilityRequest.InsuranceComponent
            {
                Coverage = new ResourceReference($"Coverage/{coverage.Id}")
            }
        }
        };
    }

    private FhirClaim MapToFhirClaim(
    DomainClaim claim,
     DomainPatient patient,
    DomainOrganization provider,
   DomainOrganization insurer,
        DomainCoverage coverage,
   DomainEncounter? encounter)
    {
        var fhirClaim = new FhirClaim
        {
            Id = claim.ClaimNumber,
            Status = FinancialResourceStatusCodes.Active,
            Type = new CodeableConcept(
        "http://terminology.hl7.org/CodeSystem/claim-type",
        claim.ClaimType ?? "institutional"
           ),
            Patient = new ResourceReference($"Patient/{patient.Id}"),
            Created = claim.CreatedAt.ToString("yyyy-MM-ddTHH:mm:ssZ"),
            Provider = new ResourceReference($"Organization/{provider.Id}"),
            Insurer = new ResourceReference($"Organization/{insurer.Id}"),
            Priority = new CodeableConcept(
          "http://terminology.hl7.org/CodeSystem/processpriority",
        claim.Priority ?? "normal"
       ),
            Insurance = new List<FhirClaim.InsuranceComponent>
                    {
                 new FhirClaim.InsuranceComponent
              {
                          Sequence = 1,
                           Focal = true,
                           Coverage = new ResourceReference($"Coverage/{coverage.Id}")
                  }
                    }
        };



        // Add total
        fhirClaim.Total = new Money
        {
            Value = claim.Total,
            Currency = Money.Currencies.SAR
        };

        // Add encounter reference if provided
        if (encounter != null)
        {
            fhirClaim.Item = new List<FhirClaim.ItemComponent>
                    {
                    new FhirClaim.ItemComponent
                          {
                         Sequence = 1,
                       ProductOrService = new CodeableConcept("http://terminology.hl7.org/CodeSystem/data-absent-reason", "unknown"),
                       Encounter = new List<ResourceReference>
                        {
                      new ResourceReference($"Encounter/{encounter.Id}")
                            }
                        }
                        };
        }

        return fhirClaim;
    }

    private FhirPatient MapToFhirPatient(DomainPatient patient)
    {
        var fhirPatient = new FhirPatient
        {
            Id = patient.Id.ToString(),
            Identifier = new List<Identifier>
                  {
              new Identifier
             {
                System = "http://nphies.sa/identifier/iqama",
                     Value = patient.NationalId
                        }
              },
            Name = new List<HumanName>
             {
             new HumanName
                        {
             Given = new[] { patient.FirstName },
             Family = patient.LastName,
                  Use = HumanName.NameUse.Official
              }
                },
            Gender = patient.Gender?.ToLower() == "male"
             ? AdministrativeGender.Male
                : AdministrativeGender.Female,
            BirthDate = patient.DateOfBirth.ToString("yyyy-MM-dd")
        };

        return fhirPatient;
    }

    private FhirCoverage MapToFhirCoverage(
        DomainCoverage coverage,
        DomainPatient patient,
        DomainOrganization insurer)
    {
        return new FhirCoverage
        {
            Id = coverage.Id.ToString(),
            Status = FinancialResourceStatusCodes.Active,
            Beneficiary = new ResourceReference($"Patient/{patient.Id}"),
            Payor = new List<ResourceReference>
  {
        new ResourceReference($"Organization/{insurer.Id}")
      },
            Subscriber = new ResourceReference($"Patient/{patient.Id}"),
            SubscriberId = coverage.PolicyNumber
        };
    }

    private FhirOrganization MapToFhirOrganization(DomainOrganization org)
    {
        return new FhirOrganization
        {
            Id = org.Id.ToString(),
            Identifier = new List<Identifier>
  {
     new Identifier
  {
       System = "http://nphies.sa/license/provider-license",
        Value = org.LicenseNumber
       }
    },
            Name = org.OrganizationName,
            Active = org.IsActiveOrganization
        };
    }

    private FhirEncounter MapToFhirEncounter(
        DomainEncounter encounter,
        DomainPatient patient,
     DomainOrganization provider)
    {
        return new FhirEncounter
        {
            Id = encounter.Id.ToString(),
            Status = FhirEncounter.EncounterStatus.InProgress,
            Class = new Coding(
               "http://terminology.hl7.org/CodeSystem/v3-ActCode",
               encounter.Class
      ),
            Subject = new ResourceReference($"Patient/{patient.Id}"),
            ServiceProvider = new ResourceReference($"Organization/{provider.Id}"),
            Period = new Period
            {
                Start = encounter.PeriodStart.ToString("yyyy-MM-ddTHH:mm:ssZ"),
                End = encounter.PeriodEnd?.ToString("yyyy-MM-ddTHH:mm:ssZ")
            }
        };
    }
}
