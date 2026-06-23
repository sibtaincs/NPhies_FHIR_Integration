using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json.Linq;
using Nphies.Core.Data.Common;
using Nphies.Core.Data.Entities;
using Nphies.Core.DTOs;
using Nphies.Core.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text.RegularExpressions;
using System.Text;
using System.Threading.Tasks;
using System.Net.Http;
using Newtonsoft.Json;

namespace Nphies.Core.Services.Claims
{
    public interface IClaimUpdate
    {
        Task<ApiResponseOnUpdate> UpdateMedicalDataForClaim(EncounterMedicalDetail payload);
    }
    public class ClaimUpdate : IClaimUpdate
    {
        private readonly ZyklusCoreContext context;
        private readonly IConfiguration config;
        private readonly IMemoryCache memoryCache;
        private readonly ILogger<ClaimService> logger;
        public ClaimUpdate(ZyklusCoreContext context,
            IConfiguration _config,
            IMemoryCache memoryCache,
            ILogger<ClaimService> logger)
        {
            this.context = context;
            this.config = _config;
            this.memoryCache = memoryCache;
            this.logger = logger;
        }

        public async Task<ApiResponseOnUpdate> UpdateMedicalDataForClaim(EncounterMedicalDetail claimEncounterMedical)
        {
            var clientId = claimEncounterMedical.ClientId;
            var client = Convert.ToInt32(clientId);
            try
            {
                JObject medicalData = JObject.Parse(claimEncounterMedical.MedicalData);
                InsertMedicalRecordAsync(
                                            client,
                                            claimEncounterMedical.OrganizationId,
                                           claimid: claimEncounterMedical.ClaimId,
                                           previousDoctorId: claimEncounterMedical.DoctorId,
                                           facilityId: claimEncounterMedical.FacilityID,
                                           medicalRecord: medicalData,
                                           encounterType: (EncounterType)claimEncounterMedical.EncounterType)
                    .GetAwaiter().GetResult();

                bool IsForKsa = (client == (int)ClientClusters.DUBAI) ? false : true;

                if (!IsForKsa)
                    await DeleteExistingObservation(claimEncounterMedical);

                await UpdateServicePerformedStatus(claimEncounterMedical, medicalData, claimEncounterMedical.FacilityID);
                if (!IsForKsa)
                    await FillClaimObservations(medicalData, claimEncounterMedical.ClaimId, claimEncounterMedical.OrganizationId, IsForKsa);

                return new ApiResponseOnUpdate { Success = true, Message = "Success" };
            }
            catch (Exception ex)
            {

                return new ApiResponseOnUpdate { Success = false, Message = ex.Message };
            }
        }
        private async Task<Tuple<long, short>> GetConsultationServiceOfClaim(long claimId)
        {
            List<string> validCodes = new List<string> { "9", "9.01", "10", "10.01", "11", "11.01" };

            // Get the first service which should be consultation
            var consultationService = await context.RcmClaimServicesDetails
                .Where(x => x.ClaimId == claimId && validCodes.Contains(x.StandardCode))
                .OrderBy(x => x.RowId)
                .Select(x => new
                {
                    ServiceId = x.ServiceId,
                    LineItemNumber = x.LineitemNo
                })
                .FirstOrDefaultAsync();

            if (consultationService != null)
            {
                return Tuple.Create(consultationService.ServiceId, consultationService.LineItemNumber);
            }
            else
            {
                var first_availableService = await context.RcmClaimServicesDetails
                .Where(x => x.ClaimId == claimId)
                .OrderBy(x => x.RowId)
                .Select(x => new
                {
                    ServiceId = x.ServiceId,
                    LineItemNumber = x.LineitemNo
                })
                .FirstOrDefaultAsync();

                if (first_availableService != null)
                {
                    return Tuple.Create(first_availableService.ServiceId, first_availableService.LineItemNumber);
                }
            }

            return Tuple.Create((long)0, (short)0);
        }
        private async Task InsertChiefComplaintObservation(long claimId, int OrgId, DHPO_Observation observation, Tuple<long, short> consultation)
        {
            // Create a new observation
            var newObservation = new Claim_Service_Observation
            {
                ClaimID = claimId,
                OrganizationID = OrgId,
                ServiceID = consultation.Item1,
                ServiceLineitemNo = consultation.Item2,
                LineItemNo = await GetNextLineItemNumberAsync(claimId, consultation.Item1, consultation.Item2),
                Type = observation.Type,
                Code = observation.Code.ToString(),
                Value = observation.Value,
                Valuetype = observation.ValueType,
                Status = 2,
                IsActive = true,
                CreatedBy = -1,
                CreatedOn = DateTime.Now,
                ModifiedBy = null,
                ModifiedOn = null,
            };

            // Add the new observation to the context
            context.Claim_Service_Observations.Add(newObservation);
            try
            {
                await context.SaveChangesAsync();
            }
            catch (Exception ex)
            {

            }

        }
        private async Task FillClaimObservations(JObject medicalRecord, long claimId, int orgId, bool isForKsa)
        {
            if (medicalRecord != null && medicalRecord.HasValues)
            {

                if (string.IsNullOrWhiteSpace(medicalRecord["medicalInfo"].ToString()))
                    return;
                DHPO_Observation observation = new DHPO_Observation();

                JObject medicalInfo = JObject.Parse(medicalRecord["medicalInfo"].ToString());
                if (medicalInfo["cheifComplaint"] != null && medicalInfo["cheifComplaint"].HasValues)
                {
                    var consultation = await GetConsultationServiceOfClaim(claimId);
                    if (consultation.Item1 > 0 && consultation.Item2 > 0)
                    {
                        // Access and print all values in chiefComplaint
                        var chiefComplaintObj = medicalInfo["cheifComplaint"];
                        var chiefComplaint = chiefComplaintObj["mainSymptoms"]?.ToString();
                        //string encounterNo = chiefComplaint["encounterNo"]?.ToString();
                        //string mainSymptoms = chiefComplaint["mainSymptoms"]?.ToString();
                        string hopi = chiefComplaintObj["physicianNotesConditions"]?.ToString();
                        //string significantSigns = chiefComplaint["significantSigns"]?.ToString();
                        //string createdOn = chiefComplaint["createdon"]?.ToString();

                        observation.Type = (short)ObservationParameterType.Text;
                        observation.Code = (short)TextCodes.PresentingComplaint;// "PresentingComplaint";

                        string HPIObservation = "";
                        int length = hopi == "" ? chiefComplaint.ToString().Length : hopi.Length;
                        if (length > 1999)
                        {
                            HPIObservation = (hopi == "" ? chiefComplaint.ToString() : hopi).Substring(0, 1999);
                        }
                        else
                        {
                            HPIObservation = (hopi == "" ? chiefComplaint.ToString() : hopi);
                        }
                        observation.Value = HPIObservation;
                        observation.ValueType = ((short)ValueTypeCodes.Other).ToString();// "Other";

                        if (!isForKsa)
                        {
                            if (!string.IsNullOrWhiteSpace(observation.Value))
                                await InsertChiefComplaintObservation(claimId, orgId, observation, consultation);
                        }

                        Console.WriteLine("Inserting Chief Complaint observation");


                    }


                }
                else
                {

                }

            }
        }
        private async Task<short> GetNextLineItemNumberAsync(long claimId, long serviceId, short lineitemnumber)
        {
            var latestObservation = await context.Claim_Service_Observations
                .Where(x => x.ClaimID == claimId && x.ServiceID == serviceId && x.ServiceLineitemNo == lineitemnumber)
                .OrderByDescending(x => x.LineItemNo)
                .FirstOrDefaultAsync();

            return (short)((latestObservation?.LineItemNo ?? 0) + 1);
        }
        private void InsertLabObservationsAsync(RcmClaimServicesDetail item, DHPO_Observation observation,
         short lineItemNumber)
        {

            if (observation.Type == 0
                || observation.Code == 0
                || observation.Value == null
                || string.IsNullOrWhiteSpace(observation.Value))
            {
                return;
            }
            // Create a new observation
            var newObservation = new Claim_Service_Observation
            {
                ClaimID = item.ClaimId,
                OrganizationID = item.OrganizationId,
                ServiceID = item.ServiceId,
                ServiceLineitemNo = item.LineitemNo,
                LineItemNo = lineItemNumber,
                Type = observation.Type,
                Code = observation.Code.ToString(),
                Value = observation.Value,
                Valuetype = observation.ValueType,
                Status = 2,
                IsActive = true,
                CreatedBy = -1,
                CreatedOn = DateTime.Now,
                ModifiedBy = null,
                ModifiedOn = null,
            };

            // Add the new observation to the context
            context.Claim_Service_Observations.Add(newObservation);

        }
        private void PersistLabReports(
                List<LabReportDetail> reportDetail,
                RcmClaimServicesDetail claimServicesDetail,
                EncounterMedicalDetail medicalDetail,
                int facilityId)
        {
            var reportResultData = reportDetail.Select(
                labReport => new RcmLabResult()
                {
                    ClaimId = claimServicesDetail.ClaimId,
                    ServiceReferenceNumber = claimServicesDetail.ServiceReferenceNumber,
                    CreatedBy = -1,
                    CreatedOn = DateTime.Now,
                    IsActive = true,
                    FacilityId = facilityId,
                    TestId = labReport.testId,
                    LabResult = labReport.labResult,
                    ServiceCode = labReport.serviceCode,

                }).ToList();

            var existedItem = reportResultData.Where(x => context.RcmLabResults
                            .FirstOrDefault(p => p.ClaimId == x.ClaimId
                            && p.ServiceCode == x.ServiceCode
                            && p.FacilityId == x.FacilityId
                            && p.ServiceReferenceNumber == x.ServiceReferenceNumber) == null).ToList();

            if (existedItem.Any())
                context.RcmLabResults.AddRange(existedItem);
        }
        private void InsertRadiologyResultAsync(RcmClaimServicesDetail item, string radResult, short lineItemNumber)
        {
            // Create a new observation
            var newObservation = new Claim_Service_Observation
            {
                ClaimID = item.ClaimId,
                OrganizationID = item.OrganizationId,
                ServiceID = item.ServiceId,
                ServiceLineitemNo = item.LineitemNo,
                LineItemNo = lineItemNumber, //await GetNextLineItemNumberAsync(item.ClaimID, item.ServiceID, context) ,
                Type = (short)ObservationParameterType.Text,
                Code = ((short)TextCodes.Description).ToString(),
                Value = radResult,
                Valuetype = ((int)ValueTypeCodes.Other).ToString(),
                Status = 2,
                IsActive = true,
                CreatedBy = -1,
                CreatedOn = DateTime.Now,
                ModifiedBy = null,
                ModifiedOn = null,
            };

            // Add the new observation to the context
            context.Claim_Service_Observations.Add(newObservation);
        }
        private async Task UpdateServicePerformedStatus(EncounterMedicalDetail medicalDetail, JObject medicalRecord, int facilityId)
        {
            bool IsForKsa = (  Convert.ToInt32(medicalDetail.ClientId) == (int)ClientClusters.DUBAI) ?  false : true;
            const string Hb1CCode = "02011052";

            if (medicalRecord != null && medicalRecord.HasValues)
            {

                var reportDetails = new List<LabReportDetail>();
                foreach (var item in context.RcmClaimServicesDetails.Where(t => t.ClaimId == medicalDetail.ClaimId &&
                             (t.ServiceCode.StartsWith("02")
                              || t.ServiceCode.StartsWith("03")
                             )))
                {
                    var checkLabReportInOtherBranch = false;
                    if (item.ServiceCode.StartsWith("02"))
                    {
                        if (medicalRecord != null && medicalRecord.HasValues)
                        {
                            JArray jlaboratory = JArray.Parse(medicalRecord["medicalInfo"]["laboratory"].ToString()) as JArray;
                            if (jlaboratory != null && jlaboratory.Children().Count() > 0)
                            {
                                var result = jlaboratory.FirstOrDefault(predicate =>
                                    predicate["invoiceNo"].ToString() == item.ServiceReferenceNumber
                                 && predicate["serviceCode"].ToString().Trim() == item.ServiceCode.Trim()
                                 // && predicate["referenceId"].ToString() == item.ServiceReferenceIdentity
                                 );

                                if (result != null)
                                {
                                    item.IsPerformed = true;


                                    if (!IsForKsa)
                                    {
                                        var labResult = jlaboratory.Where(predicate =>
                                           predicate["invoiceNo"].ToString() == item.ServiceReferenceNumber
                                        && predicate["serviceCode"].ToString().Trim() == item.ServiceCode.Trim()).ToList();

                                        JArray filterArray = new JArray(labResult);
                                        DataTable dtLabProcedure = ObservationCreator.ConvertJArrayToDataTable(filterArray);

                                        string standardCode = item.StandardCode ?? "";//get from item.StandarcCode
                                        List<DHPO_Observation> observationList = ObservationCreator.FillObservation(item, dtLabProcedure, standardCode);
                                        int maxLineItemNumber = await GetNextLineItemNumberAsync(item.ClaimId, item.ServiceId, item.LineitemNo);
                                        foreach (var observation in observationList)
                                        {


                                            Console.WriteLine("Inserting Lab observation");
                                            InsertLabObservationsAsync(item, observation, (short)maxLineItemNumber);

                                            maxLineItemNumber++;
                                        }
                                    }
                                    else
                                    {
                                        if (result["serviceCode"].ToString() != Hb1CCode)
                                            continue;

                                        var labReportDetail = new LabReportDetail()
                                        {
                                            testId = Convert.ToInt32(result["testID"].ToString()),
                                            labResult = result["labResult"].ToString(),
                                            serviceCode = result["serviceCode"].ToString(),
                                        };
                                        if (reportDetails.FirstOrDefault(x =>
                                            x.testId == labReportDetail.testId &&
                                            x.labResult == labReportDetail.labResult &&
                                            x.serviceCode == labReportDetail.serviceCode) == null)
                                        {
                                            reportDetails.Add(labReportDetail);
                                            PersistLabReports(reportDetails, item, medicalDetail, facilityId);
                                        }




                                    }

                                }

                                else
                                    checkLabReportInOtherBranch = true;
                            }
                            else
                                checkLabReportInOtherBranch = true;
                        }
                        else
                            checkLabReportInOtherBranch = true;
                    }
                    else if (item.ServiceCode.StartsWith("03"))
                    {
                        if (medicalRecord != null && medicalRecord.HasValues)
                        {
                            JArray jradiology = JArray.Parse(medicalRecord["medicalInfo"]["radiology"].ToString()) as JArray;
                            if (jradiology != null && jradiology.Children().Count() > 0)
                            {
                                var result = jradiology.FirstOrDefault(predicate => predicate["invoiceNo"].ToString() == item.ServiceReferenceNumber
                                 && predicate["serviceCode"].ToString().Trim() == item.ServiceCode.Trim()
                                 //&& predicate["referenceId"].ToString() == item.ServiceReferenceIdentity
                                 );
                                if (result != null)
                                {
                                    item.IsPerformed = true;
                                    if (!IsForKsa)
                                    {
                                        int maxLineItemNumber = await GetNextLineItemNumberAsync(item.ClaimId, item.ServiceId, item.LineitemNo);
                                        string radResult = result["result"].ToString();
                                        InsertRadiologyResultAsync(item, radResult, (short)maxLineItemNumber);
                                        maxLineItemNumber++;
                                    }

                                }
                                else
                                    item.IsPerformed = false;
                            }
                        }
                    }


                    if (checkLabReportInOtherBranch && IsForKsa && item.ServiceCode == Hb1CCode)
                    {
                        var invoiceInfo = JsonConvert.DeserializeObject<List<InvoiceInfoDetailModel>>(medicalRecord["invoiceInfoDetails"].ToString());
                        if (invoiceInfo != null && invoiceInfo.Any())
                        {
                            reportDetails = await UpdateServicePerformedStatus(item, medicalDetail, invoiceInfo);
                            if (reportDetails.Any())
                            {
                                PersistLabReports(reportDetails, item, medicalDetail, facilityId);
                                item.IsPerformed = true;
                            }
                        }
                    }

                }
                foreach (var item in context.RcmClaimServicesDetails.Where(t => t.ClaimId == medicalDetail.ClaimId && (!t.ServiceCode.StartsWith("02")
                      || !t.ServiceCode.StartsWith("03")

                      )))
                {
                    if (!string.IsNullOrWhiteSpace(item.ToothNo) && !item.ToothNo.Equals("0"))
                    {
                        int maxLineItemNumber = await GetNextLineItemNumberAsync(item.ClaimId, item.ServiceId, item.LineitemNo);
                        InsertDentalActivitytAsync(item, item.ToothNo, (short)maxLineItemNumber);
                        maxLineItemNumber++;
                    }
                }

                try
                {
                    await context.SaveChangesAsync();
                }
                catch (Exception ex)
                {
                    //foreach (var eve in ex.EntityValidationErrors)
                    //{
                    //    Console.WriteLine($"Entity of type {eve.Entry.Entity.GetType().Name} in state {eve.Entry.State} has the following validation errors:");
                    //    foreach (var ve in eve.ValidationErrors)
                    //    {
                    //        Console.WriteLine($"- Property: {ve.PropertyName}, Error: {ve.ErrorMessage}");
                    //    }
                    //}
                    //throw; // Re-throw or handle as needed
                }

            }


        }
        private void InsertDentalActivitytAsync(RcmClaimServicesDetail item, string value, short lineItemNumber)
        {
            // Create a new observation
            var newObservation = new Claim_Service_Observation
            {
                ClaimID = item.ClaimId,
                OrganizationID = item.OrganizationId,
                ServiceID = item.ServiceId,
                ServiceLineitemNo = item.LineitemNo,
                LineItemNo = lineItemNumber, //await GetNextLineItemNumberAsync(item.ClaimID, item.ServiceID, context) ,
                Type = (short)ObservationParameterType.UniversalDental,
                Code = value,
                Value = value,
                Valuetype = ((int)ValueTypeCodes.ToothNumber).ToString(),
                Status = 2,
                IsActive = true,
                CreatedBy = -1,
                CreatedOn = DateTime.Now,
                ModifiedBy = null,
                ModifiedOn = null,
            };

            // Add the new observation to the context
            context.Claim_Service_Observations.Add(newObservation);
        }
        private async Task<List<LabReportDetail>> UpdateServicePerformedStatus(RcmClaimServicesDetail claimServicesDetail, EncounterMedicalDetail medicalDetail, List<InvoiceInfoDetailModel> invoiceDetailModels)
        {
            var baseurl = config["InfoMedServiceUrl"];
            //TODO:: Irshad - call the new api to get the report details
            var apiUrl = $"{baseurl}/lab-report/detail";
            var reportDetails = new List<LabReportDetail>();
            foreach (var invoiceModel in invoiceDetailModels)
            {
                var facility = await context.RcmFacilities.Where(x => x.ExternalCode == invoiceModel.PerformedBranch)
                    .AsNoTracking()
                    .Select(f => new { f.ExternalCode, f.ExternalCode2 })
                    .FirstOrDefaultAsync();

                if (facility == null) continue;
                var labReportDetailRequest = new LabReportDetailRequest()
                {
                    EncounterType = medicalDetail.EncounterType,
                    EncounterNumber = Convert.ToInt32(medicalDetail.EncounterNo),
                    PatientId = Convert.ToInt32(medicalDetail.PatientMrn),
                    FacilityGroup = facility.ExternalCode2,
                    FacilityId = Convert.ToInt32(invoiceModel.PerformedBranch),
                    InvoiceNumber = invoiceModel.InvoiceNo,
                    ServiceCode = claimServicesDetail.ServiceCode
                };

                var response = await ApiHelper.CallApiAsync<LabReportDetailResponse>(apiUrl, HttpMethod.Post, labReportDetailRequest);
                if (response != null && response.data.Any())
                    reportDetails.AddRange(response.data.ToList());
            }
            return reportDetails;
        }
        private async Task DeleteExistingObservation(EncounterMedicalDetail claimEncounterMedical)
        {

            try
            {
                var observationsToDelete = await context.Claim_Service_Observations
               .Where(x => x.ClaimID == claimEncounterMedical.ClaimId && x.CreatedBy == -1).ToListAsync();
                context.Claim_Service_Observations.RemoveRange(observationsToDelete);
                await context.SaveChangesAsync();
            }
            catch (Exception ex)
            {

                // throw;
            }
        }
        private async Task InsertMedicalRecordAsync(int client ,int orgId, long claimid, int previousDoctorId, int facilityId,
            JObject medicalRecord, EncounterType encounterType, bool IsRefetchMedicalInfo = false)
        {
            try
            {
                //  MedicalDataInfo MedicalDataInfo = MedicalInfofactory.GetMeficalInfo(setting, (Clients)setting.Client);
                if (medicalRecord != null && medicalRecord.HasValues)
                {
                    if (string.IsNullOrWhiteSpace(medicalRecord["medicalInfo"].ToString()))
                        return;

                    JObject medicalInfo = JObject.Parse(medicalRecord["medicalInfo"].ToString());
                    JObject encounterDetail = JObject.Parse(medicalRecord["encounter"].ToString());
                    if (encounterDetail.HasValues)
                    {
                        string encounterDoctor = encounterDetail["doctorId"].ToString();
                        bool sameDoctor = await IsDoctorChanged(orgId,facilityId, previousDoctorId, encounterDoctor);
                        if (sameDoctor)
                        {
                            int doctorId = await RetriveDoctorID(orgId,facilityId, previousDoctorId, encounterDoctor);
                            await UpdateClaimDoctor(claimid, doctorId);
                        }

                    }
                    if (medicalInfo.HasValues)
                    {

                        await InsertVitalSignAndChiefComplaintsAsync(orgId, claimid,
                            medicalInfo["cheifComplaint"].ToString(),
                            medicalInfo["vitalSign"].ToString(),
                            IsRefetchMedicalInfo,
                            medicalInfo["treatmentPlan"] != null && medicalInfo["treatmentPlan"].Type != JTokenType.Null ? medicalInfo["treatmentPlan"].ToString() : String.Empty,
                            medicalInfo["patientHistory"] != null && medicalInfo["patientHistory"].Type != JTokenType.Null ? medicalInfo["patientHistory"].ToString() : String.Empty
                            , medicalInfo["physicalExamination"] != null && medicalInfo["physicalExamination"].Type != JTokenType.Null ? medicalInfo["physicalExamination"].ToString() : String.Empty
                            );
                        if (encounterType != EncounterType.InPatient)
                        {
                            if (client == (int)ClientClusters.DUBAI)
                            {
                                JArray jdiagnosis = JArray.Parse(medicalInfo["approvalDiagnosis"].ToString()) as JArray;
                                if (jdiagnosis != null && jdiagnosis.Count > 0)
                                {
                                    await InsertApprovalDiagnosisAsyn(medicalInfo["approvalDiagnosis"].ToString(), claimid, orgId);
                                }
                                else
                                {
                                    await InsertDubaiDiagnosisAsync(claimid, medicalInfo["diagnosis"].ToString(), orgId);
                                }

                            }
                            else
                            {
                                await InsertKSADiagnosisAsync(claimid, medicalInfo["diagnosis"].ToString(), orgId);
                            }

                        }

                        if (encounterType == EncounterType.InPatient)
                        {
                            if (client == (int)ClientClusters.DUBAI)
                            {
                                JArray jdiagnosis = JArray.Parse(medicalInfo["approvalDiagnosis"].ToString()) as JArray;
                                if (jdiagnosis != null && jdiagnosis.Count > 0)
                                {
                                    await InsertApprovalDiagnosisAsyn(medicalInfo["approvalDiagnosis"].ToString(), claimid, orgId);
                                }
                                else if (!string.IsNullOrWhiteSpace(medicalInfo["discharge"].ToString()))
                                {
                                    await InsertDischargeSummaryAsync(claimid, medicalInfo["discharge"].ToString(), encounterDetail.ToString(), orgId);
                                    if (!string.IsNullOrWhiteSpace(medicalInfo["discharge"]["diagnosis"].ToString()))
                                    {
                                        await InsertDischargeDiagnosisAsync(claimid, medicalInfo["discharge"]["diagnosis"].ToString(), orgId);
                                    }


                                }
                            }
                            else
                            {
                                if (!string.IsNullOrWhiteSpace(medicalInfo["discharge"].ToString()))
                                {
                                    await InsertDischargeSummaryAsync(claimid, medicalInfo["discharge"].ToString(), encounterDetail.ToString(), orgId);
                                    if (!string.IsNullOrWhiteSpace(medicalInfo["discharge"]["diagnosis"].ToString()))
                                    {
                                        await InsertDischargeDiagnosisAsync(claimid, medicalInfo["discharge"]["diagnosis"].ToString(), orgId);
                                    }

                                }
                            }

                        }


                    }
                }
            }
            catch (Exception ex)
            {
                // logger.WriteError("InsertMedicalRecordAsync", ex.Message);
            }



        }
        private async Task<bool> IsDoctorChanged(int orgId, int facilityId, int previousDoctorId, string encounterDoctorId)
        {
            var doctorEntity = await context.RcmDoctors.Where(x => x.FacilityId == facilityId && x.OrganizationId == orgId
                                                && x.ExternalCode == encounterDoctorId
                                                && x.IsActive == true).FirstOrDefaultAsync();
            if (doctorEntity is null)
                return false;
            else
            {

                return previousDoctorId != doctorEntity.DoctorId;
            }
        }
        private async Task<int> RetriveDoctorID(int orgId, int facilityId, int previousDoctorId, string encounterDoctorId)
        {
            var doctorEntity = await context.RcmDoctors.Where(x => x.FacilityId == facilityId && x.OrganizationId == orgId
                                                && x.ExternalCode == encounterDoctorId
                                                && x.IsActive == true).FirstOrDefaultAsync();
            if (doctorEntity is null)
                return previousDoctorId;
            else
                return doctorEntity.DoctorId;

        }
        private async Task UpdateClaimDoctor(long claimid, int encounterDoctorId)
        {
            RcmClaim claimEntity = await context.RcmClaims.Where(c => c.ClaimId == claimid).FirstOrDefaultAsync();
            if (claimEntity != null)
            {
                claimEntity.DoctorId = encounterDoctorId;
                // claimEntity.ModifiedBy = 9999;
                claimEntity.ModifiedOn = System.DateTime.Now;
                await context.SaveChangesAsync();
            }
        }
        private static string Filters(string str)
        {
            string value = str.Replace("\r", " ")
                              .Replace("\n", " ")
                              .Replace("<", " ")
                              .Replace(">", " ")
                              .Replace("?", " ");

            return Regex.Replace((Regex.Replace(value, "[^0-9A-Za-z _-]", "")) ?? string.Empty, @"[^\w ]", " ");
        }
        private static double GetStringToDouble(string vitalParameter)
        {
            double vitalValue = 0;

            try
            {
                vitalValue = Convert.ToDouble(vitalParameter);
            }
            catch (FormatException)
            {
                vitalValue = 0;
            }

            return vitalValue;
        }
        private async Task InsertVitalSignAndChiefComplaintsAsync(int orgId, long claimid, string chief, string vital, bool IsRefetchMedicalInfo = false
                  , string treatmentPlan = "", string patientHistory = "", string physicalExamination = "")
        {
            try
            {
                JObject jChief = new JObject();
                JObject jVital = new JObject();
                JObject jTreatmentPlan = new JObject();
                JObject jPatientHistory = new JObject();
                JObject jPhysicalExam = new JObject();
                if (!string.IsNullOrWhiteSpace(chief))
                {
                    jChief = JObject.Parse(chief);
                }
                if (!string.IsNullOrWhiteSpace(vital))
                {
                    jVital = JObject.Parse(vital);
                }

                try
                {
                    if (!string.IsNullOrWhiteSpace(treatmentPlan))
                    {
                        jTreatmentPlan = JObject.Parse(treatmentPlan);
                    }
                }
                catch (Exception ex)
                {
                    //  logger.WriteError("treatmentPlan Json", ex.Message);
                }
                try
                {
                    if (!string.IsNullOrWhiteSpace(patientHistory))
                    {
                        jPatientHistory = JObject.Parse(patientHistory);
                    }
                }
                catch (Exception ex)
                {
                    //  logger.WriteError("patientHistory Json", ex.Message);
                }
                try
                {
                    if (!string.IsNullOrWhiteSpace(physicalExamination))
                    {
                        jPhysicalExam = JObject.Parse(physicalExamination);
                    }
                }
                catch (Exception ex)
                {
                    //  logger.WriteError("physicalExamination Json", ex.Message);
                }

                if (!context.RcmClaimVitalSigns.Any(x => x.OrganizationId == orgId && x.ClaimId == claimid))
                {

                    var rcmClaimVital = new RcmClaimVitalSign();
                    rcmClaimVital.OrganizationId = orgId;
                    rcmClaimVital.ClaimId = claimid;

                    if (!string.IsNullOrWhiteSpace(jChief["mainSymptoms"].ToString())
                        && !string.IsNullOrWhiteSpace(jChief["physicianNotesConditions"].ToString()))
                    {
                        StringBuilder systoms = new StringBuilder();
                        systoms.Append(Filters(jChief["mainSymptoms"].ToString()));
                        systoms.Append(Filters(jChief["physicianNotesConditions"].ToString()));
                        rcmClaimVital.MainSymptoms = systoms.ToString();

                    }
                    else if (!string.IsNullOrWhiteSpace(jChief["mainSymptoms"].ToString())
                         && string.IsNullOrWhiteSpace(jChief["physicianNotesConditions"].ToString()))
                    {
                        string condition = Filters(jChief["mainSymptoms"].ToString());

                        rcmClaimVital.MainSymptoms = condition;

                    }
                    else if (string.IsNullOrWhiteSpace(jChief["mainSymptoms"].ToString())
                       && !string.IsNullOrWhiteSpace(jChief["physicianNotesConditions"].ToString()))
                    {
                        string condition = Filters(jChief["physicianNotesConditions"].ToString());

                        rcmClaimVital.MainSymptoms = condition;

                    }

                    if (!string.IsNullOrWhiteSpace(jChief["significantSigns"].ToString()))
                        rcmClaimVital.SignificantSigns = Filters(jChief["significantSigns"].ToString());
                    else
                        rcmClaimVital.SignificantSigns = string.Empty;
                    if (!string.IsNullOrWhiteSpace(jVital["vitalSignCreatedon"].ToString()))
                        rcmClaimVital.VitalSignCreatedOn = Convert.ToDateTime(jVital["vitalSignCreatedon"].ToString());


                    if (!string.IsNullOrWhiteSpace(jVital["bloodPressureLower"].ToString()))
                        rcmClaimVital.BloodPressure = $"{jVital["bloodPressureLower"]}/{jVital["bloodPressureHigher"]}";
                    else
                        rcmClaimVital.BloodPressure = "115/75";

                    if (!string.IsNullOrWhiteSpace(jVital["pulse"].ToString()))
                        rcmClaimVital.Pulse = GetStringToDouble(jVital["pulse"].ToString());
                    else
                        rcmClaimVital.Pulse = 0;
                    if (!string.IsNullOrWhiteSpace(jVital["temperature"].ToString()))
                        rcmClaimVital.Temperature = GetStringToDouble(jVital["temperature"].ToString());
                    else
                        rcmClaimVital.Temperature = 0;

                    if (!string.IsNullOrWhiteSpace(jVital["height"].ToString()))
                        rcmClaimVital.Height = GetStringToDouble(jVital["height"].ToString());
                    else
                        rcmClaimVital.Height = 0;

                    if (!string.IsNullOrWhiteSpace(jVital["weight"].ToString()))
                        rcmClaimVital.Weight = GetStringToDouble(jVital["weight"].ToString());
                    else
                        rcmClaimVital.Weight = 0;
                    if (!string.IsNullOrWhiteSpace(jVital["bodyMassIndex"].ToString()))
                        rcmClaimVital.BodyMassIndex = GetStringToDouble(jVital["bodyMassIndex"].ToString());
                    else
                        rcmClaimVital.BodyMassIndex = 0;
                    if (jVital["respiratoryRate"] != null && jVital["respiratoryRate"].Type != JTokenType.Null
                        && !string.IsNullOrWhiteSpace(jVital["respiratoryRate"].ToString()))
                    {
                        rcmClaimVital.RespiratoryRate = jVital["respiratoryRate"].ToString();
                    }
                    if (!string.IsNullOrWhiteSpace(jVital["oxygenSaturation"].ToString()))
                    {
                        rcmClaimVital.OxygenSaturation = jVital["oxygenSaturation"].ToString();
                    }
                    if (jTreatmentPlan.HasValues && !string.IsNullOrWhiteSpace(jTreatmentPlan["treatment"].ToString()))
                    {
                        rcmClaimVital.TreatmentPlan = jTreatmentPlan["treatment"].ToString();
                    }
                    if (jPatientHistory.HasValues && !string.IsNullOrWhiteSpace(jPatientHistory["history"].ToString()))
                    {
                        rcmClaimVital.PatientHistory = jPatientHistory["history"].ToString();
                    }
                    if (jPhysicalExam.HasValues && !string.IsNullOrWhiteSpace(jPhysicalExam["examination"].ToString()))
                    {
                        rcmClaimVital.PhysicalExamination = jPhysicalExam["examination"].ToString();
                    }
                    if (jChief["mainSymptoms"].HasValues && !string.IsNullOrWhiteSpace(jChief["mainSymptoms"].ToString()))
                    {
                        rcmClaimVital.HistoryOfPresentIllness = jChief["mainSymptoms"].ToString();
                    }
                    rcmClaimVital.CreatedBy = 999;
                    rcmClaimVital.CreatedOn = System.DateTime.Now;

                    context.RcmClaimVitalSigns.Add(rcmClaimVital);
                    await context.SaveChangesAsync();
                }
                else // for time later on we have to check based on modified by and modifiedon
                {
                    var existingVitalEntity = await context.RcmClaimVitalSigns
                                  //.OrderByDescending(x => x.ClaimID)
                                  .FirstOrDefaultAsync(x => x.OrganizationId == orgId && x.ClaimId == claimid);
                    if (existingVitalEntity != null) // && (Convert.ToInt32(existingVitalEntity.ModifiedBy) == 0 || IsRefetchMedicalInfo)
                    {
                        if (!string.IsNullOrWhiteSpace(jChief["mainSymptoms"].ToString())
                         && !string.IsNullOrWhiteSpace(jChief["physicianNotesConditions"].ToString()))
                        {
                            StringBuilder systoms = new StringBuilder();
                            systoms.Append(Filters(jChief["mainSymptoms"].ToString()));
                            systoms.Append(Filters(jChief["physicianNotesConditions"].ToString()));
                            existingVitalEntity.MainSymptoms = systoms.ToString();

                        }
                        else if (!string.IsNullOrWhiteSpace(jChief["mainSymptoms"].ToString())
                             && string.IsNullOrWhiteSpace(jChief["physicianNotesConditions"].ToString()))
                        {
                            string condition = Filters(jChief["mainSymptoms"].ToString());

                            existingVitalEntity.MainSymptoms = condition;

                        }
                        else if (string.IsNullOrWhiteSpace(jChief["mainSymptoms"].ToString())
                           && !string.IsNullOrWhiteSpace(jChief["physicianNotesConditions"].ToString()))
                        {
                            string condition = Filters(jChief["physicianNotesConditions"].ToString());

                            existingVitalEntity.MainSymptoms = condition;

                        }

                        if (!string.IsNullOrWhiteSpace(jChief["significantSigns"].ToString()))
                            existingVitalEntity.SignificantSigns = Filters(jChief["significantSigns"].ToString());

                        if (!string.IsNullOrWhiteSpace(jVital["vitalSignCreatedon"].ToString()))
                            existingVitalEntity.VitalSignCreatedOn = Convert.ToDateTime(jVital["vitalSignCreatedon"].ToString());

                        if (!string.IsNullOrWhiteSpace(jVital["bloodPressureLower"].ToString()))
                            existingVitalEntity.BloodPressure = $"{jVital["bloodPressureLower"]}/{jVital["bloodPressureHigher"]}";


                        if (!string.IsNullOrWhiteSpace(jVital["pulse"].ToString()))
                            existingVitalEntity.Pulse = GetStringToDouble(jVital["pulse"].ToString());

                        if (!string.IsNullOrWhiteSpace(jVital["temperature"].ToString()))
                            existingVitalEntity.Temperature = GetStringToDouble(jVital["temperature"].ToString());

                        if (!string.IsNullOrWhiteSpace(jVital["height"].ToString()))
                            existingVitalEntity.Height = GetStringToDouble(jVital["height"].ToString());

                        if (!string.IsNullOrWhiteSpace(jVital["weight"].ToString()))
                            existingVitalEntity.Weight = GetStringToDouble(jVital["weight"].ToString());

                        if (!string.IsNullOrWhiteSpace(jVital["bodyMassIndex"].ToString()))
                            existingVitalEntity.BodyMassIndex = GetStringToDouble(jVital["bodyMassIndex"].ToString());
                        if (!string.IsNullOrWhiteSpace(jVital["respiratoryRate"].ToString()))
                        {
                            existingVitalEntity.RespiratoryRate = jVital["respiratoryRate"].ToString();
                        }
                        if (!string.IsNullOrWhiteSpace(jVital["oxygenSaturation"].ToString()))
                        {
                            existingVitalEntity.OxygenSaturation = jVital["oxygenSaturation"].ToString();
                        }
                        if (jTreatmentPlan.HasValues && !string.IsNullOrWhiteSpace(jTreatmentPlan["treatment"].ToString()))
                        {
                            existingVitalEntity.TreatmentPlan = jTreatmentPlan["treatment"].ToString();
                        }
                        if (jPatientHistory.HasValues && !string.IsNullOrWhiteSpace(jPatientHistory["history"].ToString()))
                        {
                            existingVitalEntity.PatientHistory = jPatientHistory["history"].ToString();
                        }
                        if (jPhysicalExam.HasValues && !string.IsNullOrWhiteSpace(jPhysicalExam["examination"].ToString()))
                        {
                            existingVitalEntity.PhysicalExamination = jPhysicalExam["examination"].ToString();
                        }
                        if (jChief["mainSymptoms"].HasValues && !string.IsNullOrWhiteSpace(jChief["mainSymptoms"].ToString()))
                        {
                            existingVitalEntity.HistoryOfPresentIllness = jChief["mainSymptoms"].ToString();
                        }
                        await context.SaveChangesAsync();

                    }

                }



            }
            catch (Exception ex)
            {

                // logger.WriteError($"InsertVitalSignAndChiefComplaintsAsync - {claimid}", ex.Message);
            }


        }

