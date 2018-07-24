using System;
using System.Collections.Generic;
using LBHTenancyAPI.Gateways;

namespace LBHTenancyAPI.UseCases
{
    public class AllPaymentsForTenancy : IListAllPayments
    {
        private readonly ITenanciesGateway tenanciesGateway;

        public AllPaymentsForTenancy(ITenanciesGateway gateway)
        {
            tenanciesGateway = gateway;
        }

        public PaymentTransactionResponse Execute(string tenancyRef)
        {
            var response = new PaymentTransactionResponse();
            var paymentTransaction = tenanciesGateway.GetPaymentTransactionsByTenancyRef(tenancyRef);

            response.PaymentTransactions = paymentTransaction.ConvertAll(paymentTrans => new PaymentTransaction()
                {
                    TransactionRef= paymentTrans.TransactionsRef,
                    TenancyRef = paymentTrans.TenancyRef,
                    TransactionAmount= paymentTrans.TransactionAmount.ToString("C"),
                    TransactionDate = string.Format("{0:u}", paymentTrans.TransactionDate),
                    TransactionType = paymentTrans.TransactionType,
                    PropertyRef = paymentTrans.PropertyRef
                }
            );

            return response;
        }

        public struct PaymentTransactionResponse
        {
            public List<PaymentTransaction> PaymentTransactions { get; set; }
        }

        public struct PaymentTransaction
        {
            public string TransactionRef { get; set; }
            public string TransactionDate { get; set; }
            public string PropertyRef{ get; set; }
            public string TenancyRef { get; set; }
            public string TransactionType { get; set; }
            public string TransactionAmount { get; set; }
        }
    }
}
