using System;
using System.Collections.Generic;

namespace Nphies.Core.Data.Entities
{
    public partial class RcmRejectionReason
    {
        public int RejectionId { get; set; }
        public string RejectionReason { get; set; }
        public bool? IsActive { get; set; }
        public int? OrganizationId { get; set; }
        public byte? RejectionType { get; set; }
    }
}
