namespace Nphies.Core.Models.Services
{
    public class ClaimService
    {
        public byte? Status { get; set; }
        public int? NphiesSeqNo { get; set; }
        public string NphieseRemarks { get; set; }
        public string SubmittedAmount { get; set; }
        public string SubmittedQuantity { get; set; }
    }
}
