using System.Collections.Generic;
using LBHTenancyAPI.Infrastructure.API;
using Newtonsoft.Json;

namespace LBHTenancyAPI.UseCases.Search.Models
{
    public class SearchTenancyResponse:IPagedResponse
    {
        [JsonProperty("tenancies")]
        public List<SearchSummary> Tenancies { get; set; }

        public int PageCount { get; set; }
        public int TotalCount { get; set; }
    }

    public class SearchSummary
    {
        [JsonProperty("ref")]
        public string TenancyRef { get; set; }
        [JsonProperty("prop_ref")]
        public string PropertyRef { get; set; }
        [JsonProperty("tenure")]
        public string Tenure { get; set; }

        [JsonProperty("current_arrears_agreement_status")]
        public string ArrearsAgreementStatus { get; set; }

        [JsonProperty("current_balance")]
        public Currency CurrentBalance { get; set; }

        [JsonProperty("primary_contact")]
        public PrimaryContact PrimaryContact { get; set; }
    }

    public class PrimaryContact
    {
        [JsonProperty("name")]
        public string Name { get; set; }
        [JsonProperty("short_address")]
        public string ShortAddress { get; set; }
        [JsonProperty("postcode")]
        public string Postcode { get; set; }
    }

    public class Currency
    {
        [JsonProperty("value")]
        public decimal Value { get; set; }
        [JsonProperty("currency_code")]
        public string CurrencyCode { get; set; }

        public Currency(decimal value)
        {
            Value = value;
            CurrencyCode = "GBP";
        }
        public Currency()
        {
            CurrencyCode = "GBP";
        }
    }

    public class LatestTenancyAction
    {
        [JsonProperty("code")]
        public string LastActionCode { get; set; }
        [JsonProperty("date")]
        public string LastActionDate { get; set; }
    }
}
