using System;
using Newtonsoft.Json;

namespace LBH.Data.Domain
{
    public struct ArrearsAgreement
    {
        private decimal _amount;
        public Decimal Amount
        {
            get => _amount;
            set => _amount = decimal.Round(value, 2, MidpointRounding.AwayFromZero);
        }

        public bool Breached { get; set; }

        public DateTime ClearBy { get; set; }

        private string _frequency;
        public string Frequency
        {
            get => _frequency;
            set => _frequency = value.Trim();
        }

        private decimal _startBalance;
        public Decimal StartBalance
        {
            get => _startBalance;
            set => _startBalance = decimal.Round(value, 2, MidpointRounding.AwayFromZero);
        }

        public DateTime Startdate { get; set; }

        private string _status;
        public string Status
        {
            get => _status;
            set => _status = value.Trim();
        }

        private string _tenancyRef;
        public string TenancyRef
        {
            get => _tenancyRef;
            set => _tenancyRef = value.Trim();
        }
    }

}
