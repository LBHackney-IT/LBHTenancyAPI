using System.Threading;
using System.Threading.Tasks;
using LBHTenancyAPI.Infrastructure.V2.API;

namespace LBHTenancyAPI.Infrastructure.V2.UseCase
{
    public interface IRawUseCaseAsync<TRequest, TResponse> where TRequest : IRequest
    {
        Task<TResponse> ExecuteAsync(TRequest request, CancellationToken cancellationToken);
    }
}
