
using AgreementService;

namespace LBHTenancyAPI.Interfaces
{
    public interface IArrearsServiceRequestBuilder
    {
        T BuildArrearsRequest<T>(T request) where T : WebRequest;
    }
}
