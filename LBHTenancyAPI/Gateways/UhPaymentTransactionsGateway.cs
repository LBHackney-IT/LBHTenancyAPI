using System.Collections.Concurrent;
using LBHTenancyAPI.Domain;

namespace LBHTenancyAPI.Gateways
{
    public class UhPaymentTransactionsGateway : IUhPaymentTransactionsGateway
    {
        public readonly ConcurrentDictionary<string, PaymentTransactionDescription> _transactions;

        public UhPaymentTransactionsGateway()
        {
            _transactions = new ConcurrentDictionary<string, PaymentTransactionDescription>();
            //test codes
            _transactions.TryAdd("RTrans", new PaymentTransactionDescription {Code = "RTrans", Description = "Direct Debit"});
            _transactions.TryAdd("DTrans", new PaymentTransactionDescription { Code = "DTrans", Description = "Online Payment" });
        }

        public string GetTransactionDescription(string transactionType)
        {
            var unknownTransactionType = "Unknown transaction type";
            if (string.IsNullOrEmpty(transactionType))
                return unknownTransactionType;
            _transactions.TryGetValue(transactionType, out var transactionDescription);
            return transactionDescription != null ? transactionDescription.Description: unknownTransactionType;
        }
    }
}
