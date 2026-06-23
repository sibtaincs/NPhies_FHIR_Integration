using System;
using System.Collections.Generic;

namespace Nphies.Core.Data.Entities
{
    public partial class RcmServiceCatalog
    {
        public RcmServiceCatalog()
        {
            RcmServiceCharges = new HashSet<RcmServiceCharge>();
        }

        public long ServiceId { get; set; }
        public int OrganizationId { get; set; }
        public int FacilityGroupId { get; set; }
        public int ServiceCategoryId { get; set; }
        public int ServiceGroupId { get; set; }
        public int? ServiceSubGroupId { get; set; }
        public string ServiceCatalogCode { get; set; }
        public short? ServiceType { get; set; }
        public string ServiceCatalogName { get; set; }
        public string ServiceCatalogNameN { get; set; }
        public bool IsPackage { get; set; }
        public bool? IsAutoAccepted { get; set; }
        public bool IsDiscountAllowed { get; set; }
        public byte Gender { get; set; }
        public byte? TaxType { get; set; }
        public decimal? TaxRate { get; set; }
        public bool IsActive { get; set; }
        public int CreatedBy { get; set; }
        public DateTime CreatedOn { get; set; }
        public int? ModifiedBy { get; set; }
        public DateTime? ModifiedOn { get; set; }
        public string ExternalCode { get; set; }
        public int? NphiesCodeSystem { get; set; }

        public virtual RcmFacilityGroup RcmFacilityGroup { get; set; }
        public virtual RcmServiceCategory RcmServiceCategory { get; set; }
        public virtual RcmServiceGroup RcmServiceGroup { get; set; }
        public virtual RcmServiceSubGroup RcmServiceSubGroup { get; set; }
        public virtual ICollection<RcmServiceCharge> RcmServiceCharges { get; set; }
    }
}
