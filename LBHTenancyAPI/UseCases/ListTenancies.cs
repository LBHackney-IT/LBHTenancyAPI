using System;
using System.Collections.Generic;
using LBHTenancyAPI.Gateways;

namespace LBHTenancyAPI.UseCases
{
    public class ListTenancies
    {
        private ITenanciesGateway _tenanciesGateway;

        public ListTenancies(ITenanciesGateway tenanciesGateway)
        {
            this._tenanciesGateway = tenanciesGateway;
        }

        public Response Execute(List<string> tenancyRefs)
        {
            var response = new Response();
            var tenancies = _tenanciesGateway.GetTenanciesByRefs(tenancyRefs);

            response.Tenancies = tenancies.ConvertAll<ResponseTenancy>(tenancy => new ResponseTenancy()
                {
                    TenancyRef = tenancy.TenancyRef,
                    LastActionCode = tenancy.LastActionCode,
                    LastActionDate = String.Format("{0:u}", tenancy.LastActionDate),
                    CurrentBalance = tenancy.CurrentBalance.ToString(),
                    ArrearsAgreementStatus = tenancy.ArrearsAgreementStatus,
                    PrimaryContactName = tenancy.PrimaryContactName,
                    PrimaryContactShortAddress = tenancy.PrimaryContactShortAddress,
                    PrimaryContactPostcode = tenancy.PrimaryContactPostcode
                }
            );

            return response;
        }

        public struct Response
        {
            public List<ResponseTenancy> Tenancies { get; set; }
        }

        public struct ResponseTenancy
        {
            public string TenancyRef { get; set; }
            public string LastActionCode { get; set; }
            public string LastActionDate { get; set; }
            public string CurrentBalance { get; set; }
            public string ArrearsAgreementStatus { get; set; }
            public string PrimaryContactName { get; set; }
            public string PrimaryContactShortAddress { get; set; }
            public string PrimaryContactPostcode { get; set; }
        }
    }
}
