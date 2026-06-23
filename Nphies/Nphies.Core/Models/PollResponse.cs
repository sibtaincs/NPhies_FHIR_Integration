using System;
using System.Collections.Generic;

namespace Nphies.Core.Models
{
    public class PollResponse
    {
        public string ClaimIdentifier { get; set; }
        public string InvoiceNo { get; set; }
        public string TotalSubmitted { get; set; }
        public string TotalApproved { get; set; }
        public string Status { get; set; }
        public string Outcome { get; set; }
        public string Disposition { get; set; }
        public object ResponseBundle { get; set; }
        public string ResponseBundleID { get; set; }
        public string ClaimReason { get; set; }
        public string CommunicationUrl { get; set; }
        public string CommunicationIdentifier { get; set; }
        public List<ClaimItemResponse> ItemsResponse { get; set; }
        public bool? IsPended { get; set; }
    }

    public class ClaimItemResponse
    {
        public int ItemSequence { get; set; }
        public string Status { get; set; }
        public string ItemReason { get; set; }
        public string ReasonCode { get; set; }
        public string ApprovedQuantity { get; set; }
        public string TaxApproved { get; set; }
        public string SubmittedAmount { get; set; }
        public string BenefitAmount { get; set; }
        public string InvoiceNo { get; set; }
    }
}
