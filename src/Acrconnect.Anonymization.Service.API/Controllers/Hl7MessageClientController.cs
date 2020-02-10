using AcrConnect.Anonymization.Service.Models;
using AcrConnect.Anonymization.Service.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Threading.Tasks;

namespace AcrConnect.Anonymization.Service.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class Hl7MessageClientController : ControllerBase
    {
        private readonly IHL7MessageHandlerService _service;
        private readonly ILogger<AcrWebApiClientController> _logger;

        public Hl7MessageClientController( IHL7MessageHandlerService service, ILogger<AcrWebApiClientController> logger)
        {
            _service = service;
            _logger = logger;
        }

        [HttpPost("apiid/{id}")]
        [Produces("application/json")]
        public async Task<ActionResult> Post(Int32 id,HL7MessageData hl7MessageData)
        {
            try
            {
                var result = await _service.DoAnonymizatione(hl7MessageData, id);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest("Exception : " + ex.Message);
            }
        }


        [HttpGet("IsAlive")]
        [Produces("application/json")]
        public ActionResult AmAlive()
        {
         //   return new FileContentResult(_service.GetSampleFile(), "text/xml") { FileDownloadName  = "HL7AnonymizationRules.xml" };

          return Ok("I AM ANONYMIZATION. ALIVE OOLLAAA.");
        }
    }

}