        private async Task RemoveDiagnosis(long claimId, int orgId)
        {
            var listOfDignosis = await context.RcmClaimDiagnoses.Where(x => x.OrganizationId == orgId
            && x.ClaimId == claimId).ToListAsync();
            if (listOfDignosis != null && listOfDignosis.Count() > 0)
            {
                context.RcmClaimDiagnoses.RemoveRange(listOfDignosis);
                await context.SaveChangesAsync();
            }
        }
        private string RetriveDiagnosisDescription(string icdCode)
        {

            var codeDescription = context.RcmDiagnoses.AsNoTracking().Where(code => code.DiagnosisCode.Equals(icdCode))
                .Select(x => x.DiagnosisDescription)
                .FirstOrDefault();
            if (codeDescription == null)
                return "";
            return codeDescription;


        }
        private async Task InsertApprovalDiagnosisAsyn(string approvalDiagnosis, long claimId, int orgId)
        {
            try
            {
                JArray jdiagnosis = JArray.Parse(approvalDiagnosis) as JArray;
                if (jdiagnosis != null && jdiagnosis.Count > 0)
                {

                    #region Remove existing
                    await RemoveDiagnosis(claimId, orgId);
                    #endregion

                    //JArray jdiagnosis = JArray.Parse(approvalDiagnosis) as JArray;

                    short i = 1;
                    var generateLineItem = (await context.RcmClaimDiagnoses
                        .Where(t => t.ClaimId == claimId && t.OrganizationId == orgId && t.IsActive == true)
                                                                    .AsNoTracking().ToListAsync());
                    if (generateLineItem == null || generateLineItem.Count() == 0)
                        i = 1;
                    else
                        i = Convert.ToInt16((generateLineItem.Max(x => x.LineitemNo)) + 1);
                    int index = 1;
                    bool isDiagnosisHasValue = false;
                    foreach (var item in jdiagnosis)
                    {
                        var rcmClaimDiagnos = new RcmClaimDiagnosis();
                        rcmClaimDiagnos.OrganizationId = orgId;
                        rcmClaimDiagnos.ClaimId = claimId;
                        rcmClaimDiagnos.LineitemNo = i;
                        rcmClaimDiagnos.OrderBy = index;

                        if (item["type"]?.ToString().Contains("1") == true)
                        {
                            rcmClaimDiagnos.DiagnosisType = 1;
                        }
                        else
                        {
                            rcmClaimDiagnos.DiagnosisType = 2;
                        }

                        if (!string.IsNullOrWhiteSpace(item["code"].ToString()))
                        {
                            rcmClaimDiagnos.DiagnosisCode = item["code"].ToString();
                            rcmClaimDiagnos.DiagnoisInfoCode = item["code"].ToString();
                        }

                        if (!string.IsNullOrWhiteSpace(item["type"].ToString()))
                        {
                            rcmClaimDiagnos.DiagnosisCodeType = Convert.ToByte(item["type"].ToString());
                            rcmClaimDiagnos.DiagnoisInfoType = item["type"].ToString();
                        }
                        if (!string.IsNullOrWhiteSpace(item["code"].ToString()))
                            rcmClaimDiagnos.DiagnosisDescription = RetriveDiagnosisDescription(item["code"].ToString());

                        rcmClaimDiagnos.IsActive = true;
                        rcmClaimDiagnos.Isdischarge = false;
                        rcmClaimDiagnos.CreatedBy = 999;
                        rcmClaimDiagnos.CreatedOn = DateTime.Now;
                        context.RcmClaimDiagnoses.Add(rcmClaimDiagnos);
                        isDiagnosisHasValue = true;
                        i++;
                        index++;
                    }
                    if (isDiagnosisHasValue)
                    {
                        await context.SaveChangesAsync();
                    }


                }
            }
            catch (Exception ex)
            {

                //  logger.WriteError("Approval Diagnosis", ex.Message);
            }
        }
        public async Task InsertDubaiDiagnosisAsync(long claimId, string diagnosis, int orgId)
        {
            try
            {
                JArray jdiagnosis = JArray.Parse(diagnosis) as JArray;
                if (jdiagnosis != null && jdiagnosis.Count > 0)
                {


                    #region Remove existing
                    await RemoveDiagnosis(claimId, orgId);
                    #endregion

                    List<RcmClaimDiagnosis> rcmClaimDiagnoses = new List<RcmClaimDiagnosis>();
                    short i = 1;
                    var generateLineItem = (await context.RcmClaimDiagnoses
                        .Where(t => t.ClaimId == claimId && t.OrganizationId == orgId && t.IsActive == true)
                                                                    .AsNoTracking().ToListAsync());
                    if (generateLineItem == null || generateLineItem.Count() == 0)
                        i = 1;
                    else
                        i = Convert.ToInt16((generateLineItem.Max(x => x.LineitemNo)) + 1);

                    int index = 1;
                    bool isDiagnosisHasValue = false;
                    bool isPrimaryDiagnosis = false;

                    bool containPrimary = jdiagnosis.Any(d => d["diagnosisType"]?.Value<int>() == 1);
                    if (!containPrimary)
                    {
                        jdiagnosis[0]["diagnosisType"] = 1;
                    }


                    foreach (JObject item in jdiagnosis)
                    {
                        var rcmClaimDiagnos = new RcmClaimDiagnosis();
                        rcmClaimDiagnos.OrganizationId = orgId;
                        rcmClaimDiagnos.ClaimId = claimId;
                        rcmClaimDiagnos.LineitemNo = i;


                        if (item["diagnosisType"]?.ToString().Contains("1") == true
                            && isPrimaryDiagnosis == false)
                        {
                            rcmClaimDiagnos.DiagnosisType = 1;
                            isPrimaryDiagnosis = true;
                            rcmClaimDiagnos.OrderBy = 1;
                        }
                        else
                        {
                            rcmClaimDiagnos.DiagnosisType = 2;
                            rcmClaimDiagnos.OrderBy = index;
                        }

                        if (!string.IsNullOrWhiteSpace(item["code"].ToString()))
                            rcmClaimDiagnos.DiagnosisCode = item["code"].ToString();
                        if (!string.IsNullOrWhiteSpace(item["codeType"].ToString()))
                        {
                            rcmClaimDiagnos.DiagnosisCodeType = Convert.ToByte(item["codeType"].ToString());
                        }
                        if (!string.IsNullOrWhiteSpace(item["description"].ToString()))
                            rcmClaimDiagnos.DiagnosisDescription = item["description"].ToString();
                        if (!string.IsNullOrWhiteSpace(item["diagnoisInfoType"].ToString()))
                            rcmClaimDiagnos.DiagnoisInfoType = item["diagnoisInfoType"].ToString();
                        if (!string.IsNullOrWhiteSpace(item["diagnoisInfoCode"].ToString()))
                            rcmClaimDiagnos.DiagnoisInfoCode = item["diagnoisInfoCode"].ToString();
                        if (!string.IsNullOrWhiteSpace(item["morphologyCode"].ToString()))
                            rcmClaimDiagnos.MorphologyCode = item["morphologyCode"].ToString();
                        if (!string.IsNullOrWhiteSpace(item["conditionOnset"].ToString()))
                            rcmClaimDiagnos.ConditionOnset = item["conditionOnset"].ToString();

                        rcmClaimDiagnos.IsActive = true;
                        rcmClaimDiagnos.Isdischarge = false;
                        rcmClaimDiagnos.CreatedBy = 999;
                        rcmClaimDiagnos.CreatedOn = DateTime.Now;
                        context.RcmClaimDiagnoses.Add(rcmClaimDiagnos);
                        isDiagnosisHasValue = true;
                        i++;
                        index++;
                    }

                    if (isDiagnosisHasValue)
                    {

                        await context.SaveChangesAsync();
                    }




                }

            }
            catch (Exception ex)
            {

                //  logger.WriteError("InsertDiagnosisAsync", ex.Message);
            }
        }

