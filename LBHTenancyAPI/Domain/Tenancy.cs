using System;
using System.Collections.Generic;
using System.Linq;

namespace LBHTenancyAPI.Domain
{
    public struct Tenancy
    {
        public List<ArrearsAgreementDetail> ArrearsAgreements { get; set; }
        public List<ArrearsActionDiaryDetails> ArrearsActionDiary { get; set; }

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

        private string lastActionCode;
        public string LastActionCode
        {
            get => lastActionCode;

            set => lastActionCode = value.Trim();
        }

        public DateTime LastActionDate { get; set; }

        public string PrimaryContactName { get; set; }

        private string primaryContactPhone;
        public string PrimaryContactPhone
        {
            get => primaryContactPhone;

            set => primaryContactPhone = value.Trim();
        }

        private string primaryContactPostcode;
        public string PrimaryContactPostcode
        {
            get => primaryContactPostcode;

            set => primaryContactPostcode = value.Trim();
        }

        private string primaryContactLongAddress;
        public string PrimaryContactLongAddress
        {
            get => primaryContactLongAddress;

            set => primaryContactLongAddress = string.IsNullOrWhiteSpace(value) ? null : value.Trim();
        }
    }
}
