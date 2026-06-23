using Cyclus.Storage.Attachment.Enums;
using Nphies.Core.DTOs;
using System;
using System.Collections.Generic;

namespace Nphies.Core.Models
{
    public class ClaimResponseModel
    {
        public string Remarks { get; set; }
        public string ClaimIdentifier { get; set; }
        public string Status { get; set; }
        public Object ResponseBundle { get; set; }
        public string BundleID { get; set; }
    }
    public class ClaimRequestModel
    {
        public ClaimRequestModel()
        {
            ClaimDetails = new List<ClaimDetail>();
            ClaimBundle = new List<string>();
        }
        public string ClaimbatchID { get; set; }
        public string ClaimBundleIdentifier { get; set; }
        public int ApplicationType { get; set; }
        public List<ClaimDetail> ClaimDetails { get; set; }
        public List<string> ClaimBundle { get; set; }
        public string Createdon { get; set; }
    }
    public class BatchResponse
    {
        public List<ClaimDetailResponse> ClaimDetailResponse { get; set; }
    }
    public class ClaimDetailResponse
    {
        public string Remarks { get; set; }
        public string ClaimIdentifier { get; set; }
        public string Status { get; set; }
        public string TotalApproved { get; set; }
        public Object ResponseBundle { get; set; }
        public string BundleID { get; set; }
        public bool IsPended { get; set; }
        public List<ClaimItemDetail> ClaimItemDetail { get; set; }
    }

    public class ClaimItemDetail
    {
        public string ErrorCode { get; set; }
        public string Code { get; set; }
        public string Display { get; set; }
        public string System { get; set; }
        public string Error { get; set; }
        public int? Sequence { get; set; }
        public string Status { get; set; }
        public string InvoiceNo { get; set; }       
        public string ApprovedQuantity { get; set; }
        public string BenefitAmount { get; set; }
    }

    public class Patient
    {
        public string PatientID { get; set; }
        public string PatientIdentificationType { get; set; }
        public string PatientIdentificationDescription { get; set; }
        public string PatientIdentificationNo { get; set; }
        public string STATUS { get; set; }
        public string FirstName { get; set; }
        public string MiddleName { get; set; }
        public string LastName { get; set; }
        public string FirstNameAr { get; set; }
        public string MiddleNameAr { get; set; }
        public string LastNameAr { get; set; }
        public string PhoneResi { get; set; }
        public string PhoneOffice { get; set; }
        public string MobileNumber { get; set; }
        public string EmailAddress { get; set; }
        public string DateofBirth { get; set; }
        public string Gender { get; set; }
        public string POBox { get; set; }
        public string ZipCode { get; set; }
        public string Address { get; set; }
        public string ISOCountryID { get; set; }
        public string MaritalStatus { get; set; }
        public int PatientOccuption { get; set; }
        public string PolicyHolderNo { get; set; }
        public string PolicyHolderName { get; set; }
        public string CauseOfDeath { get; set; }
    }

    public class ClaimDetail
    {
        public int SequenceNo { get; set; }
        public string ApprovalNo { get; set; }
        public string ClaimID { get; set; } //Combination of Invoiceid and Project ID (in case of Pharmacy combination of Invoiceid,Projectid and Location ID)
        public string ClaimNo { get; set; }// Invoice id in VIDA teated as Extension Batch Number
        public string ClaimIdentifier { get; set; }
        public string PreviousClaimIdentifier { get; set; }
        public string MessageBundleID { get; set; }
        public string ClaimType { get; set; }
        public string ClaimSubType { get; set; }
        public string CreatedOn { get; set; }
        public string ClaimTotal { get; set; }
        public string EpisodeNo { get; set; }
        public List<Diagnosis> Diagnoses { get; set; }
        public List<ClaimItem> ClaimItems { get; set; }
        public VitalSign VitalSign { get; set; }
        public Encounter Encounter { get; set; }
        public Payer Payer { get; set; }
        public Provider Provider { get; set; }
        public Patient Patient { get; set; }
        public Patient PatientChild { get; set; }

