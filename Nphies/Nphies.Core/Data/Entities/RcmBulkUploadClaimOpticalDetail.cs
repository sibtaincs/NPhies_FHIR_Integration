using System;
using System.Collections.Generic;

namespace Nphies.Core.Data.Entities
{
    public partial class RcmBulkUploadClaimOpticalDetail
    {
        public long BulkUploadClaimOpticalDetailId { get; set; }
        public long BulkUploadProcessLogId { get; set; }
        public int OrganizationId { get; set; }
        public int UserId { get; set; }
        public int EncounterNo { get; set; }
        public string InvoiceNo { get; set; }
        public string RegularLensesType { get; set; }
        public string LensSpecifications { get; set; }
        public string ContactLenses { get; set; }
        public string FramesLensIndicator { get; set; }
        public string NumberOfPairs { get; set; }
        public int CreatedBy { get; set; }
        public DateTime? CreatedOn { get; set; }
        public int ModifiedBy { get; set; }
        public DateTime? ModifiedOn { get; set; }
        public bool? IsDeleted { get; set; }
        public int? ErrorId { get; set; }
        public string ErrorDescription { get; set; }
        public bool? IsValidated { get; set; }
        public bool? IsProcessed { get; set; }
        public bool? IsAnyError { get; set; }
        public long? MidTableReferenceId { get; set; }
    }
}
