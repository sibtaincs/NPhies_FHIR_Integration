using System;
using System.Collections.Generic;

namespace Nphies.Core.Data.Entities
{
    public partial class RcmClaimServicesDetail
    {
        public RcmClaimServicesDetail()
        {
            RcmClaimServiceObservation = new HashSet<Claim_Service_Observation>();

        }
        public long ClaimId { get; set; }
        public int OrganizationId { get; set; }
        public long ServiceId { get; set; }
        public short LineitemNo { get; set; }
        public DateTime? ServiceStartDateTime { get; set; }
        public DateTime? ServiceEndDateTime { get; set; }
        public byte? ServiceType { get; set; }
        public decimal? ServiceQuantity { get; set; }
        public string ServiceCode { get; set; }
        public byte? ServiceTypeIndicator { get; set; }
        public string ServiceReferenceNumber { get; set; }
        public decimal? ServiceGrossAmount { get; set; }
        public decimal? ServiceNetAmount { get; set; }
        public decimal? ServiceDiscountPct { get; set; }
        public decimal? ServiceDiscountAmount { get; set; }
        public decimal? ServicePatientSharePct { get; set; }
        public decimal? ServicePatientShare { get; set; }
        public decimal? ServiceCompanyShare { get; set; }
        public decimal? CompanyTax { get; set; }
        public decimal? PatientTax { get; set; }
        public int? EditedBy { get; set; }
        public DateTime? EditedOn { get; set; }
        public string StandardCode { get; set; }
        public decimal? PatientTaxPct { get; set; }
        public decimal? CompanyTaxPct { get; set; }
        public string ObservationRemarks { get; set; }
        public string EncounterStartType { get; set; }
        public string EncounterEndType { get; set; }
        public string ActivityEncounterType { get; set; }
        public decimal? SettlementAmount { get; set; }
        public short? RejectionReasonId { get; set; }
        public short? RemittanceStatus { get; set; }
        public string ApprovalNo { get; set; }
        public byte? Status { get; set; }
        public short? RejectionReasonIdByReviewer { get; set; }
        public DateTime? RejectionDate { get; set; }
        public string ServiceReferenceIdentity { get; set; }
        public decimal? RefundAmount { get; set; }
        public decimal? ReturnAmount { get; set; }
        public bool? IsRefund { get; set; }
        public bool? IsReturn { get; set; }
        public string Remarks { get; set; }
        public bool? IsPartOfPackageInvoice { get; set; }
        public int? ParentInvoiceNo { get; set; }
        public bool? IsPackage { get; set; }
        public string ParentServiceCode { get; set; }
        public DateTime? InvoiceDate { get; set; }
        public bool? IsDeleted { get; set; }
        public string ApprovalNoOld { get; set; }
        public int? ClassId { get; set; }
        public string ToothNo { get; set; }
        public DateTime? CreatedOn { get; set; }
        public int RowId { get; set; }
        public string NphieseRemarks { get; set; }
        public double? ApprovedAmount { get; set; }
        public byte? NphiesStatus { get; set; }
        public string ReasonCode { get; set; }
        public int? ApprovedQuantity { get; set; }
        public decimal? TaxApproved { get; set; }
        public string ClaimIdentifier { get; set; }
        public string DaysSupplies { get; set; }
        public string SubmittedQuantity { get; set; }
        public string ItemReason { get; set; }
        public string SubmittedAmount { get; set; }
        public string BenefitAmount { get; set; }
        public int? DenialParameterType { get; set; }
        public int? DenialParameterCode { get; set; }
        public bool? IsPerformed { get; set; }
        public string StandardCode2 { get; set; }
        public short? NphiesCodeType { get; set; }
        public bool? HasChild { get; set; }
        public long? ParentServiceId { get; set; }
        public int? NphiesSeqNo { get; set; }
        public long? ProcessId { get; set; }
        public byte? ReSubmissionStatus { get; set; }
        public DateTime? ResponseReceivedOn { get; set; }
        public string MOHServiceCode { get; set; }
        public string MOHServiceCodeDescription { get; set; }
        public virtual ICollection<Claim_Service_Observation> RcmClaimServiceObservation { get; set; }

        public virtual RcmClaim RcmClaim { get; set; }
    }


   

}
