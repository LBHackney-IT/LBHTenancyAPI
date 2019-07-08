using System;
using System.Threading.Tasks;
using AgreementService;
using LBH.Data.Domain;
using LBHTenancyAPI.Gateways.V1;
using LBHTenancyAPI.Gateways.V2.Arrears;
using LBHTenancyAPI.Gateways.V2.Arrears.UniversalHousing;
using LBHTenancyAPI.UseCases.V1;
using LBHTenancyAPI.UseCases.V2.ArrearsActions.Models;
using MessagePack;

namespace LBHTenancyAPI.UseCases.V2.ArrearsActions
{
    /// <summary>
    /// Use Case for creating Arrears actions diary entry
    /// </summary>
    public class CreateArrearsActionDiaryUseCase: ICreateArrearsActionDiaryUseCase
    {
        private readonly IArrearsActionDiaryGateway _arrearsActionDiaryGateway;
        private readonly ITenanciesGateway _tenanciesGateway;
        private readonly IArrearsServiceRequestBuilder _requestBuilder;
        public CreateArrearsActionDiaryUseCase(IArrearsActionDiaryGateway arrearsActionDiaryGateway,
            ITenanciesGateway tenanciesGateway,
            IArrearsServiceRequestBuilder arrearsRequestBuilder)
        {
            _arrearsActionDiaryGateway = arrearsActionDiaryGateway;
            _tenanciesGateway = tenanciesGateway;
            _requestBuilder = arrearsRequestBuilder;
        }

        public async Task<ArrearsActionResponse> ExecuteAsync(ActionDiaryRequest request)
        {
            Tenancy tenancy = _tenanciesGateway.GetTenancyForRef(request.TenancyAgreementRef);

            var thisRequest = _requestBuilder.BuildNewActionDiaryRequest(request, tenancy.CurrentBalance);

            var response = await _arrearsActionDiaryGateway.CreateActionDiaryEntryAsync(thisRequest);

            if (response.Success)
            {
                var requestDateTime = request.CreatedDate.GetValueOrDefault(DateTime.Now);

                await _arrearsActionDiaryGateway.UpdateRecordingDetails(request.Username,
                    response.ArrearsAction.Id, requestDateTime);
                response.ArrearsAction.UserName = request.Username;
                response.ArrearsAction.ActionDate = requestDateTime;
            }
            return response;
        }
    }
}
