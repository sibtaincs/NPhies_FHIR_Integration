namespace Nphies.Core.Models.Cancellations
{
    public class Cancellation
    {
        public string ClaimBundleIdentifier { get; set; }
        public string ClaimIdentifier { get; set; }
        public long ClaimId { get; set; }
        public string Remarks { get; set; }
    }
}
