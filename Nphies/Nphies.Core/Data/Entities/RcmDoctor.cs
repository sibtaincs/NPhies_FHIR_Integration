using System;
using System.Collections.Generic;

namespace Nphies.Core.Data.Entities
{
    public partial class RcmDoctor
    {
        public RcmDoctor()
        {
            RcmDoctorLicenses = new HashSet<RcmDoctorLicense>();
            RcmDoctorSpecialities = new HashSet<RcmDoctorSpeciality>();
        }

        public int DoctorId { get; set; }
        public int OrganizationId { get; set; }
        public int FacilityGroupId { get; set; }
        public int FacilityId { get; set; }
        public string DoctorName { get; set; }
        public string DoctorNameN { get; set; }
        public int? NationalityId { get; set; }
        public byte? MaritalStatus { get; set; }
        public byte? Gender { get; set; }
        public DateTime? DateofBirth { get; set; }
        public byte? EmploymentType { get; set; }
        public string Remarks { get; set; }
        public string PhoneResidence { get; set; }
        public string MobileNumber { get; set; }
        public string FaxNo { get; set; }
        public string Address { get; set; }
        public int? CountryId { get; set; }
        public int? StateId { get; set; }
        public int? CityId { get; set; }
        public string ZipCode { get; set; }
        public string EmailExternal { get; set; }
        public string EmailInternal { get; set; }
        public DateTime? DateofJoining { get; set; }
        public bool? IsActive { get; set; }
        public int CreatedBy { get; set; }
        public DateTime CreatedOn { get; set; }
        public int? ModifiedBy { get; set; }
        public DateTime? ModifiedOn { get; set; }
        public string ExternalCode { get; set; }

        public virtual ICollection<RcmDoctorLicense> RcmDoctorLicenses { get; set; }
        public virtual ICollection<RcmDoctorSpeciality> RcmDoctorSpecialities { get; set; }
    }
}
