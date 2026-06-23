using System;
using System.Collections.Generic;

namespace Nphies.Core.Data.Entities
{
    public partial class RcmClientReportNew
    {
        public long ClientReportId { get; set; }
        public int OrganizationId { get; set; }
        public bool? IsActive { get; set; }
        public string BatchId { get; set; }
        public int FacilityId { get; set; }
        public string Branch { get; set; }
        public int CompanyGroup { get; set; }
        public string CompanyGroupName { get; set; }
        public int Company { get; set; }
        public string CompanyName { get; set; }
        public long PatientId { get; set; }
        public string PatientName { get; set; }
        public string InvoiceNo { get; set; }
        public DateTime? InvoiceDate { get; set; }
        public string RefundOrReturnNo { get; set; }
        public DateTime? RefundReturnDate { get; set; }
        public string PharmacyLocation { get; set; }
        public string InvoiceType { get; set; }
        public decimal? GrossAmount { get; set; }
        public decimal? NetRevenueAmount { get; set; }
        public decimal? DiscountAmount { get; set; }
        public decimal? PatientShare { get; set; }
        public decimal? CompanyShare { get; set; }
        public decimal? VatOnCompanyShare { get; set; }
        public decimal? VatOnPatientShare { get; set; }
        public decimal? TotalVatAmount { get; set; }
        public decimal? PatientShareInclusiveOfVat { get; set; }
        public decimal? CompanyShareInclusiveOfVat { get; set; }
        public decimal? TotalRevenueInclusiveOfVat { get; set; }
        public int? CreatedBy { get; set; }
        public DateTime? CreatedOn { get; set; }
    }
}
