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

            response.PaymentTransactions = paymentTransaction.ConvertAll(paymentTrans => new ResponsePaymentTransactions()
                {
                    TransactionsRef= paymentTrans.TransactionsRef,
                    TenancyRef = paymentTrans.TenancyRef,
                    TransactionAmount= paymentTrans.TransactionAmount,
                    TransactionDate = paymentTrans.TransactionDate,
                    TransactionType = paymentTrans.TransactionType,
                    PropertyRef = paymentTrans.PropertyRef
                }
            );

            return response;
        }

        public struct PaymentTransactionResponse
        {
            public List<ResponsePaymentTransactions> PaymentTransactions { get; set; }
        }

        public struct ResponsePaymentTransactions
        {
            public string TransactionsRef { get; set; }
            public DateTime TransactionDate { get; set; }
            public string PropertyRef{ get; set; }
            public string TenancyRef { get; set; }
            public string TransactionType { get; set; }
            public decimal TransactionAmount { get; set; }
        }
    }
}
