using LBHTenancyAPI.Connected_Services.AgreementService;

namespace LBHTenancyAPI.Services
{
    public interface ICredentialsService
    {
        string GetUhSourceSystem();
        UserCredential GetUhUserCredentials();
    }
}
