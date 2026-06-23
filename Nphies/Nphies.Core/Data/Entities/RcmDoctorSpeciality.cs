using System;
using System.Collections.Generic;

namespace Nphies.Core.Data.Entities
{
    public partial class RcmDoctorSpeciality
    {
        public int SpecialityId { get; set; }
        public int DoctorId { get; set; }
        public int OrganizationId { get; set; }
        public int FacilityGroupId { get; set; }
        public int FacilityId { get; set; }
        public bool? IsActive { get; set; }
        public int CreatedBy { get; set; }
        public DateTime CreatedOn { get; set; }
        public int? ModifiedBy { get; set; }
        public DateTime? ModifiedOn { get; set; }
        public string ExternalCode { get; set; }

        public virtual RcmDoctor RcmDoctor { get; set; }
    }
}
