using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using LBHTenancyAPI.Gateways;
using Newtonsoft.Json;

namespace LBHTenancyAPI.UseCases
{
    public class ListAllPayments : IListAllPayments
    {
        private readonly ITenanciesGateway tenanciesGateway;

        public ListAllPayments(ITenanciesGateway gateway)
        {
            tenanciesGateway = gateway;
        }

        public async Task<PaymentTransactionResponse> ExecuteAsync(string tenancyRef)
        {
            var response = new PaymentTransactionResponse();
            var paymentTransaction = await tenanciesGateway.GetPaymentTransactionsByTenancyRefAsync(tenancyRef).ConfigureAwait(false);

            response.PaymentTransactions = paymentTransaction.ConvertAll(paymentTrans => new PaymentTransaction()
                {
                    Ref= paymentTrans.TransactionRef,
                    Amount= paymentTrans.Amount.ToString("C"),
                    Date = string.Format("{0:u}", paymentTrans.Date),
                    Type = paymentTrans.Type,
                    PropertyRef = paymentTrans.PropertyRef,
                    Description = paymentTrans.Description
                }
            );

            return response;
        }

        public class PaymentTransactionResponse
        {
            [JsonProperty("payment_transactions")]
            public List<PaymentTransaction> PaymentTransactions { get; set; }
        }


        public class PaymentTransaction
        {
            [JsonProperty("ref")]
            public string Ref { get; set; }
            [JsonProperty("amount")]
            public string Amount { get; set; }
            [JsonProperty("date")]
            public string Date { get; set; }
            [JsonProperty("type")]
            public string Type { get; set; }
            [JsonProperty("property_ref")]
            public string PropertyRef{ get; set; }
            [JsonProperty("description")]
            public string Description { get; set; }
        }
    }
}
