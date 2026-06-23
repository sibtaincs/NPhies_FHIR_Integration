using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Nphies.Core.Services.Communication
{
    public interface IMongoDb<T>
    {
        IMongoDatabase mongoDatabase { get; set; }
        IMongoCollection<T> mongoCollection { get; set; }
    }
}
