using System;
using System.Collections.Generic;

namespace Nphies.Core.Data.Entities
{
    public partial class RcmNphiesprocessQueue
    {
        public long ProcessId { get; set; }
        public int OrganizationId { get; set; }
        public int FacilityId { get; set; }
        public byte CriteriaType { get; set; }
        public int? ClaimNo { get; set; }
        public int? PayerId { get; set; }
        public int? PolicyId { get; set; }
        public DateTime? FromDate { get; set; }
        public DateTime? ToDate { get; set; }
        public bool? IsProcessed { get; set; }
        public int CreatedBy { get; set; }
        public DateTime CreatedOn { get; set; }
        public int? ProcessedBy { get; set; }
        public DateTime? ProcessedOn { get; set; }
        public int? Limit { get; set; }
        public int? TotalClaim { get; set; }
        public int? TotalError { get; set; }
        public int? TotalQueued { get; set; }
        public int? TotalTimout { get; set; }
        public int? TotalPended { get; set; }
        public int? TotalPError { get; set; }
        public bool? IncludePreAuthRef { get; set; }
        public int? VerifiedBy { get; set; }
        public DateTime? VerifiedOn { get; set; }
        public bool? SubmittedSingleClaim { get; set; }
        public bool? IsRunningProcess { get; set; }
        public bool? IsQueueProcessed { get; set; }
        public int? AdapterId { get; set; }
    }
}
