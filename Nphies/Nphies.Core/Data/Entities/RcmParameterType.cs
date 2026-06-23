using System;
using System.Collections.Generic;

namespace Nphies.Core.Data.Entities
{
    public partial class RcmParameterType
    {
        public int Id { get; set; }
        public int ParameterGroup { get; set; }
        public int ParameterType { get; set; }
        public string ParameterTypeName { get; set; }
        public string ParameterTypeNameN { get; set; }
        public bool? IsActive { get; set; }
        public int? CreatedBy { get; set; }
        public DateTime? CreatedOn { get; set; }
        public int? ModifiedBy { get; set; }
        public DateTime? ModifiedOn { get; set; }
    }
}
