using System;
using System.Collections.Generic;

namespace Nphies.Core.Data.Entities
{
    public partial class RcmClaimMedicalInfoDataDischargeSummary
    {
        public long ClaimMedicalInfoId { get; set; }
        public byte MedicalRecordsType { get; set; }
        public string ParameterName { get; set; }
        public string ParameterValue { get; set; }
        public int CreatedBy { get; set; }
        public DateTime CreatedOn { get; set; }
        public int? ModifiedBy { get; set; }
        public DateTime? ModifiedOn { get; set; }
        public bool? IsActive { get; set; }
        public string EncounterNo { get; set; }
        public int FacilityId { get; set; }
        public int? OrganizationId { get; set; }
        public DateTime? ReceivedOn { get; set; }
        public long ClaimId { get; set; }
        public long? ReferenceId { get; set; }
    }
}
