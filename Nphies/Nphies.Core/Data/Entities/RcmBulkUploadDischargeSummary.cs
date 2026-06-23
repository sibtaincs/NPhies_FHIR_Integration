using System;
using System.Collections.Generic;

namespace Nphies.Core.Data.Entities
{
    public partial class RcmBulkUploadDischargeSummary
    {
        public int RcmdischargeSummaryId { get; set; }
        public int OrganizationId { get; set; }
        public int DischargeNo { get; set; }
        public DateTime DischargeDate { get; set; }
        public DateTime AdmissionDate { get; set; }
        public int EncounterNo { get; set; }
        public short ClinicId { get; set; }
        public int DoctorId { get; set; }
        public string FinalDiagnosis { get; set; }
        public string Persentation { get; set; }
        public string PastHistory { get; set; }
        public string PlanOfCare { get; set; }
        public string Investigations { get; set; }
        public string FollowupPlan { get; set; }
        public string ConditionOnDischarge { get; set; }
        public string SignificantFindings { get; set; }
        public string PlanedProcedure { get; set; }
        public string PatientCondition { get; set; }
        public int? DaysStayed { get; set; }
        public string Remarks { get; set; }
        public string Ercare { get; set; }
        public byte? Status { get; set; }
        public bool? IsActive { get; set; }
        public int CreatedBy { get; set; }
        public DateTime CreatedOn { get; set; }
        public int? EditedBy { get; set; }
        public DateTime? EditedOn { get; set; }
        public bool? IsPatientDied { get; set; }
        public string DischargeInstructions { get; set; }
        public string Reason { get; set; }
        public byte? DischargeDisposition { get; set; }
        public short? HospitalId { get; set; }
        public string RoomId { get; set; }
        public string BedId { get; set; }
        public string ClinicName { get; set; }
        public string DoctorName { get; set; }
        public bool? IsProcessed { get; set; }
        public DateTime? ReeivedOn { get; set; }
    }
}
