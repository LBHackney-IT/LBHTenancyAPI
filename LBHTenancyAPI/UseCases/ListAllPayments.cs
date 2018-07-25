using System;
using System.Collections.Generic;
using LBHTenancyAPI.Gateways;

namespace LBHTenancyAPI.UseCases
{
    public class ListAllPayments : IListAllPayments
    {
        private readonly ITenanciesGateway tenanciesGateway;

        public ListAllPayments(ITenanciesGateway gateway)
        {
            tenanciesGateway = gateway;
        }

        public PaymentTransactionResponse Execute(string tenancyRef)
        {
            var response = new PaymentTransactionResponse();
            var paymentTransaction = tenanciesGateway.GetPaymentTransactionsByTenancyRef(tenancyRef);

            response.PaymentTransactions = paymentTransaction.ConvertAll(paymentTrans => new PaymentTransaction()
                {
                    Ref= paymentTrans.TransactionRef,
                    Amount= paymentTrans.Amount.ToString("C"),
                    Date = string.Format("{0:u}", paymentTrans.Date),
                    Type = paymentTrans.Type,
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
            public string Ref { get; set; }
            public string Date { get; set; }
            public string PropertyRef{ get; set; }
            public string Type { get; set; }
            public string Amount { get; set; }
        }
    }
}
