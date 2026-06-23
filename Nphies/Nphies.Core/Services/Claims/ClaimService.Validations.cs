namespace Nphies.Core.Services.Claims
{
    public partial class ClaimService
    {

        private void Validate()
        {
            //#region Validation
            //var facilities = await context.RcmFacilities
            //                 .Where(facility => facility.OrganizationId == organizationId
            //                 && facility.FacilityId == facilityId
            //                 && facility.IsNphiesEnable == true
            //                 && facility.IsActive).ToArrayAsync();
            //if (!facilities.Any())
            //{
            //    //claimResponse.Message = "Nphies is Not Enabled for this Facility.";
            //    //return claimResponse;
            //}

            //var payers = await context.RcmPayers.AsNoTracking()
            //    .Where(payer => payer.OrganizationId == organizationId
            //    //&& adaptors.Contains(payer.ClaimType.ToString())
            //    // && payer.ClaimType.Contains(adaptors)
            //     && payer.IsNphiesEnable == true).ToArrayAsync();
            //if (!payers.Any())
            //{
            //    //claimResponse.Message = "Nphies is Not Enabled for this Payer.";
            //    //return claimResponse;
            //}
            //#endregion
        }

        private static void Invlaid(int id)
        {

        }
    }
}
