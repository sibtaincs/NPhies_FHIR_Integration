using System;
using System.Collections.Generic;

namespace Nphies.Core.Data.Entities
{
    public partial class RcmReportsDayWise
    {
        public long ReportId { get; set; }
        public long HeaderId { get; set; }
        public int OrganizationId { get; set; }
        public long? ClaimId { get; set; }
        public int? PolicyId { get; set; }
        public string PolicyNo { get; set; }
        public int FacilityId { get; set; }
        public string FacilityName { get; set; }
        public int CompanyGroup { get; set; }
        public string CompanyGroupName { get; set; }
        public string Status { get; set; }
        public string PolicyName { get; set; }
        public long PatientId { get; set; }
        public string PatientName { get; set; }
        public string InvoiceNo { get; set; }
        public DateTime? InvoiceDate { get; set; }
        public string ReturnRefundReferenceNo { get; set; }
        public DateTime? RefundReturnDate { get; set; }
        public string InvoiceType { get; set; }
        public decimal? GrossAmount { get; set; }
        public decimal? NetAmount { get; set; }
        public decimal? DiscountAmount { get; set; }
        public decimal? PatientShare { get; set; }
        public decimal? CompanyShare { get; set; }
        public decimal? CompanyTax { get; set; }
        public decimal? PatientTax { get; set; }
        public decimal? TotalVatAmount { get; set; }
        public decimal? PatientShareInclusiveOfVat { get; set; }
        public decimal? CompanyShareInclusiveOfVat { get; set; }
        public decimal? TotalRevenueInclusiveOfVat { get; set; }
        public int? CreatedBy { get; set; }
        public DateTime? CreatedOn { get; set; }
    }
}
