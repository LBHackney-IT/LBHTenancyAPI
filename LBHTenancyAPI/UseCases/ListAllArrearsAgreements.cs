using System.Collections.Generic;
using LBHTenancyAPI.Gateways;

namespace LBHTenancyAPI.UseCases
{
    public class ListAllArrearsAgreements
    {
        public struct ArrearsAgreementResponse
        {
            public List<ArrearsAgreement> ArrearsAgreements{ get; set; }
        }


        private ITenanciesGateway tenanciesGateway;

        public ListAllArrearsAgreements(ITenanciesGateway gateway)
        {
            tenanciesGateway = gateway;
        }

        public ArrearsAgreementResponse Execute(string tenancyRef)
        {
            var response = new ArrearsAgreementResponse();
            var agreementResponse = tenanciesGateway.get(tenancyRef);

            response.ArrearsAgreements = actionDiaryResponse.ConvertAll(actionDiary => new ArrearsAgreements()
                {
                    Code = actionDiary.Code,
                    CodeName = actionDiary.CodeName,
                    Balance = actionDiary.Balance.ToString("C"),
                    Comment = actionDiary.Comment,
                    Date = string.Format("{0:u}", actionDiary.Date),
                    UniversalHousingUsername = actionDiary.UniversalHousingUsername
                }
            );

            return response;
        }

        public struct ArrearsAgreement
        {
            public string Amount{ get; set; }
            public bool Breached { get; set; }
            public string ClearBy { get; set; }
            public string Frequency { get; set; }
            public string StartBalance { get; set; }
            public string Startdate { get; set; }
            public string Status{ get; set; }
        }
    }
}
