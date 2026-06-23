using System;
using System.Collections.Generic;

namespace Nphies.Core.Data.Entities
{
    public partial class RcmClaimMedicalInfoDetail
    {
        public long MedicalInfoDetailsId { get; set; }
        public long MedicalInfoId { get; set; }
        public long ClaimId { get; set; }
        public string ParameterName { get; set; }
        public string ParameterValue { get; set; }
        public int CreatedBy { get; set; }
        public DateTime CreatedOn { get; set; }
        public int? ModifiedBy { get; set; }
        public DateTime? ModifiedOn { get; set; }
        public bool? IsActive { get; set; }
        public bool? IsDeleted { get; set; }
    }
}
