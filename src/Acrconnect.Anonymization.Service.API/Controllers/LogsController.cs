using AcrConnect.Anonymization.Service.Extensions;
using AcrConnect.Anonymization.Service.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;

namespace AcrConnect.Anonymization.Service.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class LogsController : ControllerBase
    {
        private AnonymizationServiceDBContext _db;

        public LogsController(AnonymizationServiceDBContext db)
        {
            _db = db;
        }


        /// <summary>
        /// Get logs
        /// </summary>
        /// <remarks>
        /// Gets the logs.
        /// </remarks>
        /// <response code="200">Success</response>  
        [HttpGet]
        [Produces("application/json")]
        [ProducesResponseType(typeof(IEnumerable<AnonymizationLog>), 200)]
        public IActionResult Get()
        {
            var logs = _db.Logs.AsNoTracking().Select(x => new
            {
                x.Id,
                Time = x.Time.CreateDate().ToString("yyyy-MM-dd hh:mm tt"),
                x.Message
            }).ToArray().OrderByDescending(x => x.Time).Take(20);
            return Ok(logs);
        }

        /// <summary>
        /// Get logs within seconds
        /// </summary>
        /// <remarks>
        /// Get list of all logs with in a timespan of give senconds.
        /// </remarks>
        /// <param name="seconds">Specify number of seconds, to retrieve all recent logs logged in the timespan.</param>
        /// <returns></returns>
        /// <response code="200">Success</response>
        [HttpGet("in/{seconds}")]
        [Produces("application/json")]
        [ProducesResponseType(typeof(IEnumerable<AnonymizationLog>), 200)]
        public IActionResult GetWithin(int seconds)
        {
            var time = DateTime.UtcNow;
            var timeStamp = time.ToUnixTimestamp();
            timeStamp -= seconds;
            var logs = _db.Logs.Where(x => x.Time > timeStamp)
                .AsNoTracking().Select(x => new
                {
                    x.Id,
                    Time = x.Time.CreateDate(),
                    x.Message
                }).ToArray();
            return Ok(logs);
        }
    }
}