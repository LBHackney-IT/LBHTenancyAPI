using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using AgreementService;
using LBHTenancyAPI.Gateways;
using LBHTenancyAPI.Interfaces;
using LBHTenancyAPI.Services;
using LBHTenancyAPI.UseCases;
using LBHTenancyAPI.UseCases.ArrearsActions;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
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
            services.AddMvc();
            services.AddTransient<IListTenancies, ListTenancies>();
            services.AddTransient<IListAllArrearsActions, ListAllArrearsActions>();
            services.AddTransient<IListAllPayments, ListAllPayments>();
            services.AddTransient<ITenancyDetailsForRef, TenancyDetailsForRef>();
            services.AddTransient<ITenanciesGateway>(s => new UhTenanciesGateway(Environment.GetEnvironmentVariable("UH_URL")));
            services.AddTransient<IArrearsActionDiaryGateway, ArrearsActionDiaryGateway>();
            services.AddTransient<ICreateArrearsActionDiaryUseCase, CreateArrearsActionDiaryUseCase>();
            services.AddTransient<IArrearsServiceRequestBuilder, ArrearsServiceRequestBuilder>();
            services.AddTransient<IArrearsAgreementService, ArrearsAgreementServiceClient>();
            services.AddTransient<ICredentialsService, CredentialsService>();

            //add swagger gen to generate the swagger.json file
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new Info { Title = "TenancyAPI", Version = "v1" });
            });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
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
