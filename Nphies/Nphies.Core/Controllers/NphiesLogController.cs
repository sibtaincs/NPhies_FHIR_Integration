using Microsoft.AspNetCore.Mvc;
using Nphies.Core.Brokers.Loggings;
using Nphies.Core.Models;
using Nphies.Core.Services.Logger;
using System.Threading.Tasks;

namespace Nphies.Core.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class NphiesLogController : ControllerBase
    {
        ILogService service;
        private readonly ILoggingBroker loggingBroker;
        public NphiesLogController(ILogService _service, ILoggingBroker loggingBroker)
        {
            service = _service;
            this.loggingBroker = loggingBroker;
        }
        [HttpGet("GetClaimRequestLog")]
        public async Task<ClaimRequestModelMD> GetClaimRequestLog(string claimIdentifier)
        {
            return await service.GetClaimRequestLog(claimIdentifier);
        }

        [HttpGet("GetBatchRequestLog")]
        public async Task<ClaimBatchModelMD> GetBatchRequestLog(string claimIdentifier)
        {
            return await service.GetBatchRequestLog(claimIdentifier);
        }

        [HttpGet("GetClaimFriendlyViewer")]
        public async Task<FriendlyViewRequestModel> GetClaimFriendlyViewer(string claimIdentifier)
        {
            try
            {
                return await service.GetClaimRequestFriendlyViewer(claimIdentifier);
            }
            catch (System.Exception ex)
            {
                string message = string.Format("{0}-{1}-{2}",nameof(GetClaimFriendlyViewer),claimIdentifier, ex.Message) ;
                this.loggingBroker.LogError(message);
                return await Task.FromResult(new FriendlyViewRequestModel());
            }
        }
        [HttpGet("GetCommunicationLog")]
        public async Task<ClaimRequestModelMD> GetCommunicationLog(string claimIdentifier)
        {
            return await service.GetCommunicationLog(claimIdentifier);
        }
        [HttpGet("getPollResponse")]
        public async Task<bool> UpdatePollResponse(long claimId)
        {
            return await service.UpdatePollResponse(claimId);
        }

    }
}
