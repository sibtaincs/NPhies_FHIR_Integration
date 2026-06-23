using Hl7.Fhir.Model;
using Hl7.Fhir.Serialization;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using MongoDB.Bson;
using MongoDB.Driver;
using Nphies.Core.Brokers.Loggings;
using Nphies.Core.Data.Entities;
using Nphies.Core.DTOs;
using Nphies.Core.Helper;
using Nphies.Core.Models;
using Nphies.Core.Services.Claims;
using NphiesCertificates;
using NphiesCertificates.Configurations;
using RestSharp;
using System;
using System.Linq;
using System.Net;
using System.Security.Cryptography.X509Certificates;

namespace Nphies.Core.Services.Submission
{
    public class SubmissionService : ISubmissionService
    {
        private readonly IConfiguration _config;
        private readonly ILoggingBroker _loggingBroker;
        private readonly ZyklusCoreContext _dbContext;
        private readonly IClaimService _claimService;
        private readonly IMongoDatabase _mongoDatabase;
        private readonly IMongoCollection<BsonDocument> _claimRequestCollection;
        private readonly string _mongoDbName;
        private readonly int _clientId;

        public SubmissionService(
            IConfiguration config,
            ILoggingBroker loggingBroker,
            ZyklusCoreContext dbContext,
            IClaimService claimService)
        {
            _config = config;
            _loggingBroker = loggingBroker;
            _dbContext = dbContext;
            _claimService = claimService;

            // Initialize MongoDB connection
            var connectionString = GetMongoConnectionString();
            var mongoClient = new MongoClient(connectionString);
            _mongoDbName = _config["MongodbName"];
            _mongoDatabase = mongoClient.GetDatabase(_mongoDbName);
            _claimRequestCollection = _mongoDatabase.GetCollection<BsonDocument>("ClaimRequest");

            // Get client configuration
            _clientId = Convert.ToInt32(_config["ClientId"] ?? "1");
        }

