using System;
using System.Linq;

namespace LBHTenancyAPI.Domain
{
    public struct TenancyListItem
    {
        public DateTime ArrearsAgreementStartDate { get; set; }

        private string arrearsAgreementStatus;
        public string ArrearsAgreementStatus
        {
            get => arrearsAgreementStatus;

            set => arrearsAgreementStatus = value.Trim();
        }

        private decimal currentBalance;
        public Decimal CurrentBalance
        {
            get => currentBalance;

            set => currentBalance = decimal.Round(value, 2, MidpointRounding.AwayFromZero);
        }

        private string tenancyRef;
        public string TenancyRef
        {
            get => tenancyRef;

            set => tenancyRef = value.Trim();
        }

        private string propertyRef;
        public string PropertyRef
        {
            get => propertyRef;

            set => propertyRef = value.Trim();
        }

        private string tenure;
        public string Tenure
        {
            get => tenure;

            set => tenure = value.Trim();
        }


        private string lastActionCode;
        public string LastActionCode
        {
            get => lastActionCode;

            set => lastActionCode = value.Trim();
        }

        public DateTime LastActionDate { get; set; }

        public string PrimaryContactName { get; set; }

        private string primaryContactPostcode;
        public string PrimaryContactPostcode
        {
            get => primaryContactPostcode;

            set => primaryContactPostcode = value.Trim();
        }

        private string primaryContactShortAddress;
        public string PrimaryContactShortAddress
        {
            get => primaryContactShortAddress;

            set => primaryContactShortAddress = string.IsNullOrWhiteSpace(value) ? null : value.Split("\n").First().Trim();
        }
    }
}
