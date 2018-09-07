using System.Threading;
using System.Threading.Tasks;
using AgreementService;
using LBHTenancyAPI.UseCases.ArrearsAgreements;

namespace LBHTenancyAPI.Gateways
{
    public interface IArrearsAgreementGateway
    {
        Task<IExecuteWrapper<ArrearsAgreementResponse>> CreateArrearsAgreementAsync(ArrearsAgreementRequest request, CancellationToken cancellationToken);
    }
}
