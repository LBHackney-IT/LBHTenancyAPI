using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using AgreementService;
using LBHTenancyAPI.Gateways.V1.Arrears;
using LBHTenancyAPI.Infrastructure.V1.UseCase.Execution;
using LBHTenancyAPI.Infrastructure.V1.Validation;
using LBHTenancyAPI.UseCases.V1.ArrearsAgreements.Models;

namespace LBHTenancyAPI.UseCases.V1.ArrearsAgreements
{
    public class CreateArrearsAgreementUseCase : ICreateArrearsAgreementUseCase
    {
        private readonly IArrearsAgreementGateway _arrearsAgreementGateway;

        public CreateArrearsAgreementUseCase(IArrearsAgreementGateway arrearsAgreementGateway)
        {
            _arrearsAgreementGateway = arrearsAgreementGateway;
        }

        public async Task<IExecuteWrapper<CreateArrearsAgreementResponse>> ExecuteAsync(CreateArrearsAgreementRequest request, CancellationToken cancellationToken)
        {
            //validate
            if(request == null)
                return new ExecuteWrapper<CreateArrearsAgreementResponse>(new RequestValidationResponse(false, ""));

            var validationResponse = request.Validate(request);
            if(!validationResponse.IsValid)
                return new ExecuteWrapper<CreateArrearsAgreementResponse>(validationResponse);

            //execute business logic
            var webServiceRequest = new ArrearsAgreementRequest
            {
                Agreement = request?.AgreementInfo,
                PaymentSchedule = request?.PaymentSchedule?.ToArray()
            };
            var response = await _arrearsAgreementGateway.CreateArrearsAgreementAsync(webServiceRequest,cancellationToken).ConfigureAwait(false);
            //marshall unsuccessful WCF Service response
            if (!response.IsSuccess)
                return new ExecuteWrapper<CreateArrearsAgreementResponse>(response.Error);
            //marshall successful WCF Service response
            var useCaseResponse = new CreateArrearsAgreementResponse
            {
                Agreement = response?.Result?.Agreement
            };

            return new ExecuteWrapper<CreateArrearsAgreementResponse>(useCaseResponse);
        }
    }
}
