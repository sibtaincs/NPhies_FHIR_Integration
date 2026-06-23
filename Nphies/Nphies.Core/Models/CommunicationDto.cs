using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Cyclus.Storage.Attachment.Enums;

namespace Nphies.Core.Models
{
    public class CommunicationDto
    {
        public long ClaimId { get; set; }
        public string ClaimIdentifier { get; set; }
        public string License { get; set; }
        public string TpaNhicId { get; set; }
        public string Remarks { get; set; }
        public int ClaimItemSequence { get; set; }
        public List<Payload> payloads { get; set; }
        public Patient patient { get; set; }
        public bool IsMohClaim { get; set; }
    }

    public class ClaimAttachmentCommunicationDto
    {
        public string ClaimIdentifier { get; set; }
        public long ClaimID { get; set; }
        public string DocumentReferenceId { get; set; }
        public string License { get; set; }
        public string TpaNhicId { get; set; }
        public Patient Patient { get; set; }
        public string EncounterIdentifier { get; set; }
        public string OriginalEncounter { get; set; }
        public byte EncounterType { get; set; }
        public int ClinicId { get; set; }
        public FileStorageProviderType FileStorageProviderType { get; set; }
    }
    public class AttachmentDto
    {
        public string remarks { get; set; }
        public List<Payload> payloads { get; set; }
    }

    public class ResubmissionStatusDto
    {
        public int OrganizationId { get; set; }
        public int FacilityId { get; set; }
        public string BundleId { get; set; }
        public string ParsedbjectId { get; set; }
        public long ClaimNo { get; set; }
        public string ClaimIdentifier { get; set; }
        public string Status { get; set; }
        public string Remarks { get; set; }
        public string Errors { get; set; }
        public int NphiesSequenceNo { get; set; }
    }

    public class ClaimDto
    {
        public int OrganizationId { get; set; }
        public int facilityId { get; set; }
        public long ClaimId { get; set; }
        public string ClaimIdentifier { get; set; }
        public int PayerId { get; set; }
        public int? PayerPolicyId { get; set; }
        public string PatientFileNumber { get; set; }
        public int? PatientIdentificationNo { get; set; }
        public string NationalityId { get; set; }
        public string PatientName { get; set; }
        public string PatientSurnameFamilyName { get; set; }
        public string PatientMobileNo { get; set; }
        public DateTime? PatientDob { get; set; }
        public byte? PatientGender { get; set; }
        public string Address1 { get; set; }
        public string PatientNationality { get; set; }
        public string MaritalStatus { get; set; }
        public int EncounterType { get; set; }
        public string EncounterNo { get; set; }
        public string OriginalEncounter { get; set; }
        public string DocdocumentReferenceId { get; set; }
        public int ClaimItemSequence { get; set; }
        public long RowId { get; set; }
        public string ClinicId { get; set; }
        public bool IsMohClaim { get; set; }
    }

}
