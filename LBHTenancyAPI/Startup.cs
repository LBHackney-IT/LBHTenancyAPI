using System;
using System.ServiceModel;
using AgreementService;
using LBHTenancyAPI.Factories;
using LBHTenancyAPI.Gateways;
using LBHTenancyAPI.Gateways.Arrears;
using LBHTenancyAPI.Gateways.Arrears.Impl;
using LBHTenancyAPI.Infrastructure;
using LBHTenancyAPI.Interfaces;
using LBHTenancyAPI.Middleware;
using LBHTenancyAPI.Services;
using LBHTenancyAPI.Settings;
using LBHTenancyAPI.UseCases;
using LBHTenancyAPI.UseCases.ArrearsActions;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Swashbuckle.AspNetCore.Swagger;

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
            services.AddSingleton<IWCFClientFactory, WCFClientFactory>();

            services.AddTransient<IArrearsAgreementService>(s=>
            {
                var clientFactory = s.GetService<IWCFClientFactory>();
                var client = clientFactory.CreateClient<IArrearsAgreementServiceChannel>(Environment.GetEnvironmentVariable("ServiceSettings__AgreementServiceEndpoint"));
                if(client.State != CommunicationState.Opened)
                    client.Open();
                return client;
            });
            services.AddTransient<ICredentialsService, CredentialsService>();

            //add swagger gen to generate the swagger.json file
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new Info { Title = "TenancyAPI", Version = "v1" });
            });

            services.AddLogging(configure =>
            {
                configure.AddConfiguration(Configuration.GetSection("Logging"));
                configure.AddConsole();
                configure.AddDebug();
                configure.AddProvider(new SentryLoggerProvider(settings.SentrySettings?.Url));
            });

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
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "TenancyAPI");
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
