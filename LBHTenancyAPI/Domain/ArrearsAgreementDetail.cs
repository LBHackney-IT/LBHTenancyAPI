using System;

namespace LBHTenancyAPI.Domain
{
    public class ArrearsAgreementDetail
    {
        private decimal amount;
        public Decimal Amount
        {
            get => amount;

            set => amount = decimal.Round(value, 2, MidpointRounding.AwayFromZero);
        }

        public bool Breached { get; set; }

        public DateTime ClearBy { get; set; }

        private string frequency;
        public string Frequency
        {
            get => frequency;

            set => frequency = value.Trim();
        }

        private decimal startBalance;
        public Decimal StartBalance
        {
            get => startBalance;

            set => startBalance = decimal.Round(value, 2, MidpointRounding.AwayFromZero);
        }

        public DateTime Startdate { get; set; }

        private string status;
        public string Status
        {
            get => status;

            set => status = value.Trim();
        }

        private string tenancyRef;
        public string TenancyRef
        {
            get => tenancyRef;

            set => tenancyRef = value.Trim();
        }
    }

}
