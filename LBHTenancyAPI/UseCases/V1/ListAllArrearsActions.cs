using System.Collections.Generic;
using LBHTenancyAPI.Gateways;

namespace LBHTenancyAPI.UseCases.V1
{
    public class ListAllArrearsActions : IListAllArrearsActions
    {
        private ITenanciesGateway tenanciesGateway;

        public ListAllArrearsActions(ITenanciesGateway gateway)
        {
            tenanciesGateway = gateway;
        }

        public ArrearsActionDiaryResponse Execute(string tenancyRef)
        {
            var response = new ArrearsActionDiaryResponse();
            var actionDiaryResponse = tenanciesGateway.GetActionDiaryEntriesbyTenancyRef(tenancyRef);

            response.ActionDiaryEntries = actionDiaryResponse.ConvertAll(actionDiary => new ArrearsActionDiaryEntry()
                {
                    Code = actionDiary.Code,
                    Type = actionDiary.Type,
                    Balance = actionDiary.Balance.ToString("C"),
                    Comment = actionDiary.Comment,
                    Date = string.Format("{0:u}", actionDiary.Date),
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
            public string Type { get; set; }
            public string Code { get; set; }
            public string Comment{ get; set; }
            public string Date { get; set; }
            public string UniversalHousingUsername { get; set; }
        }
    }
}
