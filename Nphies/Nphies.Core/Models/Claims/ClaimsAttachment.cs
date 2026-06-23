using Hl7.Fhir.Model;
using System.Collections.Generic;

namespace Nphies.Core.Models.Claims
{
    public class ClaimsAttachment
    {
        public int OrginzationId { get; set; }
        public int FacilityId { get; set; }
        public long ClaimId { get; set; }
        public string DocumentReferenceNo { get; set; }
    }
}
