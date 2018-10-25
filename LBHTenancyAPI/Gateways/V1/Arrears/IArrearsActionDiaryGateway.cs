using System.Threading.Tasks;
using AgreementService;

namespace LBHTenancyAPI.Gateways.V1.Arrears
{
    public interface IArrearsActionDiaryGateway
    {
        Task<ArrearsActionResponse> CreateActionDiaryEntryAsync(ArrearsActionCreateRequest request);
    }
}
