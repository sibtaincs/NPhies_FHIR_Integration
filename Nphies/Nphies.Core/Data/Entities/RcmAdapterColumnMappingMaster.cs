using System;
using System.Collections.Generic;

namespace Nphies.Core.Data.Entities
{
    public partial class RcmAdapterColumnMappingMaster
    {
        public int Id { get; set; }
        public int AdapterId { get; set; }
        public int ColumnId { get; set; }
        public int CreatedBy { get; set; }
        public DateTime CreatedOn { get; set; }
        public int? ModifiedBy { get; set; }
        public DateTime? ModifiedOn { get; set; }
        public bool? IsActive { get; set; }

        public virtual RcmAdapter Adapter { get; set; }
    }
}
