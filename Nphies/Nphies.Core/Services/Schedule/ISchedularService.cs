using Nphies.Core.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Nphies.Core.Services.Schedule
{
    public interface ISchedularService
    {
        Task<List<NphiesprocessQueueModel>> GetProcessQueue(int organizationId, int FacilityId);
        Task<List<NphiesprocessQueueModel>> GetProcessQueueCNHI(int organizationId, int FacilityId, int payerId);
        Task<bool> ProcessQueueResponse(ProcessQueueModel model);
        Task<List<NphiesprocessQueueModel>> GetCommunicationProcessQueue(int organizationId, int FacilityId);
        Task<List<NphiesprocessQueueModel>> GetAttachmentProcessQueue(int organizationId, int FacilityId);
        Task<bool> UpdateCommunicationProcessQueue(ProcessQueueModel model);
        Task<bool> UpdateRunningStateNphiesQueue(ProcessQueueModel model);
        Task<List<NphiesprocessQueueModel>> GetCommunicationProcessQueueCNHI(int organizationId, int FacilityId, int payerId);
    }
}
