using System.Threading.Tasks;
using AgreementService;
using LBHTenancyAPI.Gateways;
using LBHTenancyAPI.Interfaces;

namespace LBHTenancyAPI.UseCases.ArrearsActions
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
            request = requestBuilder.BuildArrearsRequest(request);

            var response = await _arrearsActionDiaryGateway.CreateActionDiaryEntryAsync(request);

            return response;
        }
    }
}
