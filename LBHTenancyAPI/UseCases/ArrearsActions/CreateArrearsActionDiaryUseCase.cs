using System;
using LBHTenancyAPI.Interfaces;
using System.Threading.Tasks;
using AgreementService;
using LBHTenancyAPI.Gateways;

namespace LBHTenancyAPI.UseCases
{
    public class CreateArrearsActionDiaryUseCase: ICreateArrearsActionDiaryUseCase
    {
        private readonly IArrearsActionDiaryGateway _arrearsActionDiaryGateway;
        public IArrearsServiceRequestBuilder requestBuilder;
        public CreateArrearsActionDiaryUseCase(
            IArrearsActionDiaryGateway arrearsActionDiaryGateway,
            IArrearsServiceRequestBuilder ArrearsRequestBuilder)
        {
            _arrearsActionDiaryGateway = arrearsActionDiaryGateway;
            requestBuilder = ArrearsRequestBuilder;
        }

        public async Task<ArrearsActionResponse> CreateActionDiaryRecordsAsync(ArrearsActionCreateRequest request)
        {
            var actionDiaryRequest = requestBuilder.BuildArrearsRequest(request);
            var response = await _arrearsActionDiaryGateway.CreateActionDiaryEntryAsync(request);

            return response;
        }
    }
}
