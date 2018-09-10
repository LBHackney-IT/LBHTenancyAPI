using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using LBHTenancyAPI.Infrastructure.API;
using LBHTenancyAPI.UseCases.ArrearsAgreements;
using Microsoft.AspNetCore.Mvc;

namespace LBHTenancyAPI.Extensions.Controller
{
    public static class ControllerExtensions
    {
        public static IActionResult StandardResponse<T>(this Microsoft.AspNetCore.Mvc.Controller controller, IExecuteWrapper<T> result)
        {
            var apiResponse = new APIResponse<T>(result);
            return controller.StatusCode(apiResponse.StatusCode, apiResponse);
        }
    }
}
