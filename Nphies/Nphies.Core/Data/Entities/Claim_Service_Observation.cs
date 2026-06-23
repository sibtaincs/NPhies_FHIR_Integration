using Nphies.Core.DTOs;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace Nphies.Core.Data.Entities
{
    public class Claim_Service_Observation
    {
        public long ClaimID { get; set; }
        public int OrganizationID { get; set; }
        public long ServiceID { get; set; }
        public short ServiceLineitemNo { get; set; }
        public short LineItemNo { get; set; }
        public Nullable<short> Type { get; set; }
        public string Code { get; set; }
        public string Value { get; set; }
        public string Valuetype { get; set; }
        public Nullable<byte> Status { get; set; }
        public Nullable<bool> IsActive { get; set; }
        public int CreatedBy { get; set; }
        public System.DateTime CreatedOn { get; set; }
        public Nullable<int> ModifiedBy { get; set; }
        public Nullable<System.DateTime> ModifiedOn { get; set; }

        public virtual RcmClaimServicesDetail RCM_ClaimServicesDetail { get; set; }
    }
}
