using System.Collections.Generic;
using Hl7.Fhir.Model;

namespace Nphies.Core.Models.Claims
{
    public class UpdateClaimAndServicesSeqRequest
    {
        public string ClaimbatchID { get; set; }
        public long ClaimID { get; set; }
        public string MessageBundleID { get; set; }
        public string ClaimIdentifier { get; set; }
        public string ClaimBundleIdentifier { get; set; }
        public decimal ClaimTotal { get; set; }
        public List<ClaimServices> ClaimServices { get; set; }
    }

    public class ClaimServices
    {
        public int RowId { get; set; }
        public int Seqno { get; set; }
        public decimal NET { get; set; }
        public decimal Quantity { get; set; }
       
    }
}
