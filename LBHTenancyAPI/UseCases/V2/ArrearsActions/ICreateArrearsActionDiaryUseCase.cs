using System.Threading.Tasks;
using AgreementService;
using LBHTenancyAPI.UseCases.V2.ArrearsActions.Models;

namespace LBHTenancyAPI.UseCases.V2.ArrearsActions
{
    public interface ICreateArrearsActionDiaryUseCase
    {
        Task<ArrearsActionResponse> ExecuteAsync(ActionDiaryRequest request);
    }
}
