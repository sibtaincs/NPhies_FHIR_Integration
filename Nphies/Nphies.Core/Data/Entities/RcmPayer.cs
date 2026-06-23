using System;
using System.Collections.Generic;

namespace Nphies.Core.Data.Entities
{
    public partial class RcmPayer
    {
        public RcmPayer()
        {
            RcmClaims = new HashSet<RcmClaim>();
            RcmPayerDetails = new HashSet<RcmPayerDetail>();
            RcmPayerPolicies = new HashSet<RcmPayerPolicy>();
            RePayerEventRuleGroups = new HashSet<RePayerEventRuleGroup>();
        }

        public int PayerId { get; set; }
        public int OrganizationId { get; set; }
        public string PayerName { get; set; }
        public string PayerNameN { get; set; }
        public string EmailAddress { get; set; }
        public string ContactPerson { get; set; }
        public string ContactMobile { get; set; }
        public string Fax { get; set; }
        public string PhoneOffice1 { get; set; }
        public string PhoneOffice2 { get; set; }
        public string ContactPersonEmail { get; set; }
        public string CompanyUrl { get; set; }
        public int? CountryId { get; set; }
        public int? StateId { get; set; }
        public int? CityId { get; set; }
        public string Pobox { get; set; }
        public string ZipCode { get; set; }
        public string Address { get; set; }
        public bool? Taxable { get; set; }
        public string CardFormat { get; set; }
        public string TaxRegistrationNo { get; set; }
        public short? ClaimType { get; set; }
        public byte? PayerType { get; set; }
        public bool IsActive { get; set; }
        public int CreatedBy { get; set; }
        public DateTime CreatedOn { get; set; }
        public int? ModifiedBy { get; set; }
        public DateTime? ModifiedOn { get; set; }
        public string ExternalCode { get; set; }
        public bool? IsNphiesEnable { get; set; }
        public string NphiesLicenseNo { get; set; }
        public bool? IsTPA { get; set; }
        public bool? IsManagedByTPA { get; set; }
        public bool? IsReferral { get; set; }
        public bool? Shadowbillingenable { get; set; }
        public virtual RcmOrganization Organization { get; set; }
        public virtual ICollection<RcmClaim> RcmClaims { get; set; }
        public virtual ICollection<RcmPayerDetail> RcmPayerDetails { get; set; }
        public virtual ICollection<RcmPayerPolicy> RcmPayerPolicies { get; set; }
        public virtual ICollection<RePayerEventRuleGroup> RePayerEventRuleGroups { get; set; }
    }
}
