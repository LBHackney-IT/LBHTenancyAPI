﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using LBHTenancyAPI.Gateways;
using LBHTenancyAPI.UseCases;
using Microsoft.AspNetCore.Mvc;

namespace LBHTenancyAPI.Controllers
{
    [Produces("application/json")]
    [Route("api/tenancies")]
    public class TenanciesController : Controller
    {
        private readonly IListTenancies listTenancies;

        public TenanciesController(IListTenancies listTenancies)
        {
            this.listTenancies = listTenancies;
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
    }
}
