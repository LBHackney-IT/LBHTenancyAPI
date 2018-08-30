using System;
using LBHTenancyAPI.Interfaces;
using System.Threading.Tasks;
using AgreementService;

namespace LBHTenancyAPI.UseCases
{

    public interface ICreateArrearsActionDiaryUseCase
    {
        Task<object> CreateActionDiaryRecords(ArrearsActionCreateRequest request);
    }

    public class CreateArrearsActionDiaryUseCase: ICreateArrearsActionDiaryUseCase
    {
        public IArrearsActionDiaryService requestService;
        public IArrearsServiceRequestBuilder requestBuilder;
        public ILoggerAdapter<CreateArrearsActionDiaryUseCase> log;
        public CreateArrearsActionDiaryUseCase(IArrearsActionDiaryService actionDiaryService,
                                               IArrearsServiceRequestBuilder ArrearsRequestBuilder,
                                               ILoggerAdapter<CreateArrearsActionDiaryUseCase> logger)
        {
            requestService = actionDiaryService;
            requestBuilder = ArrearsRequestBuilder;
            log = logger;
        }

        public async Task<object> CreateActionDiaryRecords(ArrearsActionCreateRequest request)
        {
            log.LogInformation($"Creating action Diary record with order (tenancyRef ref: {request.ArrearsAction.TenancyAgreementRef})");
            var actionDiaryRequest = requestBuilder.BuildArrearsRequest(request);

            var response = await requestService.CreateActionDiaryRecord(request);

            if (!response.Success)
            {
                throw new ArrearsActionDiaryServiceException();
            }
            return response;
        }

        public class ArrearsActionDiaryServiceException : Exception
        {

        }
    }
}
