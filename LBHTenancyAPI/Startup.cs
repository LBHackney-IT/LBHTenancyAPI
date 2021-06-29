using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using LBHTenancyAPI.Extensions.Versioning;
using LBHTenancyAPI.Infrastructure.V1.Services;
using LBHTenancyAPI.Middleware;
using LBHTenancyAPI.Settings;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ApiExplorer;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Swashbuckle.AspNetCore.Swagger;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace LBHTenancyAPI
{
    /// <summary>
    /// Configures API
    /// </summary>
    public class Startup
    {
        private readonly ILogger<Startup> _logger;

        public Startup(IHostingEnvironment env, ILogger<Startup> logger)
        {
            _logger = logger;
            var builder = new ConfigurationBuilder()
                .SetBasePath(env.ContentRootPath)
                .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
                .AddJsonFile($"appsettings.{env.EnvironmentName}.json", optional: true)
                .AddEnvironmentVariables();

            Configuration = builder.Build();
        }

        public IConfiguration Configuration { get; }
        private static List<ApiVersionDescription> _apiVersions { get; set; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            //get settings from appSettings.json and EnvironmentVariables
            services.Configure<ConfigurationSettings>(Configuration);
            var settings = Configuration.Get<ConfigurationSettings>();

            var connectionString = Environment.GetEnvironmentVariable("UH_URL");
            //var connectionString = "Data Source=127.0.0.1,1433;Initial Catalog=StubUH;User ID=sa;Password=Rooty-Tooty";
            // var connectionString = "Data Source=127.0.0.1;Initial Catalog=SOW2b;User ID=housingadmin;Password=Vcf:8efGbuEv2qmD";

            services.AddMvc();

            services.ConfigureTenancies(connectionString);

            services.ConfigureUniversalHousingRelated(connectionString);

            services.ConfigureSearch(connectionString);

            services.ConfigureFactoriesAndHealthChecks(connectionString);

            services.ConfigureContacts(settings);

            services.ConfigureApiVersioning();

            services.ConfigureServiceDetails(settings.ServiceDetailsSettings);

            //Set [ApiVersion("x")] on Controllers to automatically add them to swagger docs
            services.AddSingleton<IApiVersionDescriptionProvider, DefaultApiVersionDescriptionProvider>();

            //add swagger gen to generate the swagger.json file - delayed execution
            // Automatically Generates Swagger docs with XML comments based on the [ApiVersion("x")] on a controller
            // and assigns that
            services.AddSwaggerGen(c =>
            {
                c.AddSecurityDefinition("Token",
                    new ApiKeyScheme
                    {
                        In = "header",
                        Description = "Your Hackney API Key",
                        Name = "X-Api-Key",
                        Type = "apiKey"
                    });
                c.AddSecurityRequirement(new Dictionary<string, IEnumerable<string>>
                {
                    {"Token", Enumerable.Empty<string>()}
                });

                //Looks at the APIVersionAttribute [ApiVersion("x")] on controllers and decides whether or not
                //to include it in that version of the swagger document
                //Controllers must have this [ApiVersion("x")] to be included in swagger documentation!!
                c.DocInclusionPredicate((docName, apiDesc) =>
                {
                    var versions = apiDesc.ControllerAttributes()
                        .OfType<ApiVersionAttribute>()
                        .SelectMany(attr => attr.Versions).ToList();

                    var any = versions.Any(v => $"{v.GetFormattedApiVersion()}" == docName);
                    return any;
                });

                //Get every ApiVersion attribute specified and create swagger docs for them
                foreach (var apiVersion in _apiVersions)
                {
                    var version = $"v{apiVersion.ApiVersion.ToString()}";
                    c.SwaggerDoc(version, new Info
                    {
                        Title = $"TenancyAPI {version}",
                        Version = version,
                        Description = "Only superseded methods are included in newer api versions. " +
                                      "Please check older versions for more paths and newer versions " +
                                      "if a method has been marked deprecated",
                    });
                }

                c.CustomSchemaIds(x => x.FullName);
                // Set the comments path for the Swagger JSON and UI.
                var xmlFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
                var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
                if (File.Exists(xmlPath))
                    c.IncludeXmlComments(xmlPath);
            });


            services.ConfigureLogging(Configuration, settings, _logger);
            _logger.LogInformation("Logging configured");

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

            _apiVersions = app.ConfigureSwaggerUI(_apiVersions);

            //required for swagger to work
            app.UseMvc(routes =>
            {
                // SwaggerGen won't find controllers that are routed via this technique.
                routes.MapRoute("default", "{controller=Home}/{action=Index}/{id?}");
            });
        }
    }
}
