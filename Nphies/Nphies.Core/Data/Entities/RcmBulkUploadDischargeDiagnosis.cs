using System;
using System.Collections.Generic;

namespace Nphies.Core.Data.Entities
{
    public partial class RcmBulkUploadDischargeDiagnosis
    {
        public int RcmdischargeDiagnosisId { get; set; }
        public int FacilityId { get; set; }
        public int OrganizationId { get; set; }
        public int EncounterNo { get; set; }
        public int DischargeNo { get; set; }
        public string CodeId { get; set; }
        public string Remarks { get; set; }
        public int? CreatedBy { get; set; }
        public DateTime? CreatedOn { get; set; }
        public int? EditedBy { get; set; }
        public DateTime? EditedOn { get; set; }
        public bool? Isactive { get; set; }
        public byte? DiagnosisType { get; set; }
        public string DiagnosisDescription { get; set; }
        public bool? IsProcessed { get; set; }
    }
}
