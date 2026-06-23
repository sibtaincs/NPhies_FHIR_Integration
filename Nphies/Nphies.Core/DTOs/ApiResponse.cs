namespace Nphies.Core.DTOs
{
    public class ApiResponseOnUpdate
    {
        public bool Success { get; set; }
        public string Message { get; set; }
    }
    public class EncounterMedicalDetail
    {
        public int OrganizationId { get; set; }
        public string FacilityGroupID { get; set; }
        public int FacilityID { get; set; }
        public long ClaimId { get; set; }
        public int DoctorId { get; set; }
        public byte EncounterType { get; set; }
        public string EncounterNo { get; set; }
        public string PatientMrn { get; set; }
        public string MedicalData { get; set; }
        public string ClientId { get; set; }
    }
}