        public async Task InsertKSADiagnosisAsync(long claimId, string diagnosis, int orgId)
        {
            try
            {

                if ((!string.IsNullOrWhiteSpace(diagnosis) && !string.IsNullOrWhiteSpace(diagnosis)
                    && !diagnosis.Equals("[]"))
                               )
                {

                    #region Remove existing
                    await RemoveDiagnosis(claimId, orgId);
                    #endregion


                    JArray jdiagnosis = JArray.Parse(diagnosis) as JArray;
                    List<RcmClaimDiagnosis> rcmClaimDiagnoses = new List<RcmClaimDiagnosis>();
                    short i = 1;
                    var generateLineItem = (await context.RcmClaimDiagnoses
                        .Where(t => t.ClaimId == claimId && t.OrganizationId == orgId && t.IsActive == true)
                                                                    .AsNoTracking().ToListAsync());
                    if (generateLineItem == null || generateLineItem.Count() == 0)
                        i = 1;
                    else
                        i = Convert.ToInt16((generateLineItem.Max(x => x.LineitemNo)) + 1);
                    int index = 1;
                    bool isDiagnosisHasValue = false;
                    foreach (JObject item in jdiagnosis)
                    {
                        var rcmClaimDiagnos = new RcmClaimDiagnosis();
                        rcmClaimDiagnos.OrganizationId = orgId;
                        rcmClaimDiagnos.ClaimId = claimId;
                        rcmClaimDiagnos.LineitemNo = i;
                        rcmClaimDiagnos.OrderBy = index;

                        if (item["diagnosisType"]?.ToString().Contains("1") == true)
                        {
                            rcmClaimDiagnos.DiagnosisType = 1;
                        }
                        else
                        {
                            rcmClaimDiagnos.DiagnosisType = 2;
                        }

                        if (!string.IsNullOrWhiteSpace(item["code"].ToString()))
                            rcmClaimDiagnos.DiagnosisCode = item["code"].ToString();
                        if (!string.IsNullOrWhiteSpace(item["codeType"].ToString()))
                        {
                            rcmClaimDiagnos.DiagnosisCodeType = Convert.ToByte(item["codeType"].ToString());
                        }
                        if (!string.IsNullOrWhiteSpace(item["description"].ToString()))
                            rcmClaimDiagnos.DiagnosisDescription = item["description"].ToString();
                        if (!string.IsNullOrWhiteSpace(item["diagnoisInfoType"].ToString()))
                            rcmClaimDiagnos.DiagnoisInfoType = item["diagnoisInfoType"].ToString();
                        if (!string.IsNullOrWhiteSpace(item["diagnoisInfoCode"].ToString()))
                            rcmClaimDiagnos.DiagnoisInfoCode = item["diagnoisInfoCode"].ToString();
                        if (!string.IsNullOrWhiteSpace(item["morphologyCode"].ToString()))
                            rcmClaimDiagnos.MorphologyCode = item["morphologyCode"].ToString();
                        if (!string.IsNullOrWhiteSpace(item["conditionOnset"].ToString()))
                            rcmClaimDiagnos.ConditionOnset = item["conditionOnset"].ToString();
                        rcmClaimDiagnos.IsActive = true;
                        rcmClaimDiagnos.Isdischarge = false;
                        rcmClaimDiagnos.CreatedBy = 999;
                        rcmClaimDiagnos.CreatedOn = DateTime.Now;
                        context.RcmClaimDiagnoses.Add(rcmClaimDiagnos);
                        isDiagnosisHasValue = true;
                        i++;
                        index++;
                    }

                    if (isDiagnosisHasValue)
                    {

                        await context.SaveChangesAsync();
                    }




                }

            }
            catch (Exception ex)
            {


            }
        }

