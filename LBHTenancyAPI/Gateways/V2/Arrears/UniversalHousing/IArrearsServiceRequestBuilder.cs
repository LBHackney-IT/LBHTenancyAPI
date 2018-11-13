
using AgreementService;
using LBHTenancyAPI.UseCases.V2.ArrearsActions.Models;

namespace LBHTenancyAPI.Gateways.V2.Arrears.UniversalHousing
{
    public interface IArrearsServiceRequestBuilder
    {
        T BuildArrearsRequest<T>(T request) where T : WebRequest;
        ArrearsActionCreateRequest BuildNewActionDiaryRequest(ActionDiaryRequest request);
    }
}
