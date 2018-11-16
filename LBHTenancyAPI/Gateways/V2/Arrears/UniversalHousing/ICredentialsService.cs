using AgreementService;

namespace LBHTenancyAPI.Services.V2
{
    public interface ICredentialsService
    {
        string GetUhSourceSystem();
        UserCredential GetUhUserCredentials();
    }
}
