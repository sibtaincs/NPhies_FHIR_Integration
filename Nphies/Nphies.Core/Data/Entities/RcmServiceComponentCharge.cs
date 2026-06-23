using System;
using System.Collections.Generic;

namespace Nphies.Core.Data.Entities
{
    public partial class RcmServiceComponentCharge
    {
        public int ServiceChargeId { get; set; }
        public int DimensionId { get; set; }
        public int ServiceComponentId { get; set; }
        public short LineitemNo { get; set; }
        public long ServiceId { get; set; }
        public int OrganizationId { get; set; }
        public string DimReference { get; set; }
        public decimal? Price { get; set; }
        public bool IsActive { get; set; }
        public int CreatedBy { get; set; }
        public DateTime CreatedOn { get; set; }
        public int? ModifiedBy { get; set; }
        public DateTime? ModifiedOn { get; set; }

        public virtual RcmServiceComponent RcmServiceComponent { get; set; }
    }
}
