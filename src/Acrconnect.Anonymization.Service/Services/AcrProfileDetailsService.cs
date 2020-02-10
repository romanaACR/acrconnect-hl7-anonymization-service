using AcrConnect.Anonymization.Service.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;

namespace AcrConnect.Anonymization.Service.Services
{
    public class AcrProfileDetailsService : IAcrProfileDetailsService
    {
        private AnonymizationServiceDBContext _context;
        private ILogger<AnonymizationLog> _logger;

        public AcrProfileDetailsService(AnonymizationServiceDBContext context, ILogger<AnonymizationLog> logger)
        {
            _context = context;
            _logger = logger;
        }
              

        public async Task<IList<AcrProfileDetail>> GetAcrProfileDetails()
        {
            try
            {
                return await _context.AcrProfileDetails.ToListAsync();
            }
            catch (Exception ex)
            {
                _logger.LogInformation("Failed to retrieve AcrProfileDetail. Exception:" + ex.Message);
                throw new InvalidOperationException("Failed to retrieve AcrProfileDetail.");
            }
        }

        public async Task<bool> SaveAcrProfileDetails(AcrProfileDetail record)
        {
            try
            {
                _context.AcrProfileDetails.Add(record);
                 await _context.SaveChangesAsync();
                return true;
               
            }
            catch (Exception ex)
            {
                _logger.LogInformation("Failed to retrieve AcrProfileDetail. Exception:" + ex.Message);
                throw new InvalidOperationException("Failed to retrieve AcrProfileDetail.");
            }
        }
    }
}
