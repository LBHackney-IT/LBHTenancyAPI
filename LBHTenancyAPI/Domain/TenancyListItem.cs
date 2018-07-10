using System;
using System.Linq;

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

        private string primaryContactShortAddress;
        public string PrimaryContactShortAddress
        {
            get => primaryContactShortAddress;

            set => primaryContactShortAddress = !string.IsNullOrEmpty(value) ? value.Split("\n").First() : null;
        }

        public string PrimaryContactPostcode { get; set; }
    }
}
