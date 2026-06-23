using System;

namespace Nphies.Core.Models
{
    public static class AppCons
    {
        public static readonly string Active = "true";
        public static readonly string Inactive = "false";
        public static readonly string Male = "male";
        public static readonly string Female = "female";
        public static readonly string Unknown = "other";


    }

    public static class DoctorsInfo
    {
        public static readonly string RoleCode = "doctor";
        public static readonly string RoleDisplay = "Doctor";
        public static readonly string SpecialityCode = "08.26";
        public static readonly string SpecialityDisplay = "General Medicine";

    }

    public static class ClaimInfo
    {
        public static readonly string ORGType = "prov";
        public static readonly string claimTypeProfessional = "professional";
        public static readonly string claimTypeInstitutional = "institutional";
        public static readonly string claimTypeOral = "oral";
        public static readonly string claimTypePharmacy = "pharmacy";

    }
    public enum CommunicationStatus : byte
    {
        ResponseAdded = 1,
        Completed = 2,
        Rejected = 52
    }
    public enum AdaptorType : byte
    {
        Provider = 1,
        Payer = 2
    }

    public enum Enounter : byte
    {
        All = 0,
        Outpatient = 1,
        Inpatient = 2
    }
    public enum DoctorLicenseType : byte
    {
        SCFHS = 1,
        MOH = 2
    }

    
    public enum ClaimStatus : byte
    {
        New = 1,
        Pending = 2,
        Reviewed = 3,
        Error = 4,
        ReadyToSubmit = 5,
        Submitted = 6,
        SubmissionError = 7,
       // Settled = 8,
        //Accepted = 9,
       // Rejected = 10,
        PartialAccepted = 11,
        RejectedByReviewer = 12,
        WriteOff = 13,
        Completed = 14,
        Transaction = 15,
        Refund = 16,
        Return = 17,
        Cancelled = 18,
        Locked = 19,
        InternalError = 20,
        InternalProccess = 23,
        Nphies_Queued = 67,
        Nphies_Rejected = 71,
        Nphies_Complete = 9,
        Nphies_Completed = 9,
        Nphies_Approved = 9,
        Nphies_PartialApproved = 8,
        Nphies_Partial = 8,
        Nphies_Error = 52,
        Nphies_FurtherDetails = 59,
        Nphies_Perror = 51,
        Nphies_Pended = 93,
        Nphies_OutCome = 99,
        PendingForNphies = 100,
        InProgress = 103,
        Nphies_Failed = 107,
        Nphies_Success = 109,
        UpdateError = 113,
        Nphies_Cancelled = 127
    }
    public enum RecordType : byte
    {
        Original = 1,
        Modified = 2
    }

    public enum PayerCompany : byte
    {
        Bupa = 121
    }

   

    public enum NphiesFiltercriteria : byte
    {
        Ready_To_Submit_Claims = 1,
        Submit_Error_Resolved_Claims = 2,
        Submit_Nphies_TimeOut_Claims = 3,
        Submit_Payer_Error = 4,
        Submit_Naphies_Error_Claims = 5,
        ReSubmit_Nphies_Error_Claims = 6,
        ReProcess_With_Same_BundleId = 7,
        ReProcess_With_New_BundleId = 8,
        Cancel_Nphies_Process = 9
    }

    public static class DateFormate
    {
        public static string ToNaphiesFormat(this DateTime dateTime)
        {
            return dateTime.ToString("yyyy-MM-ddTHH:mm:ss.fffzzz");
        }


    }
    public enum ClaimMode
    {
        Long = 1,
        Referral = 2,
        Emergency = 3
    }

}