        public async System.Threading.Tasks.Task<SubmitClaimResponse> SubmitClaimWithSameBundleAsync(SubmitClaimRequest request)
        {
            var response = new SubmitClaimResponse
            {
                Success = false
            };

            try
            {
                // Step 1: Get claim details from database
                var claimDetails = await GetClaimDetailsAsync(request.ClaimIdentifier, request.OrganizationId);

                if (claimDetails == null)
                {
                    response.Message = $"Claim not found for identifier: {request.ClaimIdentifier}";
                    _loggingBroker.LogWarning($"Claim details not found for ClaimIdentifier: {request.ClaimIdentifier}, OrganizationId: {request.OrganizationId}");
                    return response;
                }

                // Step 2: Parse claim identifier as GUID
                if (!Guid.TryParse(request.ClaimIdentifier, out Guid claimIdentifierGuid))
                {
                    response.Message = "Invalid claim identifier format.";
                    return response;
                }

                // Step 3: Fetch request JSON from MongoDB
                var requestJson = await GetRequestJsonFromMongoDb(claimIdentifierGuid);
                
                if (string.IsNullOrWhiteSpace(requestJson))
                {
                    response.Message = "Request JSON not found in MongoDB.";
                    return response;
                }

                // Step 4: Parse FHIR Bundle
                var fhirJsonParser = new FhirJsonParser();
                Bundle parsedBundle;
                
                try
                {
                    parsedBundle = fhirJsonParser.Parse<Bundle>(requestJson);
                }
                catch (Exception ex)
                {
                    response.Message = $"Failed to parse FHIR bundle: {ex.Message}";
                    return response;
                }

                // Step 5: Configure certificates dynamically
                ConfigureCertificates(claimDetails.ProjectId, _clientId);


                // Step 6: Send claim to NPHIES
                BatchResponse claimItemResponse = null;
                Bundle responseBundle = null;

                try
                {
                    var result = await SendClaimToNphiesAsync(
                        parsedBundle, 
                        requestJson, 
                        claimDetails.FacilityId,
                        claimDetails.ProjectId);
                    claimItemResponse = result.Item1;
                    responseBundle = result.Item2;
                }
                catch (Exception ex)
                {
                    response.Message = $"Error sending claim to NPHIES: {ex.Message}";
                    _loggingBroker.LogError($"NPHIES send error for claim {claimDetails.ClaimId}: {ex.Message}");
                    await UpdateClaimStatusAsync(claimDetails.ClaimId, $"Error: {ex.Message}", 113);
                    return response;
                }

                // Step 7: Process response and update database
                if (claimItemResponse != null && claimItemResponse.ClaimDetailResponse?.Count > 0 && responseBundle != null)
                {
                    try
                    {
                        //_loggingBroker.LogInfo($"Claim {request.ClaimId} processed with status: {claimItemResponse.ClaimDetailResponse[0].Status}");

                        // Parse FHIR response to ClaimPollModel
                        var pollResponse = Hl7Parser.ParseFhirResponseToClaimPollModel(responseBundle);

                        if (pollResponse != null)
                        {
                            // Update claim services using the existing UpdatePoolingResponse
                            await UpdateClaimServicesAsync(request.OrganizationId, pollResponse);
                        }

                        // Serialize and update MongoDB
                        var jsonSer = new FhirJsonSerializer();
                        var jsonRes = jsonSer.SerializeToString(responseBundle);

                        await UpdateMongoDbResponse(claimIdentifierGuid, jsonRes, claimItemResponse.ClaimDetailResponse[0].Status);

                        response.Success = true;
                        response.ClaimIdentifier = request.ClaimIdentifier;
                        response.Status = claimItemResponse.ClaimDetailResponse[0].Status;
                        response.ResponseDetails = claimItemResponse.ClaimDetailResponse[0].Remarks;
                        response.Message = "Claim submitted successfully.";
                        response.IsPended = pollResponse?.IsPended ?? false;
                    }
                    catch (Exception ex)
                    {
                        _loggingBroker.LogError($"Update error for claim {claimDetails.ClaimId}: {ex.Message}");
                        response.Message = $"Claim sent but update failed: {ex.Message}";
                    }
                }
                else
                {
                    response.Message = "No response received from NPHIES.";
                    _loggingBroker.LogError($"No response from NPHIES for claim {claimDetails.ClaimId}");
                }
            }
            catch (Exception ex)
            {
                response.Message = $"Unexpected error: {ex.Message}";
                _loggingBroker.LogCritical(ex);
            }

            return response;
        }

        private string GetMongoConnectionString()
        {
            var connectionstring = _config.GetSection("ConnectionStrings:MongodbConnection")
                                    .Value.Split(new string[] { "//" },
                                    StringSplitOptions.None);
            var constr = connectionstring[0] + "//" + _config["Mongo_UserName"]
                                    + ":" + _config["Mongo_Password"]
                                    + "@" + connectionstring[1];

            return constr;
        }

        private async System.Threading.Tasks.Task<ClaimDetailsDto> GetClaimDetailsAsync(string claimidentifier, int organizationId)
        {
            try
            {
                // First attempt: Try to get claim details from PostTrails
                var claimDetails = await (from pt in _dbContext.ClaimNphiesPostTrails
                                          join claim in _dbContext.RcmClaims
                                          on pt.ClaimId equals claim.ClaimId
                                          join facility in _dbContext.RcmFacilities
                                          on claim.FacilityId equals facility.FacilityId
                                          where pt.ClaimIdentifier == claimidentifier
                                          && claim.OrganizationId == organizationId
                                          select new ClaimDetailsDto
                                          {
                                              ClaimId = pt.ClaimId,
                                              ClaimIdentifier = pt.ClaimIdentifier,
                                              FacilityId = facility.FacilityId,
                                              ProjectId = facility.ExternalCode,
                                              SetupId = facility.ExternalCode2,
                                              ProcessId = claim.ProcessId
                                          })
                                      .AsNoTracking()
                                      .FirstOrDefaultAsync();

                // If not found in PostTrails, try fetching directly from RCM_Claim
                if (claimDetails == null)
                {
                    _loggingBroker.LogWarning($"Claim not found in PostTrails for identifier: {claimidentifier}. Attempting to fetch from RCM_Claim.");

                    claimDetails = await (from claim in _dbContext.RcmClaims
                                          join facility in _dbContext.RcmFacilities
                                          on claim.FacilityId equals facility.FacilityId
                                          where claim.ClaimIdentifier == claimidentifier
                                          && claim.OrganizationId == organizationId
                                          select new ClaimDetailsDto
                                          {
                                              ClaimId = claim.ClaimId,
                                              ClaimIdentifier = claim.ClaimIdentifier,
                                              FacilityId = facility.FacilityId,
                                              ProjectId = facility.ExternalCode,
                                              SetupId = facility.ExternalCode2,
                                              ProcessId = claim.ProcessId
                                          })
                                      .AsNoTracking()
                                      .FirstOrDefaultAsync();

                    if (claimDetails != null)
                    {
                        _loggingBroker.LogInfo($"Claim found in RCM_Claim for identifier: {claimidentifier}");
                    }
                }

                return claimDetails;
            }
            catch (Exception e)
            {

                throw;
            }
        }

