using System.Threading.Tasks;
using AgreementService;

namespace LBHTenancyAPI.UseCases.V1.ArrearsActions
{
    public interface ICreateArrearsActionDiaryUseCase
    {
        Task<ArrearsActionResponse> ExecuteAsync(ArrearsActionCreateRequest request);
    }
}
