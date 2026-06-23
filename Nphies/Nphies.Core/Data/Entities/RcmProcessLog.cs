using System;
using System.Collections.Generic;

namespace Nphies.Core.Data.Entities
{
    public partial class RcmProcessLog
    {
        public long ProcessLogId { get; set; }
        public byte? Jobtype { get; set; }
        public byte? ProcessStatus { get; set; }
        public DateTime StartTime { get; set; }
        public DateTime? EndTime { get; set; }
        public bool? IsActive { get; set; }
        public DateTime CreatedOn { get; set; }
        public int? CreatedBy { get; set; }
        public DateTime? UpdatedOn { get; set; }
        public int? UpdatedBy { get; set; }
        public string ErrorMessage { get; set; }
        public int? FacilityId { get; set; }
    }
}
