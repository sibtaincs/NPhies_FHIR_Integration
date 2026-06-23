using System;
using System.Collections.Generic;

namespace Nphies.Core.Data.Entities
{
    public partial class RcmDiagnosis
    {
        public int DiagnosisId { get; set; }
        public string DiagnosisCode { get; set; }
        public string DiagnosisDescription { get; set; }
        public bool? IsMorphology { get; set; }
        public bool? IsRTADiagnosis { get; set; }
    }
}
