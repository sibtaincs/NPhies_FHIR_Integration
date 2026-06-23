namespace Nphies.Core.DTOs
{
    public class SubmitClaimRequest
    {
      //  public long ClaimId { get; set; }
        public string ClaimIdentifier { get; set; }
        public int OrganizationId { get; set; }
    }

    public class SubmitClaimResponse
    {
        public bool Success { get; set; }
        public string Message { get; set; }
        public string ClaimIdentifier { get; set; }
        public string Status { get; set; }
        public string ResponseDetails { get; set; }
        public bool IsPended { get; set; }
    }
}
