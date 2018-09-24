using LBHTenancyAPI.Infrastructure.API;
using LBHTenancyAPI.Infrastructure.UseCase.Execution;
using Microsoft.AspNetCore.Mvc;

namespace LBHTenancyAPI.Extensions.Controller
{
    public static class ControllerExtensions
    {
        public static IActionResult StandardResponse<T>(this Microsoft.AspNetCore.Mvc.Controller controller, IExecuteWrapper<T> result) where T  : class
        {
            var apiResponse = new APIResponse<T>(result);
            return controller.StatusCode(apiResponse.StatusCode, apiResponse);
        }
    }
}
