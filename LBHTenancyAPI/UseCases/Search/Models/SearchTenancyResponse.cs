using LBH.Data.Domain;
using System.Collections.Generic;

namespace LBHTenancyAPI.UseCases.Contacts.Models
{
    public class SearchTenancyResponse
    {
        public IList<TenancyListItem> SearchResults { get; set; }
    }
}
