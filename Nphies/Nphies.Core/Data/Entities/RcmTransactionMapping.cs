using System;
using System.Collections.Generic;

namespace Nphies.Core.Data.Entities
{
    public partial class RcmTransactionMapping
    {
        public int MappingTypeId { get; set; }
        public int TransactionTypeId { get; set; }

        public virtual RcmMappingType MappingType { get; set; }
    }
}
