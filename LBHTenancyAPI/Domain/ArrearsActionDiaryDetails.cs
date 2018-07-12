using System;
namespace LBHTenancyAPI.Domain
{
    public class ArrearsActionDiaryDetails
    {
        public string ActionCodeName { get; set; }
        public string ActionCode { get; set; }
        public DateTime ActionDate { get; set; }
        public string ActionComment{ get; set; }
        public decimal ActionBalance { get; set; }
        public string UHUsername { get; set; }
        public string TenancyRef{ get; set; }

    }
}
