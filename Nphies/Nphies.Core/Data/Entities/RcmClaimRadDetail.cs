using System;
using System.Collections.Generic;

namespace Nphies.Core.Data.Entities
{
    public partial class RcmClaimRadDetail
    {
        public long ClaimId { get; set; }
        public int OrganizationId { get; set; }
        public long ServiceId { get; set; }
        public short ServiceLineItemNo { get; set; }
        public DateTime? RadiologyVisitDate { get; set; }
        public short LineItemNo { get; set; }
        public string ClinicalData { get; set; }
        public string RadiologyResult { get; set; }
    }
}
