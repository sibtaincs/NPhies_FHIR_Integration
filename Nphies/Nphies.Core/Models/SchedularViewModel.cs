using System;

namespace Nphies.Core.Models
{
    public class ProcessQueueModel
    {
        public int OrginzationId { get; set; }
        public int FacilityId { get; set; }
        public long ProcessId { get; set; }

    }
    public class NphiesprocessQueueModel
    {
        public long ProcessId { get; set; }
        public int OrganizationId { get; set; }
        public int FacilityId { get; set; }
        public string ProjectId { get; set; }
        public string SetupId { get; set; }
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
        //public bool? IsSubmissionByEpisode { get; set; }
        public int? Limit { get; set; } = 100;
        public long TotalClaim { get; set; }
        public bool? IncludePreAuthRef { get; set; }
        public bool SubmittedSingleClaim { get; set; } = false;
        public int? AdapterId { get; set; }
        public bool IsRegular { get; set; }
    }

}
