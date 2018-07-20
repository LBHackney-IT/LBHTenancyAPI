using System;
using System.Collections.Generic;
using LBHTenancyAPI.Domain;
using LBHTenancyAPI.Gateways;

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

            response.Tenancies = tenancies.ConvertAll(tenancy => new ResponseTenancy()
                {
                    TenancyRef = tenancy.TenancyRef,
                    LastActionCode = tenancy.LastActionCode,
                    LastActionDate = string.Format("{0:u}", tenancy.LastActionDate),
                    CurrentBalance = tenancy.CurrentBalance.ToString(),
                    ArrearsAgreementStatus = tenancy.ArrearsAgreementStatus,
                    PrimaryContactName = tenancy.PrimaryContactName,
                    PrimaryContactShortAddress = tenancy.PrimaryContactShortAddress,
                    PrimaryContactPostcode = tenancy.PrimaryContactPostcode
                }
            );

            return response;
        }

        public ArrearsActionDiaryResponse ExecuteActionDiaryQuery(List<string> tenancyRefs)
        {
            var response = new ArrearsActionDiaryResponse();
            var actionDiary = tenanciesGateway.GetActionDiaryDetailsbyTenancyRefs(tenancyRefs);

            response.ActionDiary = actionDiary.ConvertAll(actiondiary => new ResponseArrearsActionDiary()
                {
                    TenancyRef = actiondiary.TenancyRef,
                    ActionCode = actiondiary.ActionCode,
                    ActionCodeName = actiondiary.ActionCodeName,
                    ActionBalance = actiondiary.ActionBalance,
                    ActionComment = actiondiary.ActionComment,
                    ActionDate = actiondiary.ActionDate,
                    UniversalHousingUsername = actiondiary.UniversalHousingUsername
                }
            );

            return response;
        }

        public struct Response
        {
            public List<ResponseTenancy> Tenancies { get; set; }
        }

        public struct ArrearsActionDiaryResponse
        {
            public List<ResponseArrearsActionDiary> ActionDiary { get; set; }
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

        public struct ResponseArrearsActionDiary
        {
            public decimal ActionBalance { get; set; }
            public string ActionCodeName { get; set; }
            public string ActionCode { get; set; }
            public string ActionComment{ get; set; }
            public DateTime ActionDate { get; set; }
            public string TenancyRef{ get; set; }
            public string UniversalHousingUsername { get; set; }
        }
    }
}
