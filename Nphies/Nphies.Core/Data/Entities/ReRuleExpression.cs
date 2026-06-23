using System;
using System.Collections.Generic;

namespace Nphies.Core.Data.Entities
{
    public partial class ReRuleExpression
    {
        public int RuleExpressionId { get; set; }
        public int RuleId { get; set; }
        public string BracketFirst { get; set; }
        public string Field { get; set; }
        public bool? FieldType { get; set; }
        public string Operator { get; set; }
        public string Value { get; set; }
        public bool? ValueType { get; set; }
        public string BracketLast { get; set; }
        public string Condition { get; set; }
        public int? CreatedBy { get; set; }
        public DateTime? CreatedOn { get; set; }
        public int? ModifiedBy { get; set; }
        public DateTime? ModifiedOn { get; set; }
        public string Expression { get; set; }
        public bool? IsActive { get; set; }

        public virtual ReRule Rule { get; set; }
    }
}
