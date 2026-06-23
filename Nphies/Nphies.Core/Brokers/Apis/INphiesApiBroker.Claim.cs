using System;
using System.Threading.Tasks;

namespace Nphies.Core.Brokers.Apis
{
    public partial interface INphiesApiBroker
    {
        ValueTask<string> GetStudentByIdAsync(Guid studentId);
    }
}
