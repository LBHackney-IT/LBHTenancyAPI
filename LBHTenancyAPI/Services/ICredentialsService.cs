using AgreementService;

namespace LBHTenancyAPI.Services
{
    public interface ICredentialsService
    {
        string GetUhSourceSystem();
        UserCredential GetUhUserCredentials();
    }
}
