using System;
using System.Collections.Generic;

namespace Nphies.Core.Data.Entities
{
    public partial class ReResultSeverityLevel
    {
        public ReResultSeverityLevel()
        {
            ReRuleResults = new HashSet<ReRuleResult>();
        }

        public int SeverityId { get; set; }
        public string SeverityName { get; set; }
        public bool? IsActive { get; set; }

        public virtual ICollection<ReRuleResult> ReRuleResults { get; set; }
    }
}
