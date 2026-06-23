using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Nphies.Core.Data.Entities;
using Nphies.Core.Models;
using static Nphies.Core.Models.Common;

namespace Nphies.Core.Services.Schedule
{
    public class SchedularService : ISchedularService
    {
        private readonly ZyklusCoreContext context;
        private readonly IConfiguration config;

        public SchedularService(ZyklusCoreContext context, IConfiguration _config)
        {
            this.context = context;
            this.config = _config;

        }

        public async Task<List<NphiesprocessQueueModel>> GetProcessQueue(int organizationId, int FacilityId)
        {
            try
            {
                List<byte> noContainCriteriaType = new List<byte>();
                noContainCriteriaType.Add(6);
                noContainCriteriaType.Add(11);

                var adapterId = await RetriveAdapterAsync(organizationId);

                var mohPayerId = Convert.ToInt32(config["MohPayerId"]);

                var proceessQueueList =  await context.RcmNphiesprocessQueues.AsNoTracking()
                                .Where(predicate => predicate.OrganizationId == organizationId
                                && predicate.FacilityId == FacilityId
                                && (predicate.IsProcessed == null || predicate.IsProcessed != true)
                                && (predicate.IsRunningProcess == null || predicate.IsRunningProcess != true)
                                && (!noContainCriteriaType.Contains(predicate.CriteriaType))
                                && predicate.VerifiedBy != null
                                && predicate.PayerId != (mohPayerId)
                                )
                                // .Where(pre=>pre.ProcessId == 44841)
                                .Select(x => SelectProperties(x, null)).ToListWithNoLockAsync();

                foreach (var item in proceessQueueList)
                {
                   item.IsRegular = CheckAdapterType(adapterId, item);
                }
               
              return proceessQueueList;
            }
            catch (Exception ex) { throw; }
        }

        private static bool CheckAdapterType(int adapterId, NphiesprocessQueueModel item)
        {
            return (adapterId != 0 && item.AdapterId == adapterId);
        }

        private async Task<int> RetriveAdapterAsync(int organizationId)
        {
            return await context.RcmAdapters.AsNoTracking().
                Where(x => x.AdaptorCode == AdapterCode.Regular
                && x.OrganizationId == organizationId)
                .Select(x=>x.AdapterId).FirstOrDefaultAsync();
        }
        public async Task<List<NphiesprocessQueueModel>> GetProcessQueueCNHI(int organizationId, int facilityId, int payerId)
        {
            try
            {
                var noContainCriteriaType = new List<byte> { 6, 11 };
                var nphiesModels = await (
                    from q in context.RcmNphiesprocessQueues.AsNoTracking()
                    join facility in context.RcmFacilities.AsNoTracking()
                        on q.FacilityId equals facility.FacilityId
                    where q.OrganizationId == organizationId
                          && (q.IsProcessed == null || q.IsProcessed != true)
                          && (q.IsRunningProcess == null || q.IsRunningProcess != true)
                          && (!noContainCriteriaType.Contains(q.CriteriaType))
                          && q.VerifiedBy != null
                          && q.PayerId == payerId
                    select SelectProperties(q, facility)
                ).ToListWithNoLockAsync();

                return nphiesModels;
            }
            catch (Exception ex) { throw; }
        }

