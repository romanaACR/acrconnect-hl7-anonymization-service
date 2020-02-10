using AcrConnect.Anonymization.Service.Extensions;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using AcrConnect.Anonymization.Service.Models;
using System.Net.Http;
using System;

namespace AcrConnect.Submission.Service.API
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddTriadService(Configuration);
            services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_2_2);
            services.AddLogging((options) => {
                options.AddConfiguration(Configuration.GetSection("Logging"));
                options.AddConsole();
                options.AddDebug();
                options.AddAnonymizationLogger();
            });
            
            services.AddMvc().AddXmlSerializerFormatters();

            services.AddOptions();

            var masterIdIndexServiceUrl = Environment.GetEnvironmentVariable("MASTER_ID_INDEX_SERVICE_URL");  
            services.AddHttpClient<IdMapping>("IdMappingService", client =>
            {
                client.BaseAddress = new System.Uri(masterIdIndexServiceUrl);
            }).ConfigurePrimaryHttpMessageHandler(() =>
            {
                return new HttpClientHandler
                {
                    ServerCertificateCustomValidationCallback = (message, cert, chain, errors) => true
                };
            });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }
            //  app.UseCors(builder =>builder.AllowAnyOrigin().AllowAnyHeader().AllowAnyMethod().AllowCredentials());
            app.UseHttpsRedirection();
            app.Initialize();
            app.UseMvc();
        }
    }
}
