using System;

namespace Nphies.Core.Data.Entities
{
    public partial class RcmLabResult
    {
        public long Id { get; set; } // BIGINT
        public int FacilityId { get; set; }
        public long ClaimId { get; set; } // BIGINT
        public string? ServiceCode { get; set; } // NVARCHAR(100)
        public string? ServiceReferenceNumber { get; set; } // NVARCHAR(100)
        public int? TestId { get; set; }
        public string? LabResult { get; set; } // NVARCHAR(500)
        public int? CreatedBy { get; set; }
        public DateTime? CreatedOn { get; set; }
        public int? ModifiedBy { get; set; }
        public DateTime? ModifiedOn { get; set; }
        public bool? IsActive { get; set; }
    }
}
