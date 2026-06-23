using System;
using System.Collections.Generic;

namespace Nphies.Core.Data.Entities
{
    public partial class RcmRemittanceLog
    {
        public long LogId { get; set; }
        public long ClaimId { get; set; }
        public long? ServiceId { get; set; }
        public short? ActionType { get; set; }
        public int? ReasonId { get; set; }
        public DateTime CreatedOn { get; set; }
        public int? CreatedBy { get; set; }
        public DateTime? ModifedOn { get; set; }
        public int? ModifiedBy { get; set; }
        public bool? IsActive { get; set; }
        public string Remarks { get; set; }
        public int OrganizationId { get; set; }
    }
}
