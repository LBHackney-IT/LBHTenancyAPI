using System.Collections.Generic;
using System.Threading.Tasks;
using LBHTenancyAPI.UseCases;
using Microsoft.AspNetCore.Mvc;

namespace LBHTenancyAPI.Controllers
{
    [Produces("application/json")]
    [Route("api/v1/tenancy")]
    public class TenancyController : Controller
    {
        private readonly IListAllPayments listAllPayments;

        public TenancyController(IListAllPayments listAllPayments)
        {
            this.listAllPayments = listAllPayments;
        }

        [Route("/payments")]
        [HttpGet]
        public async Task<IActionResult> PaymentTransactionDetails([FromQuery(Name = "tenancy_ref")] string tenancyRef)
        {
            var response = listAllPayments.Execute(tenancyRef);
            var paymentsTransaction = response.PaymentTransactions.ConvertAll(paymentTrans => new Dictionary<string, object>
            {
                {"transaction_ref", paymentTrans.TransactionRef},
                {"transaction_amount", paymentTrans.TransactionAmount},
                {"transaction_date", paymentTrans.TransactionDate},
                {"transaction_type", paymentTrans.TransactionType},
                {"tenancy_ref", paymentTrans.TenancyRef},
                {"property_ref", paymentTrans.PropertyRef}
            });

            var result = new Dictionary<string, object>
            {
                {"payment_transactions", paymentsTransaction}
            };

            return Ok(result);
        }
    }
}
