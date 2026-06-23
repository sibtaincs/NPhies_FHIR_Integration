using System;
using System.Collections.Generic;

namespace Nphies.Core.Data.Entities
{
    public partial class RcmAlertGroup
    {
        public RcmAlertGroup()
        {
            RcmAlertGroupMembers = new HashSet<RcmAlertGroupMember>();
        }

        public int AlertGroupId { get; set; }
        public int OrganizationId { get; set; }
        public string GroupName { get; set; }
        public bool DeleteFlag { get; set; }
        public int CreatedBy { get; set; }
        public DateTime CreatedOn { get; set; }
        public int? ModifiedBy { get; set; }
        public DateTime? ModifiedOn { get; set; }

        public virtual ICollection<RcmAlertGroupMember> RcmAlertGroupMembers { get; set; }
    }
}
