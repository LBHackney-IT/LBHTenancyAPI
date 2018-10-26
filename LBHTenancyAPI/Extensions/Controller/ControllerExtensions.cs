using System;
using LBHTenancyAPI.Infrastructure.V1.API;
using LBHTenancyAPI.Infrastructure.V1.UseCase.Execution;
using Microsoft.AspNetCore.Mvc;

namespace LBHTenancyAPI.Extensions.Controller
{
    /// <summary>
    /// Extension class to help format a positive response into an standardised API Response
    /// </summary>
    public static class ControllerExtensions
    {
        /// <summary>
        /// Formats ExecuteWrapper which can be positve or negative Responses into standardised APIResponses
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="controller"></param>
        /// <param name="result"></param>
        /// <returns></returns>
        [Obsolete("ExecuteStandardResponse is deprecated please do not use the ExecuteWrapper Pattern")]
        public static IActionResult ExecuteStandardResponse<T>(this Microsoft.AspNetCore.Mvc.Controller controller, IExecuteWrapper<T> result) where T  : class
        {
            var apiResponse = new APIResponse<T>(result);
            //Use Extension Method to set a statusCode as well as an object
            return controller.StatusCode(apiResponse.StatusCode, apiResponse);
        }

        /// <summary>
        /// Formats postive responses from UseCases into APIResponses
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="controller"></param>
        /// <param name="result"></param>
        /// <returns></returns>
        public static IActionResult StandardResponse<T>(this Microsoft.AspNetCore.Mvc.Controller controller, T result) where T : class
        {
            var apiResponse = new APIResponse<T>(result);
            //Use Extension Method to set a statusCode as well as an object
            return controller.StatusCode(apiResponse.StatusCode, apiResponse);
        }
    }
}
