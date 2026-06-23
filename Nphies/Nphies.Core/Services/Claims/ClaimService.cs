using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using MongoDB.Driver;
using Newtonsoft.Json.Linq;
using Nphies.Core.Data.Entities;
using Nphies.Core.DTOs;
using MongoDB.Driver.Linq;
using Nphies.Core.Helper;
using Nphies.Core.Models;
using Nphies.Core.Models.Cancellations;
using Nphies.Core.Models.Submission.SameBundles.Claims;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using System.Text.RegularExpressions;
using System.IO;
using Nphies.Core.Models.Claims;
using Clinic = Nphies.Core.Models.Clinic;
using System.Runtime.CompilerServices;
using Nphies.Core.Models.Services;
using Nphies.Core.Brokers.Loggings;
using Microsoft.Data.SqlClient;
using Hl7.FhirPath.Sprache;
using RestSharp.Extensions;
using System.Text.Json;
using Cyclus.Storage.Attachment.Enums;
using Nphies.Core.Models.Diagnoses;
using MongoDB.Driver.Core.Operations;

namespace Nphies.Core.Services.Claims
{
    public partial class ClaimService : IClaimService
    {
        private readonly ZyklusCoreContext context;
        private readonly IConfiguration config;
        private readonly IMemoryCache memoryCache;
        private readonly ILoggingBroker logger;
        private readonly List<string> serviceCodes = new List<string>() { "05005009" };
        private readonly IClaimUpdate _claimUpdate;
        public ClaimService(ZyklusCoreContext context,
            IConfiguration _config,
            IMemoryCache memoryCache,
            ILoggingBroker logger,
            IClaimUpdate claimUpdate)
        {
            this.context = context;
            this.config = _config;
            this.memoryCache = memoryCache;
            this.logger = logger;
            _claimUpdate = claimUpdate;
        }

        public ValueTask<List<ClaimDetail>> GetClaims
        (
            int orginzationId,
            int facilityId,
            string adaptorCode,
            DateTime dtfrom,
            DateTime dtTo,
            long processId,
            int batchsize,
            string PayerId,
            bool extend
        )
        => TryCatch(async () =>
        {
            Validate();
            //Validate(orginzationId, facilityId, adaptorCode, dtfrom, dtTo, batchsize);

            return await ClaimDetailsAsync(orginzationId, facilityId, adaptorCode, dtfrom, dtTo, processId, batchsize, PayerId, extend);
        });

        public ValueTask<List<ClaimDetail>> GetClaimsByProcess
        (
           int orginzationId,
           int facilityId,
           string adaptorCode,
           long processId,
           int batchsize,
           string PayerId
        )
        => TryCatch(async () =>
        {
            Validate();
            return await ClaimDetailsAsync(orginzationId, facilityId, adaptorCode, processId, batchsize, PayerId);
        });
        public ValueTask<bool> UpdateClaimStatus(ClaimResponseRequest claimResponseRequest)
        => TryCatch(async () =>
        {
            Validate();
            return await UpdateClaimAsync(claimResponseRequest);
        });

        public ValueTask<bool> UpdatePoolingResponse(ClaimUpdateModel claimPoolingRequest)
        => TryCatch(async () =>
        {
            Validate();
            return await ClaimPoolingAsync(claimPoolingRequest);
        });
        public async Task<bool> ClaimPoolingAsync(ClaimUpdateModel claimPoolingRequest)
        {
            bool response = false;
            //if (claimPoolingRequest != null)
            //   await AddClaimSubmissionResponse(claimPoolingRequest);
            //return response;
            try
            {
                if (claimPoolingRequest != null)
                {
                    //var claim = await context.RcmClaims
                    //    .Where(claim => claim.OrganizationId == claimPoolingRequest.OrganizationID
                    //                            && claim.ClaimIdentifier == claimPoolingRequest.ClaimIdentifier
                    //                              && claim.Status != (byte)Status.WriteOff
                    //    ).FirstOrDefaultAsync();
                    var claim = await context.RcmClaims
                        .Where(claim => claim.ClaimIdentifier == claimPoolingRequest.ClaimIdentifier
                                        && claim.Status != (byte)Status.WriteOff
                        ).FirstOrDefaultAsync();

                    if (claim != null)
                    {
                        var services = await context.RcmClaimServicesDetails
                                .Where(service => service.OrganizationId == claim.OrganizationId
                                       && service.ClaimId == claim.ClaimId
                                       && (service.IsDeleted == null || service.IsDeleted == false)
                                       && (service.IsRefund == null || service.IsRefund == false)
                                       && (service.IsReturn == null || service.IsReturn == false)
                                       && (service.NphiesSeqNo != null)
                                ).ToListAsync();
                        if (!claim.ResponseReceivedOn.HasValue)
                        {
                            claim.ResponseReceivedOn = DateTime.Now;
                        }
                        claim.pollResponseReceivedOn = DateTime.Now;
                        //claim.NphieseRemarks = string.Empty;
                        claim.IsPendedReceived = true;
                        claim.ResponseBundleId = IsString(claimPoolingRequest.ResponseBundleID);

                        if (claimPoolingRequest.Status > 0)
                        {
                            //claim.Status = Convert.ToByte(claimPoolingRequest.Status);
                            if (Convert.ToByte(claimPoolingRequest.Status)
    == (byte)ClaimStatus.Nphies_Queued && claimPoolingRequest.IsPended.HasValue && claimPoolingRequest.IsPended.Value)
                            {
                                claim.Status = (byte)ClaimStatus.Nphies_Pended;
                            }
                            else
                            {
                                claim.Status = Convert.ToByte(claimPoolingRequest.Status);
                            }
                        }
                        else if (claimPoolingRequest.IsPended.HasValue && claimPoolingRequest.IsPended.Value)
                        {
                            claim.Status = (byte)ClaimStatus.Nphies_Pended;
                        }

                        if (claim.IsPendedReceived.HasValue && !claim.IsPendedReceived.Value
                            && claimPoolingRequest.IsPended.HasValue && claimPoolingRequest.IsPended.Value)
                        {
                            claim.IsPendedReceived = true;
                        }

                        if (!string.IsNullOrEmpty(claimPoolingRequest.Remarks))
                            claim.NphieseRemarks = claimPoolingRequest.Remarks.Trim();

                        var statusList = new List<byte>();

                        if (claimPoolingRequest.ClaimItems != null
                            && claimPoolingRequest.ClaimItems.Count > 0)
                        {
                            claim.CommunicationUrl = IsString(claimPoolingRequest.CommunicationUrl);
                            claim.CommunicationIdentifier = IsString(claimPoolingRequest.CommunicationIdentifier);
                            claim.TotalApproved = IsString(claimPoolingRequest.TotalApproved);


                            foreach (var claimItems in claimPoolingRequest.ClaimItems)
                            {
                                var service = services.FirstOrDefault(service => service.NphiesSeqNo == claimItems.ItemSequence);

                                if (service != null)
                                {
                                    // service.ReSubmissionStatus = null;
                                    //service.Status = Convert.ToByte(claimItems.Status);


                                    if (!string.IsNullOrWhiteSpace(claimItems.SubmittedQuantity))
                                        service.SubmittedQuantity = claimItems.SubmittedQuantity;

                                    if (!string.IsNullOrWhiteSpace(claimItems.ApprovedQuantity))
                                        service.ApprovedQuantity = GetApprovedQuantity(claimItems.ApprovedQuantity);
                                    else
                                        service.ApprovedQuantity = 0;

                                    service.ReasonCode = claimItems.ReasonCode;
                                    service.ItemReason = claimItems.ItemReason;
                                    if (!string.IsNullOrWhiteSpace(claimItems.SubmittedAmount))
                                        service.SubmittedAmount = claimItems.SubmittedAmount;

                                    service.ApprovedAmount = !string.IsNullOrWhiteSpace(claimItems.BenefitAmount)
                                            ? Convert.ToDouble(claimItems.BenefitAmount) : 0;

                                    service.BenefitAmount = claimItems.BenefitAmount.ToString();
                                    service.TaxApproved = Convert.ToDecimal(claimItems.CompanyTax);

                                    service.ResponseReceivedOn = DateTime.Now;

                                    if (Convert.ToByte(claimItems.Status) == (byte)ClaimStatus.Nphies_Rejected)
                                    {
                                        service.ReSubmissionStatus = null;
                                        service.Status = Convert.ToByte(claimItems.Status);
                                        service.ProcessId = null;


                                        var addresponse = await context.RcmAddResponseBindings
                                                                .Where(ad => ad.RowId == service.RowId && ad.IsActive == true)
                                                                .FirstOrDefaultAsync();
                                        if (addresponse != null)
                                        {
                                            addresponse.IsActive = false;
                                            addresponse.ModifiedBy = 777;
                                            addresponse.ModifiedOn = DateTime.Now;
                                            addresponse.Description = "Polling Update";
                                        }
                                    }
                                    else if (Convert.ToByte(claimItems.Status) == (byte)ClaimStatus.Nphies_Partial)
                                    {
                                        service.ReSubmissionStatus = null;
                                        service.Status = Convert.ToByte(claimItems.Status);
                                        var addresponse = await context.RcmAddResponseBindings
                                                          .Where(ad => ad.RowId == service.RowId && ad.IsActive == true)
                                                          .FirstOrDefaultAsync();
                                        if (addresponse != null)
                                        {
                                            addresponse.IsActive = false;
                                            addresponse.ModifiedBy = 777;
                                            addresponse.ModifiedOn = DateTime.Now;
                                            addresponse.Description = "Polling Updated To Completed";
                                        }
                                    }
                                    else if (Convert.ToByte(claimItems.Status) == (byte)ClaimStatus.Nphies_Approved)
                                    {
                                        service.ReSubmissionStatus = null;
                                        service.Status = Convert.ToByte(claimItems.Status);
                                        var addresponse = await context.RcmAddResponseBindings
                                                          .Where(ad => ad.RowId == service.RowId && ad.IsActive == true)
                                                          .FirstOrDefaultAsync();
                                        if (addresponse != null)
                                        {
                                            addresponse.IsActive = true;
                                            addresponse.ModifiedBy = 777;
                                            addresponse.ModifiedOn = DateTime.Now;
                                            addresponse.Description = "Polling Updated To Approved";
                                        }
                                    }
                                    else
                                    {
                                        //service.ReSubmissionStatus = null;
                                        service.Status = Convert.ToByte(claimItems.Status);
                                    }


                                    statusList.Add(service.Status.Value);


                                }


                            }
                        }

                        // Approved
                        var approvedServices = statusList.Where(s => s == (byte)ClaimStatus.Nphies_Approved).ToList();

                        // Rejected
                        var rejectedServices = statusList.Where(s => s == (byte)ClaimStatus.Nphies_Rejected).ToList();

                        // Partial 
                        var partialServices = statusList.Where(s => s == (byte)ClaimStatus.Nphies_PartialApproved).ToList();

                        if (claim.Status != (byte)ClaimStatus.Nphies_Approved
                            && approvedServices.Count() > 0 && approvedServices.Count() == statusList.Count())
                        {
                            claim.StatusOld = claim.Status;
                            claim.Status = (byte)ClaimStatus.Nphies_Approved;
                        }
                        else if (claim.Status != (byte)ClaimStatus.Nphies_Rejected
                             && rejectedServices.Count() > 0 && rejectedServices.Count() == statusList.Count())
                        {
                            claim.StatusOld = claim.Status;
                            claim.Status = (byte)ClaimStatus.Nphies_Rejected;
                        }
                        else if (claim.Status != (byte)ClaimStatus.Nphies_PartialApproved
                            && partialServices.Count() >= 1)
                        {
                            claim.StatusOld = claim.Status;
                            claim.Status = (byte)ClaimStatus.Nphies_PartialApproved;
                        }
                        else if (claim.Status != (byte)ClaimStatus.Nphies_PartialApproved
                            && approvedServices.Count() >= 1 && rejectedServices.Count() >= 1)
                        {
                            claim.StatusOld = claim.Status;
                            claim.Status = (byte)ClaimStatus.Nphies_PartialApproved;
                        }

                    }
                    else
                    {
                        //TODO: Identifier not found log the identifier Id's
                    }

                    await UpdateClaimPostTrailStatus(claimPoolingRequest, claim);

                    if (await context.SaveChangesAsync() > 0)
                    {
                        await AddClaimSubmissionResponse(claimPoolingRequest);
                        response = true;
                    }

                }
            }
            catch (Exception ex)
            {

            }


            return response;
        }

        private async Task UpdateClaimPostTrailStatus(ClaimUpdateModel claimPoolingRequest, RcmClaim claim)
        {
            var postTrailLogs = await context.ClaimNphiesPostTrails
                .Where(log => log.ClaimIdentifier == claimPoolingRequest.ClaimIdentifier
                           && log.OrganizationId == claimPoolingRequest.OrganizationID)
                .ToListAsync();

            if (postTrailLogs != null && postTrailLogs.Any() && claim.Status != null)
            {
                string statusName = GetStatusName(claim.Status);
                foreach (var postTrailLog in postTrailLogs)
                {
                    postTrailLog.NphiesStatus = statusName;
                }
            }
        }

        public async Task<bool> AddClaimSubmissionResponse(ClaimUpdateModel claimSubmissionResponse)
        {
            // Validate claim exists
            var claim = await context.RcmClaims
                .FirstOrDefaultAsync(c => c.OrganizationId == claimSubmissionResponse.OrganizationID
                    && c.ClaimIdentifier == claimSubmissionResponse.ClaimIdentifier);

            if (claim == null)
            {
                return false; // Claim not found
            }

            // Ensure TotalApproved is not null or empty
            if (claimSubmissionResponse.TotalApproved == null || !claimSubmissionResponse.TotalApproved.Any())
            {
                return false; // No amounts to process
            }

            // Fetch existing responses ordered by ReceivedOn, with active responses only
            var existingResponses = context.RcmClaimSubmissionResponses
                .Where(r => r.ClaimIdentifier == claimSubmissionResponse.ClaimIdentifier)
                .OrderByDescending(r => r.ReceivedOn)
                .ToList();

            decimal totalApprovedAmount = Convert.ToDecimal(claimSubmissionResponse.TotalApproved); // Incoming final amount

            // Check if any active response matches the incoming amount
            var activeResponses = existingResponses.Where(r => r.IsActive == true).ToList();

            if (activeResponses.Any(r => r.ReceivedAmount == totalApprovedAmount))
            {
                // If a match is found, deactivate all previous records
                foreach (var response in existingResponses)
                {
                    response.IsActive = false;
                }
                await context.SaveChangesAsync();

                // Insert the new record with the matching amount
                var newClaimResponse = new RcmClaimSubmissionResponse
                {
                    ClaimIdentifier = claimSubmissionResponse.ClaimIdentifier,
                    ResponseBundleId = claimSubmissionResponse.ResponseBundleID,
                    ClaimBundleId = claim.ClaimBundleId,
                    ReceivedAmount = totalApprovedAmount,  // Insert the approved amount
                    Status = (byte)claimSubmissionResponse.Status,
                    IsActive = true,
                    ReceivedOn = DateTime.Now,
                    FacilityId = claim.FacilityId,
                    OrganizationId = claim.OrganizationId,
                    ClaimId = claim.ClaimId,
                    SubmittedAmount = Convert.ToDecimal(claim.TotalSubmitted)
                };

                await context.RcmClaimSubmissionResponses.AddAsync(newClaimResponse);
                await context.SaveChangesAsync();

                return true; // Success
            }

            // Calculate the total of active responses
            decimal totalReceivedAmount = (decimal)activeResponses.Sum(r => r.ReceivedAmount);

            // Calculate the difference between the incoming amount and the total received amount
            decimal amountDifference = totalApprovedAmount - totalReceivedAmount;

            // If the incoming amount is less than the difference, insert the lesser amount
            if (amountDifference < 0)
            {
                foreach (var response in existingResponses)
                {
                    response.IsActive = false; // Deactivate all previous responses
                }
                await context.SaveChangesAsync();

                var newClaimResponse = new RcmClaimSubmissionResponse
                {
                    ClaimIdentifier = claimSubmissionResponse.ClaimIdentifier,
                    ResponseBundleId = claimSubmissionResponse.ResponseBundleID,
                    ClaimBundleId = claim.ClaimBundleId,
                    ReceivedAmount = totalApprovedAmount,  // Insert the incoming amount since it's less
                    Status = (byte)claimSubmissionResponse.Status,
                    IsActive = true,
                    ReceivedOn = DateTime.Now,
                    FacilityId = claim.FacilityId,
                    OrganizationId = claim.OrganizationId,
                    ClaimId = claim.ClaimId,
                    SubmittedAmount = Convert.ToDecimal(claim.TotalSubmitted)
                };

                await context.RcmClaimSubmissionResponses.AddAsync(newClaimResponse);
                await context.SaveChangesAsync();

                return true; // Success
            }

            // If the amountDifference is positive, insert the difference
            if (amountDifference > 0)
            {
                var newClaimResponse = new RcmClaimSubmissionResponse
                {
                    ClaimIdentifier = claimSubmissionResponse.ClaimIdentifier,
                    ResponseBundleId = claimSubmissionResponse.ResponseBundleID,
                    ClaimBundleId = claim.ClaimBundleId,
                    ReceivedAmount = amountDifference,  // Insert the difference
                    Status = (byte)claimSubmissionResponse.Status,
                    IsActive = true,
                    ReceivedOn = DateTime.Now,
                    FacilityId = claim.FacilityId,
                    OrganizationId = claim.OrganizationId,
                    ClaimId = claim.ClaimId,
                    SubmittedAmount = Convert.ToDecimal(claim.TotalSubmitted)
                };

                await context.RcmClaimSubmissionResponses.AddAsync(newClaimResponse);
                await context.SaveChangesAsync();
            }

            return true; // Success after processing all rules
        }

        private async ValueTask<List<ClaimDetail>> ClaimDetailsAsync
        (
            int organizationId,
            int facilityId,
            string adaptorCode,
            DateTime dateFrom,
            DateTime dateTo,
            long processId,
            int batchSize,
            string PayerId,
            bool extend
        )
        {
            try
            {
                string providerLicense = string.Empty;
                List<ClaimDetail> claimDetails = new List<ClaimDetail>();

                #region Master Record
                //if claim is extended inside this method will fetch related resubmission data
                var claims = await RetriveClaimsAsync(
                    organizationId,
                    facilityId,
                    dateFrom, dateTo,
                    processId,
                    batchSize,
                    Convert.ToInt32(PayerId),
                    extend
                    );


                #endregion

                if (claims != null && claims.Count > 0)
                {
                    RcmFacility facility = (await RetriveFacilitiesAsync(organizationId, facilityId));

                    IQueryable<RcmServiceCatalog> serviceCatelog = context.RcmServiceCatalogs;
                    RcmStandardCode[] standardCodes = await RetriveStandardAsync(organizationId, facilityId);
                    RcmClinic[] clinics = await RetriveClinicsAsync(organizationId, facilityId);

                    foreach (var claim in claims)
                    {
                        try
                        {
                            logger.LogInfo($"Retrived Claim Id # {claim.ClaimId}");

                            JArray ComponentServices = RetriveComponentInvoices(organizationId, claim);

                            JArray jlaboratory = RetriveLabResultsFromMedicalFile(claim.MedicalJsonData);
                            var claimservices = await ClaimServices(organizationId, claim, serviceCatelog,
                                    standardCodes,
                                    ComponentServices,
                                    jlaboratory);

                            if (claimservices != null && claimservices.Count == 0)
                            {
                                await UpdateClaimStatusByClaimId(claim.ClaimId, StringConstant.InternalError + $"No Services Found", (byte)ClaimStatus.InternalError);
                                continue;
                            }

                            string messageBundleId = Guid.NewGuid().ToString();
                            //Claim
                            ClaimDetail claimDetail = ClaimModel(claim, clinics, messageBundleId
                                    , await RetriveMaxSubmissionCount(claim.ClaimId)
                                    , PayerIdEnableAlais(claim));

                            if (claimDetail is null)
                                continue;

                            //Services
                            claimDetail.ClaimItems = claimservices;

                            //Diagnosis
                            RcmPayer claimPayersData = await RetrivePayers(organizationId, PayerId).FirstOrDefaultAsync();
                            if (facility != null && facility.Shadowbillingenable.HasValue && facility.Shadowbillingenable.Value)
                            {
                                if (claimPayersData != null && claimPayersData.Shadowbillingenable.HasValue && claimPayersData.Shadowbillingenable.Value)
                                {
                                    if (await CheckCoDskEncounterDiagnosis(claim))
                                    {
                                        claimDetail.Diagnoses = await GetCoDskEncounterDiagnosis(claim);
                                    }
                                    else
                                    {
                                        if (claim.RcmClaimDiagnoses != null
                                           && claim.RcmClaimDiagnoses.Count > 0)
                                        {
                                            claimDetail.Diagnoses = ClaimDiagnosis(claim);
                                        }
                                    }
                                }
                                else
                                {
                                    if (claim.RcmClaimDiagnoses != null
                                   && claim.RcmClaimDiagnoses.Count > 0)
                                    {
                                        claimDetail.Diagnoses = ClaimDiagnosis(claim);
                                    }
                                }
                            }
                            else
                            {
                                if (claim.RcmClaimDiagnoses != null
                                   && claim.RcmClaimDiagnoses.Count > 0)
                                {
                                    claimDetail.Diagnoses = ClaimDiagnosis(claim);
                                }
                            }


                            if ((claimDetail.Diagnoses is null) || (claimDetail.Diagnoses != null && claimDetail.Diagnoses.Count == 0))
                            {
                                await UpdateClaimStatusByClaimId(claim.ClaimId, StringConstant.DiagnosisRequired, (byte)ClaimStatus.InternalError);
                                continue;
                            }
                            //Vitalsigns and chief Complaint
                            claimDetail.VitalSign = ClaimVitalSign(claim);

                            //Encounters

                            claimDetail.Encounter = ClaimEncounters(
                                    claim,
                                    await GetPayerEntity(claim),//.Where(x => x.PayerId == claim.PayerId).FirstOrDefault(),
                                    GetEncounterStartDateNew(claim),
                                    GetEncounterEndDateNew(claim)
                                    );


                            //Payers
                            var claimPayers = await ClaimPayers(claim, adaptorCode);
                            if (claimPayers != null && !string.IsNullOrWhiteSpace(claimPayers.License))
                                claimDetail.Payer = claimPayers;
                            else
                            {
                                await UpdateClaimStatusByClaimId(claim.ClaimId, "payer license is not found", (byte)ClaimStatus.InternalError);
                                continue;
                            }

                            //Providers
                            var provider = await ClaimProviders(claim, facility, adaptorCode);
                            if (provider != null && !string.IsNullOrWhiteSpace(provider.License))
                                claimDetail.Provider = provider;
                            else
                            {
                                await UpdateClaimStatusByClaimId(claim.ClaimId, "Provider license is not found", (byte)ClaimStatus.InternalError);
                                continue;
                            }
                            ;

                            #region DRG COde
                            if (claim.EncounterType == (byte)Enounter.Inpatient)
                            {

                                if (facility != null && facility.Shadowbillingenable.HasValue && facility.Shadowbillingenable.Value)
                                {
                                    if (claimPayers != null && claimPayers.Shadowbillingenable.HasValue && claimPayers.Shadowbillingenable.Value)
                                    {
                                        var drgData = await getDRGCode(claim);
                                        claimDetail.IsDrgEnable = true;
                                        claimDetail.DRGCode = drgData.Item1;
                                        claimDetail.DRGWeight = drgData.Item2;
                                    }
                                }

                                var dischargeDisposition = await RetrieveDischargeSummaryAsync(organizationId, claim.ClaimId);
                                if (dischargeDisposition > 0)
                                {
                                    claimDetail.DischargeDisposition = dischargeDisposition;
                                }
                            }
                            else
                            {
                                if (claim.DischargeDisposition.HasValue)
                                {
                                    claimDetail.DischargeDisposition = (byte)claim.DischargeDisposition.Value;
                                }

                            }
                            #endregion

                            //Patient
                            var claimPatient = PatientDetail(claim);
                            if (claimPatient != null && !string.IsNullOrWhiteSpace(claimPatient.PatientID))
                            {
                                claimDetail.Patient = claimPatient;
                            }
                            else
                            {
                                await UpdateClaimStatusByClaimId(claim.ClaimId, "Patient is not found", (byte)ClaimStatus.InternalError);
                                continue;
                            }
                            //claimDetail.PatientChild = new Patient();

                            //Clinic
                            claimDetail.Clinic = await ClinicDetail(claim, clinics);

                            //Practitioner
                            var doctors = await RetriveDoctorsAsync(claim.OrganizationId, claim.FacilityId, claim.DoctorId);
                            if (doctors != null && doctors.Length > 0)
                            {
                                claimDetail.Practitioner = DoctorDetail(claim, doctors, clinics);
                            }
                            else
                            {
                                await UpdateClaimStatusByClaimId(claim.ClaimId, "Doctor is not linked with the facility", (byte)ClaimStatus.InternalError);
                                continue;
                            }


                            //ChiefComplaint
                            claimDetail.ChiefComplaint = ClaimChiefComplaint(claim);

                            claimDetail.Discovery = string.Empty;
                            claimDetail.OfflineEligibility = claim.OfflineEligibility;
                            claimDetail.NphiesEligibility = claim.NphiesEligibility;
                            claimDetail.NphiesEligibilitySystem = claim.NphiesEligibilitySystem;
                            claimDetail.OfflineApproval = claim.OfflineApproval;
                            claimDetail.NphiesApprovalIdentifier = claim.NphiesApprovalIdentifier;
                            claimDetail.NphiesApprovalSystem = GetNphiesApprovalSystem(claim, await GetPayerEntity(claim));
                            claimDetail.NphiesApprovalAuthRef = claim.NphiesApprovalAuthRef;
                            //claimDetail.IsDental = string.Empty;
                            claimDetail.ResubmissionReference = string.Empty;
                            claimDetail.ResubmissionDetail = new ResubmissionDetail();
                            claimDetail.DocumentReferenceID = claim.DocumentReferenceNo.ToString();
                            claimDetail.AppendSlashInPreAuthResponse = string.Empty;
                            claimDetail.ClaimTotal = Convert.ToString(claimDetail.ClaimItems.Sum(x => x.NET));
                            //claimResponse.ClaimBundle.Add(messageBundleId);
                            claimDetail.IsReferral = claim.EncounterType == 3 ? true : false;
                            claimDetail.ApprovalNo = claim.PreAuthNo; //RetriveApprovalNumber(claim.RcmClaimServicesDetails.Select(service => service.ApprovalNo).ToArray());
                            if (claim.EncounterType == 2)
                                claimDetail.DischargeSummary = await RetrieveClaimDischargeSummaryAsync(claim.OrganizationId, claim.ClaimId);

                            claimDetail.FileStorageProviderType = (FileStorageProviderType)facility.FileStorageProvider;
                            claimDetail.DocumentIds = await RetrieveAttachmentDocumentIds(claim.ClaimId);
                            claimDetails.Add(claimDetail);


                        }
                        catch (Exception ex)
                        {
                            await UpdateClaimStatusByClaimId(claim.ClaimId, StringConstant.InternalError + $" {ex.Message}", (byte)ClaimStatus.InternalError);
                            logger.LogError(nameof(ClaimService) + "_" + nameof(ClaimDetailsAsync) + "_ " + claim.ClaimId + "_" + ex.Message);
                            continue;
                        }
                    }

                }

                return claimDetails;
            }
            catch (Exception ex)
            {

                logger.LogCritical(ex);
                return await Task.FromResult(new List<ClaimDetail>());
            }
        }

