using System;
using System.Collections.Generic;

namespace Nphies.Core.Data.Entities
{
    public partial class RcmClaimDentalDetail
    {
        public long ClaimId { get; set; }
        public int OrganizationId { get; set; }
        public long ServiceId { get; set; }
        public short ServiceLineitemNo { get; set; }
        public string ToothNumber { get; set; }
        public byte? ToothSurface { get; set; }
        public short LineItemNo { get; set; }
    }
}
