using AgreementService;

namespace LBHTenancyAPI.Services
{
    public class CredentialsService : ICredentialsService
    {
        public string GetUhSourceSystem()
        {
            return "HackneyAPI";
        }

        public UserCredential GetUhUserCredentials()
        {
            return new UserCredential
            {
                UserName = "HackneyAPI",
                UserPassword = "Hackney1",
            };
        }
    }
}
