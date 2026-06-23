using System;
using System.Collections.Generic;

namespace Nphies.Core.Data.Entities
{
    public partial class RcmAddResponseBinding
    {
        public int Id { get; set; }
        public int? RowId { get; set; }
        public int? AttachId { get; set; }
        public int? RemarksId { get; set; }
        public DateTime? CreatedOn { get; set; }
        public int? CreatedBy { get; set; }
        public DateTime? ModifiedOn { get; set; }
        public int? ModifiedBy { get; set; }
        public bool? IsActive { get; set; }
        public long ClaimId { get; set; }
        public int OrganizationId { get; set; }
        public string Description { get; set; }

    }
}
