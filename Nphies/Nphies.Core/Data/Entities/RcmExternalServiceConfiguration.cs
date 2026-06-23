using System;
using System.Collections.Generic;

namespace Nphies.Core.Data.Entities
{
    public partial class RcmExternalServiceConfiguration
    {
        public int Id { get; set; }
        public int OrganizationId { get; set; }
        public int FacilityId { get; set; }
        public int PayerId { get; set; }
        public string ConfigKey { get; set; }
        public string ConfigValue { get; set; }
        public bool? IsActive { get; set; }
    }
}