        private static JArray RetriveLabResultsFromMedicalFile(string medicalFile)
        {
            if (string.IsNullOrWhiteSpace(medicalFile))
                return null;

            try
            {
                var medicalFileInfo = JObject.Parse(medicalFile);
                var labToken = medicalFileInfo?["medicalInfo"]?["laboratory"];

                if (labToken != null && labToken.Type == JTokenType.Array)
                {
                    return (JArray)labToken;
                }
            }
            catch (JsonException)
            {
                // Handle malformed JSON
            }
            catch (Exception)
            {
                // Handle any other unexpected exception
            }

            return null;
        }


        private async Task<Tuple<string, string>> getDRGCode(RcmClaim claim)
        {
            var drgResponse = await context.CoDskResponseData
                 .AsNoTracking()
                 .Where(DRG => (DRG.OrganizationId == claim.OrganizationId &&
                                DRG.FacilityId == claim.FacilityId &&
                                DRG.EncounterNo == claim.EncounterNo.ToString()) &&
                                DRG.EncounterType == claim.EncounterType &&
                                DRG.RecordType == (byte)RecordType.Modified)
                 .Select(drg => new
                 {
                     drg.Drg,
                     drg.NationalWeight
                 })
                 .FirstOrDefaultAsync();

            return drgResponse == null ? Tuple.Create(string.Empty, string.Empty) :
                                Tuple.Create(drgResponse.Drg, drgResponse.NationalWeight.ToString());
        }
        private async Task<bool> CheckCoDskEncounterDiagnosis(RcmClaim rcmClaim)
        {
            return await context.CoDskEncounterDiagnoses.AsNoTracking()
                .Where(x => x.OrganizationId == rcmClaim.OrganizationId
                 && x.FacilityId == rcmClaim.FacilityId
                 && x.EncounterType == 2
                 && x.EncounterNo == rcmClaim.EncounterNo.ToString()
                 && x.RecordType == 2
                && x.IsActive == true

               ).CountAsync() > 0;
        }
        private async Task<List<Diagnosis>> GetCoDskEncounterDiagnosis(RcmClaim rcmClaim)
        {
            try
            {
                var coDskDiagnosis = await context.CoDskEncounterDiagnoses.AsNoTracking()
                .Where(x => x.OrganizationId == rcmClaim.OrganizationId
                 && x.FacilityId == rcmClaim.FacilityId
                 && x.EncounterType == rcmClaim.EncounterType
                 && x.EncounterNo == rcmClaim.EncounterNo.ToString()
                 && x.RecordType == 2
                && x.IsActive == true).ToListAsync();

                List<Diagnosis> claimDiagnosis = new List<Diagnosis>();
                if (coDskDiagnosis != null && coDskDiagnosis.Count > 0)
                {
                    int seq = 1;
                    bool isPrimaryAdded = false;
                    bool isMorphology = false;
                    bool isRTADiagnosis = false;
                    foreach (var diagnosis in coDskDiagnosis)
                    {
                        List<Diagnosis> alreadyDiagnosis = claimDiagnosis
                            .Where(d => d.ICD10CM.Trim() == IsString(diagnosis.ICDCode)).ToList();
                        if (alreadyDiagnosis != null && alreadyDiagnosis.Count() > 0)
                            continue;
                        RcmDiagnosis rcmDiagnosis = RetriveDiagnosis(diagnosis.ICDCode.Trim());
                        if (rcmDiagnosis != null)
                        {
                            isMorphology = rcmDiagnosis.IsMorphology.HasValue && rcmDiagnosis.IsMorphology.Value ? true : false;
                            isRTADiagnosis = rcmDiagnosis.IsRTADiagnosis.HasValue && rcmDiagnosis.IsRTADiagnosis.Value ? true : false;
                        }
                        if ((diagnosis.IsPrincipal != null && diagnosis.IsPrincipal == true) && isPrimaryAdded == false)
                        {
                            claimDiagnosis.Add(new Diagnosis
                            {
                                ICD10CM = IsString(diagnosis.ICDCode),
                                SequenceNo = Convert.ToString(seq),
                                TypeCode = "principal",
                                TypeDescription = "Principal Diagnosis",
                                IsMorphology = isMorphology,
                                IsRTADiagnosis = isRTADiagnosis,
                                MorphologyCode = isMorphology ? RetriveDRGMorphologyCode(diagnosis) : "M8000/3"
                            });
                            isPrimaryAdded = true;
                        }
                        else
                        {
                            claimDiagnosis.Add(new Diagnosis
                            {
                                ICD10CM = IsString(diagnosis.ICDCode),
                                SequenceNo = Convert.ToString(seq),
                                TypeCode = "differential",
                                TypeDescription = "Differential Diagnosis",
                                IsMorphology = isMorphology,
                                IsRTADiagnosis = isRTADiagnosis,
                                MorphologyCode = isMorphology ? RetriveDRGMorphologyCode(diagnosis) : "M8000/3"
                            });
                        }
                        seq++;
                    }
                }

                return claimDiagnosis;
            }
            catch (Exception ex)
            {

                throw;
            }

        }

        private DateTime? GetEncounterStartDateNew(RcmClaim claim)
        {
            if (claim.EncounterDateTime == null || DateTime.MinValue == claim.EncounterDateTime)
            {
                var startDate = claim.RcmClaimServicesCategories
                                    .OrderBy(x => x.ServiceStartDate)
                                    .FirstOrDefault().ServiceStartDate;

                return new DateTime(startDate.Value.Year, startDate.Value.Month, startDate.Value.Day, 0, 0, 0);
            }
            else
                return claim.EncounterDateTime.Value;
        }
        private DateTime? GetEncounterEndDateNew(RcmClaim claim)
        {
            if (claim.DischargeDateTime == null || DateTime.MinValue == claim.DischargeDateTime)
            {
                var endDate = claim.RcmClaimServicesCategories
                                .OrderByDescending(x => x.ServiceStartDate)
                                .FirstOrDefault().ServiceStartDate;
                return new DateTime(endDate.Value.Year, endDate.Value.Month, endDate.Value.Day, 0, 0, 0);
            }
            else
            {
                return claim.DischargeDateTime.Value;
            }

        }
        private DateTime? GetEncounterStartDate(RcmClaim claim)
        {
            if (claim.EncounterDateTime == null || DateTime.MinValue == claim.EncounterDateTime)
            {
                var startDate = claim.RcmClaimServicesCategories
                                    .OrderBy(x => x.ServiceStartDate)
                                    .FirstOrDefault().ServiceStartDate;

                return new DateTime(startDate.Value.Year, startDate.Value.Month, startDate.Value.Day, 0, 0, 0);
            }
            else
                return claim.EncounterDateTime.Value;
        }
        private DateTime? GetEncounterEndDate(RcmClaim claim)
        {
            if (claim.DischargeDateTime == null || DateTime.MinValue == claim.DischargeDateTime)
            {
                var endDate = claim.RcmClaimServicesCategories
                                .OrderByDescending(x => x.ServiceStartDate)
                                .FirstOrDefault().ServiceStartDate;
                return new DateTime(endDate.Value.Year, endDate.Value.Month, endDate.Value.Day, 0, 0, 0);
            }
            else
            {
                return claim.DischargeDateTime.Value;
            }
        }
        private static RcmPayer GetPayerForClaim(RcmPayer[] payer, RcmClaim claim)
        {
            return payer.Where(p => p.PayerId == claim.PayerId).FirstOrDefault();
        }

        private static string GetNphiesApprovalSystem(RcmClaim claim, RcmPayer payer)
        {
            return !string.IsNullOrWhiteSpace(claim.NphiesApprovalSystem) ? claim.NphiesApprovalSystem :
                            payer != null ? payer.CompanyUrl : string.Empty;
        }

        private async ValueTask<List<ClaimDetail>> ClaimDetailsAsync
        (
            int organizationId,
            int facilityId,
            string adaptorCode,
            long processId,
            int batchSize,
            string PayerId
        )
        {
            try
            {
                string providerLicense = string.Empty;
                List<ClaimDetail> claimDetails = new List<ClaimDetail>();

                #region Validation

                RcmFacility facility = await RetriveFacilitiesAsync(organizationId, facilityId);

                #endregion

                #region Master Record
                var claims = await RetriveClaimsAsync(organizationId, facilityId, processId, batchSize, Convert.ToInt32(PayerId));
                
                #endregion

                if (claims != null)
                {
                    IQueryable<RcmServiceCatalog> serviceCatelog = context.RcmServiceCatalogs;
                    RcmStandardCode[] standardCodes = await RetriveStandardAsync(organizationId, facilityId);
                    RcmClinic[] clinics = await RetriveClinicsAsync(organizationId, facilityId);
                    foreach (var claim in claims)
                    {
                        try
                        {
                            // string encounterJson = await GetEncounterJson(organizationId, claim.EncounterNo.ToString());
                            JArray ComponentServices = RetriveComponentInvoices(organizationId, claim);
                            JArray jlaboratory = RetriveLabResultsFromMedicalFile(claim.MedicalJsonData);
                            var claimservices = await ClaimServices(organizationId,
                                claim,
                                serviceCatelog,
                                standardCodes,

                                ComponentServices,
                                jlaboratory);

                            if (claimservices != null && claimservices.Count == 0)
                                continue;
                            string messageBundleId = Guid.NewGuid().ToString();
                            //Claim
                            ClaimDetail claimDetail = ClaimModel(claim, clinics, messageBundleId,
                                    await RetriveMaxSubmissionCount(claim.ClaimId)
                                    , PayerIdEnableAlais(claim));
                            if (claimDetail is null)
                                continue;

                            //Services
                            claimDetail.ClaimItems = claimservices;

                            //Payers
                            var claimPayers = await ClaimPayers(claim, adaptorCode);
                            if (claimPayers != null)
                                claimDetail.Payer = claimPayers;
                            else
                                continue;

                            //Diagnosis
                            #region Diagnosis
                            if (facility != null && facility.Shadowbillingenable.HasValue
                                && facility.Shadowbillingenable.Value)
                            {
                                if (claimPayers != null && claimPayers.Shadowbillingenable.HasValue
                                    && claimPayers.Shadowbillingenable.Value)
                                {
                                    if (await CheckCoDskEncounterDiagnosis(claim))
                                    {
                                        claimDetail.Diagnoses = await GetCoDskEncounterDiagnosis(claim);
                                    }
                                    else
                                    {
                                        if (claim.RcmClaimDiagnoses != null
                                                        && claim.RcmClaimDiagnoses.Count > 0)
                                            claimDetail.Diagnoses = ClaimDiagnosis(claim);
                                    }
                                }
                            }
                            else
                            {
                                if (claim.RcmClaimDiagnoses != null
                                                       && claim.RcmClaimDiagnoses.Count > 0)
                                    claimDetail.Diagnoses = ClaimDiagnosis(claim);

                                if (claimDetail.Diagnoses != null && claimDetail.Diagnoses.Count == 0)
                                {
                                    await UpdateClaimStatusByClaimId(claim.ClaimId, StringConstant.DiagnosisRequired, (byte)ClaimStatus.InternalError);
                                    continue;
                                }
                            }
                            #endregion

                            //Vitalsigns and chief Complaint
                            claimDetail.VitalSign = ClaimVitalSign(claim);

                            //Encounters
                            claimDetail.Encounter = ClaimEncounters(claim
                                                    , await GetPayerEntity(claim)
                                                    , GetEncounterStartDate(claim)
                                                    , GetEncounterEndDate(claim));


                            if (claim.EncounterType == (byte)Enounter.Inpatient)
                            {
                                if (facility != null && facility.Shadowbillingenable.HasValue && facility.Shadowbillingenable.Value)
                                {
                                    if (claimPayers != null && claimPayers.Shadowbillingenable.HasValue && claimPayers.Shadowbillingenable.Value)
                                    {
                                        var drgData = await getDRGCode(claim);
                                        claimDetail.IsDrgEnable = true;
                                        claimDetail.DRGCode = drgData.Item1;
                                        claimDetail.DRGWeight = drgData.Item2;
                                    }
                                }
                                var dischargeDisposition = await RetrieveDischargeSummaryAsync(organizationId, claim.ClaimId);
                                if (dischargeDisposition != null)
                                {
                                    claimDetail.DischargeDisposition = dischargeDisposition;
                                }
                            }
                            else
                            {
                                if (claim.DischargeDisposition.HasValue)
                                {
                                    claimDetail.DischargeDisposition = (byte)claim.DischargeDisposition.Value;
                                }

                            }
                            //Providers
                            var provider = await ClaimProviders(claim, facility, adaptorCode);
                            if (provider != null)
                                claimDetail.Provider = provider;
                            else
                                continue;


                            //Patient
                            claimDetail.Patient = PatientDetail(claim);
                            //claimDetail.PatientChild = new Patient();

                            //Clinic
                            claimDetail.Clinic = await ClinicDetail(claim, clinics);

                            //Practitioner
                            var doctors = await RetriveDoctorsAsync(claim.OrganizationId, claim.FacilityId, claim.DoctorId);
                            claimDetail.Practitioner = DoctorDetail(claim, doctors, clinics);

                            //ChiefComplaint
                            claimDetail.ChiefComplaint = ClaimChiefComplaint(claim);

                            claimDetail.Discovery = string.Empty;
                            claimDetail.OfflineEligibility = claim.OfflineEligibility;
                            claimDetail.NphiesEligibility = claim.NphiesEligibility;
                            claimDetail.NphiesEligibilitySystem = claim.NphiesEligibilitySystem;
                            claimDetail.OfflineApproval = claim.OfflineApproval;
                            claimDetail.NphiesApprovalIdentifier = claim.NphiesApprovalIdentifier;
                            claimDetail.NphiesApprovalSystem = claim.NphiesApprovalSystem;
                            claimDetail.NphiesApprovalAuthRef = claim.NphiesApprovalAuthRef;
                            //claimDetail.IsDental = string.Empty;
                            claimDetail.ResubmissionReference = string.Empty;
                            claimDetail.ResubmissionDetail = new ResubmissionDetail();
                            claimDetail.DocumentReferenceID = claim.DocumentReferenceNo.ToString();
                            claimDetail.AppendSlashInPreAuthResponse = string.Empty;
                            claimDetail.ClaimTotal = Convert.ToString(claimDetail.ClaimItems.Sum(x => x.NET));
                            //claimResponse.ClaimBundle.Add(messageBundleId);
                            claimDetail.IsReferral = claim.EncounterType == 3 ? true : false;
                            claimDetail.ApprovalNo = claim.PreAuthNo; //RetriveApprovalNumber(claim.RcmClaimServicesDetails.Select(service => service.ApprovalNo).ToArray());
                            if (claim.EncounterType == 2)
                                claimDetail.DischargeSummary = await RetrieveClaimDischargeSummaryAsync(claim.OrganizationId, claim.ClaimId);
                            claimDetail.FileStorageProviderType = (FileStorageProviderType)facility.FileStorageProvider;
                            claimDetail.DocumentIds = await RetrieveAttachmentDocumentIds(claim.ClaimId);
                            claimDetails.Add(claimDetail);

                        }
                        catch (Exception ex)
                        {
                            await UpdateClaimStatusByClaimId(claim.ClaimId, StringConstant.InternalError + $" {ex.Message.Trim()}", (byte)ClaimStatus.InternalError);
                            logger.LogError(nameof(ClaimService) + "_" + nameof(ClaimDetailsAsync) + ex.Message);
                            continue;
                        }
                    }
                }
                return claimDetails;
            }
            catch (Exception ex)
            {
                logger.LogError(nameof(ClaimService) + "_" + nameof(ClaimDetailsAsync) + ex.Message);
                throw;
            }
        }

        private IQueryable<RcmPayer> RetrivePayers(int organizationId, string PayerId)
        {

            var payers = context.RcmPayers.AsNoTracking();
            if (!string.IsNullOrWhiteSpace(PayerId))
            {
                List<int> payersId = new List<int>();
                if (PayerId.Contains(","))
                {
                    string[] str = PayerId.Split(',');
                    payersId = Array.ConvertAll(str, s => int.Parse(s)).ToList();
                }
                else
                    payersId.Add(Convert.ToInt32(PayerId));

                payers = payers.Where(payer => payer.OrganizationId == organizationId
                && payersId.Contains(payer.PayerId)
                );
            }

            return payers;
        }

        private string RetriveApprovalNumber(string[] approvalNumbers)
        {
            if (approvalNumbers != null)
            {
                foreach (var approvalNumber in approvalNumbers)
                {
                    if (!string.IsNullOrWhiteSpace(approvalNumber) && approvalNumber.Length > 1)
                    {
                        return approvalNumber;

                    }

                }
            }
            return String.Empty;
        }

        private async Task<RcmFacility> RetriveFacilitiesAsync(int organizationId, int facilityId)
        {
            try
            {
                string key = $"organizationId-{organizationId}_facilityId-{facilityId}";

                return await memoryCache.GetOrCreateAsync(
                    key,
                    async entry =>
                    {
                        entry.SetAbsoluteExpiration(TimeSpan.FromHours(8));

                        return await context.RcmFacilities
                            .AsNoTracking()
                            .Where(f => f.OrganizationId == organizationId
                                        && f.FacilityId == facilityId
                                        && f.IsActive)
                            .FirstOrDefaultWithNoLockAsync();
                    });


            }
            catch (Exception ex)
            {
                logger.LogError(nameof(ClaimDetailsAsync) + "_" + nameof(RetriveFacilitiesAsync) + ex.Message);
                string customMessage = StringConstant.RetriveFacilitiesError + $" {ex.Message}";
                throw new Exception(customMessage, ex);
            }
        }

        private async Task<RcmClinic[]> RetriveClinicsAsync(int organizationId, int facilityId)
        {
            string key = $"clinic-organizationId-{organizationId}-{facilityId}";
            try
            {
                return await memoryCache.GetOrCreateAsync(
                    key,
                    async entity =>
                    {
                        entity.SetAbsoluteExpiration(TimeSpan.FromHours(8));
                        return await context.RcmClinics.AsNoTracking()
                                        .Where(c => c.OrganizationId == organizationId
                                                && c.FacilityId == facilityId)
                                        .ToArrayAsync();
                    }
                );
            }
            catch (SqlException sql)
            {
                logger.LogCritical(sql);
                throw new Exception(sql.Message);
            }
            catch (Exception ex)
            {
                logger.LogError(nameof(ClaimDetailsAsync) + "_" + nameof(RetriveClinicsAsync) + "_ " + facilityId + " - " + ex.Message);
                return null; // Or handle as appropriate
            }
        }


        private async Task<RcmDoctor[]> RetriveDoctorsAsync(int organizationId, int facilityId, int doctorId)
        {
            try
            {
                return await context.RcmDoctors.AsNoTracking()
                                   .Include(x => x.RcmDoctorLicenses).AsNoTracking()
                                   .Where(doctor => doctor.OrganizationId == organizationId
                                    && doctor.FacilityId == facilityId
                                   && doctor.DoctorId == doctorId)
                                   .ToArrayAsync();
            }
            catch (Exception ex)
            {
                logger.LogError(nameof(ClaimDetailsAsync) + "_" + nameof(RetriveDoctorsAsync) + ex.Message);
                return await Task.FromResult(Array.Empty<RcmDoctor>());
            }

        }

        private async Task<RcmTransactionMapingValue[]> RetriveTransactionMapingAsync(int organizationId)
        {
            try
            {
                string key = $"TransactionMaping";
                return await memoryCache.GetOrCreateAsync(
                                           key,
                                           async entity =>
                                           {
                                               entity.SetAbsoluteExpiration(TimeSpan.FromHours(8));
                                               return await context.RcmTransactionMapingValues.AsNoTracking()
                                               .Where(t => t.OrganizationId == organizationId)
                                               .ToArrayAsync();
                                           });
            }
            catch (Exception ex)
            {
                logger.LogError(nameof(ClaimDetailsAsync) + "_" + nameof(RetriveTransactionMapingAsync) + ex.Message);
                return await Task.FromResult(Array.Empty<RcmTransactionMapingValue>());
            }

        }

