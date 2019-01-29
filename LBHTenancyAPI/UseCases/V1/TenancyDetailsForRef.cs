using System.Collections.Generic;
using LBH.Data.Domain;
using LBHTenancyAPI.Gateways.V1;

namespace LBHTenancyAPI.UseCases.V1
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

            if (string.IsNullOrWhiteSpace(tenancyRef))
            {
                response.TenancyDetails = new Tenancy
                {
                    TenancyRef = tenancyResponse.TenancyRef,
                    PropertyRef = tenancyResponse.PropertyRef,
                    PaymentRef = tenancyResponse.PaymentRef,
                    Tenure = tenancyResponse.PropertyRef,
                    CurrentBalance = new Currency(tenancyResponse.CurrentBalance),
                    Rent = tenancyResponse.Rent.ToString("C"),
                    Service = tenancyResponse.Service.ToString("C"),
                    OtherCharge = tenancyResponse.OtherCharge.ToString("C"),
                    PrimaryContactName = tenancyResponse.PrimaryContactName,
                    PrimaryContactLongAddress = tenancyResponse.PrimaryContactLongAddress,
                    PrimaryContactPostcode = tenancyResponse.PrimaryContactPostcode,
                    ArrearsActionDiary = new List<ArrearsActionDiaryEntry>(),
                    ArrearsAgreements = new List<ArrearsAgreement>()
                };
            }
            else
            {
                response.TenancyDetails = new Tenancy
                {
                    TenancyRef = tenancyResponse.TenancyRef,
                    PropertyRef = tenancyResponse.PropertyRef,
                    PaymentRef = tenancyResponse.PaymentRef,
                    Tenure = tenancyResponse.Tenure,
                    CurrentBalance = new Currency(tenancyResponse.CurrentBalance),
                    Rent = tenancyResponse.Rent.ToString("C"),
                    Service = tenancyResponse.Service.ToString("C"),
                    OtherCharge = tenancyResponse.OtherCharge.ToString("C"),
                    PrimaryContactName = tenancyResponse.PrimaryContactName,
                    PrimaryContactLongAddress = tenancyResponse.PrimaryContactLongAddress,
                    PrimaryContactPostcode = tenancyResponse.PrimaryContactPostcode,
                    ArrearsAgreementStatus = tenancyResponse.AgreementStatus,
                    ArrearsActionDiary = tenancyResponse.ArrearsActionDiary.ConvertAll(actionDiary => new ArrearsActionDiaryEntry
                    {
                        Code = actionDiary.Code,
                        Type = actionDiary.Type,
                        Balance = actionDiary.Balance.ToString("C"),
                        Comment = actionDiary.Comment,
                        Date = string.Format("{0:u}", actionDiary.Date),
                        UniversalHousingUsername = actionDiary.UniversalHousingUsername
                    }),

                    ArrearsAgreements = tenancyResponse.ArrearsAgreements.ConvertAll(agreement => new ArrearsAgreement
                    {
                        Amount = agreement.Amount.ToString("C"),
                        Breached = agreement.Breached,
                        ClearBy = string.Format("{0:u}", agreement.ClearBy),
                        Frequency = agreement.Frequency,
                        StartBalance = agreement.StartBalance.ToString("C"),
                        Startdate = string.Format("{0:u}", agreement.Startdate),
                        Status = agreement.Status
                    })
                };
            }
            return response;
        }

        public struct TenancyResponse
        {
            public Tenancy TenancyDetails { get; set; }
        }

        public struct Tenancy
        {
            public string TenancyRef { get; set; }
            public string PropertyRef { get; set; }
            public string PaymentRef { get; set; }
            public string Tenure { get; set; }
            public string Rent { get; set; }
            public string Service { get; set; }
            public string OtherCharge { get; set; }
            public Currency CurrentBalance { get; set; }
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
            public bool Breached { get; set; }
            public string ClearBy { get; set; }
            public string Frequency { get; set; }
            public string StartBalance { get; set; }
            public string Startdate { get; set; }
            public string Status{ get; set; }
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
