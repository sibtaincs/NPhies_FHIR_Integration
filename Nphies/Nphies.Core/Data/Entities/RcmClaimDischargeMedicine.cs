using System;
using System.Collections.Generic;

namespace Nphies.Core.Data.Entities
{
    public partial class RcmClaimDischargeMedicine
    {
        public int ClaimDischargeMedicineId { get; set; }
        public int OrganizationId { get; set; }
        public int FacilityId { get; set; }
        public long ClaimId { get; set; }
        public int EncounterNo { get; set; }
        public int MedicineCode { get; set; }
        public int DischargeNo { get; set; }
        public string Description { get; set; }
        public string GenericName { get; set; }
        public double DoseDailyQuantity { get; set; }
        public byte? DoseDailyUnitId { get; set; }
        public byte DoseDurationDays { get; set; }
        public byte? DoseTimingId { get; set; }
        public byte FrequencyId { get; set; }
        public byte? RouteId { get; set; }
        public string Remarks { get; set; }
        public DateTime? ReceivedOn { get; set; }
        public bool? IsActive { get; set; }
        public string DoseDetail { get; set; }
        public string Direction { get; set; }
        public int CreatedBy { get; set; }
        public DateTime CreatedOn { get; set; }
        public int? EditedBy { get; set; }
        public DateTime? EditedOn { get; set; }
    }
}
