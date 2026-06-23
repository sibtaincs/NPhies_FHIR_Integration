using Hl7.Fhir.Model;
using System.Collections.Generic;
using System.Linq;

namespace Nphies.Core.Models.Submission.SameBundles.Claims
{
    public class Claim
    {
        public long ClaimId { get; set; }
        public string ClaimIdentifier { get; set; }
        public string ClaimBundleId { get; set; }


    }

    public class ClaimCancelation
    {
        public string PayerLicense { get; set; }
        public string TPALicense { get; set; }
        public List<Nphies.Core.Models.Submission.SameBundles.Claims.Claim> Claims { get; set; }
    }
}
