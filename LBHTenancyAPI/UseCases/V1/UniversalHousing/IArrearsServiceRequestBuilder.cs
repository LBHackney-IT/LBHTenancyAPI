
using AgreementService;

namespace LBHTenancyAPI.UseCases.V1.UniversalHousing
{
    public interface IArrearsServiceRequestBuilder
    {
        T BuildArrearsRequest<T>(T request) where T : WebRequest;
    }
}
