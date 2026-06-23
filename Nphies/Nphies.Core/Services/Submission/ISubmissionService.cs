using Nphies.Core.DTOs;
using System.Threading.Tasks;

namespace Nphies.Core.Services.Submission
{
    public interface ISubmissionService
    {
        Task<SubmitClaimResponse> SubmitClaimWithSameBundleAsync(SubmitClaimRequest request);
    }
}
