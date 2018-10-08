using LBHTenancyAPI.Infrastructure.UseCase;
using LBHTenancyAPI.UseCases.Contacts.Models;
using LBHTenancyAPI.UseCases.Search.Models;

namespace LBHTenancyAPI.UseCases.Search
{
    public interface ISearchTenancyUseCase : IRawUseCaseAsync<SearchTenancyRequest, SearchTenancyResponse>
    {

    }
}
