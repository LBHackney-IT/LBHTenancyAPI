using System.Threading;
using System.Threading.Tasks;
using AgreementService;
using LBHTenancyAPI.Infrastructure.V1.UseCase.Execution;

namespace LBHTenancyAPI.Gateways.V1.Arrears
{
    public interface IArrearsAgreementGateway
    {
        Task<IExecuteWrapper<ArrearsAgreementResponse>> CreateArrearsAgreementAsync(ArrearsAgreementRequest request, CancellationToken cancellationToken);
    }
}
