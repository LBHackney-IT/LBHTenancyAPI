using System.Threading;
using System.Threading.Tasks;
using LBHTenancyAPI.Infrastructure.V2.API;
using LBHTenancyAPI.Infrastructure.V2.UseCase.Execution;

namespace LBHTenancyAPI.Infrastructure.V2.UseCase
{
    public interface IUseCaseAsync<TRequest, TResponse> where TRequest: IRequest 
    {
        Task<IExecuteWrapper<TResponse>> ExecuteAsync(TRequest request, CancellationToken cancellationToken);
    }
}
