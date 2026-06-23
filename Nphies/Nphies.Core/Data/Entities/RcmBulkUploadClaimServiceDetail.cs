using System;
using System.Collections.Generic;

namespace Nphies.Core.Data.Entities
{
    public partial class RcmBulkUploadClaimServiceDetail
    {
        public long BulkUploadClaimServiceDetailId { get; set; }
        public long BulkUploadProcessLogId { get; set; }
        public int OrganizationId { get; set; }
        public int UserId { get; set; }
        public int EncounterNo { get; set; }
        public string InvoiceNo { get; set; }
        public byte? ServiceType { get; set; }
        public string ServiceCode { get; set; }
        public DateTime? ServiceStartDateTime { get; set; }
        public DateTime? ServiceEndDateTime { get; set; }
        public string StandardCode { get; set; }
        public decimal? ServiceQuantity { get; set; }
        public string ServiceReferenceNumber { get; set; }
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
        public string EncounterType { get; set; }
        public string EncounterStartType { get; set; }
        public string EncounterEndType { get; set; }
        public string Remarks { get; set; }
        public bool? IsAnyError { get; set; }
        public int CreatedBy { get; set; }
        public DateTime? CreatedOn { get; set; }
        public int ModifiedBy { get; set; }
        public DateTime? ModifiedOn { get; set; }
        public bool? IsDeleted { get; set; }
        public long? ErrorId { get; set; }
        public string ErrorDescription { get; set; }
        public int? RcmcategoryId { get; set; }
        public string RcmstandardCode { get; set; }
        public int? RcmserviceId { get; set; }
        public bool? IsValidated { get; set; }
        public bool? IsProcessed { get; set; }
        public string ServiceReferenceIdentity { get; set; }
        public byte SubmissionType { get; set; }
        public bool? IsPartOfPackageInvoice { get; set; }
        public int? ParentInvoiceNo { get; set; }
        public string TransactionReferenceNo { get; set; }
        public DateTime? TransactionReferenceDate { get; set; }
        public bool? IsPackage { get; set; }
        public string ParentServiceCode { get; set; }
        public DateTime? InvoiceDate { get; set; }
        public long? MidTableReferenceId { get; set; }
        public short? ErrorType { get; set; }
        public int? CompanyId { get; set; }
        public int? ClassId { get; set; }
        public string ToothNo { get; set; }
    }
}
