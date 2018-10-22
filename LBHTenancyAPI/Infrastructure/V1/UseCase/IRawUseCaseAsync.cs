using System.Threading;
using System.Threading.Tasks;
using LBHTenancyAPI.Infrastructure.V1.API;

namespace LBHTenancyAPI.Infrastructure.V1.UseCase
{
    public interface IRawUseCaseAsync<TRequest, TResponse> where TRequest : IRequest
    {
        Task<TResponse> ExecuteAsync(TRequest request, CancellationToken cancellationToken);
    }
}
