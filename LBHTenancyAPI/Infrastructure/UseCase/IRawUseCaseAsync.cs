using System.Threading;
using System.Threading.Tasks;
using LBHTenancyAPI.Infrastructure.API;

namespace LBHTenancyAPI.Infrastructure.UseCase
{
    public interface IRawUseCaseAsync<TRequest, TResponse> where TRequest : IRequest
    {
        Task<TResponse> ExecuteAsync(TRequest request, CancellationToken cancellationToken);
    }
}
