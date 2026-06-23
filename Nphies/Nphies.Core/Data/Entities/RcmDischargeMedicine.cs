using System;
using System.Collections.Generic;

namespace Nphies.Core.Data.Entities
{
    public partial class RcmDischargeMedicine
    {
        public int RcmdischargeMedicineId { get; set; }
        public int OrganizationId { get; set; }
        public long ClaimId { get; set; }
        public int EncounterNo { get; set; }
        public int MedicineCode { get; set; }
        public int DischargeNo { get; set; }
        public string MedicineDescription { get; set; }
        public string GenericName { get; set; }
        public short LineItemNo { get; set; }
        public DateTime StartDate { get; set; }
        public double DoseDailyQuantity { get; set; }
        public byte? DoseDailyUnitId { get; set; }
        public byte DoseDurationDays { get; set; }
        public byte? DoseTimingId { get; set; }
        public byte FrequencyId { get; set; }
        public byte? RouteId { get; set; }
        public int? Refill { get; set; }
        public string Remarks { get; set; }
        public byte Status { get; set; }
        public int CreatedBy { get; set; }
        public DateTime CreatedOn { get; set; }
        public int? EditedBy { get; set; }
        public DateTime? EditedOn { get; set; }
        public bool? IsMedicineApproved { get; set; }
        public int? BoxQuantity { get; set; }
        public string PharmacistRemarks { get; set; }
        public byte? PrescriptionType { get; set; }
        public bool? IsActive { get; set; }
        public string DoseDetail { get; set; }
    }
}
