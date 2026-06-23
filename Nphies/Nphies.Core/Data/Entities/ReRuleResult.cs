using System;
using System.Collections.Generic;

namespace Nphies.Core.Data.Entities
{
    public partial class ReRuleResult
    {
        public int RuleResultId { get; set; }
        public int RuleId { get; set; }
        public string ResultName { get; set; }
        public string ResultValue { get; set; }
        public int? CreatedBy { get; set; }
        public DateTime? CreatedOn { get; set; }
        public int? ModifiedBy { get; set; }
        public DateTime? ModifiedOn { get; set; }
        public int SeverityId { get; set; }
        public byte? SeverityLevelTypeId { get; set; }
        public bool? IsActive { get; set; }
        public string ResultField { get; set; }

        public virtual ReRule Rule { get; set; }
        public virtual ReResultSeverityLevel Severity { get; set; }
    }
}
