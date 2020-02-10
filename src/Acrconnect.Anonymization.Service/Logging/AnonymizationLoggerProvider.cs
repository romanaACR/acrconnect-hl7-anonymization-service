using AcrConnect.Anonymization.Service;
using AcrConnect.Anonymization.Service.Logging;
using AcrConnect.Anonymization.Service.Services;
using Microsoft.Extensions.Logging;
using System;

namespace AcrConnect.Anonymization.Service.Logging
{
    class AnonymizationLoggerProvider : ILoggerProvider
    {
        private AnonymizationServiceDBContext _db;

        public AnonymizationLoggerProvider(AnonymizationServiceDBContext db)
        {
            _db = db;
        }

        public ILogger CreateLogger(string categoryName)
        {
            var logger = new AnonymizationLogger(categoryName, new Type[] {
                typeof(IAcrProfileDetailsService),
                typeof(IHL7MessageHandlerService)
            }, _db);
            return logger;
        }

        public void Dispose()
        {
        }
    }
}
