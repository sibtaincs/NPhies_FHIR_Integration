using System;
using System.Collections.Generic;

namespace Nphies.Core.Data.Entities
{
    public partial class RcmVitalSignDetailsTemp
    {
        public long VitalSignDetailId { get; set; }
        public int OrganizationId { get; set; }
        public int UserId { get; set; }
        public int EncounterNo { get; set; }
        public string MainSymptoms { get; set; }
        public string BloodPressure { get; set; }
        public decimal? Height { get; set; }
        public decimal? Temperature { get; set; }
        public decimal? Weight { get; set; }
        public int? Pulse { get; set; }
        public string PhysicianNotesConditions { get; set; }
        public string SignificantSigns { get; set; }
        public string BodyMassIndex { get; set; }
        public string VitalSignCreatedon { get; set; }
        public bool? IsProcessed { get; set; }
        public int CreatedBy { get; set; }
        public DateTime CreatedOn { get; set; }
        public int ModifiedBy { get; set; }
        public DateTime? ModifiedOn { get; set; }
        public int FacilityId { get; set; }
        public DateTime? ReceivedOn { get; set; }
    }
}