        private async Task<RcmStandardCode[]> RetriveStandardAsync(int organizationId, int facilityId)
        {
            try
            {
                string key = $"Standard-Code-OrganizationId-{organizationId}";
                return await memoryCache.GetOrCreateAsync(
                     key,
                     entity =>
                     {
                         entity.SetAbsoluteExpiration(TimeSpan.FromHours(8));
                         return context.RcmStandardCodes.AsNoTracking()
                                                                .Select(x => new RcmStandardCode
                                                                {
                                                                    OrganizationId = x.OrganizationId,
                                                                    ServiceId = x.ServiceId,
                                                                    StandardCodeDescription = x.StandardCodeDescription
                                                                })
                                                                .Where(x => x.OrganizationId == organizationId)
                                                                .ToArrayAsync();
                     }
                     );
            }
            catch (SqlException sqlException)
            {
                logger.LogCritical(sqlException);
                return await Task.FromResult(Array.Empty<RcmStandardCode>());

            }
            catch (Exception ex)
            {
                logger.LogCritical(ex);
                return await Task.FromResult(Array.Empty<RcmStandardCode>());
            }

        }
        private async Task<string> GetEncounterJson(int organizationId, string encounterNo)
        {
            var medicalRecord = await context.RcmEncounterMedicalDetails.AsNoTracking()

                                                    .Where(x => x.EncounterNo == encounterNo
                                                    && x.OrganizationId == organizationId
                                                    && x.IsActive).Select(x => x.MedicalData)
                                                    .FirstOrDefaultWithNoLockAsync()
                                                    ;
            if (medicalRecord != null)
                return medicalRecord;
            else
                return string.Empty;
        }

        private Task<List<RcmAdapter>> GetAdaptors(int organizationId, string adaptorCode)
        {
            string key = $"{organizationId}-{adaptorCode}";
            return memoryCache.GetOrCreateAsync(
                 key,
                 entity =>
                 {
                     entity.SetAbsoluteExpiration(TimeSpan.FromHours(8));
                     return context.RcmAdapters.AsNoTracking()
                                            .Include(adaptor => adaptor.RcmAdapterMappings)
                                            .Where(adaptor => adaptor.OrganizationId == organizationId
                                              && adaptor.AdaptorCode == adaptorCode
                                              && adaptor.IsActive == true).ToListAsync();
                 }
                 );

        }

        private async ValueTask<bool> UpdateClaimAsync(ClaimResponseRequest claimResponseRequest)
        {
            bool status2Return = false;

            if (claimResponseRequest.ClaimRequestModel != null)
            {
                var claimRequest = claimResponseRequest.ClaimRequestModel;
                var claimResponse = claimResponseRequest.ClaimResponseModel;
                if (claimResponse != null && claimResponse.Count > 0)
                {
                    var resmodel = claimResponse.Select(x => new BatchResponse
                    {
                        ClaimDetailResponse = x.ClaimDetailResponse
                    }).ToList();

                    var claimDetailResponses = resmodel.ToList();
                    List<ClaimDetailResponse> detailResponses = new List<ClaimDetailResponse>();
                    foreach (var item in claimDetailResponses)
                        detailResponses = item.ClaimDetailResponse;

                    if (claimRequest != null)
                    {
                        foreach (var claim in claimRequest.ClaimDetails)
                        {
                            var claimEntity = await context.RcmClaims
                                .FirstOrDefaultAsync(predicate => predicate.ClaimId == Convert.ToInt64(claim.ClaimID));
                            if (claimEntity != null)
                            {
                                // Add Or Update ClaimNphiesPostTrail
                                if (claimResponseRequest.CriteriaType > 0)
                                {
                                    await AddClaimPostTrail(claimResponseRequest, claim, claimEntity.OrganizationId);
                                }

                                if (claimResponseRequest.CriteriaType != (byte)NphiesFiltercriteria.ReProcess_With_Same_BundleId)
                                {
                                    if (await UpdateClaimDetails(claimResponseRequest, detailResponses, claim, claimEntity) > 0)
                                        status2Return = true;
                                }
                                else
                                {
                                    if (await UpdateClaimDetailsResponseRequest(detailResponses, claim, claimEntity) > 0)
                                        status2Return = true;
                                }

                            }
                        }
                    }
                }
                else
                {
                    List<ClaimDetailResponse> detailResponses = new List<ClaimDetailResponse>();
                    var batchNo = claimRequest.ClaimbatchID;
                    foreach (var claim in claimRequest.ClaimDetails)
                    {
                        Int64 claimId = Convert.ToInt64(claim.ClaimID);
                        var claimEntity = await context.RcmClaims
                            .FirstOrDefaultAsync(predicate => predicate.ClaimId == claimId);
                        if (claimEntity != null)
                        {
                            claimEntity.ClaimBundleId = claim.MessageBundleID;
                            claimEntity.ClaimIdentifier = claim.ClaimIdentifier;
                            claimEntity.BatchIdentifier = batchNo;
                            claimEntity.BatchBundleId = claimRequest.ClaimBundleIdentifier;
                            claimEntity.ProcessId = claimResponseRequest.ProcessId;


                            // Update the Claim Status
                            claimEntity.Status = (byte)ClaimStatus.InProgress;
                        }
                        var updatedServices = claimResponseRequest.UpdatedServices.Where(x => x.ClaimId == claimId).ToList();
                        var rowIds = updatedServices.Select(x => x.RowId).ToList();

                        // Fetch all service entities in one go
                        var serviceEntities = await context.RcmClaimServicesDetails
                                             .Where(x =>
                                                 rowIds.Contains(x.RowId) &&
                                                 (x.IsDeleted == null || x.IsDeleted == false) &&
                                                 (x.IsRefund == null || x.IsRefund == false) &&
                                                 (x.IsReturn == null || x.IsReturn == false)
                                             )
                                             .ToListAsync();

                        // Create a dictionary for faster lookup
                        var updatedServicesDict = updatedServices.ToDictionary(x => x.RowId);

                        // Update matching entities
                        foreach (var serviceEntity in serviceEntities)
                        {
                            if (updatedServicesDict.TryGetValue(serviceEntity.RowId, out var updatedItem))
                            {
                                serviceEntity.NphiesSeqNo = updatedItem.Seqno;
                                serviceEntity.ClaimIdentifier = claim.ClaimIdentifier;
                            }
                        }
                        //foreach (var item in claimResponseRequest.UpdatedServices.Where(x => x.ClaimId == claimId).ToList())
                        //{
                        //    var serviceEntity = await context.RcmClaimServicesDetails.FirstOrDefaultAsync(x => x.RowId == item.RowId);
                        //    if (serviceEntity != null)
                        //    {
                        //        serviceEntity.NphiesSeqNo = item.Seqno;
                        //        serviceEntity.ClaimIdentifier = claim.ClaimIdentifier;

                        //        // context.RcmClaimServicesDetails.Add(serviceEntity);
                        //    }
                        //}
                    }

                    try
                    {
                        if (await context.SaveChangesAsync() > 0)
                            status2Return = true;
                    }
                    catch (Exception ex)
                    {

                        // throw;
                    }
                }

            }
            return status2Return;
        }

        private async Task<int> UpdateClaimDetails(ClaimResponseRequest claimResponseRequest, List<ClaimDetailResponse> detailResponses,
            ClaimDetail claim, RcmClaim claimEntity)
        {

            int updated = 0;
            try
            {
                if (detailResponses != null && detailResponses.Count > 0)
                {
                    var response = detailResponses.Where(x => x.ClaimIdentifier == claim.ClaimIdentifier).FirstOrDefault();
                    if (response != null)
                    {
                        if (response.IsPended)
                        {
                            claimEntity.IsPendedReceived = response.IsPended;
                            claimEntity.Status = Convert.ToByte(Common.GetStatus("pended"));
                        }
                        else
                        {
                            claimEntity.Status = Convert.ToByte(Common.GetStatus(response.Status));
                        }

                        claimEntity.NphieseRemarks = response.Remarks.Replace("'", "");
                        claimEntity.SubmittedBy = claimResponseRequest.SubmitedBy;
                        claimEntity.ResponseReceivedOn = DateTime.Now;
                    }

                    foreach (var service in claim.ClaimItems)
                    {
                        var serviceEntity = await context.RcmClaimServicesDetails
                                            .Where(predicate => predicate.ClaimId == Convert.ToInt64(claim.ClaimID)
                                                && predicate.NphiesSeqNo == service.Sequence)
                                              .FirstOrDefaultAsync();

                        if (serviceEntity != null)
                        {
                            if (response != null)
                            {
                                try
                                {
                                    serviceEntity.Status = Convert.ToByte(Common.GetStatus(response.Status));
                                    var arr = response.ClaimItemDetail?.Where(x => x.Sequence == serviceEntity.NphiesSeqNo).Select(error => error.Error).Distinct().ToArray();
                                    var error = arr != null && arr.Length > 0 ? string.Join(",", arr) : "";
                                    if (Convert.ToByte(Common.GetStatus(response.Status)) == 8)
                                        serviceEntity.NphieseRemarks = null;
                                    else
                                        serviceEntity.NphieseRemarks = error;
                                }
                                catch (Exception ex)
                                {
                                    logger.LogError(ex.Message);
                                    //throw;
                                }



                            }

                            serviceEntity.SubmittedAmount = Convert.ToString(service.NET);

                            try
                            {
                                if (float.Parse(service.Quantity) < 1.00f)
                                    service.Quantity = "1.0";

                            }
                            catch (Exception ex)
                            {
                                logger.LogError(ex.Message);

                            }
                            serviceEntity.SubmittedQuantity = service.Quantity;



                        }
                    }
                    if (claimEntity.Status == (byte)ClaimStatus.Nphies_Queued
                        || claimEntity.Status == (byte)ClaimStatus.Nphies_Approved)
                    {
                        claimEntity.NphieseRemarks = string.Empty;
                    }
                    else if (claimEntity.Status == (byte)ClaimStatus.Nphies_Error)
                    {
                        claimEntity.IsErrorResolved = false;
                    }

                    if (!string.IsNullOrEmpty(claim.ClaimTotal))
                    {
                        claimEntity.TotalSubmitted = claim.ClaimTotal;
                    }

                }

                updated = await context.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                logger.LogError(ex.Message);
            }
            return updated;
        }

        private async Task<int> UpdateClaimDetailsAsync(string claimIdentifier,
            decimal ClaimTotal,
            List<ClaimServices> ClaimServices,
            ClaimResponseDetail detailResponses,
            RcmClaim claimEntity)
        {

            int updated = 0;
            try
            {
                if (detailResponses != null)
                {
                    var response = detailResponses;
                    if (response != null)
                    {
                        if (response.IsPended)
                        {
                            claimEntity.IsPendedReceived = response.IsPended;
                            claimEntity.Status = Convert.ToByte(Common.GetStatus("pended"));
                        }
                        else
                        {
                            claimEntity.Status = Convert.ToByte(Common.GetStatus(response.Status));
                        }

                        claimEntity.NphieseRemarks = response.Remarks.Replace("'", "");
                        claimEntity.SubmittedOn = DateTime.Now;
                    }

                    #region Old Logic
                    //foreach (var service in ClaimServices)
                    //{
                    //    var serviceEntity = await context.RcmClaimServicesDetails
                    //                        .Where(predicate => predicate.ClaimId == claimEntity.ClaimId
                    //                            && predicate.NphiesSeqNo == service.Seqno
                    //                            && predicate.RowId == service.RowId)
                    //                          .FirstOrDefaultAsync();

                    //    if (serviceEntity != null)
                    //    {
                    //        if (response != null)
                    //        {
                    //            try
                    //            {
                    //                serviceEntity.Status = Convert.ToByte(Common.GetStatus(response.Status));
                    //                var arr = response.ClaimItemDetail?.Where(x => x.Sequence == serviceEntity.NphiesSeqNo)
                    //                    .Select(error => error.Error).Distinct().ToArray();
                    //                var error = arr != null && arr.Length > 0 ? string.Join(",", arr) : "";
                    //                if (Convert.ToByte(Common.GetStatus(response.Status)) == 8)
                    //                    serviceEntity.NphieseRemarks = null;
                    //                else
                    //                    serviceEntity.NphieseRemarks = error;
                    //            }
                    //            catch (Exception ex)
                    //            {
                    //                logger.LogError(ex.Message);
                    //                //throw;
                    //            }



                    //        }

                    //        serviceEntity.SubmittedAmount = Convert.ToString(service.NET);

                    //        try
                    //        {
                    //            if (service.Quantity < 1.0m)
                    //                service.Quantity = 1;

                    //        }
                    //        catch (Exception ex)
                    //        {
                    //            logger.LogError(ex.Message);

                    //        }
                    //        serviceEntity.SubmittedQuantity = service.Quantity.ToString();



                    //    }
                    //} 
                    #endregion
                    // Step 1: Extract all RowIds from input
                    var rowIds = ClaimServices.Select(s => s.RowId).ToList();

                    // Step 2: Fetch all relevant entities in one DB query
                    var serviceEntities = await context.RcmClaimServicesDetails
                        .Where(x => rowIds.Contains(x.RowId) && x.ClaimId == claimEntity.ClaimId)
                        .ToListAsync();

                    // Step 3: Dictionary for quick access by RowId
                    var entityDict = serviceEntities.ToDictionary(x => x.RowId);

                    // Step 4: Precompute status and error (only once)
                    byte responseStatus = 0;
                    string combinedError = "";

                    if (response != null)
                    {
                        responseStatus = Convert.ToByte(Common.GetStatus(response.Status));

                        if (response.ClaimItemDetail != null)
                        {
                            var errorList = response.ClaimItemDetail
                                .Where(x => !string.IsNullOrWhiteSpace(x.Error))
                                .Select(x => x.Error)
                                .Distinct()
                                .ToArray();

                            combinedError = errorList.Length > 0 ? string.Join(",", errorList) : "";
                        }
                    }

                    // Step 5: Apply updates
                    foreach (var service in ClaimServices)
                    {
                        if (entityDict.TryGetValue(service.RowId, out var serviceEntity))
                        {
                            try
                            {
                                if (response != null)
                                {
                                    serviceEntity.Status = responseStatus;
                                    serviceEntity.NphieseRemarks = responseStatus == 8 ? null : combinedError;
                                }

                                serviceEntity.SubmittedAmount = Convert.ToString(service.NET);

                                // Ensure quantity is at least 1
                                var validQty = service.Quantity < 1.0m ? 1.0m : service.Quantity;
                                serviceEntity.SubmittedQuantity = validQty.ToString();
                            }
                            catch (Exception ex)
                            {
                                logger.LogError($"Failed to update RowId {service.RowId}");
                            }
                        }
                    }

                    if (claimEntity.Status == (byte)ClaimStatus.Nphies_Queued
                        || claimEntity.Status == (byte)ClaimStatus.Nphies_Approved)
                    {
                        claimEntity.NphieseRemarks = string.Empty;
                    }
                    else if (claimEntity.Status == (byte)ClaimStatus.Nphies_Error)
                    {
                        claimEntity.IsErrorResolved = false;
                    }

                    if (ClaimTotal > 0)
                    {
                        claimEntity.TotalSubmitted = ClaimTotal.ToString();
                    }

                }

                updated = await context.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                logger.LogError(ex.Message);
            }
            return updated;
        }
        private async Task<int> UpdateClaimDetailsResponseRequest(List<ClaimDetailResponse> detailResponses,
            ClaimDetail claim, RcmClaim claimEntity)
        {
            int updated = 0;
            try
            {
                if (detailResponses != null && detailResponses.Count > 0)
                {
                    var response = detailResponses.Where(x => x.ClaimIdentifier.Contains(claim.ClaimIdentifier)).FirstOrDefault();
                    if (response != null)
                    {

                        if (!string.IsNullOrEmpty(response.Remarks))
                            claimEntity.NphieseRemarks = response.Remarks.Replace("'", "");

                        if (response.IsPended)
                        {
                            claimEntity.IsPendedReceived = true;
                            claimEntity.Status = Convert.ToByte(Common.GetStatus("pended"));
                        }
                        else
                        {
                            claimEntity.Status = Convert.ToByte(Common.GetStatus(response.Status));
                        }
                        if (!string.IsNullOrEmpty(response.TotalApproved))
                        {
                            claimEntity.TotalApproved = response.TotalApproved;
                        }

                        claimEntity.ModifiedOn = DateTime.Now;

                        if (response.ClaimItemDetail != null)
                        {
                            foreach (var service in response.ClaimItemDetail)
                            {
                                var serviceEntity = await context.RcmClaimServicesDetails.Where(predicate => predicate.ClaimId == Convert.ToInt64(claim.ClaimID)
                                                                                            && predicate.ServiceReferenceNumber == service.InvoiceNo
                                                                                            && predicate.NphiesSeqNo == service.Sequence)
                                                                                        .FirstOrDefaultAsync();
                                if (serviceEntity != null)
                                {
                                    try
                                    {
                                        serviceEntity.Status = Convert.ToByte(Common.GetStatus(service.Status));

                                        //var arr = response.ClaimItemDetail?.Where(x => x.Sequence == serviceEntity.NphiesSeqNo)
                                        //                                    .Select(error => error.Error)
                                        //                                    .Distinct().ToArray();
                                        //var error = arr != null && arr.Length > 0 ? string.Join(",", arr) : "";

                                        serviceEntity.BenefitAmount = service.BenefitAmount;
                                        if (!string.IsNullOrWhiteSpace(service.ApprovedQuantity))
                                            serviceEntity.ApprovedQuantity = GetApprovedQuantity(service.ApprovedQuantity);
                                        else
                                            serviceEntity.ApprovedQuantity = 0;
                                        if (Convert.ToByte(Common.GetStatus(response.Status)) == (byte)ClaimStatus.Nphies_Approved)
                                        {
                                            serviceEntity.NphieseRemarks = null;
                                            serviceEntity.Status = (byte)ClaimStatus.Nphies_Approved;
                                        }
                                        else
                                        {
                                            if (serviceEntity.Status == (byte)ClaimStatus.Nphies_Approved)
                                            {
                                                serviceEntity.NphieseRemarks = null;
                                            }
                                            else
                                            {
                                                if (!string.IsNullOrEmpty(service.ErrorCode))
                                                {
                                                    serviceEntity.ReasonCode = service.ErrorCode;
                                                }
                                                if (!string.IsNullOrEmpty(service.Error))
                                                {
                                                    serviceEntity.NphieseRemarks = service.Error;
                                                }
                                            }
                                        }

                                        serviceEntity.ResponseReceivedOn = DateTime.Now;
                                    }
                                    catch (Exception ex)
                                    {
                                        logger.LogError(ex.Message);
                                        //throw;
                                    }
                                }
                            }
                        }
                    }

                    updated = await context.SaveChangesAsync();
                }
            }
            catch (Exception ex)
            {
                logger.LogError(ex.Message);
            }

            return updated;
        }
        private async Task<int> UpdateClaimDetailsResponseRequestAsync(ClaimResponseDetail detailResponses,
            RcmClaim claimEntity)
        {
            int updated = 0;
            try
            {
                if (detailResponses != null)
                {
                    var response = detailResponses;
                    if (response != null)
                    {

                        if (!string.IsNullOrEmpty(response.Remarks))
                            claimEntity.NphieseRemarks = response.Remarks.Replace("'", "");

                        if (response.IsPended)
                        {
                            claimEntity.IsPendedReceived = true;
                            claimEntity.Status = Convert.ToByte(Common.GetStatus("pended"));
                        }
                        else
                        {
                            claimEntity.Status = Convert.ToByte(Common.GetStatus(response.Status));
                        }
                        //if (!string.IsNullOrEmpty(response.TotalApproved))
                        //{
                        //    claimEntity.TotalApproved = response.TotalApproved;
                        //}
                        claimEntity.ResponseReceivedOn = DateTime.Now;
                        claimEntity.ModifiedOn = DateTime.Now;

                        if (response.ClaimItemDetail != null)
                        {
                            #region Old Logic
                            //foreach (var service in response.ClaimItemDetail)
                            //{
                            //    var serviceEntity = await context.RcmClaimServicesDetails
                            //        .Where(predicate => predicate.ClaimId == claimEntity.ClaimId
                            //                && predicate.ServiceReferenceNumber == service.InvoiceNo
                            //               && predicate.NphiesSeqNo == service.Sequence)
                            //                                                            .FirstOrDefaultAsync();
                            //    if (serviceEntity != null)
                            //    {
                            //        try
                            //        {
                            //            serviceEntity.Status = Convert.ToByte(Common.GetStatus(service.Status));

                            //            serviceEntity.BenefitAmount = service.BenefitAmount;
                            //            if (!string.IsNullOrWhiteSpace(service.ApprovedQuantity))
                            //                serviceEntity.ApprovedQuantity = GetApprovedQuantity(service.ApprovedQuantity);
                            //            else
                            //                serviceEntity.ApprovedQuantity = 0;
                            //            if (Convert.ToByte(Common.GetStatus(response.Status)) == (byte)ClaimStatus.Nphies_Approved)
                            //            {
                            //                serviceEntity.NphieseRemarks = null;
                            //                serviceEntity.Status = (byte)ClaimStatus.Nphies_Approved;
                            //            }
                            //            else
                            //            {
                            //                if (serviceEntity.Status == (byte)ClaimStatus.Nphies_Approved)
                            //                {
                            //                    serviceEntity.NphieseRemarks = null;
                            //                }
                            //                else
                            //                {
                            //                    if (!string.IsNullOrEmpty(service.ErrorCode))
                            //                    {
                            //                        serviceEntity.ReasonCode = service.ErrorCode;
                            //                    }
                            //                    if (!string.IsNullOrEmpty(service.Error))
                            //                    {
                            //                        serviceEntity.NphieseRemarks = service.Error;
                            //                    }
                            //                }
                            //            }

                            //            serviceEntity.ResponseReceivedOn = DateTime.Now;
                            //        }
                            //        catch (Exception ex)
                            //        {
                            //            logger.LogError(ex.Message);
                            //            //throw;
                            //        }
                            //    }
                            //} 
                            #endregion

                            // Step 1: Extract keys to match on
                            var serviceKeys = response.ClaimItemDetail
                                .Select(x => new { InvoiceNo = x.InvoiceNo, Sequence = x.Sequence })
                                .ToList();

                            // Step 2: Fetch all matching service entities in one query
                            var serviceEntities = await context.RcmClaimServicesDetails
                                .Where(s => s.ClaimId == claimEntity.ClaimId &&
                                            serviceKeys.Any(k => k.InvoiceNo == s.ServiceReferenceNumber && k.Sequence == s.NphiesSeqNo))
                                .ToListAsync();

                            // Step 3: Create a dictionary for fast lookup
                            var entityDict = serviceEntities.ToDictionary(
                                s => (s.ServiceReferenceNumber, s.NphiesSeqNo),
                                s => s
                            );

                            // Step 4: Update entities in memory
                            var claimStatus = Convert.ToByte(Common.GetStatus(response.Status));
                            var isApproved = claimStatus == (byte)ClaimStatus.Nphies_Approved;

                            foreach (var service in response.ClaimItemDetail)
                            {
                                if (entityDict.TryGetValue((service.InvoiceNo, service.Sequence), out var serviceEntity))
                                {
                                    try
                                    {
                                        serviceEntity.Status = Convert.ToByte(Common.GetStatus(service.Status));
                                        serviceEntity.BenefitAmount = service.BenefitAmount;
                                        serviceEntity.ApprovedQuantity = !string.IsNullOrWhiteSpace(service.ApprovedQuantity)
                                            ? GetApprovedQuantity(service.ApprovedQuantity)
                                            : 0;

                                        if (isApproved)
                                        {
                                            serviceEntity.NphieseRemarks = null;
                                            serviceEntity.Status = (byte)ClaimStatus.Nphies_Approved;
                                        }
                                        else
                                        {
                                            if (serviceEntity.Status == (byte)ClaimStatus.Nphies_Approved)
                                            {
                                                serviceEntity.NphieseRemarks = null;
                                            }
                                            else
                                            {
                                                if (!string.IsNullOrEmpty(service.ErrorCode))
                                                    serviceEntity.ReasonCode = service.ErrorCode;

                                                if (!string.IsNullOrEmpty(service.Error))
                                                    serviceEntity.NphieseRemarks = service.Error;
                                            }
                                        }

                                        serviceEntity.ResponseReceivedOn = DateTime.Now;
                                    }
                                    catch (Exception ex)
                                    {
                                        logger.LogError($"Failed to update service entity for InvoiceNo: {service.InvoiceNo}, Seq: {service.Sequence}");
                                    }
                                }
                            }
                        }
                    }

                    updated = await context.SaveChangesAsync();
                }
            }
            catch (Exception ex)
            {
                logger.LogError(ex.Message);
            }

            return updated;
        }
        private int GetApprovedQuantity(string approvedQuantity)
        {
            try
            {
                return (int)Convert.ToDecimal(approvedQuantity);
            }
            catch (ArgumentNullException ex)
            {
                logger.LogError(ex.Message);
            }
            catch (FormatException ex)
            {
                logger.LogError(ex.Message);
            }
            catch (OverflowException ex)
            {
                logger.LogError(ex.Message);
            }

            return 0;
        }

