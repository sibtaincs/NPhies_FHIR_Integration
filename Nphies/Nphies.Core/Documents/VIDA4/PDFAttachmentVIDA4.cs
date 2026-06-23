using Microsoft.Extensions.Configuration;
using Nphies.Core.Data.Entities;
using Nphies.Core.Models;
using System.Net.Http;
using System.Net;
using System.Text;
using System;
using System.Threading.Tasks;
using Nphies.Core.Models.Configurations;
using System.Linq;
using Newtonsoft.Json;
using Microsoft.EntityFrameworkCore;
using System.Net.Http.Json;
using Nphies.Core.Services.Logger;
using Nphies.Core.Helper;
using System.Collections.Generic;
using static Hl7.Fhir.Model.Claim;
using Hl7.Fhir.Model;

namespace Nphies.Core.Documents.VIDA4
{
    public class PDFAttachmentVIDA4 : Document
    {
        private readonly IConfiguration configuration;
        public PDFAttachmentVIDA4(IConfiguration configuration, ZyklusCoreContext zyklusCoreContext, ILogService logService)
            : base(configuration, zyklusCoreContext, logService)
        {
            this.configuration = configuration;
        }

        public override async Task<DocumentResponse> GetDocument(long claimId)
        {
            try
            {
                    
               // DocumentConfiguration documentConfiguration = Configuration.Get<DocumentConfiguration>();
                DocumentConfiguration documentConfiguration = new DocumentConfiguration {
                     ExternalURL = this.configuration.GetValue<string>("ExternalURL"),
                      DocumentPassword = this.configuration.GetValue<string>("DocumentPassword"),
                      DocumentUser = this.configuration.GetValue<string>("DocumentUsername")
                };
                var result = new DocumentResponse();

                var claimDetails = await GetClaimDetailsAsync(claimId);
                var SubmittedStatus = new List<Status>
                {
                    Status.Nphies_Queued,
                    Status.Nphies_Pended,
                    Status.Nphies_Error,
                    Status.Nphies_Perror,
                    Status.Nphies_OutCome,
                    Status.Nphies_Cancelled,
                    Status.Submitted
                };

                if (SubmittedStatus.Contains((Status)claimDetails.claimStatus))
                {
                    await GetAttachmentfromJsonRequest(result, claimDetails);

                    if (result.byteFile != null)
                    {
                        return result;
                    }
                }
        //        claimDetails.encounterType = claimDetails.OriginalEncounterNumber
        //            .StartsWith("OPD", StringComparison.OrdinalIgnoreCase)
        //          ? "OPD"
        //:          "INP";

               
                var encounterNumber = claimDetails.OriginalEncounterNumber;
                var payload = !string.IsNullOrWhiteSpace(encounterNumber)
                           ? new
                           {
                               encounterNumber = encounterNumber.Split('-')[1],
                               encounterType = encounterNumber.Split('-')[0]
                           }
                           : null;
                //var payload = new
                //{
                //    encounterNumber = claimDetails.OriginalEncounterNumber,
                //    encounterType = claimDetails.encounterType,
                //};
                using (var httpClient = CreateHttpClient(documentConfiguration))
                {
                    var baseURL = documentConfiguration.ExternalURL;
                    httpClient.BaseAddress = new Uri(baseURL);
                    
                    var httpResponse = await httpClient.PostAsJsonAsync("cyclus-attachments", payload);

                    if (httpResponse != null && httpResponse.IsSuccessStatusCode)
                    {
                        var jsonResponse = await httpResponse.Content.ReadAsStringAsync();
                        result = JsonConvert.DeserializeObject<DocumentResponse>(jsonResponse);

                        if (result != null)
                        {
                            result.byteFile = Convert.FromBase64String(result.pdf);
                            result.pdf = null;
                        }

                        return result;
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }

            return null;
        }

        private async System.Threading.Tasks.Task GetAttachmentfromJsonRequest(DocumentResponse result, Vida4Request claimDetails)
        {
            var bundle = await _logService.GetClaimRequestLog(claimDetails.ClaimIdentifier);
            var parsedObject = Hl7Parser.ParseBundle(bundle.Requestjson);
            foreach (var entry in parsedObject.Entry)
            {
                // Access the full URL of the entry.
                string fullUrl = entry.FullUrl;
                Resource resource = entry.Resource;
                if (resource is Claim claim)
                {
                    if (claim.SupportingInfo is List<SupportingInformationComponent>)
                    {
                        foreach (var supportingInfo in claim.SupportingInfo)
                        {
                            if (supportingInfo.Value is Attachment)
                            {
                                var attachment = (Attachment)supportingInfo.Value;
                                result.byteFile = attachment.Data;
                            }
                        }
                    }
                }
            }
        }

        private async Task<Vida4Request> GetClaimDetailsAsync(long claimId)
        {


            return await (from claim in _dbContext.RcmClaims
                          where claim.ClaimId == claimId
                          join clinic in _dbContext.RcmClinics
                          on claim.ClinicId equals clinic.ClinicId into clinics
                          from clinic in clinics.DefaultIfEmpty()
                          select new Vida4Request
                          {
                              encounterNumber = Convert.ToInt64(claim.EncounterNo),
                              encounterType = EncounterTypeHelper.IsERClinic(clinic.ClinicId, Configuration) ? "INP" : "OPD",
                              // encounterType = clinic.ExternalCode == "10" ? "ER" : "OPD",
                              ClaimIdentifier = claim.ClaimIdentifier,
                              claimStatus = claim.Status,
                              OriginalEncounterNumber = claim.OriginalEncounter
                          })
                          .AsNoTracking()
                          .FirstOrDefaultAsync();
        }

        private HttpClient CreateHttpClient(DocumentConfiguration documentConfiguration)
        {
            var httpClientHandler = new HttpClientHandler
            {
                Credentials = new NetworkCredential(documentConfiguration.DocumentUser, documentConfiguration.DocumentPassword)
            };

            return new HttpClient(httpClientHandler);
        }
        //private bool IsERClinic(int clinicId)
        //{
        //    //int clinicId = externalCode;
        //    ErClinicConfiguration erClinicConfiguration = Configuration.Get<ErClinicConfiguration>();
        //    int[] clinics = Array.ConvertAll(erClinicConfiguration.ErClinics.Split(','), int.Parse);

        //   return clinics.Contains(clinicId);

        //}
    }
}
