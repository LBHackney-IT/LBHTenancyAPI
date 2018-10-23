using System;
using System.Linq;
using System.ServiceModel;
using AgreementService;
using LBHTenancyAPI.Controllers.V1;
using LBHTenancyAPI.Factories;
using LBHTenancyAPI.Gateways;
using LBHTenancyAPI.Gateways.V1;
using LBHTenancyAPI.Gateways.V1.Arrears;
using LBHTenancyAPI.Gateways.V1.Arrears.Impl;
using LBHTenancyAPI.Gateways.V1.Contacts;
using LBHTenancyAPI.Gateways.V2.Search;
using LBHTenancyAPI.Infrastructure;
using LBHTenancyAPI.Infrastructure.V1.Dynamics365.Authentication;
using LBHTenancyAPI.Infrastructure.V1.Dynamics365.Client.Factory;
using LBHTenancyAPI.Infrastructure.V1.Health;
using LBHTenancyAPI.Infrastructure.V1.Logging;
using LBHTenancyAPI.Middleware;
using LBHTenancyAPI.Services;
using LBHTenancyAPI.Services.Impl;
using LBHTenancyAPI.Settings;
using LBHTenancyAPI.UseCases;
using LBHTenancyAPI.UseCases.V1;
using LBHTenancyAPI.UseCases.V1.ArrearsActions;
using LBHTenancyAPI.UseCases.V1.ArrearsAgreements;
using LBHTenancyAPI.UseCases.V1.Contacts;
using LBHTenancyAPI.UseCases.V1.Search;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Versioning;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Swashbuckle.AspNetCore.Swagger;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace LBHTenancyAPI
{
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

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            var environmentVariables = Environment.GetEnvironmentVariables();
            
            Console.WriteLine("Environment Variables");

            var serviceUserName = Environment.GetEnvironmentVariable("Credentials__UHServiceSystemCredentials__UserName");
            Console.WriteLine($"Credentials__UHServiceSystemCredentials__UserName isNullOrEmpty: {string.IsNullOrEmpty(serviceUserName)}");

            var userName = Environment.GetEnvironmentVariable("Credentials__UHServiceUserCredentials__UserName");
            Console.WriteLine($"Credentials__UHServiceUserCredentials__UserName isNullOrEmpty: {string.IsNullOrEmpty(userName)}");

            var password = Environment.GetEnvironmentVariable("Credentials__UHServiceUserCredentials__UserPassword");
            Console.WriteLine($"Credentials__UHServiceUserCredentials__UserPassword isNullOrEmpty: {string.IsNullOrEmpty(password)}");

            Console.WriteLine(environmentVariables);
            //get settings from appSettings.json and EnvironmentVariables
            services.Configure<ConfigurationSettings>(Configuration);
            var settings = Configuration.Get<ConfigurationSettings>();

            var connectionString = Environment.GetEnvironmentVariable("UH_URL");

            services.AddMvc();
            services.AddTransient<IListTenancies, ListTenancies>();
            services.AddTransient<IListAllArrearsActions, ListAllArrearsActions>();
            services.AddTransient<IListAllPayments, ListAllPayments>();
            services.AddTransient<ITenancyDetailsForRef, TenancyDetailsForRef>();
            services.AddTransient<ITenanciesGateway>(s => new UhTenanciesGateway(connectionString));
            services.AddTransient<IArrearsActionDiaryGateway, ArrearsActionDiaryGateway>();
            services.AddTransient<ICreateArrearsActionDiaryUseCase, CreateArrearsActionDiaryUseCase>();
            services.AddTransient<IArrearsServiceRequestBuilder, ArrearsServiceRequestBuilder>();

            services.AddTransient<IArrearsAgreementGateway, ArrearsAgreementGateway>();
            services.AddTransient<ICreateArrearsAgreementUseCase, CreateArrearsAgreementUseCase>();

            services.AddSingleton<IWCFClientFactory, WCFClientFactory>();
            
            services.AddTransient<IArrearsAgreementServiceChannel>(s=>
            {
                var clientFactory = s.GetService<IWCFClientFactory>();
                var client = clientFactory.CreateClient<IArrearsAgreementServiceChannel>(Environment.GetEnvironmentVariable("ServiceSettings__AgreementServiceEndpoint"));
                if(client.State != CommunicationState.Opened)
                    client.Open();
                return client;
            });

            services.AddTransient<ICredentialsService, CredentialsService>();

            services.AddTransient<LBHTenancyAPI.UseCases.V1.Search.ISearchTenancyUseCase, LBHTenancyAPI.UseCases.V1.Search.SearchTenancyUseCase>();
            services.AddTransient<LBHTenancyAPI.Gateways.V1.Search.ISearchGateway>(s=>new LBHTenancyAPI.Gateways.V1.Search.SearchGateway(connectionString));

            services.AddTransient<LBHTenancyAPI.UseCases.V1.Search.ISearchTenancyUseCase, LBHTenancyAPI.UseCases.V1.Search.SearchTenancyUseCase>();
            services.AddTransient<LBHTenancyAPI.Gateways.V2.Search.ISearchGateway>(s => new LBHTenancyAPI.Gateways.V2.Search.SearchGateway(connectionString));

            var loggerFactory = new LoggerFactory();
            var sqlHealthCheckLogger = loggerFactory.CreateLogger<SqlConnectionHealthCheck>();

            services.AddSingleton<ISqlConnectionFactory>(s => new SqlConnectionFactory(connectionString, loggerFactory.CreateLogger<SqlConnectionFactory>()));

            services.AddTransient<SqlConnectionHealthCheck>(s=> new SqlConnectionHealthCheck(s.GetService<ISqlConnectionFactory>(), sqlHealthCheckLogger));
            services.AddHealthChecks(healthCheck =>healthCheck.AddCheck<SqlConnectionHealthCheck>("SqlConnectionHealthCheck", TimeSpan.FromSeconds(1)));

            ConfigureContacts(services, settings);

            services.AddApiVersioning(o=>
            {
                o.DefaultApiVersion = new ApiVersion(1, 0);
                o.AssumeDefaultVersionWhenUnspecified = true; // assume that the caller wants the default version if they don't specify
                o.ApiVersionReader = new UrlSegmentApiVersionReader(); // read the version number from the accept header)
                o.Conventions.Controller<LBHTenancyAPI.Controllers.V1.SearchController>().HasDeprecatedApiVersion(new ApiVersion(1, 0));
                o.Conventions.Controller<LBHTenancyAPI.Controllers.V2.SearchController>().HasApiVersion(new ApiVersion(2, 0));

            }); // specify the default api version

            //add swagger gen to generate the swagger.json file
            services.AddSwaggerGen(c =>
            {
                c.DocInclusionPredicate((docName, apiDesc) =>
                {
                    var versions = apiDesc.ControllerAttributes()
                        .OfType<ApiVersionAttribute>()
                        .SelectMany(attr => attr.Versions);

                    return versions.Any(v => $"v{v.ToString()}" == docName);
                });
                c.SwaggerDoc("v1", new Info { Title = "TenancyAPI", Version = "v1" });
                c.SwaggerDoc("v2", new Info { Title = "TenancyAPI", Version = "v2" });
                c.CustomSchemaIds(x => x.FullName);
            });

            services.AddLogging(configure =>
            {
                configure.AddConfiguration(Configuration.GetSection("Logging"));
                configure.AddConsole();
                configure.AddDebug();
                if(!string.IsNullOrEmpty(settings.SentrySettings?.Url))
                    configure.AddProvider(new SentryLoggerProvider(settings.SentrySettings?.Url, settings.SentrySettings?.Environment));
            });

        }

        private static void ConfigureContacts(IServiceCollection services, ConfigurationSettings settings)
        {
            services.AddTransient<IDynamics365AuthenticationService>(s => new Dynamics365AuthenticationService(settings.Dynamics365Settings));
            services.AddSingleton<IDynamics365ClientFactory>(s => new Dynamics365ClientFactory(settings.Dynamics365Settings, s.GetService<IDynamics365AuthenticationService>()));
            services.AddTransient<IContactsGateway, Dynamics365RestApiContactsGateway>();
            services.AddTransient<IGetContactsForTenancyUseCase, GetContactsForTenancyUseCase>();

            

        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseMiddleware<CustomExceptionHandlerMiddleware>();
            

            //Swagger ui to view the swagger.json file
            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "TenancyAPI v1");
                c.SwaggerEndpoint("/swagger/v2/swagger.json", "TenancyAPI v2");
            });
            app.UseSwagger();

            //required for swagger to work
            app.UseMvc(routes =>
            {
                // SwaggerGen won't find controllers that are routed via this technique.
                routes.MapRoute("default", "{controller=Home}/{action=Index}/{id?}");
            });

            
        }
    }
}
