using Hl7.Fhir.Model;
using Hl7.Fhir.Rest;
using Hl7.Fhir.Serialization;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Microsoft.Extensions.Configuration;
using Microsoft.OpenApi.Models;
using MongoDB.Bson;
using MongoDB.Driver;
using MongoDB.Driver.Core.Configuration;
using Newtonsoft.Json.Linq;
using Nphies.Core.Brokers.Loggings;
using Nphies.Core.Data.Entities;
using Nphies.Core.Helper;
using Nphies.Core.Models;
using Nphies.Core.Models.Configurations;
using Nphies.Core.Services.Claims;
using Nphies.Core.Services.Communication;
using NphiesCertificates;
using NphiesCertificates.Configurations;
using RestSharp;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Net;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;

namespace Nphies.Core.Services.Logger
{
    public class LogService<T> : ILogService, IMongoDb<T>
        where T : BsonDocument
    {
        public IMongoDatabase mongoDatabase { get; set; }
        public IMongoCollection<T> mongoCollection { get; set; }

        private IMongoCollection<ClaimRequestModelMD> ClaimRequestCollection;
        private IMongoCollection<ClaimBatchModelMD> ClaimBatchCollection;
        private IMongoCollection<ClaimPollResponseMD> ClaimPoolCollection;
        private IMongoCollection<ClaimCommunicationsMD> ClaimCommunicationCollection;
        protected readonly ZyklusCoreContext _dbContext;
        private readonly IClaimService _claimService;
        private IMongoDatabase mongoDb;
        private string MongodbName;
        MongoClient client;
        private readonly IConfiguration config;
        private readonly ILoggingBroker loggingBroker;
        private readonly int _clientId;

        public LogService(IConfiguration _config, ZyklusCoreContext dbContext,IClaimService claimService, ILoggingBroker loggingBroker)
        {
            this.config = _config;
            this.GetConnection();
            _dbContext = dbContext;
            _claimService = claimService;
            this.loggingBroker = loggingBroker;
            
            // Get client configuration for certificate management
            _clientId = Convert.ToInt32(config["ClientId"] ?? "1");
        }
        private void GetConnection()
        {
            var connectionstring = config.GetSection("ConnectionStrings:MongodbConnection")
                                    .Value.Split(new string[] { "//" },
                                    StringSplitOptions.None);
            var constr = connectionstring[0] + "//" + config["Mongo_UserName"]
                                    + ":" + config["Mongo_Password"]
                                    + "@" + connectionstring[1];

            client = new MongoClient(constr);
            MongodbName = config["MongodbName"];
            mongoDatabase = client.GetDatabase(config["MongodbName"]);
        }
        public async Task<ClaimRequestModelMD> GetClaimRequestLog(string claimIdentifier)
        {
            ClaimRequestModelMD claimRequest = new ClaimRequestModelMD();
            try
            {
                Guid IdGuid;
                bool isValidGuid = Guid.TryParse(claimIdentifier, out IdGuid);
                if (isValidGuid)
                {
                    mongoDb = client.GetDatabase(MongodbName);
                    ClaimRequestCollection = mongoDb.GetCollection<ClaimRequestModelMD>("ClaimRequest");
                    claimRequest = await LoadDocument(IdGuid);

                    ClaimPoolCollection = mongoDb.GetCollection<ClaimPollResponseMD>("ClaimPollResponse");

                    claimRequest = claimRequest == null ? new ClaimRequestModelMD() : claimRequest;
                    claimRequest.pollRequestResponse.multipleResponse = new List<MultipleResponse>();
                    claimRequest.pollRequestResponse = await LoadPoolDocument(IdGuid);

                    //claimRequest.communications = new List<ClaimCommunicationsMD>();
                    //claimRequest.communications = await LoadCommunicationDocument(IdGuid);

                }
            }
            catch (Exception ex) { throw ex; }
            return claimRequest;
        }

        private async Task<ClaimRequestModelMD> LoadDocument(Guid claimIdentifier)
        {
            return await ClaimRequestCollection
                   .Find(x => x.Id == claimIdentifier)
                   .SortByDescending(x => x.CreatedOn)
                   .FirstOrDefaultAsync();
        }
        private async Task<ClaimPollResponseMD> LoadPoolDocument(Guid claimIdentifier)
        {
            //return await ClaimPoolCollection
            //       .Find(_=>true)
            //       .SortByDescending(x => x.CreatedOn)
            //       .FirstOrDefaultAsync();


            return await ClaimPoolCollection
                   .Find(x => x.Id == claimIdentifier)
                   .SortByDescending(x => x.CreatedOn)
                   .FirstOrDefaultAsync();
        }

        public async Task<ClaimBatchModelMD> GetBatchRequestLog(string claimIdentifier)
        {
            ClaimBatchModelMD claimBatch = new ClaimBatchModelMD();
            try
            {
                Guid IdGuid;
                bool isValidGuid = Guid.TryParse(claimIdentifier, out IdGuid);
                if (isValidGuid)
                {
                    mongoDb = client.GetDatabase(MongodbName);
                    ClaimBatchCollection = mongoDb.GetCollection<ClaimBatchModelMD>("ClaimBatch");

                    //var batch = await ClaimBatchCollection
                    //         .Find(_=>true).ToListAsync();

                    claimBatch = await LoadBatchDocument(IdGuid);
                }
            }
            catch (Exception ex) { throw ex; }
            return claimBatch;
        }
        private async Task<ClaimBatchModelMD> LoadBatchDocument(Guid claimIdentifier)
        {

            return await ClaimBatchCollection
                .Find(x => x.BatchID == claimIdentifier.ToString())
                .SortByDescending(x => x.CreatedOn)
                .FirstOrDefaultAsync();
        }

        public async Task<FriendlyViewRequestModel> GetClaimRequestFriendlyViewer(string claimIdentifier)
        {
            Guid IdGuid;
            FriendlyViewRequestModel friendlyviewModel = new FriendlyViewRequestModel();
            FHIRRequestModel fhirRequestModel = new FHIRRequestModel();
            FHIRResponseModel fhirResponseModel = new FHIRResponseModel();

            ClaimRequestModelMD claimRequest = new ClaimRequestModelMD();
            try
            {
                bool isValidGuid = Guid.TryParse(claimIdentifier, out IdGuid);
                if (isValidGuid)
                {
                    mongoDb = client.GetDatabase(MongodbName);
                    ClaimRequestCollection = mongoDb.GetCollection<ClaimRequestModelMD>("ClaimRequest");

                    claimRequest = await LoadDocument(IdGuid);
                    if (claimRequest != null)
                    {
                        Hl7.Fhir.Model.Bundle requestBundle = Parser.ParseBundle(claimRequest.Requestjson.ToString());
                        RequestBundleModel request = Parser.GetNPHIESRequestModel(requestBundle);

                        if (request != null)
                        {
                            friendlyviewModel.fhirRequestModel = ClaimFriendlyViewerRequest(fhirRequestModel, request);

                        }
                        if (!string.IsNullOrWhiteSpace(claimRequest.ResponseJson))
                        {
                            Hl7.Fhir.Model.Bundle responseBundle = Parser.ParseBundle(claimRequest.ResponseJson.ToString());
                            ResponseBundleModel response = Parser.GetNPHIESResponseModel(responseBundle);
                            if (response != null)
                            {
                                friendlyviewModel.fhirResponseModel = ClaimFriendlyViewerResponse(fhirResponseModel, response);
                            }
                        }
                    }
                }
            }
            catch (Exception ex) {
                string errorMessage = string.Format("{0}-{1}-{2}",nameof(GetClaimRequestFriendlyViewer), claimIdentifier,ex.Message);
                this.loggingBroker.LogError(errorMessage);
            }
            return friendlyviewModel;
        }

        private static FHIRResponseModel ClaimFriendlyViewerResponse(FHIRResponseModel fhirResponseModel, ResponseBundleModel response)
        {
            if (response.patient != null)
            {
                fhirResponseModel.patient.patientMRN = response.patient.Id.ToString();
                fhirResponseModel.patient.patientName = response.patient.Name.FirstOrDefault().ToString();
                fhirResponseModel.patient.gender = response.patient.Gender.ToString();
                fhirResponseModel.patient.dateOfBirth = response.patient.BirthDate;
            }
            if (response.payer != null)
            {
                fhirResponseModel.payer.licenseNo = response.payer.Identifier[0].Value;
                fhirResponseModel.payer.payerName = response.payer.Name;
            }
            if (response.provider != null)
            {
                fhirResponseModel.provider.providerName = response.provider.Name;
                fhirResponseModel.provider.licenseNo = response.provider.Identifier[0].Value;
            }
            if (response.Coverage != null)
            {
                fhirResponseModel.coverageDetail.identifier = response.Coverage.Type.Coding.Select(x => x.Code).FirstOrDefault();
                fhirResponseModel.coverageDetail.status = response.Coverage.Status.ToString();
                fhirResponseModel.coverageDetail.type = response.Coverage.Type.Coding.Select(x => x.Code).FirstOrDefault();
                fhirResponseModel.coverageDetail.beneficiary = response.Coverage.Beneficiary.Reference.ToString();
                fhirResponseModel.coverageDetail.relationship = response.Coverage.Relationship.Coding.Select(x => x.Code).FirstOrDefault();
                fhirResponseModel.coverageDetail.payerName = response.Coverage.Payor[0].Reference;
                fhirResponseModel.coverageDetail.payerClass = response.Coverage.Class.Count > 0 ? response.Coverage.Class.Select(x => x.Value).ToString() : "N/A";
            }
            if (response.claim != null)
            {
                fhirResponseModel.claim.bundleId = response.bundleId.ToString();
                fhirResponseModel.claim.identifier = response.claim.Identifier.Count > 0 ? response.claim.Identifier[0].Value : "-NA-";
                fhirResponseModel.claim.claimId = response.claim.Id.ToString();
                fhirResponseModel.claim.insurer = response.claim.Insurer != null ? response.claim.Insurer.Reference : "-NA-";
                fhirResponseModel.claim.insurance = response.claim.Insurance.Count > 0 ? response.claim.Insurance[0].Coverage.Reference : "";
                fhirResponseModel.claim.requestor = response.claim.Requestor != null ? response.claim.Requestor.Reference : "-NA-";
                fhirResponseModel.claim.request = response.claim.Request != null ? response.claim.Request.Reference : "-NA-";
                fhirResponseModel.claim.type = response.claim.Type != null ? response.claim.Type.Coding[0].Code : "-NA-";
                fhirResponseModel.claim.outcome = response.claim.Outcome != null ? response.claim.Outcome.Value.ToString() : "-NA-";
                fhirResponseModel.claim.preAuthRef = response.claim.PreAuthRef != null ? response.claim.PreAuthRef : "-NA-";
                fhirResponseModel.claim.subType = response.claim.SubType != null ? response.claim.SubType.Coding[0].Code : "-NA-";
                fhirResponseModel.claim.subType = response.claim.Disposition != null ? response.claim.Disposition : "-NA-";
                fhirResponseModel.claim.status = response.claim.Status.ToString();
                fhirResponseModel.claim.patientName = response.claim.Patient.Reference;
                fhirResponseModel.claim.use = response.claim.Use.ToString();
                if (response.claim.PreAuthPeriod != null)
                {
                    fhirResponseModel.claim.preAuthPeriodStart = response.claim.PreAuthPeriod.Start;
                    fhirResponseModel.claim.preAuthPeriodEnd = response.claim.PreAuthPeriod.End;
                }
                else
                {
                    fhirResponseModel.claim.preAuthPeriodStart = "-NA-";
                    fhirResponseModel.claim.preAuthPeriodEnd = "-NA-";
                }

                #region Adjudication Outcome
                Extension extAdjOutcome = response.claim.Extension.FirstOrDefault(e => e.Url.Contains("adjudication-outcome"));
                if (extAdjOutcome != null)
                {
                    CodeableConcept ccAdjOutcome = (CodeableConcept)extAdjOutcome.Value;
                    fhirResponseModel.claim.adjOutcome = ccAdjOutcome.Coding.Count > 0 ? ccAdjOutcome.Coding[0].Code : string.Empty;
                }
                #endregion

                #region Total benefit|submitted
                ClaimResponse.TotalComponent benefitTotal = response.claim.Total.FirstOrDefault(t => t.Category.Coding[0].Code == "benefit");
                ClaimResponse.TotalComponent submittedTotal = response.claim.Total.FirstOrDefault(t => t.Category.Coding[0].Code == "submitted");
                if (benefitTotal != null)
                    fhirResponseModel.claim.totalBenefit = benefitTotal.Amount != null ? benefitTotal.Amount.Value.ToString() + " " + benefitTotal.Amount.Currency : "-NA-";
                if (submittedTotal != null)
                    fhirResponseModel.claim.totalSubmitted = submittedTotal.Amount != null ? submittedTotal.Amount.Value.ToString() + " " + submittedTotal.Amount.Currency : "-NA-";
                #endregion

                #region Adjudication Breakdown benefit|eligible
                ClaimResponse.AdjudicationComponent benefitAdjud = response.claim.Adjudication.FirstOrDefault(a => a.Category.Coding[0].Code == "benefit");
                ClaimResponse.AdjudicationComponent eligibleAdjud = response.claim.Adjudication.FirstOrDefault(a => a.Category.Coding[0].Code == "eligible");

                if (benefitAdjud != null)
                {
                    fhirResponseModel.claim.adjBenefit = benefitAdjud.Amount != null ? benefitAdjud.Amount.Value.ToString() + " " + benefitAdjud.Amount.Currency : "-NA-";
                }
                if (eligibleAdjud != null)
                {
                    fhirResponseModel.claim.adjEligible = eligibleAdjud.Amount != null ? eligibleAdjud.Amount.Value.ToString() + " " + eligibleAdjud.Amount.Currency : "-NA-";
                }
                #endregion

                List<Hl7.Fhir.Model.ClaimResponse.ItemComponent> claimItems = response.claim.Item;
                List<Coding> adjudications = new List<Coding>();
                List<FHIRBreakdown> breakdownList = new List<FHIRBreakdown>();
                StringBuilder sbReason = new StringBuilder();
                foreach (ClaimResponse.ItemComponent item in claimItems)
                {
                    FHIRBreakdown breakdown = new FHIRBreakdown();

                    breakdown.itemSequence = item.ItemSequence.ToString();
                    CodeableConcept codeExtension;
                    codeExtension = item.Extension.Count > 0 ? (CodeableConcept)item.Extension[0].Value : new CodeableConcept();
                    breakdown.code = codeExtension.Coding[0].Code;
                    if (item.Adjudication.Count > 0)
                    {
                        foreach (ClaimResponse.AdjudicationComponent adj in item.Adjudication)
                        {
                            switch (adj.Category.Coding[0].Code)
                            {
                                case "submitted":
                                    breakdown.submitted = adj.Amount != null ? adj.Amount.Value.ToString() + " " + adj.Amount.Currency.ToString() : "-NA-";
                                    break;
                                case "benefit":
                                    breakdown.benefit = adj.Amount != null ? adj.Amount.Value.ToString() + " " + adj.Amount.Currency.ToString() : "-NA-";
                                    break;
                                case "discount":
                                    breakdown.discount = adj.Amount != null ? adj.Amount.Value.ToString() + " " + adj.Amount.Currency.ToString() : "-NA-";
                                    break;
                                case "deductible":
                                    breakdown.deductible = adj.Amount != null ? adj.Amount.Value.ToString() + " " + adj.Amount.Currency.ToString() : "-NA-";
                                    break;
                                case "copay":
                                    breakdown.copay = adj.Amount != null ? adj.Amount.Value.ToString() + " " + adj.Amount.Currency.ToString() : "-NA-";
                                    break;
                                case "unallocdeduct":
                                    breakdown.unallocdeduct = adj.Amount != null ? adj.Amount.Value.ToString() + " " + adj.Amount.Currency.ToString() : "-NA-";
                                    break;
                                case "eligpercent":
                                    breakdown.eligpercent = adj.Amount != null ? adj.Amount.Value.ToString() + " " + adj.Amount.Currency.ToString() : "-NA-";
                                    break;
                                case "tax":
                                    breakdown.tax = adj.Amount != null ? adj.Amount.Value.ToString() + " " + adj.Amount.Currency.ToString() : "-NA-";
                                    break;
                                case "approved-quantity":
                                    breakdown.approvedquantity = adj.Value != null ? adj.Value.ToString() : "-NA-";
                                    break;
                                default:
                                    break;
                            }
                            if (adj.Reason != null)
                            {
                                sbReason.Append("Code(Category) : " + adj.Reason.Coding[0].Code + " (" + adj.Category.Coding[0].Code + ")" + Environment.NewLine);
                                sbReason.Append("Reason: " + adj.Reason.Coding[0].Display + Environment.NewLine);
                            }
                        }
                        breakdown.reason = sbReason.ToString();

                    }

                    breakdownList.Add(breakdown);
                }
                fhirResponseModel.breakdown = breakdownList;

                if (response.claim.ProcessNote.Count > 0)
                {
                    List<FHIRNotes> notes = new List<FHIRNotes>();
                    foreach (ClaimResponse.NoteComponent note in response.claim.ProcessNote)
                    {
                        FHIRNotes noteModel = new FHIRNotes();
                        noteModel.no = note.Number != null ? note.NumberElement.Value.ToString() : "-NA-";
                        noteModel.note = note.Text != null ? note.Text : "-NA-";
                        notes.Add(noteModel);
                    }
                    fhirResponseModel.note = notes;

                }
                if (response.claim.Error.Count > 0)
                {
                    List<FHIRError> errorList = new List<FHIRError>();
                    foreach (ClaimResponse.ErrorComponent error in response.claim.Error)
                    {
                        FHIRError errorModel = new FHIRError();
                        errorModel.code = error.Code.Coding[0].Code;
                        errorModel.display = error.Code.Coding[0].Display;
                        List<Extension> expression = error.Code.Coding[0].Extension;
                        StringBuilder sbErrors = new StringBuilder();

                        foreach (Extension ext in error.Code.Coding[0].Extension)
                        {
                            sbErrors.Append(ext.Value.ToString() + Environment.NewLine);
                        }
                        errorModel.details = sbErrors.ToString();
                        errorList.Add(errorModel);
                    }
                    fhirResponseModel.error = errorList;
                }

            }

            return fhirResponseModel;
        }

        private static FHIRRequestModel ClaimFriendlyViewerRequest(FHIRRequestModel fhirRequestModel, RequestBundleModel request)
        {
            fhirRequestModel.claim.bundleId = request.bundleId.ToString();
            fhirRequestModel.patient.patientMRN = request.patient.Identifier.Count > 0 ? request.patient.Identifier[0].Value : "-NA-";
            fhirRequestModel.patient.patientName = request.patient.Name.Count > 0 ? request.patient.Name[0].Text : "-NA-";
            fhirRequestModel.patient.gender = request.patient.GenderElement != null
                                          ? request.patient.GenderElement.Value.ToString() : "-NA-";
            fhirRequestModel.patient.dateOfBirth = request.patient.BirthDateElement != null
                                              ? request.patient.BirthDateElement
                                              .ToDateTimeOffset().ToString() : "-NA-";

            if (request.practitioner != null)
            {
                fhirRequestModel.practioner.practionerIdentifier = request.practitioner.Identifier.Count > 0
                                                              ? request.practitioner.Identifier[0].Value.ToString() : "-NA-";
                fhirRequestModel.practioner.practionerId = request.practitioner.Id.ToString();
                fhirRequestModel.practioner.practionerName = request.practitioner.Name.Count > 0 ? request.practitioner.Name[0].Text : "-NA-";
                fhirRequestModel.practioner.gender = request.practitioner.Gender != null ? request.practitioner.Gender.ToString() : "-NA-";
            }

            fhirRequestModel.payer.licenseNo = request.payer.Identifier.Count > 0 ? request.payer.Identifier[0].Value : "-NA-";
            fhirRequestModel.payer.payerName = request.payer.Name != null ? request.payer.Name : "-NA-";

            fhirRequestModel.provider.licenseNo = request.provider.Identifier.Count > 0 ? request.provider.Identifier[0].Value : "-NA-";
            fhirRequestModel.provider.providerName = request.provider.Name != null ? request.provider.Name : "-NA-";

            if (request.coverage != null)
            {
                fhirRequestModel.coverageDetail.identifier = request.coverage.Identifier.Count > 0 ? request.coverage.Identifier[0].Value : "-NA-";
                fhirRequestModel.coverageDetail.status = request.coverage.Status != null ? request.coverage.Status.ToString() : "-NA-";
                fhirRequestModel.coverageDetail.type = request.coverage.Type != null ? request.coverage.Type.Coding[0].Code : "-NA-";
                fhirRequestModel.coverageDetail.beneficiary = request.coverage.Beneficiary != null ? request.coverage.Beneficiary.Reference : "-NA-";
                fhirRequestModel.coverageDetail.relationship = request.coverage.Relationship != null ? request.coverage.Relationship.Coding[0].Code : "-NA-";
                fhirRequestModel.coverageDetail.payerName = request.coverage.Payor.Count > 0 ? request.coverage.Payor[0].Reference : "-NA-";
                fhirRequestModel.coverageDetail.payerClass = request.coverage.Class.Count > 0 ? request.coverage.Class[0].Value : "-NA-";
            }

            if (request.claim != null)
            {
                fhirRequestModel.claim.identifier = request.claim.Identifier.Count > 0 ? request.claim.Identifier[0].Value : "-NA-";
                fhirRequestModel.claim.claimId = request.claim.Id.ToString();
                fhirRequestModel.claim.insurer = request.claim.Insurer != null ? request.claim.Insurer.Reference : "-NA-";
                fhirRequestModel.claim.insurance = request.claim.Insurance.Count > 0 ? request.claim.Insurance[0].Coverage.Reference : "-NA-";
                fhirRequestModel.claim.status = request.claim.Status.ToString();
                fhirRequestModel.claim.patientName = request.claim.Patient != null ? request.claim.Patient.Reference : "-NA-";
                fhirRequestModel.claim.provider = request.claim.Provider != null ? request.claim.Provider.Reference : "-NA-";
                fhirRequestModel.claim.total = request.claim.Total != null
                                                            ? request.claim.Total.Value.ToString() + " - "
                                                            + request.claim.Total.Currency.ToString() : "-NA-";

                List<FHIRDiagnosis> diagnosisList = new List<FHIRDiagnosis>();
                if (request.claim.Diagnosis != null && request.claim.Diagnosis.Count > 0)
                {
                    foreach (Hl7.Fhir.Model.Claim.DiagnosisComponent diagnosis in request.claim.Diagnosis)
                    {
                        CodeableConcept ccDiagnosis = (CodeableConcept)request.claim.Diagnosis[0].Diagnosis;
                        FHIRDiagnosis model = new FHIRDiagnosis();
                        model.Code = ccDiagnosis.Coding[0].Code;
                        model.Type = diagnosis.Type.Count > 0 ? diagnosis.Type[0].Coding[0].Code : "-NA-";

                        diagnosisList.Add(model);
                    }
                }
                fhirRequestModel.diagnosis = diagnosisList;

                string chiefComplaint = string.Empty;
                string Systolic = string.Empty;
                string Diastolic = string.Empty;
                string Weight = string.Empty;
                string Height = string.Empty;

                if (request.claim.SupportingInfo != null
                    && request.claim.SupportingInfo.Count > 0)
                {
                    foreach (Hl7.Fhir.Model.Claim.SupportingInformationComponent info in request.claim.SupportingInfo)
                    {
                        CodeableConcept ccCategory = info?.Category;
                        if (ccCategory.Coding.Count > 0)
                        {
                            if (ccCategory.Coding[0].Code.Contains("chief-complaint"))
                            {
                                chiefComplaint += "Cheif complaint : " + Environment.NewLine + info?.Code.Text + Environment.NewLine;
                            }
                            if (ccCategory.Coding[0].Code.Contains("info"))
                            {
                                chiefComplaint += "Info : " + Environment.NewLine + info?.Value.ToString() + Environment.NewLine;
                            }
                            if (ccCategory.Coding[0].Code.Contains("vital-sign-systolic"))
                            {
                                Quantity ccValue = (Quantity)info?.Value;
                                Systolic = ccValue?.Value + " " + ccValue?.Code;
                            }
                            if (ccCategory.Coding[0].Code.Contains("vital-sign-diastolic"))
                            {
                                Quantity ccValue = (Quantity)info?.Value;
                                Diastolic = ccValue?.Value + " " + ccValue?.Code;
                            }
                            if (ccCategory.Coding[0].Code.Contains("vital-sign-weight"))
                            {
                                Quantity ccValue = (Quantity)info?.Value;
                                Weight = ccValue?.Value + " " + ccValue?.Code;
                            }
                            if (ccCategory.Coding[0].Code.Contains("vital-sign-height"))
                            {
                                Quantity ccValue = (Quantity)info?.Value;
                                Height = ccValue?.Value + " " + ccValue?.Code;
                            }
                        }
                    }
                }
                fhirRequestModel.claim.chiefComplaint = chiefComplaint;
                fhirRequestModel.claim.systolic = Systolic;
                fhirRequestModel.claim.diastolic = Diastolic;
                fhirRequestModel.claim.weight = Weight;
                fhirRequestModel.claim.height = Height;

                if (request.claim.Item.Count > 0)
                {
                    List<FHIRServices> services = new List<FHIRServices>();
                    List<Hl7.Fhir.Model.Claim.ItemComponent> claimItems = request.claim.Item;
                    List<Coding> productsOrServices = new List<Coding>();
                    foreach (Hl7.Fhir.Model.Claim.ItemComponent item in claimItems)
                    {
                        FHIRServices serviceModel = new FHIRServices();

                        List<Coding> hmgCoding = item.ProductOrService.Coding.Where(c => c.System.Contains("hmg")).ToList();
                        List<Coding> nphiesCoding = item.ProductOrService.Coding.Where(c => c.System.Contains("nphies")).ToList();

                        serviceModel.sequence = item.Sequence.ToString();
                        serviceModel.hmgCoding = hmgCoding.Count > 0 ? hmgCoding[0].Display : string.Empty;
                        serviceModel.nphiesCoding = nphiesCoding.Count > 0 ? nphiesCoding[0].Code : string.Empty;
                        serviceModel.nphiesDisplay = nphiesCoding.Count > 0 ? nphiesCoding[0].Display : string.Empty;
                        serviceModel.serviced = item.Serviced.ToString();
                        serviceModel.quantity = item.Quantity.Value.ToString();
                        serviceModel.unitPrice = item.UnitPrice.Value.ToString();
                        serviceModel.net = item.Net.Value.ToString();

                        services.Add(serviceModel);
                    }
                    fhirRequestModel.services = services;
                }
            }

            if (request.communication != null)
            {
                fhirRequestModel.communication.identifier = request.communication.Identifier.Count > 0 ? request.communication.Identifier[0].Value : "-NA-";
                fhirRequestModel.communication.status = request.communication.Status != null ? request.communication.Status.ToString() : "-NA-";
                fhirRequestModel.communication.category = request.communication.Category.Count > 0 ? request.communication.Category[0].Coding[0].Code : "-NA-";
                fhirRequestModel.communication.priority = request.communication.Priority != null ? request.communication.Priority.ToString() : "-NA-";
                fhirRequestModel.communication.subject = request.communication.Subject != null ? request.communication.Subject.Reference.ToString() : "-NA-";
                fhirRequestModel.communication.about = request.communication.Category.Count > 0 ? request.communication.About[0].Type + " | " + request.communication.About[0].Identifier.Value : "-NA-";
                fhirRequestModel.communication.recepient = request.communication.Recipient.Count > 0 ? request.communication.Recipient[0].Reference : "-NA-";
                fhirRequestModel.communication.sender = request.communication.Sender != null ? request.communication.Sender.Reference.ToString() : "-NA-";
                fhirRequestModel.communication.reasonCode = request.communication.ReasonCode.Count > 0 ? request.communication.ReasonCode[0].Coding[0].Code : "-NA-";
                fhirRequestModel.communication.payload = request.communication.Payload.Count > 0 ? request.communication.Payload[0].Content.ToString() : string.Empty;
            }

            return fhirRequestModel;
        }

        public async Task<ResponseBundleModel> GetClaimResponseFriendlyViewer(string claimIdentifier)
        {
            Guid IdGuid;
            ResponseBundleModel responseModel = new ResponseBundleModel();
            ClaimRequestModelMD claimRequest = new ClaimRequestModelMD();
            try
            {
                bool isValidGuid = Guid.TryParse(claimIdentifier, out IdGuid);
                if (isValidGuid)
                {
                    mongoDb = client.GetDatabase(MongodbName);
                    ClaimRequestCollection = mongoDb.GetCollection<ClaimRequestModelMD>("ClaimRequest");

                    claimRequest = await LoadDocument(IdGuid);
                    Hl7.Fhir.Model.Bundle requestBundle = Parser.ParseBundle(claimRequest.Requestjson.ToString());
                    responseModel = Parser.GetNPHIESResponseModel(requestBundle);
                }
            }
            catch (Exception ex) { throw ex; }
            return responseModel;
        }

        public async Task<ClaimRequestModelMD> GetCommunicationLog(string claimIdentifier)
        {
            Guid IdGuid;
            BsonDocument document = new BsonDocument();
            ClaimRequestModelMD viewer = new ClaimRequestModelMD();
            FHIRRequestModel fhirRequestModel = new FHIRRequestModel();
            try
            {
                bool isValidGuid = Guid.TryParse(claimIdentifier, out IdGuid);
                if (isValidGuid)
                {

                    mongoCollection = mongoDatabase.GetCollection<T>("Communications");

                    document = await GenericLoadDocument("ClaimIdentifier", IdGuid, mongoCollection);
                    if (document != null)
                    {
                        Hl7.Fhir.Model.Bundle requestBundle = Parser.ParseBundle(document.GetValue("Request").ToString());
                        RequestBundleModel request = Parser.GetNPHIESRequestModel(requestBundle);

                        if (request != null)
                        {
                            viewer.friendlyViewer.fhirRequestModel = ClaimFriendlyViewerRequest(fhirRequestModel, request);
                            viewer.Requestjson = document.GetValue("Request").ToString();
                        }
                        if (!string.IsNullOrWhiteSpace(document.GetValue("Response").ToString()))
                        {
                            viewer.ResponseJson = document.GetValue("Response").ToString();
                        }
                    }
                }
            }
            catch (Exception ex) { throw ex; }
            return viewer;
        }
        public async Task<bool> UpdatePollResponse(long claimId)
        {
            bool result = false;
            try
            {
                var claimDetails = await GetClaimDetailsAsync(claimId);
                var bundle = await this.GetClaimRequestLog(claimDetails.ClaimIdentifier);
                var parsedObject = Hl7Parser.ParseBundle(bundle.Requestjson);

                var claimitemResponse =this.GetPollResponse(parsedObject, out var responseBundle, claimDetails.FacilityId);

                if (claimitemResponse != null && claimitemResponse.ClaimDetailResponse != null && claimitemResponse.ClaimDetailResponse.Count > 0)
                {
                    ClaimRequestModel crmodel = Hl7Parser.ParseRequestJsonBundle(bundle.Requestjson, claimDetails.ClaimIdentifier);
                    await this.UpdateProcedureSeqAsync(crmodel,claimDetails.ProcessId,claimDetails.CreatedBy,claimDetails.CriteriaType,false,claimitemResponse);
                }
                if (claimitemResponse != null)
                {
                        var respbundle = (Bundle)claimitemResponse.ClaimDetailResponse[0].ResponseBundle;

                        await this.UpdateClaimRequest(parsedObject, responseBundle, claimitemResponse.ClaimDetailResponse[0].Status,
                            claimitemResponse.ClaimDetailResponse[0].ClaimIdentifier);
                        result = true;
                }
            }
            catch (Exception ex)
            {
                result = false;
            }

            return result;
        }

        public async Task<PollResponseDTO> GetClaimDetailsAsync(long claimId)
        {
            var claimDetails = await (from claim in _dbContext.RcmClaims
                                      join ProcessQueue in _dbContext.RcmNphiesprocessQueues
                                      on claim.ProcessId equals ProcessQueue.ProcessId into NphiesProcessQueue
                                      from ProcessQueue in NphiesProcessQueue.DefaultIfEmpty()
                                      where claim.ClaimId == claimId
                                      && claim.OrganizationId == ProcessQueue.OrganizationId
                                      select new PollResponseDTO
                                      {
                                          ClaimIdentifier = claim.ClaimIdentifier,
                                          FacilityId = claim.FacilityId,
                                          ProcessId = ProcessQueue.ProcessId,
                                          CriteriaType = ProcessQueue.CriteriaType,
                                          CreatedBy = ProcessQueue.CreatedBy,
                                      })
                                    .AsNoTracking()
                                    .FirstOrDefaultAsync();

            return claimDetails;
        }
        public BatchResponse GetPollResponse(Bundle bundle, out Bundle responseBundle, int facilityId)
        {
            responseBundle = null;
            var att = new Attachment();
            try
            {
                var jsonSer = new FhirJsonSerializer();
                var json = jsonSer.SerializeToString(bundle);

                var restResponse = CallNPHIES(json, facilityId);

                if (restResponse.StatusCode == HttpStatusCode.OK)
                {
                    var fhirJsonParser = new FhirJsonParser();
                    var parsedObject = fhirJsonParser.Parse(restResponse.Content);

                    var responseParsing = new ResponseParsing();
                    if (parsedObject.TypeName == "Bundle")
                    {
                        responseBundle = (Hl7.Fhir.Model.Bundle)parsedObject;
                        return responseParsing.CreateResponse(responseBundle, bundle);
                    }
                    else if (parsedObject.TypeName == "OperationOutcome")
                    {
                        return responseParsing.CreateResponseObjectFromRequestForOperationOutcome(bundle, restResponse.Content);
                    }
                    else
                    {
                        return responseParsing.CreateResponseObjectFromRequestForOperationOutcome(bundle, "Response of Type: " + parsedObject.TypeName + " Received From NPHIES");
                    }
                }
                else
                {
                    if (string.IsNullOrEmpty(restResponse.Content))
                    {
                        var responseObjectJson = Newtonsoft.Json.JsonConvert.SerializeObject(restResponse);
                        var responseParsing = new ResponseParsing();
                        return responseParsing.CreateResponseObjectFromRequestForOperationOutcome(bundle, "Timeout Exception From NPHIES");
                    }
                    else
                    {
                        var fhirJsonParser = new FhirJsonParser();
                        var parsedObject = fhirJsonParser.Parse(restResponse.Content);

                        var responseParsing = new ResponseParsing();
                        return responseParsing.CreateResponseObjectFromRequestForOperationOutcome(bundle, restResponse.Content);
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }


        private IRestResponse CallNPHIES(string RequestJson, int facilityId)
        {
            try
            {
                var NPHIES_URL = config["GetPollResponse:NPHIES_URL"];
                var NPHIES_UserName = config["GetPollResponse:NPHIES_UserName"];
                var NPHIES_Password = config["GetPollResponse:NPHIES_Password"];

                var client = new RestClient(NPHIES_URL);

                // Get projectId from database based on facilityId
                var projectId = GetProjectIdFromFacilityId(facilityId);
                var (certificateBytes, certificatePassword) = GetNphiesCertificateDynamic(projectId);

                // Validate certificate
                if (certificateBytes == null || certificateBytes.Length == 0)
                {
                    loggingBroker.LogWarning($"Certificate not available for facility {facilityId}, project {projectId}. Falling back to old method.");
                    // Fallback to old method if dynamic certificate retrieval fails
                    var (certPath, certPassword) = GetNphiesCertificateFromConfig(facilityId);
                    if (!string.IsNullOrEmpty(certPath))
                    {
                        certificateBytes = System.IO.File.ReadAllBytes(certPath);
                        certificatePassword = certPassword;
                    }
                    else
                    {
                        throw new InvalidOperationException($"Certificate not available for facility {facilityId}");
                    }
                }

                ServicePointManager.Expect100Continue = true;
                ServicePointManager.DefaultConnectionLimit = 9999;
                ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls | SecurityProtocolType.Tls11 | SecurityProtocolType.Tls12;

                X509Certificate2 certificate = new X509Certificate2(certificateBytes, certificatePassword);
                client.ClientCertificates = new X509CertificateCollection() { certificate };

                loggingBroker.LogInfo($"Certificate loaded for facility {facilityId} - Subject: {certificate.Subject}, HasPrivateKey: {certificate.HasPrivateKey}");

                client.Proxy = new WebProxy();
                var restrequest = new RestRequest(Method.POST);
                restrequest.Method = Method.POST;
                restrequest.AddHeader("Username", NPHIES_UserName);
                restrequest.AddHeader("Password", NPHIES_Password);
                restrequest.AddHeader("Accept", ContentType.JSON_CONTENT_HEADER);
                restrequest.AddHeader("Content-Type", ContentType.JSON_CONTENT_HEADER);
                restrequest.AddHeader("Cache-Control", "no-cache");
                restrequest.Timeout = 90000;
                restrequest.AddParameter(ContentType.JSON_CONTENT_HEADER, RequestJson, ParameterType.RequestBody);

                return client.Execute(restrequest);
            }
            catch (Exception ex)
            {
                loggingBroker.LogError($"CallNPHIES error for facility {facilityId}: {ex.Message}");
                throw;
            }
        }

        private string GetProjectIdFromFacilityId(int facilityId)
        {
            try
            {
                var facility = _dbContext.RcmFacilities
                    .AsNoTracking()
                    .FirstOrDefault(f => f.FacilityId == facilityId);

                return facility?.ExternalCode ?? string.Empty;
            }
            catch (Exception ex)
            {
                loggingBroker.LogError($"Error getting project ID for facility {facilityId}: {ex.Message}");
                return string.Empty;
            }
        }

        private (byte[] certificateBytes, string certificatePassword) GetNphiesCertificateDynamic(string projectId)
        {
            try
            {
                if (string.IsNullOrEmpty(projectId))
                {
                    loggingBroker.LogWarning("ProjectId is empty, cannot retrieve certificate dynamically");
                    return (null, string.Empty);
                }

                var certificateFactory = new CertificateManagerFactory();
                var client = (NphiesCertificates.Enums.Clients)_clientId;

                if (ClientConfiguration.SupportedClients().Contains(client))
                {
                    var certificateManager = certificateFactory.Create(projectId, client);
                    
                    if (certificateManager != null)
                    {
                        var certBytes = certificateManager.GetCertificate();
                        var certPassword = certificateManager.GetCertificatePassword();
                        
                        loggingBroker.LogInfo($"Certificate retrieved dynamically for project {projectId}, client {client}");
                        return (certBytes, certPassword);
                    }
                    else
                    {
                        loggingBroker.LogWarning($"Certificate manager returned null for project {projectId}, client {client}");
                        return (null, string.Empty);
                    }
                }
                else
                {
                    loggingBroker.LogWarning($"Client {_clientId} is not in supported clients list");
                    return (null, string.Empty);
                }
            }
            catch (Exception ex)
            {
                loggingBroker.LogError($"Error retrieving certificate dynamically for project {projectId}: {ex.Message}");
                return (null, string.Empty);
            }
        }

        private (string certificatePath, string certificatePassword) GetNphiesCertificateFromConfig(int facilityId)
        {
            try
            {
                var nphiesCertificateConfig = config.GetSection("NphiesCertificate");
                var nphiesCertificates = nphiesCertificateConfig.Get<List<NphiesCertificateConfiguration>>();
                
                if (nphiesCertificates == null || !nphiesCertificates.Any())
                {
                    loggingBroker.LogWarning("NphiesCertificate configuration section is empty or not found");
                    return (string.Empty, string.Empty);
                }

                var certificate = nphiesCertificates.FirstOrDefault(cert => cert.FacilityId == facilityId);

                if (certificate != null)
                {
                    return (certificate.Path.ToString(), certificate.Password.ToString());
                }

                loggingBroker.LogWarning($"Certificate not found in config for facility {facilityId}");
                return (string.Empty, string.Empty);
            }
            catch (Exception ex)
            {
                loggingBroker.LogError($"Error reading certificate from config for facility {facilityId}: {ex.Message}");
                return (string.Empty, string.Empty);
            }
        }
        public async System.Threading.Tasks.Task UpdateClaimRequest(Bundle claimBundle, Bundle claimResponseBundle, string status, string claimIdentifier)
        {
            try
            {
                var jsonSer = new FhirJsonSerializer();
                string jsonRes = jsonSer.SerializeToString(claimResponseBundle);
                await this.UpdateClaimRequestCollection(claimIdentifier, jsonRes, status);
            }
            catch (Exception ex)
            {
                throw ex;
            }     
        }
        public async System.Threading.Tasks.Task UpdateClaimRequestCollection(string id, string responseJson, string status)
        {
            try
            {
                if (Guid.TryParse(id, out Guid idGuid))
                {
                    await UpdateDocumentByIdResponseJson(idGuid, responseJson, status);
                }
            }
            catch (Exception ex) { throw ex; }
        }
        public async Task<UpdateResult> UpdateDocumentByIdResponseJson( Guid id, string responseRequest, string status)
        {
            var collection = mongoDb.GetCollection<ClaimRequestModelMD>("ClaimRequest");

            var filter = Builders<ClaimRequestModelMD>.Filter.Eq("Id", id);
            var update = Builders<ClaimRequestModelMD>.Update
                                .Set(x => x.ResponseJson, responseRequest)
                                .Set(x => x.ModifiedOn, DateTime.Now);

            if (!string.IsNullOrEmpty(status))
            {
                update = update.Set(x => x.Status, status);
            }

            return await collection.UpdateOneAsync(filter, update);
        }
        public async Task<bool> UpdateProcedureSeqAsync(ClaimRequestModel claimBundleRequest, long processId, int submittedBy,
       int criteriaType = 0, bool isEpisode = false, Models.BatchResponse batchResponse = null)
        {
            bool result = false;

            try
            {
                ClaimResponseRequest claimReq = new Models.ClaimResponseRequest
                {
                    ProcessId = processId,
                    CriteriaType = criteriaType,
                    SubmitedBy = submittedBy,
                    ClaimRequestModel = claimBundleRequest,
                    ClaimResponseModel = batchResponse != null ? new List<Models.BatchResponse> { batchResponse } : null
                };

                result = await _claimService.UpdateClaimStatus(claimReq);
            }
            catch (Exception ex)
            {
            }

            return result;
        }



        private async Task<BsonDocument> GenericLoadDocument(string colName, Guid claimIdentifier, IMongoCollection<T> mongoCollection)
        {
            var documents = await mongoCollection.Find(new BsonDocument(colName, claimIdentifier.ToString()))
                            .SortBy(bson => bson["CreatedOn"])
                            .ThenByDescending(bson => bson["CreatedOn"]).ToListAsync();
            return documents.FirstOrDefault();
        }
    }
}
