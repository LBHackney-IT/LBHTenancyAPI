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
            var actionDiaryResponse = tenanciesGateway.GetActionDiaryDetailsbyTenancyRefs(tenancyRef);

            response.ActionDiaryEntries = actionDiaryResponse.ConvertAll(actionDiary => new ArrearsActionDiaryEntry()
                {
                    TenancyRef = actionDiary.TenancyRef,
                    ActionCode = actionDiary.ActionCode,
                    ActionCodeName = actionDiary.ActionCodeName,
                    ActionBalance = actionDiary.ActionBalance.ToString("C"),
                    ActionComment = actionDiary.ActionComment,
                    ActionDate = string.Format("{0:u}", actionDiary.ActionDate),
                    UniversalHousingUsername = actionDiary.UniversalHousingUsername
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
            public string ActionBalance { get; set; }
            public string ActionCodeName { get; set; }
            public string ActionCode { get; set; }
            public string ActionComment{ get; set; }
            public string ActionDate { get; set; }
            public string TenancyRef{ get; set; }
            public string UniversalHousingUsername { get; set; }
        }
    }
}
