using System;
using System.Collections.Generic;

namespace Nphies.Core.Data.Entities
{
    public partial class RcmBulkUploadClaimDiagnosisDetail
    {
        public long BulkUploadClaimDiagnosisDetailId { get; set; }
        public long BulkUploadProcessLogId { get; set; }
        public int OrganizationId { get; set; }
        public int UserId { get; set; }
        public int EncounterNo { get; set; }
        public string DiagnosisCode { get; set; }
        public string DiagnosisCodeType { get; set; }
        public byte? DiagnosisType { get; set; }
        public string DiagnosisDescription { get; set; }
        public string DiagnoisInfoCode { get; set; }
        public string DiagnoisInfoType { get; set; }
        public bool? IsAnyError { get; set; }
        public int CreatedBy { get; set; }
        public DateTime? CreatedOn { get; set; }
        public int ModifiedBy { get; set; }
        public DateTime? ModifiedOn { get; set; }
        public bool? IsDeleted { get; set; }
        public int? ErrorLogId { get; set; }
        public string ErrorDescription { get; set; }
        public bool? IsValidated { get; set; }
        public bool? IsProcessed { get; set; }
        public long? MidTableReferenceId { get; set; }
    }
}
