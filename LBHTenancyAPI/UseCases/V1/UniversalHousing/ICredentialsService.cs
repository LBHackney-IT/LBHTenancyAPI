using AgreementService;

namespace LBHTenancyAPI.UseCases.Service
{
    public interface ICredentialsService
    {
        string GetUhSourceSystem();
        UserCredential GetUhUserCredentials();
    }
}
