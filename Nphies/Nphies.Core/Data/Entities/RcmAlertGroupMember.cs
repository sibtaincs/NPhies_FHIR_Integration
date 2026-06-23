using System;
using System.Collections.Generic;

namespace Nphies.Core.Data.Entities
{
    public partial class RcmAlertGroupMember
    {
        public int AlertGroupMemberId { get; set; }
        public int OrganizationId { get; set; }
        public int AlertGroupId { get; set; }
        public int MemberId { get; set; }
        public bool DeleteFlag { get; set; }
        public int CreatedBy { get; set; }
        public DateTime CreatedOn { get; set; }
        public int? ModifiedBy { get; set; }
        public DateTime? ModifiedOn { get; set; }

        public virtual RcmAlertGroup AlertGroup { get; set; }
    }
}
