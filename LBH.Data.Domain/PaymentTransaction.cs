using System;

namespace LBH.Data.Domain
{
    public class PaymentTransaction
    {
        public string TransactionRef { get; set; }
        public string PropertyRef { get; set; }
        public string TenancyRef { get; set; }
        public string Type{ get; set; }
        public DateTime Date { get; set; }
        public decimal Amount{ get; set; }
        public string Description { get; set; }
    }
}