        private async Task<Provider> ClaimProviders(RcmClaim claim, RcmFacility facility, string adaptorCode)
        {
            try
            {
                Provider provider = new Provider();
                var adapterMapping = await GetProviderAdapterMapping(facility, adaptorCode);

                if (adapterMapping == null)
                    return provider;

                if (claim.FacilityId == facility.FacilityId)
                {
                    provider.Name = facility.FacilityName;
                    provider.ORGType = ClaimInfo.ORGType;
                    provider.ProjectID = facility.ExternalCode.ToString();
                    provider.SetupID = facility.ExternalCode2.ToString();
                    provider.STATUS = AppCons.Active;
                    provider.License = adapterMapping != null ? IsString(adapterMapping.ParameterValue) : null;
                    provider.ProviderType = (int)facility.ProviderType;
                }

                return provider;
            }
            catch (Exception ex)
            {
                logger.LogError(nameof(ClaimDetailsAsync) + "_" + nameof(ClaimProviders) + ex.Message);
                throw new Exception(StringConstant.ClaimProvidersError + $" {ex.Message}", ex);
            }
        }

        private Encounter ClaimEncounters(RcmClaim claim, RcmPayer? payers, DateTime? encounterStart, DateTime? encounterEnd)
        {
            Encounter encounter = new Encounter();
            if (claim.EncounterNo != null)
            {
                try
                {
                    encounter.Identifier = claim.EncounterNo.ToString();
                    encounter.OriginalEncounter = claim.OriginalEncounter;
                    if (payers != null && payers.IsReferral.HasValue && payers.IsReferral.Value)
                    {
                        encounter.EncounterType = 3.ToString(); // 3 encounter is for referral
                    }
                    else
                        encounter.EncounterType = claim.EncounterType.ToString();


                    encounter.STATUS = AppCons.Active;
                    encounter.Class = string.Empty;
                    encounter.ServiceTypeCode = string.Empty;
                    encounter.ServiceTypeDisplay = string.Empty;
                    encounter.priorityCode = string.Empty;
                    encounter.priorityDisplay = string.Empty;
                    encounter.Start = encounterStart != null ? encounterStart.Value.ToNaphiesFormat() : claim.EncounterDateTime.Value.ToNaphiesFormat();
                    encounter.END = encounterEnd != null ? encounterEnd.Value.ToNaphiesFormat() : claim.DischargeDateTime.Value.ToNaphiesFormat();
                    encounter.TriageCategory = claim.TriageCategory;
                    encounter.EmergencyArrivalCode = claim.EmergencyArrivalCode;
                    encounter.IntendedLenghtOfStay = claim.IntendedLenghtOfStay;
                    encounter.EncounterAdmitSource = claim.EncounterAdmitSource;
                    encounter.EmergencyDepartmentDisposition = claim.EmergencyDepartmentDisposition;
                    if (claim.TriageDate != null)
                    {
                        encounter.TriageDate = (DateTime)claim.TriageDate;
                    }
                    encounter.DischargeDisposition = claim.DischargeDisposition;
                }
                catch (Exception ex)
                {
                    string errorMessage = string.Format("{0},{1},{2}", claim.ClaimId, claim.FacilityId, ex.Message);
                    this.logger.LogError(errorMessage);

                }
            }
            return encounter;
        }
        //private static Practitioner DoctorDetail(RcmClaim claim, RcmDoctor[] doctors)
        //{
        //    Practitioner doctorModel = new Practitioner();

        //    var doctorEntity = doctors.FirstOrDefault(x => x.DoctorId == claim.DoctorId);
        //    //var clinicDetail = 
        //    if (doctorEntity != null)
        //    {
        //        if (doctorEntity.IsActive != null)
        //            doctorModel.Active = doctorEntity.IsActive == true ? AppCons.Active : AppCons.Inactive;

        //        doctorModel.DoctorID = doctorEntity.ExternalCode;
        //        doctorModel.License2 = string.Empty;
        //        doctorModel.License2Expiry = string.Empty;
        //        doctorModel.DoctorName = doctorEntity.DoctorName;
        //        doctorModel.Gender = doctorEntity.Gender != null ? doctorEntity.Gender.Value.ToString() : string.Empty;
        //        doctorModel.DateofJoining = doctorEntity.DateofJoining != null
        //                                    ? doctorEntity.DateofJoining.Value.ToNaphiesFormat()
        //                                    : string.Empty;
        //        doctorModel.RoleCode = DoctorsInfo.RoleCode;
        //        doctorModel.RoleDisplay = DoctorsInfo.RoleDisplay;
        //        doctorModel.SpecialityCode = Convert.ToString(claim.ClinicId);//DoctorsInfo.SpecialityCode;
        //        doctorModel.SpecialityDisplay = DoctorsInfo.SpecialityDisplay;

        //        var licenses = doctorEntity.RcmDoctorLicenses.ToList();
        //        if (licenses != null && licenses.Count > 0)
        //        {
        //            foreach (var license in licenses)
        //            {
        //                if (license.LicenseType == (byte)DoctorLicenseType.SCFHS)
        //                {
        //                    doctorModel.License1 = license.LicenseNumber;
        //                    doctorModel.License1Expiry = license.LicenseExpiryDate != null
        //                                           ? license.LicenseExpiryDate.Value.ToNaphiesFormat()
        //                                           : string.Empty;
        //                }
        //                else if (license.LicenseType == (byte)DoctorLicenseType.MOH)
        //                {
        //                    doctorModel.License2 = license.LicenseNumber;
        //                    doctorModel.License2Expiry = license.LicenseExpiryDate != null
        //                                           ? license.LicenseExpiryDate.Value.ToNaphiesFormat()
        //                                           : string.Empty;
        //                }
        //            }
        //        }

        //    }

        //    return doctorModel;
        //}

        private static Practitioner DoctorDetail(RcmClaim claim, RcmDoctor[] doctors, RcmClinic[] clinics)
        {
            try
            {
                Practitioner doctorModel = new Practitioner();

                var doctorEntity = doctors.FirstOrDefault(x => x.DoctorId == claim.DoctorId);
                var clinicEntity = clinics.FirstOrDefault(x => x.ClinicId == claim.ClinicId);
                //var clinicDetail = 
                if (doctorEntity != null)
                {
                    if (doctorEntity.IsActive != null)
                        doctorModel.Active = doctorEntity.IsActive == true ? AppCons.Active : AppCons.Inactive;

                    doctorModel.DoctorID = doctorEntity.ExternalCode;
                    // doctorModel.License2 = string.Empty;
                    //doctorModel.License2Expiry = string.Empty;
                    doctorModel.DoctorName = doctorEntity.DoctorName;
                    doctorModel.Gender = doctorEntity.Gender != null ? doctorEntity.Gender.Value.ToString() : string.Empty;
                    doctorModel.DateofJoining = doctorEntity.DateofJoining != null
                                                ? doctorEntity.DateofJoining.Value.ToNaphiesFormat()
                                                : string.Empty;
                    doctorModel.RoleCode = DoctorsInfo.RoleCode;
                    doctorModel.RoleDisplay = DoctorsInfo.RoleDisplay;
                    doctorModel.SpecialityCode = Convert.ToString(clinicEntity.ExternalCode);//DoctorsInfo.SpecialityCode;
                    doctorModel.SpecialityDisplay = DoctorsInfo.SpecialityDisplay;
                    if (claim.EncounterType == (byte)Enounter.Inpatient)
                    {


                        if (!string.IsNullOrWhiteSpace(claim.MedicalJsonData))
                        {
                            JObject dischargeSummary = RetriveDischargeSummary(claim.MedicalJsonData);
                            if (dischargeSummary != null && dischargeSummary.HasValues)
                            {
                                if (!string.IsNullOrWhiteSpace(dischargeSummary["clinicId"].ToString()))
                                {
                                    doctorModel.DischargeSpecialityCode = Convert.ToString(dischargeSummary["clinicId"].ToString());
                                }
                            }
                        }
                    }

                    var licenses = doctorEntity.RcmDoctorLicenses.ToList();
                    if (licenses != null && licenses.Count > 0)
                    {
                        foreach (var license in licenses)
                        {
                            if (license.LicenseType == (byte)DoctorLicenseType.SCFHS)
                            {
                                doctorModel.License1 = license.LicenseNumber;
                                doctorModel.License1Expiry = license.LicenseExpiryDate != null
                                                       ? license.LicenseExpiryDate.Value.ToNaphiesFormat()
                                                       : string.Empty;
                            }
                            else if (license.LicenseType == (byte)DoctorLicenseType.MOH)
                            {
                                doctorModel.License2 = license.LicenseNumber;
                                doctorModel.License2Expiry = license.LicenseExpiryDate != null
                                                       ? license.LicenseExpiryDate.Value.ToNaphiesFormat()
                                                       : string.Empty;
                            }
                        }
                    }

                }

                return doctorModel;
            }
            catch (Exception ex)
            {
                string customMessage = StringConstant.DoctorDetailError + $" {ex.Message}";
                throw new Exception(customMessage, ex);
            }
        }

        private async Task<Clinic> ClinicDetail(RcmClaim claim, RcmClinic[] clinics)
        {
            var clinicEntity = clinics.Where(clinic => clinic.ClinicId == claim.ClinicId).FirstOrDefault();
            if (clinicEntity == null)
            {
                var clinic = await context.RcmClinics.FirstOrDefaultAsync(clinic => clinic.ClinicId == claim.ClinicId);
                if (clinic == null)
                {
                    throw new ArgumentNullException("Clinic Not Mapped");
                }
                else
                {
                    Clinic clinicModel = new Clinic()
                    {
                        ClinicID = clinic.ExternalCode.ToString(),
                        ClinicName = clinic.ClinicName,
                        ClinicNameN = clinic.ClinicNameN
                    };

                    return clinicModel;
                }
            }

            else
            {
                Clinic clinicModel = new Clinic()
                {
                    ClinicID = clinicEntity.ExternalCode.ToString(),
                    ClinicName = clinicEntity.ClinicName,
                    ClinicNameN = clinicEntity.ClinicNameN
                };

                return clinicModel;
            }

        }

        private Patient PatientDetail(RcmClaim claim)
        {
            try
            {
                RcmClaimPatientDetail patient = claim.RcmClaimPatientDetail;
                Patient patientDetail = new Patient();
                patientDetail.PatientID = patient.PatientFileNumber;
                patientDetail.PatientIdentificationType = patient.PatientIdentificationNo.ToString();
                patientDetail.PatientIdentificationDescription = string.Empty;
                patientDetail.PatientIdentificationNo = patient.NationalityId.ToString().Trim();
                patientDetail.STATUS = AppCons.Active;
                patientDetail.FirstName = patient.PatientName;
                patientDetail.MiddleName = string.Empty;
                patientDetail.LastName = !string.IsNullOrWhiteSpace(patient.PatientSurnameFamilyName) ? patient.PatientSurnameFamilyName : IsString(patient.PatientName);
                patientDetail.FirstNameAr = patient.PatientNameN;
                patientDetail.MiddleNameAr = string.Empty;
                patientDetail.LastNameAr = !string.IsNullOrWhiteSpace(patient.FamilyNameN) ? patient.FamilyNameN : IsString(patient.PatientNameN);
                patientDetail.PhoneResi = string.Empty;
                patientDetail.PhoneOffice = string.Empty;
                patientDetail.MobileNumber = IsString(patient.PatientMobileNo);
                patientDetail.EmailAddress = string.Empty;
                patientDetail.DateofBirth = patient.PatientDob.Value.ToNaphiesFormat();
                patientDetail.Gender = patient.PatientGender != null ? patient.PatientGender.Value.ToString() : string.Empty;
                patientDetail.POBox = string.Empty;
                patientDetail.ZipCode = string.Empty;
                patientDetail.Address = IsString(patient.Address1);
                patientDetail.ISOCountryID = IsString(patient.PatientNationality);
                patientDetail.MaritalStatus = patient.MaritalStatus != null ? patient.MaritalStatus.ToString() : string.Empty;
                patientDetail.PatientOccuption = patient.PatientOccuption;
                patientDetail.CauseOfDeath = patient.CauseOfDeath;
                patientDetail.PolicyHolderName = patient.PolicyHolderName;
                patientDetail.PolicyHolderNo = patient.PolicyHolderNo;
                return patientDetail;
            }
            catch (Exception ex)
            {
                logger.LogError(nameof(ClaimDetailsAsync) + "_" + nameof(PatientDetail) + ex.Message);
                //  throw new Exception(StringConstant.PatientDetailError + $" {ex.Message}", ex);
                return new Patient();
            }
        }

        private async ValueTask<Payer> ClaimPayers(RcmClaim claim, string adaptorCode)
        {
            try
            {
                Payer payerModel = new Payer();
                if (claim.PayerId.ToString() is not null)
                {
                    var payerEntity = await GetPayerEntity(claim);
                    var adapterMapping = await GetPayerAdapterMapping(payerEntity, adaptorCode);

                    if (adapterMapping == null)
                        return payerModel;

                    if (payerEntity != null)
                    {
                        Tuple<string, string, string> policyLicenseAndName = await RetriveInsuranceNhicForTPA(claim, payerEntity, adapterMapping);
                        payerModel.CompanyID = String.Format("{0}-{1}", IsString(payerEntity.ExternalCode), claim.FacilityId);
                        payerModel.ORGType = payerEntity.PayerType != null ? payerEntity.PayerType.ToString() : string.Empty;
                        payerModel.CompanyStatus = AppCons.Active;
                        payerModel.PatientCardID = IsString(claim.RcmClaimPatientDetail.PatientMembershipNumber);

                        if (!string.IsNullOrWhiteSpace(policyLicenseAndName.Item3))
                            payerModel.CompanyName = IsString(policyLicenseAndName.Item3);
                        else
                            payerModel.CompanyName = IsString(payerEntity.PayerName);

                        #region License
                        payerModel.License = policyLicenseAndName.Item1;
                        payerModel.PayerTpaNhicId = policyLicenseAndName.Item2;
                        payerModel.PolicyNumber = claim.PolicyNumber;
                        payerModel.Shadowbillingenable = payerEntity.Shadowbillingenable;
                        //if (Convert.ToBoolean(payerEntity.IsManagedByTPA))
                        //{
                        //    if (!string.IsNullOrWhiteSpace(policyLicenseAndName.Item1))
                        //        payerModel.PayerTpaNhicId = policyLicenseAndName.Item1;

                        //    payerModel.License = adapterMapping != null ? IsString(adapterMapping.ParameterValue) : null;
                        //}
                        //else
                        //{
                        //    payerModel.PayerTpaNhicId = adapterMapping != null ? IsString(adapterMapping.ParameterValue) : null;

                        //    if (!string.IsNullOrWhiteSpace(policyLicenseAndName.Item1))
                        //        payerModel.License = policyLicenseAndName.Item1;
                        //    else
                        //        payerModel.License = adapterMapping != null ? IsString(adapterMapping.ParameterValue) : null;
                        //}
                        #endregion


                    }
                }
                return payerModel;
            }
            catch (Exception ex)
            {
                logger.LogError(nameof(ClaimDetailsAsync) + "_" + nameof(ClaimPayers) + ex.Message);
                // throw new Exception(StringConstant.ClaimPayersError + $" {ex.Message}", ex);
                return await Task.FromResult(new Payer());
            }
        }



        private async Task<RcmAdapterMapping> GetPayerAdapterMapping(RcmPayer payerEntity, string adaptorCode)
        {
            try
            {
                var adapter = context.RcmAdapters.Where(x => x.AdaptorCode == adaptorCode && x.OrganizationId == payerEntity.OrganizationId).FirstOrDefault();
                if (adapter == null)
                    throw new Exception(StringConstant.AdapterError);

                return await context.RcmAdapterMappings.Where(x => x.ParameterType == payerEntity.PayerId.ToString()
                                                   && x.ParameterMasterId == (byte)AdaptorType.Payer
                                                   && x.AdapterId == adapter.AdapterId)
                                                .FirstOrDefaultWithNoLockAsync();
            }
            catch (Exception ex)
            {
                logger.LogError(nameof(ClaimDetailsAsync) + "_" + nameof(GetPayerAdapterMapping) + ex.Message);
                throw new Exception(StringConstant.AdapterError + $" {ex.Message}", ex);
            }

        }
        private async Task<RcmAdapterMapping> GetProviderAdapterMapping(RcmFacility facilityEntity, string adaptorCode)
        {
            try
            {
                var adapter = context.RcmAdapters.Where(x => x.AdaptorCode == adaptorCode && x.OrganizationId == facilityEntity.OrganizationId).FirstOrDefault();
                if (adapter == null)
                    throw new Exception(StringConstant.AdapterError);

                return await context.RcmAdapterMappings.Where(x => x.ParameterType == facilityEntity.FacilityId.ToString()
                                                && x.ParameterMasterId == (byte)AdaptorType.Provider
                                                && x.AdapterId == adapter.AdapterId)
                                            .FirstOrDefaultWithNoLockAsync();
            }
            catch (Exception ex)
            {
                logger.LogError(nameof(ClaimDetailsAsync) + "_" + nameof(GetPayerAdapterMapping) + ex.Message);
                throw new Exception(StringConstant.AdapterError + $" {ex.Message}", ex);
            }
        }

        private async Task<RcmPayer> GetPayerEntity(RcmClaim claim)
        {
            return await context.RcmPayers.Where(x => x.PayerId == claim.PayerId)
                .FirstOrDefaultWithNoLockAsync();
        }

        private async Task<Tuple<string, string, string>> RetriveInsuranceNhicForTPA(RcmClaim claim
                        , RcmPayer payer,
                        RcmAdapterMapping rcmAdapterMapping)
        {
            string insNhicId = string.Empty;
            string tpaNhicId = string.Empty;
            string insOrTpaNhicId = string.Empty;
            int payerId = claim.PayerId;
            bool isManagedByTPA = false;
            bool isTPA = false;

            if (claim is not null)
            {


                if (payer != null)
                {
                    isTPA = payer.IsTPA ?? false;
                    isManagedByTPA = payer.IsManagedByTPA ?? false;
                    insOrTpaNhicId = rcmAdapterMapping.ParameterValue;
                }
                var payerPolicy = await context.RcmPayerPolicies
                    .AsNoTracking()
                   .Where(policy => policy.PayerId == claim.PayerId
                    && policy.PayerPolicyId == claim.PayerPolicyId
                    && policy.IsActive == true)
                   .FirstOrDefaultAsync();
                if (payerPolicy != null)
                {
                    if (!isTPA && !isManagedByTPA)
                    {
                        insNhicId = insOrTpaNhicId;
                        tpaNhicId = insOrTpaNhicId;
                        return Tuple.Create(insNhicId, tpaNhicId, payer.PayerName);
                    }
                    else if (isTPA && !isManagedByTPA)
                    {
                        tpaNhicId = insOrTpaNhicId;
                        insNhicId = payerPolicy.InsuranceNhic;
                        return Tuple.Create(insNhicId, tpaNhicId, payerPolicy.PayerPolicyName);
                    }
                    else if (!isTPA && isManagedByTPA)
                    {
                        insNhicId = insOrTpaNhicId;
                        tpaNhicId = !string.IsNullOrWhiteSpace(payerPolicy.InsuranceNhic) ? payerPolicy.InsuranceNhic : string.Empty;
                        return Tuple.Create(insNhicId, tpaNhicId, payerPolicy.PayerPolicyName);
                    }
                }





            }
            return Tuple.Create(string.Empty, string.Empty, string.Empty);
        }

        private ChiefComplaints ClaimChiefComplaint(RcmClaim claim)
        {
            try
            {
                ChiefComplaints chiefComplaint = new ChiefComplaints();

                if (claim.RcmClaimVitalSign != null && claim.EncounterType != 2) // For inpatient chief complaient is not important
                {
                    RcmClaimVitalSign rcmChiefComplaint = claim.RcmClaimVitalSign;

                    chiefComplaint.ChiefComplaint = IsString(rcmChiefComplaint.MainSymptoms);
                    chiefComplaint.CurrentMedication = string.Empty;
                    chiefComplaint.Status = AppCons.Active;
                    chiefComplaint.Comment = IsString(rcmChiefComplaint.PhysicianNotesConditions);
                    chiefComplaint.OtherCondition = string.Empty;
                    chiefComplaint.SignificantSigns = IsString(rcmChiefComplaint.SignificantSigns);

                }
                return chiefComplaint;
            }
            catch (Exception ex)
            {
                throw new Exception(StringConstant.ClaimChiefComplaintError + $" {ex.Message}", ex);
            }
        }
        private VitalSign ClaimVitalSign(RcmClaim claim)
        {
            try
            {
                VitalSign vitalSign = new VitalSign();
                RcmClaimVitalSign vitals = claim.RcmClaimVitalSign;

                if (claim.RcmClaimVitalSign != null)
                {
                    if (vitals.BloodPressure != null && vitals.BloodPressure.Contains("/"))
                    {
                        string[] bloodPres = vitals.BloodPressure.Split("/");
                        vitalSign.BloodPressureLower = bloodPres[0];
                        vitalSign.BloodPressureHigher = bloodPres[1];
                    }
                    vitalSign.HeightCm = vitals.Height != null ? vitals.Height.ToString() : string.Empty;
                    vitalSign.WeightKg = vitals.Weight != null ? vitals.Weight.ToString() : string.Empty;
                    vitalSign.Temperature = vitals.Temperature != null ? vitals.Temperature.ToString() : string.Empty;
                    vitalSign.Pluse = vitals.Pulse != null ? vitals.Pulse.ToString() : string.Empty;
                    vitalSign.VitalSignDate = vitals.VitalSignCreatedOn != null ? vitals.VitalSignCreatedOn.Value.ToString() : string.Empty;
                    vitalSign.RespiratoryRate = vitals.RespiratoryRate;
                    vitalSign.OxygenSaturation = vitals.OxygenSaturation;
                    vitalSign.InvestigationResult = vitals.InvestigationResult;
                    vitalSign.TreatmentPlan = vitals.TreatmentPlan;
                    vitalSign.PatientHistory = vitals.PatientHistory;
                    vitalSign.PhysicalExamination = vitals.PhysicalExamination;
                    vitalSign.HistoryOfPresentIllness = vitals.HistoryOfPresentIllness;
                }
                return vitalSign;
            }
            catch (Exception ex)
            {
                string errorMessage = string.Format("{0}-{1}-{2}", nameof(ClaimVitalSign), claim.ClaimId, ex.Message);
                this.logger.LogError(ex.Message);
                // throw new Exception(StringConstant.VitalSignError + $" {ex.Message}", ex);
                return new VitalSign();
            }

        }
        private List<Diagnosis> ClaimDiagnosis(RcmClaim claim)
        {
            List<Diagnosis> claimDiagnosis = new List<Diagnosis>();
            if (claim.RcmClaimDiagnoses != null && claim.RcmClaimDiagnoses.Count > 0)
            {
                int seq = 1;
                bool isMorphology = false;
                bool isRTADiagnosis = false;
                foreach (var diagnosis in claim.RcmClaimDiagnoses.Where(x => x.IsActive == true))
                {
                    List<Diagnosis> alreadyDiagnosis = ValidateDiagnosis(claimDiagnosis, diagnosis);
                    if (alreadyDiagnosis != null && alreadyDiagnosis.Count() > 0)
                        continue;
                    RcmDiagnosis rcmDiagnosis = RetriveDiagnosis(diagnosis.DiagnosisCode.Trim());
                    if (rcmDiagnosis != null)
                    {
                        isMorphology = rcmDiagnosis.IsMorphology.HasValue && rcmDiagnosis.IsMorphology.Value ? true : false;
                        isRTADiagnosis = rcmDiagnosis.IsRTADiagnosis.HasValue && rcmDiagnosis.IsRTADiagnosis.Value ? true : false;
                    }

                    if (seq == 1)
                    {




                        claimDiagnosis.Add(new Diagnosis
                        {
                            ICD10CM = IsString(diagnosis.DiagnosisCode),
                            SequenceNo = Convert.ToString(seq),
                            TypeCode = "principal",
                            TypeDescription = "Principal Diagnosis",
                            ConditionOnset = diagnosis.ConditionOnset,
                            IsMorphology = isMorphology,
                            IsRTADiagnosis = isRTADiagnosis,
                            MorphologyCode = isMorphology ? RetriveMorphologyCode(diagnosis) : "M8000/3"
                        });
                    }
                    else
                    {
                        claimDiagnosis.Add(new Diagnosis
                        {
                            ICD10CM = IsString(diagnosis.DiagnosisCode),
                            SequenceNo = Convert.ToString(seq),
                            TypeCode = "differential",
                            TypeDescription = "Differential Diagnosis",
                            ConditionOnset = diagnosis.ConditionOnset,
                            IsMorphology = isMorphology,
                            IsRTADiagnosis = isRTADiagnosis,
                            MorphologyCode = isMorphology ? RetriveMorphologyCode(diagnosis) : "M8000/3"

                        });
                    }
                    seq++;
                }
            }

            return claimDiagnosis;
        }

