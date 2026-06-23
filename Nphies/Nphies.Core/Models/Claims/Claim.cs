using Hl7.Fhir.ElementModel.Types;
using Nphies.Core.Data.Entities;
using System.Collections.Generic;

namespace Nphies.Core.Models.Claims
{
    public class ClaimModel
    {
        public int OrganizationId { get; set; }
        public int FacilityId { get; set; }
        public int PayerId { get; set; }
        public long ClaimId { get; set; }

        public byte Status { get; set; }
        public int ProcessId { get; set; }
        public System.DateTime InvoiceDate { get; set; }
        public byte EncounterType { get; set; }
        public int EncounterNo { get; set; }
        public long? EpisodeId { get; set; }
        public string NphiesEligibilitySystem { get; set; }
        public string OfflineApproval { get; set; }
        public string NphiesApprovalIdentifier { get; set; }
        public string NphiesApprovalSystem { get; set; }
        public string NphiesApprovalAuthRef { get; set; }
        public  Models.Patients.Patient Patient { get; set; }
        public  VitalSigns.VitalSign VitalSign { get; set; }
        public Doctor Doctor { get; set; }
        public Clinic Clinic { get; set; }
        public  ICollection<Models.Diagnoses.ClaimDiagnosis> Diagnosis { get; set; }
        public  ICollection<Models.Services.Service> ServicesDetails { get; set; }
    }
    public class Doctor
    {
        public int DoctorId { get; set; }
        public string DoctorName { get; set; }
        public List<DoctorLicenses> Licenses { get; set; }
    }

    public class Clinic
    {
        public int ClinicId { get; set; }
        public string ClinicName { get; set; }
        public string Code { get; set; }
        public bool? IsDental { get; set; }
    }
    public class DoctorLicenses
    {

    }
}
