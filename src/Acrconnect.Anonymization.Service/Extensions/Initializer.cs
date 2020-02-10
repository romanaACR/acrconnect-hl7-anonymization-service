using AcrConnect.Anonymization.Service;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace Microsoft.AspNetCore.Builder
{
    public static class Initializer
    {
        public static void Initialize(this IApplicationBuilder app)
        {
            // to instantiate a scoped dependency without injecting you have to create a service scope.
            using (var serviceScope = app.ApplicationServices.CreateScope())
            {
                var context = (AnonymizationServiceDBContext)
                    serviceScope.ServiceProvider.GetService(typeof(AnonymizationServiceDBContext));

                context.Database.Migrate();
            }
        }
    }
}
