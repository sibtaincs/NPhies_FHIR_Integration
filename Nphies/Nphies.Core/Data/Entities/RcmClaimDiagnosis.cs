using System;
using System.Collections.Generic;

namespace Nphies.Core.Data.Entities
{
    public partial class RcmClaimDiagnosis
    {
        public long ClaimId { get; set; }
        public int OrganizationId { get; set; }
        public string DiagnosisCode { get; set; }
        public byte? DiagnosisType { get; set; }
        public byte? DiagnosisCodeType { get; set; }
        public string DiagnosisDescription { get; set; }
        public short LineitemNo { get; set; }
        public byte? IllnessTypeIndicator { get; set; }
        public string DiagnoisInfoType { get; set; }
        public string DiagnoisInfoCode { get; set; }
        public bool? IsActive { get; set; }
        public bool? Isdischarge { get; set; }
        //public bool? IsMorphology { get; set; }
        //public bool? IsRTADiagnosis { get; set; }
        public string MorphologyCode { get; set; }
        public int? CreatedBy { get; set; }
        public DateTime? CreatedOn { get; set; }
        public int? ModifiedBy { get; set; }
        public DateTime? ModifiedOn { get; set; }
        public int? OrderBy { get; set; }
        public string? ConditionOnset { get; set; }
        public virtual RcmClaim RcmClaim { get; set; }
    }
}
