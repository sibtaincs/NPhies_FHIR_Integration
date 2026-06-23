using System;
using System.Collections.Generic;

namespace Nphies.Core.Data.Entities
{
    public partial class RcmReportdiscrepancy
    {
        public long Id { get; set; }
        public int HeaderId { get; set; }
        public int OrganizationId { get; set; }
        public int FacilityId { get; set; }
        public DateTime InvoiceDate { get; set; }
        public int? InvoiceNo { get; set; }
        public int? SourceType { get; set; }
        public string Discrepancy { get; set; }
        public byte? DiscrepancyType { get; set; }
        public int? Createdby { get; set; }
        public DateTime? CreatedOn { get; set; }
        public string ReturnRefundReferenceNo { get; set; }
        public DateTime? RefundReturnDate { get; set; }
        public string InvoiceType { get; set; }
        public decimal? VidaGrossAmount { get; set; }
        public decimal? VidaNetAmount { get; set; }
        public decimal? VidaDiscountAmount { get; set; }
        public decimal? VidaPatientShare { get; set; }
        public decimal? VidaCompanyShare { get; set; }
        public decimal? VidaCompanyTax { get; set; }
        public decimal? VidaPatientTax { get; set; }
        public decimal? RcmgrossAmount { get; set; }
        public decimal? RcmnetAmount { get; set; }
        public decimal? RcmdiscountAmount { get; set; }
        public decimal? RcmpatientShare { get; set; }
        public decimal? RcmcompanyShare { get; set; }
        public decimal? RcmcompanyTax { get; set; }
        public decimal? RcmpatientTax { get; set; }
        public string RcmcompanyName { get; set; }
        public string VidaCompanyName { get; set; }
        public int? Status { get; set; }
    }
}
