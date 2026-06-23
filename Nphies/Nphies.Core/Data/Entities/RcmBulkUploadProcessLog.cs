using System;
using System.Collections.Generic;

namespace Nphies.Core.Data.Entities
{
    public partial class RcmBulkUploadProcessLog
    {
        public long BulkUploadProcessLogId { get; set; }
        public int OrganizationId { get; set; }
        public int UserId { get; set; }
        public int? FacilityId { get; set; }
        public string FileType { get; set; }
        public int? TotalCountRecords { get; set; }
        public int? TotalSucceedCount { get; set; }
        public int? TotalErrorCount { get; set; }
        public string ErrorDescription { get; set; }
        public bool? IsValidated { get; set; }
        public bool? IsProcessed { get; set; }
        public bool? IsActive { get; set; }
        public bool? IsDeleted { get; set; }
        public string FileCaption { get; set; }
        public int CreatedBy { get; set; }
        public DateTime CreatedOn { get; set; }
        public int ModifiedBy { get; set; }
        public DateTime ModifiedOn { get; set; }
        public string SourceType { get; set; }
        public int? TotalProcessedCount { get; set; }
        public bool? IsSucceed { get; set; }
        public string BatchNo { get; set; }
        public byte? DataUploadType { get; set; }
    }
}
