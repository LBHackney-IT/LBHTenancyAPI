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
using LBHTenancyAPI.UseCases.Service;
using LBHTenancyAPI.UseCases.Versioning;
using Swashbuckle.AspNetCore.SwaggerGen;
using System.Configuration;

namespace LBHTenancyAPI.Infrastructure.V1.Services
{
    public static class ConfigureServices
    {
        public static void ConfigureLogging(this IServiceCollection services, IConfiguration configuration, LBHTenancyAPI.Settings.ConfigurationSettings settings)
        {
            services.AddLogging(configure =>
            {
                configure.AddConfiguration(configuration.GetSection("Logging"));
                configure.AddConsole();
                configure.AddDebug();
                //logs errors to sentry if configured
                if (!string.IsNullOrEmpty(settings.SentrySettings?.Url))
                    configure.AddProvider(new SentryLoggerProvider(settings.SentrySettings?.Url, settings.SentrySettings?.Environment));
            });
        }

        public static void ConfigureApiVersioning(this IServiceCollection services)
        {
            services.AddApiVersioning(o =>
            {
                o.DefaultApiVersion = new ApiVersion(1, 0);
                o.AssumeDefaultVersionWhenUnspecified =
                    true; // assume that the caller wants the default version if they don't specify
                o.ApiVersionReader = new UrlSegmentApiVersionReader(); // read the version number from the accept header)
            });
        }

        public static void ConfigureTenancies(this IServiceCollection services, string connectionString)
        {
            services.AddTransient<IListTenancies, ListTenancies>();
            services.AddTransient<IListAllArrearsActions, ListAllArrearsActions>();
            services.AddTransient<IListAllPayments, ListAllPayments>();
            services.AddTransient<ITenancyDetailsForRef, TenancyDetailsForRef>();
            services.AddTransient<ITenanciesGateway>(s => new UhTenanciesGateway(connectionString));
        }

        public static void ConfigureSearch(this IServiceCollection services, string connectionString)
        {
            services.AddTransient<UseCases.V1.Search.ISearchTenancyUseCase, UseCases.V1.Search.SearchTenancyUseCase>();
            services.AddTransient<Gateways.V1.Search.ISearchGateway>(s => new Gateways.V1.Search.SearchGateway(connectionString));

            services.AddTransient<UseCases.V1.Search.ISearchTenancyUseCase, UseCases.V1.Search.SearchTenancyUseCase>();
            services.AddTransient<Gateways.V2.Search.ISearchGateway>(s => new Gateways.V2.Search.SearchGateway(connectionString));
        }

        public static void ConfigureUniversalHousingRelated(this IServiceCollection services)
        {
            services.AddTransient<IArrearsActionDiaryGateway, ArrearsActionDiaryGateway>();
            services.AddTransient<ICreateArrearsActionDiaryUseCase, CreateArrearsActionDiaryUseCase>();
            services.AddTransient<IArrearsServiceRequestBuilder, ArrearsServiceRequestBuilder>();

            services.AddTransient<IArrearsAgreementGateway, ArrearsAgreementGateway>();
            services.AddTransient<ICreateArrearsAgreementUseCase, CreateArrearsAgreementUseCase>();

            services.AddTransient<ICredentialsService, CredentialsService>();
        }

        public static void ConfigureFactoriesAndHealthChecks(this IServiceCollection services, string connectionString)
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

            services.AddSingleton<ISqlConnectionFactory>(s => new SqlConnectionFactory(connectionString, loggerFactory.CreateLogger<SqlConnectionFactory>()));

            services.AddTransient<SqlConnectionHealthCheck>(s => new SqlConnectionHealthCheck(s.GetService<ISqlConnectionFactory>(), sqlHealthCheckLogger));
            services.AddHealthChecks(healthCheck => healthCheck.AddCheck<SqlConnectionHealthCheck>("SqlConnectionHealthCheck", TimeSpan.FromSeconds(1)));
        }

        /// <summary>
        /// Automatically Generates Swagger docs with XML comments based on the [ApiVersion("x")] on a controller
        /// and assigns that 
        /// </summary>
        /// <param name="services"></param>
        public static void ConfigureSwaggerGen(this IServiceCollection services, List<ApiVersionDescription> apiVersions)
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
                foreach (var apiVersion in apiVersions)
                {
                    var version = $"v{apiVersion.ApiVersion.ToString()}";
                    c.SwaggerDoc(version, new Info { Title = $"TenancyAPI {version}", Version = version });
                }

                c.CustomSchemaIds(x => x.FullName);
                // Set the comments path for the Swagger JSON and UI.
                var xmlFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
                var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
                if (File.Exists(xmlPath))
                    c.IncludeXmlComments(xmlPath);
            });

            services.AddSingleton<IApiVersionDescriptionProvider, DefaultApiVersionDescriptionProvider>();
        }

        public static void ConfigureContacts(this IServiceCollection services, LBHTenancyAPI.Settings.ConfigurationSettings settings)
        {
            services.AddTransient<IDynamics365AuthenticationService>(s => new Dynamics365AuthenticationService(settings.Dynamics365Settings));
            services.AddSingleton<IDynamics365ClientFactory>(s => new Dynamics365ClientFactory(settings.Dynamics365Settings, s.GetService<IDynamics365AuthenticationService>()));
            services.AddTransient<IContactsGateway, Dynamics365RestApiContactsGateway>();
            services.AddTransient<IGetContactsForTenancyUseCase, GetContactsForTenancyUseCase>();
        }

        public static void ConfigureSwaggerUI(this IApplicationBuilder app, List<ApiVersionDescription> apiVersions)
        {
            //Get ApiVersionDescriptionProvider from API Explorer
            var api = app.ApplicationServices.GetService<IApiVersionDescriptionProvider>();
            //Get All ApiVersions, 
            apiVersions = api.ApiVersionDescriptions.Select(s => s).ToList();
            //Swagger ui to view the swagger.json file
            app.UseSwaggerUI(c =>
            {
                foreach (var apiVersionDescription in apiVersions)
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
