using System;
using System.Collections.Generic;

namespace Nphies.Core.Data.Entities
{
    public partial class RePayerEventRuleGroup
    {
        public int PayerEventsRuleGroupId { get; set; }
        public int PayerId { get; set; }
        public int OrganizationId { get; set; }
        public int EventId { get; set; }
        public int RuleGroupId { get; set; }
        public string EntityName { get; set; }
        public bool? IsActive { get; set; }

        public virtual RcmPayer RcmPayer { get; set; }
        public virtual ReRuleGroup RuleGroup { get; set; }
    }
}
