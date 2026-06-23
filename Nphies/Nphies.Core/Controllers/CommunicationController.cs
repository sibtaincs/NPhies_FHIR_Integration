using Microsoft.AspNetCore.Mvc;
using Nphies.Core.Models;
using Nphies.Core.Services.Communication;
using System;
using System.Threading.Tasks;

namespace Nphies.Core.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CommunicationController : ControllerBase
    {
        ICommunicationService communicationService;
        public CommunicationController(ICommunicationService _communicationService)
        {
            communicationService = _communicationService;
        }

        [HttpGet("GetCommunicationServices")]
        public async Task<IActionResult> GetCommunicationServices(
            int orginzationId,
            int facilityId,
            string adaptorCode, 
            DateTime dateFrom,
            DateTime dateTo, 
            long processId, 
            int batchSize, 
            string payerId = null)
        {
            var response = await communicationService.GetCommunicationServices(orginzationId,
                            facilityId,
                            adaptorCode,
                            dateFrom,
                            dateTo,
                            processId,
                            batchSize,
                            payerId);
            return Ok(response);
        }

        [HttpGet("GetCommunication")]
        public async Task<IActionResult> GetCommunication(
            int orginzationId,
            int facilityId,
            string adaptorCode,
            DateTime dateFrom,
            DateTime dateTo,
            long processId,
            int batchSize,
            string payerId = null)
        {
            var response = await communicationService.GetCommunication(orginzationId,
                            facilityId,
                            adaptorCode,
                            dateFrom,
                            dateTo,
                            processId,
                            batchSize,
                            payerId);
            return Ok(response);
        }

        [HttpPost("UpdateStatusAfterResubmission")]
        public async Task<IActionResult> UpdateStatusAfterResubmission(ResubmissionStatusDto resubmissionStatus)
        {
            var response = await communicationService.UpdateStatusAfterResubmission(resubmissionStatus);
            return Ok(response);
        }



    }
}
