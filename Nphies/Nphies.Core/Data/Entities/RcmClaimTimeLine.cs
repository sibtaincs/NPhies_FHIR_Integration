using System;
using System.Collections.Generic;

namespace Nphies.Core.Data.Entities
{
    public partial class RcmClaimTimeLine
    {
        public int Id { get; set; }
        public long ClaimId { get; set; }
        public int OrganizationId { get; set; }
        public int UserId { get; set; }
        public byte TechnicalStatus { get; set; }
        public byte MedicalStatus { get; set; }
        public byte ClaimStatus { get; set; }
        public DateTime CreatedOn { get; set; }
        public int CreatedBy { get; set; }
        public DateTime? ReviewedOn { get; set; }
        public DateTime? VerifiedOn { get; set; }
        public DateTime? ApprovedOn { get; set; }
        public DateTime? RejectedOn { get; set; }
    }
}
