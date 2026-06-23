using Nphies.Core.Documents;
using System.Threading.Tasks;

namespace Nphies.Core.Services.PDFAttachment
{
    public interface IPDFAttachmentService
    {
        Task<Document> GetPDFAttachment(long claimId);

    }
}
