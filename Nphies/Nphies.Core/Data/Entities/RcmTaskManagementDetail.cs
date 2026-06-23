using System;
using System.Collections.Generic;

namespace Nphies.Core.Data.Entities
{
    public partial class RcmTaskManagementDetail
    {
        public int Id { get; set; }
        public int TaskId { get; set; }
        public int RoleId { get; set; }
        public int UserId { get; set; }
        public decimal TaskPercentage { get; set; }
        public int CreatedBy { get; set; }
        public DateTime CreatedOn { get; set; }
        public int? ModifiedBy { get; set; }
        public DateTime? ModifiedOn { get; set; }
        public bool? IsActive { get; set; }
        public byte? ClaimType { get; set; }

        public virtual RcmTaskManagementMaster Task { get; set; }
    }
}
