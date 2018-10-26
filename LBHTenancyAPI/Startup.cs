using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.ServiceModel;
using AgreementService;
using LBHTenancyAPI.Extensions.Versioning;
using LBHTenancyAPI.Factories;
using LBHTenancyAPI.Gateways.V1;
using LBHTenancyAPI.Gateways.V1.Arrears;
using LBHTenancyAPI.Gateways.V1.Arrears.Impl;
using LBHTenancyAPI.Gateways.V1.Contacts;
using LBHTenancyAPI.Infrastructure.V1.Dynamics365.Authentication;
using LBHTenancyAPI.Infrastructure.V1.Dynamics365.Client.Factory;
using LBHTenancyAPI.Infrastructure.V1.Health;
using LBHTenancyAPI.Infrastructure.V1.Logging;
using LBHTenancyAPI.Middleware;
using LBHTenancyAPI.Services;
using LBHTenancyAPI.Services.Impl;
using LBHTenancyAPI.Settings;
using LBHTenancyAPI.UseCases.V1;
using LBHTenancyAPI.UseCases.V1.ArrearsActions;
using LBHTenancyAPI.UseCases.V1.ArrearsAgreements;
using LBHTenancyAPI.UseCases.V1.Contacts;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ApiExplorer;
using Microsoft.AspNetCore.Mvc.Versioning;
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
            
            ConfigureTenancies(services, connectionString);

            ConfigureUniversalHousingRelated(services);

            ConfigureSearch(services, connectionString);

            ConfigureFactoriesAndHealthChecks(services, connectionString);

            ConfigureContacts(services, settings);
            
            ConfigureApiVersioning(services);

            //Set [ApiVersion("x")] on Controllers to automatically add them to swagger docs
            ConfigureSwaggerGen(services);

            ConfigureLogging(services, settings);

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

            ConfigureSwaggerUI(app);

            //required for swagger to work
            app.UseMvc(routes =>
            {
                // SwaggerGen won't find controllers that are routed via this technique.
                routes.MapRoute("default", "{controller=Home}/{action=Index}/{id?}");
            });
        }

        private void ConfigureLogging(IServiceCollection services, ConfigurationSettings settings)
        {
            services.AddLogging(configure =>
            {
                configure.AddConfiguration(Configuration.GetSection("Logging"));
                configure.AddConsole();
                configure.AddDebug();
                //logs errors to sentry if configured
                if (!string.IsNullOrEmpty(settings.SentrySettings?.Url))
                    configure.AddProvider(new SentryLoggerProvider(settings.SentrySettings?.Url,settings.SentrySettings?.Environment));
            });
        }

        private static void ConfigureApiVersioning(IServiceCollection services)
        {
            services.AddApiVersioning(o =>
            {
                o.DefaultApiVersion = new ApiVersion(1, 0);
                o.AssumeDefaultVersionWhenUnspecified =
                    true; // assume that the caller wants the default version if they don't specify
                o.ApiVersionReader = new UrlSegmentApiVersionReader(); // read the version number from the accept header)
            });
        }

        private static void ConfigureTenancies(IServiceCollection services, string connectionString)
        {
            services.AddTransient<IListTenancies, ListTenancies>();
            services.AddTransient<IListAllArrearsActions, ListAllArrearsActions>();
            services.AddTransient<IListAllPayments, ListAllPayments>();
            services.AddTransient<ITenancyDetailsForRef, TenancyDetailsForRef>();
            services.AddTransient<ITenanciesGateway>(s => new UhTenanciesGateway(connectionString));
        }

        private static void ConfigureSearch(IServiceCollection services, string connectionString)
        {
            services.AddTransient<UseCases.V1.Search.ISearchTenancyUseCase,UseCases.V1.Search.SearchTenancyUseCase>();
            services.AddTransient<Gateways.V1.Search.ISearchGateway>(s =>new Gateways.V1.Search.SearchGateway(connectionString));

            services.AddTransient<UseCases.V1.Search.ISearchTenancyUseCase,UseCases.V1.Search.SearchTenancyUseCase>();
            services.AddTransient<Gateways.V2.Search.ISearchGateway>(s =>new Gateways.V2.Search.SearchGateway(connectionString));
        }

        private static void ConfigureUniversalHousingRelated(IServiceCollection services)
        {
            services.AddTransient<IArrearsActionDiaryGateway, ArrearsActionDiaryGateway>();
            services.AddTransient<ICreateArrearsActionDiaryUseCase, CreateArrearsActionDiaryUseCase>();
            services.AddTransient<IArrearsServiceRequestBuilder, ArrearsServiceRequestBuilder>();

            services.AddTransient<IArrearsAgreementGateway, ArrearsAgreementGateway>();
            services.AddTransient<ICreateArrearsAgreementUseCase, CreateArrearsAgreementUseCase>();

            services.AddTransient<ICredentialsService, CredentialsService>();
        }

        private static void ConfigureFactoriesAndHealthChecks(IServiceCollection services, string connectionString)
        {
            services.AddSingleton<IWCFClientFactory, WCFClientFactory>();

            services.AddTransient<IArrearsAgreementServiceChannel>(s =>
            {
                var clientFactory = s.GetService<IWCFClientFactory>();
                var client =
                    clientFactory.CreateClient<IArrearsAgreementServiceChannel>(
                        Environment.GetEnvironmentVariable("ServiceSettings__AgreementServiceEndpoint"));
                if (client.State != CommunicationState.Opened)
                    client.Open();
                return client;
            });

            var loggerFactory = new LoggerFactory();
            var sqlHealthCheckLogger = loggerFactory.CreateLogger<SqlConnectionHealthCheck>();

            services.AddSingleton<ISqlConnectionFactory>(s =>new SqlConnectionFactory(connectionString, loggerFactory.CreateLogger<SqlConnectionFactory>()));

            services.AddTransient<SqlConnectionHealthCheck>(s =>new SqlConnectionHealthCheck(s.GetService<ISqlConnectionFactory>(), sqlHealthCheckLogger));
            services.AddHealthChecks(healthCheck =>healthCheck.AddCheck<SqlConnectionHealthCheck>("SqlConnectionHealthCheck", TimeSpan.FromSeconds(1)));
        }

        /// <summary>
        /// Automatically Generates Swagger docs with XML comments based on the [ApiVersion("x")] on a controller
        /// and assigns that 
        /// </summary>
        /// <param name="services"></param>
        private void ConfigureSwaggerGen(IServiceCollection services)
        {
        //add swagger gen to generate the swagger.json file - delayed execution
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
                    c.SwaggerDoc(version, new Info {Title = $"TenancyAPI {version}", Version = version});
                }

                c.CustomSchemaIds(x => x.FullName);
                // Set the comments path for the Swagger JSON and UI.
                var xmlFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
                var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
                c.IncludeXmlComments(xmlPath);
            });

            services.AddSingleton<IApiVersionDescriptionProvider, DefaultApiVersionDescriptionProvider>();
        }

        private static void ConfigureContacts(IServiceCollection services, ConfigurationSettings settings)
        {
            services.AddTransient<IDynamics365AuthenticationService>(s => new Dynamics365AuthenticationService(settings.Dynamics365Settings));
            services.AddSingleton<IDynamics365ClientFactory>(s => new Dynamics365ClientFactory(settings.Dynamics365Settings, s.GetService<IDynamics365AuthenticationService>()));
            services.AddTransient<IContactsGateway, Dynamics365RestApiContactsGateway>();
            services.AddTransient<IGetContactsForTenancyUseCase, GetContactsForTenancyUseCase>();
        }

        private void ConfigureSwaggerUI(IApplicationBuilder app)
        {
            //Get ApiVersionDescriptionProvider from API Explorer
            var api = app.ApplicationServices.GetService<IApiVersionDescriptionProvider>();
            //Get All ApiVersions, 
            _apiVersions = api.ApiVersionDescriptions.Select(s => s).ToList();
            //Swagger ui to view the swagger.json file
            app.UseSwaggerUI(c =>
            {
                foreach (var apiVersionDescription in _apiVersions)
                {
                    //Create a swagger endpoint for each swagger version
                    c.SwaggerEndpoint($"{apiVersionDescription.GetFormattedApiVersion()}/swagger.json",
                        $"TenancyAPI {apiVersionDescription.GetFormattedApiVersion()}");
                }
            });

            app.UseSwagger();
        }
    }
}
