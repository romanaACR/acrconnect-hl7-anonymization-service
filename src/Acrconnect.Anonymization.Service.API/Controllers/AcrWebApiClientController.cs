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
    public class AcrWebApiClientController : ControllerBase
    {
        private readonly IAcrProfileDetailsService _service;
        private readonly ILogger<AcrWebApiClientController> _logger;

        public AcrWebApiClientController(IAcrProfileDetailsService service, ILogger<AcrWebApiClientController> logger)
        {
            _service = service;
            _logger = logger;
        }

        [HttpGet("acrwebapis")]
        [Produces("application/json")]
        public async Task<ActionResult> GetAcrApps()
        {
            try
            {
                var result = await _service.GetAcrProfileDetails();
                return Ok(result);
            }
            catch (Exception ex)
            {
                var tmp = "Exception Message : " + ex.Message + ", Inner exception : " + ex.InnerException?.Message
                   + ", stack trace:" + ex?.StackTrace + ex?.Data;

                _logger.LogError(tmp);
                return BadRequest("Exception Message : " + ex.Message + ", Inner exception : " + ex.InnerException?.Message
                  + ", stack trace:" + ex?.StackTrace + ex?.Data);
            }
        }


        [HttpGet("acrprofiledetails")]
        [Produces("application/json")]
        public async Task<ActionResult> GetAcrProfileDetails()
        {       
            try
            {
               var result = await _service.GetAcrProfileDetails();
                return Ok(result);
            }
            catch (Exception ex)
            {
                var tmp = "Exception Message : " + ex.Message + ", Inner exception : " + ex.InnerException?.Message
                   + ", stack trace:" + ex?.StackTrace + ex?.Data;

                _logger.LogError(tmp);
                return BadRequest("Exception Message : "  + ex.Message + ", Inner exception : " + ex.InnerException?.Message
                  + ", stack trace:" + ex?.StackTrace + ex?.Data);
            }
        }

       

        [HttpGet("IsAlive")]
        [Produces("application/json")]
        public ActionResult AmAlive()
        {
        //    var fileName = "HL7AnonymizationRules.xml";
        //    var filePath = Path.Combine("Rules", fileName);


        //    return new FileContentResult(System.IO.File.ReadAllBytes(filePath), "text/xml") { FileDownloadName = fileName };

             return Ok("I AM ANONYMIZATION. ALIVE OOLLAAA");
        }
    }

    public class ViewModel
    {

        public AcrProfileDetail AcrProfileDetail { get; set; }
        public byte[] Profile { get; set; }
    }
}