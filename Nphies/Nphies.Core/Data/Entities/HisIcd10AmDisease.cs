using System;
using System.Collections.Generic;

namespace Nphies.Core.Data.Entities
{
    public partial class HisIcd10AmDisease
    {
        public string SetupId { get; set; }
        public string CodeId { get; set; }
        public string Chapter { get; set; }
        public int CodeLength { get; set; }
        public int Dagger { get; set; }
        public int Asterisk { get; set; }
        public int Valid { get; set; }
        public int AustCode { get; set; }
        public string AsciiDesc { get; set; }
        public string AsciiShor { get; set; }
        public DateTime? Effective { get; set; }
        public DateTime? Inactive { get; set; }
        public DateTime? Reactivate { get; set; }
        public int? Sex { get; set; }
        public int? Stype { get; set; }
        public int? Agel { get; set; }
        public int? Ageh { get; set; }
        public int? Atype { get; set; }
        public int? Rdiag { get; set; }
        public DateTime? ConceptCh { get; set; }
        public bool? IsActive { get; set; }
        public int? CreatedBy { get; set; }
        public DateTime? CreatedOn { get; set; }
        public int? EditedBy { get; set; }
        public DateTime? EditedOn { get; set; }
        public byte[] RowVer { get; set; }
        public byte CodeType { get; set; }
        public string CodeUnformatted { get; set; }
        public bool? IsInfectiousDisease { get; set; }
        public short? TypeOfInfection { get; set; }
        public int? SickLeavePeriod { get; set; }
        public bool? IsHighRisk { get; set; }
    }
}
