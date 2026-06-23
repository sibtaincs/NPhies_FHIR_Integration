using System;
using System.Collections.Generic;

namespace Nphies.Core.Data.Entities
{
    public partial class RcmMappingType
    {
        public RcmMappingType()
        {
            RcmTransactionMappings = new HashSet<RcmTransactionMapping>();
        }

        public int TransactionTypeId { get; set; }
        public string TransactionTypeName { get; set; }

        public virtual ICollection<RcmTransactionMapping> RcmTransactionMappings { get; set; }
    }
}