        public Clinic Clinic { get; set; }
        public Practitioner Practitioner { get; set; }
        public ChiefComplaints ChiefComplaint { get; set; }
        public string Discovery { get; set; }
        public string OfflineEligibility { get; set; }
        public string NphiesEligibility { get; set; }
        public string NphiesEligibilitySystem { get; set; }
        public string OfflineApproval { get; set; }
        public string NphiesApprovalIdentifier { get; set; }
        public string NphiesApprovalSystem { get; set; }
        public string NphiesApprovalAuthRef { get; set; }
        public string IsDental { get; set; }
        public string IsPharmacy { get; set; }
        public string ResubmissionReference { get; set; }
        public ResubmissionDetail ResubmissionDetail { get; set; }
        public string DocumentReferenceID { get; set; }
        public string AppendSlashInPreAuthResponse { get; set; }
        public string Status { get; set; }
        public string NphieseRemarks { get; set; }
        public bool IsReferral { get; set; }
        public bool IsDrgEnable { get; set; } = false;
        public string DRGCode { get; set; } = string.Empty;
        public string DRGWeight { get; set; }
        public byte? DischargeDisposition { get; set; }
        public string AccountingPeriod { get; set; }
        public bool IsMohClaim { get; set; } = false;
        public byte ClaimMode { get; set; } 
        public DischargeSummary DischargeSummary { get; set; }
        public FileStorageProviderType FileStorageProviderType { get; set; }
        public List<string> DocumentIds { get; set; }


    }

    public class ResubmissionDetail
    {
        public List<ResubmissionRemark> resubmissionRemarks { get; set; }
        public List<ResubmissionAttachment> resubmissionAttachments { get; set; }
    }
   
    public class SupportingInfo
    {
        public string Code { get; set; }
        public string Name { get; set; }
        public string Value { get; set; }
    }
    public class ResubmissionRemark
    {
        public string Remarks { get; set; }
        public short ProcedureSeq { get; set; }
    }
    public class DischargeSummary
    {
        public short ClinicId { get; set; }
        public int DoctorId { get; set; }
        public byte? DischargeDisposition { get; set; }
    }
    public class ResubmissionAttachment
    {
        public string FileName { get; set; }
        public string ContentType { get; set; }
        public byte[] data { get; set; }
        public string CreatedOn { get; set; }
        public short ProcedureSeq { get; set; }
    }
    public class DaysSupply
    {
        public int SeqNo { get; set; }
        public string DaySupply { get; set; }
    }
    public class ChiefComplaints
    {
        public string ChiefComplaint { get; set; }
        public string CurrentMedication { get; set; }
        public string Status { get; set; }
        public string Comment { get; set; }
        public string OtherCondition { get; set; }
        public string SignificantSigns { get; set; }
    }

