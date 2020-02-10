using AcrConnect.Anonymization.Service.Logging;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace AcrConnect.Anonymization.Service.Extensions
{
    public static class AnonymizationLoggingExtension
    {
        public static void AddAnonymizationLogger(this ILoggingBuilder builder)
        {
            var services = builder.Services.BuildServiceProvider();
            var db = services.GetService<AnonymizationServiceDBContext>();
            var logger = new AnonymizationLoggerProvider(db);
            builder.AddProvider(logger);
        }
    }
}
