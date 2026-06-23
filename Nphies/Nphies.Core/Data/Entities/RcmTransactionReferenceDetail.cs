using System;
using System.Collections.Generic;

namespace Nphies.Core.Data.Entities
{
    public partial class RcmTransactionReferenceDetail
    {
        public long TransactionReferenceDetailsId { get; set; }
        public int OrganizationId { get; set; }
        public long ClaimId { get; set; }
        public byte Type { get; set; }
        public string TransactionReferenceId { get; set; }
        public byte ServiceType { get; set; }
        public int InvoiceNo { get; set; }
        public long ServiceId { get; set; }
        public DateTime? TransactionReferenceDate { get; set; }
        public DateTime? CreatedOn { get; set; }
        public int? CreatedBy { get; set; }
        public DateTime? ModifiedOn { get; set; }
        public int? ModifiedBy { get; set; }
        public int? Quantity { get; set; }
        public decimal? ServiceGrossAmount { get; set; }
        public decimal? ServiceDiscountAmount { get; set; }
        public decimal? ServiceNetAmount { get; set; }
        public decimal? ServicePatientShare { get; set; }
        public decimal? ServiceCompanyShare { get; set; }
        public decimal? PatientTax { get; set; }
        public decimal? CompanyTax { get; set; }
        public DateTime? InvoiceDate { get; set; }
        public short LineItemNo { get; set; }
        public bool? IsDeleted { get; set; }
        public string ServiceReferenceNumber { get; set; }
    }
}