    public class Practitioner
    {
        public string DoctorID { get; set; }
        public string License1 { get; set; }
        public string License2 { get; set; }
        public string License1Expiry { get; set; }
        public string License2Expiry { get; set; }
        public string Active { get; set; }
        public string DoctorName { get; set; }
        public string Gender { get; set; }
        public string DateofJoining { get; set; }
        public string RoleCode { get; set; }
        public string RoleDisplay { get; set; }
        public string SpecialityCode { get; set; }
        public string SpecialityDisplay { get; set; }
        public string DischargeSpecialityCode { get; set; }
    }
    public class ClaimItem
    {
        private bool _package = false;
        public ClaimItem()
        {
            packageComponent = new List<PackageComponent>();
        }
        public string SBSProcedureCode { get; set; }
        public string SBSCodeDescription { get; set; }
        public string NphiesProcedureCodeSystem { get; set; }
        public string InvoiceNo { get; set; }
        public string ProcedureID { get; set; }
        public string ProcedureName { get; set; }
        public string InvoiceDate { get; set; }
        public string UnitPrice { get; set; }
        public string Quantity { get; set; }
        public string GrossAmount { get; set; }
        public string GrossDisc { get; set; }
        public string PatientShare { get; set; }
        public string CompanyShare { get; set; }
        public string VATRate { get; set; }
        public string CompanyTaxAmount { get; set; }
        public string PatientTaxAmount { get; set; }
        public string ToothNo { get; set; }
        public bool isPackage {
            get => _package;
            set
            {
                if (packageComponent != null && packageComponent.Count > 0)
                {
                    _package = true;
                }
                else
                    _package = false;
            }
        }  
        public string DaysSupply { get; set; }
        public string PackageDetail { get; set; }
        public int Sequence { get; set; }
        public decimal Factor { get; set; }
        public decimal NET { get; set; }
        public List<PackageComponent> packageComponent { get; set; } = new List<PackageComponent>();
        public string NphieseRemarks { get; set; }
        public string NphiesStatus { get; set; }
        public string ReasonCode { get; set; }
        public string ApprovedQuantity { get; set; }
        public string TaxApproved { get; set; }
        public string ClaimIdentifier { get; set; }
        public int RowId { get; set; }
        public string ApprovalNumber { get; set; }
        public long ServiceId { get; set; }
        public string ScientificCodes { get; set; }
        public string SelectionReason { get; set; }
        public string PharmacistSubstitute { get; set; }
        //Add resub remarmk and attachments 
        public string ServiceStartDate { get; set; }
        public string ServiceEndDate { get; set; }
        public bool? UnCatagoriesService { get; set; }
        public ResubmissionRemarksAndAttachments ResubmissionData { get; set; }
        public SupportingInfo? SupportingInfo { get; set; } 
        public bool? IsMohCategory { get; set; }
        public string MOHServiceCode { get; set; }
        public string MOHServiceCodeDescription { get; set; }
    }





public class Diagnosis
    {
        public string SequenceNo { get; set; }
        public string TypeCode { get; set; }
        public string TypeDescription { get; set; }
        public string ICD10CM { get; set; }
        public string ConditionOnset { get; set; }
        public bool? IsMorphology { get; set; }
        public bool? IsRTADiagnosis { get; set; }
        public string MorphologyCode { get; set; }
       
    }
    public class VitalSign
    {
        public string WeightKg { get; set; }
        public string HeightCm { get; set; }
        public string BloodPressureLower { get; set; }
        public string BloodPressureHigher { get; set; }
        public string VitalSignDate { get; set; }
        public string Temperature { get; set; }
        public string Pluse { get; set; }
        public string OxygenSaturation { get; set; }
        public string RespiratoryRate { get; set; }
        public string InvestigationResult { get; set; }//
        public string TreatmentPlan { get; set; }
        public string PatientHistory { get; set; }
        public string PhysicalExamination { get; set; }
        public string HistoryOfPresentIllness { get; set; }
    }
    public class Encounter
    {
        public string Identifier { get; set; }
        public string EncounterType { get; set; }
        public string STATUS { get; set; }
        public string Class { get; set; }
        public string ServiceTypeCode { get; set; }
        public string ServiceTypeDisplay { get; set; }
        public string priorityCode { get; set; }
        public string priorityDisplay { get; set; }
        public string Start { get; set; }
        public string END { get; set; }
        public string OriginalEncounter { get; set; }
        public string ServiceEventType { get; set; } = string.Empty;
        public string CauseOfDeath { get; set; } = string.Empty;
        public DateTime TriageDate { get; set; }
        public int? TriageCategory { get; set; }
        public int? EmergencyArrivalCode { get; set; }
        public int? EncounterAdmitSource { get; set; }
        public int? IntendedLenghtOfStay { get; set; }
        public int? DischargeDisposition { get; set; }
        public int? EmergencyDepartmentDisposition { get; set; }

    }
    public class Payer
    {
        public string CompanyID { get; set; }
        public string ORGType { get; set; }
        public string CompanyStatus { get; set; }
        public string CompanyName { get; set; }
        public string License { get; set; }
        public string PayerTpaNhicId { get; set; }
        public string PatientCardID { get; set; }
        public string PolicyNumber { get; set; }
        public bool? Shadowbillingenable { get; set; }

    }
    public class Provider
    {
        public string ProjectID { get; set; }
        public string SetupID { get; set; }
        public string Name { get; set; }
        public string STATUS { get; set; }
        public string ORGType { get; set; }
        public string License { get; set; }
        public int ProviderType { get; set; }

    }
    public class Payload
    {
        public string contentType { get; set; }
        public byte[] data { get; set; }
        public string title { get; set; }
        public string creation { get; set; }
        public int claimItemSequence { get; set; }
    }

    public class PackageComponent
    {
        public int SequenceNo { get; set; }

        public string ProcedureID { get; set; }

        public string ProcedureName { get; set; }

        public string SBSProcedureCode { get; set; }

        public string SBSCodeDescription { get; set; }

        public string NphiesProcedureCodeSystem { get; set; }

        public int Quantity { get; set; }

        public int UnitPrice { get; set; }

        public int Net { get; set; }
    }

    public class Clinic
    {
        public string ClinicID { get; set; }
        public string ClinicName { get; set; }
        public string ClinicNameN { get; set; }
    }
    //public class DischargeSummary
    //{
    //    public short ClinicId { get; set; }
    //    public int DoctorId { get; set; }
    //    public byte? DischargeDisposition { get; set; }
    //}
    public class ResponseExtention
    {
        public string ClaimStatus { get; set; }
        public string ClaimRemark { get; set; }
        public string TotalApproved { get; set; }
    }
    public class PollResponseDTO
    {
        public string ClaimIdentifier { get; set; }
        public int FacilityId { get; set; }
        public long ProcessId { get; set; }
        public byte CriteriaType { get; set; }
        public int CreatedBy { get; set; }
    }
}
