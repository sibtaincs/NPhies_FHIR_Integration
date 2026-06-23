using System;
using System.Collections.Generic;

namespace Nphies.Core.Data.Entities
{
    public partial class RcmReportDiscripancy
    {
        public int ReportId { get; set; }
        public string DiscripancyType { get; set; }
        public int InvoiceNo { get; set; }
        public DateTime InvoiceDate { get; set; }
        public decimal? CyclusGrossAmount { get; set; }
        public decimal? VidaGrossAmount { get; set; }
        public decimal? CyclusCompanyShare { get; set; }
        public decimal? VidaCompanyShare { get; set; }
        public DateTime CreatedDate { get; set; }
        public int CreateBy { get; set; }
        public int? FacilityId { get; set; }
        public string VidaCompanyName { get; set; }
        public string RcmCompanyName { get; set; }
    }
}
