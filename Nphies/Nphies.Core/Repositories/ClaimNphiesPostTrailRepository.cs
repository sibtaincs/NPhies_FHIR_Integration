using Microsoft.EntityFrameworkCore;
using MongoDB.Driver.Linq;
using Nphies.Core.Data.Entities;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Nphies.Core.Repositories
{
    public class ClaimNphiesPostTrailRepository : IClaimNphiesPostTrailRepository
    {
        private readonly ZyklusCoreContext _context;

        public ClaimNphiesPostTrailRepository(ZyklusCoreContext context)
        {
            _context = context;
        }

        public async Task AddAsync(RcmClaimNphiesPostTrail claim)
        {
            _context.ClaimNphiesPostTrails.Add(claim);
            await _context.SaveChangesAsync();
        }

        public async Task<RcmClaimNphiesPostTrail> GetByClaimAsync(long claimId)
        {
            var nphiesLog = await _context.ClaimNphiesPostTrails
                                    .Where(npt => npt.ClaimId == claimId)
                                    .AsNoTracking()
                                    .FirstOrDefaultWithNoLockAsync();
            return nphiesLog;
        }

        public async Task<IEnumerable<RcmClaimNphiesPostTrail>> GetByClaimIdIdentifierAsync(long claimId, string claimIdentifier)
        {
            var nphiesLogs = await _context.ClaimNphiesPostTrails
                                        .Where(npt => npt.ClaimId == claimId && claimIdentifier.Contains(npt.ClaimIdentifier))
                                        .AsNoTracking()
                                        .ToListWithNoLockAsync();
            return nphiesLogs;

        }

        public Task UpdateAsync(RcmClaimNphiesPostTrail claim)
        {
            throw new System.NotImplementedException();
        }
    }
}
