using System;

namespace Nphies.Core.Data.Entities
{
    public partial class CoDskEncounter
    {
        public int OrganizationId { get; set; }
        public int FacilityId { get; set; }
        public byte EncounterType { get; set; }
        public string EncounterNo { get; set; }
        public string FacilityType { get; set; }
        public long? ClaimId { get; set; }
        public string State { get; set; }
        public string PatientName { get; set; }
        public string MedicalRecordNumber { get; set; }
        public DateTime AdmissionDate { get; set; }
        public DateTime? DischargeDate { get; set; }
        public string DischargeStatus { get; set; }
        public DateTime? BirthDate { get; set; }
        public double? BirthWeight { get; set; }
        public byte? AgeInYears { get; set; }
        public int? AgeInDays { get; set; }
        public string Gender { get; set; }
        public bool? SameDayField { get; set; }
        public string AdmissionType { get; set; }
        public int? PayerId { get; set; }
        public int? PayerPolicyId { get; set; }
        public int? PayerPolicyClassId { get; set; }
        public string PolicyNumber { get; set; }
        public string MembershipNo { get; set; }
        public DateTime? InsuranceExpiryDate { get; set; }
        public int? AssignedTo { get; set; }
        public byte? Status { get; set; }
        public int? CodedBy { get; set; }
        public DateTime? CodedOn { get; set; }
        public int? ApprovedBy { get; set; }
        public DateTime? ApprovedOn { get; set; }
        public double? OriginalInvoiceAmount { get; set; }
        public double? DrginvoicedAmount { get; set; }
        public short? TotalLeaveDays { get; set; }
        public string MentalHealthLegalStatus { get; set; }
        public string PatientIdentificationType { get; set; }
        public string PatientIdentificationNo { get; set; }
        public string EhealthId { get; set; }
        public string AdmittedClinicName { get; set; }
        public string MrpdoctorName { get; set; }
        public int CreatedBy { get; set; }
        public DateTime CreatedOn { get; set; }
        public int? ModifiedBy { get; set; }
        public DateTime? ModifiedOn { get; set; }
        public int? RejectedBy { get; set; }
        public DateTime? RejectedOn { get; set; }
        public string RejectedRemarks { get; set; }
        public bool? UpdatedBySupervisor { get; set; }
    }
}
