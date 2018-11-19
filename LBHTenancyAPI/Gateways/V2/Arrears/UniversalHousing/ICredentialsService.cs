using AgreementService;

namespace LBHTenancyAPI.Gateways.V2.Arrears.UniversalHousing
{
    public interface ICredentialsService
    {
        string GetUhSourceSystem();
        UserCredential GetUhUserCredentials();
    }
}
