using System.Threading.Tasks;
using AgreementService;

namespace LBHTenancyAPI.UseCases
{
    public interface ICreateArrearsActionDiaryUseCase
    {
        Task<ArrearsActionResponse> CreateActionDiaryRecordsAsync(ArrearsActionCreateRequest request);
    }
}
