using System.Threading;
using System.Threading.Tasks;

namespace LBHTenancyAPI.Infrastructure.V2.UseCase
{
    public interface IResponseUseCase<TResponse>
    {
        Task<TResponse> ExecuteAsync(CancellationToken cancellationToken);
    }
}
