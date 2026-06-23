using System;
using System.Collections.Generic;

namespace Nphies.Core.Data.Entities
{
    public partial class RcmBulkAction
    {
        public long Id { get; set; }
        public int OrganizationId { get; set; }
        public long ClaimId { get; set; }
        public byte CurrentStatus { get; set; }
        public byte NextAction { get; set; }
        public bool? IsActive { get; set; }
        public bool? IsDeleted { get; set; }
        public bool? IsProcessed { get; set; }
        public int? CreatedBy { get; set; }
        public DateTime? CreatedOn { get; set; }
        public int? ModifiedBy { get; set; }
        public DateTime? ModifiedOn { get; set; }
        public byte? FinalStatus { get; set; }
    }
}
