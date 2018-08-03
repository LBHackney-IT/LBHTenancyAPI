using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using LBHTenancyAPI.Domain;
using LBHTenancyAPI.UseCases;
using Microsoft.AspNetCore.Mvc;

namespace LBHTenancyAPI.Controllers
{
    [Produces("application/json")]
    [Route("api/v1/tenancies")]
    public class TenanciesController : Controller
    {
        private readonly IListAllArrearsActions listAllArrearsActions;
        private readonly IListAllPayments listAllPayments;
        private readonly IListTenancies listTenancies;
        private readonly ITenancyDetailsForRef tenancyDetailsForRef;

        public TenanciesController(IListTenancies listTenancies, IListAllArrearsActions listAllArrearsActions,
            IListAllPayments listAllPayments, ITenancyDetailsForRef tenancyDetailsForRef)
        {
            this.listTenancies = listTenancies;
            this.listAllArrearsActions = listAllArrearsActions;
            this.listAllPayments = listAllPayments;
            this.tenancyDetailsForRef = tenancyDetailsForRef;
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
                {
                    "latest_action", new Dictionary<string, string>
                    {
                        {"code", tenancy.LastActionCode},
                        {"date", tenancy.LastActionDate}
                    }
                },
                {
                    "primary_contact", new Dictionary<string, string>
                    {
                        {"name", tenancy.PrimaryContactName},
                        {"short_address", tenancy.PrimaryContactShortAddress},
                        {"postcode", tenancy.PrimaryContactPostcode}
                    }
                }
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
            var paymentsTransaction = response.PaymentTransactions.ConvertAll(paymentTrans =>
                new Dictionary<string, object>
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
                {"type", actionDiary.Type},
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
        [Route("{tenancyRef}")]
        public async Task<IActionResult> GetTenancyDetails(string tenancyRef)
        {
            Dictionary<string, object> result;
            var tenancyDetails = new Dictionary<string, object>();
            try
            {
                if (string.IsNullOrWhiteSpace(tenancyRef))
                {
                    var errors = new List<APIErrorMessage>
                    {
                        new APIErrorMessage
                        {
                            developerMessage = "Invalid parameter - Tenancy Reference",
                            userMessage = "No tenancy for reference: {tenancyRef}"
                        }
                    };
                    var json = Json(errors);
                    json.StatusCode = 404;
                    return json;
                }

                var response = tenancyDetailsForRef.Execute(tenancyRef);
                var tenancy = response.TenancyDetails;

                if (tenancy.TenancyRef != null)
                    tenancyDetails = new Dictionary<string, object>
                    {
                        {"current_arrears_agreement_status", tenancy.ArrearsAgreementStatus},
                        {"primary_contact_name", tenancy.PrimaryContactName},
                        {"primary_contact_long_address", tenancy.PrimaryContactLongAddress},
                        {"primary_contact_postcode", tenancy.PrimaryContactPostcode}
                    };

                List<Dictionary<string, object>> latestActionDiary;

                if (tenancy.ArrearsActionDiary == null)
                    latestActionDiary = new List<Dictionary<string, object>>();
                else
                    latestActionDiary = tenancy.ArrearsActionDiary.ConvertAll(actionDiary =>
                        new Dictionary<string, object>
                        {
                            {"balance", actionDiary.Balance},
                            {"code", actionDiary.Code},
                            {"type", actionDiary.Type},
                            {"date", actionDiary.Date.ToString()},
                            {"comment", actionDiary.Comment},
                            {"universal_housing_username", actionDiary.UniversalHousingUsername}
                        });

                List<Dictionary<string, object>> latestAgreement;

                if (tenancy.ArrearsAgreements == null)
                    latestAgreement = new List<Dictionary<string, object>>();
                else
                    latestAgreement = tenancy.ArrearsAgreements.ConvertAll(agreement =>
                        new Dictionary<string, object>
                        {
                            {"amount", agreement.Amount},
                            {"breached", agreement.Breached},
                            {"clear_by", agreement.ClearBy},
                            {"frequency", agreement.Frequency},
                            {"start_balance", agreement.StartBalance},
                            {"start_date", agreement.Startdate},
                            {"status", agreement.Status}
                        });

                if (tenancyDetails.Count != 0)
                    result = new Dictionary<string, object>
                    {
                        {"tenancy_details", tenancyDetails},
                        {"latest_action_diary_events", latestActionDiary},
                        {"latest_arrears_agreements", latestAgreement}
                    };
                else
                    result = new Dictionary<string, object>
                    {
                        {"tenancy_details", new Dictionary<string, object>()},
                        {"latest_action_diary_events", new List<Dictionary<string, object>>()},
                        {"latest_arrears_agreements", new List<Dictionary<string, object>>()}
                    };
            }

            catch (Exception ex)
            {
                var errors = new List<APIErrorMessage>
                {
                    new APIErrorMessage
                    {
                        developerMessage = ex.Message,
                        userMessage = "Something went wrong with retrieving tenancy for ref: {tenancyRef}"
                    }
                };
                var json = Json(errors);
                json.StatusCode = 500;
                return json;
            }

            return Ok(result);
        }
    }
}
