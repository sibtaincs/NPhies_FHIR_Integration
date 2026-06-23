using System;
using System.Collections.Generic;

namespace Nphies.Core.Data.Entities
{
    public partial class RcmTaskAssignmentLog
    {
        public int Id { get; set; }
        public int TaskId { get; set; }
        public byte Status { get; set; }
        public string Description { get; set; }
        public int CreatedBy { get; set; }
        public DateTime CreatedOn { get; set; }
    }
}
