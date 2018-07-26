using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using LBHTenancyAPI.Gateways;
using LBHTenancyAPI.UseCases;
using Microsoft.AspNetCore.Mvc;

namespace LBHTenancyAPI.Controllers
{
    [Produces("application/json")]
    [Route("api/v1/tenancies")]
    public class TenanciesController : Controller
    {
        private readonly IListTenancies listTenancies;
        private readonly IListAllPayments listAllPayments;
        private readonly IListAllArrearsActions listAllArrearsActions;

        public TenanciesController(IListTenancies listTenancies, IListAllArrearsActions listAllArrearsActions, IListAllPayments listAllPayments)
        {
            this.listTenancies = listTenancies;
            this.listAllArrearsActions = listAllArrearsActions;
            this.listAllPayments = listAllPayments;
        }

        [HttpGet]
        public async Task<IActionResult> Get([FromQuery(Name = "tenancy_refs")] List<string> tenancyRefs)
        {
            var response = listTenancies.Execute(tenancyRefs);
            var tenancies = response.Tenancies.ConvertAll(tenancy => new Dictionary<string, object>
            {
                {"ref", tenancy.TenancyRef},
                {"current_balance", tenancy.CurrentBalance},
                {"current_arrears_agreement_status", tenancy.ArrearsAgreementStatus},
                {"latest_action", new Dictionary<string, string>
                {
                    {"code", tenancy.LastActionCode},
                    {"date", tenancy.LastActionDate}
                }},
                {"primary_contact", new Dictionary<string, string>
                {
                    {"name", tenancy.PrimaryContactName},
                    {"short_address", tenancy.PrimaryContactShortAddress},
                    {"postcode", tenancy.PrimaryContactPostcode}
                }}
            });

            var result = new Dictionary<string, object>
            {
                {"tenancies", tenancies}
            };

            return Ok(result);
        }



        [HttpGet]
        [Route("{tenancyRef}/payments")]
        public async Task<IActionResult> PaymentTransactionDetails(string tenancyRef)
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

        [HttpGet]
        [Route("{tenancyRef}/actions")]
        public async Task<IActionResult> GetActionDiaryDetails(string tenancyRef)
        {
            var response = listAllArrearsActions.Execute(tenancyRef);
            var arrearActionDiary = response.ActionDiaryEntries.ConvertAll(actionDiary => new Dictionary<string, object>
            {
                {"balance", actionDiary.Balance},
                {"code", actionDiary.Code},
                {"code_name", actionDiary.CodeName},
                {"date", actionDiary.Date.ToString()},
                {"comment", actionDiary.Comment},
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
