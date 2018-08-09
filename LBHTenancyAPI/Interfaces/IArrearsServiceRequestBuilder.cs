
using AgreementService;

namespace LBHTenancyAPI.Interfaces
{
    public interface IArrearsServiceRequestBuilder
    {
        ArrearsActionCreateRequest BuildArrearsRequest(ArrearsActionCreateRequest request);
    }
}
