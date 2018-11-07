using System.Threading.Tasks;
using AgreementService;

namespace LBHTenancyAPI.UseCases.V2.ArrearsActions
{
    public interface ICreateArrearsActionDiaryUseCase
    {
        Task<ArrearsActionResponse> ExecuteAsync(ArrearsActionCreateRequest request);
    }
}
