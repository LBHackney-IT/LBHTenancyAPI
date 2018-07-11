using System;
using System.Linq;

namespace LBHTenancyAPI.Domain
{
    public struct TenancyListItem
    {
        public string TenancyRef { get; set; }
        public string LastActionCode { get; set; }
        public DateTime LastActionDate { get; set; }

        private decimal currentBalance;

        public Decimal CurrentBalance
        {
            get => currentBalance;

            set => currentBalance = decimal.Round(value, 2, MidpointRounding.AwayFromZero);
        }

        public string ArrearsAgreementStatus { get; set; }
        public DateTime ArrearsAgreementStartDate { get; set; }
        public string PrimaryContactName { get; set; }

        private string primaryContactShortAddress;

        public string PrimaryContactShortAddress
        {
            get => primaryContactShortAddress;

            set => primaryContactShortAddress = string.IsNullOrWhiteSpace(value) ? null : value.Split("\n").First();
        }

        public string PrimaryContactPostcode { get; set; }
    }
}
