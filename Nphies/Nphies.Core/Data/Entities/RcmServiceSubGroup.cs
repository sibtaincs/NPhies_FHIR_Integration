using System;
using System.Collections.Generic;

namespace Nphies.Core.Data.Entities
{
    public partial class RcmServiceSubGroup
    {
        public RcmServiceSubGroup()
        {
            RcmServiceCatalogs = new HashSet<RcmServiceCatalog>();
        }

        public int ServiceSubGroupId { get; set; }
        public int FacilityGroupId { get; set; }
        public int OrganizationId { get; set; }
        public string ServiceSubGroupName { get; set; }
        public string ServiceSubGroupNameN { get; set; }
        public string ServiceSubGroupCode { get; set; }
        public bool IsActive { get; set; }
        public int CreatedBy { get; set; }
        public DateTime CreatedOn { get; set; }
        public int? ModifiedBy { get; set; }
        public DateTime? ModifiedOn { get; set; }
        public string ExternalCode { get; set; }

        public virtual ICollection<RcmServiceCatalog> RcmServiceCatalogs { get; set; }
    }
}