        public async Task<bool> ProcessQueueResponse(ProcessQueueModel model)
        {

            try
            {
                var queue = await context.RcmNphiesprocessQueues
                                                                .FirstOrDefaultAsync(predicate => predicate.OrganizationId == model.OrginzationId
                                                                && predicate.FacilityId == model.FacilityId
                                                                && predicate.ProcessId == model.ProcessId);

                if (queue != null)
                {

                    //var claims = context.RcmClaims.AsNoTracking()
                    //           .Where(claim =>
                    //               claim.OrganizationId == queue.OrganizationId
                    //               && claim.FacilityId == queue.FacilityId
                    //               && claim.PayerId == queue.PayerId
                    //               && claim.Status != (byte)ClaimStatus.WriteOff
                    //               && claim.ProcessId == model.ProcessId);

                    //if (claims != null)
                    //{
                    //    //queue.TotalClaim = await claims.CountAsync();
                    //    queue.TotalQueued = await claims.CountAsync(claim => claim.Status == (byte)ClaimStatus.Nphies_Queued);
                    //    queue.TotalError = await claims.CountAsync(claim => claim.Status == (byte)ClaimStatus.Nphies_Error);
                    //    queue.TotalTimout = await claims.CountAsync(claim => claim.Status == (byte)ClaimStatus.Nphies_OutCome);
                    //    queue.TotalPError = await claims.CountAsync(claim => claim.Status == (byte)ClaimStatus.Nphies_Perror);
                    //    queue.TotalPended = await claims.CountAsync(claim => claim.Status == (byte)ClaimStatus.Nphies_Pended);
                    //}

                    var claims = await context.RcmClaims.AsNoTracking()
                                                                    .Where(claim =>
                                                                                    claim.OrganizationId == queue.OrganizationId &&
                                                                                    claim.FacilityId == queue.FacilityId &&
                                                                                    claim.PayerId == queue.PayerId &&
                                                                                    claim.Status != (byte)ClaimStatus.WriteOff &&
                                                                                    claim.ProcessId == model.ProcessId)
                                                                    .GroupBy(claim => claim.Status)
                                                                    .Select(group => new { Status = group.Key, Count = group.Count() })
                                                                    .ToDictionaryAsync(g => g.Status, g => g.Count);
                    if (claims != null)
                    {
                        queue.TotalQueued = claims.GetValueOrDefault((byte)ClaimStatus.Nphies_Queued, 0);
                        queue.TotalError = claims.GetValueOrDefault((byte)ClaimStatus.Nphies_Error, 0);
                        queue.TotalTimout = claims.GetValueOrDefault((byte)ClaimStatus.Nphies_OutCome, 0);
                        queue.TotalPError = claims.GetValueOrDefault((byte)ClaimStatus.Nphies_Perror, 0);
                        queue.TotalPended = claims.GetValueOrDefault((byte)ClaimStatus.Nphies_Pended, 0);

                        queue.IsProcessed = true;
                        queue.ProcessedBy = 899;
                        queue.ProcessedOn = DateTime.Now;
                        context.RcmNphiesprocessQueues.Update(queue);
                        if (await context.SaveChangesAsync() > 0)
                            return await Task.FromResult(true);
                    }

                }
            }
            catch (Exception ex)
            {

            }
            return await Task.FromResult(false);
        }



        private static NphiesprocessQueueModel SelectProperties(RcmNphiesprocessQueue x, RcmFacility? facility = null)
        {
            return new NphiesprocessQueueModel
            {
                OrganizationId = x.OrganizationId,
                FacilityId = x.FacilityId,
                ProcessId = x.ProcessId,
                ClaimNo = x.ClaimNo,
                PayerId = x.PayerId,
                PolicyId = x.PolicyId,
                CriteriaType = x.CriteriaType,
                IsProcessed = x.IsProcessed,
                FromDate = x.FromDate,
                ToDate = x.ToDate,
                CreatedBy = x.CreatedBy,
                CreatedOn = x.CreatedOn,
                ProcessedBy = x.ProcessedBy,
                ProcessedOn = x.ProcessedOn,
                Limit = x.Limit,
                TotalClaim = x.TotalClaim.Value,
                IncludePreAuthRef = x.IncludePreAuthRef,
                SubmittedSingleClaim = x.SubmittedSingleClaim ?? false,
                ProjectId = facility?.ExternalCode,
                SetupId = facility?.ExternalCode2,
                AdapterId = x.AdapterId
            };
        }
        public async Task<List<NphiesprocessQueueModel>> GetCommunicationProcessQueue(int organizationId, int facilityId)
        {
            try
            {
                var mohPayerId = Convert.ToInt32(config["MohPayerId"]);
                var nphiesModels = await (
                    from q in context.RcmNphiesprocessQueues.AsNoTracking()
                    join facility in context.RcmFacilities.AsNoTracking()
                        on q.FacilityId equals facility.FacilityId
                    where q.OrganizationId == organizationId
                          && q.FacilityId == facilityId
                          && q.CriteriaType == 6
                          && (q.IsProcessed == null || q.IsProcessed != true)
                          && (q.IsQueueProcessed.HasValue && q.IsQueueProcessed == true)
                          && q.VerifiedBy != null
                          && q.PayerId != mohPayerId
                    select SelectProperties(q, facility)
                ).ToListWithNoLockAsync();

                foreach (var model in nphiesModels)
                {
                    model.TotalClaim = await GetTotalClaim(model.OrganizationId, model.FacilityId, model.ProcessId, model.PayerId.Value);
                }

                nphiesModels = nphiesModels.OrderBy(d => d.TotalClaim).ToList();

                return nphiesModels;
            }
            catch (Exception ex) { throw; }
        }
        public async Task<List<NphiesprocessQueueModel>> GetCommunicationProcessQueueCNHI(int organizationId, int facilityId, int payerId)
        {
            try
            {

                var nphiesModels = await (
                    from q in context.RcmNphiesprocessQueues.AsNoTracking()
                    join facility in context.RcmFacilities.AsNoTracking()
                        on q.FacilityId equals facility.FacilityId
                    where q.OrganizationId == organizationId
                          && q.CriteriaType == 6
                          && (q.IsProcessed == null || q.IsProcessed != true)
                          && (q.IsQueueProcessed.HasValue && q.IsQueueProcessed == true)
                          && q.VerifiedBy != null
                          && q.PayerId == payerId
                    select SelectProperties(q, facility)
                ).ToListWithNoLockAsync();


                foreach (var model in nphiesModels)
                {
                    model.TotalClaim = await GetTotalClaim(model.OrganizationId, model.FacilityId, model.ProcessId, model.PayerId.Value);
                }

                nphiesModels = nphiesModels.OrderBy(d => d.TotalClaim).ToList();

                return nphiesModels;
            }
            catch (Exception ex) { throw; }
        }