        private RcmDiagnosis RetriveDiagnosis(string icdCode)
        {
            return context.RcmDiagnoses.AsNoTracking()
                                    .Where(x => x.DiagnosisCode.Trim() == icdCode)
                                    .FirstOrDefault();
        }

        private bool? IsCodebelongToMorphology(string diagnoisInfoCode)
        {
            throw new NotImplementedException();
        }

        private static string RetriveMorphologyCode(RcmClaimDiagnosis diagnosis)
        {
            return diagnosis.MorphologyCode;
        }
        private static string RetriveDRGMorphologyCode(CoDskEncounterDiagnosis diagnosis)
        {
            return "M8000/3";
        }

        private List<Diagnosis> ClaimDiagnosisByClinic(string clainicId)
        {
            List<Diagnosis> claimDiagnosis = new List<Diagnosis>();
            int seq = 1;
            switch (clainicId)
            {
                case "23":
                case "261":
                case "618":
                    claimDiagnosis.Add(new Diagnosis
                    {
                        ICD10CM = "M25.5",
                        SequenceNo = Convert.ToString(seq),
                        TypeCode = "principal",
                        TypeDescription = "Principal Diagnosis"
                    });
                    break;
                case "24":

                    claimDiagnosis.Add(new Diagnosis
                    {
                        ICD10CM = "Z51.8",
                        SequenceNo = Convert.ToString(seq),
                        TypeCode = "principal",
                        TypeDescription = "Principal Diagnosis"
                    });
                    break;
                case "262":

                    claimDiagnosis.Add(new Diagnosis
                    {
                        ICD10CM = "Z01.0",
                        SequenceNo = Convert.ToString(seq),
                        TypeCode = "principal",
                        TypeDescription = "Principal Diagnosis"
                    });
                    break;
                case "263":

                    claimDiagnosis.Add(new Diagnosis
                    {
                        ICD10CM = "Z51.8",
                        SequenceNo = Convert.ToString(seq),
                        TypeCode = "principal",
                        TypeDescription = "Principal Diagnosis"
                    });
                    break;
                case "93":
                case "50":
                    claimDiagnosis.Add(new Diagnosis
                    {
                        ICD10CM = "Z51.8",
                        SequenceNo = Convert.ToString(seq),
                        TypeCode = "principal",
                        TypeDescription = "Principal Diagnosis"
                    });
                    break;
                case "11":
                    claimDiagnosis.Add(new Diagnosis
                    {
                        ICD10CM = "F48",
                        SequenceNo = Convert.ToString(seq),
                        TypeCode = "principal",
                        TypeDescription = "Principal Diagnosis"
                    });
                    break;

                case "38":
                    claimDiagnosis.Add(new Diagnosis { ICD10CM = "J45.9", SequenceNo = Convert.ToString(seq), TypeCode = "principal", TypeDescription = "Principal Diagnosis" });
                    break;
                case "108":
                    claimDiagnosis.Add(new Diagnosis { ICD10CM = "E66", SequenceNo = Convert.ToString(seq), TypeCode = "principal", TypeDescription = "Principal Diagnosis" }); break;
                case "21":
                    claimDiagnosis.Add(new Diagnosis { ICD10CM = "R07.3", SequenceNo = Convert.ToString(seq), TypeCode = "principal", TypeDescription = "Principal Diagnosis" }); break;
                case "17":
                    claimDiagnosis.Add(new Diagnosis { ICD10CM = "Z01.2", SequenceNo = Convert.ToString(seq), TypeCode = "principal", TypeDescription = "Principal Diagnosis" }); break;
                case "5":
                    claimDiagnosis.Add(new Diagnosis { ICD10CM = "R23", SequenceNo = Convert.ToString(seq), TypeCode = "principal", TypeDescription = "Principal Diagnosis" }); break;
                case "70":
                    claimDiagnosis.Add(new Diagnosis { ICD10CM = "Z51.9", SequenceNo = Convert.ToString(seq), TypeCode = "principal", TypeDescription = "Principal Diagnosis" }); break;
                case "14":
                    claimDiagnosis.Add(new Diagnosis { ICD10CM = "E11", SequenceNo = Convert.ToString(seq), TypeCode = "principal", TypeDescription = "Principal Diagnosis" }); break;
                case "130":
                    claimDiagnosis.Add(new Diagnosis { ICD10CM = "E10", SequenceNo = Convert.ToString(seq), TypeCode = "principal", TypeDescription = "Principal Diagnosis" }); break;
                case "53":
                    claimDiagnosis.Add(new Diagnosis { ICD10CM = "E11", SequenceNo = Convert.ToString(seq), TypeCode = "principal", TypeDescription = "Principal Diagnosis" }); break;
                case "7":
                    claimDiagnosis.Add(new Diagnosis { ICD10CM = "Z01.1", SequenceNo = Convert.ToString(seq), TypeCode = "principal", TypeDescription = "Principal Diagnosis" }); break;
                case "20":
                    claimDiagnosis.Add(new Diagnosis { ICD10CM = "E66", SequenceNo = Convert.ToString(seq), TypeCode = "principal", TypeDescription = "Principal Diagnosis" }); break;
                case "9":
                    claimDiagnosis.Add(new Diagnosis { ICD10CM = "A09.9", SequenceNo = Convert.ToString(seq), TypeCode = "principal", TypeDescription = "Principal Diagnosis" }); break;
                case "25":
                    claimDiagnosis.Add(new Diagnosis { ICD10CM = "Z51", SequenceNo = Convert.ToString(seq), TypeCode = "principal", TypeDescription = "Principal Diagnosis" }); break;
                case "26":
                    claimDiagnosis.Add(new Diagnosis { ICD10CM = "Z51", SequenceNo = Convert.ToString(seq), TypeCode = "principal", TypeDescription = "Principal Diagnosis" }); break;
                case "47":
                    claimDiagnosis.Add(new Diagnosis { ICD10CM = "Z00.8", SequenceNo = Convert.ToString(seq), TypeCode = "principal", TypeDescription = "Principal Diagnosis" }); break;
                case "15":
                    claimDiagnosis.Add(new Diagnosis { ICD10CM = "Z51.9", SequenceNo = Convert.ToString(seq), TypeCode = "principal", TypeDescription = "Principal Diagnosis" }); break;
                case "1":
                    claimDiagnosis.Add(new Diagnosis { ICD10CM = "D50", SequenceNo = Convert.ToString(seq), TypeCode = "principal", TypeDescription = "Principal Diagnosis" }); break;
                case "13":
                    claimDiagnosis.Add(new Diagnosis { ICD10CM = "Z01.7", SequenceNo = Convert.ToString(seq), TypeCode = "principal", TypeDescription = "Principal Diagnosis" }); break;
                case "30":
                    claimDiagnosis.Add(new Diagnosis { ICD10CM = "N28", SequenceNo = Convert.ToString(seq), TypeCode = "principal", TypeDescription = "Principal Diagnosis" }); break;
                case "110":
                    claimDiagnosis.Add(new Diagnosis { ICD10CM = "G47.8", SequenceNo = Convert.ToString(seq), TypeCode = "principal", TypeDescription = "Principal Diagnosis" }); break;
                case "59":
                    claimDiagnosis.Add(new Diagnosis { ICD10CM = "B99", SequenceNo = Convert.ToString(seq), TypeCode = "principal", TypeDescription = "Principal Diagnosis" }); break;
                case "36":
                    claimDiagnosis.Add(new Diagnosis { ICD10CM = "D50.8", SequenceNo = Convert.ToString(seq), TypeCode = "principal", TypeDescription = "Principal Diagnosis" }); break;
                case "41":
                    claimDiagnosis.Add(new Diagnosis { ICD10CM = "R51", SequenceNo = Convert.ToString(seq), TypeCode = "principal", TypeDescription = "Principal Diagnosis" }); break;
                case "501":
                    claimDiagnosis.Add(new Diagnosis { ICD10CM = "Z71.9", SequenceNo = Convert.ToString(seq), TypeCode = "principal", TypeDescription = "Principal Diagnosis" }); break;

                case "40":
                    claimDiagnosis.Add(new Diagnosis { ICD10CM = "Z01.4", SequenceNo = Convert.ToString(seq), TypeCode = "principal", TypeDescription = "Principal Diagnosis" }); break;
                case "3":
                    claimDiagnosis.Add(new Diagnosis { ICD10CM = "Z01.4", SequenceNo = Convert.ToString(seq), TypeCode = "principal", TypeDescription = "Principal Diagnosis" }); break;
                case "255":
                    claimDiagnosis.Add(new Diagnosis { ICD10CM = "C50.9", SequenceNo = Convert.ToString(seq), TypeCode = "principal", TypeDescription = "Principal Diagnosis" }); break;
                case "31":
                    claimDiagnosis.Add(new Diagnosis { ICD10CM = "F41", SequenceNo = Convert.ToString(seq), TypeCode = "principal", TypeDescription = "Principal Diagnosis" }); break;
                case "6":
                    claimDiagnosis.Add(new Diagnosis { ICD10CM = "Z01.0", SequenceNo = Convert.ToString(seq), TypeCode = "principal", TypeDescription = "Principal Diagnosis" }); break;
                case "12":
                    claimDiagnosis.Add(new Diagnosis { ICD10CM = "M25.5", SequenceNo = Convert.ToString(seq), TypeCode = "principal", TypeDescription = "Principal Diagnosis" }); break;
                case "2":
                    claimDiagnosis.Add(new Diagnosis { ICD10CM = "Z00.1", SequenceNo = Convert.ToString(seq), TypeCode = "principal", TypeDescription = "Principal Diagnosis" }); break;
                case "16":
                    claimDiagnosis.Add(new Diagnosis { ICD10CM = "Z48.0", SequenceNo = Convert.ToString(seq), TypeCode = "principal", TypeDescription = "Principal Diagnosis" }); break;
                case "43":
                    claimDiagnosis.Add(new Diagnosis { ICD10CM = "R05", SequenceNo = Convert.ToString(seq), TypeCode = "principal", TypeDescription = "Principal Diagnosis" }); break;
                case "19":
                    claimDiagnosis.Add(new Diagnosis { ICD10CM = "Z01.6", SequenceNo = Convert.ToString(seq), TypeCode = "principal", TypeDescription = "Principal Diagnosis" }); break;
                case "107":
                    claimDiagnosis.Add(new Diagnosis { ICD10CM = "R53", SequenceNo = Convert.ToString(seq), TypeCode = "principal", TypeDescription = "Principal Diagnosis" }); break;
                case "37":
                    claimDiagnosis.Add(new Diagnosis { ICD10CM = "R23.8", SequenceNo = Convert.ToString(seq), TypeCode = "principal", TypeDescription = "Principal Diagnosis" }); break;
                case "28":
                    claimDiagnosis.Add(new Diagnosis { ICD10CM = "Z50.5", SequenceNo = Convert.ToString(seq), TypeCode = "principal", TypeDescription = "Principal Diagnosis" }); break;
                case "4":
                    claimDiagnosis.Add(new Diagnosis { ICD10CM = "Z98", SequenceNo = Convert.ToString(seq), TypeCode = "principal", TypeDescription = "Principal Diagnosis" }); break;
                case "18":
                    claimDiagnosis.Add(new Diagnosis { ICD10CM = "G43.8", SequenceNo = Convert.ToString(seq), TypeCode = "principal", TypeDescription = "Principal Diagnosis" }); break;
                case "55":
                    claimDiagnosis.Add(new Diagnosis { ICD10CM = "R52.9", SequenceNo = Convert.ToString(seq), TypeCode = "principal", TypeDescription = "Principal Diagnosis" }); break;
                case "27":
                    claimDiagnosis.Add(new Diagnosis { ICD10CM = "M54.5", SequenceNo = Convert.ToString(seq), TypeCode = "principal", TypeDescription = "Principal Diagnosis" }); break;
                case "35":
                    claimDiagnosis.Add(new Diagnosis { ICD10CM = "I83", SequenceNo = Convert.ToString(seq), TypeCode = "principal", TypeDescription = "Principal Diagnosis" }); break;
                case "8":
                    claimDiagnosis.Add(new Diagnosis { ICD10CM = "N39.0", SequenceNo = Convert.ToString(seq), TypeCode = "principal", TypeDescription = "Principal Diagnosis" }); break;
                case "999":
                    claimDiagnosis.Add(new Diagnosis { ICD10CM = "Z51", SequenceNo = Convert.ToString(seq), TypeCode = "principal", TypeDescription = "Principal Diagnosis" }); break;


            }



            return claimDiagnosis;
        }
        private static List<Diagnosis> ValidateDiagnosis(List<Diagnosis> claimDiagnosis, RcmClaimDiagnosis diagnosis)
        {
            return claimDiagnosis.Where(d => d.ICD10CM.Trim() == IsString(diagnosis.DiagnosisCode)).ToList();
        }
        private JArray RetriveComponentInvoices(int organizationId, RcmClaim claim)
        {
            if (string.IsNullOrWhiteSpace(claim?.MedicalJsonData))
                return null;

            try
            {
                var medicalFile = JObject.Parse(claim.MedicalJsonData);
                return medicalFile?["componentsInvoice"] as JArray;
            }
            catch (JsonException)
            {
                // Optional: Log error for invalid JSON format
                return null;
            }
        }
       
