using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using LBHTenancyAPI.Gateways;
using Newtonsoft.Json;

namespace LBHTenancyAPI.UseCases
{
    public class ListTenancies : IListTenancies
    {
        private readonly ITenanciesGateway tenanciesGateway;

        public ListTenancies(ITenanciesGateway tenanciesGateway)
        {
            this.tenanciesGateway = tenanciesGateway;
        }

        public async Task<Response> ExecuteAsync(List<string> tenancyRefs)
        {
            var tenancies = await tenanciesGateway.GetTenanciesByRefsAsync(tenancyRefs);
            var response = new Response
            {
                Tenancies = tenancies.ConvertAll(tenancy => new ResponseTenancy
                    {
                        TenancyRef = tenancy.TenancyRef,
                        PropertyRef = tenancy.PropertyRef,
                        Tenure = tenancy.Tenure,
                        LatestTenancyAction = new LatestTenancyAction
                        {
                            LastActionCode = tenancy.LastActionCode,
                            LastActionDate = String.Format("{0:u}", tenancy.LastActionDate)
                        },
                        CurrentBalance = tenancy.CurrentBalance.ToString("C"),
                        ArrearsAgreementStatus = tenancy.ArrearsAgreementStatus,
                        PrimaryContact = new PrimaryContact
                        {
                            PrimaryContactName = tenancy.PrimaryContactName,
                            PrimaryContactShortAddress = tenancy.PrimaryContactShortAddress,
                            PrimaryContactPostcode = tenancy.PrimaryContactPostcode
                        }
                    }
                )
            };

            return response;
        }

        public class Response
        {
            [JsonProperty("tenancies")]
            public List<ResponseTenancy> Tenancies { get; set; }
        }

        public class ResponseTenancy
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
            public string PrimaryContactName { get; set; }
            [JsonProperty("short_address")]
            public string PrimaryContactShortAddress { get; set; }
            [JsonProperty("postcode")]
            public string PrimaryContactPostcode { get; set; }
        }

        public class LatestTenancyAction
        {
            [JsonProperty("code")]
            public string LastActionCode { get; set; }
            [JsonProperty("date")]
            public string LastActionDate { get; set; }
        }
    }
}
