using System.Collections.Generic;
using LBHTenancyAPI.Gateways;

namespace LBHTenancyAPI.UseCases
{
    public class TenancyDetailsForRef : ITenancyDetailsForRef
    {
        private readonly ITenanciesGateway tenanciesGateway;

        public TenancyDetailsForRef(ITenanciesGateway gateway)
        {
            tenanciesGateway = gateway;
        }

        public TenancyResponse Execute(string tenancyRef)
        {
            var response = new TenancyResponse();
            var tenancyResponse = tenanciesGateway.GetTenancyForRef(tenancyRef);

                response.TenancyDetails = new Tenancy
                {
                    CurrentBalance = tenancyResponse.CurrentBalance.ToString("C"),
                    PrimaryContactName = tenancyResponse.PrimaryContactName,
                    PrimaryContactLongAddress = tenancyResponse.PrimaryContactLongAddress,
                    PrimaryContactPostcode = tenancyResponse.PrimaryContactPostcode,

                    ArrearsActionDiary = tenancyResponse.ArrearsActionDiary.ConvertAll(actionDiary => new ArrearsActionDiaryEntry
                    {
                        Code = actionDiary.Code,
                        CodeName = actionDiary.CodeName,
                        Balance = actionDiary.Balance.ToString("C"),
                        Comment = actionDiary.Comment,
                        Date = string.Format("{0:u}", actionDiary.Date),
                        UniversalHousingUsername = actionDiary.UniversalHousingUsername
                    }),
                    ArrearsAgreements = tenancyResponse.ArrearsAgreements.ConvertAll(agreement => new ArrearsAgreement
                    {
                        Amount = agreement.Amount.ToString("C"),
                        Breached = agreement.Breached.ToString(),
                        ClearBy = string.Format("{0:u}", agreement.ClearBy),
                        Frequency = agreement.Frequency,
                        StartBalance = agreement.StartBalance.ToString("C"),
                        Startdate = string.Format("{0:u}", agreement.Startdate),
                        Status = agreement.Status
                    })
                };

            return response;
        }

        public struct TenancyResponse
        {
            public Tenancy TenancyDetails { get; set; }
        }

        public struct Tenancy
        {
            public string TenancyRef { get; set; }
            public string CurrentBalance { get; set; }
            public string ArrearsAgreementStatus { get; set; }
            public string PrimaryContactName { get; set; }
            public string PrimaryContactLongAddress { get; set; }
            public string PrimaryContactPostcode { get; set; }
            public List<ArrearsAgreement> ArrearsAgreements { get; set; }
            public List<ArrearsActionDiaryEntry> ArrearsActionDiary { get; set; }
        }

        public struct ArrearsAgreement
        {
            public string Amount{ get; set; }
            public string Breached { get; set; }
            public string ClearBy { get; set; }
            public string Frequency { get; set; }
            public string StartBalance { get; set; }
            public string Startdate { get; set; }
            public string Status{ get; set; }
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
