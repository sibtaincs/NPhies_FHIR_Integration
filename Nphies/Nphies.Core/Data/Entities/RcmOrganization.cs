using System;
using System.Collections.Generic;

namespace Nphies.Core.Data.Entities
{
    public partial class RcmOrganization
    {
        public RcmOrganization()
        {
            RcmClaims = new HashSet<RcmClaim>();
            RcmFacilityGroups = new HashSet<RcmFacilityGroup>();
            RcmPayers = new HashSet<RcmPayer>();
            RcmPaymentReconcilations = new HashSet<RcmPaymentReconcilation>();
        }

        public int OrganizationId { get; set; }
        public string OrganizationName { get; set; }
        public string OrganizationNameN { get; set; }
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

        public virtual ICollection<RcmClaim> RcmClaims { get; set; }
        public virtual ICollection<RcmFacilityGroup> RcmFacilityGroups { get; set; }
        public virtual ICollection<RcmPayer> RcmPayers { get; set; }
        public virtual ICollection<RcmPaymentReconcilation> RcmPaymentReconcilations { get; set; }
    }
}
