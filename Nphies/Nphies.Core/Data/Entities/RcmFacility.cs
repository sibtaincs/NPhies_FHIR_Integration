using System;
using System.Collections.Generic;

namespace Nphies.Core.Data.Entities
{
    public partial class RcmFacility
    {
        public RcmFacility()
        {
            RcmClinics = new HashSet<RcmClinic>();
        }

        public int FacilityId { get; set; }
        public int OrganizationId { get; set; }
        public int FacilityGroupId { get; set; }
        public string FacilityName { get; set; }
        public string FacilityNameN { get; set; }
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
        public string ClientName { get; set; }
        public byte[] PasswordHash { get; set; }
        public byte? DataUploadingType { get; set; }
        public byte? CutOfDays { get; set; }
        public byte? CutOfDate { get; set; }
        public string ExternalCode2 { get; set; }
        public bool? IsNphiesEnable { get; set; }
        public string NphiesLicenseNo { get; set; }
        public string FacilityNameAlias { get; set; }
        public bool? Shadowbillingenable { get; set; }
        public int? ProviderType { get; set; }
        public byte? FileStorageProvider { get; set; }

        public virtual RcmFacilityGroup RcmFacilityGroup { get; set; }
        public virtual ICollection<RcmClinic> RcmClinics { get; set; }
    }
}
