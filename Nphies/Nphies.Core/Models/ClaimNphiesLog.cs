using MongoDB.Bson.Serialization.Attributes;
using System;
using System.Collections.Generic;

namespace Nphies.Core.Models
{
    public class ClaimBatchModelMD
    {
        public ClaimBatchModelMD()
        {
            CreatedOn = DateTime.Now;
        }

        [BsonId]
        public Guid Id { get; set; }
        public string BatchID { get; set; }

        [BsonElement("Requestbatch")]
        public string Requestbatch { get; set; }
        public string Responsebatch { get; set; }
        public DateTime CreatedOn { get; set; }

    }
    public class ClaimRequestModelMD
    {
        public ClaimRequestModelMD()
        {
            CreatedOn = DateTime.Now;
            pollRequestResponse = new ClaimPollResponseMD();
            friendlyViewer = new FriendlyViewRequestModel();
        }
        [BsonId]
        public Guid Id { get; set; }

        [BsonElement("Requestjson")]
        public string Requestjson { get; set; }
        public string ResponseJson { get; set; }
        public string Status { get; set; }
        public int ClaimNo { get; set; }
        public int EncounterNo { get; set; }
        public string Invoices { get; set; }
        public DateTime CreatedOn { get; set; }
        public DateTime ModifiedOn { get; set; }

        public ClaimPollResponseMD pollRequestResponse { get; set; }
        public List<ClaimCommunicationsMD> communications { get; set; }
        public FriendlyViewRequestModel friendlyViewer { get; set; }
        public int FacilityId { get; set; }

    }
    public class ClaimCommunicationsMD
    {
        public ClaimCommunicationsMD()
        {
            CreatedOn = DateTime.Now;//.ToString("yyyy-MM-ddTHH:mm:ss");
        }
        [BsonId]  // _id BatchID
        public Guid Id { get; set; }

        public string ClaimID { get; set; }
        [BsonElement("Request")]
        public string Request { get; set; }
        public string Response { get; set; }
        public bool IsSolicit { get; set; }
        public DateTime CreatedOn { get; set; }

    }
    public class ClaimPollResponseMD
    {
        public ClaimPollResponseMD()
        {
            CreatedOn = DateTime.Now;
        }
        [BsonId]  
        public Guid Id { get; set; }
        [BsonElement("Response")]
        public string Response { get; set; }
        public string Status { get; set; }
        public string ResponseBundleID { get; set; }
        public string ndResponse { get; set; }
        public List<MultipleResponse> multipleResponse { get; set; }
        public DateTime CreatedOn { get; set; }
        public bool isPended { get; set; }
        public int ProcessingStatus { get; set; }
        public string Remarks { get; set; }
        public int FacilityId { get; set; }

    }
    public class MultipleResponse
    {
        public MultipleResponse()
        {
            CreatedOn = DateTime.Now;
        }
        public DateTime CreatedOn { get; set; }
        public int ProcStatus { get; set; }

        public string Response { get; set; }

    }

    public class ClaimNphiesLog
    {
    }
}
