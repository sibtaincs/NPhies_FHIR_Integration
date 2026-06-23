using System;
using System.Collections.Generic;

namespace Nphies.Core.Data.Entities
{
    public partial class RcmClaimSubmissionResponse
    {
        public long Id { get; set; }
        public string ClaimBundleId { get; set; }
        public string ClaimIdentifier { get; set; }
        public string ResponseBundleId { get; set; }
        public decimal? ReceivedAmount { get; set; }
        public byte? Status { get; set; }
        public DateTime? ReceivedOn { get; set; }
        public long? ClaimId { get; set; }
        public int? FacilityId { get; set; }
        public int? OrganizationId { get; set; }
        public bool? IsActive { get; set; }
        public decimal? SubmittedAmount { get; set; }

    }
}
