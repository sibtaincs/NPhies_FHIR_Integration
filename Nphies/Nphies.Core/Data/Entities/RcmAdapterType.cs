using System;
using System.Collections.Generic;

namespace Nphies.Core.Data.Entities
{
    public partial class RcmAdapterType
    {
        public RcmAdapterType()
        {
            RcmAdapterParameters = new HashSet<RcmAdapterParameter>();
            RcmAdapters = new HashSet<RcmAdapter>();
        }

        public int AdapterTypeId { get; set; }
        public string Name { get; set; }
        public int CreatedBy { get; set; }
        public DateTime CreatedOn { get; set; }
        public bool? IsActive { get; set; }

        public virtual ICollection<RcmAdapterParameter> RcmAdapterParameters { get; set; }
        public virtual ICollection<RcmAdapter> RcmAdapters { get; set; }
    }
}
