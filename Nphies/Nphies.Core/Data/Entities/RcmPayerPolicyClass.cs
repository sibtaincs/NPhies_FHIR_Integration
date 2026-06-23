using System;
using System.Collections.Generic;

namespace Nphies.Core.Data.Entities
{
    public partial class RcmPayerPolicyClass
    {
        public RcmPayerPolicyClass()
        {
            RcmPayerPolicyClassDetails = new HashSet<RcmPayerPolicyClassDetail>();
        }

        public int PayerPolicyClassId { get; set; }
        public int OrganizationId { get; set; }
        public int PayerPolicyId { get; set; }
        public int PayerId { get; set; }
        public string PayerPolicyClassName { get; set; }
        public string PayerPolicyClassNameN { get; set; }
        public DateTime? ValidFrom { get; set; }
        public DateTime? ValidTo { get; set; }
        public string Remarks { get; set; }
        public bool IsActive { get; set; }
        public int CreatedBy { get; set; }
        public DateTime CreatedOn { get; set; }
        public int? ModifiedBy { get; set; }
        public DateTime? ModifiedOn { get; set; }
        public string ExternalCode { get; set; }

        public virtual RcmPayerPolicy RcmPayerPolicy { get; set; }
        public virtual ICollection<RcmPayerPolicyClassDetail> RcmPayerPolicyClassDetails { get; set; }
    }
}
