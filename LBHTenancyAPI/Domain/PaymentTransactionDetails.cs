using System;
namespace LBHTenancyAPI.Domain
{
    public struct PaymentTransactionDetails
    {
        public string TransactionRef { get; set; }
        public string PropertyRef { get; set; }
        public string TenancyRef { get; set; }
        public string TransactionType{ get; set; }
        public DateTime TransactionDate { get; set; }
        public decimal TransactionAmount{ get; set; }
    }
}
