using LBHTenancyAPI.Infrastructure.V1.UseCase;
using LBHTenancyAPI.UseCases.V2.Search.Models;

namespace LBHTenancyAPI.UseCases.V2.Search
{
    public interface ISearchTenancyUseCase : IRawUseCaseAsync<SearchTenancyRequest, SearchTenancyResponse>
    {

    }
}
