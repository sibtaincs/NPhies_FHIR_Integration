using System;
using System.Collections.Generic;

namespace Nphies.Core.Data.Entities
{
    public partial class ReRule
    {
        public ReRule()
        {
            ReRuleExpressions = new HashSet<ReRuleExpression>();
            ReRuleResults = new HashSet<ReRuleResult>();
        }

        public int RuleGroupId { get; set; }
        public int RuleId { get; set; }
        public string RuleName { get; set; }
        public string RuleDescription { get; set; }
        public int? ExecutionOrder { get; set; }
        public bool? IsActive { get; set; }
        public int? CreatedBy { get; set; }
        public DateTime? CreatedOn { get; set; }
        public int? ModifiedBy { get; set; }
        public DateTime? ModifiedOn { get; set; }
        public int? RuleType { get; set; }

        public virtual ReRuleGroup RuleGroup { get; set; }
        public virtual ICollection<ReRuleExpression> ReRuleExpressions { get; set; }
        public virtual ICollection<ReRuleResult> ReRuleResults { get; set; }
    }
}
