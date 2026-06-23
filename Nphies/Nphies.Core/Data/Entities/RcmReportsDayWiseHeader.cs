using System;
using System.Collections.Generic;

namespace Nphies.Core.Data.Entities
{
    public partial class RcmReportsDayWiseHeader
    {
        public long Id { get; set; }
        public int OrganizationId { get; set; }
        public int FacilityId { get; set; }
        public DateTime InvoiceDate { get; set; }
        public bool? IsDeleted { get; set; }
    }
}