        private async System.Threading.Tasks.Task<string> GetRequestJsonFromMongoDb(Guid claimIdentifierGuid)
        {
            var filter = Builders<BsonDocument>.Filter.Eq("_id", claimIdentifierGuid);
            var projection = Builders<BsonDocument>.Projection.Include("Requestjson").Exclude("_id");
            
            var document = await _claimRequestCollection
                .Find(filter)
                .Project(projection)
                .FirstOrDefaultAsync();

            if (document != null && document.Contains("Requestjson"))
            {
                return document["Requestjson"].AsString;
            }

            return null;
        }

        private void ConfigureCertificates(string projectId, int clientId)
        {
            try
            {
                var certificateFactory = new CertificateManagerFactory();
                var client = (NphiesCertificates.Enums.Clients)clientId;

                if (ClientConfiguration.SupportedClients().Contains(client))
                {
                    var certificateManager = certificateFactory.Create(projectId, client);
                    if (certificateManager != null)
                    {
                        _loggingBroker.LogInfo($"Certificate manager configured for client {client}, project {projectId}");
                    }
                    else
                    {
                        _loggingBroker.LogWarning($"Certificate manager returned null for client {client}");
                    }
                }
                else
                {
                    _loggingBroker.LogWarning($"Client {clientId} is not supported");
                }
            }
            catch (Exception ex)
            {
                _loggingBroker.LogError($"Error configuring certificates: {ex.Message}");
            }
        }

        private async System.Threading.Tasks.Task<Tuple<BatchResponse, Bundle>> SendClaimToNphiesAsync(
            Bundle bundle, 
            string requestJson, 
            int facilityId,
            string projectId)
        {
            Bundle responseBundle = null;

            try
            {
                var jsonSer = new FhirJsonSerializer();
                var json = jsonSer.SerializeToString(bundle);

                var restResponse = await CallNphiesAsync(json, projectId);

                if (restResponse.StatusCode == HttpStatusCode.OK)
                {
                    var fhirJsonParser = new FhirJsonParser();
                    var parsedObject = fhirJsonParser.Parse(restResponse.Content);

                    var responseParsing = new ResponseParsing();
                    
                    if (parsedObject.TypeName == "Bundle")
                    {
                        responseBundle = (Bundle)parsedObject;
                        var batchResponse = responseParsing.CreateResponse(responseBundle, bundle);
                        return new Tuple<BatchResponse, Bundle>(batchResponse, responseBundle);
                    }
                    else if (parsedObject.TypeName == "OperationOutcome")
                    {
                        var batchResponse = responseParsing.CreateResponseObjectFromRequestForOperationOutcome(bundle, restResponse.Content);
                        return new Tuple<BatchResponse, Bundle>(batchResponse, responseBundle);
                    }
                    else
                    {
                        var batchResponse = responseParsing.CreateResponseObjectFromRequestForOperationOutcome(
                            bundle, 
                            "Response of Type: " + parsedObject.TypeName + " Received From NPHIES");
                        return new Tuple<BatchResponse, Bundle>(batchResponse, responseBundle);
                    }
                }
                else
                {
                    if (string.IsNullOrEmpty(restResponse.Content))
                    {
                        var responseParsing = new ResponseParsing();
                        var batchResponse = responseParsing.CreateResponseObjectFromRequestForOperationOutcome(bundle, "Timeout Exception From NPHIES");
                        return new Tuple<BatchResponse, Bundle>(batchResponse, responseBundle);
                    }
                    else
                    {
                        var fhirJsonParser = new FhirJsonParser();
                        var parsedObject = fhirJsonParser.Parse(restResponse.Content);

                        var responseParsing = new ResponseParsing();
                        var batchResponse = responseParsing.CreateResponseObjectFromRequestForOperationOutcome(bundle, restResponse.Content);
                        return new Tuple<BatchResponse, Bundle>(batchResponse, responseBundle);
                    }
                }
            }
            catch (Exception ex)
            {
                _loggingBroker.LogError($"CallNphies error: {ex.Message}");
                throw;
            }
        }

