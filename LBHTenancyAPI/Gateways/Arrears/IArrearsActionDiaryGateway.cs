using System.Threading.Tasks;
using AgreementService;

namespace LBHTenancyAPI.Gateways.Arrears
{
    public interface IArrearsActionDiaryGateway
    {
        Task<ArrearsActionResponse> CreateActionDiaryEntryAsync(ArrearsActionCreateRequest request);
    }
}
