using LBHTenancyAPI.ArrearsAgreementService;

namespace LBHTenancyAPI.Interfaces
{
    public interface IArrearsServiceRequestBuilder
    {
        ArrearsActionCreateRequest BuildArrearsRequest(ArrearsActionCreateRequest request);
    }
}