        private async System.Threading.Tasks.Task<IRestResponse> CallNphiesAsync(string requestJson, string projectId)
        {
            var nphiesUrl = _config["GetPollResponse:NPHIES_URL"];
            var nphiesUserName = _config["GetPollResponse:NPHIES_UserName"];
            var nphiesPassword = _config["GetPollResponse:NPHIES_Password"];

            // Get certificate dynamically using NphiesCertificates project
            var (certificateBytes, certificatePassword) = GetNphiesCertificateDynamic(projectId);

            // Validate certificate
            if (certificateBytes == null || certificateBytes.Length == 0)
            {
                var errorMsg = $"Certificate not available for project {projectId}. Ensure NphiesCertificates is properly configured.";
                _loggingBroker.LogError(errorMsg);
                throw new InvalidOperationException(errorMsg);
            }

            try
            {
                // Match the exact implementation from Nphies_Claims_Post\Common\Nphies.cs
                var client = new RestClient(nphiesUrl);

                ServicePointManager.Expect100Continue = true;
                ServicePointManager.DefaultConnectionLimit = 9999;
                // .NET 6 compatible TLS protocols (removed Ssl3)
                ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls | SecurityProtocolType.Tls11 | SecurityProtocolType.Tls12;

                X509Certificate2 certificate = new X509Certificate2(certificateBytes, certificatePassword);
                client.ClientCertificates = new X509CertificateCollection() { certificate };

                _loggingBroker.LogInfo($"Certificate loaded - Project: {projectId}, Subject: {certificate.Subject}, HasPrivateKey: {certificate.HasPrivateKey}");

                client.Proxy = new WebProxy();
                var restRequest = new RestRequest(Method.POST);
                restRequest.Method = Method.POST;
                restRequest.AddHeader("Username", nphiesUserName);
                restRequest.AddHeader("Password", nphiesPassword);
                restRequest.AddHeader("Accept", "application/fhir+json");
                restRequest.AddHeader("Content-Type", "application/fhir+json");
                restRequest.AddHeader("Cache-Control", "no-cache");
                restRequest.Timeout = 90000;
                restRequest.AddParameter("application/fhir+json", requestJson, ParameterType.RequestBody);

                _loggingBroker.LogInfo($"Sending request to NPHIES: {nphiesUrl}");

                IRestResponse response = null;
                try
                {
                    response = await System.Threading.Tasks.Task.Run(() => client.Execute(restRequest));
                }
                catch (Exception executeEx)
                {
                    _loggingBroker.LogError($"RestClient.Execute exception: {executeEx.Message}");
                    if (executeEx.InnerException != null)
                    {
                        _loggingBroker.LogError($"Execute Inner Exception: {executeEx.InnerException.Message}");
                    }
                    throw;
                }

                _loggingBroker.LogInfo($"NPHIES Response - StatusCode: {response.StatusCode}, ResponseStatus: {response.ResponseStatus}");

                // Check for errors in the response
                if (response.ErrorException != null)
                {
                    _loggingBroker.LogError($"NPHIES Request Error: {response.ErrorMessage ?? "No error message"}");
                    _loggingBroker.LogError($"Error Exception Type: {response.ErrorException.GetType().Name}");
                    _loggingBroker.LogError($"Error Exception Message: {response.ErrorException.Message}");
                    
                    if (response.ErrorException.InnerException != null)
                    {
                        _loggingBroker.LogError($"Inner Exception Type: {response.ErrorException.InnerException.GetType().Name}");
                        _loggingBroker.LogError($"Inner Exception Message: {response.ErrorException.InnerException.Message}");
                    }
                    
                    // Don't throw here, let the caller handle the error response
                    // The response object contains the error details
                }

                return response;
            }
            catch (Exception ex)
            {
                _loggingBroker.LogError($"Exception during NPHIES call: {ex.Message}");
                _loggingBroker.LogError($"Exception Type: {ex.GetType().Name}");
                if (ex.InnerException != null)
                {
                    _loggingBroker.LogError($"Inner Exception: {ex.InnerException.Message}");
                    _loggingBroker.LogError($"Inner Exception Type: {ex.InnerException.GetType().Name}");
                }
                throw;
            }
        }

