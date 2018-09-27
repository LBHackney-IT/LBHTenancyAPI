using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using LBH.Data.Domain;
using LBHTenancyAPI.UseCases.Contacts.Models;

namespace LBHTenancyAPITest.Test.Gateways.Search
{
    public interface ISearchGateway
    {
        Task<IList<TenancySummary>> SearchTenanciesAsync(SearchTenancyRequest request, CancellationToken cancellationToken);
    }

    public class SearchGateway : ISearchGateway
    {
        public async Task<IList<TenancySummary>> SearchTenanciesAsync(SearchTenancyRequest request, CancellationToken cancellationToken)
        {

        }
    }
}
