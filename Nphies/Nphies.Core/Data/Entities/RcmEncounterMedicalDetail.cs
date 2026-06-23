using System;
using System.Collections.Generic;

namespace Nphies.Core.Data.Entities
{
    public partial class RcmEncounterMedicalDetail
    {
        public long MedicalInfoId { get; set; }
        public int OrganizationId { get; set; }
        public string FacilityGroupId { get; set; }
        public int FacilityId { get; set; }
        public string EncounterNo { get; set; }
        public string PatientMrn { get; set; }
        public string MedicalData { get; set; }
        public bool IsActive { get; set; }
        public int Createdby { get; set; }
        public DateTime CreatedOn { get; set; }
        public int? Updatedby { get; set; }
        public DateTime? UpdatedOn { get; set; }
    }
}
