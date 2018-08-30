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
            //log.LogInformation($"Creating action Diary record with order (tenancyRef ref: {request.ArrearsAction.TenancyAgreementRef})");
            var actionDiaryRequest = requestBuilder.BuildArrearsRequest(request);

            var response = await _arrearsActionDiaryGateway.CreateActionDiaryEntryAsync(request);

            if (!response.Success)
                throw new ArrearsActionDiaryServiceException();
            return response;
        }

        public class ArrearsActionDiaryServiceException : Exception
        {

        }
    }
}
