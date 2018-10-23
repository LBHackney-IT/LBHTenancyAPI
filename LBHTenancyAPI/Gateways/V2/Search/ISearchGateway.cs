using System.Threading;
using System.Threading.Tasks;
using LBH.Data.Domain;
using LBHTenancyAPI.UseCases.V2.Search.Models;

namespace LBHTenancyAPI.Gateways.V2.Search
{
    public interface ISearchGateway
    {
        Task<PagedResults<TenancyListItem>> SearchTenanciesAsync(SearchTenancyRequest request, CancellationToken cancellationToken);
    }
}
