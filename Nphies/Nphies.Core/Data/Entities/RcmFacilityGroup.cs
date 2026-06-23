using System;
using System.Collections.Generic;

namespace Nphies.Core.Data.Entities
{
    public partial class RcmFacilityGroup
    {
        public RcmFacilityGroup()
        {
            RcmFacilities = new HashSet<RcmFacility>();
            RcmServiceCatalogs = new HashSet<RcmServiceCatalog>();
            RcmServiceCategories = new HashSet<RcmServiceCategory>();
            RcmServiceGroups = new HashSet<RcmServiceGroup>();
        }

        public int FacilityGroupId { get; set; }
        public int OrganizationId { get; set; }
        public string FacilityGroupName { get; set; }
        public string FacilityGroupNameN { get; set; }
        public string EmailAddress { get; set; }
        public string WebSiteUrl { get; set; }
        public int? CountryId { get; set; }
        public int? StateId { get; set; }
        public int? CityId { get; set; }
        public string AddressArea { get; set; }
        public string AddressStreet { get; set; }
        public string ZipCode { get; set; }
        public string PhoneOffice1 { get; set; }
        public string PhoneOffice2 { get; set; }
        public string FaxNo { get; set; }
        public string RegistrationNo { get; set; }
        public string TaxRegistrationNo { get; set; }
        public bool IsActive { get; set; }
        public int CreatedBy { get; set; }
        public DateTime CreatedOn { get; set; }
        public int? ModifiedBy { get; set; }
        public DateTime? ModifiedOn { get; set; }
        public string ExternalCode { get; set; }

        public virtual RcmOrganization Organization { get; set; }
        public virtual ICollection<RcmFacility> RcmFacilities { get; set; }
        public virtual ICollection<RcmServiceCatalog> RcmServiceCatalogs { get; set; }
        public virtual ICollection<RcmServiceCategory> RcmServiceCategories { get; set; }
        public virtual ICollection<RcmServiceGroup> RcmServiceGroups { get; set; }
    }
}
