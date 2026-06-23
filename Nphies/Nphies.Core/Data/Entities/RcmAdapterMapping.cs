using System;
using System.Collections.Generic;

namespace Nphies.Core.Data.Entities
{
    public partial class RcmAdapterMapping
    {
        public int AdapterMappingId { get; set; }
        public int AdapterId { get; set; }
        public int ParameterMasterId { get; set; }
        public string ParameterType { get; set; }
        public string ParameterValue { get; set; }
        public int CreatedBy { get; set; }
        public DateTime CreatedOn { get; set; }
        public int? ModifiedBy { get; set; }
        public DateTime? ModifiedOn { get; set; }
        public bool? IsActive { get; set; }

        public virtual RcmAdapter Adapter { get; set; }
        public virtual RcmAdapterParameter ParameterMaster { get; set; }
    }
}
