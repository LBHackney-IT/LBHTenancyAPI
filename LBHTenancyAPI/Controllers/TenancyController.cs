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
                {"ref", paymentTrans.Ref},
                {"amount", paymentTrans.Amount},
                {"date", paymentTrans.Date},
                {"type", paymentTrans.Type},
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
                {"balance", actionDiary.ActionBalance},
                {"code", actionDiary.ActionCode},
                {"code_name", actionDiary.ActionCodeName},
                {"date", actionDiary.ActionDate.ToString()},
                {"comment", actionDiary.ActionComment},
                {"universal_housing_username", actionDiary.UniversalHousingUsername}
            });

            var result = new Dictionary<string, object>
            {
                {"arrears_action_diary_events", arrearActionDiary}
            };

            return Ok(result);
        }
    }
}
