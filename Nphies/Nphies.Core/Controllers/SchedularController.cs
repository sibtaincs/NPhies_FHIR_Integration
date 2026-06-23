using Microsoft.AspNetCore.Mvc;
using Nphies.Core.Models;
using Nphies.Core.Services.Schedule;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Nphies.Core.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SchedularController : ControllerBase
    {
        ISchedularService service;
        public SchedularController(ISchedularService _service)
        {
            service = _service;
        }
        [HttpGet("GetProcessQueue")]
        public async Task<List<NphiesprocessQueueModel>> GetProcessQueue(int orginzationId, int facilityId, int? payerId)
        {
            if (payerId.HasValue)
                return await service.GetProcessQueueCNHI(orginzationId, facilityId, payerId.Value);
            return await service.GetProcessQueue(orginzationId, facilityId);
        }

        [HttpPost("ProcessQueueResponse")]
        public async Task<bool> ProcessQueueResponse(ProcessQueueModel model)
        {
            return await service.ProcessQueueResponse(model);
        }
        [HttpPost("UpdateRunningStateNphiesQueue")]
        public async Task<bool> UpdateRunningStateNphiesQueue(ProcessQueueModel model)
        {
            return await service.UpdateRunningStateNphiesQueue(model);
        }
        [HttpGet("GetCommunicationProcessQueue")]
        public async Task<List<NphiesprocessQueueModel>> GetCommunicationProcessQueue(int orginzationId, int facilityId, int? payerId)
        {
            if (payerId.HasValue)
                return await service.GetCommunicationProcessQueueCNHI(orginzationId, facilityId, payerId.Value);
            return await service.GetCommunicationProcessQueue(orginzationId, facilityId);
        }
        [HttpGet("GetAttachmentProcessQueue")]
        public async Task<List<NphiesprocessQueueModel>> GetAttachmentCommunicationProcessQueue(int orginzationId, int facilityId)
        {
            return await service.GetAttachmentProcessQueue(orginzationId, facilityId);
        }
        [HttpPost("UpdateCommunicationProcessQueue")]
        public async Task<bool> UpdateCommunicationProcessQueue(ProcessQueueModel model)
        {
            return await service.UpdateCommunicationProcessQueue(model);
        }

    }
}
