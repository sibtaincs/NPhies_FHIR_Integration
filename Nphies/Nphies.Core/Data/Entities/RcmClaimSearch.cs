using System;
using System.Collections.Generic;

namespace Nphies.Core.Data.Entities
{
    public partial class RcmClaimSearch
    {
        public int Id { get; set; }
        public int OrganizationId { get; set; }
        public long UserId { get; set; }
        public string SearchName { get; set; }
        public string SearchData { get; set; }
        public bool? IsActive { get; set; }
    }
}
