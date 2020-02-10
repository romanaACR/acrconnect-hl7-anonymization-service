
using AcrConnect.Anonymization.Service.Extensions;
using AcrConnect.Anonymization.Service.Models;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using System;
using System.Linq;
namespace AcrConnect.Anonymization.Service.Logging
{
    class AnonymizationLogger : ILogger
    {
        private string _categoryName;
        private string[] _filterCategories = null;
        private AnonymizationServiceDBContext _db;

        class VirtualScope : IDisposable
        {
            public void Dispose()
            {
            }
        }

        public AnonymizationLogger(string categoryName, Type[] filters, AnonymizationServiceDBContext db)
        {
            _categoryName = categoryName;
            _filterCategories = filters.Select(x => x.FullName).ToArray();
            _db = db;
        }

        public IConfiguration Config { get; private set; }
        public LogLevel MinimumLogLevel { get; private set; }

        public IDisposable BeginScope<TState>(TState state)
        {
            return new VirtualScope();
        }

        public bool IsEnabled(LogLevel logLevel)
        {
            if (logLevel != LogLevel.Information)
            {
                return false;
            }
            return true;
        }

        public void Log<TState>(LogLevel logLevel, EventId eventId, TState state,
            Exception exception, Func<TState, Exception, string> formatter)
        {
            if (!_filterCategories.Contains(this._categoryName))
            {
                return;
            }

            var timeStamp = DateTime.UtcNow
                .ToUnixTimestamp();
            var message = state.ToString();
            var log = new AnonymizationLog()
            {
                Time = timeStamp,
                Message = message
            };
            _db.Logs.Add(log);
            _db.SaveChanges();
        }

       
    }
}
