using System;
using System.Collections.Generic;

namespace Nphies.Core.Data.Entities
{
    public partial class RcmBulkUploadClaimPatientDetail
    {
        public long BulkUploadClaimPatientDetailId { get; set; }
        public long BulkUploadProcessLogId { get; set; }
        public int OrganizationId { get; set; }
        public int UserId { get; set; }
        public int EncounterNo { get; set; }
        public string PatientFileNumber { get; set; }
        public string PatientMembershipNumber { get; set; }
        public string PatientName { get; set; }
        public string PatientSurnameFamilyName { get; set; }
        public DateTime? PatientDob { get; set; }
        public string MaritalStatus { get; set; }
        public string PatientGender { get; set; }
        public string NationalityId { get; set; }
        public string PatientNationality { get; set; }
        public string PatientMobileNo { get; set; }
        public bool? IsAnyError { get; set; }
        public int CreatedBy { get; set; }
        public DateTime CreatedOn { get; set; }
        public int ModifiedBy { get; set; }
        public DateTime? ModifiedOn { get; set; }
        public bool? IsDeleted { get; set; }
        public int? ErrorId { get; set; }
        public string ErrorDescription { get; set; }
        public bool? IsValidated { get; set; }
        public bool? IsProcessed { get; set; }
        public long? MidTableReferenceId { get; set; }
    }
}
