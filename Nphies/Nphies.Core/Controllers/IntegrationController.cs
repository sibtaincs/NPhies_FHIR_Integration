using Microsoft.AspNetCore.Mvc;
using Nphies.Core.Brokers.Loggings;
using Nphies.Core.Data.Entities;
using Nphies.Core.DTOs;
using Nphies.Core.Models;
using Nphies.Core.Models.Cancellations;
using Nphies.Core.Models.Claims;
using Nphies.Core.Models.Submission.SameBundles.Claims;
using Nphies.Core.Services.Claims;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Nphies.Core.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class IntegrationController : ControllerBase
    {
       private readonly IClaimService service;
        private readonly ILoggingBroker loggingBroker;
        public IntegrationController(IClaimService _service, ILoggingBroker loggingBroker)
        {
            service = _service;
            this.loggingBroker = loggingBroker;
        }

        [HttpGet("Claims")]
        public async ValueTask<List<ClaimDetail>> GetClaims(int orginzationId, int facilityId,
            string adaptorCode, DateTime dateFrom, DateTime dateTo, long processId, int batchSize, string payerId = null,bool extend= false)
        {
            try
            {
                return await service.GetClaims(orginzationId, facilityId, adaptorCode, dateFrom, dateTo, processId, batchSize, payerId, extend);
            }
            catch (Exception ex)
            {
                string errorMessage = string.Format("{0}-{1}-{2}-{3}", nameof(GetClaims), facilityId, processId, ex.Message);
                this.loggingBroker.LogError(errorMessage);
                this.loggingBroker.LogCritical(ex);
                return await Task.FromResult(new List<ClaimDetail>());
            }
        }
        [HttpGet("getclaimsbyprocess")]
        public async ValueTask<List<ClaimDetail>> GetClaimsByProcess(int orginzationId, int facilityId,
            string adaptorCode,  long processId, int batchSize, string payerId = null)
        {
            try
            {
                return await service.GetClaimsByProcess(orginzationId, facilityId, adaptorCode, processId, batchSize, payerId);
            }
            catch (Exception ex)
            {

                string errorMessage = string.Format("{0}-{1}-{2}-{3}", nameof(GetClaimsByProcess),facilityId, processId, ex.Message);
                this.loggingBroker.LogError(errorMessage);
                this.loggingBroker.LogCritical(ex);
                return await Task.FromResult(new List<ClaimDetail>());
            }
        }
        [HttpGet("getclaims")]
        public async ValueTask<ClaimCancelation> GetClaimsForSubmissionWithSameBundle(int orginzationId, int facilityId,int payerId, long processId)
        {
            try
            {
                return await service.RetriveClaims(orginzationId, facilityId, payerId, processId);
            }
            catch (Exception ex)
            {

                string errorMessage = string.Format("{0}-{1}-{2}", nameof(GetClaimsForSubmissionWithSameBundle), processId, ex.Message);
                this.loggingBroker.LogError(errorMessage);
                this.loggingBroker.LogCritical(ex);
                return await Task.FromResult(new ClaimCancelation ());
            }
        }
        [HttpPost("UpdateResponse")]
        public async ValueTask<bool> UpdateClaimStatus(ClaimResponseRequest claimResponseRequest)
        {
            try
            {
                return await service.UpdateClaimStatus(claimResponseRequest);
            }
            catch (Exception ex)
            {
               
                string errorMessage = string.Format("{0}-{1}-{2}", nameof(UpdateClaimStatus), claimResponseRequest.ClaimRequestModel.ClaimDetails[0].ClaimID, ex.Message);
                this.loggingBroker.LogError(errorMessage);
                this.loggingBroker.LogCritical(ex);
                return await Task.FromResult(false);
            }
        }

        [HttpPost("UpdateClaimStatus")]
        public async ValueTask<bool> UpdateClaim(UpdateClaimStatusResponse claimResponseRequest)
        {
            try
            {
                return await service.UpdateClaim(claimResponseRequest);
            }
            catch (Exception ex)
            {

                string errorMessage = string.Format("{0}-{1}-{2}", nameof(UpdateClaim), claimResponseRequest.ClaimId, ex.Message);
                this.loggingBroker.LogError(errorMessage);
                this.loggingBroker.LogCritical(ex);
                return await Task.FromResult(false);
            }
        }

        [HttpPost("UpdatePoolingResponse")]
        public async Task<bool> UpdatePoolingResponse(ClaimUpdateModel claimPoolingRequest)
        {
            try
            {
                return await service.UpdatePoolingResponse(claimPoolingRequest);
            }
            catch (Exception ex)
            {

                string errorMessage = string.Format("{0}-{1}-{2}", nameof(UpdatePoolingResponse), claimPoolingRequest.ClaimIdentifier, ex.Message);
                this.loggingBroker.LogError(errorMessage);
                this.loggingBroker.LogCritical(ex);
                return await Task.FromResult(false);
            }
        }

        //[HttpGet("GetClaimById")]
        //public async Task GetClaimbyId(long claimId)
        //{

        //     await service.GetClaimById(claimId);
        //}

        [HttpPost("Cancellation")]
        public async ValueTask<bool> Cancellation(Cancellation claimResponseRequest)
        {
            try
            {
                return await service.CancellationClaimAsync(claimResponseRequest);
            }
            catch (Exception ex)
            {

                string errorMessage = string.Format("{0}-{1}-{2}", nameof(Cancellation), claimResponseRequest.ClaimId, ex.Message);
                this.loggingBroker.LogError(errorMessage);
                this.loggingBroker.LogCritical(ex);
                return await Task.FromResult(false);
            }
        }

        [HttpPost("UpdateClaimsAttachment")]
        public async ValueTask<bool> UpdateClaimsAttachment(ClaimsAttachment claimsAttachment)
        {
            try
            {
                return await service.UpdateClaimAttachmentAsync(claimsAttachment);
            }
            catch (Exception ex)
            {
                string errorMessage = string.Format("{0}-{1}-{2}",nameof(UpdateClaimsAttachment), claimsAttachment.ClaimId,ex.Message);
                this.loggingBroker.LogError(errorMessage);
                this.loggingBroker.LogCritical(ex);
               return await Task.FromResult(false);
            }
        }
        [HttpPost("UpdateClaimAgainstCoderEncounter")]
        public async ValueTask<bool> UpdateClaimAgainstCoderEncounter(ClaimDRG claimDRG)
        {
            try
            {
                return await service.UpdateClaimAgainstCoderEncounter(claimDRG);
            }
            catch (Exception ex)
            {

                string errorMessage = string.Format("{0}-{1}-{2}", nameof(UpdateClaimAgainstCoderEncounter), claimDRG.ClaimId, ex.Message);
                this.loggingBroker.LogError(errorMessage);
                this.loggingBroker.LogCritical(ex);
                return await Task.FromResult(false);
            }
        }

        [HttpPost("AddClaimLogs")]
        public async ValueTask<bool> AddClaimLogs(NphiesPostTrailDto claimLog)
        {
            try
            {
                return await service.AddNphiesPostTrail(claimLog);
            }
            catch (Exception ex)
            {

                string errorMessage = string.Format("{0}-{1}-{2}", nameof(AddClaimLogs), claimLog.ClaimId, ex.Message);
                this.loggingBroker.LogError(errorMessage);
                this.loggingBroker.LogCritical(ex);
                return await Task.FromResult(false);
            }
        }

        [HttpPost("UpdateClaimStatusById")]
        public async ValueTask<bool> UpdateClaimStatusById(ClaimStatusUpdateDto claimStatus)
        {
            try
            {
                var isUpdated = await service.UpdateClaimStatusByClaimId(claimStatus.ClaimId, claimStatus.errorMessage, (byte)claimStatus.Status);
                return isUpdated;
            }
            catch (Exception ex)
            {

                string errorMessage = string.Format("{0}-{1}-{2}", nameof(UpdateClaimStatusById), claimStatus.ClaimId, ex.Message);
                this.loggingBroker.LogError(errorMessage);
                this.loggingBroker.LogCritical(ex);
                return await Task.FromResult(false);
            }
        }

        [HttpPost("UpdateClaimAndServicesSeqAsync")]
        public async ValueTask<bool> UpdateClaimAndServicesSeqAsync(UpdateClaimAndServicesSeqRequest  updateClaimAndServicesSeqRequest)
        {
            try
            {
               return await service.UpdateClaimAndServicesSeqAsync(updateClaimAndServicesSeqRequest);
               
            }
            catch (Exception ex)
            {
                string errorMessage = string.Format("{0}-{1}-{2}", nameof(UpdateClaimAndServicesSeqAsync), updateClaimAndServicesSeqRequest.ClaimID, ex.Message);
                this.loggingBroker.LogCritical(ex);
                loggingBroker.LogError(errorMessage);
                return false;
                
            }
        }

        [HttpPost("UpdateClaimResponseAsync")]
        public async ValueTask<bool> UpdateClaimResponseAsync(UpdateClaimResponseModel updateClaimAndServicesSeqRequest)
        {
            try
            {
                return await service.UpdateClaimResponseAsync(updateClaimAndServicesSeqRequest);

            }
            catch (Exception ex)
            {
                string errorMessage = string.Format("{0}-{1}-{2}", nameof(UpdateClaimResponseAsync), updateClaimAndServicesSeqRequest.ClaimDetailResponse.ClaimId, ex.Message);
                this.loggingBroker.LogCritical(ex);
                loggingBroker.LogError(errorMessage);

                return false;
            }
        }
        [HttpPost("UpdateMedicalDataForClaim")]
        public async Task<ApiResponseOnUpdate> UpdateMedicalDataForClaim(EncounterMedicalDetail payload)
        {
            var isUpdated = await service.UpdateMedicalDataForClaim(payload);
            return isUpdated;
        }
        [HttpGet("GetClaimsId")]
        public async ValueTask<long> GetClaimIds(int orginzationId, int facilityId,
            string adaptorCode, DateTime dateFrom, DateTime dateTo, long processId, int batchSize, string payerId = null, bool extend = false)
        {
            try
            {
                return await service.GetClaimId(orginzationId, facilityId, adaptorCode, dateFrom, dateTo, processId, batchSize, payerId, extend);
            }
            catch (Exception ex)
            {
                string errorMessage = string.Format("{0}-{1}-{2}-{3}", nameof(GetClaims), facilityId, processId, ex.Message);
                this.loggingBroker.LogError(errorMessage);
                this.loggingBroker.LogCritical(ex);
                return await Task.FromResult(0);
            }
        }
    }

}
