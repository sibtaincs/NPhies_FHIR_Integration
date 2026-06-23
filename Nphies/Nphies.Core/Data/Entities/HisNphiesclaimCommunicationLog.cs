using System;
using System.Collections.Generic;

#nullable disable

namespace Nphies.Core.Data.Entities
{
    public partial class HisNphiesclaimCommunicationLog
    {
        public int OrganizationId { get; set; }
        public int FacilityId { get; set; }
        public int ServiceReferenceNo { get; set; }
        public string ClaimIdentifier { get; set; }
        public short LineItemNo { get; set; }
        public string IdentifierSystem { get; set; }
        public string IdentifierValue { get; set; }
        public string Reason { get; set; }
        public string RequestBundleId { get; set; }
        public string ResponseBundleId { get; set; }
        public int CreatedBy { get; set; }
        public DateTime CreatedOn { get; set; }
        public int? EditedBy { get; set; }
        public DateTime? EditedOn { get; set; }
        public byte[] RowVer { get; set; }
    }
}
