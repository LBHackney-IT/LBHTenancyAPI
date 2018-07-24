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
        private readonly IListAllArrearsActions listAllArrearsActions;

        public TenancyController(IListAllPayments listAllPayments)
        {
            this.listAllPayments = listAllPayments;
        }

        public TenancyController(IListAllArrearsActions listAllArrearsActions)
        {
            this.listAllArrearsActions = listAllArrearsActions;
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

        [Route("/actions")]
        [HttpGet]
        public async Task<IActionResult> GetActionDiaryDetails([FromQuery(Name = "tenancy_ref")] string tenancyRef)
        {
            var response = listAllArrearsActions.Execute(tenancyRef);
            var arrearActionDiary = response.ActionDiaryEntries.ConvertAll(actionDiary => new Dictionary<string, object>
            {
                {"action_balance", actionDiary.ActionBalance},
                {"action_code", actionDiary.ActionCode},
                {"action_code_name", actionDiary.ActionCodeName},
                {"action_date", actionDiary.ActionDate.ToString()},
                {"action_comment", actionDiary.ActionComment},
                {"universal_housing_username", actionDiary.UniversalHousingUsername},
                {"tenancy_ref", actionDiary.TenancyRef}
            });

            var result = new Dictionary<string, object>
            {
                {"arrears_action_diary", arrearActionDiary}
            };

            return Ok(result);
        }
    }
}
