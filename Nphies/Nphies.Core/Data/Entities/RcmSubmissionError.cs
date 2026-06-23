using System;
using System.Collections.Generic;

namespace Nphies.Core.Data.Entities
{
    public partial class RcmSubmissionError
    {
        public long Id { get; set; }
        public int FacilityId { get; set; }
        public int OrganizationId { get; set; }
        public long? EncounterNo { get; set; }
        public byte? SubmissionType { get; set; }
        public string BatchReferenceNo { get; set; }
        public byte? ErrorType { get; set; }
        public string ErrorDetails { get; set; }
        public long? ReferenceNo { get; set; }
        public bool? IsActive { get; set; }
        public bool? IsDeleted { get; set; }
        public DateTime CreatedOn { get; set; }
        public int CreatedBy { get; set; }
        public DateTime? ModifiedOn { get; set; }
        public int? ModifiedBy { get; set; }
        public int? CompanyId { get; set; }
        public string PolicyNo { get; set; }
        public string BatchNo { get; set; }
    }
}
