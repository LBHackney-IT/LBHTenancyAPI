using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using AgreementService;
using LBHTenancyAPI.Gateways.V2.Arrears;
using LBHTenancyAPI.Infrastructure.V1.Exceptions;
using LBHTenancyAPI.UseCases.V2.ArrearsAgreements.Models;

namespace LBHTenancyAPI.UseCases.V2.ArrearsAgreements
{
    public class CreateArrearsAgreementUseCase : ICreateArrearsAgreementUseCase
    {
        private readonly IArrearsAgreementGateway _arrearsAgreementGateway;

        public CreateArrearsAgreementUseCase(IArrearsAgreementGateway arrearsAgreementGateway)
        {
            _arrearsAgreementGateway = arrearsAgreementGateway;
        }

        public async Task<CreateArrearsAgreementResponse> ExecuteAsync(CreateArrearsAgreementRequest request, CancellationToken cancellationToken)
        {
            //validate
            if(request == null)
                throw new BadRequestException();

            var validationResponse = request.Validate(request);
            if(!validationResponse.IsValid)
                throw new BadRequestException(validationResponse);

            //execute business logic
            var webServiceRequest = new ArrearsAgreementRequest
            {
                Agreement = request?.AgreementInfo,
                PaymentSchedule = request?.PaymentSchedule?.ToArray()
            };
            var response = await _arrearsAgreementGateway.CreateArrearsAgreementAsync(webServiceRequest,cancellationToken).ConfigureAwait(false);
            
            var useCaseResponse = new CreateArrearsAgreementResponse
            {
                Agreement = response?.Agreement
            };

            return useCaseResponse;
        }
    }
}
