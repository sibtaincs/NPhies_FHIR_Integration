using System;
using System.Collections.Generic;

namespace Nphies.Core.Data.Entities
{
    public partial class RcmNphiesclaimRejectionRemark
    {
        public RcmNphiesclaimRejectionRemark()
        {
            RcmNphiesclaimRejectionAttachmentsReferences = new HashSet<RcmNphiesclaimRejectionAttachmentsReference>();
        }

        public long Id { get; set; }
        public int OrganizationId { get; set; }
        public string ClaimIdentifier { get; set; }
        public long ClaimNo { get; set; }
        public string ServiceReferenceNumber { get; set; }
        public int? NphiesSeqNo { get; set; }
        public string Remarks { get; set; }
        public int CreatedBy { get; set; }
        public DateTime CreatedOn { get; set; }
        public int? ModifiedBy { get; set; }
        public DateTime? ModifiedOn { get; set; }
        public bool? IsActive { get; set; }

        public virtual ICollection<RcmNphiesclaimRejectionAttachmentsReference> RcmNphiesclaimRejectionAttachmentsReferences { get; set; }
    }
}
