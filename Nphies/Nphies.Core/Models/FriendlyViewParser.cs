using Hl7.Fhir.Serialization;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using FM = Hl7.Fhir.Model;

namespace Nphies.Core.Models
{
    public static class Parser
    {
        public static FM.Bundle ParseBundle(string bundleJson)
        {
            try
            {
                ParserSettings settings = new ParserSettings();
                settings.PermissiveParsing = true;
                FhirJsonParser parser = new FhirJsonParser(settings);
                Task<FM.Bundle> t = Task.Run(async () => await parser.ParseAsync<FM.Bundle>(bundleJson));
                FM.Bundle bundle = t.Result;
                return bundle;
            }
            catch (Exception ex)
            {

                throw;
            }
        }
        public static RequestBundleModel GetNPHIESRequestModel(FM.Bundle bundle)
        {
            try
            {
                List<FM.Bundle.EntryComponent> nphiesRequestNodes = new List<FM.Bundle.EntryComponent>();
                string[] requiredNodes = { "Claim", "Patient", "Coverage", "Practitioner", "Organization", "Communication" };
                RequestBundleModel requestModel = new RequestBundleModel();
                requestModel.bundleId = bundle.Id;
                foreach (var entry in bundle.Entry)
                {
                    switch (entry.Resource.TypeName)
                    {
                        case "Claim":
                            requestModel.claim = (FM.Claim)entry.Resource;
                            break;
                        case "Patient":
                            requestModel.patient = (FM.Patient)entry.Resource;
                            break;
                        case "Coverage":

                            requestModel.coverage = (FM.Coverage)entry.Resource;
                            break;
                        case "Practitioner":
                            requestModel.practitioner = (FM.Practitioner)entry.Resource;
                            break;
                        case "Organization":
                            FM.Organization org = (FM.Organization)entry.Resource;
                            if (org.Identifier[0].System.ToLower().Contains("payer"))
                            {
                                requestModel.payer = org;
                            }
                            if (org.Identifier[0].System.ToLower().Contains("provider"))
                            {
                                requestModel.provider = org;
                            }
                            break;
                        case "Communication":
                            requestModel.communication = (FM.Communication)entry.Resource;
                            break;
                        case "Task":
                            requestModel.task = (FM.Task)entry.Resource;
                            break;
                        default:
                            break;
                    }
                }
                return requestModel;
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        public static ResponseBundleModel GetNPHIESResponseModel(FM.Bundle bundle)
        {
            try
            {
                List<FM.Bundle.EntryComponent> nphiesResponseNodes = new List<FM.Bundle.EntryComponent>();
                string[] requiredNodes = { "ClaimResponse", "Patient", "Coverage", "Organization", "CommunicationRequest", "Task" };
                ResponseBundleModel responseModel = new ResponseBundleModel();
                responseModel.bundleId = bundle.Id;
                foreach (var entry in bundle.Entry)
                {
                    switch (entry.Resource.TypeName)
                    {
                        case "ClaimResponse":
                            responseModel.claim = (FM.ClaimResponse)entry.Resource;
                            break;
                        case "Patient":
                            responseModel.patient = (FM.Patient)entry.Resource;
                            break;
                        case "Coverage":
                            responseModel.Coverage = (FM.Coverage)entry.Resource;
                            break;
                        case "Organization":
                            FM.Organization org = (FM.Organization)entry.Resource;
                            if (org.Identifier[0].System.ToLower().Contains("payer"))
                            {
                                responseModel.payer = org;
                            }
                            if (org.Identifier[0].System.ToLower().Contains("provider"))
                            {
                                responseModel.provider = org;
                            }
                            break;
                        case "CommunicationRequest":
                            responseModel.comRequest = (FM.CommunicationRequest)entry.Resource;
                            break;
                        case "Task":
                            responseModel.task = (FM.Task)entry.Resource;
                            break;
                        default:
                            break;
                    }
                }
                return responseModel;
            }
            catch (Exception ex)
            {

                return null;
            }

        }

    }
}