        private async Task<long> GetTotalClaim(int orginaztionId,
                        int facilityId,
                        long processId,
                        int payerId)
        {
            return await (from claim in context.RcmClaims.AsNoTracking()
                          join services in context.RcmClaimServicesDetails.AsNoTracking()
                          on claim.ClaimId equals services.ClaimId
                          join patient in context.RcmClaimPatientDetails.AsNoTracking()
                          on claim.ClaimId equals patient.ClaimId
                          where claim.OrganizationId == orginaztionId
                                      && claim.FacilityId == facilityId
                                      && claim.PayerId == payerId
                                      && services.ProcessId == processId
                                      && services.ReSubmissionStatus == (byte)CommunicationStatus.ResponseAdded
                                      && (services.IsDeleted == null || services.IsDeleted == false)
                                      && (services.IsReturn == null || services.IsReturn == false)
                                      && (services.IsRefund == null || services.IsRefund == false)
                                      && (services.NphiesSeqNo != null)
                          select new
                          {
                              ClaimId = claim.ClaimId
                          }).CountWithNoLockAsync();
        }


        public async Task<List<NphiesprocessQueueModel>> GetAttachmentProcessQueue(int organizationId, int FacilityId)
        {
            try
            {
                return await context.RcmNphiesprocessQueues.AsNoTracking()
                                                            .Where(predicate => predicate.OrganizationId == organizationId
                                                                            && predicate.FacilityId == FacilityId
                                                                            && predicate.CriteriaType == 11
                                                                            && (predicate.IsProcessed == null || predicate.IsProcessed != true)
                                                                            && predicate.VerifiedBy != null)
                                                            .Select(x => SelectProperties(x, null)).ToListWithNoLockAsync();
            }
            catch (Exception ex) { throw; }
        }
        public async Task<bool> UpdateCommunicationProcessQueue(ProcessQueueModel model)
        {
            bool response = false;
            try
            {
                var queue = await context.RcmNphiesprocessQueues
                                                                .FirstOrDefaultAsync(predicate => predicate.OrganizationId == model.OrginzationId
                                                                                && predicate.FacilityId == model.FacilityId
                                                                                && predicate.ProcessId == model.ProcessId
                                                                );
                if (queue != null)
                {
                    if (queue.CriteriaType == 6)
                    {
                        var services = context.RcmClaimServicesDetails.AsNoTracking()
                                                                                                                                                                        .Where(claim => claim.OrganizationId == queue.OrganizationId
                                                                                                                                                                                        && claim.ReSubmissionStatus != null
                                                                                                                                                                                        && claim.ProcessId == model.ProcessId);
                        var totalServices = await services.CountAsync();
                        var totalCompleted = await services.CountAsync(claim => claim.ReSubmissionStatus == (byte)CommunicationStatus.Completed);
                        var totalError = await services.CountAsync(claim => claim.ReSubmissionStatus == (byte)CommunicationStatus.Rejected);

                        if (totalServices == (totalCompleted + totalError))
                        {
                            queue.IsProcessed = true;
                        }
                    }
                    else
                    {
                        queue.IsProcessed = true;
                    }

                    queue.ProcessedBy = 999;
                    queue.ProcessedOn = DateTime.Now;
                    context.RcmNphiesprocessQueues.Update(queue);
                    if (await context.SaveChangesAsync() > 0)
                        response = true;

                }
            }
            catch (Exception ex) { response = false; }
            return response;
        }

        public async Task<bool> UpdateRunningStateNphiesQueue(ProcessQueueModel model)
        {
            try
            {
                if (model == null)
                    return false;
                else if (model.ProcessId == 0)
                    return false;
                var response = await context.RcmNphiesprocessQueues.Where(x => x.ProcessId == model.ProcessId).FirstOrDefaultAsync();
                if (response != null)
                {
                    response.IsRunningProcess = true;
                    context.RcmNphiesprocessQueues.Update(response);
                    await context.SaveChangesAsync();
                    return true;

                }
                else
                    return false;

            }
            catch (Exception ex)
            {

                return false;
            }
        }
    }
}
