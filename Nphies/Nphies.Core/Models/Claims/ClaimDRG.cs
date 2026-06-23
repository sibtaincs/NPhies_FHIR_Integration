namespace Nphies.Core.Models.Claims
{
    public class ClaimDRG
    {
        public int OrginzationId { get; set; }
        public int FacilityId { get; set; }
        public long ClaimId { get; set; }
        public string EncounterNo { get; set; }
        public string EncounterType { get; set; }
    }
}
