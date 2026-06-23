
using Microsoft.Extensions.Configuration;
using Nphies.Core.Data.Entities;
using Nphies.Core.Models;
using Nphies.Core.Services.Logger;
using System.Configuration;
using System.Threading.Tasks;

namespace Nphies.Core.Documents
{
    public abstract class Document
    {
        protected readonly IConfiguration Configuration;
        protected readonly ZyklusCoreContext _dbContext;
        protected readonly ILogService _logService;
        public Document(IConfiguration configuration,ZyklusCoreContext zyklusCoreContext, ILogService logService)
        {
            this.Configuration = configuration;
            this._dbContext = zyklusCoreContext;
            _logService = logService;
        }
        public abstract Task<DocumentResponse> GetDocument(long claimId);
    }
}
