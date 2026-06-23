using Nphies.Core.Models;
using System.Threading.Tasks;

namespace Nphies.Core.Services.Logger
{
    public interface ILogService
    {
        Task<ClaimRequestModelMD> GetClaimRequestLog(string claimIdentifier);
        Task<ClaimBatchModelMD> GetBatchRequestLog(string claimIdentifier);
        Task<FriendlyViewRequestModel> GetClaimRequestFriendlyViewer(string claimIdentifier);
        Task<ResponseBundleModel> GetClaimResponseFriendlyViewer(string claimIdentifier);
        Task<ClaimRequestModelMD> GetCommunicationLog(string claimIdentifier); 
        Task<bool> UpdatePollResponse(long claimId); 
    }
}
