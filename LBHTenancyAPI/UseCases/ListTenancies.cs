using System;
using System.Collections.Generic;
using LBHTenancyAPI.Domain;
using LBHTenancyAPI.Gateways;
using Microsoft.AspNetCore.Server.Kestrel.Transport.Libuv.Internal.Networking;

namespace LBHTenancyAPI.UseCases
{
    public class ListTenancies : IListTenancies
    {
        private readonly ITenanciesGateway tenanciesGateway;

        public ListTenancies(ITenanciesGateway tenanciesGateway)
        {
            this.tenanciesGateway = tenanciesGateway;
        }

        public Response Execute(List<string> tenancyRefs)
        {
            var response = new Response();
            var tenancies = tenanciesGateway.GetTenanciesByRefs(tenancyRefs);

            response.Tenancies = tenancies.ConvertAll(tenancy => new ResponseTenancy
                {
                    TenancyRef = tenancy.TenancyRef,
                    LastActionCode = tenancy.LastActionCode,
                    LastActionDate = string.Format("{0:u}", tenancy.LastActionDate),
                    CurrentBalance = tenancy.CurrentBalance.ToString("C"),
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
