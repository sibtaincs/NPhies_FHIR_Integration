using System;
using System.Collections.Generic;

namespace Nphies.Core.Data.Entities
{
    public partial class RcmReportTemplate
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public bool IsTemplate { get; set; }
        public string ReportFile { get; set; }
        public int ReportType { get; set; }
        public string Description { get; set; }
        public string ServiceUrl { get; set; }
        public int CreatedBy { get; set; }
        public DateTime CreatedDate { get; set; }
        public int? ModifyBy { get; set; }
        public DateTime? ModifyDate { get; set; }
        public bool IsActive { get; set; }
        public int OrganizationId { get; set; }
        public bool HasReportFilters { get; set; }
        public int? ReportCode { get; set; }
        public bool? IsRequestBasedReport { get; set; }
    }
}
