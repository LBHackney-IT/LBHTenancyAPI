using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using LBHTenancyAPI.Domain;
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
        private readonly IListAllTenancyDetails listTenancyDetails;

        public TenanciesController(IListTenancies listTenancies, IListAllArrearsActions listAllArrearsActions,
                                   IListAllPayments listAllPayments,IListAllTenancyDetails listTenancyDetails)
        {
            this.listTenancies = listTenancies;
            this.listAllArrearsActions = listAllArrearsActions;
            this.listAllPayments = listAllPayments;
            this.listTenancyDetails = listTenancyDetails;
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


        [HttpGet]
        [Route("{tenancyRef}/tenancyDetails")]
        public async Task<IActionResult> GetTenancyDetails(string tenancyRef)
        {
            var response = listTenancyDetails.Execute(tenancyRef);
            var tenancy = response.TenancyDetails;
            var latestActionDiary = new List<Dictionary<string,object>>();
            var latestAgreement = new List<Dictionary<string,object>>();

            var tenancies = new Dictionary<string, object>
            {
                {"action_code", tenancy.LastActionCode},
                {"agreement_status", tenancy.ArrearsAgreementStatus},
                {"last_action_date", tenancy.LastActionDate},
                {"primary_contact_name", tenancy.PrimaryContactName},
                {"primary_contact_short_address", tenancy.PrimaryContactShortAddress},
                {"primary_contact_postcode", tenancy.PrimaryContactPostcode},
            };

            if (tenancy.ArrearsActionDiary == null)
            {
                latestActionDiary = new List<Dictionary<string, object>>();
                latestActionDiary = null;
            }
            else
            {
                latestActionDiary=tenancy.ArrearsActionDiary.ConvertAll(actionDiary => new Dictionary<string, object>
                {
                    {"balance", actionDiary.Balance},
                    {"code", actionDiary.Code},
                    {"code_name", actionDiary.CodeName},
                    {"date", actionDiary.Date.ToString()},
                    {"comment", actionDiary.Comment},
                    {"universal_housing_username", actionDiary.UniversalHousingUsername}
                });

            }
            if (tenancy.ArrearsAgreements == null)
            {
                latestAgreement = new List<Dictionary<string, object>>();
                latestAgreement = null;
            }
            else
            {

                latestAgreement = tenancy.ArrearsAgreements.ConvertAll(agreement => new Dictionary<string, object>
                {
                    {"amount", agreement.Amount},
                    {"breached", agreement.Breached},
                    {"clear_by", agreement.ClearBy},
                    {"frequency", agreement.Frequency},
                    {"start_balance", agreement.StartBalance},
                    {"start_date", agreement.Startdate},
                    {"status", agreement.Status}
                });
            }

            var result = new Dictionary<string, object>
            {
                {"tenancy_details",tenancies},
                {"latest_action_diary",latestActionDiary},
                {"latest_arrears_agreements",latestAgreement},
            };

            return Ok(result);
        }
    }
}
