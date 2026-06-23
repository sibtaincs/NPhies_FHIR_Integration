namespace Nphies.Core.Models
{
        public class DocumentResponse
        {
            public string pdf { get; set; }
            public byte[] byteFile { get; set; }
        }

       public class Vida3Request
       {
           public string setupid { get; set; }
           public string project { get; set; }
           public int? encounterNo { get; set; }
           public byte? fileStorageProvider { get; set; }

    }
    public class Vida4Request
       {
           public string ClaimIdentifier { get; set; }
           public string encounterType { get; set; }
           public long encounterNumber { get; set; }
           public string OriginalEncounterNumber { get; set; }
        public byte? claimStatus { get; set; }
    }
    public enum Status : byte
    {

        New = 1, // New Submited claim for current date 
        Pending = 2, // New Submited Claim which are before today
        Reviewed = 3, // Ready to Submit
        Error = 4,
        ReadyToSubmit = 5,// If reviewed then Posting job will get data based on this status
        Submitted = 6, // If claim is submited 
        SubmissionError = 7,
        //Settled = 8,
        //Accepted = 9,
        Rejected = 10,
        PartialAccepted = 11,
        RejectedByReviewer = 12,
        WriteOff = 13,
        Completed = 14,
        Transaction = 15,
        Refund = 16,
        Return = 17,
        Cancelled = 18,
        Locked = 19, // This Status will be use to
        InternalError = 20,
        InternalProccess = 23,
        PendingForNphies = 100,
        Nphies_Queued = 67,
        Nphies_Pended = 93,
        Nphies_Rejected = 71,
        Nphies_Approved = 9,
        Nphies_PartialApproved = 8,
        Nphies_Error = 52,
        Nphies_FurtherDetails = 59,
        Nphies_Perror = 51,
        Nphies_OutCome = 99,
        Nphies_Cancelled = 127,
        InProgress = 103,
        UpdateError = 113,


    }



}
