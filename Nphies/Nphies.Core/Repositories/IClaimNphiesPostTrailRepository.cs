using Nphies.Core.Data.Entities;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Nphies.Core.Repositories
{
    public interface IClaimNphiesPostTrailRepository
    {
        Task<RcmClaimNphiesPostTrail> GetByClaimAsync(long claimId);
        Task<IEnumerable<RcmClaimNphiesPostTrail>> GetByClaimIdIdentifierAsync(long claimId, string claimIdentifier);
        Task AddAsync(RcmClaimNphiesPostTrail claim);
        Task UpdateAsync(RcmClaimNphiesPostTrail claim);
    }
}
