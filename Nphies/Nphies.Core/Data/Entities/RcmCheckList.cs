using System;
using System.Collections.Generic;

namespace Nphies.Core.Data.Entities
{
    public partial class RcmCheckList
    {
        public long CheckListId { get; set; }
        public int OrganizationId { get; set; }
        public int? FacilityId { get; set; }
        public int PayerId { get; set; }
        public int PayerPolicyId { get; set; }
        public int SubCategoryId { get; set; }
        public byte CheckListType { get; set; }
        public string CheckList { get; set; }
        public string CheckListN { get; set; }
        public bool? IsActive { get; set; }
        public int CreatedBy { get; set; }
        public DateTime CreatedOn { get; set; }
        public int? EditedBy { get; set; }
        public DateTime? EditedOn { get; set; }
    }
}
