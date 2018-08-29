using System;
using LBHTenancyAPI.Interfaces;
using System.Threading.Tasks;
using LBHTenancyAPI.UseCases;
using AgreementService;
using LBHTenancyAPITest.Helpers;

namespace LBHTenancyAPITest.Services
{
    public class FakeArrearsActionDiaryService : IArrearsActionDiaryService
    {
        private IArrearsActionDiaryService actionDiaryService;
        private ILoggerAdapter<CreateArrearsActionDiaryUseCase> logger;

        public FakeArrearsActionDiaryService(ILoggerAdapter<CreateArrearsActionDiaryUseCase> log)
        {
            logger = log;
        }

        public Task<AgreementService.ArrearsActionResponse> CreateActionDiaryRecord(AgreementService.ArrearsActionCreateRequest request)
        {
            
            logger.LogInformation($"ArrearsActionDiary/CreateArrearsActionAsync(): Sent request to upstream arrearsAgreementServiceClient (Request ref: {request.ArrearsAction.TenancyAgreementRef})");
            var response = Fake.CreateArrearsActionAsync(Fake.GenerateActionDiaryRequest());
            logger.LogInformation($"ArrearsActionDiary/CreateArrearsActionAsync(): Received response from upstream arrearsAgreementServiceClient (Request ref: {request.ArrearsAction.TenancyAgreementRef})");
            return Task.Run(() => response);
        }
    }
}
