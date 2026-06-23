using System;
using System.Collections.Generic;

namespace Nphies.Core.Data.Entities
{
    public partial class RcmServiceCategory
    {
        public RcmServiceCategory()
        {
            RcmServiceCatalogs = new HashSet<RcmServiceCatalog>();
        }

        public int ServiceCategoryId { get; set; }
        public int FacilityGroupId { get; set; }
        public int OrganizationId { get; set; }
        public string ServiceCategoryName { get; set; }
        public string ServiceCategoryNameN { get; set; }
        public string ServiceCategoryCode { get; set; }
        public bool IsActive { get; set; }
        public int CreatedBy { get; set; }
        public DateTime CreatedOn { get; set; }
        public int? ModifiedBy { get; set; }
        public DateTime? ModifiedOn { get; set; }
        public string ExternalCode { get; set; }

        public virtual RcmFacilityGroup RcmFacilityGroup { get; set; }
        public virtual ICollection<RcmServiceCatalog> RcmServiceCatalogs { get; set; }
    }
}
