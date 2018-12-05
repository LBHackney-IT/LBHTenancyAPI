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
using System.Configuration;
using LBHTenancyAPI.Gateways.V1.Arrears.UniversalHousing;
using LBHTenancyAPI.Gateways.V1.Arrears.UniversalHousing.Impl;
using LBHTenancyAPI.Infrastructure.V1.Versioning;
using LBHTenancyAPI.UseCases.V1.Versioning;

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
                o.AssumeDefaultVersionWhenUnspecified = true; // assume that the caller wants the default version if they don't specify
                o.ApiVersionReader = new UrlSegmentApiVersionReader(); // read the version number from the url segment header)
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

            services.AddTransient<UseCases.V2.Search.ISearchTenancyUseCase, UseCases.V2.Search.SearchTenancyUseCase>();
            services.AddTransient<Gateways.V2.Search.ISearchGateway>(s => new Gateways.V2.Search.SearchGateway(connectionString));
        }

        public static void ConfigureServiceDetails(this IServiceCollection services, UseCases.V1.Service.ServiceDetails serviceDetails)
        {
            services.AddTransient<UseCases.V1.Service.IGetServiceDetailsUseCase>(s=> new UseCases.V1.Service.GetServiceDetailsUseCase(s.GetService<IGetVersionUseCase>(), serviceDetails));
            services.AddTransient<UseCases.V1.Versioning.IGetVersionUseCase, UseCases.V1.Versioning.GetVersionUseCase>();
        }

        public static void ConfigureUniversalHousingRelated(this IServiceCollection services, string connectionString)
        {
            services.AddTransient<IArrearsActionDiaryGateway, ArrearsActionDiaryGateway>();
            services.AddTransient<ICreateArrearsActionDiaryUseCase, CreateArrearsActionDiaryUseCase>();
            services.AddTransient<IArrearsServiceRequestBuilder, ArrearsServiceRequestBuilder>();

            services.AddTransient<IArrearsAgreementGateway, ArrearsAgreementGateway>();
            services.AddTransient<ICreateArrearsAgreementUseCase, CreateArrearsAgreementUseCase>();

            services.AddTransient<ICredentialsService, CredentialsService>();

            services.AddTransient<UseCases.V2.ArrearsActions.ICreateArrearsActionDiaryUseCase, UseCases.V2.ArrearsActions.CreateArrearsActionDiaryUseCase>();
            services.AddTransient<Gateways.V2.Arrears.IArrearsActionDiaryGateway>(s => new Gateways.V2.Arrears.Impl.ArrearsActionDiaryGateway(s.GetService<IArrearsAgreementServiceChannel>(), connectionString));
            services.AddTransient<Gateways.V2.Arrears.UniversalHousing.IArrearsServiceRequestBuilder, Gateways.V2.Arrears.UniversalHousing.Impl.ArrearsServiceRequestBuilder>();
            services.AddTransient<Gateways.V2.Arrears.UniversalHousing.ICredentialsService, Gateways.V2.Arrears.UniversalHousing.Impl.CredentialsService>();
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



        public static void ConfigureContacts(this IServiceCollection services, LBHTenancyAPI.Settings.ConfigurationSettings settings)
        {
            services.AddTransient<IDynamics365AuthenticationService>(s => new Dynamics365AuthenticationService(settings.Dynamics365Settings));
            services.AddSingleton<IDynamics365ClientFactory>(s => new Dynamics365ClientFactory(settings.Dynamics365Settings, s.GetService<IDynamics365AuthenticationService>()));
            services.AddTransient<IContactsGateway, Dynamics365RestApiContactsGateway>();
            services.AddTransient<IGetContactsForTenancyUseCase, GetContactsForTenancyUseCase>();
        }

        public static List<ApiVersionDescription> ConfigureSwaggerUI(this IApplicationBuilder app, List<ApiVersionDescription> apiVersions)
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

            return apiVersions;
        }
    }
}
