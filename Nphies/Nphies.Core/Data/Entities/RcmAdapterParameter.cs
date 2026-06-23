using System;
using System.Collections.Generic;

namespace Nphies.Core.Data.Entities
{
    public partial class RcmAdapterParameter
    {
        public RcmAdapterParameter()
        {
            RcmAdapterMappings = new HashSet<RcmAdapterMapping>();
        }

        public int ParameterId { get; set; }
        public int AdapterTypeId { get; set; }
        public int ParameterMasterId { get; set; }
        public bool AllowDuplicate { get; set; }
        public int CreatedBy { get; set; }
        public DateTime CreatedOn { get; set; }
        public bool? IsActive { get; set; }

        public virtual RcmAdapterType AdapterType { get; set; }
        public virtual RcmAdapterParameterMaster ParameterMaster { get; set; }
        public virtual ICollection<RcmAdapterMapping> RcmAdapterMappings { get; set; }
    }
}