        public async Task InsertDischargeSummaryAsync(long claimId, string dischargeSummary, string encounterDetail, int orgId)
        {
            try
            {
                if (!string.IsNullOrWhiteSpace(dischargeSummary))
                {
                    JObject JdischargeSummary = JObject.Parse(dischargeSummary);
                    JObject JencounterDetail = JObject.Parse(encounterDetail);
                    if (!string.IsNullOrWhiteSpace(JdischargeSummary["dischargeNo"].ToString()))
                    {

                        // if (!_dbContext.RCM_ClaimDischargeSummary.Any(x => x.OrganizationID == organizationId
                        //&& x.ClaimID == claimId
                        //&& x.CreatedBy == 999))
                        //     return;

                        if (!context.RcmClaimDischargeSummaries.Any(x => x.OrganizationId == orgId && x.ClaimId == claimId))
                        {
                            #region Discharge Summary Fields Mapping
                            var rcmClaimDischargeSummary = new RcmClaimDischargeSummary();
                            rcmClaimDischargeSummary.OrganizationId = orgId;
                            rcmClaimDischargeSummary.ClaimId = claimId;
                            rcmClaimDischargeSummary.IsActive = true;

                            if (!string.IsNullOrWhiteSpace(JencounterDetail["encounterNo"].ToString()))
                                rcmClaimDischargeSummary.EncounterNo = Convert.ToInt32(JencounterDetail["encounterNo"].ToString());
                            if (!string.IsNullOrWhiteSpace(JdischargeSummary["dischargeNo"].ToString()))
                                rcmClaimDischargeSummary.DischargeNo = Convert.ToInt32(JdischargeSummary["dischargeNo"].ToString());
                            if (!string.IsNullOrWhiteSpace(JdischargeSummary["dischargeDate"].ToString()))
                                rcmClaimDischargeSummary.DischargeDate = Convert.ToDateTime(JdischargeSummary["dischargeDate"].ToString());

                            if (!string.IsNullOrWhiteSpace(JencounterDetail["encounterDate"].ToString()))
                                rcmClaimDischargeSummary.AdmissionDate = Convert.ToDateTime(JencounterDetail["encounterDate"].ToString());

                            if (!string.IsNullOrWhiteSpace(JdischargeSummary["clinicId"].ToString()))
                                rcmClaimDischargeSummary.ClinicId = Convert.ToInt16(JdischargeSummary["clinicId"].ToString());

                            if (!string.IsNullOrWhiteSpace(JdischargeSummary["doctorId"].ToString()))
                            {
                                rcmClaimDischargeSummary.DoctorId = Convert.ToInt32(JdischargeSummary["doctorId"].ToString());
                                rcmClaimDischargeSummary.CreatedBy = 999;//rcmClaimDischargeSummary.DoctorId;
                                rcmClaimDischargeSummary.CreatedOn = Convert.ToDateTime(JdischargeSummary["createdOn"].ToString());
                            }

                            if (!string.IsNullOrWhiteSpace(JdischargeSummary["persentation"].ToString()))
                                rcmClaimDischargeSummary.Persentation = JdischargeSummary["persentation"].ToString();
                            else
                                rcmClaimDischargeSummary.Persentation = string.Empty;

                            if (!string.IsNullOrWhiteSpace(JdischargeSummary["pastHistory"].ToString()))
                                rcmClaimDischargeSummary.PastHistory = JdischargeSummary["pastHistory"].ToString();
                            else
                                rcmClaimDischargeSummary.PastHistory = string.Empty;

                            if (!string.IsNullOrWhiteSpace(JdischargeSummary["planOfCare"].ToString()))
                                rcmClaimDischargeSummary.PlanOfCare = JdischargeSummary["planOfCare"].ToString();
                            else
                                rcmClaimDischargeSummary.PlanOfCare = string.Empty;

                            if (!string.IsNullOrWhiteSpace(JdischargeSummary["investigations"].ToString()))
                                rcmClaimDischargeSummary.Investigations = JdischargeSummary["investigations"].ToString();
                            else
                                rcmClaimDischargeSummary.Investigations = string.Empty;

                            if (!string.IsNullOrWhiteSpace(JdischargeSummary["followupPlan"].ToString()))
                                rcmClaimDischargeSummary.FollowupPlan = JdischargeSummary["followupPlan"].ToString();
                            else
                                rcmClaimDischargeSummary.FollowupPlan = string.Empty;

                            if (!string.IsNullOrWhiteSpace(JdischargeSummary["planedProcedure"].ToString()))
                                rcmClaimDischargeSummary.PlanedProcedure = JdischargeSummary["planedProcedure"].ToString();
                            else
                                rcmClaimDischargeSummary.PlanedProcedure = string.Empty;


                            if (!string.IsNullOrWhiteSpace(JdischargeSummary["conditionOnDischarge"].ToString()))
                                rcmClaimDischargeSummary.ConditionOnDischarge = JdischargeSummary["conditionOnDischarge"].ToString();
                            else
                                rcmClaimDischargeSummary.ConditionOnDischarge = string.Empty;


                            if (!string.IsNullOrWhiteSpace(JdischargeSummary["significantFindings"].ToString()))
                                rcmClaimDischargeSummary.SignificantFindings = JdischargeSummary["significantFindings"].ToString();
                            else
                                rcmClaimDischargeSummary.SignificantFindings = string.Empty;

                            if (!string.IsNullOrWhiteSpace(JdischargeSummary["patientCodition"].ToString()))
                                rcmClaimDischargeSummary.PatientCondition = JdischargeSummary["patientCodition"].ToString();
                            else
                                rcmClaimDischargeSummary.PatientCondition = string.Empty;

                            if (!string.IsNullOrWhiteSpace(JdischargeSummary["daysStayed"].ToString()))
                                rcmClaimDischargeSummary.DaysStayed = Convert.ToInt32(JdischargeSummary["daysStayed"].ToString());

                            if (!string.IsNullOrWhiteSpace(JdischargeSummary["remarks"].ToString()))
                                rcmClaimDischargeSummary.Remarks = JdischargeSummary["remarks"].ToString();
                            else
                                rcmClaimDischargeSummary.Remarks = string.Empty;

                            if (!string.IsNullOrWhiteSpace(JdischargeSummary["erCare"].ToString()))
                                rcmClaimDischargeSummary.Ercare = JdischargeSummary["erCare"].ToString();
                            else
                                rcmClaimDischargeSummary.Ercare = string.Empty;

                            if (!string.IsNullOrWhiteSpace(JdischargeSummary["roomID"].ToString()))
                                rcmClaimDischargeSummary.RoomId = JdischargeSummary["roomID"].ToString();
                            else
                                rcmClaimDischargeSummary.RoomId = string.Empty;

                            if (!string.IsNullOrWhiteSpace(JdischargeSummary["bedID"].ToString()))
                                rcmClaimDischargeSummary.RoomId = JdischargeSummary["bedID"].ToString();
                            else
                                rcmClaimDischargeSummary.RoomId = string.Empty;

                            if (!string.IsNullOrWhiteSpace(JdischargeSummary["clinicName"].ToString()))
                                rcmClaimDischargeSummary.ClinicName = JdischargeSummary["clinicName"].ToString();
                            else
                                rcmClaimDischargeSummary.ClinicName = string.Empty;

                            if (!string.IsNullOrWhiteSpace(JdischargeSummary["doctorName"].ToString()))
                                rcmClaimDischargeSummary.DoctorName = JdischargeSummary["doctorName"].ToString();
                            else
                                rcmClaimDischargeSummary.DoctorName = string.Empty;
                            #endregion

                            if (JdischargeSummary["dischargeDisposition"] != null)
                                rcmClaimDischargeSummary.DischargeDisposition = (byte)JdischargeSummary["dischargeDisposition"];
                            else
                                rcmClaimDischargeSummary.DischargeDisposition = 0;
                            rcmClaimDischargeSummary.FinalDiagnosis = string.Empty;

                            #region Update discharge date from the summary

                            if (rcmClaimDischargeSummary.DischargeDate != null
                                          && rcmClaimDischargeSummary.DischargeDate != DateTime.MinValue)
                            {
                                //  await UpdateDischargeDate(claimId, rcmClaimDischargeSummary.DischargeDate);
                                //_dbContext.RCM_Claim.Update(claim);
                            }
                            else
                            {
                                //  await UpdateDischargeDateByServiceMaxDate(claimId);
                            }

                            #endregion

                            context.RcmClaimDischargeSummaries.Add(rcmClaimDischargeSummary);

                        }
                        else
                        {
                            var existingDischargeSummary = await context.RcmClaimDischargeSummaries
                                .FirstOrDefaultAsync(x => x.OrganizationId == orgId && x.ClaimId == claimId && x.IsActive == true);
                            //if data is updated by the user , then don't replace it .
                            if (existingDischargeSummary != null)
                            {
                                if (!string.IsNullOrWhiteSpace(JdischargeSummary["dischargeDate"].ToString()))
                                    existingDischargeSummary.DischargeDate = Convert.ToDateTime(JdischargeSummary["dischargeDate"].ToString());

                                if (!string.IsNullOrWhiteSpace(JencounterDetail["encounterDate"].ToString()))
                                    existingDischargeSummary.AdmissionDate = Convert.ToDateTime(JencounterDetail["encounterDate"].ToString());

                                if (!string.IsNullOrWhiteSpace(JdischargeSummary["clinicId"].ToString()))
                                    existingDischargeSummary.ClinicId = Convert.ToInt16(JdischargeSummary["clinicId"].ToString());

                                if (!string.IsNullOrWhiteSpace(JdischargeSummary["doctorId"].ToString()))
                                {
                                    existingDischargeSummary.DoctorId = Convert.ToInt32(JdischargeSummary["doctorId"].ToString());

                                }

                                if (!string.IsNullOrWhiteSpace(JdischargeSummary["persentation"].ToString()))
                                    existingDischargeSummary.Persentation = JdischargeSummary["persentation"].ToString();


                                if (!string.IsNullOrWhiteSpace(JdischargeSummary["pastHistory"].ToString()))
                                    existingDischargeSummary.PastHistory = JdischargeSummary["pastHistory"].ToString();


                                if (!string.IsNullOrWhiteSpace(JdischargeSummary["planOfCare"].ToString()))
                                    existingDischargeSummary.PlanOfCare = JdischargeSummary["planOfCare"].ToString();

                                if (!string.IsNullOrWhiteSpace(JdischargeSummary["investigations"].ToString()))
                                    existingDischargeSummary.Investigations = JdischargeSummary["investigations"].ToString();


                                if (!string.IsNullOrWhiteSpace(JdischargeSummary["followupPlan"].ToString()))
                                    existingDischargeSummary.FollowupPlan = JdischargeSummary["followupPlan"].ToString();

                                if (!string.IsNullOrWhiteSpace(JdischargeSummary["planedProcedure"].ToString()))
                                    existingDischargeSummary.PlanedProcedure = JdischargeSummary["planedProcedure"].ToString();

                                if (!string.IsNullOrWhiteSpace(JdischargeSummary["conditionOnDischarge"].ToString()))
                                    existingDischargeSummary.ConditionOnDischarge = JdischargeSummary["conditionOnDischarge"].ToString();

                                if (!string.IsNullOrWhiteSpace(JdischargeSummary["significantFindings"].ToString()))
                                    existingDischargeSummary.SignificantFindings = JdischargeSummary["significantFindings"].ToString();


                                if (!string.IsNullOrWhiteSpace(JdischargeSummary["patientCodition"].ToString()))
                                    existingDischargeSummary.PatientCondition = JdischargeSummary["patientCodition"].ToString();


                                if (!string.IsNullOrWhiteSpace(JdischargeSummary["daysStayed"].ToString()))
                                    existingDischargeSummary.DaysStayed = Convert.ToInt32(JdischargeSummary["daysStayed"].ToString());

                                if (!string.IsNullOrWhiteSpace(JdischargeSummary["remarks"].ToString()))
                                    existingDischargeSummary.Remarks = JdischargeSummary["remarks"].ToString();


                                if (!string.IsNullOrWhiteSpace(JdischargeSummary["erCare"].ToString()))
                                    existingDischargeSummary.Ercare = JdischargeSummary["erCare"].ToString();


                                if (!string.IsNullOrWhiteSpace(JdischargeSummary["roomID"].ToString()))
                                    existingDischargeSummary.RoomId = JdischargeSummary["roomID"].ToString();


                                if (!string.IsNullOrWhiteSpace(JdischargeSummary["bedID"].ToString()))
                                    existingDischargeSummary.BedId = JdischargeSummary["bedID"].ToString();


                                if (!string.IsNullOrWhiteSpace(JdischargeSummary["clinicName"].ToString()))
                                    existingDischargeSummary.ClinicName = JdischargeSummary["clinicName"].ToString();


                                if (!string.IsNullOrWhiteSpace(JdischargeSummary["doctorName"].ToString()))
                                    existingDischargeSummary.DoctorName = JdischargeSummary["doctorName"].ToString();
                                if (JdischargeSummary["dischargeDisposition"] != null)
                                    existingDischargeSummary.DischargeDisposition = (byte)JdischargeSummary["dischargeDisposition"];
                                else
                                    existingDischargeSummary.DischargeDisposition = 0;
                                #region Update discharge date from the summary


                                if (existingDischargeSummary.DischargeDate != null
                                              && existingDischargeSummary.DischargeDate != DateTime.MinValue)
                                {
                                    // await UpdateDischargeDate(claimId, existingDischargeSummary.DischargeDate);
                                    //_dbContext.RCM_Claim.Update(claim);
                                }
                                else
                                {
                                    //  await UpdateDischargeDateByServiceMaxDate(claimId);
                                }
                                #endregion

                            }
                        }
                        await context.SaveChangesAsync();

                    }
                    else
                    {
                        //   await UpdateDischargeDateByServiceMaxDate(claimId);
                    }

                }
                else
                {
                    //  await UpdateDischargeDateByServiceMaxDate(claimId);
                }
            }
            catch (Exception ex)
            {

                // logger.WriteError("InsertDischargeSummaryAsync", ex.Message);
            }


        }

