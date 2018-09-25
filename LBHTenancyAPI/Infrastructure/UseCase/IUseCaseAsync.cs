using System.Threading;
using System.Threading.Tasks;
using LBHTenancyAPI.Infrastructure.UseCase.Execution;

namespace LBHTenancyAPI.UseCases.ArrearsAgreements
{
    public interface IUseCaseAsync<TRequest, TResponse> where TRequest: IRequest 
    {
        Task<IExecuteWrapper<TResponse>> ExecuteAsync(TRequest request, CancellationToken cancellationToken);
    }
}
