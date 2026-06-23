using System;
using System.Collections.Generic;

namespace Nphies.Core.Data.Entities
{
    public partial class ReRuleGroup
    {
        public ReRuleGroup()
        {
            RePayerEventRuleGroups = new HashSet<RePayerEventRuleGroup>();
            ReRuleFieldParameterGroups = new HashSet<ReRuleFieldParameterGroup>();
            ReRules = new HashSet<ReRule>();
        }

        public int RuleGroupId { get; set; }
        public string RuleGroupName { get; set; }
        public string Remarks { get; set; }
        public bool? IsActive { get; set; }
        public int? CreatedBy { get; set; }
        public DateTime? CreatedOn { get; set; }
        public int? ModifiedBy { get; set; }
        public DateTime? ModifiedOn { get; set; }
        public int? OrganizationId { get; set; }

        public virtual ICollection<RePayerEventRuleGroup> RePayerEventRuleGroups { get; set; }
        public virtual ICollection<ReRuleFieldParameterGroup> ReRuleFieldParameterGroups { get; set; }
        public virtual ICollection<ReRule> ReRules { get; set; }
    }
}
