using System;
using AgreementService;
using LBHTenancyAPI.UseCases.Service;
using Microsoft.Extensions.Configuration;

namespace LBHTenancyAPI.UseCases.V1.UniversalHousing.Impl
{
    public class CredentialsService : ICredentialsService
    {
        private readonly IConfiguration _configuration;

        public CredentialsService(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public string GetUhSourceSystem()
        {
            return Environment.GetEnvironmentVariable("Credentials__UHServiceSystemCredentials__UserName");
        }

        public UserCredential GetUhUserCredentials()
        {
            return new UserCredential
            {
                UserName = Environment.GetEnvironmentVariable("Credentials__UHServiceUserCredentials__UserName"),
                UserPassword = Environment.GetEnvironmentVariable("Credentials__UHServiceUserCredentials__UserPassword")
            };
        }
    }
}
