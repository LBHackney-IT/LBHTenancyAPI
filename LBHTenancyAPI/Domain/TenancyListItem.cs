using System;

namespace LBHTenancyAPI.Domain
{
    public struct TenancyListItem
    {
        public string TenancyRef { get; set; }
        public string LastActionCode { get; set; }
        public DateTime LastActionDate { get; set; }
        public double CurrentBalance { get; set; }
        public string ArrearsAgreementStatus { get; set; }
        public DateTime ArrearsAgreementStartDate { get; set; }
        public string PrimaryContactName { get; set; }
        public string PrimaryContactShortAddress { get; set; }
        public string PrimaryContactPostcode { get; set; }

        public TenancyListItem mergeWith(TenancyListItem t)
        {
            if (t.TenancyRef != null && TenancyRef == null)
            {
                TenancyRef = t.TenancyRef;
            }

            if (t.LastActionCode != null && LastActionCode == null)
            {
                LastActionCode = t.LastActionCode;
            }

            if (t.LastActionDate != new DateTime() && LastActionDate == new DateTime())
            {
                LastActionDate = t.LastActionDate;
            }

            if (!t.CurrentBalance.Equals(0.0) && CurrentBalance.Equals(0.0))
            {
                CurrentBalance = t.CurrentBalance;
            }

            if (t.ArrearsAgreementStatus != null && ArrearsAgreementStatus == null)
            {
                ArrearsAgreementStatus = t.ArrearsAgreementStatus;
            }

            if (t.ArrearsAgreementStartDate != new DateTime() && ArrearsAgreementStartDate == new DateTime())
            {
                ArrearsAgreementStartDate = t.ArrearsAgreementStartDate;
            }

            if (t.PrimaryContactName != null && PrimaryContactName == null)
            {
                PrimaryContactName = t.PrimaryContactName;
            }

            if (t.PrimaryContactPostcode != null && PrimaryContactPostcode == null)
            {
                PrimaryContactPostcode = t.PrimaryContactPostcode;
            }

            if (t.PrimaryContactShortAddress != null && PrimaryContactShortAddress == null)
            {
                PrimaryContactShortAddress = t.PrimaryContactShortAddress;
            }

            foreach (var prop in t.GetType().GetProperties())
            {

            }

            return this;
        }
    }
}
