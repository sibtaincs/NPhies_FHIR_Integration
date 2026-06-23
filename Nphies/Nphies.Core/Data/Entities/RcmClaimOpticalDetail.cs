using System;
using System.Collections.Generic;

namespace Nphies.Core.Data.Entities
{
    public partial class RcmClaimOpticalDetail
    {
        public long ClaimId { get; set; }
        public int OrganizationId { get; set; }
        public byte? RegularLensesType { get; set; }
        public byte? LensSpecifications { get; set; }
        public byte? ContactLenses { get; set; }
        public byte? FramesLensIndicator { get; set; }
        public byte? NumberOfPairs { get; set; }

        public virtual RcmClaim RcmClaim { get; set; }
    }
}
