using System;

namespace Nphies.Core.Data.Entities
{
    public partial class RcmClaimNphiesPostTrail
    {
        public long Id { get; set; }
        public long ClaimId { get; set; }
        public string ClaimIdentifier { get; set; }
        public string ClaimBundleId { get; set; }
        public string PreviousClaimIdentifier { get; set; }
        public string PrevoiusClaimBundleId { get; set; }
        public int? OrganizationId { get; set; }
        public int SequenceNumber { get; set; }
        public long? ProcessId { get; set; }
        public int? CriteriaType { get; set; }
        public int CreatedBy { get; set; }
        public DateTime CreatedOn { get; set; }
        public byte[] Rowversion { get; set; }
        public string NphiesStatus { get; set; }
        public int? ClaimStatus { get; set; } // Nullable because SQL int usually defaults to null unless specified NOT NULL
        public string AdjucationOutcome { get; set; }
        public string RequestMessageId { get; set; }
        public string ResponseId { get; set; }
        public string ResponseMessageId { get; set; }
        public string ResponseIdentifier { get; set; }

    }
}
