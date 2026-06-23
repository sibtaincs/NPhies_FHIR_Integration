using Microsoft.AspNetCore.Mvc;
using Nphies.Core.Brokers.Loggings;
using Nphies.Core.DTOs;
using Nphies.Core.Services.Submission;
using System;
using System.Threading.Tasks;

namespace Nphies.Core.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SubmissionController : ControllerBase
    {
        private readonly ISubmissionService _submissionService;
        private readonly ILoggingBroker _loggingBroker;

        public SubmissionController(ISubmissionService submissionService, ILoggingBroker loggingBroker)
        {
            _submissionService = submissionService;
            _loggingBroker = loggingBroker;
        }

        [HttpPost("SubmitClaimWithSameBundle")]
        public async Task<IActionResult> SubmitClaimWithSameBundle([FromBody] SubmitClaimRequest request)
        {
            try
            {
                if (request == null || string.IsNullOrWhiteSpace(request.ClaimIdentifier) || request.OrganizationId <= 0)
                {
                    return BadRequest(new SubmitClaimResponse
                    {
                        Success = false,
                        Message = "Invalid request. ClaimId and OrganizationId are required."
                    });
                }

                var result = await _submissionService.SubmitClaimWithSameBundleAsync(request);

                if (result.Success)
                {
                    return Ok(result);
                }

                return BadRequest(result);
            }
            catch (Exception ex)
            {
                string errorMessage = $"{nameof(SubmitClaimWithSameBundle)}-ClaimId:{request?.ClaimIdentifier}-{ex.Message}";
                _loggingBroker.LogError(errorMessage);
                _loggingBroker.LogCritical(ex);

                return StatusCode(500, new SubmitClaimResponse
                {
                    Success = false,
                    Message = $"An error occurred while processing the claim: {ex.Message}"
                });
            }
        }
    }
}
