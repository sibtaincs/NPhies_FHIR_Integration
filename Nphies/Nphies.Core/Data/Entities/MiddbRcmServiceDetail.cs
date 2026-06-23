using System;
using System.Collections.Generic;

namespace Nphies.Core.Data.Entities
{
    public partial class MiddbRcmServiceDetail
    {
        public long ServiceDetailId { get; set; }
        public long? ServiceMainId { get; set; }
        public int? TargetOrgId { get; set; }
        public string FacilityGroupId { get; set; }
        public int? FacilityId { get; set; }
        public int? EncounterNo { get; set; }
        public string EncounterType { get; set; }
        public string ServiceReferenceNo { get; set; }
        public string ServiceReferenceIdentity { get; set; }
        public DateTime? ServiceDate { get; set; }
        public DateTime? ServiceOrderDate { get; set; }
        public byte? SubmissionType { get; set; }
        public int? PolicyId { get; set; }
        public int? PayerClassId { get; set; }
        public byte? ServiceType { get; set; }
        public string ServiceCode { get; set; }
        public decimal? ServiceQuantity { get; set; }
        public decimal? ServiceGrossAmount { get; set; }
        public decimal? ServiceNetAmount { get; set; }
        public decimal? ServiceDiscountPct { get; set; }
        public decimal? ServiceDiscountAmount { get; set; }
        public decimal? ServiceDeductiblePct { get; set; }
        public decimal? ServiceDeductibleAmount { get; set; }
        public decimal? ServiceReimbursementAmount { get; set; }
        public decimal? CompanyTax { get; set; }
        public decimal? PatientTax { get; set; }
        public string ApprovalNo { get; set; }
        public string ObservationRemarks { get; set; }
        public bool? IsPackage { get; set; }
        public string ParentServiceCode { get; set; }
        public bool? IsPartOfPackageInvoice { get; set; }
        public int? ParentServiceReferenceNo { get; set; }
        public string StandardCode { get; set; }
        public string ToothNo { get; set; }
        public string RefundReferenceNo { get; set; }
        public DateTime? RefundReferenceDate { get; set; }
        public string Remarks { get; set; }
        public int? ServiceCreatedBy { get; set; }
        public DateTime? ServiceCreatedOn { get; set; }
        public int? ServiceModifiedBy { get; set; }
        public DateTime? ServiceModifiedOn { get; set; }
        public DateTime? CreatedOn { get; set; }
        public bool? IsProcessed { get; set; }
        public DateTime? ProcessedOn { get; set; }
        public string ErrorDescription { get; set; }
        public bool? HasChild { get; set; }
        public long? ParentServiceDetailId { get; set; }
    }
}
