using System;
using System.Collections.Generic;

namespace Nphies.Core.Data.Entities
{
    public partial class RcmReportRequest
    {
        public int RequestId { get; set; }
        public int OrganizationId { get; set; }
        public int ReportId { get; set; }
        public int? IsProcessed { get; set; }
        public bool? IsActive { get; set; }
        public bool? IsDeleted { get; set; }
        public int CreatedBy { get; set; }
        public DateTime CreatedOn { get; set; }
        public int? ModifiedBy { get; set; }
        public DateTime? ModifiedOn { get; set; }
        public string FilePath { get; set; }
        public bool? IsDownLoaded { get; set; }
    }
}
