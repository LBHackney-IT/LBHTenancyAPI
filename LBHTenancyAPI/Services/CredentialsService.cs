using AgreementService;
using LBHTenancyAPI.Settings.Credentials;
using Microsoft.Extensions.Configuration;

namespace LBHTenancyAPI.Services
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
            return _configuration.Get<ConfigurationSettings>().Credentials.UHServiceSystemCredentials.UserName;
        }

        public UserCredential GetUhUserCredentials()
        {
            return new UserCredential
            {
                UserName = _configuration.Get<ConfigurationSettings>().Credentials.UHServiceUserCredentials.UserName,
                UserPassword = _configuration.Get<ConfigurationSettings>().Credentials.UHServiceUserCredentials.UserPassword
            };
        }
    }
}
