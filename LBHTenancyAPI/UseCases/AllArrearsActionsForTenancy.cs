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
                    Code = actionDiary.ActionCode,
                    CodeName = actionDiary.ActionCodeName,
                    Balance = actionDiary.ActionBalance.ToString("C"),
                    Comment = actionDiary.ActionComment,
                    Date = string.Format("{0:u}", actionDiary.ActionDate),
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
            public string Balance { get; set; }
            public string CodeName { get; set; }
            public string Code { get; set; }
            public string Comment{ get; set; }
            public string Date { get; set; }
            public string UniversalHousingUsername { get; set; }
        }
    }
}
