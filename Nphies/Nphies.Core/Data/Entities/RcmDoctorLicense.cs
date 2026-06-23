using System;
using System.Collections.Generic;

namespace Nphies.Core.Data.Entities
{
    public partial class RcmDoctorLicense
    {
        public int LicenseId { get; set; }
        public int DoctorId { get; set; }
        public int OrganizationId { get; set; }
        public int FacilityGroupId { get; set; }
        public int FacilityId { get; set; }
        public int? LicenseType { get; set; }
        public string LicenseNumber { get; set; }
        public DateTime? LicenseExpiryDate { get; set; }
        public string DoctorLogin { get; set; }
        public string DoctorPwd { get; set; }
        public bool? IsActive { get; set; }
        public int CreatedBy { get; set; }
        public DateTime CreatedOn { get; set; }
        public int? ModifiedBy { get; set; }
        public DateTime? ModifiedOn { get; set; }

        public virtual RcmDoctor RcmDoctor { get; set; }
    }
}
