using System;
using System.Collections.Generic;

namespace Nphies.Core.Models
{

    public class ClaimResponseRequest
    {
        public long ProcessId { get; set; }
        public int? CriteriaType { get; set; }
        public int SubmitedBy { get; set; }
        public bool IsDrgCodeAttached { get; set; } = false;
        public ClaimRequestModel ClaimRequestModel { get; set; }
        public List<BatchResponse> ClaimResponseModel { get; set; }
        public List<NphiesSeqUpdate> UpdatedServices { get; set; }
        //public List<ClaimResponseModel> ClaimResponseModel { get; set; }
    }

    public class UpdateClaimStatusResponse
    {
       
        public string MessageBundleID { get; set; }
        public string ClaimbatchIdentifier { get; set; }
        public string ClaimBundleIdentifier { get; set; }
        public string ClaimIdentifier { get; set; }
        public string ClaimBundleId { get; set; }
        public long ProcessId { get; set; }
        public int? CriteriaType { get; set; }
        public long ClaimId { get; set; }
        public decimal ClaimTotal { get; set; }
        public int SubmitedBy { get; set; }
        public bool IsDrgCodeAttached { get; set; } = false;

        public List<NphiesSeqUpdate> UpdatedServices { get; set; }
    }
    public class NphiesSeqUpdate
    {
        public long ClaimId { get; set; }
        public int RowId { get; set; }
        public int Seqno { get; set; }
        public float Quantity { get; set; }
        public decimal NET { get; set; }
    }

    public class NphiesPostTrailDto
    {
        public long ClaimId { get; set; }
        public string ClaimIdentifier { get; set; }
        public string ClaimBundleId { get; set; }
        public long? ProcessId { get; set; }
        public int? CriteriaType { get; set; }
        public int OrganizationId { get; set; }
    }

    public class ClaimStatusUpdateDto
    {
        public long ClaimId { get; set; }
        public string errorMessage { get; set; }
        public int Status { get; set; }
    }
    public class LabReportDetailResponse
    {
        public LabReportDetail[] data { get; set; }
        public string responseMessage { get; set; }
        public string responseDescription { get; set; }
        public int responseCode { get; set; }
    }

    public class LabReportDetail
    {
        public int testId { get; set; }
        public string labResult { get; set; }
        public string serviceCode { get; set; }
    }
    public class InvoiceInfoDetailModel
    {
        public string PerformedBranch { get; set; }
        public DateTime InvoiceDate { get; set; }
        public string InvoiceNo { get; set; }
    }
    public class LabReportDetailRequest
    {
        public int EncounterType { get; set; }
        public int EncounterNumber { get; set; }
        public int PatientId { get; set; }
        public string FacilityGroup { get; set; }
        public int FacilityId { get; set; }
        public string InvoiceNumber { get; set; }
        public string ServiceCode { get; set; }
    }

}
