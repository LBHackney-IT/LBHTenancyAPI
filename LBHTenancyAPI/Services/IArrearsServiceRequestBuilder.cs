
using AgreementService;

namespace LBHTenancyAPI.Services
{
    public interface IArrearsServiceRequestBuilder
    {
        T BuildArrearsRequest<T>(T request) where T : WebRequest;
    }
}
