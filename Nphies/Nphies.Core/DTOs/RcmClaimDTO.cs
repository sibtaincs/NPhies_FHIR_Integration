using Nphies.Core.Data.Entities;
using System;
using System.Collections.Generic;

namespace Nphies.Core.DTOs
{
    public class RcmClaimDTO
    {
        public long ClaimId { get; set; }
        public int OrganizationId { get; set; }
        public int PayerId { get; set; }
        public int DoctorId { get; set; }
        public int FacilityId { get; set; }
        public int ClinicId { get; set; }
        public byte? EncounterType { get; set; }
        public int? EncounterNo { get; set; }
        public string BatchRefNo { get; set; }
        public string EligibilityCode { get; set; }
        public string PreAuthNo { get; set; }
        public DateTime? EncounterDateTime { get; set; }
        public DateTime? DischargeDateTime { get; set; }
        public string Remarks { get; set; }
        public byte? SubmissionType { get; set; }
        public decimal? GrossAmount { get; set; }
        public decimal? NetAmount { get; set; }
        public decimal? DiscountAmount { get; set; }
        public decimal? PatientShare { get; set; }
        public decimal? CompanyShare { get; set; }
        public string ExternalReferenceNo { get; set; }
        public byte? Status { get; set; }
        public int? CreatedBy { get; set; }
        public DateTime? CreatedOn { get; set; }
        public int? ModifiedBy { get; set; }
        public DateTime? ModifiedOn { get; set; }
        public long? EpisodeId { get; set; }
        public string PolicyNumber { get; set; }
        public string ApproverId { get; set; }
        public bool? IsReviewdByRuleEngine { get; set; }
        public string Comments { get; set; }
        public string ClaimSubmisionRefrenceNo { get; set; }
        public string RoomNo { get; set; }
        public string BedNo { get; set; }
        public decimal? SettlementAmount { get; set; }
        public long? RemittanceLogId { get; set; }
        public short? RejectionCount { get; set; }
        public DateTime? InvoiceDate { get; set; }
        public byte? ClaimType { get; set; }
        public bool? IsTechnicalReviewed { get; set; }
        public bool? IsMedicalReviewed { get; set; }
        public decimal? CompanyTax { get; set; }
        public decimal? PatientTax { get; set; }
        public DateTime? SubmittedDateTime { get; set; }
        public bool? IsInsuranceEligible { get; set; }
        public bool? IsProcessedByBgjob { get; set; }
        public decimal? RefundAmount { get; set; }
        public decimal? ReturnAmount { get; set; }
        public bool? IsRefund { get; set; }
        public bool? IsReturn { get; set; }
        public int? PayerPolicyId { get; set; }
        public bool? IsUpdatedInvoice { get; set; }
        public bool? IsReadyForSubmit { get; set; }
        public int? StatusOld { get; set; }
        public int? PostingStatus { get; set; }
        public string ClaimBundleId { get; set; }
        public string ClaimIdentifier { get; set; }
        public int? SequenceNo { get; set; }
        public string NphieseRemarks { get; set; }
        public string Discovery { get; set; }
        public string OfflineEligibility { get; set; }
        public string NphiesEligibility { get; set; }
        public string NphiesEligibilitySystem { get; set; }
        public string OfflineApproval { get; set; }
        public string NphiesApprovalIdentifier { get; set; }
        public string NphiesApprovalSystem { get; set; }
        public string NphiesApprovalAuthRef { get; set; }
        public string CommunicationUrl { get; set; }
        public string CommunicationIdentifier { get; set; }
        public string TotalApproved { get; set; }
        public string TotalSubmitted { get; set; }
        public string ResponseBundleId { get; set; }
        public bool? IsErrorResolved { get; set; }
        public string ItemReason { get; set; }
        public string ReasonCode { get; set; }
        public DateTime? ResponseReceivedOn { get; set; }
        public bool? IsPendedReceived { get; set; }
        public string BatchBundleId { get; set; }
        public byte? ReSubmissionStatus { get; set; }
        public long? ProcessId { get; set; }
        public int? SubmittedBy { get; set; }
        public DateTime? SubmittedOn { get; set; }
        public string BatchIdentifier { get; set; }
        public int? DenialParameterType { get; set; }
        public int? DenialParameterCode { get; set; }
        public string SubStatus { get; set; }
        public Guid? DocumentReferenceNo { get; set; }
        public bool? IsDocumentProcessed { get; set; }
        public bool? TechnicalError { get; set; }
        public bool? MedicalError { get; set; }
        public int? TaskId { get; set; }
        public byte? TechnicalStatus { get; set; }
        public byte? MedicalStatus { get; set; }
        public string TechnicalRejectionRemarks { get; set; }
        public string MedicalRejectionRemarks { get; set; }
        public bool? ISPDFAttached { get; set; }

        public virtual RcmOrganization Organization { get; set; }
        public virtual RcmPayer RcmPayer { get; set; }
        public virtual RcmClaimOpticalDetail RcmClaimOpticalDetail { get; set; }
        public virtual RcmClaimPatientDetail RcmClaimPatientDetail { get; set; }
        public virtual RcmClaimVitalSign RcmClaimVitalSign { get; set; }
        public virtual ICollection<RcmClaimDiagnosis> RcmClaimDiagnoses { get; set; }
        public virtual ICollection<RcmClaimServicesDetail> RcmClaimServicesDetails { get; set; }
        public virtual ICollection<RcmPaymentReconcilationDetail> RcmPaymentReconcilationDetails { get; set; }


         public  List<RcmClaimServicesDetailDTO> RcmClaimServicesDetailsDTO { get; set; }
    }
    public partial class RcmClaimServicesDetailDTO
    {
        public long ServiceId { get; set; }
        public string ServiceCode { get; set; }
        public string StandardCode { get; set; }
        public short? NphiesCodeType { get; set; }
        public string StandardCode2 { get; set; }
        public string ServiceReferenceNumber { get; set; }
        public DateTime? InvoiceDate { get; set; }
        public decimal? ServiceQuantity { get; set; }
        public decimal? ServiceGrossAmount { get; set; }
        public decimal? ServiceDiscountAmount { get; set; }
        public decimal? ServicePatientShare { get; set; }
        public decimal? ServiceCompanyShare { get; set; }
        public decimal? CompanyTax { get; set; }
        public decimal? PatientTax { get; set; }
        public string ToothNo { get; set; }
        public int RowId { get; set; }
        public string ApprovalNo { get; set; }

        public short ServiceType { get; set; }
        public string ServiceCatalogName { get; set; }

        public string ParentServiceCode { get; set; }
        public bool? IsRefund { get; set; }
        public bool? HasChild { get; set; }
        public bool? IsDeleted { get; set; }
        public bool? IsReturn { get; set; }
        public ResubmissionRemarksAndAttachments ResubmissionData { get; set; }
        public DateTime? ServiceStartDate { get; set; }
        public DateTime? ServiceEndDate { get; set; }
        public bool? UnCatagoriesService { get; set; }

        public string MOHServiceCode { get; set; }
        public string MOHServiceCodeDescription { get; set; }



    }
    public class ResubmissionRemarksAndAttachments
    {
        public string remarks { get; set; }
        public AttachmentFileReference attachment { get; set; }
    }
    public class AttachmentFileReference
    {
        public string contentType { get; set; }
        public byte[] data { get; set; }
        public string title { get; set; }
        public string creation { get; set; }
        public int claimItemSequence { get; set; }
    }
}
