using System;
using System.Collections.Generic;

namespace Nphies.Core.Data.Entities
{
    public partial class ReEntityMaster
    {
        public int EntityId { get; set; }
        public string EntityName { get; set; }
        public bool? IsActive { get; set; }
        public int? CreatedBy { get; set; }
        public DateTime? CreatedOn { get; set; }
        public int? ModifiedBy { get; set; }
        public DateTime? ModifiedOn { get; set; }
    }
}
