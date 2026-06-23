using System;
using System.Collections.Generic;

namespace Nphies.Core.Data.Entities
{
    public partial class RcmDiagnosisDetailsTemp
    {
        public int Id { get; set; }
        public int OrganizationId { get; set; }
        public int EncounterNo { get; set; }
        public string DiagnosisCode { get; set; }
        public string DiagnosisCodeType { get; set; }
        public byte? DiagnosisType { get; set; }
        public string DiagnosisDescription { get; set; }
        public string DiagnoisInfoCode { get; set; }
        public string DiagnoisInfoType { get; set; }
        public DateTime? ReceivedOn { get; set; }
        public bool? IsProcessed { get; set; }
    }
}
