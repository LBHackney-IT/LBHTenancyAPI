using System.Collections.Generic;
using Newtonsoft.Json;

namespace LBHTenancyAPI.UseCases.Search.Models
{
    public class SearchTenancyResponse
    {
        [JsonProperty("tenancies")]
        public List<TenancySummary> Tenancies { get; set; }
    }

    public class TenancySummary
    {
        [JsonProperty("ref")]
        public string TenancyRef { get; set; }
        [JsonProperty("prop_ref")]
        public string PropertyRef { get; set; }
        [JsonProperty("tenure")]
        public string Tenure { get; set; }
        [JsonProperty("current_balance")]
        public string CurrentBalance { get; set; }
        [JsonProperty("current_arrears_agreement_status")]
        public string ArrearsAgreementStatus { get; set; }
        [JsonProperty("latest_action")]
        public LatestTenancyAction LatestTenancyAction { get; set; }
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

    public class LatestTenancyAction
    {
        [JsonProperty("code")]
        public string LastActionCode { get; set; }
        [JsonProperty("date")]
        public string LastActionDate { get; set; }
    }
}
