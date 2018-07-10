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
    }
}
