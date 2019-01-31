using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using LBHTenancyAPI.UseCases.V1;
using Microsoft.AspNetCore.Mvc;

namespace LBHTenancyAPI.Controllers.V1
{
    [ApiVersion("1")]
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

        /// <summary>
        /// Get inforation on one or more tenancies
        /// </summary>
        /// Sample response:
        ///
        ///     {
        ///        "id": 1,
        ///        "name": "Item1",
        ///        "isComplete": true
        ///     }
        ///
        /// <param name="tenancyRefs"></param>
        /// <returns>IActionResult</returns>
        [HttpGet]
        public async Task<IActionResult> Get([FromQuery(Name = "tenancy_refs")] List<string> tenancyRefs)
        {
            var response = listTenancies.Execute(tenancyRefs);
            var tenancies = response.Tenancies.ConvertAll(tenancy => new Dictionary<string, object>
            {
                {"ref", tenancy.TenancyRef},
                {"prop_ref", tenancy.PropertyRef},
                {"payment_ref", tenancy.PaymentRef},
                {"start_date", tenancy.StartDate},
                {"tenure", tenancy.Tenure},
                {
                    "current_balance", new Dictionary<string, object>
                    {
                        {"value", tenancy.CurrentBalance.Value},
                        {"currency_code", tenancy.CurrentBalance.CurrencyCode}
                    }

                },
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
            var response = await listAllPayments.ExecuteAsync(tenancyRef).ConfigureAwait(false);

            return Ok(response);
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

            var response = tenancyDetailsForRef.Execute(tenancyRef);

            var tenancy = response.TenancyDetails;

            tenancyDetails = new Dictionary<string, object>
            {
                {"ref", tenancy.TenancyRef},
                {"prop_ref", tenancy.PropertyRef},
                {"payment_ref", tenancy.PaymentRef},
                {"start_date", tenancy.StartDate},
                {"tenure", tenancy.Tenure},
                {"rent", tenancy.Rent},
                {"service", tenancy.Service},
                {"other_charge", tenancy.OtherCharge},
                {"current_arrears_agreement_status", tenancy.ArrearsAgreementStatus},
                {"current_balance", tenancy.CurrentBalance},
                {"primary_contact_name", tenancy.PrimaryContactName},
                {"primary_contact_long_address", tenancy.PrimaryContactLongAddress},
                {"primary_contact_postcode", tenancy.PrimaryContactPostcode}
            };

            List<Dictionary<string, object>> latestActionDiary = new List<Dictionary<string, object>>();
            List<Dictionary<string, object>> latestAgreement = new List<Dictionary<string, object>>();

            try
            {
                latestActionDiary = tenancy.ArrearsActionDiary.ConvertAll(
                    actionDiary =>
                        new Dictionary<string, object>
                        {
                            {"balance", actionDiary.Balance},
                            {"code", actionDiary.Code},
                            {"type", actionDiary.Type},
                            {"date", actionDiary.Date.ToString()},
                            {"comment", actionDiary.Comment},
                            {"universal_housing_username", actionDiary.UniversalHousingUsername}
                        });

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
            }
            catch (NullReferenceException)
            {
                // happens when nothing is found when trying to convert contents of internal lists

            }

            result = new Dictionary<string, object>
            {
                {"tenancy_details", tenancyDetails},
                {"latest_action_diary_events", latestActionDiary},
                {"latest_arrears_agreements", latestAgreement}
            };

            return Ok(result);
        }
    }
}
