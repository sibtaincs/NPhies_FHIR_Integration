using System;
using System.Collections.Generic;

namespace Nphies.Core.Data.Entities
{
    public partial class RcmBulkUploadClaim
    {
        public long BulkUploadClaimId { get; set; }
        public long BulkUploadProcessLogId { get; set; }
        public int OrganizationId { get; set; }
        public byte SubmissionType { get; set; }
        public string EligibilityCode { get; set; }
        public string FacilityId { get; set; }
        public int UserId { get; set; }
        public string VisitType { get; set; }
        public string Remarks { get; set; }
        public string ReferenceNo { get; set; }
        public string PolicyNumber { get; set; }
        public DateTime? EncounterStartTime { get; set; }
        public DateTime? InvoiceDate { get; set; }
        public byte? ClaimType { get; set; }
        public int EncounterNo { get; set; }
        public DateTime? EncounterEndTime { get; set; }
        public string BedNo { get; set; }
        public string RoomNo { get; set; }
        public string PayerId { get; set; }
        public string PreAuthNo { get; set; }
        public string DoctorId { get; set; }
        public string ClinicId { get; set; }
        public byte? EncounterType { get; set; }
        public int? ErrorId { get; set; }
        public string ErrorDescription { get; set; }
        public bool? IsValidated { get; set; }
        public bool? IsProcessed { get; set; }
        public bool? IsAnyError { get; set; }
        public bool? IsValidData { get; set; }
        public int CreatedBy { get; set; }
        public DateTime CreatedOn { get; set; }
        public int? ModifiedBy { get; set; }
        public DateTime? ModifiedOn { get; set; }
        public bool? IsDeleted { get; set; }
        public bool? IsSucceed { get; set; }
        public int? RcmfacilityId { get; set; }
        public int? RcmclinicId { get; set; }
        public int? RcmdoctorId { get; set; }
        public int? RcmpayerId { get; set; }
        public long? MidTableReferenceId { get; set; }
        public short? ErrorType { get; set; }
    }
}
