using System;
using System.Collections.Generic;

namespace Nphies.Core.Data.Entities
{
    public partial class RcmPayerPolicy
    {
        public RcmPayerPolicy()
        {
            RcmPayerPolicyClasses = new HashSet<RcmPayerPolicyClass>();
            RcmPayerPolicyDetails = new HashSet<RcmPayerPolicyDetail>();
        }

        public int PayerPolicyId { get; set; }
        public int OrganizationId { get; set; }
        public int PayerId { get; set; }
        public string PayerPolicyName { get; set; }
        public string PayerPolicyNameN { get; set; }
        public string PayerPolicyNo { get; set; }
        public DateTime PayerPolicyExpiryDate { get; set; }
        public string Remarks { get; set; }
        public bool IsActive { get; set; }
        public int CreatedBy { get; set; }
        public DateTime CreatedOn { get; set; }
        public int? ModifiedBy { get; set; }
        public DateTime? ModifiedOn { get; set; }
        public string ExternalCode { get; set; }
        public bool? IsNphiesEnable { get; set; }
        public string InsuranceNhic { get; set; }

        public virtual RcmPayer RcmPayer { get; set; }
        public virtual ICollection<RcmPayerPolicyClass> RcmPayerPolicyClasses { get; set; }
        public virtual ICollection<RcmPayerPolicyDetail> RcmPayerPolicyDetails { get; set; }
    }
}
