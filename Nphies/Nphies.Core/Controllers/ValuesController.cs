using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Nphies.Core.Brokers.Loggings;

namespace Nphies.Core.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ValuesController : ControllerBase
    {
        private readonly ILoggingBroker logger;
        public ValuesController(ILoggingBroker logger)
        {
            this.logger = logger;
        }
        [HttpGet]
        public ActionResult<string> Get()
        {
            logger.LogInfo("ValuesController");
           return Ok("I'm alive to serve your request!");
        }
    }
}
