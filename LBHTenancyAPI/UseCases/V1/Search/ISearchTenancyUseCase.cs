using LBHTenancyAPI.Infrastructure.UseCase;
using LBHTenancyAPI.UseCases.V1.Search.Models;

namespace LBHTenancyAPI.UseCases.V1.Search
{
    public interface ISearchTenancyUseCase : IRawUseCaseAsync<SearchTenancyRequest, SearchTenancyResponse>
    {

    }
}
