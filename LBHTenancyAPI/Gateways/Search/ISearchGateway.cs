using System.Threading;
using System.Threading.Tasks;
using LBH.Data.Domain;
using LBHTenancyAPI.UseCases.Search.Models;

namespace LBHTenancyAPI.Gateways.Search
{
    public interface ISearchGateway
    {
        Task<PagedResults<TenancyListItem>> SearchTenanciesAsync(SearchTenancyRequest request, CancellationToken cancellationToken);
    }
}
