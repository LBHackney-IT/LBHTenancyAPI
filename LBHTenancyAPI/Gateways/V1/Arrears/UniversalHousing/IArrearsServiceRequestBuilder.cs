
using AgreementService;

namespace LBHTenancyAPI.Gateways.V1.Arrears.UniversalHousing
{
    public interface IArrearsServiceRequestBuilder
    {
        T BuildArrearsRequest<T>(T request) where T : WebRequest;
    }
}
