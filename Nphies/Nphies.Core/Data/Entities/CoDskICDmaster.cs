using System;

namespace Nphies.Core.Data.Entities
{
    public class CoDskICDmaster
    {
        public int OrganizationId { get; set; }
        public int Code { get; set; }
        public int Description { get; set; }
        public int ShortDesc { get; set; }
        public bool IsActive { get; set; }
        public int CreatedBy { get; set; }
        public DateTime CreatedOn { get; set; }
    }
}
