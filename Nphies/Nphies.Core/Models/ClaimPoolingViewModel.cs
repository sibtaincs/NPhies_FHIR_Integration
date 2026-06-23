using System.Collections.Generic;

namespace Nphies.Core.Models
{
    public class ClaimUpdateModel
    {
        public int OrganizationID { get; set; }
        public int FacilityId { get; set; }
        public string ClaimIdentifier { get; set; }
        public int Status { get; set; }
        public string Remarks { get; set; }
        public string CommunicationUrl { get; set; }
        public string CommunicationIdentifier { get; set; }
        public string TotalApproved { get; set; }
        public string TotalSubmitted { get; set; }
        public string ResponseBundleID { get; set; }
        public List<ClaimItems> ClaimItems { get; set; }
        public bool? IsPended { get; set; }
    }

    public class ClaimItems
    {

        public int ItemSequence { get; set; }
        public string ServiceCode { get; set; }
        public string Status { get; set; }
        public string SubmittedQuantity { get; set; }
        public string ApprovedQuantity { get; set; }
        public string ReasonCode { get; set; }
        public string ItemReason { get; set; }
        public string SubmittedAmount { get; set; }
        public string BenefitAmount { get; set; }
        public string PatientShare { get; set; }
        public string CompanyTax { get; set; }
        public string TaxApproved { get; set; }
        public string ServiceReferenceNo { get; set; }
    }

}
