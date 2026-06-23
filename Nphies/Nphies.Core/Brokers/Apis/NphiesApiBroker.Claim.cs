using System;
using System.Threading.Tasks;

namespace Nphies.Core.Brokers.Apis
{
    public partial class NphiesApiBroker
    {
        const string StudentsRelativeUrl = "api/students";

        public async ValueTask<string> GetStudentByIdAsync(Guid studentId) =>
            await Task.FromResult<string>("");
    }
}
