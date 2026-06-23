using Hl7.Fhir.Model;
using Hl7.Fhir.Serialization;
using Nphies.Core.Models;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using System;
using System.Linq;
using System.Text;
using static Hl7.Fhir.Model.Claim;
using Patient = Hl7.Fhir.Model.Patient;

namespace Nphies.Core.Helper
{
    public class Hl7Parser
    {
        public static ClaimRequestModel ParseRequestJsonBundle(string json, string claimIdentifier)
        {
            ClaimRequestModel crmodel = new ClaimRequestModel();
            //crmodel.ClaimDetails = new List<ClaimDetail>();

            var parser = new FhirJsonParser();
            Bundle bundle = parser.Parse<Hl7.Fhir.Model.Bundle>(json);

            Console.WriteLine($"Parsing Bundle ID: {bundle.Id}");
            //Console.WriteLine($"Parsing Bundle Type: {bundle.Type}");
            //Console.WriteLine($"Parsing Timestamp: {bundle.Timestamp}");

            // Iterate through the entries in the Bundle.
            ClaimDetail claimDetail = new ClaimDetail();
            claimDetail.ClaimIdentifier = claimIdentifier;
            claimDetail.MessageBundleID = bundle.Id;

            foreach (var entry in bundle.Entry)
            {

                // Access the full URL of the entry.
                string fullUrl = entry.FullUrl;
                //Console.WriteLine($"Entry Full URL: {fullUrl}");

                // Access the resource within the entry.
                Resource resource = entry.Resource;

                // Check the resource type to determine its type.
                if (resource is MessageHeader messageHeader)
                {
                    // Handle MessageHeader resource.
                    //Console.WriteLine($"MessageHeader ID: {messageHeader.Id}");
                    // Access other MessageHeader properties as needed.
                }
                else if (resource is Claim claim)
                {
                    // Handle ClaimResponse resource.
                    //Console.WriteLine($"ClaimResponse ID: {claim.Id}");
                    var claimId = GetClaimNumber(claim.Id);
                    Console.WriteLine($"Parsing Original ClaimId: {claimId}");
                    claimDetail.ClaimID = claimId;

                    // Access other ClaimResponse properties as needed.
                    claimDetail.ClaimItems = new List<ClaimItem>();

                    if (claim.Item is List<ItemComponent> itemComponent)
                    {
                        foreach (var item in itemComponent)
                        {
                            ClaimItem claimItem = new ClaimItem();
                            claimItem.Sequence = item.Sequence.Value;
                            claimItem.Quantity = item.Quantity.Value.ToString();

                            if (item.ProductOrService.Coding != null)
                            {
                                if (string.IsNullOrWhiteSpace(item.ProductOrService.Coding[0].Code))
                                {
                                    //Console.WriteLine($"{item.ProductOrService.Coding[1].Code} missing mapping");
                                }
                                else if (!string.IsNullOrWhiteSpace(item.ProductOrService.Coding[1].Code))
                                {
                                    //Console.WriteLine($"{item.ProductOrService.Coding[1].Code} {item.ProductOrService.Coding[1].Display}");
                                    claimItem.ProcedureID = item.ProductOrService.Coding[1].Code;
                                    claimItem.ProcedureName = item.ProductOrService.Coding[1].Display;
                                }
                            }

                            claimDetail.ClaimItems.Add(claimItem);
                        }
                    }

                    else if (claim.Diagnosis is List<DiagnosisComponent> diagnosis)
                    {
                        foreach (DiagnosisComponent diag in diagnosis)
                        {
                            DataType diagEntry = diag.Diagnosis;

                            if (diagEntry != null)
                            {
                                if (diagEntry is Hl7.Fhir.Model.CodeableConcept code)
                                {
                                    string diagnosisCode = code.Coding[0].CodeElement.Value;
                                }
                                //foreach (CodeableConcept code in diagEntry)
                                //{


                                //}
                            }

                        }

                    }

                }
                else if (resource is Patient patient)
                {
                    // Handle Patient resource.
                    //Console.WriteLine($"Patient ID: {patient.Id}");
                    // Access other Patient properties as needed.
                }
                else if (resource is Organization organization)
                {
                    // Handle Organization resource.
                    //Console.WriteLine($"Organization ID: {organization.Id}");
                    // Access other Organization properties as needed.
                }
                else if (resource is Coverage coverage)
                {
                    // Handle Coverage resource.
                    //Console.WriteLine($"Coverage ID: {coverage.Id}");
                    // Access other Coverage properties as needed.
                }

                // Add similar checks for other resource types as needed.
            }

            crmodel.ClaimDetails.Add(claimDetail);

            return crmodel;
        }

        public static Bundle ParseBundle(string json)
        {
            var fhirJsonParser = new FhirJsonParser();
            return fhirJsonParser.Parse<Bundle>(json);
        }

