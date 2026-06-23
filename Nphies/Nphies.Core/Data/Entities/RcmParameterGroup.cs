using System;
using System.Collections.Generic;

namespace Nphies.Core.Data.Entities
{
    public partial class RcmParameterGroup
    {
        public int Id { get; set; }
        public int ParameterGroup { get; set; }
        public string ParameterGroupName { get; set; }
        public string ParameterGroupNameN { get; set; }
        public int CreatedBy { get; set; }
        public DateTime CreatedOn { get; set; }
        public int? ModifiedBy { get; set; }
        public DateTime? ModifiedOn { get; set; }
    }
}
