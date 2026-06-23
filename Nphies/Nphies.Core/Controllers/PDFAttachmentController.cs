using Microsoft.AspNetCore.Mvc;
using Nphies.Core.Services.PDFAttachment;
using System.Threading.Tasks;
using System;
using System.Reflection.Metadata.Ecma335;
using Nphies.Core.Models;
using Hl7.Fhir.Model;
using Nphies.Core.Data.Entities;
using System.Configuration;
using Nphies.Core.Documents;

namespace Nphies.Core.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PDFAttachmentController : Controller
    {
        private readonly IPDFAttachmentService _attachmentService;
        public PDFAttachmentController(IPDFAttachmentService attachmentService)
        {
            _attachmentService = attachmentService;
        }

        [HttpGet("PDFAttachment/{claimId}")]
        public async Task<DocumentResponse> GetPDFAttachment(long claimId)
        {
            try
            {
                Document documentProcessor = await _attachmentService.GetPDFAttachment(claimId);
                var documentResponse = await documentProcessor.GetDocument(claimId);
                return documentResponse;
                
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message.ToString());

            }
        }
    }
}