        private async Task InsertDischargeDiagnosisAsync(long claimId, string diagnosis, int orgId)
        {

            try
            {
                if ((!string.IsNullOrWhiteSpace(diagnosis))
                       )
                {
                    await RemoveDiagnosis(claimId, orgId);

                    JArray jdiagnosis = JArray.Parse(diagnosis) as JArray;
                    List<RcmClaimDiagnosis> rcmClaimDiagnoses = new List<RcmClaimDiagnosis>();
                    short i = 1;
                    var generateLineItem = (await context.RcmClaimDiagnoses.Where(t => t.ClaimId == claimId
                        && t.OrganizationId == orgId).AsNoTracking().ToListAsync());
                    if (generateLineItem == null || generateLineItem.Count() == 0)
                        i = 1;
                    else
                        i = Convert.ToInt16((generateLineItem.Max(x => x.LineitemNo)) + 1);

                    bool asValue = false;
                    int orderBy = 1;
                    foreach (JObject item in jdiagnosis)
                    {
                        var incomingDiagnosis = new RcmClaimDiagnosis();
                        incomingDiagnosis.OrganizationId = orgId;
                        incomingDiagnosis.ClaimId = claimId;
                        incomingDiagnosis.LineitemNo = i;
                        incomingDiagnosis.OrderBy = Convert.ToInt32(incomingDiagnosis.LineitemNo);
                        if (orgId == 1)
                        {
                            if (!string.IsNullOrWhiteSpace(item["diagnosisType"].ToString()))
                            {
                                if (item["diagnosisType"].ToString().ToLower().Contains("1")
                                    )
                                {
                                    incomingDiagnosis.DiagnosisType = 1;
                                    incomingDiagnosis.OrderBy = orderBy;

                                }
                                else
                                {
                                    incomingDiagnosis.DiagnosisType = 2;
                                    incomingDiagnosis.OrderBy = orderBy;
                                }
                            }
                            else
                                incomingDiagnosis.DiagnosisType = 1;
                        }
                        else if (orgId != 1)
                        {
                            if (!string.IsNullOrWhiteSpace(item["type"].ToString()))
                            {
                                if (item["type"].ToString().ToLower().Contains("1"))
                                {
                                    incomingDiagnosis.DiagnosisType = 1;
                                    incomingDiagnosis.OrderBy = orderBy;
                                }
                                else
                                {
                                    incomingDiagnosis.DiagnosisType = 2;
                                    incomingDiagnosis.OrderBy = orderBy;
                                }
                            }
                            else
                                incomingDiagnosis.DiagnosisType = 1;
                        }

                        if (!string.IsNullOrWhiteSpace(item["code"].ToString()))
                            incomingDiagnosis.DiagnosisCode = item["code"].ToString();
                        if (!string.IsNullOrWhiteSpace(item["description"].ToString()))
                            incomingDiagnosis.DiagnosisDescription = item["description"].ToString();
                        if (!string.IsNullOrWhiteSpace(item["diagnosisType"].ToString()))
                            incomingDiagnosis.DiagnoisInfoType = item["diagnosisType"].ToString();
                        if (!string.IsNullOrWhiteSpace(item["code"].ToString()))
                            incomingDiagnosis.DiagnoisInfoCode = item["code"].ToString();
                        if (!string.IsNullOrWhiteSpace(item["conditionOnset"].ToString()))
                            incomingDiagnosis.ConditionOnset = item["conditionOnset"].ToString();
                        if (!string.IsNullOrWhiteSpace(item["morphologyCode"].ToString()))
                            incomingDiagnosis.MorphologyCode = item["morphologyCode"].ToString();
                        incomingDiagnosis.IsActive = true;
                        incomingDiagnosis.CreatedBy = 999;
                        incomingDiagnosis.CreatedOn = DateTime.Now;
                        /// byte isPrimary = (byte)DiagnosisType.Primary;
                        if (!string.IsNullOrWhiteSpace(item["dischargeNo"].ToString()) && item["dischargeNo"].ToString().Length > 0)
                            incomingDiagnosis.Isdischarge = true;
                        // check incoming diagnosis exists?

                        // case existing primary with different diagnosis code and new primary incoming
                        context.RcmClaimDiagnoses.Add(incomingDiagnosis);
                        orderBy++;
                        asValue = true;

                    }
                    if (asValue)
                    {
                        await context.SaveChangesAsync();
                    }

                }


            }
            catch (Exception ex)
            {

                // logger.WriteError("InsertDischargeDiagnosisAsync", ex.Message);
            }
        }
    }
    public enum EncounterType : byte
    {
        OutPatient = 1,
        InPatient = 2,
        Referral = 3

    }
    public enum ClientClusters
    {
        KSA = 1,
        DUBAI = 2,
        MOH = 3,
        ASEER = 4
    }
}
