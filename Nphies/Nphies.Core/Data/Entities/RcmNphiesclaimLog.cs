using System;
using System.Collections.Generic;

namespace Nphies.Core.Data.Entities
{
    public partial class RcmNphiesclaimLog
    {
        public long ClaimLogId { get; set; }
        public string FacilityGroup { get; set; }
        public int FacilityId { get; set; }
        public long ClaimId { get; set; }
        public byte ClaimType { get; set; }
        public long ServiceReferenceNo { get; set; }
        public string ClaimIdentifier { get; set; }
        public int SequenceNo { get; set; }
        public string ServiceCode { get; set; }
        public double? AmountSubmitted { get; set; }
        public double? AmountApproved { get; set; }
        public bool? IsApproved { get; set; }
        public double? PatientShare { get; set; }
        public double? CompanyTax { get; set; }
        public byte? NphiesStatus { get; set; }
        public string SubmittedQuantity { get; set; }
        public string ApprovedQuantity { get; set; }
        public string ReasonCode { get; set; }
        public string TaxApproved { get; set; }
        public byte? ClaimStatus { get; set; }
        public string ClaimBundleId { get; set; }
        public string Remarks { get; set; }
        public int CreatedBy { get; set; }
        public DateTime CreatedOn { get; set; }
        public int? EditedBy { get; set; }
        public DateTime? EditedOn { get; set; }
    }
}
