using System;
using System.Collections.Generic;

namespace Nphies.Core.Data.Entities
{
    public partial class RcmReferenceDimension
    {
        public RcmReferenceDimension()
        {
            RcmPayerDetails = new HashSet<RcmPayerDetail>();
            RcmPayerPolicyClassDetails = new HashSet<RcmPayerPolicyClassDetail>();
            RcmPayerPolicyDetails = new HashSet<RcmPayerPolicyDetail>();
            RcmServiceCharges = new HashSet<RcmServiceCharge>();
        }

        public int DimensionId { get; set; }
        public string DimentionType { get; set; }
        public string DimentionName { get; set; }
        public short? Precedence { get; set; }
        public bool IsActive { get; set; }
        public int CreatedBy { get; set; }
        public DateTime CreatedOn { get; set; }
        public int? ModifiedBy { get; set; }
        public DateTime? ModifiedOn { get; set; }

        public virtual ICollection<RcmPayerDetail> RcmPayerDetails { get; set; }
        public virtual ICollection<RcmPayerPolicyClassDetail> RcmPayerPolicyClassDetails { get; set; }
        public virtual ICollection<RcmPayerPolicyDetail> RcmPayerPolicyDetails { get; set; }
        public virtual ICollection<RcmServiceCharge> RcmServiceCharges { get; set; }
    }
}
