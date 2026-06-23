using Microsoft.EntityFrameworkCore.ChangeTracking;
using Newtonsoft.Json;
using Nphies.Core.Data.Entities;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Nphies.Core.Data.Common
{
    public class AuditEntry
    {
        public AuditEntry(EntityEntry entry)
        {
            Entry = entry;
        }

        public EntityEntry Entry { get; }
        public string TableName { get; set; }
        public Dictionary<string, object> KeyValues { get; } = new Dictionary<string, object>();
        public Dictionary<string, object> OldValues { get; } = new Dictionary<string, object>();
        public Dictionary<string, object> NewValues { get; } = new Dictionary<string, object>();
        public List<PropertyEntry> TemporaryProperties { get; } = new List<PropertyEntry>();
        public long ClaimId { get; set; }
        public int OrganizationId { get; set; }
        public long ServiceId { get; set; }
        public long RowId { get; set; }
        public int NphiesSeqNo { get; set; }
        public string ReasonCode { get; set; }
        public string ItemReason { get; set; }
        public int CreatedBy { get; set; }
        public bool HasTemporaryProperties => TemporaryProperties.Any();

        public AuditLog ToAudit()
        {
            var audit = new AuditLog();
            audit.TableName = TableName;
            audit.DateTime = DateTime.UtcNow;
            audit.ClaimId = ClaimId;
            audit.OrganizationId = OrganizationId;
            audit.ServiceId = ServiceId;
            audit.RowId = RowId;
            audit.NphiesSeqNo = NphiesSeqNo;
            audit.ReasonCode = ReasonCode;
            audit.ItemReason = ItemReason;
            audit.KeyValues = JsonConvert.SerializeObject(KeyValues);
            audit.OldValues = OldValues.Count == 0 ? null : JsonConvert.SerializeObject(OldValues);
            audit.NewValues = NewValues.Count == 0 ? null : JsonConvert.SerializeObject(NewValues);
            audit.Createdby = CreatedBy;
            return audit;
        }
    }

}
