using System;
using LBHTenancyAPI.Interfaces;
using System.Threading.Tasks;
using LBHTenancyAPI.ArrearsAgreementService;
using LBHTenancyAPI.UseCases;


namespace LBHTenancyAPI.Services
{
    public class ArrearsActionDiaryService :IArrearsActionDiaryService
    {
        private IArrearsActionDiaryService actionDiaryService;

        private ArrearsAgreementServiceClient arrearsAgreementServiceClient;
        private ILoggerAdapter<CreateArrearsActionDiaryUseCase> logger;

        public ArrearsActionDiaryService(ILoggerAdapter<CreateArrearsActionDiaryUseCase> log)
        {
            arrearsAgreementServiceClient = new ArrearsAgreementServiceClient();
            logger = log;
        }


        public Task<ArrearsActionResponse> CreateArrearsActionAsync(ArrearsActionCreateRequest request)
        {

            logger.LogInformation($"ArrearsActionDiary/CreateArrearsActionAsync(): Sent request to upstream arrearsAgreementServiceClient (Request ref: {request.ArrearsAction.TenancyAgreementRef})");
            var response = arrearsAgreementServiceClient.CreateArrearsActionAsync(request);
            logger.LogInformation($"ArrearsActionDiary/CreateArrearsActionAsync(): Received response from upstream arrearsAgreementServiceClient (Request ref: {request.ArrearsAction.TenancyAgreementRef})");
            return response;
        }
    }
}
