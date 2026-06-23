using System;
using System.Collections.Generic;

namespace Nphies.Core.Data.Entities
{
    public partial class RcmClaimLabDetail
    {
        public long ClaimId { get; set; }
        public int OrganizationId { get; set; }
        public long ServiceId { get; set; }
        public short ServiceLineitemNo { get; set; }
        public string LabHigh { get; set; }
        public string LabLow { get; set; }
        public string LabProfile { get; set; }
        public string LabResult { get; set; }
        public string LabSection { get; set; }
        public string LabTestName { get; set; }
        public string LabUnits { get; set; }
        public DateTime? VisitDate { get; set; }
        public short LineItemNo { get; set; }
        public string ReferenceRange { get; set; }
    }
}
