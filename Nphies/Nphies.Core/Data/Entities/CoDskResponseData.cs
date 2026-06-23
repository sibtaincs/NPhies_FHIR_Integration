using System;

namespace Nphies.Core.Data.Entities
{
    public partial class CoDskResponseData
    {
        public int OrganizationId { get; set; }
        public int FacilityId { get; set; }
        public byte EncounterType { get; set; }
        public string EncounterNo { get; set; }
        public byte RecordType { get; set; }
        public string Drg { get; set; }
        public string Drgdesc { get; set; }
        public string Mdc { get; set; }
        public string Mdcdesc { get; set; }
        public double? NationalLengthOfStay { get; set; }
        public double? NationalWeight { get; set; }
        public double? EpisodeClinicalComplexityRoundedScore { get; set; }
        public string GrouperStatusCode { get; set; }
        public int CreatedBy { get; set; }
        public DateTime CreatedOn { get; set; }
        public int? ModifiedBy { get; set; }
        public DateTime? ModifiedOn { get; set; }
    }

}
