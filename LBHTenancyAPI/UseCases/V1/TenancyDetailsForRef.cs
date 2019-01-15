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

            if (string.IsNullOrWhiteSpace(tenancyRef))
            {
                response.TenancyDetails = null;
                return response;
            }

            var tenancyResponse = tenanciesGateway.GetTenancyForRef(tenancyRef);


            response.TenancyDetails = tenancyResponse;
            // Date = string.Format("{0:u}", actionDiary.Date),

            return response;
        }

        public struct TenancyResponse
        {
            public Tenancy? TenancyDetails { get; set; }
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