        private (byte[] certificateBytes, string certificatePassword) GetNphiesCertificateDynamic(string projectId)
        {
            try
            {
                var certificateFactory = new CertificateManagerFactory();
                var client = (NphiesCertificates.Enums.Clients)_clientId;

                if (ClientConfiguration.SupportedClients().Contains(client))
                {
                    var certificateManager = certificateFactory.Create(projectId, client);
                    
                    if (certificateManager != null)
                    {
                        var certBytes = certificateManager.GetCertificate();
                        var certPassword = certificateManager.GetCertificatePassword();
                        
                        _loggingBroker.LogInfo($"Certificate retrieved dynamically for project {projectId}, client {client}");
                        return (certBytes, certPassword);
                    }
                    else
                    {
                        _loggingBroker.LogError($"Certificate manager returned null for project {projectId}, client {client}");
                        return (null, string.Empty);
                    }
                }
                else
                {
                    _loggingBroker.LogError($"Client {_clientId} is not in supported clients list");
                    return (null, string.Empty);
                }
            }
            catch (Exception ex)
            {
                _loggingBroker.LogError($"Error retrieving certificate dynamically for project {projectId}: {ex.Message}");
                return (null, string.Empty);
            }
        }

        private async System.Threading.Tasks.Task UpdateMongoDbResponse(Guid claimIdentifierGuid, string responseJson, string status)
        {
            var collection = _mongoDatabase.GetCollection<ClaimRequestModelMD>("ClaimRequest");

            var filter = Builders<ClaimRequestModelMD>.Filter.Eq("Id", claimIdentifierGuid);
            var update = Builders<ClaimRequestModelMD>.Update
                .Set(x => x.ResponseJson, responseJson)
                .Set(x => x.Status, status)
                .Set(x => x.ModifiedOn, DateTime.Now);

            await collection.UpdateOneAsync(filter, update);
        }

        private async System.Threading.Tasks.Task UpdateClaimStatusAsync(long claimId, string remarks, int status)
        {
            try
            {
                var claim = await _dbContext.RcmClaims.FirstOrDefaultAsync(c => c.ClaimId == claimId);
                if (claim != null)
                {
                    claim.Status = (byte)status;
                    claim.Remarks = remarks;
                    await _dbContext.SaveChangesAsync();
                }
            }
            catch (Exception ex)
            {
                _loggingBroker.LogError($"UpdateClaimStatus error for claim {claimId}: {ex.Message}");
            }
        }

