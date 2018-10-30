using System;
using System.Collections.Generic;
using LBHTenancyAPI.Infrastructure.V1.Middleware;
using LBHTenancyAPI.Infrastructure.V1.Services;
using LBHTenancyAPI.Settings;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.ApiExplorer;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace LBHTenancyAPI
{
    /// <summary>
    /// Configures API
    /// </summary>
    public class Startup
    {

        public Startup(IHostingEnvironment env)
        {
            var builder = new ConfigurationBuilder()
                .SetBasePath(env.ContentRootPath)
                .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
                .AddJsonFile($"appsettings.{env.EnvironmentName}.json", optional: true)
                .AddEnvironmentVariables();
                
            Configuration = builder.Build();
        }

        public IConfiguration Configuration { get; }
        private List<ApiVersionDescription> _apiVersions { get; set; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {   
            //get settings from appSettings.json and EnvironmentVariables
            services.Configure<ConfigurationSettings>(Configuration);
            var settings = Configuration.Get<ConfigurationSettings>();

            var connectionString = Environment.GetEnvironmentVariable("UH_URL");

            services.AddMvc();
            
            services.ConfigureTenancies(connectionString);

            services.ConfigureUniversalHousingRelated();

            services.ConfigureSearch(connectionString);

            services.ConfigureFactoriesAndHealthChecks(connectionString);

            services.ConfigureContacts(settings);

            services.ConfigureApiVersioning();

            //Set [ApiVersion("x")] on Controllers to automatically add them to swagger docs
            services.ConfigureSwaggerGen();


            services.ConfigureLogging(Configuration, settings);

        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        // Although there are zero references do not delete
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {

            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            //Register exception handling middleware early so exceptions are handled and formatted
            app.UseMiddleware<CustomExceptionHandlerMiddleware>();

            app.ConfigureSwaggerUI(_apiVersions);

            //required for swagger to work
            app.UseMvc(routes =>
            {
                // SwaggerGen won't find controllers that are routed via this technique.
                routes.MapRoute("default", "{controller=Home}/{action=Index}/{id?}");
            });
        }
    }
}
