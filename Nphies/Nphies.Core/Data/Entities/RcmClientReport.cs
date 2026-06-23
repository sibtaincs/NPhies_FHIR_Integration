using System;
using System.Collections.Generic;

namespace Nphies.Core.Data.Entities
{
    public partial class RcmClientReport
    {
        public long ClientReportId { get; set; }
        public int OrganizationId { get; set; }
        public int FacilityId { get; set; }
        public long InvoiceNo { get; set; }
        public string InvoiceType { get; set; }
        public int CompanyId { get; set; }
        public string CompanyName { get; set; }
        public int GroupId { get; set; }
        public string PayerName { get; set; }
        public string PatientType { get; set; }
        public long PatientId { get; set; }
        public string PatientName { get; set; }
        public string PatientCardId { get; set; }
        public int ClinicId { get; set; }
        public string ClinicName { get; set; }
        public int DoctorId { get; set; }
        public string DoctorName { get; set; }
        public decimal? GrossAmount { get; set; }
        public decimal? NetAmount { get; set; }
        public decimal? DiscountAmount { get; set; }
        public decimal? PatientShare { get; set; }
        public decimal? CompanyShare { get; set; }
        public decimal? CompanyTaxAmount { get; set; }
        public decimal? PatientTaxAmount { get; set; }
        public int? CreatedBy { get; set; }
        public DateTime? CreatedOn { get; set; }
        public DateTime? InvoiceDate { get; set; }
        public string CompanyType { get; set; }
        public bool? IsActive { get; set; }
        public string BatchId { get; set; }
    }
}
