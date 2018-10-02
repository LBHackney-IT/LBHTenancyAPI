using LBH.Data.Domain;
using System.Collections.Generic;
using LBHTenancyAPI.Infrastructure.API;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

namespace LBHTenancyAPI.UseCases.Contacts.Models
{
    public class SearchTenancyResponse
    {
        [JsonProperty("tenancies")]
        public IList<TenancyListItem> Tenancies { get; set; }
    }
}
