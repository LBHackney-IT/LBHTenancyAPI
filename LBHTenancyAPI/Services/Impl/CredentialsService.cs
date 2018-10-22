using System;
using LBHTenancyAPI.Connected_Services.AgreementService;
using Microsoft.Extensions.Configuration;

namespace LBHTenancyAPI.Services.Impl
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
