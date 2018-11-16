using System.Threading.Tasks;
using AgreementService;

namespace LBHTenancyAPI.Gateways.V2.Arrears
{
    public interface IArrearsActionDiaryGateway
    {
        Task<ArrearsActionResponse> CreateActionDiaryEntryAsync(ArrearsActionCreateRequest request);
        Task UpdateRecordingUserName(string requestAppUser, int actionDiaryId);
    }
}
