using System;
using System.Collections.Generic;

namespace Nphies.Core.Data.Entities
{
    public partial class RcmAlertDetail
    {
        public int AlertDetailId { get; set; }
        public int OrganizationId { get; set; }
        public long AlertId { get; set; }
        public int ToId { get; set; }
        public bool ReadStatus { get; set; }
        public bool? FollowUp { get; set; }
        public DateTime? FollowUpDate { get; set; }
        public bool? FollowUpReadStatus { get; set; }
        public bool IsGroup { get; set; }
        public int? GroupId { get; set; }
        public bool DeleteFlag { get; set; }
        public int CreatedBy { get; set; }
        public DateTime CreatedOn { get; set; }
        public int? ModifiedBy { get; set; }
        public DateTime? ModifiedOn { get; set; }
    }
}
