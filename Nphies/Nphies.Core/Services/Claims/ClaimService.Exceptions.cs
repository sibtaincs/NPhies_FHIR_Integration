using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Nphies.Core.Models;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Nphies.Core.Services.Claims
{
    public partial class ClaimService
    {
        private delegate ValueTask<List<ClaimDetail>> ReturningClaimResonse();
        private delegate ValueTask<bool> ReturningClaimUpdateStatus();
        //private delegate ValueTask<bool> ReturningNphiesResponse();

        private async ValueTask<List<ClaimDetail>> TryCatch(ReturningClaimResonse returningClaimResonse)
        {
            try
            {
                return await returningClaimResonse();
            }
            catch (DbUpdateConcurrencyException dbUpdateConcurrencyException)
            {

                this.logger.LogCritical(dbUpdateConcurrencyException);
                return null;
            }
            catch (DbUpdateException dbUpdateException)
            {

                this.logger.LogCritical(dbUpdateException);
                return null;
            }
            catch (SqlException sqlException)
            {

                this.logger.LogCritical(sqlException);
                return null;
            }
            catch (Exception exception)
            {

                this.logger.LogCritical(exception);
                return null;
            }
        }

        private async ValueTask<bool> TryCatch(ReturningClaimUpdateStatus returningClaimUpdateStatus)
        {
            try
            {
                return await returningClaimUpdateStatus();
            }
            catch (DbUpdateConcurrencyException dbUpdateConcurrencyException)
            {

                this.logger.LogCritical(dbUpdateConcurrencyException);
                return false;
            }
            catch (DbUpdateException dbUpdateException)
            {
                    
                this.logger.LogCritical(dbUpdateException);
                return false;
            }
            catch (SqlException sqlException)
            {

                this.logger.LogCritical(sqlException);
                return false;
            }
            catch (Exception exception)
            {

                this.logger.LogCritical(exception);
                return false;
            }
        }

    }
}
