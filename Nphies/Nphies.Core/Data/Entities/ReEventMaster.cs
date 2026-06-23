using System;
using System.Collections.Generic;

namespace Nphies.Core.Data.Entities
{
    public partial class ReEventMaster
    {
        public int EventId { get; set; }
        public string EventName { get; set; }
        public bool? IsActive { get; set; }
    }
}
