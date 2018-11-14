using System;
using System.Threading.Tasks;
using AgreementService;
using LBHTenancyAPI.Gateways.V2.Arrears;
using LBHTenancyAPI.Gateways.V2.Arrears.UniversalHousing;
using LBHTenancyAPI.UseCases.V2.ArrearsActions.Models;

namespace LBHTenancyAPI.UseCases.V2.ArrearsActions
{
    /// <summary>
    /// Use Case for creating Arrears actions diary entry
    /// </summary>
    public class CreateArrearsActionDiaryUseCase: ICreateArrearsActionDiaryUseCase
    {
        private readonly IArrearsActionDiaryGateway _arrearsActionDiaryGateway;
        public readonly IArrearsServiceRequestBuilder _requestBuilder;
        public CreateArrearsActionDiaryUseCase(
            IArrearsActionDiaryGateway arrearsActionDiaryGateway,
            IArrearsServiceRequestBuilder arrearsRequestBuilder)
        {
            _arrearsActionDiaryGateway = arrearsActionDiaryGateway;
            _requestBuilder = arrearsRequestBuilder;
        }

        public async Task<ArrearsActionResponse> ExecuteAsync(ActionDiaryRequest request)
        {
            var thisRequest = _requestBuilder.BuildNewActionDiaryRequest(request);

            var response = await _arrearsActionDiaryGateway.CreateActionDiaryEntryAsync(thisRequest);

            if (response.Success)
            {
                if (request.Username != null || request.Username != "")
                {
                    await _arrearsActionDiaryGateway.UpdateRecordingUserName(request.Username,
                        response.ArrearsAction.Id);
                }

            }
            return response;
        }
    }
}
