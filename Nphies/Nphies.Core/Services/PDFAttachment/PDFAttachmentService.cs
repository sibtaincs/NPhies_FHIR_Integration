using Microsoft.Extensions.Configuration;
using Nphies.Core.Data.Entities;
using Nphies.Core.Models;
using Nphies.Core.Repositories;
using System.Net.Http;
using System.Net;
using System.Runtime.InteropServices;
using System.Security.Principal;
using System.Text;
using System;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using Newtonsoft.Json;
using System.Transactions;
using Microsoft.Data.SqlClient;
using System.Data;
using Nphies.Core.Documents;
using Nphies.Core.Documents.VIDA4;
using Nphies.Core.Documents.VIDA3;
using Nphies.Core.Services.Logger;

namespace Nphies.Core.Services.PDFAttachment
{
    public class PDFAttachmentService : IPDFAttachmentService
    {
        private readonly ZyklusCoreContext _dbContext;
        private readonly IConfiguration _config;
        protected readonly ILogService _logService;

        public PDFAttachmentService(ZyklusCoreContext dbContext, IConfiguration config, ILogService logService)
        {
            _dbContext = dbContext;
            _config = config;
            _logService = logService;
        }
        public async Task<Document> GetPDFAttachment(long claimId)
        {
            var externalCode = await getExternalCodeforFacility(claimId);

            var vida4Facility = _config["Vida4Facilities"];
            if (!string.IsNullOrEmpty(vida4Facility))
            {
                var vida4Facilities = Array.ConvertAll(vida4Facility.Split(','), Convert.ToInt32);
                var vida4FacilitiesString = vida4Facilities.Select(x => x.ToString()).ToArray();
                if (vida4FacilitiesString.Contains(externalCode))
                    return new PDFAttachmentVIDA4(_config, _dbContext, _logService);
            }
            return new PDFAttachmentVIDA3(_config, _dbContext, _logService);
        }


        private async Task<string> getExternalCodeforFacility(long claimId)
        {

            string Externalcode = (await _dbContext.RcmClaims
                          .AsNoTracking()
                          .Where(x => x.ClaimId == claimId)

                          .FirstOrDefaultAsync())?.FacilityId.ToString();

            return Externalcode;
        }



    }
}