        private async Task<List<ClaimItem>> ClaimServices(
            int organizationId,
            RcmClaim claim,
            IQueryable<RcmServiceCatalog> serviceCatalogs,
            RcmStandardCode[] standardCodes,
            JArray jcomponentInvoice, JArray jlaboratory)
        {
            List<ClaimItem> claimDetail = new List<ClaimItem>();
            if (claim.RcmClaimServicesCategories != null && claim.RcmClaimServicesCategories.Count > 0)
            {
                int sequenceNo = 1; //claim.RcmClaimServicesDetails.Where(x => x.IsDeleted != true && x.IsRefund != true && x.IsReturn != true && x.ServiceCode != "05005009" && x.ServiceCompanyShare > 0).Count();
                bool ismohcategory = false;
                foreach (var service in claim.RcmClaimServicesCategories)
                {


                    if (claim.isMoh.HasValue && claim.isMoh.Value)
                        ismohcategory = await IsServiceBelongToMOH(service, claim.OrganizationId);

                    string scientificCodes = string.Empty;
                    if (claim.ClaimType == (byte)ClaimType.Pharmacy)
                        scientificCodes = RetriveScientificCodes(service.ServiceCode);

                    string lOINC_Code = await RetriveLoincCode(service.ServiceCode, claim.OrganizationId);

                    string resultValue = string.Empty;
                    string testName = string.Empty;
                    if (!string.IsNullOrWhiteSpace(lOINC_Code))
                    {
                        if (jlaboratory != null && jlaboratory.Count > 0)
                        {
                            var result = jlaboratory.FirstOrDefault(predicate =>
                                        predicate["invoiceNo"].ToString() == service.ServiceReferenceNumber
                                     && predicate["serviceCode"].ToString().Trim() == service.ServiceCode.Trim()
                                      );
                            if (result != null && result.HasValues)
                            {

                                var match = Regex.Match(result["labResult"].ToString(), @"\d+(\.\d+)?");

                                if (match.Success)
                                {

                                    decimal hbaValue = Convert.ToDecimal(match.Value);
                                    if (hbaValue > 15)
                                        hbaValue = 15;

                                    resultValue = hbaValue.ToString();
                                    testName = result["testName"].ToString();
                                }
                                else
                                {
                                    RcmLabResult rcmLabResult = RetriveLabResult(claim, service);
                                    if (rcmLabResult != null)
                                    {
                                        resultValue = rcmLabResult.LabResult.ToString();
                                        testName = string.Empty;

                                    }
                                }

                            }
                            else
                            {
                                RcmLabResult rcmLabResult = RetriveLabResult(claim, service);
                                if (rcmLabResult != null)
                                {
                                    resultValue = rcmLabResult.LabResult.ToString();
                                    testName = string.Empty;

                                }
                            }
                        }
                        else // Read from the lab_result table
                        {
                            RcmLabResult rcmLabResult = RetriveLabResult(claim, service);
                            if (rcmLabResult != null)
                            {
                                resultValue = rcmLabResult.LabResult.ToString();
                                testName = string.Empty;

                            }
                        }
                    }



                    if (service.ServiceQuantity < 1)
                    {
                        service.ServiceQuantity = 1;
                    }
                    var standardCode = standardCodes.FirstOrDefault(Filter(service.ServiceId, organizationId));

                    //MAP remarks and attachment 
                    ResubmissionRemarksAndAttachments remarksData = new ResubmissionRemarksAndAttachments();
                    if (remarksData.remarks != null)
                    {
                        remarksData.remarks = service.ResubmissionData.remarks;
                        if (remarksData.attachment != null)
                            remarksData.attachment = service.ResubmissionData.attachment;
                    }

                    var facvidaid = config["vida4facilities"].ToString();
                    var facvidaidList = facvidaid.Split(',').ToList();

                    if (facvidaidList.Contains(claim.FacilityId.ToString()))
                    {

                        if (!string.IsNullOrWhiteSpace(service.ServiceCode) && service.ServiceCode != "450102527"
                            && service.ServiceCode != "55021001"
                            && service.ParentServiceCode == null)
                        {


                            claimDetail.Add(new ClaimItem
                            {
                                SBSProcedureCode = service.StandardCode != null ? service.StandardCode : string.Empty,
                                SBSCodeDescription = standardCode != null ? IsString(standardCode.StandardCodeDescription) : RetriveServiceCatalogName(service.ServiceId),
                                NphiesProcedureCodeSystem = (service.NphiesCodeType != null && service.NphiesCodeType > 0) ? Convert.ToString(service.NphiesCodeType) :
                                                                                    (service.ServiceType != null ? service.ServiceType.ToString() : string.Empty),
                                InvoiceNo = IsString(service.ServiceReferenceNumber),
                                ProcedureID = string.IsNullOrEmpty(service.StandardCode2) ? service.ServiceCode : service.StandardCode2,
                                ProcedureName = service != null ? IsString(service.ServiceCatalogName) : RetriveServiceCatalogName(service.ServiceId),
                                InvoiceDate = service.InvoiceDate.Value.ToNaphiesFormat(),
                                UnitPrice = Convert.ToDecimal(service.ServiceQuantity) > 0 ?
                                                        IsDecimal(Math.Round(Convert.ToDecimal(service.ServiceGrossAmount) / Convert.ToDecimal(service.ServiceQuantity), 2)) :
                                                        Convert.ToDecimal(service.ServiceGrossAmount).ToString(),
                                Quantity = IsDecimal(service.ServiceQuantity),
                                GrossAmount = IsDecimal(service.ServiceGrossAmount),
                                GrossDisc = IsDecimal(service.ServiceDiscountAmount),
                                PatientShare = IsDecimal(service.ServicePatientShare),
                                CompanyShare = IsDecimal(service.ServiceCompanyShare),
                                VATRate = string.Empty,
                                CompanyTaxAmount = IsDecimal(service.CompanyTax),
                                PatientTaxAmount = IsDecimal(service.PatientTax),
                                ToothNo = IsString(service.ToothNo),

                                DaysSupply = string.Empty,
                                PackageDetail = string.Empty,
                                Sequence = sequenceNo,
                                Factor = CountFactor(Convert.ToDecimal(service.ServiceDiscountAmount), Convert.ToDecimal(service.ServiceQuantity), (Convert.ToDecimal(service.ServiceGrossAmount) / Convert.ToDecimal(service.ServiceQuantity))),//  Math.Round((1 - (Convert.ToDecimal(service.ServiceDiscountPct) / 100)), 2) : 1,
                                NET = NetAmountWithoutTax(Convert.ToDecimal(service.ServiceQuantity), (Convert.ToDecimal(service.ServiceGrossAmount) / Convert.ToDecimal(service.ServiceQuantity)),
                                CountFactor(Convert.ToDecimal(service.ServiceDiscountAmount), Convert.ToDecimal(service.ServiceQuantity), (Convert.ToDecimal(service.ServiceGrossAmount) / Convert.ToDecimal(service.ServiceQuantity))))
                                + RoundCompanyTax(IsDecimal(service.CompanyTax)),

                                packageComponent = PackageComponentInvoicesV4(organizationId, claim.RcmClaimServicesCategories, service.ServiceReferenceNumber, service.ServiceCode),
                                RowId = service.RowId,
                                ApprovalNumber = service.ApprovalNo,
                                ServiceId = service.ServiceId,
                                ResubmissionData = remarksData,
                                ScientificCodes = scientificCodes,
                                ServiceStartDate = service.ServiceStartDate.Value.ToNaphiesFormat(),
                                ServiceEndDate = service.ServiceEndDate.Value.ToNaphiesFormat(),
                                UnCatagoriesService = service.UnCatagoriesService,
                                SupportingInfo = RetrieveSupportingInfo(lOINC_Code, resultValue, testName),
                                IsMohCategory = ismohcategory,
                                MOHServiceCode = service.MOHServiceCode,
                                MOHServiceCodeDescription = service.MOHServiceCodeDescription

                            });
                            sequenceNo++;

                        }
                        else if (!string.IsNullOrEmpty(service.ServiceCode) && service.ParentServiceCode == "450102527" && service.ServiceCode != "450102527")
                        {

                            claimDetail.Add(new ClaimItem
                            {
                                SBSProcedureCode = service.StandardCode != null ? service.StandardCode : string.Empty,
                                SBSCodeDescription = standardCode != null ? IsString(standardCode.StandardCodeDescription) : RetriveServiceCatalogName(service.ServiceId),
                                NphiesProcedureCodeSystem = (service.NphiesCodeType != null && service.NphiesCodeType > 0) ? Convert.ToString(service.NphiesCodeType) : (service.ServiceType != null ? service.ServiceType.ToString() : string.Empty),
                                InvoiceNo = IsString(service.ServiceReferenceNumber),
                                ProcedureID = string.IsNullOrEmpty(service.StandardCode2) ? service.ServiceCode : service.StandardCode2,
                                ProcedureName = service != null ? IsString(service.ServiceCatalogName) : RetriveServiceCatalogName(service.ServiceId),
                                InvoiceDate = service.InvoiceDate.Value.ToNaphiesFormat(),
                                UnitPrice = Convert.ToDecimal(service.ServiceQuantity) > 0 ? IsDecimal(Math.Round(Convert.ToDecimal(service.ServiceGrossAmount) / Convert.ToDecimal(service.ServiceQuantity), 2)) : Convert.ToDecimal(service.ServiceGrossAmount).ToString(),
                                Quantity = IsDecimal(service.ServiceQuantity),
                                GrossAmount = IsDecimal(service.ServiceGrossAmount),
                                GrossDisc = IsDecimal(service.ServiceDiscountAmount),
                                PatientShare = IsDecimal(service.ServicePatientShare),
                                CompanyShare = IsDecimal(service.ServiceCompanyShare),
                                VATRate = string.Empty,
                                CompanyTaxAmount = IsDecimal(service.CompanyTax),
                                PatientTaxAmount = IsDecimal(service.PatientTax),
                                ToothNo = IsString(service.ToothNo),

                                DaysSupply = string.Empty,
                                PackageDetail = string.Empty,
                                Sequence = sequenceNo,
                                Factor = CountFactor(Convert.ToDecimal(service.ServiceDiscountAmount), Convert.ToDecimal(service.ServiceQuantity), (Convert.ToDecimal(service.ServiceGrossAmount) / Convert.ToDecimal(service.ServiceQuantity))),//  Math.Round((1 - (Convert.ToDecimal(service.ServiceDiscountPct) / 100)), 2) : 1,
                                NET = NetAmountWithoutTax(Convert.ToDecimal(service.ServiceQuantity), (Convert.ToDecimal(service.ServiceGrossAmount) / Convert.ToDecimal(service.ServiceQuantity)),
                                CountFactor(Convert.ToDecimal(service.ServiceDiscountAmount), Convert.ToDecimal(service.ServiceQuantity), (Convert.ToDecimal(service.ServiceGrossAmount) / Convert.ToDecimal(service.ServiceQuantity))))
                                + RoundCompanyTax(IsDecimal(service.CompanyTax)),

                                packageComponent = null,
                                RowId = service.RowId,
                                ApprovalNumber = service.ApprovalNo,
                                ServiceId = service.ServiceId,
                                ResubmissionData = remarksData,
                                ScientificCodes = scientificCodes,
                                ServiceStartDate = service.ServiceStartDate.Value.ToNaphiesFormat(),
                                ServiceEndDate = service.ServiceEndDate.Value.ToNaphiesFormat(),
                                UnCatagoriesService = service.UnCatagoriesService,
                                SupportingInfo = RetrieveSupportingInfo(lOINC_Code, resultValue, testName),
                                IsMohCategory = ismohcategory,
                                MOHServiceCode = service.MOHServiceCode,
                                MOHServiceCodeDescription = service.MOHServiceCodeDescription
                            });
                            sequenceNo++;

                        }
                        else if (!string.IsNullOrEmpty(service.ServiceCode) && service.ParentServiceCode == "55021001" && service.ServiceCode != "55021001")
                        {


                            claimDetail.Add(new ClaimItem
                            {
                                SBSProcedureCode = service.StandardCode != null ? service.StandardCode : string.Empty,
                                SBSCodeDescription = standardCode != null ? IsString(standardCode.StandardCodeDescription) : RetriveServiceCatalogName(service.ServiceId),
                                NphiesProcedureCodeSystem = (service.NphiesCodeType != null && service.NphiesCodeType > 0) ? Convert.ToString(service.NphiesCodeType) : (service.ServiceType != null ? service.ServiceType.ToString() : string.Empty),
                                InvoiceNo = IsString(service.ServiceReferenceNumber),
                                ProcedureID = string.IsNullOrEmpty(service.StandardCode2) ? service.ServiceCode : service.StandardCode2,
                                ProcedureName = service != null ? IsString(service.ServiceCatalogName) : RetriveServiceCatalogName(service.ServiceId),
                                InvoiceDate = service.InvoiceDate.Value.ToNaphiesFormat(),
                                UnitPrice = Convert.ToDecimal(service.ServiceQuantity) > 0 ? IsDecimal(Math.Round(Convert.ToDecimal(service.ServiceGrossAmount) / Convert.ToDecimal(service.ServiceQuantity), 2)) : Convert.ToDecimal(service.ServiceGrossAmount).ToString(),
                                Quantity = IsDecimal(service.ServiceQuantity),
                                GrossAmount = IsDecimal(service.ServiceGrossAmount),
                                GrossDisc = IsDecimal(service.ServiceDiscountAmount),
                                PatientShare = IsDecimal(service.ServicePatientShare),
                                CompanyShare = IsDecimal(service.ServiceCompanyShare),
                                VATRate = string.Empty,
                                CompanyTaxAmount = IsDecimal(service.CompanyTax),
                                PatientTaxAmount = IsDecimal(service.PatientTax),
                                ToothNo = IsString(service.ToothNo),

                                DaysSupply = string.Empty,
                                PackageDetail = string.Empty,
                                Sequence = sequenceNo,
                                Factor = CountFactor(Convert.ToDecimal(service.ServiceDiscountAmount), Convert.ToDecimal(service.ServiceQuantity), (Convert.ToDecimal(service.ServiceGrossAmount) / Convert.ToDecimal(service.ServiceQuantity))),//  Math.Round((1 - (Convert.ToDecimal(service.ServiceDiscountPct) / 100)), 2) : 1,
                                NET = NetAmountWithoutTax(Convert.ToDecimal(service.ServiceQuantity), (Convert.ToDecimal(service.ServiceGrossAmount) / Convert.ToDecimal(service.ServiceQuantity)),
                                CountFactor(Convert.ToDecimal(service.ServiceDiscountAmount), Convert.ToDecimal(service.ServiceQuantity), (Convert.ToDecimal(service.ServiceGrossAmount) / Convert.ToDecimal(service.ServiceQuantity))))
                                + RoundCompanyTax(IsDecimal(service.CompanyTax)),
                                //(Convert.ToDecimal(service.ServiceNetAmount) + Convert.ToDecimal(service.CompanyTax)),
                                packageComponent = null,
                                RowId = service.RowId,
                                ApprovalNumber = service.ApprovalNo,
                                ServiceId = service.ServiceId,
                                ResubmissionData = remarksData,
                                ScientificCodes = scientificCodes,
                                ServiceStartDate = service.ServiceStartDate.Value.ToNaphiesFormat(),
                                ServiceEndDate = service.ServiceEndDate.Value.ToNaphiesFormat(),
                                UnCatagoriesService = service.UnCatagoriesService,
                                SupportingInfo = RetrieveSupportingInfo(lOINC_Code, resultValue, testName),
                                IsMohCategory = ismohcategory,
                                MOHServiceCode = service.MOHServiceCode,
                                MOHServiceCodeDescription = service.MOHServiceCodeDescription
                            });
                            sequenceNo++;
                        }
                    }
                    else
                    {

                        if (!string.IsNullOrWhiteSpace(service.ServiceCode) && service.ServiceCode != "450102527" && service.ServiceCode != "55021001")
                        {


                            claimDetail.Add(new ClaimItem
                            {
                                SBSProcedureCode = service.StandardCode != null ? service.StandardCode : string.Empty,
                                SBSCodeDescription = standardCode != null ? IsString(standardCode.StandardCodeDescription) : RetriveServiceCatalogName(service.ServiceId),
                                NphiesProcedureCodeSystem = (service.NphiesCodeType != null && service.NphiesCodeType > 0) ? Convert.ToString(service.NphiesCodeType) :
                                                                                    (service.ServiceType != null ? service.ServiceType.ToString() : string.Empty),
                                InvoiceNo = IsString(service.ServiceReferenceNumber),
                                ProcedureID = string.IsNullOrEmpty(service.StandardCode2) ? service.ServiceCode : service.StandardCode2,
                                ProcedureName = service != null ? IsString(service.ServiceCatalogName) : RetriveServiceCatalogName(service.ServiceId),
                                InvoiceDate = service.InvoiceDate.Value.ToNaphiesFormat(),
                                UnitPrice = Convert.ToDecimal(service.ServiceQuantity) > 0 ?
                                                        IsDecimal(Math.Round(Convert.ToDecimal(service.ServiceGrossAmount) / Convert.ToDecimal(service.ServiceQuantity), 2)) :
                                                        Convert.ToDecimal(service.ServiceGrossAmount).ToString(),
                                Quantity = IsDecimal(service.ServiceQuantity),
                                GrossAmount = IsDecimal(service.ServiceGrossAmount),
                                GrossDisc = IsDecimal(service.ServiceDiscountAmount),
                                PatientShare = IsDecimal(service.ServicePatientShare),
                                CompanyShare = IsDecimal(service.ServiceCompanyShare),
                                VATRate = string.Empty,
                                CompanyTaxAmount = IsDecimal(service.CompanyTax),
                                PatientTaxAmount = IsDecimal(service.PatientTax),
                                ToothNo = IsString(service.ToothNo),

                                DaysSupply = string.Empty,
                                PackageDetail = string.Empty,
                                Sequence = sequenceNo,
                                Factor = CountFactor(Convert.ToDecimal(service.ServiceDiscountAmount), Convert.ToDecimal(service.ServiceQuantity), (Convert.ToDecimal(service.ServiceGrossAmount) / Convert.ToDecimal(service.ServiceQuantity))),//  Math.Round((1 - (Convert.ToDecimal(service.ServiceDiscountPct) / 100)), 2) : 1,
                                NET = NetAmountWithoutTax(Convert.ToDecimal(service.ServiceQuantity), (Convert.ToDecimal(service.ServiceGrossAmount) / Convert.ToDecimal(service.ServiceQuantity)),
                                CountFactor(Convert.ToDecimal(service.ServiceDiscountAmount), Convert.ToDecimal(service.ServiceQuantity), (Convert.ToDecimal(service.ServiceGrossAmount) / Convert.ToDecimal(service.ServiceQuantity))))
                                + RoundCompanyTax(IsDecimal(service.CompanyTax)),

                                packageComponent = PackageComponentInvoices(organizationId, jcomponentInvoice, service.ServiceReferenceNumber, service.ServiceCode),
                                RowId = service.RowId,
                                ApprovalNumber = service.ApprovalNo,
                                ServiceId = service.ServiceId,
                                ResubmissionData = remarksData,
                                ScientificCodes = scientificCodes,
                                ServiceStartDate = service.ServiceStartDate.Value.ToNaphiesFormat(),
                                ServiceEndDate = service.ServiceEndDate.Value.ToNaphiesFormat(),
                                UnCatagoriesService = service.UnCatagoriesService,
                                SupportingInfo = RetrieveSupportingInfo(lOINC_Code, resultValue, testName),
                                IsMohCategory = ismohcategory,
                                MOHServiceCode = service.MOHServiceCode,
                                MOHServiceCodeDescription = service.MOHServiceCodeDescription

                            });
                            sequenceNo++;

                        }
                        else if (!string.IsNullOrEmpty(service.ServiceCode) && service.ParentServiceCode == "450102527" && service.ServiceCode != "450102527")
                        {

                            claimDetail.Add(new ClaimItem
                            {
                                SBSProcedureCode = service.StandardCode != null ? service.StandardCode : string.Empty,
                                SBSCodeDescription = standardCode != null ? IsString(standardCode.StandardCodeDescription) : RetriveServiceCatalogName(service.ServiceId),
                                NphiesProcedureCodeSystem = (service.NphiesCodeType != null && service.NphiesCodeType > 0) ? Convert.ToString(service.NphiesCodeType) : (service.ServiceType != null ? service.ServiceType.ToString() : string.Empty),
                                InvoiceNo = IsString(service.ServiceReferenceNumber),
                                ProcedureID = string.IsNullOrEmpty(service.StandardCode2) ? service.ServiceCode : service.StandardCode2,
                                ProcedureName = service != null ? IsString(service.ServiceCatalogName) : RetriveServiceCatalogName(service.ServiceId),
                                InvoiceDate = service.InvoiceDate.Value.ToNaphiesFormat(),
                                UnitPrice = Convert.ToDecimal(service.ServiceQuantity) > 0 ? IsDecimal(Math.Round(Convert.ToDecimal(service.ServiceGrossAmount) / Convert.ToDecimal(service.ServiceQuantity), 2)) : Convert.ToDecimal(service.ServiceGrossAmount).ToString(),
                                Quantity = IsDecimal(service.ServiceQuantity),
                                GrossAmount = IsDecimal(service.ServiceGrossAmount),
                                GrossDisc = IsDecimal(service.ServiceDiscountAmount),
                                PatientShare = IsDecimal(service.ServicePatientShare),
                                CompanyShare = IsDecimal(service.ServiceCompanyShare),
                                VATRate = string.Empty,
                                CompanyTaxAmount = IsDecimal(service.CompanyTax),
                                PatientTaxAmount = IsDecimal(service.PatientTax),
                                ToothNo = IsString(service.ToothNo),

                                DaysSupply = string.Empty,
                                PackageDetail = string.Empty,
                                Sequence = sequenceNo,
                                Factor = CountFactor(Convert.ToDecimal(service.ServiceDiscountAmount), Convert.ToDecimal(service.ServiceQuantity), (Convert.ToDecimal(service.ServiceGrossAmount) / Convert.ToDecimal(service.ServiceQuantity))),//  Math.Round((1 - (Convert.ToDecimal(service.ServiceDiscountPct) / 100)), 2) : 1,
                                NET = NetAmountWithoutTax(Convert.ToDecimal(service.ServiceQuantity), (Convert.ToDecimal(service.ServiceGrossAmount) / Convert.ToDecimal(service.ServiceQuantity)),
                                CountFactor(Convert.ToDecimal(service.ServiceDiscountAmount), Convert.ToDecimal(service.ServiceQuantity), (Convert.ToDecimal(service.ServiceGrossAmount) / Convert.ToDecimal(service.ServiceQuantity))))
                                + RoundCompanyTax(IsDecimal(service.CompanyTax)),

                                packageComponent = null,
                                RowId = service.RowId,
                                ApprovalNumber = service.ApprovalNo,
                                ServiceId = service.ServiceId,
                                ResubmissionData = remarksData,
                                ScientificCodes = scientificCodes,
                                ServiceStartDate = service.ServiceStartDate.Value.ToNaphiesFormat(),
                                ServiceEndDate = service.ServiceEndDate.Value.ToNaphiesFormat(),
                                UnCatagoriesService = service.UnCatagoriesService,
                                SupportingInfo = RetrieveSupportingInfo(lOINC_Code, resultValue, testName),
                                IsMohCategory = ismohcategory,
                                MOHServiceCode = service.MOHServiceCode,
                                MOHServiceCodeDescription = service.MOHServiceCodeDescription
                            });
                            sequenceNo++;

                        }
                        else if (!string.IsNullOrEmpty(service.ServiceCode) && service.ParentServiceCode == "55021001" && service.ServiceCode != "55021001")
                        {


                            claimDetail.Add(new ClaimItem
                            {
                                SBSProcedureCode = service.StandardCode != null ? service.StandardCode : string.Empty,
                                SBSCodeDescription = standardCode != null ? IsString(standardCode.StandardCodeDescription) : RetriveServiceCatalogName(service.ServiceId),
                                NphiesProcedureCodeSystem = (service.NphiesCodeType != null && service.NphiesCodeType > 0) ? Convert.ToString(service.NphiesCodeType) : (service.ServiceType != null ? service.ServiceType.ToString() : string.Empty),
                                InvoiceNo = IsString(service.ServiceReferenceNumber),
                                ProcedureID = string.IsNullOrEmpty(service.StandardCode2) ? service.ServiceCode : service.StandardCode2,
                                ProcedureName = service != null ? IsString(service.ServiceCatalogName) : RetriveServiceCatalogName(service.ServiceId),
                                InvoiceDate = service.InvoiceDate.Value.ToNaphiesFormat(),
                                UnitPrice = Convert.ToDecimal(service.ServiceQuantity) > 0 ? IsDecimal(Math.Round(Convert.ToDecimal(service.ServiceGrossAmount) / Convert.ToDecimal(service.ServiceQuantity), 2)) : Convert.ToDecimal(service.ServiceGrossAmount).ToString(),
                                Quantity = IsDecimal(service.ServiceQuantity),
                                GrossAmount = IsDecimal(service.ServiceGrossAmount),
                                GrossDisc = IsDecimal(service.ServiceDiscountAmount),
                                PatientShare = IsDecimal(service.ServicePatientShare),
                                CompanyShare = IsDecimal(service.ServiceCompanyShare),
                                VATRate = string.Empty,
                                CompanyTaxAmount = IsDecimal(service.CompanyTax),
                                PatientTaxAmount = IsDecimal(service.PatientTax),
                                ToothNo = IsString(service.ToothNo),

                                DaysSupply = string.Empty,
                                PackageDetail = string.Empty,
                                Sequence = sequenceNo,
                                Factor = CountFactor(Convert.ToDecimal(service.ServiceDiscountAmount), Convert.ToDecimal(service.ServiceQuantity), (Convert.ToDecimal(service.ServiceGrossAmount) / Convert.ToDecimal(service.ServiceQuantity))),//  Math.Round((1 - (Convert.ToDecimal(service.ServiceDiscountPct) / 100)), 2) : 1,
                                NET = NetAmountWithoutTax(Convert.ToDecimal(service.ServiceQuantity), (Convert.ToDecimal(service.ServiceGrossAmount) / Convert.ToDecimal(service.ServiceQuantity)),
                                CountFactor(Convert.ToDecimal(service.ServiceDiscountAmount), Convert.ToDecimal(service.ServiceQuantity), (Convert.ToDecimal(service.ServiceGrossAmount) / Convert.ToDecimal(service.ServiceQuantity))))
                                + RoundCompanyTax(IsDecimal(service.CompanyTax)),
                                //(Convert.ToDecimal(service.ServiceNetAmount) + Convert.ToDecimal(service.CompanyTax)),
                                packageComponent = null,
                                RowId = service.RowId,
                                ApprovalNumber = service.ApprovalNo,
                                ServiceId = service.ServiceId,
                                ResubmissionData = remarksData,
                                ScientificCodes = scientificCodes,
                                ServiceStartDate = service.ServiceStartDate.Value.ToNaphiesFormat(),
                                ServiceEndDate = service.ServiceEndDate.Value.ToNaphiesFormat(),
                                UnCatagoriesService = service.UnCatagoriesService,
                                SupportingInfo = RetrieveSupportingInfo(lOINC_Code, resultValue, testName),
                                IsMohCategory = ismohcategory,
                                MOHServiceCode = service.MOHServiceCode,
                                MOHServiceCodeDescription = service.MOHServiceCodeDescription
                            });
                            sequenceNo++;
                        }

                    }

                }
                ;

            }
            return claimDetail;
        }

        private string RetriveServiceCatalogName(long serviceId)
        {
            return context.RcmServiceCatalogs.AsNoTracking().Where(x => x.ServiceId == serviceId).Select(x => x.ServiceCatalogName)
                .FirstOrDefault();
        }

        private RcmLabResult RetriveLabResult(RcmClaim claim, RcmClaimServicesDetailDTO service)
        {
            return context.RcmLabResults.AsNoTracking().Where(x => x.ClaimId == claim.ClaimId
                                                        && x.FacilityId == claim.FacilityId
                                                        && x.ServiceCode == service.ServiceCode.Trim()
                                                        // && x.ServiceReferenceNumber == service.ServiceReferenceNumber
                                                        && x.IsActive == true)
                                                        .FirstOrDefault();
        }

        private SupportingInfo? RetrieveSupportingInfo(string loincCode, string value, string textName)
        {

            return string.IsNullOrWhiteSpace(loincCode) || string.IsNullOrWhiteSpace(value)
                ? null
                : new SupportingInfo
                {
                    Code = loincCode,
                    Name = textName,
                    Value = value
                };
        }

        private async Task<string> RetriveLoincCode(string serviceCode, int organizationId)
        {
            if (serviceCode.StartsWith("02"))
                return await context.RcmTransactionMapingValues
                    .AsNoTracking()
                    .Where(m => m.OrganizationId == organizationId && m.ExternalCode == serviceCode && m.MappingType == (int)MappingType.LOINC)
                     .Select(x => x.MappingValue)
                    .FirstOrDefaultAsync();
            else
                return string.Empty;

        }

        private async Task<bool> IsServiceBelongToMOH(RcmClaimServicesDetailDTO servicesDetailDTO, int organizationId)
        {
            if (!string.IsNullOrWhiteSpace(servicesDetailDTO.MOHServiceCode))
                return true;

            return await context.RcmTransactionMapingValues
                .AsNoTracking()
                .AnyAsync(m =>
                    m.OrganizationId == organizationId &&
                    m.ExternalCode == servicesDetailDTO.ServiceCode &&
                    m.MappingType == (int)MappingType.MOH);
        }
        public static decimal CountFactor(decimal discountAmount, decimal quantity, decimal UnitPrice)
        {
            if (discountAmount > 0)
            {
                UnitPrice = Math.Round(UnitPrice, 2);
                decimal perecntage = (discountAmount * 100) / (UnitPrice * quantity);

                return Math.Round((1 - (perecntage / 100)), 2);
            }
            else
                return 1;
        }
        public static decimal NetAmountWithoutTax(decimal quantity, decimal UnitPrice, decimal? factor)
        {
            UnitPrice = Math.Round(UnitPrice, 2);
            decimal net = (UnitPrice * quantity) * Convert.ToDecimal(factor);


            return Math.Round(net, 2, MidpointRounding.AwayFromZero);//Convert.ToDecimal(0.001)
        }
        public static decimal RoundCompanyTax(string Tax)
        {
            if (string.IsNullOrWhiteSpace(Tax))
                return 0;

            var decimaltax = Convert.ToDecimal(Tax);
            if (decimaltax > 0)
            {
                return Math.Round(decimaltax, 2);
            }
            else
            {
                return 0;
            }
        }

        private string GetNphiesMappingCode(RcmTransactionMapingValue[] rcmTransactionMapingValues, string externalCode)
        {
            var mappingValue = rcmTransactionMapingValues.FirstOrDefault(x => x.ExternalCode == externalCode);
            if (mappingValue != null && !string.IsNullOrWhiteSpace(mappingValue.MappingValue))
                return mappingValue.MappingValue;
            else
                return string.Empty;
        }
        private static Func<RcmStandardCode, bool> Filter(long serviceId, int orgId)//(RcmClaimServicesDetail service)
        {
            return x => x.ServiceId == serviceId
                                && x.OrganizationId == orgId;
            // && x.StandardCode == service.StandardCode;
        }

        private static Expression<Func<RcmServiceCatalog, bool>> Filter(int organizationId, RcmClaimServicesDetail service)
        {
            return x => x.ServiceId == service.ServiceId && x.OrganizationId == organizationId;
        }

        private List<PackageComponent> PackageComponentInvoices(int organizationId, JArray jcomponentInvoice, string invoiceNo, string serviceCode)
        {
            List<PackageComponent> packages = new List<PackageComponent>();
            if (jcomponentInvoice != null && jcomponentInvoice.Count > 0)
            {
                // int sequenceNo = 1;
                var jfinal = jcomponentInvoice.Where(t => t["parentInvoiceNo"].ToString() == invoiceNo && t["parentServiceCode"].ToString() == serviceCode).ToList();
                foreach (JObject item in jfinal)
                {
                    if (item.HasValues)
                    {
                        var component = new PackageComponent();
                        //    component.SequenceNo = sequenceNo;
                        component.Net = 0;
                        component.UnitPrice = 0;


                        if (!string.IsNullOrWhiteSpace(item["serviceCode"].ToString()))
                            component.ProcedureID = item["serviceCode"].ToString();

                        if (!string.IsNullOrWhiteSpace(item["serviceName"].ToString()))
                            component.ProcedureName = item["serviceName"].ToString();

                        if (!string.IsNullOrWhiteSpace(item["serviceCode"].ToString()))
                            component.SBSProcedureCode = item["serviceCode"].ToString();

                        if (!string.IsNullOrWhiteSpace(item["serviceName"].ToString()))
                            component.SBSCodeDescription = item["serviceName"].ToString();

                        if (!string.IsNullOrWhiteSpace(item["serviceCode"].ToString()))
                            component.NphiesProcedureCodeSystem = item["serviceCode"].ToString();

                        if (!string.IsNullOrWhiteSpace(item["serviceQuantity"].ToString()))
                            component.Quantity = Convert.ToInt32(item["serviceQuantity"].ToString());

                        packages.Add(component);

                    }
                }
            }
            if (packages.Count > 0)
            {
                int sequenceNo = 1;
                var groupedPackages = packages
                .GroupBy(p => p.ProcedureID)
                .Select(g => new PackageComponent
                {
                    SequenceNo = sequenceNo++, // g.First().SequenceNo, 
                    ProcedureID = g.Key,
                    ProcedureName = g.First().ProcedureName,
                    SBSProcedureCode = g.First().SBSProcedureCode,
                    SBSCodeDescription = g.First().SBSCodeDescription,
                    NphiesProcedureCodeSystem = g.First().NphiesProcedureCodeSystem,
                    Quantity = g.Sum(p => p.Quantity),
                    UnitPrice = g.First().UnitPrice,
                    Net = g.First().Net
                })
                .ToList();
                packages = groupedPackages;


            }


            return packages;
        }

