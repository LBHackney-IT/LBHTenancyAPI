using System.Threading.Tasks;
using AgreementService;

namespace LBHTenancyAPI.UseCases.ArrearsActions
{
    public interface ICreateArrearsActionDiaryUseCase
    {
        Task<ArrearsActionResponse> CreateActionDiaryRecordsAsync(ArrearsActionCreateRequest request);
    }
}
