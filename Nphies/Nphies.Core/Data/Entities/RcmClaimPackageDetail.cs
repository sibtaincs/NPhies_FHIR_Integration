using System;
using System.Collections.Generic;

namespace Nphies.Core.Data.Entities
{
    public partial class RcmClaimPackageDetail
    {
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
        public string ParentInvoiceNo { get; set; }
        public string ParentServiceCode { get; set; }
    }
}
