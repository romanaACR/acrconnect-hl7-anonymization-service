using AcrConnect.Anonymization.Service;
using AcrConnect.Anonymization.Service.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using System;

namespace Microsoft.Extensions.DependencyInjection
{
    public static class AnonymizationServiceCollection
    {
        public static void AddTriadService(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddDbContext<AnonymizationServiceDBContext>((options) =>
            {
                var connectionString = GetConnectionString(configuration);
                options.UseNpgsql(connectionString);
            });

            services.AddScoped<IAcrProfileDetailsService, AcrProfileDetailsService>();
            services.AddScoped<IHL7MessageHandlerService, HL7MessageHandlerService>();
        }

        private static string GetConnectionString(IConfiguration configuration)
        {
            var connectionString = Environment.GetEnvironmentVariable("SQLCONNSTR_DEFAULT");
            if (string.IsNullOrEmpty(connectionString))
            {
                connectionString = configuration.GetConnectionString("Default");
                if (string.IsNullOrEmpty(connectionString))
                {
                    throw new InvalidOperationException("Connection string not mentioned.");
                }
            }

            Console.WriteLine("Connection String: {0}", connectionString);
            return connectionString;
        }
    }
}
