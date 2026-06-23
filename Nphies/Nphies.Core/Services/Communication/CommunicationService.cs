using Cyclus.Storage.Attachment;
using Cyclus.Storage.Attachment.Enums;
using Microsoft.EntityFrameworkCore;
using Nphies.Core.Data.Entities;
using Nphies.Core.Models;
using Nphies.Core.Services.Schedule;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Nphies.Core.Services.Claims;

namespace Nphies.Core.Services.Communication
{
    public class CommunicationService : ICommunicationService
    {
        private readonly ZyklusCoreContext context;
        private readonly ISchedularService _schedular;
        private readonly IClaimService _claimService;

        public CommunicationService(ZyklusCoreContext _context, ISchedularService _schedular, IClaimService claimService)
        {
            this.context = _context;
            this._schedular = _schedular;
            _claimService = claimService;
        }

        public async Task<List<ClaimAttachmentCommunicationDto>> GetCommunication
         (
             int orginzationId,
             int facilityId,
             string adaptorCode,
             DateTime dateFrom,
             DateTime dateTo,
             long processId,
             int batchsize,
             string PayerId
         )
        {

            List<ClaimAttachmentCommunicationDto> communications = new List<ClaimAttachmentCommunicationDto>();
            try
            {
                var claims = await RetriveAttachmentClaimsAsync(orginzationId,
                                       facilityId,
                                       dateFrom,
                                       dateTo,
                                       processId,
                                       batchsize,
                                       Convert.ToInt32(PayerId)
                                    );

                if (claims != null && claims.Count > 0)
                {
                    var rcmFacility = await context.RcmFacilities.FirstOrDefaultAsync(f => 
                                                                f.FacilityId == facilityId &&
                                                                f.OrganizationId == orginzationId);
                    var facilityStorageProvider = FileStorageProviderType.All;
                    if (rcmFacility != null && rcmFacility.FileStorageProvider.HasValue)
                        facilityStorageProvider = (FileStorageProviderType)rcmFacility.FileStorageProvider;


                    var adaptors = await GetAdaptors(orginzationId, adaptorCode);
                    List<RcmAdapterMapping> rcmAdapterMapping = new List<RcmAdapterMapping>();
                    if (adaptors != null && adaptors.Count() > 0)
                    {
                        foreach (var adapterMappings in adaptors.Select(x => x.RcmAdapterMappings))
                        {
                            foreach (var adapter in adapterMappings)
                            {
                                rcmAdapterMapping.Add(new RcmAdapterMapping
                                {
                                    ParameterMasterId = adapter.ParameterMasterId,
                                    ParameterType = adapter.ParameterType,
                                    ParameterValue = adapter.ParameterValue
                                });
                            }
                        }
                        foreach (var claim in claims)
                        {
                            var adapterMapping = rcmAdapterMapping.FirstOrDefault(x => x.ParameterType == claim.PayerId.ToString()
                                                  && x.ParameterMasterId == (byte)AdaptorType.Payer);

                            var payerEntity = await context.RcmPayers.Where(x => x.PayerId == claim.PayerId).FirstOrDefaultWithNoLockAsync();

                            Tuple<string, string, string> policyLicenseAndName = await RetriveInsuranceNhicForTPA(claim, payerEntity, adapterMapping);

                            var license = policyLicenseAndName.Item1 ?? string.Empty;
                            var payerTpaNhicId = policyLicenseAndName.Item2 ?? string.Empty;

                            var clinicExternalCode = string.IsNullOrEmpty( claim.ClinicId) ? 0: Convert.ToInt32(claim.ClinicId);

                            communications.Add(new ClaimAttachmentCommunicationDto()
                            {
                                ClaimID = claim.ClaimId,
                                ClaimIdentifier = claim.ClaimIdentifier,
                                License = !string.IsNullOrEmpty(license) ? license : adapterMapping.ParameterValue.ToString(),
                                TpaNhicId = !string.IsNullOrEmpty(payerTpaNhicId) ? payerTpaNhicId : adapterMapping.ParameterValue.ToString(),
                                EncounterType = (byte)claim.EncounterType,
                                EncounterIdentifier = claim.EncounterNo,
                                OriginalEncounter = claim.OriginalEncounter,
                                DocumentReferenceId = claim.DocdocumentReferenceId,
                                Patient = GetPateintDetails(claim),
                                ClinicId = clinicExternalCode,
                                FileStorageProviderType = facilityStorageProvider
                            });
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                //throw ex;
            }
            return communications;
        }

        public async Task<List<CommunicationDto>> GetCommunicationServices
         (
             int orginzationId,
             int facilityId,
             string adaptorCode,
             DateTime dateFrom,
             DateTime dateTo,
             long processId,
             int batchsize,
             string PayerId
         )
        {

            List<CommunicationDto> communications = new List<CommunicationDto>();
            try
            {
                var claims = await RetriveClaimsAsync(orginzationId,
                                                       facilityId,
                                                       dateFrom,
                                                       dateTo,
                                                       processId,
                                                       batchsize,
                                                       Convert.ToInt32(PayerId));

                if (claims != null && claims.Count > 0)
                {
                    var adaptors = await GetAdaptors(orginzationId, adaptorCode);
                    List<RcmAdapterMapping> rcmAdapterMapping = new List<RcmAdapterMapping>();
                    if (adaptors != null && adaptors.Count() > 0)
                    {
                        foreach (var adapterMappings in adaptors.Select(x => x.RcmAdapterMappings))
                        {
                            foreach (var adapter in adapterMappings)
                            {
                                rcmAdapterMapping.Add(new RcmAdapterMapping
                                {
                                    ParameterMasterId = adapter.ParameterMasterId,
                                    ParameterType = adapter.ParameterType,
                                    ParameterValue = adapter.ParameterValue
                                });
                            }
                        }
                        foreach (var claim in claims)
                        {
                            var adapterMapping = rcmAdapterMapping.FirstOrDefault(x => x.ParameterType == claim.PayerId.ToString()
                                                  && x.ParameterMasterId == (byte)AdaptorType.Payer);

                            // Mapping Payer License
                            var payerEntity = await context.RcmPayers.AsNoTracking()
                                .Where(x => x.PayerId == claim.PayerId).FirstOrDefaultWithNoLockAsync();

                            Tuple<string, string, string> policyLicenseAndName 
                                = await RetriveInsuranceNhicForTPA(claim, payerEntity, adapterMapping);

                            var license = policyLicenseAndName.Item1 ?? string.Empty;
                            var payerTpaNhicId = policyLicenseAndName.Item2 ?? string.Empty;

                            AttachmentDto remarksAndAttachments = await GetRemarksAndAttachments(claim.RowId,
                                                                            claim.OrganizationId, claim.ClaimIdentifier, claim.ClaimItemSequence, claim.EncounterNo, claim.facilityId, claim.ClaimId);

                            if(remarksAndAttachments != null && !string.IsNullOrEmpty(remarksAndAttachments.remarks))
                            {
                                communications.Add(new CommunicationDto()
                                {
                                    ClaimId = claim.ClaimId,
                                    ClaimIdentifier = claim.ClaimIdentifier,
                                    License = !string.IsNullOrEmpty(license) ? license : adapterMapping.ParameterValue.ToString(),
                                    TpaNhicId = !string.IsNullOrEmpty(payerTpaNhicId) ? payerTpaNhicId : adapterMapping.ParameterValue.ToString(),
                                    patient = GetPateintDetails(claim),
                                    Remarks = remarksAndAttachments.remarks,
                                    payloads = remarksAndAttachments.payloads,
                                    ClaimItemSequence = claim.ClaimItemSequence,
                                    IsMohClaim = claim.IsMohClaim
                                });
                            }
                            
                        }
                    }
                }
                else
                {
                    await _schedular.UpdateCommunicationProcessQueue(
                            new ProcessQueueModel
                            {
                                FacilityId = facilityId,
                                OrginzationId = orginzationId,
                                ProcessId = processId
                            }
                        );
                }
            }
            catch (Exception ex)
            {
                //throw ex;
            }
            return communications;
        }

        private async Task<Tuple<string, string, string>> RetriveInsuranceNhicForTPA(ClaimDto claim
                        , RcmPayer payer,
                        RcmAdapterMapping rcmAdapterMapping)
        {
            string insNhicId = string.Empty;
            string tpaNhicId = string.Empty;
            string insOrTpaNhicId = string.Empty;
            int payerId = claim.PayerId;
            bool isManagedByTPA = false;
            bool isTPA = false;

            try
            {
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
            } catch(Exception ex)
            {

            }
          
            return Tuple.Create(string.Empty, string.Empty, string.Empty);
        }

        public async Task<bool> UpdateStatusAfterResubmission(ResubmissionStatusDto resubmissionStatus)
        {
            bool response2Return = false;
            try
            {
                if (resubmissionStatus.ClaimNo > 0)
                {
                    var services = await context.RcmClaimServicesDetails
                                    .Where(x => x.OrganizationId == resubmissionStatus.OrganizationId
                                           && x.ClaimId == resubmissionStatus.ClaimNo
                                           //&& x.ClaimIdentifier == resubmissionStatus.ClaimIdentifier
                                           && x.ReSubmissionStatus == (byte)CommunicationStatus.ResponseAdded
                                           && x.NphiesSeqNo == resubmissionStatus.NphiesSequenceNo)
                                    .ToListAsync();
                    if (services != null && services.Count > 0)
                    {
                        response2Return = await UpdateResubmissionStatus(resubmissionStatus, response2Return, services);
                    }



                }
            }
            catch (Exception ex)
            {
                //throw ex;
            }
            return response2Return;
        }

        private async Task<bool> UpdateResubmissionStatus(
            ResubmissionStatusDto resubmissionStatus, 
            bool response2Return,
            List<RcmClaimServicesDetail> services)
        {
            if (resubmissionStatus.Status == "Completed")
            {
                foreach (var service in services)
                    service.ReSubmissionStatus = (byte)CommunicationStatus.Completed;
                

                await context.SaveChangesAsync();

                await UpdateNPHIECommunicationLog(resubmissionStatus.OrganizationId,
                        resubmissionStatus.FacilityId,
                        resubmissionStatus.ClaimIdentifier,
                        0, "",
                        resubmissionStatus.BundleId,
                        resubmissionStatus.ParsedbjectId,
                        isUnsolicited: true,
                        Reason: resubmissionStatus.Remarks);

                response2Return = true;
            }
            else
            {
                foreach (var service in services)
                {
                    service.Status = (byte)ClaimStatus.Nphies_Rejected;
                    service.ReSubmissionStatus = null;
                    service.ProcessId = null;
                    service.NphieseRemarks = !string.IsNullOrEmpty(resubmissionStatus.Errors) ? resubmissionStatus.Errors : "Resubmission Incomplete" ;

                    //Case of Failure
                    var remarks = await context.RcmAddResponseBindings
                        .Where(r => r.RowId == service.RowId && r.IsActive == true)
                        .FirstOrDefaultAsync();
                    if(remarks != null)
                    {
                        remarks.Description = resubmissionStatus.Status;
                        remarks.IsActive = false;
                        remarks.ModifiedOn = DateTime.Now;
                        remarks.ModifiedBy = 999;
                    }
                    

                }              

                await context.SaveChangesAsync();

                await UpdateNPHIECommunicationLog(resubmissionStatus.OrganizationId,
                        resubmissionStatus.FacilityId,
                        resubmissionStatus.ClaimIdentifier,
                        0, "",
                        resubmissionStatus.BundleId,
                        resubmissionStatus.ParsedbjectId,
                        isUnsolicited: true,
                        Reason: resubmissionStatus.Errors +  Environment.NewLine + resubmissionStatus.Remarks);
                response2Return = true;
            }

            return response2Return;
        }


        private async Task<bool> UpdateNPHIECommunicationLog
         (
            int organizationId,
            int facilityId,
            string ClaimIdentifier,
            int ServiceReferenceNo,
            string comIdentifierValue,
            string RequestBundleID,
            string ResponseBundleID,
            bool isClaimByEpisode = false,
            bool isUnsolicited = false,
            string Reason = ""
         )
        {
            bool response2Return = false;
            try
            {
                RcmNphiesclaimCommunicationLog communicationLog = new RcmNphiesclaimCommunicationLog();
                communicationLog.OrganizationId = organizationId;
                communicationLog.FacilityId = facilityId;
                communicationLog.CreatedBy = 999;
                communicationLog.CreatedOn = DateTime.Now;
                communicationLog.ClaimIdentifier = ClaimIdentifier;
                communicationLog.RequestBundleId = RequestBundleID;
                communicationLog.ResponseBundleId = ResponseBundleID;
                communicationLog.Reason = Reason;
                communicationLog.ServiceReferenceNo = ServiceReferenceNo;
                communicationLog.LineItemNo = await GetNextComminucationLogLineItemNo(organizationId, facilityId, ClaimIdentifier);
                await context.RcmNphiesclaimCommunicationLogs.AddAsync(communicationLog);
                if (await context.SaveChangesAsync() > 0)
                    response2Return = true;
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return response2Return;
        }

        private async Task<byte> GetNextComminucationLogLineItemNo(int organizationId, int facilityId, string ClaimIdentifier)
        {
            byte LineItemNo = 0;
            var communicationLog = await context.RcmNphiesclaimCommunicationLogs
                                          .FirstOrDefaultAsync(x => x.OrganizationId == organizationId
                                                               && x.FacilityId == facilityId
                                                               && x.ClaimIdentifier == ClaimIdentifier);
            if (communicationLog != null)
                LineItemNo = (byte)communicationLog.LineItemNo;

            return ++LineItemNo;
        }

        /*
         * * Get Attachments and Remarks
         */
        private async Task<AttachmentDto> GetRemarksAndAttachments(long rowId, int organizationId, string ClaimIdentifier, int nphiesSeqNo, string encounterIdentifier, int facilityId, long claimId)
        {
            AttachmentDto attachmentModel = new AttachmentDto();
            try
            {

                var responseBinding = await context.RcmAddResponseBindings.AsNoTracking()
                            .Where(r => r.RowId == rowId && r.IsActive == true)
                            .Select(responseBinding => new {
                                RemarksId = responseBinding.RemarksId,
                                responseBinding.AttachId,

                            })
                                .FirstOrDefaultWithNoLockAsync();

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
                        var rcmFacility = await context.RcmFacilities.FirstOrDefaultAsync(f =>
                            f.FacilityId == facilityId &&
                            f.OrganizationId == organizationId);

                        var facilityStorageProvider = FileStorageProviderType.All;
                        if (rcmFacility != null && rcmFacility.FileStorageProvider.HasValue)
                            facilityStorageProvider = (FileStorageProviderType)rcmFacility.FileStorageProvider;

                        if (facilityStorageProvider == FileStorageProviderType.All || facilityStorageProvider == FileStorageProviderType.Mongo)
                        {
                            var documentIds = await _claimService.RetrieveAttachmentDocumentIds(claimId);

                            var storageProvider = FileStorageProviderFactory.GetProvider(FileStorageProviderType.Mongo);
                            var mongoAttachments = await storageProvider.GetFiles(
                                rcmFacility?.ExternalCode2,
                                Convert.ToInt16(rcmFacility?.ExternalCode),
                                Convert.ToInt32(encounterIdentifier), documentIds: documentIds);

                            if (mongoAttachments != null && mongoAttachments.Any())
                                attachmentModel.payloads = mongoAttachments.Select(GetPayloadWithAttachment).ToList();
                        }
                        else if (facilityStorageProvider == FileStorageProviderType.Sql)
                        {
                            attachmentModel.payloads = await context.RcmAttachments.AsNoTracking()
                                .Where(x => x.OrganizationId == organizationId
                                            && x.AttachmentId == responseBinding.AttachId)
                                .Select(x => new Payload
                                {
                                    contentType = !string.IsNullOrEmpty(x.FilePath) ? x.FileType : null,
                                    creation = x.CreatedOn.Value.ToString("yyyy-MM-dd"),
                                    title = x.FileName,
                                    data = !string.IsNullOrEmpty(x.FilePath) && File.Exists(x.FilePath)
                                        ? System.IO.File.ReadAllBytes(x.FilePath)
                                        : null

                                }).ToListAsync();
                        }
                    }

                }
                else
                {
                    //var missingRemarksService = await context.RcmClaimServicesDetails
                    //    .Where(x => x.RowId == rowId).FirstOrDefaultAsync();
                    //if(missingRemarksService != null)
                    //{
                    //    missingRemarksService.ReSubmissionStatus = null;
                    //    missingRemarksService.ProcessId = null;
                    //    missingRemarksService.Remarks = "Remark or Attachment missing - 404";
                    //    await context.SaveChangesAsync();
                    //}
                }
            }
            catch (Exception ex)
            {
                throw;
            }
            return attachmentModel;
        }

        private Payload GetPayloadWithAttachment(FileStorageBaseModel fileAttachment)
        {
            try
            {
                if (fileAttachment != null && fileAttachment.FileData != null)
                {
                    byte[] byteArrFile = fileAttachment.FileData;

                    if (byteArrFile != null)
                    {
                        var payload = new Payload()
                        {
                            contentType = "application/pdf",
                            creation = DateTime.Now.ToString("yyyy-MM-dd"),
                            data = byteArrFile,
                            title = fileAttachment.DocumentReferenceId + ".pdf"
                        };
                        return payload;
                    }
                }
            }
            catch (Exception ex)
            {

                //await logger.WriteErrorAsync("Error while uploading file", item.ClaimID);
            }


            return new Payload();
        }

        private async Task<IEnumerable<RcmAdapter>> GetAdaptors(int organizationId, string adaptorCode)
        {
            return await context.RcmAdapters
                                .Include(adaptor => adaptor.RcmAdapterMappings
                                    .Where(x => x.ParameterMasterId == (byte)AdaptorType.Payer)
                                )
                                .AsNoTracking()
                                .Where(adaptor => adaptor.OrganizationId == organizationId
                                    && adaptor.AdaptorCode == adaptorCode
                                    && adaptor.IsActive == true
                                )
                                .ToListAsync();
        }
        private Patient GetPateintDetails(ClaimDto patient)
        {
            Patient patientDetail = new Patient();
            patientDetail.PatientID = patient.PatientFileNumber;
            patientDetail.PatientIdentificationType = patient.PatientIdentificationNo.ToString();
            patientDetail.PatientIdentificationDescription = string.Empty;
            patientDetail.PatientIdentificationNo = patient.NationalityId.ToString().Trim();
            patientDetail.STATUS = AppCons.Active;
            patientDetail.FirstName = patient.PatientName;
            patientDetail.MiddleName = string.Empty;
            patientDetail.LastName = Convert.ToString(patient.PatientSurnameFamilyName);
            patientDetail.FirstNameAr = string.Empty;
            patientDetail.MiddleNameAr = string.Empty;
            patientDetail.LastNameAr = string.Empty;
            patientDetail.PhoneResi = string.Empty;
            patientDetail.PhoneOffice = string.Empty;
            patientDetail.MobileNumber = Convert.ToString(patient.PatientMobileNo);
            patientDetail.EmailAddress = string.Empty;
            patientDetail.DateofBirth = patient.PatientDob.Value.ToNaphiesFormat();
            patientDetail.Gender = patient.PatientGender != null ? patient.PatientGender.Value.ToString() : string.Empty;
            patientDetail.POBox = string.Empty;
            patientDetail.ZipCode = string.Empty;
            patientDetail.Address = Convert.ToString(patient.Address1);
            patientDetail.ISOCountryID = Convert.ToString(patient.PatientNationality);
            patientDetail.MaritalStatus = patient.MaritalStatus != null ? patient.MaritalStatus.ToString() : string.Empty;

            return patientDetail;

        }
        private async Task<List<ClaimDto>> RetriveClaimsAsync(int orginaztionId,
            int facilityId,
            DateTime dateFrom,
            DateTime dateTo,
            long processId,
            int batchSize,
            int payerId)
        {
            try
            {
                var claims = await Filter(orginaztionId, facilityId, dateFrom, dateTo, processId, payerId)
                                        .Take(batchSize)
                                        .ToListWithNoLockAsync();

                //if(claims != null && claims.Count > 0)
                //    claims = claims.GroupBy(x => x.ClaimId)
                //         .Select(claim => SelectProperties(claim)).ToList();

                return claims;
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        private async Task<List<ClaimDto>> RetriveAttachmentClaimsAsync(int orginaztionId,
            int facilityId,
            DateTime dateFrom,
            DateTime dateTo,
            long processId,
            int batchSize,
            int payerId)
        {
            try
            {
                var claims = await AttachmentFilter(orginaztionId, facilityId, dateFrom, dateTo, processId, payerId)
                                  .ToListAsync();

                if (claims != null && claims.Count > 0)
                    claims = claims.GroupBy(x => x.ClaimId)
                         .Select(claim => SelectProperties(claim)).ToList();

                return claims;
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        private static ClaimDto SelectProperties(IGrouping<long, ClaimDto> claim)
        {
            return new ClaimDto
            {
                OrganizationId = claim.First().OrganizationId,
                ClaimId = claim.First().ClaimId,
                ClaimIdentifier = claim.First().ClaimIdentifier,
                PayerId = claim.First().PayerId,
                PayerPolicyId = claim.First().PayerPolicyId,
                PatientFileNumber = claim.First().PatientFileNumber,
                PatientIdentificationNo = claim.First().PatientIdentificationNo,
                NationalityId = claim.First().NationalityId,
                PatientName = claim.First().PatientName,
                PatientSurnameFamilyName = claim.First().PatientSurnameFamilyName,
                PatientMobileNo = claim.First().PatientMobileNo,
                PatientDob = claim.First().PatientDob.Value,
                PatientGender = claim.First().PatientGender,
                Address1 = claim.First().Address1,
                PatientNationality = claim.First().PatientNationality,
                MaritalStatus = claim.First().MaritalStatus,
                EncounterType = claim.First().EncounterType,
                EncounterNo = claim.First().EncounterNo,
                OriginalEncounter = claim.First().OriginalEncounter,
                DocdocumentReferenceId = claim.First().DocdocumentReferenceId,
                ClaimItemSequence = claim.First().ClaimItemSequence,
                ClinicId = claim.First().ClinicId,
            };
        }

        private IQueryable<ClaimDto> Filter(int orginaztionId, 
            int facilityId,
            DateTime dateFrom,
            DateTime dateTo,
            long processId,
            int payerId)
        {
            return (from claim in context.RcmClaims.AsNoTracking()
                    join services in context.RcmClaimServicesDetails.AsNoTracking()
                    on claim.ClaimId equals services.ClaimId
                    join patient in context.RcmClaimPatientDetails.AsNoTracking()
                    on claim.ClaimId equals patient.ClaimId
                    where claim.OrganizationId == orginaztionId
                    && claim.FacilityId == facilityId
                    && claim.PayerId == payerId

                    //&& claim.InvoiceDate.Value.Date >= dateFrom.Date
                    //&& claim.InvoiceDate.Value.Date <= dateTo.Date
                    && services.ProcessId == processId
 					&& services.ReSubmissionStatus == (byte)CommunicationStatus.ResponseAdded
                    select new ClaimDto
                    {
                        OrganizationId = claim.OrganizationId,
                        ClaimId = claim.ClaimId,
                        ClaimIdentifier = claim.ClaimIdentifier,
                        PayerId = claim.PayerId,
                        PayerPolicyId = claim.PayerPolicyId,
                        PatientFileNumber = patient.PatientFileNumber,
                        PatientIdentificationNo = patient.PatientIdentificationNo,
                        NationalityId = patient.NationalityId,
                        PatientName = patient.PatientName,
                        PatientSurnameFamilyName = patient.PatientSurnameFamilyName,
                        PatientMobileNo = patient.PatientMobileNo,
                        PatientDob = patient.PatientDob.Value,
                        PatientGender = patient.PatientGender,
                        Address1 = patient.Address1,
                        PatientNationality = patient.PatientNationality,
                        MaritalStatus = patient.MaritalStatus,
                        ClaimItemSequence = services.NphiesSeqNo ?? 0,
                        RowId = services.RowId,
                        IsMohClaim = claim.isMoh.HasValue ? claim.isMoh.Value : false,
                         facilityId = claim.FacilityId,
                         EncounterNo = claim.EncounterNo.ToString(),

                    });
        }

        private IQueryable<ClaimDto> AttachmentFilter(int orginaztionId,
             int facilityId,
             DateTime dateFrom,
             DateTime dateTo,
             long processId,
             int payerId)
        {
            return (from claim in context.RcmClaims.AsNoTracking()
                    join patient in context.RcmClaimPatientDetails.AsNoTracking()
                    on claim.ClaimId equals patient.ClaimId
                    join clinic in context.RcmClinics.AsNoTracking()
                    on claim.ClinicId equals clinic.ClinicId
                    where claim.OrganizationId == orginaztionId
                        && claim.FacilityId == facilityId
                       // && claim.ClaimId == 14088905
                        //&& claim.PayerId == payerId
                        //&& claim.InvoiceDate.Value.Date >= dateFrom.Date
                        //&& claim.InvoiceDate.Value.Date <= dateTo.Date
                        && claim.ProcessId == processId
                        && (claim.ISPDFAttached == null || claim.ISPDFAttached == false)
                    select new ClaimDto
                    {
                        OrganizationId = claim.OrganizationId,
                        ClaimId = claim.ClaimId,
                        ClaimIdentifier = claim.ClaimIdentifier,
                        PayerId = claim.PayerId,
                        PayerPolicyId = claim.PayerPolicyId,
                        PatientFileNumber = patient.PatientFileNumber,
                        PatientIdentificationNo = patient.PatientIdentificationNo,
                        NationalityId = patient.NationalityId,
                        PatientName = patient.PatientName,
                        PatientSurnameFamilyName = patient.PatientSurnameFamilyName,
                        PatientMobileNo = patient.PatientMobileNo,
                        PatientDob = patient.PatientDob.Value,
                        PatientGender = patient.PatientGender,
                        Address1 = patient.Address1,
                        PatientNationality = patient.PatientNationality,
                        MaritalStatus = patient.MaritalStatus,
                        DocdocumentReferenceId = claim.DocumentReferenceNo.ToString(),
                        EncounterNo = claim.EncounterNo.ToString(),
                        OriginalEncounter = claim.OriginalEncounter,
                        EncounterType = claim.EncounterType.Value,
                        ClinicId = clinic.ExternalCode 
                    });
        }
    }
}
