using System.Threading;
using System.Threading.Tasks;
using LBHTenancyAPI.Infrastructure.V1.API;
using LBHTenancyAPI.Infrastructure.V1.UseCase.Execution;

namespace LBHTenancyAPI.Infrastructure.V1.UseCase
{
    public interface IUseCaseAsync<TRequest, TResponse> where TRequest: IRequest 
    {
        Task<IExecuteWrapper<TResponse>> ExecuteAsync(TRequest request, CancellationToken cancellationToken);
    }
}
