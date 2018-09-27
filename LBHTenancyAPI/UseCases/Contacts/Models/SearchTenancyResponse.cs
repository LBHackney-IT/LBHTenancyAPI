using System.Collections.Generic;

namespace LBHTenancyAPI.UseCases.Contacts.Models
{
    public class SearchTenancyResponse
    {
        public IList<TenancySearch> SearchResults { get; set; }
    }
}
