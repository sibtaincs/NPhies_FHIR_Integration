using Nphies.Core.Data.Entities;
using Nphies.Core.DTOs;
using Nphies.Core.Models;
using Nphies.Core.Models.Cancellations;
using Nphies.Core.Models.Claims;
using Nphies.Core.Models.Submission.SameBundles.Claims;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Nphies.Core.Services.Claims
{
    public interface IClaimService
    {
        ValueTask<List<ClaimDetail>> GetClaims(int orginzationId, int facilityId, string adaptorCode, DateTime dtfrom, DateTime dtTo, long processId, int batchsize, string PayerId, bool extend);
        ValueTask<List<ClaimDetail>> GetClaimsByProcess(int orginzationId, int facilityId, string adaptorCode, long processId, int batchsize, string PayerId);
        ValueTask<bool> UpdateClaimStatus(ClaimResponseRequest claimResponseRequest);
        ValueTask<bool> UpdatePoolingResponse(ClaimUpdateModel claimPoolingRequest);
        ValueTask<ClaimCancelation> RetriveClaims(int orginzationId, int facilityId,int payerId, long processId);
        // Task<Payer> ClaimPayers(RcmClaim claim, RcmPayer[] payers, List<RcmAdapterMapping> rcmAdapterMapping);
        // Task GetClaimById(long claimId);
        ValueTask<bool> CancellationClaimAsync(Cancellation claimResponseRequest);

        ValueTask<bool> UpdateClaimAttachmentAsync(ClaimsAttachment claimsAttachment);
        ValueTask<bool> UpdateClaimAgainstCoderEncounter(ClaimDRG claimDRG);

        ValueTask<bool> AddNphiesPostTrail(NphiesPostTrailDto nphiesPostTrail);
        ValueTask<bool> UpdateClaimStatusByClaimId(long claimId, string errorMessage, byte status);
        ValueTask<bool> UpdateClaim(UpdateClaimStatusResponse claimResponseRequest);

        ValueTask<bool> UpdateClaimAndServicesSeqAsync(UpdateClaimAndServicesSeqRequest updateClaimAndServicesSeqRequest);
        ValueTask<bool> UpdateClaimResponseAsync(UpdateClaimResponseModel updateClaimAndServicesSeqRequest);
        Task<ApiResponseOnUpdate> UpdateMedicalDataForClaim(EncounterMedicalDetail payload);
        Task<List<string>> RetrieveAttachmentDocumentIds(long claimId);
        ValueTask<long> GetClaimId(int orginzationId, int facilityId, string adaptorCode, DateTime dtfrom, DateTime dtTo, long processId, int batchsize, string PayerId, bool extend);
    }
}
