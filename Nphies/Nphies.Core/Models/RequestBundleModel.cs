using System.Collections.Generic;
using FM = Hl7.Fhir.Model;

namespace Nphies.Core.Models
{
    public class FriendlyViewRequestModel
    {
        public FHIRRequestModel fhirRequestModel { get; set; }
        public FHIRResponseModel fhirResponseModel { get; set; }
    }
    public class FHIRResponseModel
    {
        public FHIRResponseModel()
        {
            patient = new FHIRPatientDto();
            practioner = new FHIRPractioner();
            payer = new FHIRPayer();
            provider = new FHIRProvider();
            coverageDetail = new FHIRcoverageDetail();
            claim = new FHIRClaim();
            diagnosis = new List<FHIRDiagnosis>();
            services = new List<FHIRServices>();
        }
        public FHIRPatientDto patient { get; set; }
        public FHIRPractioner practioner { get; set; }
        public FHIRPayer payer { get; set; }
        public FHIRProvider provider { get; set; }
        public FHIRcoverageDetail coverageDetail { get; set; }
        public FHIRClaim claim { get; set; }
        public List<FHIRDiagnosis> diagnosis { get; set; }
        public List<FHIRServices> services { get; set; }
        public List<FHIRBreakdown> breakdown { get; set; }
        public List<FHIRNotes> note { get; set; }
        public List<FHIRError> error { get; set; }
    }

    public class FHIRNotes
    {
        public string no { get; set; }
        public string note { get; set; }
    }
    public class FHIRError
    {
        public string code { get; set; }
        public string display { get; set; }
        public string details { get; set; }
    }

    public class FHIRRequestModel
    {
        public FHIRRequestModel()
        {
            patient = new FHIRPatientDto();
            practioner = new FHIRPractioner();
            payer = new FHIRPayer();
            provider = new FHIRProvider();
            coverageDetail = new FHIRcoverageDetail();
            claim = new FHIRClaim();
            services = new List<FHIRServices>();
            communication = new Communication();
        }
        public FHIRPatientDto patient { get; set; }
        public FHIRPractioner practioner { get; set; }
        public FHIRPayer payer { get; set; }
        public FHIRProvider provider { get; set; }
        public FHIRcoverageDetail coverageDetail { get; set; }
        public FHIRClaim claim { get; set; }
        public List<FHIRDiagnosis> diagnosis { get; set; }
        public List<FHIRServices> services { get; set; }
        public Communication communication { get; set; }
    }

    public class FHIRBreakdown
    {
        public string itemSequence { get; set; }
        public string code { get; set; }
        public string submitted { get; set; }
        public string benefit { get; set; }
        public string discount { get; set; }
        public string deductible { get; set; }
        public string copay { get; set; }
        public string unallocdeduct { get; set; }
        public string eligpercent { get; set; }
        public string tax { get; set; }
        public string approvedquantity { get; set; }
        public string reason { get; set; }


    }

    public class FHIRClaim
    {
        public string bundleId { get; set; }
        public string identifier { get; set; }
        public string claimId { get; set; }
        public string insurer { get; set; }
        public string insurance { get; set; }
        public string status { get; set; }
        public string patientName { get; set; }
        public string provider { get; set; }
        public string total { get; set; }
        public string chiefComplaint { get; set; }
        public string systolic { get; set; }
        public string diastolic { get; set; }
        public string weight { get; set; }
        public string height { get; set; }
        public string use { get; set; }
        public string requestor { get; set; }
        public string request { get; set; }
        public string type { get; set; }
        public string outcome { get; set; }
        public string preAuthRef { get; set; }
        public string subType { get; set; }
        public string preAuthPeriodStart { get; set; }
        public string preAuthPeriodEnd { get; set; }
        public string adjOutcome { get; set; }
        public string totalBenefit { get; set; }
        public string totalSubmitted { get; set; }
        public string adjBenefit { get; set; }
        public string adjEligible { get; set; }


    }
    public class FHIRcoverageDetail
    {
        public string identifier { get; set; }
        public string status { get; set; }
        public string type { get; set; }
        public string beneficiary { get; set; }
        public string relationship { get; set; }
        public string payerName { get; set; }
        public string payerClass { get; set; }
    }
    public class FHIRProvider
    {
        public string licenseNo { get; set; }
        public string providerName { get; set; }
    }
    public class FHIRPayer
    {
        public string licenseNo { get; set; }
        public string payerName { get; set; }
    }
    public class FHIRDiagnosis
    {
        public string Code { get; set; }
        public string Type { get; set; }
    }
    public class FHIRServices
    {
        public string sequence { get; set; }
        public string hmgCoding { get; set; }
        public string nphiesCoding { get; set; }
        public string nphiesDisplay { get; set; }
        public string serviced { get; set; }
        public string quantity { get; set; }
        public string unitPrice { get; set; }
        public string net { get; set; }
    }
    public class FHIRPractioner
    {
        public string practionerIdentifier { get; set; }
        public string practionerId { get; set; }
        public string practionerName { get; set; }
        public string gender { get; set; }
    }
    public class FHIRPatientDto
    {
        public string patientMRN { get; set; }
        public string patientName { get; set; }
        public string gender { get; set; }
        public string dateOfBirth { get; set; }
    }

    public class RequestBundleModel
    {
        public int nodeCount { get; set; } = 4;
        public FM.Claim claim { get; set; }
        public FM.Patient patient { get; set; }
        public FM.Coverage coverage { get; set; }
        public FM.Practitioner practitioner { get; set; }
        public FM.Organization payer { get; set; }
        public FM.Organization provider { get; set; }
        public FM.Communication communication { get; set; }
        public FM.Task task { get; set; }
        public string bundleId { get; set; }
    }
    public class ResponseBundleModel
    {
        public int nodeCount { get; set; } = 4;
        public FM.ClaimResponse claim { get; set; }
        public FM.Patient patient { get; set; }
        public FM.Coverage Coverage { get; set; }
        public string bundleId { get; set; }
        public FM.Organization payer { get; set; }
        public FM.Organization provider { get; set; }
        public FM.CommunicationRequest comRequest { get; set; }
        public FM.Task task { get; set; }
    }
    public class Communication
    {
        public string identifier { get; set; }
        public string status { get; set; }
        public string category { get; set; }
        public string priority { get; set; }
        public string subject { get; set; }
        public string about { get; set; }
        public string recepient { get; set; }
        public string sender { get; set; }
        public string reasonCode { get; set; }
        public string payload { get; set; }
    }
}
