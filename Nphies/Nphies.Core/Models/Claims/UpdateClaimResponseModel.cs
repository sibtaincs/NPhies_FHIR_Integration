using System.Collections.Generic;

namespace Nphies.Core.Models.Claims
{
    public class UpdateClaimResponseModel
    {       
        public List<ClaimServices> ClaimServices { get; set; }
        public ClaimResponseDetail ClaimDetailResponse { get; set; }
    }
    public class ClaimResponseDetail
    {
        public long ClaimId { get; set; }
        public int CriteriaType { get; set; }
        public decimal ClaimTotal { get; set; }
        public bool IsPended { get; set; }
        public string Status { get; set; }
        public string Remarks { get; set; }

        public List<ClaimItemDetail> ClaimItemDetail { get; set; }
    }
}
