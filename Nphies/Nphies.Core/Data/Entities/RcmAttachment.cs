using System;
using System.Collections.Generic;

namespace Nphies.Core.Data.Entities
{
    public partial class RcmAttachment
    {
        public RcmAttachment()
        {
            RcmNphiesclaimRejectionAttachmentsReferences = new HashSet<RcmNphiesclaimRejectionAttachmentsReference>();
        }

        public long AttachmentId { get; set; }
        public int OrganizationId { get; set; }
        public long TransactionId { get; set; }
        public short? LineItemNumber { get; set; }
        public string FileName { get; set; }
        public string FilePath { get; set; }
        public string FileType { get; set; }
        public string SourceType { get; set; }
        public bool? IsActive { get; set; }
        public int? CreatedBy { get; set; }
        public DateTime? CreatedOn { get; set; }
        public int? ModifiedBy { get; set; }
        public DateTime? ModifiedOn { get; set; }
        public string TransactionType { get; set; }
        public byte[] FileBytes { get; set; }
        public bool? IncludeSubmission { get; set; }
        public int DocumentId { get; set; }
        public string MongoDocId { get; set; }
        public virtual ICollection<RcmNphiesclaimRejectionAttachmentsReference> RcmNphiesclaimRejectionAttachmentsReferences { get; set; }
    }
}
