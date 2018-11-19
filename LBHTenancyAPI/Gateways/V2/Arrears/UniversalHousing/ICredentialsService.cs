using AgreementService;

namespace LBHTenancyAPI.Gateways.V2.Arrears.UniversalHousing.Impl
{
    public interface ICredentialsService
    {
        string GetUhSourceSystem();
        UserCredential GetUhUserCredentials();
    }
}
