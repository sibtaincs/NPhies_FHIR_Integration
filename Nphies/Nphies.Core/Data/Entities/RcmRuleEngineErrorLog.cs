using System;
using System.Collections.Generic;

namespace Nphies.Core.Data.Entities
{
    public partial class RcmRuleEngineErrorLog
    {
        public long ErrorLogId { get; set; }
        public string ErrorType { get; set; }
        public int? ErrorTypeId { get; set; }
        public int? OrganizationId { get; set; }
        public long ClaimId { get; set; }
        public string ErrorLog { get; set; }
        public bool? IsActive { get; set; }
        public DateTime CreatedOn { get; set; }
        public int CreatedBy { get; set; }
        public int? ModifiedBy { get; set; }
        public DateTime? ModifiedOn { get; set; }
        public string SeverityName { get; set; }
        public int? SeverityId { get; set; }
        public string SourceType { get; set; }
        public string RuleGroupName { get; set; }
        public int? RuleId { get; set; }
        public byte? SeverityLevelTypeId { get; set; }
        public string RuleIds { get; set; }
    }
}
