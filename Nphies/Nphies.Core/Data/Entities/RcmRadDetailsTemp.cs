using System;
using System.Collections.Generic;

namespace Nphies.Core.Data.Entities
{
    public partial class RcmRadDetailsTemp
    {
        public long RadDetailId { get; set; }
        public int OrganizationId { get; set; }
        public int UserId { get; set; }
        public int? EncounterNo { get; set; }
        public string InvoiceNo { get; set; }
        public DateTime? RadiologyVisitDate { get; set; }
        public string Remarks { get; set; }
        public string ClinicalData { get; set; }
        public string RadiologyResult { get; set; }
        public bool? IsProcessed { get; set; }
        public int CreatedBy { get; set; }
        public DateTime? CreatedOn { get; set; }
        public int ModifiedBy { get; set; }
        public DateTime? ModifiedOn { get; set; }
        public string ServiceCode { get; set; }
        public int FacilityId { get; set; }
        public int SubmissionType { get; set; }
        public DateTime ReceivedOn { get; set; }
        public int InvoiceLineItemNo { get; set; }
    }
}
