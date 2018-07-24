using System;
using System.Collections.Generic;
using LBHTenancyAPI.Gateways;

namespace LBHTenancyAPI.UseCases
{
    public class AllArrearsActionsForTenancy : IListAllArrearsActions
    {
        private ITenanciesGateway tenanciesGateway;

        public AllArrearsActionsForTenancy(ITenanciesGateway gateway)
        {
            tenanciesGateway = gateway;
        }

        public ArrearsActionDiaryResponse Execute(string tenancyRef)
        {
            var response = new ArrearsActionDiaryResponse();
            var actionDiary = tenanciesGateway.GetActionDiaryDetailsbyTenancyRefs(tenancyRef);

            response.ActionDiaryEntries = actionDiary.ConvertAll(actiondiary => new ArrearsActionDiaryEntry()
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

        public struct ArrearsActionDiaryResponse
        {
            public List<ArrearsActionDiaryEntry> ActionDiaryEntries { get; set; }
        }

        public struct ArrearsActionDiaryEntry
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
