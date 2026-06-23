using System;
using System.Collections.Generic;

namespace Nphies.Core.Data.Entities
{
    public partial class RcmStandardCode
    {
        public long StandardCodeId { get; set; }
        public string StandardCode { get; set; }
        public byte StandardCodeType { get; set; }
        public string StandardCodeShortDesc { get; set; }
        public string StandardCodeDescription { get; set; }
        public int? OrganizationId { get; set; }
        public bool? IsActive { get; set; }
        public bool? IsDeleted { get; set; }
        public DateTime CreatedOn { get; set; }
        public int CreatedBy { get; set; }
        public int? EditedBy { get; set; }
        public DateTime? EditedOn { get; set; }
        public int? ServiceId { get; set; }
    }
}
