using System;
using System.Collections.Generic;

namespace Nphies.Core.Data.Entities
{
    public partial class RcmBulkUploadSegment
    {
        public int BulkUploadSegmentId { get; set; }
        public int OrganizationId { get; set; }
        public string SegmentType { get; set; }
        public string SegmentName { get; set; }
        public string SegmentMapWithHeader { get; set; }
        public int CreatedBy { get; set; }
        public DateTime? CreatedOn { get; set; }
        public int ModifiedBy { get; set; }
        public DateTime? ModifiedOn { get; set; }
        public bool? IsDeleted { get; set; }
    }
}