        private async System.Threading.Tasks.Task UpdateClaimServicesAsync(int organizationId, PollResponse pollResponse)
        {
            try
            {
                if (pollResponse == null)
                {
                    _loggingBroker.LogWarning("Poll response is null, skipping claim services update");
                    return;
                }

                // Build the ClaimUpdateModel from PollResponse
                var claimUpdateModel = new ClaimUpdateModel
                {
                    OrganizationID = organizationId,
                    ClaimIdentifier = pollResponse.ClaimIdentifier,
                    ResponseBundleID = pollResponse.ResponseBundleID,
                    TotalApproved = pollResponse.TotalApproved,
                    TotalSubmitted = pollResponse.TotalSubmitted,
                    CommunicationUrl = pollResponse.CommunicationUrl,
                    CommunicationIdentifier = pollResponse.CommunicationIdentifier,
                    IsPended = pollResponse.IsPended,
                    Remarks = BuildRemarks(pollResponse),
                    Status = GetStatusCode(pollResponse),
                    ClaimItems = BuildClaimItems(pollResponse)
                };

                // Call the existing UpdatePoolingResponse from IClaimService
                var result = await _claimService.UpdatePoolingResponse(claimUpdateModel);

                if (result)
                {
                    _loggingBroker.LogInfo($"Successfully updated claim services for claim: {pollResponse.ClaimIdentifier}");
                }
                else
                {
                    _loggingBroker.LogWarning($"Failed to update claim services for claim: {pollResponse.ClaimIdentifier}");
                }
            }
            catch (Exception ex)
            {
                _loggingBroker.LogError($"Error updating claim services: {ex.Message}");
                throw;
            }
        }

        private string BuildRemarks(PollResponse pollResponse)
        {
            var remarks = string.Empty;

            if (!string.IsNullOrEmpty(pollResponse.ClaimReason))
            {
                remarks = pollResponse.ClaimReason;
            }

            if (!string.IsNullOrEmpty(pollResponse.Disposition))
            {
                if (!string.IsNullOrEmpty(remarks))
                {
                    remarks += " :: ";
                }
                remarks += pollResponse.Disposition;
            }

            return remarks;
        }

        private int GetStatusCode(PollResponse pollResponse)
        {
            if (!string.IsNullOrEmpty(pollResponse.Status))
            {
                return GetStatus(pollResponse.Status);
            }

            return GetStatus("error");
        }

        private int GetStatus(string status)
        {
            switch (status.ToLower().Trim())
            {
                case "queued":
                    return 67;
                case "rejected":
                    return 71;
                case "complete":
                case "completed":
                case "approved":
                    return 9;
                case "partial approved":
                case "partial":
                    return 8;
                case "error":
                    return 52;
                case "furtherdetails":
                    return 59;
                case "pended":
                    return 93;
                case "perror":
                    return 51;
                case "outcome":
                    return 99;
                default:
                    return 52; // Default to error
            }
        }

        private System.Collections.Generic.List<ClaimItems> BuildClaimItems(PollResponse pollResponse)
        {
            var claimItems = new System.Collections.Generic.List<ClaimItems>();

            if (pollResponse.ItemsResponse != null)
            {
                foreach (var itemResponse in pollResponse.ItemsResponse)
                {
                    claimItems.Add(new ClaimItems
                    {
                        ItemSequence = itemResponse.ItemSequence,
                        Status = GetStatus(itemResponse.Status).ToString(),
                        ItemReason = itemResponse.ItemReason,
                        SubmittedAmount = itemResponse.SubmittedAmount,
                        BenefitAmount = itemResponse.BenefitAmount,
                        ReasonCode = itemResponse.ReasonCode,
                        ApprovedQuantity = itemResponse.ApprovedQuantity,
                        TaxApproved = itemResponse.TaxApproved,
                        ServiceReferenceNo = string.IsNullOrEmpty(itemResponse.InvoiceNo) 
                            ? "0" 
                            : itemResponse.InvoiceNo
                    });
                }
            }

            return claimItems;
        }
    }

    internal class ClaimDetailsDto
    {
        public long ClaimId { get; set; }
        public string ClaimIdentifier { get; set; }
        public int FacilityId { get; set; }
        public string ProjectId { get; set; }
        public string SetupId { get; set; }
        public long? ProcessId { get; set; }
    }
}
