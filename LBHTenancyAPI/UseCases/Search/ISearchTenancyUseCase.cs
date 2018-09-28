using System.Linq;
using LBHTenancyAPI.Infrastructure.UseCase;
using LBHTenancyAPI.UseCases.Contacts.Models;

namespace LBHTenancyAPI.UseCases.Contacts
{
    public interface ISearchTenancyUseCase : IRawUseCaseAsync<SearchTenancyRequest, SearchTenancyResponse>
    {

    }
}
