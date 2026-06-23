using System;
using System.Collections.Generic;

namespace Nphies.Core.Data.Entities
{
    public partial class RcmTaskManagementMaster
    {
        public RcmTaskManagementMaster()
        {
            RcmTaskManagementDetails = new HashSet<RcmTaskManagementDetail>();
        }

        public int TaskId { get; set; }
        public int OrganizationId { get; set; }
        public DateTime FromDate { get; set; }
        public DateTime ToDate { get; set; }
        public int? FacilityId { get; set; }
        public int? PayerId { get; set; }
        public int? PolicyId { get; set; }
        public byte? EncounterType { get; set; }
        public int? ClinicId { get; set; }
        public int CreatedBy { get; set; }
        public DateTime CreatedOn { get; set; }
        public int? ModifiedBy { get; set; }
        public DateTime? ModifiedOn { get; set; }
        public bool? IsActive { get; set; }
        public bool IsRepetitive { get; set; }
        public string TaskName { get; set; }
        public int? ParentId { get; set; }
        public bool? IsSubTask { get; set; }
        public int? Trsupervisor { get; set; }
        public int? Trmanager { get; set; }
        public int? Mrsupervisor { get; set; }
        public int? Mrmanager { get; set; }
        public int? SeniorManager { get; set; }
        public byte? Status { get; set; }

        public virtual ICollection<RcmTaskManagementDetail> RcmTaskManagementDetails { get; set; }
    }
}
