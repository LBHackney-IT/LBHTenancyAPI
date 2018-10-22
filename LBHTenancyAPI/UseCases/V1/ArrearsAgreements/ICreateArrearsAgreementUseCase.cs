using LBHTenancyAPI.Infrastructure.UseCase;
using LBHTenancyAPI.UseCases.V1.ArrearsAgreements.Models;

namespace LBHTenancyAPI.UseCases.V1.ArrearsAgreements
{
    public interface ICreateArrearsAgreementUseCase:IUseCaseAsync<CreateArrearsAgreementRequest, CreateArrearsAgreementResponse>
    {

    }
}
