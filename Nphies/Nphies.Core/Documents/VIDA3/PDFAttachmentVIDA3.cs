using Microsoft.EntityFrameworkCore;
using Nphies.Core.Models;
using Nphies.Core.Services.PDFAttachment;
using System.Runtime.InteropServices;
using System.Security.Principal;
using System;
using System.Threading.Tasks;
using System.Configuration;
using Nphies.Core.Data.Entities;
using Nphies.Core.Models.Configurations;
using Microsoft.Extensions.Configuration;
using System.Transactions;
using Microsoft.Data.SqlClient;
using System.Data;
using System.Linq;
using Nphies.Core.Services.Logger;
using Cyclus.Storage.Attachment;
using Cyclus.Storage.Attachment.Enums;
using System.Collections.Generic;

namespace Nphies.Core.Documents.VIDA3
{
    public class PDFAttachmentVIDA3 : Document
    {

        public PDFAttachmentVIDA3(IConfiguration configuration, ZyklusCoreContext zyklusCoreContext, ILogService logService)
            : base(configuration, zyklusCoreContext, logService)
        {
        }

        public override async Task<DocumentResponse> GetDocument(long claimId)
        {
            try
            {
                DocumentConfiguration documentConfiguration = Configuration.Get<DocumentConfiguration>();

                var fileAttachment = new DocumentResponse();

                var claimDetails = await GetClaimDetailsAsync(claimId);

                string query = documentConfiguration.documentQuery;

                byte[] fileData = null;


                if (claimDetails.fileStorageProvider == 1)
                {
                    // Use SQL storage only
                    fileData = await GetDocumentFromSQL(query, claimDetails);
                }
                else if (claimDetails.fileStorageProvider == 2)
                {
                    // Use MongoDB storage only
                    fileData = await GetDocumentFromMongoDB(claimDetails, claimId);
                }
                else if (claimDetails.fileStorageProvider == 0)
                {
                    // Try SQL first, then MongoDB if not found
                    fileData = await GetDocumentFromSQL(query, claimDetails);
                    if (fileData == null)
                    {
                        fileData = await GetDocumentFromMongoDB(claimDetails, claimId);
                    }
                }
                else
                {
                    fileData = await GetDocumentFromSQL(query, claimDetails);
                }

                fileAttachment.byteFile = fileData;
                return fileAttachment;
            }
            catch (Exception ex)
            {
                ex.Message.ToString();
            }
            return null;
        }

        private async Task<byte[]> GetDocumentFromSQL(string query, Vida3Request claimDetails)
        {
            try
            {
                DocumentConfiguration documentConfiguration = Configuration.Get<DocumentConfiguration>();
                using (TransactionScope ts = new TransactionScope(TransactionScopeOption.Required, TimeSpan.FromHours(1)))
                {
                    try
                    {
                        using (SqlConnection conn = new SqlConnection(documentConfiguration.FileDBConnection))
                        {
                            conn.Open();

                            using (SqlCommand cmd = new SqlCommand(query, conn))
                            {
                                cmd.Parameters.Add("@SetupID", SqlDbType.VarChar).Value = claimDetails.setupid;
                                cmd.Parameters.Add("@ProjectID", SqlDbType.SmallInt).Value = claimDetails.project;
                                cmd.Parameters.Add("@EncounterNo", SqlDbType.Int).Value = claimDetails.encounterNo;
                                var objData = cmd.ExecuteScalar();
                                if (objData != null)
                                {
                                    using (SqlDataReader reader = cmd.ExecuteReader())
                                    {
                                        if (reader.HasRows)
                                        {
                                            reader.Read();
                                            return reader["FileData"] as byte[];
                                        }
                                    }
                                }
                            }
                        }
                    }
                    catch (Exception e)
                    {
                        Console.WriteLine(e.ToString());
                    }
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e.ToString());
            }
            return null;
        }

        private async Task<byte[]> GetDocumentFromMongoDB(Vida3Request claimDetails, long claimId)
        {
            try
            {
                var provider = FileStorageProviderFactory.GetProvider(FileStorageProviderType.Mongo);
                if (provider != null)
                {
                    var documents = await provider.GetDocumentsByClaim(claimDetails.encounterNo, claimDetails.setupid, long.Parse(claimDetails.project), claimId.ToString());
                    if (documents != null && documents.Any())
                    {
                        // If there's only one document, return its file data
                        if (documents.Count == 1)
                        {
                            return documents.First().FileData;
                        }
                        else
                        {
                            var allFileData = new List<byte>();
                            foreach (var doc in documents)
                            {
                                if (doc.FileData != null)
                                {
                                    allFileData.AddRange(doc.FileData);
                                }
                            }
                            return allFileData.ToArray();
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
            }
            return null;
        }

        private async Task<Vida3Request> GetClaimDetailsAsync(long claimId)
        {
            return await (from claim in _dbContext.RcmClaims
                          where claim.ClaimId == claimId
                          join facility in _dbContext.RcmFacilities
                          on claim.FacilityId equals facility.FacilityId into facilities
                          from facility in facilities.DefaultIfEmpty()
                          select new Vida3Request
                          {
                              encounterNo = claim.EncounterNo,
                              project = facility.ExternalCode,
                              setupid = facility.ExternalCode2,
                              fileStorageProvider = facility.FileStorageProvider
                          })
                          .AsNoTracking()
                          .FirstOrDefaultAsync();
        }
    }


}