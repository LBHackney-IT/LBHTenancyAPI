using System;

namespace LBH.Data.Domain
{
    public struct ArrearsActionDiaryEntry
    {
        public int Id { get; set; }
        public decimal Balance { get; set; }
        public string Type { get; set; }
        public string Code { get; set; }
        public string Comment{ get; set; }
        public DateTime Date { get; set; }
        public string TenancyRef{ get; set; }
        public string UniversalHousingUsername { get; set; }
    }
}
