using System;
using System.Collections.Generic;

namespace Nphies.Core.Data.Entities
{
    public partial class RcmLabDetailsVidum
    {
        public long LabDetailId { get; set; }
        public int OrganizationId { get; set; }
        public int UserId { get; set; }
        public int EncounterNo { get; set; }
        public string InvoiceNo { get; set; }
        public string LabHigh { get; set; }
        public string LabLow { get; set; }
        public string LabProfile { get; set; }
        public string LabResult { get; set; }
        public string LabSection { get; set; }
        public string LabTestName { get; set; }
        public string LabUnits { get; set; }
        public DateTime? VisitDate { get; set; }
        public bool? IsProcessed { get; set; }
        public int CreatedBy { get; set; }
        public DateTime? CreatedOn { get; set; }
        public int? ModifiedBy { get; set; }
        public DateTime? ModifiedOn { get; set; }
        public string ServiceCode { get; set; }
        public int FacilityId { get; set; }
        public byte SubmissionType { get; set; }
        public string LabResultTextBased { get; set; }
        public byte? LabResultType { get; set; }
        public DateTime? ReceivedOn { get; set; }
        public string ReferenceRange { get; set; }
        public int? ServiceLineItemNo { get; set; }
    }
}
