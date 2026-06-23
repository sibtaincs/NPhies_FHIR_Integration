using System;
using System.Collections.Generic;

namespace Nphies.Core.Data.Entities
{
    public partial class RcmPaymentReconcilation
    {
        public RcmPaymentReconcilation()
        {
            RcmPaymentReconcilationDetails = new HashSet<RcmPaymentReconcilationDetail>();
        }

        public long PaymentReconcilationId { get; set; }
        public int OrganizationId { get; set; }
        public int FacilityGroupId { get; set; }
        public int? FacilityId { get; set; }
        public int PayerId { get; set; }
        public DateTime PeriodFromDate { get; set; }
        public DateTime PeriodToDate { get; set; }
        public decimal ReceivedAmount { get; set; }
        public string PayeeReference { get; set; }
        public string TransactionReference { get; set; }
        public decimal? UnAppliedAmount { get; set; }
        public bool IsAutomatic { get; set; }
        public int? AjustmentTypeId { get; set; }
        public decimal? AdjustmentPercentage { get; set; }
        public int Status { get; set; }
        public int CreatedBy { get; set; }
        public DateTime CreatedOn { get; set; }
        public int? VerifiedBy { get; set; }
        public DateTime? VerifiedOn { get; set; }
        public int? UpdatedBy { get; set; }
        public DateTime? UpdatedOn { get; set; }

        public virtual RcmOrganization Organization { get; set; }
        public virtual ICollection<RcmPaymentReconcilationDetail> RcmPaymentReconcilationDetails { get; set; }
    }
}
