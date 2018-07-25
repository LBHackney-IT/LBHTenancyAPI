using System;
namespace LBHTenancyAPI.Domain
{
    public struct PaymentTransaction
    {
        public string TransactionRef { get; set; }
        public string PropertyRef { get; set; }
        public string TenancyRef { get; set; }
        public string Type{ get; set; }
        public DateTime Date { get; set; }
        public decimal Amount{ get; set; }
    }
}
