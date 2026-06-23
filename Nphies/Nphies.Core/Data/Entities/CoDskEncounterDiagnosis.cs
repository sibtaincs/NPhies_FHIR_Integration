using System;

namespace Nphies.Core.Data.Entities
{
    public class CoDskEncounterDiagnosis
    {
        public int? SequenceNo { get; set; }
        public int OrganizationId { get; set; }
        public int FacilityId { get; set; }
        public short EncounterType { get; set; }
        public string EncounterNo { get; set; }
        public short RecordType { get; set; }
        public string ICDCode { get; set; }
        public bool? IsPrincipal { get; set; }
        public bool? IsActive { get; set; }
        public int CreatedBy { get; set; }
        public DateTime CreatedOn { get; set; }
        public int? ModifiedBy { get; set; }
        public DateTime? ModifiedOn { get; set; }
        public short? COF { get; set; }
        public short? IsMorphoCode { get; set; }
        public string MorphoCodeDesc { get; set; }
    }
}
