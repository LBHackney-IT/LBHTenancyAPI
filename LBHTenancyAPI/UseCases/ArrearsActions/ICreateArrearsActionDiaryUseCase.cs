using System.Threading.Tasks;
using AgreementService;

namespace LBHTenancyAPI.UseCases.ArrearsActions
{
    public interface ICreateArrearsActionDiaryUseCase
    {
        Task<ArrearsActionResponse> ExecuteAsync(ArrearsActionCreateRequest request);
    }
}
