using System;
using System.Collections.Generic;

namespace Nphies.Core.Data.Entities
{
    public partial class RcmTemplateMapping
    {
        public int TemplateId { get; set; }
        public int OrganizationId { get; set; }
        public int FacilityId { get; set; }
        public int TemplateType { get; set; }
        public string EntityName { get; set; }
        public bool? IsEditable { get; set; }
        public bool? IsMandatory { get; set; }
        public int? MaxLength { get; set; }
        public int CreatedBy { get; set; }
        public DateTime CreatedOn { get; set; }
        public int? ModifiedBy { get; set; }
        public DateTime? ModifiedOn { get; set; }
        public bool? IsActive { get; set; }
        public string ValidationMessage { get; set; }
        public string Controll { get; set; }
    }
}
