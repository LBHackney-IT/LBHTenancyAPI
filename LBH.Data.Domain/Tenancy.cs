using System;
using System.Collections.Generic;

namespace LBH.Data.Domain
{
    public struct Tenancy
    {
        public List<ArrearsAgreement> ArrearsAgreements { get; set; }
        public List<ArrearsActionDiaryEntry> ArrearsActionDiary { get; set; }

        private decimal currentBalance;
        public Decimal CurrentBalance
        {
            get => currentBalance;

            set => currentBalance = decimal.Round(value, 2, MidpointRounding.AwayFromZero);
        }

        private decimal rent;
        public Decimal Rent
        {
            get => rent;

            set => rent = decimal.Round(value, 2, MidpointRounding.AwayFromZero);
        }

        private decimal service;
        public Decimal Service
        {
            get => service;

            set => service = decimal.Round(value, 2, MidpointRounding.AwayFromZero);
        }

        private decimal otherCharge;
        public Decimal OtherCharge
        {
            get => otherCharge;

            set => otherCharge = decimal.Round(value, 2, MidpointRounding.AwayFromZero);
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

        private string agreementStatus;
        public string AgreementStatus
        {
            get => agreementStatus;

            set => agreementStatus = value.Trim();
        }

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

        private string paymentRef;
        public string PaymentRef
        {
            get => paymentRef;

            set => paymentRef = value.Trim();
        }

        public DateTime? StartDate { get; set; }

        public int? NumberOfBedrooms { get; set; }
    }
}
