using System;
using System.Collections.Generic;

namespace Nphies.Core.Data.Entities
{
    public partial class RcmExternalServiceDetail
    {
        public int Id { get; set; }
        public int OrganizationId { get; set; }
        public int FacilityId { get; set; }
        public string ConstantName { get; set; }
        public string DefaultValue { get; set; }
        public string Description { get; set; }
        public string Module { get; set; }
        public DateTime CreatedOn { get; set; }
        public int CreatedBy { get; set; }
        public int? ModifiedBy { get; set; }
        public DateTime? ModifiedOn { get; set; }
        public bool? IsActive { get; set; }
    }
}