        public static PollResponse ParseFhirResponseToClaimPollModel(Bundle ResponseBundle)
        {
            PollResponse pollResponse = null;
            try
            {
                pollResponse = new PollResponse();
                var errorHeaderMessage = string.Empty;
                foreach (var item in ResponseBundle.Entry)
                {
                    Resource resource = item.Resource;
                    if (item.Resource.TypeName == "MessageHeader")
                    {
                        errorHeaderMessage = string.Empty;
                        var responseHeader = (MessageHeader)item.Resource;
                        if (responseHeader.Response != null)
                        {
                            if (responseHeader.Response.Code.HasValue)
                            {
                                var codeValue = responseHeader.Response.Code.Value;
                                if (codeValue is MessageHeader.ResponseType.TransientError)
                                {
                                    errorHeaderMessage = "transient-error";
                                }
                            }
                        }
                        continue;
                    }
                    else if (item.Resource.TypeName == "ClaimResponse")
                    {
                        string ClaimStatus = string.Empty;
                        StringBuilder ClaimReason = new StringBuilder();
                        var claimResponse = (Hl7.Fhir.Model.ClaimResponse)item.Resource;

                        if (claimResponse.Meta is Meta)
                        {
                            var metaTag = (Meta)claimResponse.Meta;
                            List<Coding> tags = new List<Coding>();
                            if (metaTag != null && metaTag.Tag.Count() > 0)
                            {
                                tags.AddRange(metaTag.Tag.Where(t => t.Code.Contains("NPHIES generated")
                                                                    || t.Display.Contains("NPHIES generated"))
                                                                .ToList());
                            }
                            if (tags.Count() > 0)
                            {
                                pollResponse.IsPended = false;
                            }
                            else
                            {
                                pollResponse.IsPended = true;
                            }
                        }

                        if (claimResponse.Outcome is ClaimProcessingCodes)
                        {
                            switch ((ClaimProcessingCodes)claimResponse.Outcome)
                            {

                                case ClaimProcessingCodes.Error:
                                    if (claimResponse.Meta is Meta meta)
                                    {
                                        if (meta.Tag != null && meta.Tag.Count > 0)
                                        {
                                            string nphiesError = "NPHIES generated";
                                            foreach (Coding tag in meta.Tag)
                                            {
                                                if (tag.Code.ToLower().Trim().Equals(nphiesError.ToLower().Trim()))
                                                {
                                                    pollResponse.Status = "error";
                                                    break;
                                                }

                                            }
                                        }
                                        else
                                        {
                                            pollResponse.Status = "perror";
                                            if (!string.IsNullOrEmpty(errorHeaderMessage))
                                            {
                                                ClaimReason.Append(errorHeaderMessage + "  ");
                                                errorHeaderMessage = string.Empty;
                                            }
                                        }

                                    }
                                    break;
                                case ClaimProcessingCodes.Partial:

                                    pollResponse.Status = "partial";

                                    break;

                                case ClaimProcessingCodes.Complete:

                                    pollResponse.Status = "complete";

                                    break;

                                case ClaimProcessingCodes.Queued:

                                    var identifier = claimResponse.Request.Identifier.Value;

                                    bool isPended = GetPended(claimResponse);

                                    pollResponse.IsPended = isPended;

                                    pollResponse.Status = "queued";

                                    pollResponse.ClaimIdentifier = identifier;

                                    break;

                            }
                        }

                        if (claimResponse.Extension.Count > 0)
                        {
                            foreach (var ext in claimResponse.Extension)
                            {
                                if (ext.Url == "http://nphies.sa/fhir/ksa/nphies-fs/StructureDefinition/extension-adjudication-outcome")
                                {
                                    ClaimStatus = ((Hl7.Fhir.Model.CodeableConcept)ext.Value).Coding[0].Code;
                                }
                                else if (ext.Url == "http://nphies.sa/fhir/ksa/nphies-fs/StructureDefinition/extension-adjudication-reissue")
                                {
                                    var codeDisplay = ((CodeableConcept)ext.Value).Coding[0].Display;
                                    if (codeDisplay != null && codeDisplay.Length > 1)
                                    {
                                        ClaimReason.Append(codeDisplay);
                                        ClaimReason.Append(":");
                                    }
                                }
                            }
                        }

                        if (claimResponse.Disposition != null)
                        {
                            ClaimReason.Append(claimResponse.Disposition);
                        }

                        if (claimResponse.Error != null)
                        {
                            foreach (var error in claimResponse.Error)
                            {
                                var errorCode = error.Code.Coding[0].Code;
                                if (!string.IsNullOrEmpty(errorCode))
                                {
                                    ClaimReason.Append(errorCode + " ");
                                }
                                if (error.Code.Coding[0].Display != null)
                                {
                                    var errorDisplay = error.Code.Coding[0].Display;
                                    if (!string.IsNullOrEmpty(errorDisplay))
                                    {
                                        ClaimReason.Append(errorDisplay + " ");
                                    }
                                }
                            }
                        }


                        pollResponse.ResponseBundleID = ResponseBundle.Id;
                        pollResponse.InvoiceNo = claimResponse.Id.Split('-')[0];
                        pollResponse.Outcome = claimResponse.Outcome.Value.ToString();
                        pollResponse.Disposition = claimResponse.Disposition;
                        if (string.IsNullOrEmpty(pollResponse.Status) && !string.IsNullOrEmpty(ClaimStatus))
                        {
                            pollResponse.Status = ClaimStatus;
                        }
                        pollResponse.ClaimReason = ClaimReason.ToString();
                        pollResponse.ClaimIdentifier = claimResponse.Request.Identifier.Value;

                        if (claimResponse.Total != null && claimResponse.Total.Count > 0)
                        {
                            foreach (var adj in claimResponse.Total)
                            {
                                if (adj.Category.Coding[0].Code == "submitted")
                                {
                                    pollResponse.TotalSubmitted = "" + adj.Amount.Value;
                                }
                                if (adj.Category.Coding[0].Code == "benefit")
                                {
                                    pollResponse.TotalApproved = "" + adj.Amount.Value;
                                }
                            }
                        }
                        if (claimResponse.Item != null && claimResponse.Item.Count > 0)
                        {
                            pollResponse.ItemsResponse = new List<ClaimItemResponse>();
                            foreach (var service in claimResponse.Item)
                            {
                                ClaimItemResponse itemResponse = new ClaimItemResponse();
                                itemResponse.ItemSequence = Convert.ToInt32(service.ItemSequence);

                                if (service.Extension != null && service.Extension.Count > 0)
                                {
                                    foreach (var ext in service.Extension)
                                    {
                                        if (ext.Url == "http://nphies.sa/fhir/ksa/nphies-fs/StructureDefinition/extension-adjudication-outcome")
                                        {
                                            itemResponse.Status = ((Hl7.Fhir.Model.CodeableConcept)ext.Value).Coding[0].Code;
                                        }
                                        else if (ext.Url == "http://nphies.sa/fhir/ksa/nphies-fs/StructureDefinition/extension-patientInvoice")
                                        {
                                            itemResponse.InvoiceNo = ((Hl7.Fhir.Model.Identifier)ext.Value).Value;
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
                                        if (adj.Category.Coding[0].Code == "submitted")
                                        {
                                            itemResponse.SubmittedAmount = "" + adj.Amount.Value;
                                        }
                                        if (adj.Reason != null && adj.Reason.Coding != null && adj.Reason.Coding.Count > 0)
                                        {
                                            itemResponse.ItemReason = adj.Reason.Coding[0].Display;
                                            itemResponse.ReasonCode = adj.Reason.Coding[0].Code;
                                        }
                                        if (adj.Category.Coding[0].Code == "approved-quantity")
                                        {
                                            itemResponse.ApprovedQuantity = adj.Value != null ? Convert.ToString(adj.Value) : "0";
                                        }
                                        if (adj.Category.Coding[0].Code == "tax")
                                        {
                                            itemResponse.TaxApproved = (adj.Amount != null ? "" + adj.Amount.Value : "0");
                                        }
                                    }
                                }
                                pollResponse.ItemsResponse.Add(itemResponse);
                            }
                        }
                        if (string.IsNullOrWhiteSpace(pollResponse.ClaimReason))
                        {
                            List<string> listOfError = new List<string>();
                            if (claimResponse.Error != null && claimResponse.Error.Count > 0)
                            {

                                foreach (var errorCode in claimResponse.Error)
                                {
                                    listOfError.Add(errorCode.Code.Coding[0].Code);
                                    listOfError.Add(errorCode.Code.Coding[0].Display);
                                }

                                string errorText = GetErrorText(listOfError);
                                pollResponse.ClaimReason = errorText;
                            }
                        }

                    }
                }

                return pollResponse;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public static bool GetPended(Hl7.Fhir.Model.ClaimResponse claimResponse)

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

        private static string GetErrorText(List<string> listOfError)
        {
            var distinctErrors = listOfError.Distinct();
            StringBuilder errorResponse = new StringBuilder();
            foreach (var error in distinctErrors)
            {
                errorResponse.Append(error);
                errorResponse.Append(" : ");
            }
            string errorText = errorResponse.ToString();
            int lastIndex = errorText.LastIndexOf(':');

            if (lastIndex >= 0)
            {
                errorText = errorText.Remove(lastIndex, 1).Trim();
            }

            if (errorText.Length > 0)
                return errorText;
            else
                return string.Empty;
        }

        private static string GetClaimNumber(string input)
        {
            string pattern = @"\d+";
            Match match = Regex.Match(input, pattern);

            if (match.Success)
            {
                string number = match.Value;
                return number;
            }
            else
            {
                return input;
            }
        }
    }

}
