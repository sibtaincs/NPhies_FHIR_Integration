using Hl7.Fhir.Model;
using Hl7.Fhir.Serialization;
using Nphies.Core.Models;
using System.Collections.Generic;
using System.Text;
using System;

namespace Nphies.Core.Helper
{
    public class ResponseParsing
    {

        public Models.BatchResponse CreateResponse(Hl7.Fhir.Model.Bundle ResponseBundle, Hl7.Fhir.Model.Bundle requestBundle)
        {
            Models.BatchResponse batchResponse = null;
            try
            {
                batchResponse = new Models.BatchResponse();
                var claimDetailResponse = new List<Models.ClaimDetailResponse>();
                int bundleEntry = 0;
                foreach (var item in ResponseBundle.Entry)
                {
                    if (item.Resource.TypeName == "MessageHeader")
                    {
                        var messageheader = (Hl7.Fhir.Model.MessageHeader)item.Resource;
                        //if (messageheader.Response.Code.ToString() == "ok")
                        //    continue;
                    }
                    else if (item.Resource.TypeName == "Bundle")
                    {
                        var bundle = (Hl7.Fhir.Model.Bundle)item.Resource;

                        foreach (var entry in bundle.Entry)
                        {
                            if (entry.Resource.TypeName == "ClaimResponse")
                            {
                                var claimResponse = (Hl7.Fhir.Model.ClaimResponse)entry.Resource;
                                if (claimResponse.Outcome == Hl7.Fhir.Model.ClaimProcessingCodes.Error)
                                {

                                    var identifier = claimResponse.Request.Identifier.Value;
                                    string Errors = "";

                                    Errors = GetErrors(claimResponse.Error, requestBundle, bundleEntry);
                                    var itemError = GetItemErrors(claimResponse.Error, requestBundle, bundleEntry);

                                    if (claimResponse.Meta is Meta meta)
                                    {
                                        if (meta.Tag != null && meta.Tag.Count > 0)
                                        {
                                            string nphiesError = "NPHIES generated";
                                            foreach (Coding tag in meta.Tag)
                                            {
                                                if (tag.Code.ToLower().Trim().Equals(nphiesError.ToLower().Trim()))
                                                {
                                                    claimDetailResponse.Add(
                                                       new Models.ClaimDetailResponse()
                                                       {
                                                           ClaimIdentifier = identifier,
                                                           Remarks = Errors,
                                                           Status = "Error",
                                                           ResponseBundle = bundle,
                                                           BundleID = bundle.Id,
                                                           ClaimItemDetail = itemError
                                                       }
                                                       );
                                                    break;
                                                }

                                            }
                                        }
                                        else
                                        {
                                            claimDetailResponse.Add(
                                                       new Models.ClaimDetailResponse()
                                                       {
                                                           ClaimIdentifier = identifier,
                                                           Remarks = Errors,
                                                           Status = "perror",
                                                           ResponseBundle = bundle,
                                                           BundleID = bundle.Id,
                                                           ClaimItemDetail = itemError
                                                       }
                                                       );
                                        }

                                    }
                                    else
                                    {
                                        claimDetailResponse.Add(
                                                      new Models.ClaimDetailResponse()
                                                      {
                                                          ClaimIdentifier = identifier,
                                                          Remarks = Errors,
                                                          Status = "Error",
                                                          ResponseBundle = bundle,
                                                          BundleID = bundle.Id,
                                                          ClaimItemDetail = itemError
                                                      }
                                                      );
                                    }

                                }
                                else if (claimResponse.Outcome == Hl7.Fhir.Model.ClaimProcessingCodes.Queued)
                                {
                                    var identifier = claimResponse.Request.Identifier.Value;
                                    claimDetailResponse.Add(
                                        new Models.ClaimDetailResponse()
                                        {
                                            ClaimIdentifier = identifier,
                                            Remarks = "",
                                            Status = "Queued",
                                            ResponseBundle = bundle,
                                            BundleID = bundle.Id
                                        });
                                }
                            }

                        }
                    }
                    else if (item.Resource.TypeName == "ClaimResponse")
                    {
                        var claimResponse = (Hl7.Fhir.Model.ClaimResponse)item.Resource;
                        if (claimResponse.Outcome == Hl7.Fhir.Model.ClaimProcessingCodes.Error)
                        {
                            var identifier = claimResponse.Request.Identifier.Value;
                            string Errors = "";
                            Errors = GetErrors(claimResponse.Error, requestBundle, bundleEntry);

                            if (claimResponse.Meta is Meta meta)
                            {
                                if (meta.Tag != null && meta.Tag.Count > 0)
                                {
                                    string nphiesError = "NPHIES generated";
                                    foreach (Coding tag in meta.Tag)
                                    {
                                        if (tag.Code.ToLower().Trim().Equals(nphiesError.ToLower().Trim()))
                                        {
                                            claimDetailResponse.Add(
                                               new Models.ClaimDetailResponse()
                                               {
                                                   ClaimIdentifier = identifier,
                                                   Remarks = Errors,
                                                   Status = "Error",
                                                   BundleID = ResponseBundle.Id,
                                                   ResponseBundle = ResponseBundle
                                               }
                                              );
                                            break;
                                        }

                                    }
                                }
                                else
                                {


                                    claimDetailResponse.Add(
                                       new Models.ClaimDetailResponse()
                                       {
                                           ClaimIdentifier = identifier,
                                           Remarks = Errors,
                                           Status = "perror",
                                           BundleID = ResponseBundle.Id,
                                           ResponseBundle = ResponseBundle
                                       }
                                       );
                                }

                            }
                            else
                            {
                                claimDetailResponse.Add(
                                new Models.ClaimDetailResponse()
                                {
                                    ClaimIdentifier = identifier,
                                    Remarks = Errors,
                                    Status = "Error",
                                    BundleID = ResponseBundle.Id,
                                    ResponseBundle = ResponseBundle
                                }
                                );
                            }



                        }
                        else if (claimResponse.Outcome == Hl7.Fhir.Model.ClaimProcessingCodes.Queued)
                        {
                            var identifier = claimResponse.Request.Identifier.Value;

                            bool isPended = GetPended(claimResponse);

                            claimDetailResponse.Add(
                                new Models.ClaimDetailResponse()
                                {
                                    ClaimIdentifier = identifier,
                                    Remarks = "Queued",
                                    Status = "Queued",
                                    BundleID = ResponseBundle.Id,
                                    ResponseBundle = ResponseBundle,
                                    IsPended = isPended
                                });
                        }
                        else if (claimResponse.Outcome == Hl7.Fhir.Model.ClaimProcessingCodes.Complete)
                        {
                            var identifier = claimResponse.Request.Identifier.Value;

                            var extention = GetClaimExtention(claimResponse, "Completed");

                            List<ClaimItemDetail> claimItemResponses = GetClaimItemDetail(claimResponse);

                            claimDetailResponse.Add(
                                new Models.ClaimDetailResponse()
                                {
                                    ClaimIdentifier = identifier,
                                    Remarks = extention.ClaimRemark,
                                    Status = extention.ClaimStatus,
                                    BundleID = ResponseBundle.Id,
                                    ResponseBundle = ResponseBundle,
                                    TotalApproved = extention.TotalApproved,
                                    ClaimItemDetail = claimItemResponses
                                });
                        }
                    }
                    bundleEntry++;
                }
                batchResponse.ClaimDetailResponse = claimDetailResponse;

                return batchResponse;
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }

        private static ResponseExtention GetClaimExtention(ClaimResponse claimResponse, string claimStatus)
        {
            StringBuilder claimRemark = new StringBuilder();

            if (claimResponse.Extension is List<Extension> extensions)
            {
                foreach (var ext in extensions)
                {
                    if (ext.Url == "http://nphies.sa/fhir/ksa/nphies-fs/StructureDefinition/extension-adjudication-outcome")
                    {
                        claimStatus = ((CodeableConcept)ext.Value).Coding[0].Code;
                    }
                    else if (ext.Url == "http://nphies.sa/fhir/ksa/nphies-fs/StructureDefinition/extension-adjudication-reissue")
                    {
                        claimRemark.Append(((CodeableConcept)ext.Value).Coding[0].Code);
                        claimRemark.Append(" ");
                    }
                }
            }

            var totalApproved = string.Empty;
            if (claimResponse.Total != null && claimResponse.Total.Count > 0)
            {
                foreach (var adj in claimResponse.Total)
                {
                    if (adj.Category.Coding[0].Code == "benefit")
                    {
                        totalApproved = "" + adj.Amount.Value;
                    }
                }
            }

            ResponseExtention responseExtention = new ResponseExtention();
            responseExtention.ClaimStatus = claimStatus;
            responseExtention.ClaimRemark = claimRemark.ToString();
            responseExtention.TotalApproved = totalApproved;

            return responseExtention;
        }

        private static List<ClaimItemDetail> GetClaimItemDetail(ClaimResponse claimResponse)
        {
            var claimItemResponses = new List<ClaimItemDetail>();

            if (claimResponse.Item != null && claimResponse.Item.Count > 0)
            {
                foreach (var service in claimResponse.Item)
                {
                    ClaimItemDetail itemResponse = new ClaimItemDetail();
                    itemResponse.Sequence = Convert.ToInt32(service.ItemSequence);

                    if (service.Extension != null && service.Extension.Count > 0)
                    {
                        foreach (var ext in service.Extension)
                        {
                            if (ext.Url == "http://nphies.sa/fhir/ksa/nphies-fs/StructureDefinition/extension-adjudication-outcome")
                            {
                                itemResponse.Status = ((CodeableConcept)ext.Value).Coding[0].Code;
                            }
                            else if (ext.Url == "http://nphies.sa/fhir/ksa/nphies-fs/StructureDefinition/extension-patientInvoice")
                            {
                                itemResponse.InvoiceNo = ((Identifier)ext.Value).Value;
                            }
                        }
                    }

                    if (service.Adjudication != null && service.Adjudication.Count > 0)
                    {
                        foreach (var adj in service.Adjudication)
                        {
                            if (adj.Category.Coding[0].Code == "benefit")
                            {
                                itemResponse.BenefitAmount = "" + adj.Amount.Value;
                            }

                            if (adj.Category.Coding[0].Code == "approved-quantity")
                            {
                                itemResponse.ApprovedQuantity = adj.Value != null ? Convert.ToString(adj.Value) : "0";
                            }

                            if (adj.Reason != null && adj.Reason.Coding != null && adj.Reason.Coding != null)
                            {
                                itemResponse.ErrorCode = adj.Reason.Coding[0].Code;
                                itemResponse.Error = adj.Reason.Coding[0].Display;
                            }
                        }
                    }

                    claimItemResponses.Add(itemResponse);
                }
            }

            return claimItemResponses;
        }

        private static string RetriveErrorMessage(ClaimResponse claimResponse, string Errors)
        {
            return !string.IsNullOrWhiteSpace(claimResponse.Disposition) ? claimResponse.Disposition : Errors;
        }

        private string GetErrors(List<Hl7.Fhir.Model.ClaimResponse.ErrorComponent> errorComponents, Hl7.Fhir.Model.Bundle reqbundle, int bundleEntry = 0)
        {
            return GetDetailsErrors(errorComponents, reqbundle, bundleEntry).Remarks;
        }


        private List<ClaimItemDetail> GetItemErrors(List<Hl7.Fhir.Model.ClaimResponse.ErrorComponent> errorComponents, Hl7.Fhir.Model.Bundle reqbundle, int bundleEntry = 0)
        {
            return GetDetailsErrors(errorComponents, reqbundle, bundleEntry).ClaimItemDetail;
        }

        private ClaimDetailResponse GetDetailsErrors(List<Hl7.Fhir.Model.ClaimResponse.ErrorComponent> errorComponents, Hl7.Fhir.Model.Bundle reqbundle, int bundleEntry = 0)
        {

            string ErrorDetails = string.Empty;
            string Error = string.Empty;
            ClaimDetailResponse retvalue = new ClaimDetailResponse();
            retvalue.ClaimItemDetail = new List<ClaimItemDetail>();
            ClaimItemDetail claimItemDetail = new ClaimItemDetail();
            try
            {
                foreach (var error in errorComponents)
                {
                    Error = string.Empty;

                    var errorCode = error.Code.Coding[0].Code;
                    var errorDisplay = error.Code.Coding[0].Display;

                    var errorExtension = (error.Code.Coding[0].Extension != null && error.Code.Coding[0].Extension.Count > 0 ? (Environment.NewLine + error.Code.Coding[0].Extension[0].Value) : "");

                    if (!string.IsNullOrEmpty(errorExtension) && errorExtension.Contains("productOrService"))
                    {
                        var bdc = errorExtension.Split('.');
                        var bundleID = bdc[1].Replace("entry[", "").Replace("]", "");
                        string itemid = "";
                        string detailid = "";
                        foreach (var d in bdc)
                        {
                            if (d.StartsWith("item"))
                            {
                                itemid = d.Replace("item[", "").Replace("]", "");
                                if (!errorExtension.Contains("detail"))
                                    break;
                            }
                            if (d.StartsWith("detail"))
                            {
                                detailid = d.Replace("detail[", "").Replace("]", "");
                            }
                        }


                        Hl7.Fhir.Model.Claim claim = null;
                        if (reqbundle.Entry[1].Resource.TypeName == "Claim")
                        {
                            claim = (Hl7.Fhir.Model.Claim)reqbundle.Entry[1].Resource;
                        }
                        else if (reqbundle.Entry[bundleEntry].Resource.TypeName == "Bundle")
                        {
                            var bundle = (Hl7.Fhir.Model.Bundle)reqbundle.Entry[bundleEntry].Resource;
                            claim = (Hl7.Fhir.Model.Claim)bundle.Entry[1].Resource;
                        }


                        //var claim = (Hl7.Fhir.Model.Claim)reqbundle.Entry[1].Resource;

                        var response = claim.Item[Convert.ToInt32(itemid)];
                        var Sequence = response.Sequence;

                        if (!string.IsNullOrEmpty(detailid))
                        {
                            var detailComp = response.Detail[Convert.ToInt32(detailid)];

                            if (detailComp.ProductOrService.Coding != null && detailComp.ProductOrService.Coding.Count == 2)
                            {
                                if (!Error.Contains(detailComp.ProductOrService.Coding[0].Code == null ? "" : detailComp.ProductOrService.Coding[0].Code)
                                    && !Error.Contains(detailComp.ProductOrService.Coding[1].Code == null ? "" : detailComp.ProductOrService.Coding[1].Code))
                                {
                                    Error += errorCode + ", " + errorDisplay + Environment.NewLine;
                                    Error += Environment.NewLine + "Code='" + detailComp.ProductOrService.Coding[0].Code + "', System='" + detailComp.ProductOrService.Coding[0].System + "', Display='" + detailComp.ProductOrService.Coding[0].Display + Environment.NewLine;
                                    Error += Environment.NewLine + "Code='" + detailComp.ProductOrService.Coding[1].Code + "', System='" + detailComp.ProductOrService.Coding[1].System + "', Display='" + detailComp.ProductOrService.Coding[1].Display + Environment.NewLine;
                                    Error += Environment.NewLine + "===========================================================================" + Environment.NewLine;

                                    claimItemDetail = new ClaimItemDetail();
                                    claimItemDetail.Code = detailComp.ProductOrService.Coding[0].Code;
                                    claimItemDetail.ErrorCode = errorCode;
                                    claimItemDetail.System = detailComp.ProductOrService.Coding[0].System;
                                    claimItemDetail.Display = detailComp.ProductOrService.Coding[0].Display;
                                    claimItemDetail.Error = errorDisplay;
                                    claimItemDetail.Sequence = Sequence;
                                    retvalue.ClaimItemDetail.Add(claimItemDetail);

                                    claimItemDetail = new ClaimItemDetail();
                                    claimItemDetail.Code = detailComp.ProductOrService.Coding[1].Code;
                                    claimItemDetail.ErrorCode = errorCode;
                                    claimItemDetail.System = detailComp.ProductOrService.Coding[1].System;
                                    claimItemDetail.Display = detailComp.ProductOrService.Coding[1].Display;
                                    claimItemDetail.Error = errorDisplay;
                                    claimItemDetail.Sequence = Sequence;
                                    retvalue.ClaimItemDetail.Add(claimItemDetail);
                                }
                                else if ((detailComp.ProductOrService.Coding[0].Code == null || detailComp.ProductOrService.Coding[0].Code == "") && !ErrorDetails.Contains(detailComp.ProductOrService.Coding[1].Code == null ? "" : detailComp.ProductOrService.Coding[1].Code))
                                {
                                    Error += errorCode + ", " + errorDisplay + Environment.NewLine;
                                    Error += Environment.NewLine + "Code='" + response.ProductOrService.Coding[0].Code + "', System='" + response.ProductOrService.Coding[0].System + "', Display='" + response.ProductOrService.Coding[0].Display + Environment.NewLine;
                                    Error += Environment.NewLine + "Code='" + response.ProductOrService.Coding[1].Code + "', System='" + response.ProductOrService.Coding[1].System + "', Display='" + response.ProductOrService.Coding[1].Display + Environment.NewLine;
                                    Error += Environment.NewLine + "===========================================================================" + Environment.NewLine;

                                    claimItemDetail = new ClaimItemDetail();
                                    claimItemDetail.Code = response.ProductOrService.Coding[0].Code;
                                    claimItemDetail.ErrorCode = errorCode;
                                    claimItemDetail.System = response.ProductOrService.Coding[0].System;
                                    claimItemDetail.Display = response.ProductOrService.Coding[0].Display;
                                    claimItemDetail.Error = errorDisplay;
                                    claimItemDetail.Sequence = Sequence;
                                    retvalue.ClaimItemDetail.Add(claimItemDetail);

                                    claimItemDetail = new ClaimItemDetail();
                                    claimItemDetail.Code = response.ProductOrService.Coding[1].Code;
                                    claimItemDetail.ErrorCode = errorCode;
                                    claimItemDetail.System = response.ProductOrService.Coding[1].System;
                                    claimItemDetail.Display = response.ProductOrService.Coding[1].Display;
                                    claimItemDetail.Error = errorDisplay;
                                    claimItemDetail.Sequence = Sequence;
                                    retvalue.ClaimItemDetail.Add(claimItemDetail);
                                }
                            }
                            else
                            {
                                foreach (var dt in detailComp.ProductOrService.Coding)
                                {

                                    Error += errorCode + ", " + errorDisplay + Environment.NewLine;
                                    Error += Environment.NewLine + "Code='" + dt.Code + "', System='" + dt.System + "', Display='" + dt.Display + Environment.NewLine;
                                    Error += Environment.NewLine + "===========================================================================" + Environment.NewLine;

                                    claimItemDetail = new ClaimItemDetail();
                                    claimItemDetail.Code = dt.Code;
                                    claimItemDetail.ErrorCode = errorCode;
                                    claimItemDetail.System = dt.System;
                                    claimItemDetail.Display = dt.Display;
                                    claimItemDetail.Error = errorDisplay;
                                    claimItemDetail.Sequence = Sequence;
                                    retvalue.ClaimItemDetail.Add(claimItemDetail);

                                }
                            }

                        }
                        else
                        {
                            if (response.ProductOrService.Coding != null && response.ProductOrService.Coding.Count == 2)
                            {
                                if (!Error.Contains(response.ProductOrService.Coding[0].Code == null ? "" : response.ProductOrService.Coding[0].Code)
                                    && !Error.Contains(response.ProductOrService.Coding[1].Code == null ? "" : response.ProductOrService.Coding[1].Code))
                                {
                                    Error += errorCode + ", " + errorDisplay + Environment.NewLine;
                                    Error += Environment.NewLine + "Code='" + response.ProductOrService.Coding[0].Code + "', System='" + response.ProductOrService.Coding[0].System + "', Display='" + response.ProductOrService.Coding[0].Display + Environment.NewLine;
                                    Error += Environment.NewLine + "Code='" + response.ProductOrService.Coding[1].Code + "', System='" + response.ProductOrService.Coding[1].System + "', Display='" + response.ProductOrService.Coding[1].Display + Environment.NewLine;
                                    Error += Environment.NewLine + "===========================================================================" + Environment.NewLine;

                                    claimItemDetail = new ClaimItemDetail();
                                    claimItemDetail.Code = response.ProductOrService.Coding[0].Code;
                                    claimItemDetail.ErrorCode = errorCode;
                                    claimItemDetail.System = response.ProductOrService.Coding[0].System;
                                    claimItemDetail.Display = response.ProductOrService.Coding[0].Display;
                                    claimItemDetail.Error = errorDisplay;
                                    claimItemDetail.Sequence = Sequence;
                                    retvalue.ClaimItemDetail.Add(claimItemDetail);

                                    claimItemDetail = new ClaimItemDetail();
                                    claimItemDetail.Code = response.ProductOrService.Coding[1].Code;
                                    claimItemDetail.ErrorCode = errorCode;
                                    claimItemDetail.System = response.ProductOrService.Coding[1].System;
                                    claimItemDetail.Display = response.ProductOrService.Coding[1].Display;
                                    claimItemDetail.Error = errorDisplay;
                                    claimItemDetail.Sequence = Sequence;
                                    retvalue.ClaimItemDetail.Add(claimItemDetail);
                                }
                                else if ((response.ProductOrService.Coding[0].Code == null || response.ProductOrService.Coding[0].Code == "") && !Error.Contains(response.ProductOrService.Coding[1].Code == null ? "" : response.ProductOrService.Coding[1].Code))
                                {
                                    Error += errorCode + ", " + errorDisplay + Environment.NewLine;
                                    Error += Environment.NewLine + "Code='" + response.ProductOrService.Coding[0].Code + "', System='" + response.ProductOrService.Coding[0].System + "', Display='" + response.ProductOrService.Coding[0].Display + Environment.NewLine;
                                    Error += Environment.NewLine + "Code='" + response.ProductOrService.Coding[1].Code + "', System='" + response.ProductOrService.Coding[1].System + "', Display='" + response.ProductOrService.Coding[1].Display + Environment.NewLine;
                                    Error += Environment.NewLine + "===========================================================================" + Environment.NewLine;

                                    claimItemDetail = new ClaimItemDetail();
                                    claimItemDetail.Code = response.ProductOrService.Coding[0].Code;
                                    claimItemDetail.ErrorCode = errorCode;
                                    claimItemDetail.System = response.ProductOrService.Coding[0].System;
                                    claimItemDetail.Display = response.ProductOrService.Coding[0].Display;
                                    claimItemDetail.Error = errorDisplay;
                                    claimItemDetail.Sequence = Sequence;
                                    retvalue.ClaimItemDetail.Add(claimItemDetail);

                                    claimItemDetail = new ClaimItemDetail();
                                    claimItemDetail.Code = response.ProductOrService.Coding[1].Code;
                                    claimItemDetail.ErrorCode = errorCode;
                                    claimItemDetail.System = response.ProductOrService.Coding[1].System;
                                    claimItemDetail.Display = response.ProductOrService.Coding[1].Display;
                                    claimItemDetail.Error = errorDisplay;
                                    claimItemDetail.Sequence = Sequence;
                                    retvalue.ClaimItemDetail.Add(claimItemDetail);
                                }
                            }
                            else
                            {
                                foreach (var cds in response.ProductOrService.Coding)
                                {
                                    Error += errorCode + ", " + errorDisplay + Environment.NewLine;
                                    Error += Environment.NewLine + "Code='" + cds.Code + "', System='" + cds.System + "', Display='" + cds.Display + Environment.NewLine;
                                    Error += Environment.NewLine + "===========================================================================" + Environment.NewLine;

                                    claimItemDetail = new ClaimItemDetail();
                                    claimItemDetail.Code = cds.Code;
                                    claimItemDetail.ErrorCode = errorCode;
                                    claimItemDetail.System = cds.System;
                                    claimItemDetail.Display = cds.Display;
                                    claimItemDetail.Error = errorDisplay;
                                    claimItemDetail.Sequence = Sequence;
                                    retvalue.ClaimItemDetail.Add(claimItemDetail);
                                    break;
                                }

                            }
                        }
                    }
                    else if (!string.IsNullOrEmpty(errorExtension) && errorExtension.Contains("diagnosis"))
                    {
                        var bdc = errorExtension.Split('.');
                        var bundleID = bdc[1].Replace("entry[", "").Replace("]", "");
                        string diagnosisid = "";
                        foreach (var d in bdc)
                        {
                            if (d.StartsWith("diagnosis"))
                            {
                                diagnosisid = d.Replace("diagnosis[", "").Replace("]", "");
                                break;
                            }
                        }
                        Hl7.Fhir.Model.Claim claim = null;
                        if (reqbundle.Entry[1].Resource.TypeName == "Claim")
                        {
                            claim = (Hl7.Fhir.Model.Claim)reqbundle.Entry[1].Resource;
                        }
                        else if (reqbundle.Entry[bundleEntry].Resource.TypeName == "Bundle")
                        {
                            var bundle = (Hl7.Fhir.Model.Bundle)reqbundle.Entry[bundleEntry].Resource;
                            claim = (Hl7.Fhir.Model.Claim)bundle.Entry[1].Resource;
                        }
                        if (claim != null)
                        {
                            var diaglist = claim.Diagnosis[Convert.ToInt32(diagnosisid)];
                            if (diaglist != null)
                            {
                                var diag = (Hl7.Fhir.Model.CodeableConcept)diaglist.Diagnosis;
                                if (diag != null)
                                {
                                    Error += errorCode + ", " + errorDisplay + Environment.NewLine;
                                    Error += Environment.NewLine + "Code='" + diag.Coding[0].Code + "', System='" + diag.Coding[0].System + "', Display='" + diag.Coding[0].Display + Environment.NewLine;
                                    Error += Environment.NewLine + "===========================================================================" + Environment.NewLine;
                                    claimItemDetail = new ClaimItemDetail();
                                    claimItemDetail.Code = diag.Coding[0].Code;
                                    claimItemDetail.ErrorCode = errorCode;
                                    claimItemDetail.System = diag.Coding[0].System;
                                    claimItemDetail.Display = diag.Coding[0].Display;
                                    claimItemDetail.Error = errorDisplay;
                                    retvalue.ClaimItemDetail.Add(claimItemDetail);
                                    //break;
                                }

                            }
                        }
                        else
                        {
                            Error += errorCode + ", " + errorDisplay + Environment.NewLine;
                            Error += Environment.NewLine + "===========================================================================" + Environment.NewLine;
                        }
                    }
                    else
                    {
                        Error += errorCode + ", " + errorDisplay + Environment.NewLine;
                        Error += Environment.NewLine + "===========================================================================" + Environment.NewLine;

                    }
                    ErrorDetails += Error;


                }
                retvalue.Remarks = ErrorDetails;
                return retvalue;
            }
            catch (Exception ex)
            {
                retvalue.Remarks = "Failed Parsing NPHIES Error";
                return retvalue;
            }
        }


        public Models.BatchResponse CreateResponseObjectFromRequestForOperationOutcome(Hl7.Fhir.Model.Bundle requestBundle, string OperationOutcome)
        {
            Models.BatchResponse batchResponse = null;
            try
            {

                var errorMessage = GetOperationOutcomeError(OperationOutcome);

                batchResponse = new Models.BatchResponse();
                var claimDetailResponse = new List<Models.ClaimDetailResponse>();
                int bundleEntry = 0;

                var bundleId = requestBundle.Id;
                foreach (var item in requestBundle.Entry)
                {

                    if (item.Resource.TypeName == "Bundle")
                    {
                        var bundle = (Hl7.Fhir.Model.Bundle)item.Resource;
                    }
                    else if (item.Resource.TypeName == "Claim")
                    {
                        var claimResponse = (Hl7.Fhir.Model.Claim)item.Resource;
                        var identifier = claimResponse.Identifier[0].Value;
                        claimDetailResponse.Add(
                              new Models.ClaimDetailResponse()
                              {
                                  ClaimIdentifier = identifier,
                                  Remarks = errorMessage,
                                  Status = "outcome",
                                  ResponseBundle = null,
                                  BundleID = bundleId
                              });
                    }

                    bundleEntry++;
                }
                batchResponse.ClaimDetailResponse = claimDetailResponse;

                return batchResponse;
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }
        private static string GetOperationOutcomeError(string OperationOutcome)
        {
            var errorBuilder = new StringBuilder();
            try
            {
                var isBundle = IsFhirJson(OperationOutcome);
                if (isBundle)
                {
                    FhirJsonParser fhirJsonParser = new FhirJsonParser();
                    var parsedObject = fhirJsonParser.Parse(OperationOutcome);

                    if (parsedObject != null && parsedObject.TypeName == "OperationOutcome")
                    {
                        var rebundle = (Hl7.Fhir.Model.Base)parsedObject;
                        if (rebundle != null)
                        {
                            var issues = ((Hl7.Fhir.Model.OperationOutcome)rebundle).Issue;
                            if (issues != null && issues.Count > 0)
                            {
                                foreach (var issue in issues)
                                {
                                    //var errorDetails = issue.Details.Coding[0].Code + " :: " + issue.Details.Coding[0].Display;
                                    errorBuilder.Append(issue.Details.Coding[0].Code);
                                    errorBuilder.Append(" :: ");
                                    errorBuilder.Append(issue.Details.Coding[0].Display);
                                }
                            }
                        }
                    }
                    else
                    {
                        errorBuilder.Append("OperationOutcome Error");
                    }
                }
                else
                {
                    errorBuilder.Append("OperationOutcome Error : ");
                    errorBuilder.Append(OperationOutcome);
                }
            }
            catch (Exception ex)
            {
            }
            var errorMessage = errorBuilder.Length > 0 ? errorBuilder.ToString() : "OperationOutcome Error";
            return errorMessage;
        }

        private static bool IsFhirJson(string jsonString)
        {
            try
            {
                FhirJsonParser fhirJsonParser = new FhirJsonParser();
                var parsedObject = fhirJsonParser.Parse(jsonString);

                if (parsedObject != null &&
                    (parsedObject.TypeName == "Resource" || parsedObject.TypeName == "OperationOutcome"))
                {
                    return true;
                }
            }
            catch (Exception)
            {
                // Parsing failed, so it's not valid FHIR JSON
            }

            return false;
        }

        public bool GetPended(Hl7.Fhir.Model.ClaimResponse claimResponse)
        {
            bool pended = false;
            if (claimResponse.Extension is List<Extension> extensions)
            {
                foreach (var ext in extensions)
                {
                    DataType value = ext.Value;
                    if (((Hl7.Fhir.Model.CodeableConcept)value).Coding is List<Coding>)
                    {
                        List<Coding> codeing = ((Hl7.Fhir.Model.CodeableConcept)value).Coding;
                        if (codeing != null && codeing.Count == 1)
                        {
                            if (!string.IsNullOrWhiteSpace(codeing[0].Code))
                            {
                                if (codeing[0].Code.Trim().Equals("pended"))
                                {
                                    pended = true;
                                    break;
                                }
                            }

                        }

                    }
                }
            }
            return pended;
        }
    }

}
