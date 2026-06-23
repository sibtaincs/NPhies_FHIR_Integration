using System;
using System.Collections.Generic;

namespace Nphies.Core.Data.Entities
{
    public partial class ReRuleFieldParameter
    {
        public ReRuleFieldParameter()
        {
            ReRuleFieldParameterGroups = new HashSet<ReRuleFieldParameterGroup>();
        }

        public int RuleFieldId { get; set; }
        public string RuleFieldName { get; set; }
        public string Remarks { get; set; }
        public bool? IsActive { get; set; }
        public int? CreatedBy { get; set; }
        public DateTime? CreatedOn { get; set; }
        public int? ModifiedBy { get; set; }
        public DateTime? ModifiedOn { get; set; }

        public virtual ICollection<ReRuleFieldParameterGroup> ReRuleFieldParameterGroups { get; set; }
    }
}
