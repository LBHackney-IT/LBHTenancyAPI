using System.Threading;
using System.Threading.Tasks;
using AgreementService;

namespace LBHTenancyAPI.Gateways.V2.Arrears
{
    public interface IArrearsAgreementGateway
    {
        Task<ArrearsAgreementResponse> CreateArrearsAgreementAsync(ArrearsAgreementRequest request, CancellationToken cancellationToken);
    }
}
