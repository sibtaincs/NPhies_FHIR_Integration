using System;
using System.Collections.Generic;

namespace Nphies.Core.Data.Entities
{
    public partial class RcmServiceComponent
    {
        public RcmServiceComponent()
        {
            RcmServiceComponentCharges = new HashSet<RcmServiceComponentCharge>();
        }

        public int ServiceComponentId { get; set; }
        public short LineItemNo { get; set; }
        public long ServiceId { get; set; }
        public int OrganizationId { get; set; }
        public bool IsActive { get; set; }
        public int CreatedBy { get; set; }
        public DateTime CreatedOn { get; set; }
        public int? ModifiedBy { get; set; }
        public DateTime? ModifiedOn { get; set; }

        public virtual ICollection<RcmServiceComponentCharge> RcmServiceComponentCharges { get; set; }
    }
}
