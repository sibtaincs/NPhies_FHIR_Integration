
using Nphies.Core.Models;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Nphies.Core.Services.Communication
{
    public interface ICommunicationService
    {
        Task<List<CommunicationDto>> GetCommunicationServices
         (
             int orginzationId,
             int facilityId,
             string adaptorCode,
             DateTime dateFrom,
             DateTime dateTo,
             long processId,
             int batchsize,
             string PayerId
         );

        Task<List<ClaimAttachmentCommunicationDto>> GetCommunication
         (
             int orginzationId,
             int facilityId,
             string adaptorCode,
             DateTime dateFrom,
             DateTime dateTo,
             long processId,
             int batchsize,
             string PayerId
         );

        Task<bool> UpdateStatusAfterResubmission(ResubmissionStatusDto resubmissionStatus);
    }
}
