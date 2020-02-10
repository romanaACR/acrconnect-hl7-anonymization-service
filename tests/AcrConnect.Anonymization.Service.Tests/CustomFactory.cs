using AcrConnect.Anonymization.Service;
using AcrConnect.Submission.Service;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace AcrConnect.Submission.Service.Tests
{
    public class CustomFactory<TStartup> : WebApplicationFactory<TStartup>
          where TStartup : class
    {
        protected override void ConfigureWebHost(IWebHostBuilder builder)
        {
            builder.ConfigureServices(services =>
            {
                var serviceCollection = new ServiceCollection();
                serviceCollection.AddEntityFrameworkInMemoryDatabase();
                var serviceProvider = serviceCollection.BuildServiceProvider();

                services.AddDbContext<AnonymizationServiceDBContext>(options =>
                {
                    options.UseInMemoryDatabase("SubmissionMemoryDB");
                    options.UseInternalServiceProvider(serviceProvider);
                });

                using (var scope = services.BuildServiceProvider().CreateScope())
                {
                    var scopedServiceProvider = scope.ServiceProvider;
                    var database = scopedServiceProvider.GetRequiredService<AnonymizationServiceDBContext>();
                    database.Database.EnsureCreated();
                }
            });
        }
    }
}
