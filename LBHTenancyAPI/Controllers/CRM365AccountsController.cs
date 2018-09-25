using System;
using LBHTenancyAPI.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace LBHTenancyAPI.Controllers
{
    public class CRM365AccountsController : Controller
    {
        private readonly IHackneyCRM365AccessTokenService _accessTokenService;

        public CRM365AccountsController()
        {
        }
    }
}