        private List<PackageComponent> PackageComponentInvoicesV4(int organizationId, List<RcmClaimServicesDetailDTO> RcmClaimServicesCategories, string invoiceNo, string serviceCode)
        {
            List<PackageComponent> packages = new List<PackageComponent>();

            if (RcmClaimServicesCategories != null && RcmClaimServicesCategories.Count > 0)
            {
                // int sequenceNo = 1;
                var componentsServices = RcmClaimServicesCategories.Where(t => t.ParentServiceCode == serviceCode).ToList();
                foreach (var componentService in componentsServices)
                {
                    if (componentService is not null)
                    {
                        var component = new PackageComponent();
                        //    component.SequenceNo = sequenceNo;
                        component.Net = 0;
                        component.UnitPrice = 0;


                        if (!string.IsNullOrWhiteSpace(componentService.ServiceCode.ToString()))
                            component.ProcedureID = componentService.ServiceCode;

                        if (!string.IsNullOrWhiteSpace(componentService.ServiceCatalogName))
                            component.ProcedureName = componentService.ServiceCatalogName.ToString();

                        if (!string.IsNullOrWhiteSpace(componentService.ServiceCode))
                            component.SBSProcedureCode = componentService.ServiceCode;

                        if (!string.IsNullOrWhiteSpace(componentService.ServiceCatalogName))
                            component.SBSCodeDescription = componentService.ServiceCatalogName;

                        if (!string.IsNullOrWhiteSpace(componentService.ServiceCode))
                            component.NphiesProcedureCodeSystem = componentService.ServiceCode;

                        if (!string.IsNullOrWhiteSpace(componentService.ServiceQuantity.ToString()))
                            component.Quantity = Convert.ToInt32(componentService.ServiceQuantity);

                        packages.Add(component);

                    }
                }
            }
            if (packages.Count > 0)
            {
                int sequenceNo = 1;
                var groupedPackages = packages
                .GroupBy(p => p.ProcedureID)
                .Select(g => new PackageComponent
                {
                    SequenceNo = sequenceNo++, // g.First().SequenceNo, 
                    ProcedureID = g.Key,
                    ProcedureName = g.First().ProcedureName,
                    SBSProcedureCode = g.First().SBSProcedureCode,
                    SBSCodeDescription = g.First().SBSCodeDescription,
                    NphiesProcedureCodeSystem = g.First().NphiesProcedureCodeSystem,
                    Quantity = g.Sum(p => p.Quantity),
                    UnitPrice = g.First().UnitPrice,
                    Net = g.First().Net
                })
                .ToList();
                packages = groupedPackages;


            }


            return packages;
        }
        private static ClaimDetail ClaimModel(RcmClaim claim, RcmClinic[] clinics, string messageBuindleId, int submissionCount, bool payerAlais = false)
        {
            int totalService = claim.RcmClaimServicesCategories.Count();
            if (claim.RcmClaimServicesCategories.Count(service => service.IsDeleted == true) == totalService)
                return null;

            if (claim.RcmClaimServicesCategories.Count(service => service.IsRefund == true || service.IsReturn == true) == totalService)
                return null;

            string claimType = string.Empty;
            ClaimDetail claimDetail = new ClaimDetail();
            claimDetail.IsDental = "false";
            claimDetail.IsPharmacy = "false";
            if (claim.ClaimType == (byte)ClaimType.Pharmacy)
            {
                claimDetail.ClaimType = "pharmacy";
                claimDetail.IsPharmacy = "true";
            }
            int countPharmacyService = claim.RcmClaimServicesCategories.Count(service => service.ServiceType == 5);

            var clinic = clinics.Where(x => x.ClinicId == claim.ClinicId).FirstOrDefault();
            if (clinic != null)
            {
                if (clinic.IsDental != null && clinic.IsDental == true
                       && claim.EncounterType == (byte)Enounter.Outpatient)
                {
                    claimDetail.IsDental = "true";
                }
            }

            if (countPharmacyService == totalService)
            {
                claimDetail.IsPharmacy = "true";
            }
            if (payerAlais)
            {
                claimDetail.ClaimNo = String.Format("{0}-{1}", claim.ClaimId,
                        submissionCount);
            }
            else
                claimDetail.ClaimNo = claim.ClaimId.ToString();

            claimDetail.ClaimID = claim.ClaimId.ToString();
            claimDetail.ClaimIdentifier = Guid.NewGuid().ToString();
            claimDetail.MessageBundleID = messageBuindleId;
            claimDetail.CreatedOn = DateTime.Now.ToNaphiesFormat();
            // claimDetail.ClaimTotal = CalculateClaimedAmount(claim.RcmClaimServicesDetails.Where(x => x.IsDeleted != true && x.IsRefund != true && x.IsReturn != true && x.ServiceCode != "05005009" && x.ServiceCompanyShare > 0).ToList());// Math.Round( Convert.ToDecimal( claim.CompanyShare) + Convert.ToDecimal(claim.CompanyTax),2).ToString();
            claimDetail.EpisodeNo = (claim.EpisodeId != null && claim.EpisodeId > 0) ? claim.EpisodeId.ToString() : claim.EncounterNo.ToString();
            claimDetail.OfflineApproval = claim.OfflineApproval;
            claimDetail.NphiesEligibility = claim.NphiesEligibility;
            claimDetail.NphiesApprovalIdentifier = claim.NphiesApprovalIdentifier;
            claimDetail.NphiesApprovalAuthRef = claim.NphiesApprovalAuthRef;
            claimDetail.NphiesApprovalSystem = claim.NphiesApprovalSystem;
            //claimDetail.EncounterDateTime = claim.EncounterDateTime;
            claimDetail.AccountingPeriod = claim.InvoiceDate.Value.ToNaphiesFormat();
            if (!string.IsNullOrWhiteSpace(claim.ClaimIdentifier))
                claimDetail.PreviousClaimIdentifier = claim.ClaimIdentifier;
            if (claim.isMoh.HasValue && claim.isMoh.Value)
            {
                claimDetail.IsMohClaim = true;

                if (claim.ClaimMode == null)
                {
                    claimDetail.ClaimMode = (byte)ClaimMode.Long;
                }
                else if (claim.ClaimMode == (byte)ClaimMode.Long)
                {
                    claimDetail.ClaimMode = (byte)ClaimMode.Long;
                }
                else if (claim.ClaimMode == (byte)ClaimMode.Referral)
                {
                    claimDetail.ClaimMode = (byte)ClaimMode.Referral;
                }
                else if (claim.ClaimMode == (byte)ClaimMode.Emergency)
                {
                    claimDetail.ClaimMode = (byte)ClaimMode.Emergency;
                }
            }
            else
            {
                claimDetail.IsMohClaim = false;
            }



            return claimDetail;
        }

        private async Task<int> RetriveMaxSubmissionCount(long claimId)
        {

            try
            {
                int maxSubmissionCount = await context.ClaimNphiesPostTrails
                                .CountAsync(x => x.ClaimId == claimId);

                return maxSubmissionCount + 1;
            }
            catch (Exception ex)
            {

                return 1;
            }

        }

        private bool PayerIdEnableAlais(RcmClaim claim)
        {
            var payerEnableAlais = config["PayerIdAlias"].ToString();
            var payerIds = Array.ConvertAll<string, int>(payerEnableAlais.Split(','), Convert.ToInt32)
                .ToArray();
            return payerIds.Where(p => p.Equals(claim.PayerId)).Count() > 0 ? true : false;
        }

        private static string CalculateClaimedAmount(List<RcmClaimServicesDetail> claimServicesDetail)
        {
            decimal companyShare = Convert.ToDecimal(claimServicesDetail.Sum(x => x.ServiceCompanyShare));
            decimal companyTax = Convert.ToDecimal(claimServicesDetail.Sum(x => x.CompanyTax));
            decimal patientSHare = Convert.ToDecimal(claimServicesDetail.Sum(x => x.ServicePatientShare));
            decimal claimedAmount = (companyShare + companyTax);
            return Math.Round(claimedAmount - patientSHare, 2).ToString();
        }
        private async Task<ResubmissionRemarksAndAttachments> GetRemarksAndAttachments(long rowId, int organizationId = 1)
        {
            ResubmissionRemarksAndAttachments attachmentModel = new ResubmissionRemarksAndAttachments();
            try
            {

                var responseBinding = await context.RcmAddResponseBindings.Where(r => r.RowId == rowId && r.IsActive == true).FirstOrDefaultWithNoLockAsync();

                if (responseBinding != null)
                {
                    var remarks = string.Join(',', context.RcmNphiesclaimRejectionRemarks.AsNoTracking()
                                                          .Where(x => x.OrganizationId == organizationId
                                                                && x.Id == responseBinding.RemarksId
                                                                //&& x.ClaimIdentifier == ClaimIdentifier
                                                                //&& x.NphiesSeqNo == nphiesSeqNo
                                                                && x.IsActive == true)
                                                          .Select(x => x.Remarks).ToArray());
                    string pattern = @"\d+:\d+:\d+"; // Pattern to match the dynamic number
                    attachmentModel.remarks = Regex.Replace(remarks, pattern, "").Replace("/n", "");


                    if (responseBinding.AttachId > 0)
                    {

                        attachmentModel.attachment = await context.RcmAttachments.AsNoTracking()
                                                    .Where(x => x.OrganizationId == organizationId
                                                         && x.AttachmentId == responseBinding.AttachId)
                                                    .Select(x => new AttachmentFileReference
                                                    {
                                                        contentType = !string.IsNullOrEmpty(x.FilePath) ? x.FileType : null,
                                                        creation = x.CreatedOn.Value.ToString("yyyy-MM-dd"),
                                                        title = x.FileName,
                                                        data = !string.IsNullOrEmpty(x.FilePath) && File.Exists(x.FilePath) ?
                                                                System.IO.File.ReadAllBytes(x.FilePath)
                                                                : null

                                                    }).FirstOrDefaultAsync();
                    }
                }

            }
            catch (Exception ex)
            {
                throw;
            }
            return attachmentModel;
        }

        private async Task<List<RcmClaim>> RetriveClaimsAsync(int orginaztionId,
            int facilityId,
            DateTime dateFrom,
            DateTime dateTo,
            long processId,
            int batchSize,
            int payerId,
            bool extend = false)
        {

            try
            {
                //string connectionString = context.Database.GetConnectionString();
                var facility = await context.RcmFacilities.AsNoTracking().Where(f => f.FacilityId == facilityId)
                        .Select(f => new { f.ExternalCode2, f.ExternalCode }).FirstOrDefaultAsync();

                var result = await context.RcmClaims.AsNoTracking()
                                .Include(patient => patient.RcmClaimPatientDetail)
                                .Include(vital => vital.RcmClaimVitalSign)
                                .Include(diagnosis => diagnosis.RcmClaimDiagnoses)
                                .Where(Filter(orginaztionId, facilityId, dateFrom, dateTo, processId, payerId))
                                .Take(batchSize)
                                .OrderByDescending(x => x.ClaimId)
                                .ToListWithNoLockAsync();

                // List<RcmClaimServicesDetailDTO> service_category_data = new List<RcmClaimServicesDetailDTO>();
                foreach (var item in result)
                {
                    try
                    {
                        item.RcmClaimServicesCategories = await (
                                       from rcsd in context.RcmClaimServicesDetails
                                       where rcsd.ClaimId == item.ClaimId
                                             && rcsd.IsDeleted != true
                                             && rcsd.IsRefund != true
                                             && rcsd.IsReturn != true
                                             && !serviceCodes.Contains(rcsd.ServiceCode)
                                             && (rcsd.ServiceCompanyShare > 0 || rcsd.ServiceCode == "01002007")
                                       join rsc in context.RcmServiceCatalogs on rcsd.ServiceId equals rsc.ServiceId
                                       select new RcmClaimServicesDetailDTO
                                       {
                                           // Map properties from RcmClaimServicesDetail
                                           ServiceId = rcsd.ServiceId,
                                           ServiceCode = ServiceCode(item, rcsd),
                                           StandardCode = rcsd.StandardCode,
                                           NphiesCodeType = rcsd.NphiesCodeType,
                                           StandardCode2 = rcsd.StandardCode2,
                                           ServiceReferenceNumber = rcsd.ServiceReferenceNumber,
                                           InvoiceDate = rcsd.InvoiceDate,
                                           ServiceQuantity = rcsd.ServiceQuantity,
                                           ServiceGrossAmount = rcsd.ServiceGrossAmount,
                                           ServiceDiscountAmount = rcsd.ServiceDiscountAmount,
                                           ServicePatientShare = rcsd.ServicePatientShare,
                                           ServiceCompanyShare = rcsd.ServiceCompanyShare,
                                           CompanyTax = rcsd.CompanyTax,
                                           PatientTax = rcsd.PatientTax,
                                           ToothNo = rcsd.ToothNo,
                                           RowId = rcsd.RowId,
                                           ApprovalNo = rcsd.ApprovalNo,
                                           // Map properties from RcmServiceCatalog
                                           ServiceType = !rcsd.ServiceType.HasValue ? (short) 7 : (short)rsc.ServiceType, // if service type is null set it to 7 (sbs)
                                           ServiceCatalogName = ServiceCatalogName(item, rcsd, rsc),
                                           ParentServiceCode = rcsd.ParentServiceCode,
                                           IsReturn = rcsd.IsReturn,
                                           IsDeleted = rcsd.IsDeleted,
                                           HasChild = rcsd.HasChild,
                                           IsRefund = rcsd.IsRefund,
                                           ServiceStartDate = rcsd.ServiceStartDateTime,
                                           ServiceEndDate = rcsd.ServiceEndDateTime,
                                           UnCatagoriesService = !string.IsNullOrWhiteSpace(rcsd.MOHServiceCode) ? true : false,
                                           MOHServiceCodeDescription = rcsd.MOHServiceCodeDescription,
                                           MOHServiceCode = rcsd.MOHServiceCode

                                       }
                                   ).AsNoTracking().ToListAsync();

                        if (extend)
                        {
                            foreach (var service in item.RcmClaimServicesCategories)
                            {
                                var data = await GetRemarksAndAttachments(service.RowId, 1);
                                service.ResubmissionData = data;
                            }
                        }

                        if (item.isMoh == null && item.isMoh.HasValue && item.isMoh == false)
                        {
                            var medicalRecord = await context.RcmEncounterMedicalDetails.AsNoTracking()
                                .Where(x => x.EncounterNo == item.EncounterNo.ToString()
                                    && x.OrganizationId == item.OrganizationId
                                    && x.FacilityGroupId == facility.ExternalCode2
                                    && x.FacilityId == Convert.ToInt32(facility.ExternalCode)
                                    && x.PatientMrn == item.RcmClaimPatientDetail.PatientFileNumber
                                    && x.IsActive)
                                .Select(x => x.MedicalData)
                                .FirstOrDefaultWithNoLockAsync()
                                ;
                            item.MedicalJsonData = medicalRecord ?? string.Empty;
                        }
                    }
                    catch (Exception ex)
                    {

                        logger.LogCritical(ex);
                        //throw new Exception($"Retriving {nameof(RetriveClaimsAsync)} error : {ex.Message}");
                        await UpdateClaimStatusByClaimId(item.ClaimId, StringConstant.InternalError + $"{ex.Message}", (byte)ClaimStatus.UpdateError);
                    }

                }



                return result;
            }
            catch (SqlException sql)
            {
                logger.LogCritical(sql);
                throw new Exception($"SQL Error : {sql.Message}");
            }
           
            catch (Exception ex)
            {
                logger.LogCritical(ex);
                throw new Exception($"Retriving {nameof(RetriveClaimsAsync)} error : {ex.Message}");
            }


        }

        private static string ServiceCatalogName(RcmClaim claim, RcmClaimServicesDetail rcsd, RcmServiceCatalog rsc)
        {

            if (claim.isMoh != null && claim.isMoh.Value == true && !string.IsNullOrWhiteSpace(rcsd.MOHServiceCode))
            {
                return rcsd.MOHServiceCodeDescription;
            }
            return rsc.ServiceCatalogName;
        }

        private static string ServiceCode(RcmClaim claim, RcmClaimServicesDetail rcsd)
        {
            if (claim.isMoh != null && claim.isMoh.Value == true && !string.IsNullOrWhiteSpace(rcsd.MOHServiceCode))
            {
                return rcsd.MOHServiceCode;
            }
            return rcsd.ServiceCode;
        }

        private async Task<List<Models.Claims.ClaimModel>> RetriveClaimsWithNolockAsync(int orginaztionId,
            int facilityId,
            DateTime dateFrom,
            DateTime dateTo,
            long processId,
            int batchSize,
            int payerId)
        {

            try
            {

                List<Models.Claims.ClaimModel> claimDetails = await (from claim in context.RcmClaims
                                                                     join doctor in context.RcmDoctors on new { claim.DoctorId, claim.OrganizationId, claim.FacilityId }
                                                                         equals new { doctor.DoctorId, doctor.OrganizationId, doctor.FacilityId }

                                                                     join clinic in context.RcmClinics on new { claim.OrganizationId, claim.ClinicId }
                                                                         equals new { clinic.OrganizationId, clinic.ClinicId }
                                                                     join pd in context.RcmClaimPatientDetails on new { claim.ClaimId, claim.OrganizationId }
                                                                         equals new { pd.ClaimId, pd.OrganizationId }
                                                                     join payer in context.RcmPayers on new { claim.OrganizationId, claim.PayerId }
                                                                         equals new { payer.OrganizationId, payer.PayerId }

                                                                     join vitalsigns in context.RcmClaimVitalSigns
                                                                         on new { O = claim.OrganizationId, T = claim.ClaimId }
                                                                         equals new { O = vitalsigns.OrganizationId, T = vitalsigns.ClaimId } into vsGroup
                                                                     from vs in vsGroup.DefaultIfEmpty()


                                                                     where claim.OrganizationId == orginaztionId
                                                                        && claim.FacilityId == facilityId
                                                                        && claim.PayerId == payerId
                                                                        && claim.Status == (byte)ClaimStatus.PendingForNphies
                                                                        && claim.ProcessId == processId
                                                                        && claim.InvoiceDate.Value.Date >= dateFrom.Date
                                                                        && claim.InvoiceDate.Value.Date <= dateTo.Date

                                                                     select new Models.Claims.ClaimModel
                                                                     {

                                                                         ClaimId = claim.ClaimId,
                                                                         EncounterNo = claim.EncounterNo.Value,
                                                                         EncounterType = claim.EncounterType.Value,

                                                                         Clinic = new Models.Claims.Clinic
                                                                         {
                                                                             ClinicId = clinic.ClinicId,
                                                                             ClinicName = clinic.ClinicName,
                                                                             Code = clinic.ExternalCode,
                                                                             IsDental = clinic.IsDental
                                                                         },
                                                                         Doctor = new Models.Claims.Doctor
                                                                         {
                                                                             DoctorId = doctor.DoctorId,
                                                                             DoctorName = doctor.DoctorName,
                                                                             Licenses = doctor.RcmDoctorLicenses
                                                                              .Select(licenses => new Models.Claims.DoctorLicenses
                                                                              {

                                                                              })
                                                                              .ToList(),

                                                                         },
                                                                         Diagnosis = claim.RcmClaimDiagnoses
                                                                                            .Select(diagnosis => new Models.Diagnoses.ClaimDiagnosis
                                                                                            {
                                                                                                DiagnosisCode = diagnosis.DiagnosisCode

                                                                                            }).ToList(),

                                                                         ServicesDetails = claim.RcmClaimServicesDetails
                                                                                .Where(x => x.IsDeleted != true
                                                                                       && x.IsRefund != true
                                                                                       && x.IsReturn != true
                                                                                       && x.ServiceCode != "05005009"
                                                                                       && x.ServiceCompanyShare > 0)
                                                                                            .Select(service => new Models.Services.Service
                                                                                            {
                                                                                                ClaimId = service.ClaimId,



                                                                                            }).ToList(),

                                                                     }).AsNoTracking()
                                                                                 .Take(batchSize)
                                                                                 .ToListWithNoLockAsync();



                return claimDetails;
            }
            catch (Exception ex)
            {

                throw;
            }


        }
        private async Task<List<RcmClaim>> RetriveClaimsAsync(int orginaztionId,
           int facilityId,
          long processId,
           int batchSize,
           int payerId)
        {

            try
            {
                var result = await context.RcmClaims.AsNoTracking()
                          .Include(claim => claim.RcmClaimServicesDetails)
                          .Include(patient => patient.RcmClaimPatientDetail)
                          .Include(vital => vital.RcmClaimVitalSign)
                          .Include(diagnosis => diagnosis.RcmClaimDiagnoses)
                          .Where(FilterClaimByProcessIds(orginaztionId, facilityId, processId, payerId))
                          .OrderBy(claim => claim.RcmClaimServicesDetails.Count())
                          .Take(batchSize)
                          .ToListAsync();
                return result;
            }
            catch (Exception ex)
            {

                throw;
            }


        }

        private static Expression<Func<RcmClaim, bool>> Filter(int orginaztionId, int facilityId, DateTime dateFrom, DateTime dateTo, long processId, int payerId)
        {
            List<long> claimsId = new List<long>();
            // claimsId.Add(26793172);
           
            return
                    claim => claim.OrganizationId == orginaztionId && claim.FacilityId == facilityId
                    && claim.PayerId == payerId
                 && claim.Status == (byte)ClaimStatus.PendingForNphies
                    && claim.ProcessId == processId
                    && claim.InvoiceDate.Value.Date >= dateFrom.Date
                    && claim.InvoiceDate.Value.Date <= dateTo.Date
                    //&& claimsId.Contains(claim.ClaimId )
                    // && claim.ClaimId == 26327744
                    ;
           
        }
        private static Expression<Func<RcmClaim, bool>> FilterClaimByProcessIds(
            int orginaztionId,
            int facilityId,
           long processId,
            int payerId)
        {
            return claim => claim.OrganizationId == orginaztionId && claim.FacilityId == facilityId
            && claim.PayerId == payerId
            && claim.Status == (byte)ClaimStatus.PendingForNphies
            && claim.ProcessId == processId
           ;
        }
        private static string IsString(string str)
        {
            return !string.IsNullOrWhiteSpace(str) ? str.Replace("'", "") : string.Empty;
        }
        private static string IsDecimal(decimal? dec)
        {
            return dec != null ? dec.ToString() : string.Empty;
        }

        public async ValueTask<ClaimCancelation> RetriveClaims(int orginzationId, int facilityId, int payerId, long processId)
        {
            var PayerLicense = GetPayerLicense(payerId);

            string licensr = PayerLicense?.ParameterValue;
            var Claims = context.RcmClaims.AsNoTracking().Where(
                            claim => claim.OrganizationId == orginzationId
                            && claim.FacilityId == facilityId
                            && claim.PayerId == payerId
                            && (claim.Status != (byte)ClaimStatus.Nphies_Approved 
                            && claim.Status != (byte)ClaimStatus.PartialAccepted
                            && claim.Status != (byte)ClaimStatus.Nphies_Rejected
                            && claim.Status != (byte)ClaimStatus.Nphies_Perror
                            && claim.Status != (byte)ClaimStatus.Nphies_Error
                            )
                            && claim.ProcessId == processId
                        ).Select(claim => new Claim
                        {

                            ClaimId = claim.ClaimId,
                            ClaimIdentifier = claim.ClaimIdentifier,
                            ClaimBundleId = claim.ClaimBundleId,

                        });

            return new ClaimCancelation
            {
                PayerLicense = licensr,
                Claims = await Claims.ToListAsync()
            };



        }
        private async Task<Tuple<string, string>> RetrivePayerLicenseAndTPA(RcmClaim claim
                , RcmPayer payer,
                RcmAdapterMapping rcmAdapterMapping)
        {
            string insNhicId = string.Empty;
            string tpaNhicId = string.Empty;
            string insOrTpaNhicId = string.Empty;
            int payerId = claim.PayerId;
            bool isManagedByTPA = false;
            bool isTPA = false;

            if (claim is not null)
            {


                if (payer != null)
                {
                    isTPA = payer.IsTPA ?? false;
                    isManagedByTPA = payer.IsManagedByTPA ?? false;
                    insOrTpaNhicId = rcmAdapterMapping.ParameterValue;
                }
                var payerPolicy = await context.RcmPayerPolicies
                    .AsNoTracking()
                   .Where(policy => policy.PayerId == claim.PayerId
                    && policy.PayerPolicyId == claim.PayerPolicyId
                    && policy.IsActive == true)
                   .FirstOrDefaultAsync();
                if (payerPolicy != null)
                {
                    if (!isTPA && !isManagedByTPA)
                    {
                        insNhicId = insOrTpaNhicId;
                        tpaNhicId = insOrTpaNhicId;
                        return Tuple.Create(insNhicId, tpaNhicId);
                    }
                    else if (isTPA && !isManagedByTPA)
                    {
                        tpaNhicId = insOrTpaNhicId;
                        insNhicId = payerPolicy.InsuranceNhic;
                        return Tuple.Create(insNhicId, tpaNhicId);
                    }
                    else if (!isTPA && isManagedByTPA)
                    {
                        insNhicId = insOrTpaNhicId;
                        tpaNhicId = !string.IsNullOrWhiteSpace(payerPolicy.InsuranceNhic) ? payerPolicy.InsuranceNhic : string.Empty;
                        return Tuple.Create(insNhicId, tpaNhicId);
                    }
                }





            }
            return Tuple.Create(string.Empty, string.Empty);
        }
        private RcmAdapterMapping GetPayerLicense(int payerId)
        {
            return context.RcmAdapterMappings.FirstOrDefault(x => x.ParameterMasterId == 2 && x.ParameterType == payerId.ToString() && x.IsActive == true);
        }

        public async Task GetClaimById(long claimId, string adaptorCode)
        {
            var claim = await context.RcmClaims.FirstOrDefaultAsync(claim => claim.ClaimId == claimId);
            var payerList = await context.RcmPayers.Where(payer => payer.PayerId == claim.PayerId).ToArrayAsync();
            var adapterMapping = await context.RcmAdapterMappings.Where(x => x.IsActive == true).ToListAsync();

            var result = ClaimPayers(claim, adaptorCode);

            await Task.CompletedTask;
        }

        public async ValueTask<bool> CancellationClaimAsync(Cancellation claimResponseRequest)
        {
            var claim = await context.RcmClaims.Where(claim => claim.ClaimId ==
                                 claimResponseRequest.ClaimId).FirstOrDefaultAsync();
            if (claim != null)
            {
                claim.NphieseRemarks = claimResponseRequest.Remarks;
                if (claim.NphieseRemarks.Contains("nullified"))
                {
                    claim.Status = (byte)ClaimStatus.Nphies_Cancelled;
                    claim.ProcessId = null;
                }
                await context.SaveChangesAsync();
                return true;
            }
            return false;
        }

        public async ValueTask<bool> UpdateClaimAttachmentAsync(Models.Claims.ClaimsAttachment claimsAttachment)
        {
            try
            {
                var claim = await context.RcmClaims.Where(claim => claim.OrganizationId == claimsAttachment.OrginzationId
                    && claim.FacilityId == claimsAttachment.FacilityId
                    && claimsAttachment.ClaimId == claim.ClaimId)
                        .FirstOrDefaultAsync();

                if (claim != null)
                {
                    claim.ISPDFAttached = true;


                    if (!string.IsNullOrEmpty(claimsAttachment.DocumentReferenceNo))
                    {
                        if (Guid.TryParse(claimsAttachment.DocumentReferenceNo, out _))
                        {
                            claim.DocumentReferenceNo = Guid.Parse(claimsAttachment.DocumentReferenceNo);
                        }
                    }

                    claim.ModifiedOn = DateTime.Now;
                    claim.ModifiedBy = 899;

                    return await context.SaveChangesAsync() > 1 ? await Task.FromResult(true) : await Task.FromResult(false);
                }
            }
            catch (Exception ex)
            {


            }
            return await Task.FromResult(false);
        }
        public async ValueTask<bool> UpdateClaimAgainstCoderEncounter(Models.Claims.ClaimDRG claimDRG)
        {
            try
            {
                var drgEncounter = await context.CoDskEncounter.Where(drg => drg.OrganizationId == claimDRG.OrginzationId
                    && drg.FacilityId == claimDRG.FacilityId
                    && drg.EncounterNo == claimDRG.EncounterNo
                    && drg.EncounterType.ToString() == claimDRG.EncounterType)
                        .FirstOrDefaultAsync();

                if (drgEncounter != null)
                {
                    drgEncounter.ClaimId = claimDRG.ClaimId;
                }

                return await context.SaveChangesAsync() > 0 ? true : false;
            }
            catch (Exception ex)
            {
                return false;
            }
        }

        public async ValueTask<bool> UpdateClaimStatusByClaimId(long claimId, string errorMessage, byte status)
        {
            try
            {
                RcmClaim rcmClaim = await context.RcmClaims
                       .FirstOrDefaultAsync(claimE => claimE.ClaimId == claimId);

                string errorLog = errorMessage.Length > 200 ? errorMessage.Substring(0, 200) : errorMessage;

                if (rcmClaim != null)
                {
                    rcmClaim.Status = status;
                    rcmClaim.Remarks = errorLog.Trim();
                    await context.SaveChangesAsync();
                    return true;
                }
                return false;
            }
            catch (SqlException sql)
            {
                string errormessage = string.Format("{0}-{1}-{2}", nameof(UpdateClaimStatusByClaimId), claimId, sql.Message);
                this.logger.LogError(errormessage);
                this.logger.LogCritical(sql);
                return await Task.FromResult(false);
            }
            catch (Exception ex)
            {
                string errormessage = string.Format("{0}-{1}-{2}-{3}", nameof(UpdateClaimStatusByClaimId), claimId, errorMessage, ex.Message);
                this.logger.LogError(errormessage);
                this.logger.LogCritical(ex);
                return await Task.FromResult(false);
            }
        }


        private async Task AddClaimPostTrail(ClaimResponseRequest claimResponseRequest, ClaimDetail claim, int orginzationId)
        {
            // Add Or Update ClaimNphiesPostTrail
            NphiesPostTrailDto postTrail = new NphiesPostTrailDto()
            {
                ClaimId = Convert.ToInt64(claim.ClaimID),
                ClaimIdentifier = claim.ClaimIdentifier,
                ClaimBundleId = claim.MessageBundleID,
                ProcessId = claimResponseRequest.ProcessId,
                CriteriaType = claimResponseRequest.CriteriaType,
                OrganizationId = orginzationId
            };

            await AddNphiesPostTrail(postTrail);
        }
        private async Task AddClaimPostTrail(
            long processId,
            int criteriaType,
            long claimId,
            string claimIdentifier,
            string claimBundleId,
            int orginzationId)
        {
            // Add Or Update ClaimNphiesPostTrail
            NphiesPostTrailDto postTrail = new NphiesPostTrailDto()
            {
                ClaimId = claimId,
                ClaimIdentifier = claimIdentifier,
                ClaimBundleId = claimBundleId,
                ProcessId = processId,
                CriteriaType = criteriaType,
                OrganizationId = orginzationId
            };

            await AddNphiesPostTrail(postTrail);
        }
        public async ValueTask<bool> AddNphiesPostTrail(NphiesPostTrailDto claimRequest)
        {
            var isSuccess = false;
            try
            {
                var nphiesLog = await context.ClaimNphiesPostTrails
                                        .AsNoTracking()
                                        .Where(npt => npt.ClaimId == claimRequest.ClaimId
                                            && npt.OrganizationId == claimRequest.OrganizationId)
                                        .OrderByDescending(npt => npt.Id)
                                        .FirstOrDefaultWithNoLockAsync();

                RcmClaimNphiesPostTrail nphiesPostTrail = new RcmClaimNphiesPostTrail();
                nphiesPostTrail.ClaimId = claimRequest.ClaimId;
                nphiesPostTrail.CreatedBy = 899;
                nphiesPostTrail.CreatedOn = DateTime.Now;
                nphiesPostTrail.CriteriaType = claimRequest.CriteriaType.Value;
                nphiesPostTrail.ProcessId = claimRequest.ProcessId;
                nphiesPostTrail.OrganizationId = claimRequest.OrganizationId;
                nphiesPostTrail.ClaimIdentifier = claimRequest.ClaimIdentifier;
                nphiesPostTrail.ClaimBundleId = claimRequest.ClaimBundleId;

                if (nphiesLog != null)
                {
                    nphiesPostTrail.PreviousClaimIdentifier = nphiesLog.ClaimIdentifier;
                    nphiesPostTrail.PrevoiusClaimBundleId = nphiesLog.ClaimBundleId;
                    nphiesPostTrail.SequenceNumber = nphiesLog.SequenceNumber + 1;
                }
                else
                {
                    nphiesPostTrail.SequenceNumber = 1;
                }

                context.ClaimNphiesPostTrails.Add(nphiesPostTrail);
                await context.SaveChangesAsync();
                isSuccess = true;
            }
            catch (Exception ex)
            {
                string errorMessage = string.Format("{0}-{1}-{2}", nameof(AddNphiesPostTrail), claimRequest.ClaimId, ex.Message);
                logger.LogError(errorMessage);
            }

            return isSuccess;
        }

        private async Task<byte> RetrieveDischargeSummaryAsync(int organizationId, long claimId)
        {
            try
            {
                byte? dischargeDisposition = await context.RcmDischargeSummaries
                    .AsNoTracking()
                    .Where(x => x.OrganizationId == organizationId && x.ClaimId == claimId)
                    .Select(x => x.DischargeDisposition)
                    .FirstOrDefaultAsync();
                return dischargeDisposition ?? 0;
            }
            catch (Exception ex)
            {
                logger.LogError(nameof(ClaimDetailsAsync) + "_" + nameof(RetrieveDischargeSummaryAsync) + ex.Message);
                //  throw new Exception(StringConstant.RetrieveDischargeSummaryAsync + $" {ex.Message}", ex);
                return await Task.FromResult(Convert.ToByte(0));
            }
        }
        private async Task<DischargeSummary> RetrieveClaimDischargeSummaryAsync(int organizationId, long claimId)
        {
            try
            {
                var dischargeSummary = await context.RcmDischargeSummaries
                    .AsNoTracking()
                    .Where(x => x.OrganizationId == organizationId && x.ClaimId == claimId)
                    .Select(x => new DischargeSummary
                    {
                        DischargeDisposition = x.DischargeDisposition.Value,
                        ClinicId = x.ClinicId,
                        DoctorId = x.DoctorId,
                    })
                    .FirstOrDefaultAsync();
                return dischargeSummary;
            }
            catch (Exception ex)
            {
                //  logger.LogError(nameof(ClaimDetailsAsync) + "_" + nameof(RetrieveClaimDischargeSummaryAsync) + ex.Message);
                // throw new Exception(StringConstant.RetrieveClaimDischargeSummaryAsync + $" {ex.Message}", ex);
            }
            return new DischargeSummary();
        }
        private string RetriveScientificCodes(string serviceCode)
        {
            return context.RcmTransactionMapingValues.AsNoTracking()
                     .Where(m => m.MappingType == 12  // is for Scientific code
                             && m.ExternalCode == serviceCode)
                     .Select(x => x.MappingValue).FirstOrDefault();
        }
        private static JObject RetriveDischargeSummary(string medicalinfo)
        {
            JObject medicalFile = JObject.Parse(medicalinfo);
            JObject medicalInfo = JObject.Parse(medicalFile["medicalInfo"].ToString());
            if (medicalInfo != null && medicalInfo.HasValues && medicalInfo["discharge"] != null)
                return JObject.Parse(medicalInfo["discharge"].ToString());
            else
                return null;
        }

        public ValueTask<bool> UpdateClaim(UpdateClaimStatusResponse claimResponseRequest)
          => TryCatch(async () =>
          {
              Validate();
              return await UpdateClaimStatusAsync(claimResponseRequest);
          });

        private async ValueTask<bool> UpdateClaimStatusAsync(UpdateClaimStatusResponse updateClaimStatusResponse)
        {
            bool status2Return = false;

            if (updateClaimStatusResponse != null)
            {
                //var claimResponse = claimResponseRequest.ClaimResponseModel;
                //if (claimResponse != null && claimResponse.Count > 0)
                //{
                //    var resmodel = claimResponse.Select(x => new BatchResponse
                //    {
                //        ClaimDetailResponse = x.ClaimDetailResponse
                //    }).ToList();

                //    var claimDetailResponses = resmodel.ToList();
                //    List<ClaimDetailResponse> detailResponses = new List<ClaimDetailResponse>();
                //    foreach (var item in claimDetailResponses)
                //        detailResponses = item.ClaimDetailResponse;


                //    var claimEntity = await context.RcmClaims.FirstOrDefaultAsync(predicate => predicate.ClaimId == updateClaimStatusResponse.ClaimId);
                //    if (claimEntity != null)
                //    {
                //        var claimResponseRequest = new ClaimResponseRequest
                //        {
                //            ProcessId = updateClaimStatusResponse.ProcessId,
                //            CriteriaType = updateClaimStatusResponse.CriteriaType
                //        };
                //        if (updateClaimStatusResponse.CriteriaType > 0)
                //        {

                //            await AddClaimPostTrail(claimResponseRequest,
                //                 updateClaimStatusResponse.ClaimId,
                //                 updateClaimStatusResponse.ClaimIdentifier,
                //                 updateClaimStatusResponse.ClaimBundleId,
                //                 claimEntity.OrganizationId);
                //        }

                //        if (updateClaimStatusResponse.CriteriaType != (byte)NphiesFiltercriteria.ReProcess_With_Same_BundleId)
                //        {
                //            if (await UpdateClaimDetailsAsync(updateClaimStatusResponse, detailResponses, claimEntity) > 0)
                //                status2Return = true;
                //        }
                //        else
                //        {
                //            if (await UpdateClaimDetailsResponseRequest(detailResponses, claim, claimEntity) > 0)
                //                status2Return = true;
                //        }

                //    }


                //}
                //else
                //{


                //        var claimEntity = await context.RcmClaims
                //            .FirstOrDefaultAsync(predicate => predicate.ClaimId == updateClaimStatusResponse.ClaimId);
                //        if (claimEntity != null)
                //        {
                //            claimEntity.ClaimBundleId = updateClaimStatusResponse.MessageBundleID;
                //            claimEntity.ClaimIdentifier = updateClaimStatusResponse.ClaimIdentifier;
                //            claimEntity.BatchIdentifier = updateClaimStatusResponse.ClaimbatchIdentifier;
                //            claimEntity.BatchBundleId = updateClaimStatusResponse.ClaimBundleIdentifier;
                //            claimEntity.ProcessId = updateClaimStatusResponse.ProcessId;


                //            // Update the Claim Status
                //            claimEntity.Status = (byte)ClaimStatus.InProgress;
                //        }
                //        foreach (var item in updateClaimStatusResponse.UpdatedServices)
                //        {
                //            var serviceEntity = await context.RcmClaimServicesDetails.FirstOrDefaultAsync(x => x.RowId == item.RowId);
                //            if (serviceEntity != null)
                //            {
                //                serviceEntity.NphiesSeqNo = item.Seqno;
                //                serviceEntity.ClaimIdentifier = updateClaimStatusResponse.ClaimIdentifier;

                //                // context.RcmClaimServicesDetails.Add(serviceEntity);
                //            }
                //        }


                //    try
                //    {
                //        if (await context.SaveChangesAsync() > 0)
                //            status2Return = true;
                //    }
                //    catch (Exception ex)
                //    {

                //        // throw;
                //    }
                //}

            }
            return status2Return;
        }
        public async ValueTask<bool> UpdateClaimAndServicesSeqAsync(UpdateClaimAndServicesSeqRequest updateClaimAndServicesSeqRequest)
        {
            try
            {
                if (!string.IsNullOrWhiteSpace(updateClaimAndServicesSeqRequest.ClaimIdentifier))
                {
                    var claimEntity = await this.context.RcmClaims.Where(claim => claim.ClaimId == updateClaimAndServicesSeqRequest.ClaimID)
                        .FirstOrDefaultAsync();

                    if (claimEntity is RcmClaim)
                    {
                        claimEntity.ClaimBundleId = updateClaimAndServicesSeqRequest.MessageBundleID;
                        claimEntity.ClaimIdentifier = updateClaimAndServicesSeqRequest.ClaimIdentifier;
                        claimEntity.BatchIdentifier = updateClaimAndServicesSeqRequest.ClaimbatchID;
                        claimEntity.BatchBundleId = updateClaimAndServicesSeqRequest.ClaimBundleIdentifier;
                        //claimEntity.Status = (byte)ClaimStatus.InProgress;

                        //foreach (var service in updateClaimAndServicesSeqRequest.ClaimServices)
                        //{
                        //    var claimService = await this.context.RcmClaimServicesDetails
                        //        .Where(s =>s.ClaimId == claimEntity.ClaimId && s.RowId == service.RowId)
                        //        .FirstOrDefaultAsync();
                        //    if (claimService is RcmClaimServicesDetail)
                        //    {
                        //        claimService.NphiesSeqNo = service.Seqno;
                        //        claimService.ClaimIdentifier = updateClaimAndServicesSeqRequest.ClaimIdentifier;

                        //       // await this.context.SaveChangesAsync();
                        //    }
                        //}
                        // Get the rowIds for lookup
                        var rowIds = updateClaimAndServicesSeqRequest.ClaimServices
                            .Select(s => s.RowId)
                            .ToHashSet();

                        // Fetch all matching services in one query
                        var claimServices = await context.RcmClaimServicesDetails.Where(s => rowIds.Contains(s.RowId))
                            .ToListAsync();

                        // Build a dictionary for fast lookup
                        var updatedServicesDict = updateClaimAndServicesSeqRequest.ClaimServices
                            .ToDictionary(s => s.RowId);

                        // Update matching services
                        foreach (var service in claimServices)
                        {
                            if (updatedServicesDict.TryGetValue(service.RowId, out var updatedService))
                            {
                                service.NphiesSeqNo = updatedService.Seqno;
                                service.ClaimIdentifier = updateClaimAndServicesSeqRequest.ClaimIdentifier;
                            }
                        }
                        if (await this.context.SaveChangesAsync() > 0)
                        {
                            return await Task.FromResult(true);

                        }

                    }
                }

            }
            catch (DbUpdateConcurrencyException dbupdate)
            {
                logger.LogError($"{nameof(UpdateClaimAndServicesSeqAsync)} DbUpdateConcurrencyException {dbupdate.Message}");
                logger.LogCritical(dbupdate);
            }
            catch (SqlException sql)
            {
                logger.LogError($"{nameof(UpdateClaimAndServicesSeqAsync)}  SqlException  {sql.Message}");
                logger.LogCritical(sql);

            }
            catch (Exception ex)
            {
                logger.LogError($"{nameof(UpdateClaimAndServicesSeqAsync)}  Error occure while updating claim sequences- {ex.Message}");
                logger.LogCritical(ex);

            }
            return await Task.FromResult(false);
        }
        public async ValueTask<bool> UpdateClaimResponseAsync(UpdateClaimResponseModel updateClaimResponse)
        {
            try
            {
                if (updateClaimResponse != null && updateClaimResponse.ClaimDetailResponse != null)
                {
                    var claimEntity = await this.context.RcmClaims.Where(claim => claim.ClaimId ==
                                        updateClaimResponse.ClaimDetailResponse.ClaimId)
                       .FirstOrDefaultAsync();
                    if (claimEntity is RcmClaim)
                    {

                        if (updateClaimResponse.ClaimDetailResponse.CriteriaType != (byte)NphiesFiltercriteria.ReProcess_With_Same_BundleId)
                        {
                            if (
                                await UpdateClaimDetailsAsync(
                                        claimEntity.ClaimIdentifier,
                                        updateClaimResponse.ClaimDetailResponse.ClaimTotal,
                                        updateClaimResponse.ClaimServices,
                                         updateClaimResponse.ClaimDetailResponse,
                                         claimEntity) < 0
                                 )

                                return await Task.FromResult<bool>(false);
                        }
                        else
                        {
                            if (await UpdateClaimDetailsResponseRequestAsync(updateClaimResponse.ClaimDetailResponse, claimEntity) < 0)
                                return await Task.FromResult<bool>(false);
                        }

                        await AddClaimPostTrail(claimEntity.ProcessId.Value,
                              updateClaimResponse.ClaimDetailResponse.CriteriaType,
                              claimEntity.ClaimId,
                              claimEntity.ClaimIdentifier,
                              claimEntity.ClaimBundleId,
                              claimEntity.OrganizationId);

                        return await Task.FromResult<bool>(true);
                    }
                }
            }
            catch (DbUpdateConcurrencyException dbupdate)
            {
                logger.LogError($"{nameof(UpdateClaimResponseAsync)} DbUpdateConcurrencyException {dbupdate.Message}");
                logger.LogCritical(dbupdate);
            }
            catch (SqlException sql)
            {
                logger.LogError($"{nameof(UpdateClaimResponseAsync)} SqlException {sql.Message}");
                logger.LogCritical(sql);

            }
            catch (Exception ex)
            {

                logger.LogError($"{nameof(UpdateClaimResponseAsync)} Error occure while  UpdateClaimResponseAsync- {ex.Message}");
                return await Task.FromResult(false);
            }
            return await Task.FromResult(false);
        }
        public async Task<ApiResponseOnUpdate> UpdateMedicalDataForClaim(EncounterMedicalDetail claimEncounterMedical)
        {
            try
            {
                var res = await _claimUpdate.UpdateMedicalDataForClaim(claimEncounterMedical);
                return res;
            }
            catch (Exception ex)
            {

                return new ApiResponseOnUpdate { Success = false, Message = ex.Message };
            }


        }
        public async Task<List<string>> RetrieveAttachmentDocumentIds(long claimId)
        {
            return await context.RcmAttachments.AsNoTracking()
                .Where(x => x.TransactionId == claimId
                 && x.IsActive.HasValue && x.IsActive.Value && x.IncludeSubmission.HasValue && x.IncludeSubmission.Value)
                .Select(x => x.MongoDocId).ToListAsync();
        }
        public async ValueTask<long> GetClaimId
       (
           int orginzationId,
           int facilityId,
           string adaptorCode,
           DateTime dtfrom,
           DateTime dtTo,
           long processId,
           int batchsize,
           string PayerId,
           bool extend
       )
        {
            try
            {
                return await RetriveClaimIdAsync(orginzationId, facilityId, dtfrom, dtTo, processId, batchsize, Convert.ToInt32(PayerId), extend);
            }
            catch (Exception ex)
            {

                logger.LogError($"{nameof(GetClaimId)} Error occure while  GetClaimsId- {ex.Message}");
                return await Task.FromResult(0);

            }
        }
        private async Task<long> RetriveClaimIdAsync(int orginaztionId,
                   int facilityId,
                   DateTime dateFrom,
                   DateTime dateTo,
                   long processId,
                   int batchSize,
                   int payerId,
                   bool extend = false)
        {

            try
            {
               
                var result = await context.RcmClaims.AsNoTracking()
                                 .Where(Filter(orginaztionId, facilityId, dateFrom, dateTo, processId, payerId))
                                .Select(x => x.ClaimId)
                                .OrderByDescending(x => x)
                                .FirstOrDefaultWithNoLockAsync();


                return result;
            }
            catch (SqlException sql)
            {
                logger.LogCritical(sql);
                throw new Exception($"SQL Error : {sql.Message}");
            }
            catch (Exception ex)
            {
                logger.LogCritical(ex);
                throw new Exception($"Retriving {nameof(RetriveClaimIdAsync)} error : {ex.Message}");
            }

        }
        private string GetStatusName(byte? statusId)
        {
            return statusId switch
            {
                67 => "queued",
                71 => "rejected",
                9 => "completed",
                8 => "partial",
                52 => "error",
                59 => "furtherdetails",
                93 => "pended",
                51 => "perror",
                99 => "outcome",
                _ => "unknown" // Handles any numbers not defined in your list
            };
        }
    }
}

