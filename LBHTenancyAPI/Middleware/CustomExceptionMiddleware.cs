using System;
using System.Net;
using System.Threading.Tasks;
using LBHTenancyAPI.Infrastructure.V1.API;
using LBHTenancyAPI.Infrastructure.V1.Exceptions;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;

namespace LBHTenancyAPI.Middleware
{
    /// <summary>
    /// Middleware to handle exceptions and return them as APIResponses with APIErrors and correct
    /// HttpStatus Methods
    /// </summary>
    public sealed class CustomExceptionHandlerMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger _logger;

        public CustomExceptionHandlerMiddleware(
            RequestDelegate next,
            ILoggerFactory loggerFactory)
        {
            _next = next;
            _logger = loggerFactory.CreateLogger<CustomExceptionHandlerMiddleware>();
        }

        /// <summary>
        /// Intercept the request and wrap it in a TryCatch
        /// Allows us to throw exceptions and know they will be properly handled
        /// Centralises exception handling and removes 80-90% of exception handling code from codebase
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        public async Task Invoke(HttpContext context)
        {
            try
            {
                //Configure await is a performance optimisation that allows the
                //asynchronous method call to resume on any available thread
                await _next(context).ConfigureAwait(false);
            }
            catch (BadRequestException ex)
            {
                //log exception
                _logger.LogError(ex, $"{nameof(ex)} occurred");
                //clear response
                context.Response.Clear();
                //create API response
                var apiResponse = new APIResponse<object>(ex);
                //we are currently only producing JSON so when that changes we need to change this
                context.Response.ContentType = "application/json";
                context.Response.StatusCode = (int)ex.StatusCode;
                var response = JsonConvert.SerializeObject(apiResponse);
                //write response in event of error
                await context.Response.WriteAsync(response, context.RequestAborted).ConfigureAwait(false);
            }
            catch (APIException ex)
            {
                //log exception
                _logger.LogError(ex, $"{nameof(ex)} occurred");
                //clear response
                context.Response.Clear();
                //create API response
                var apiResponse = new APIResponse<string>(ex);
                //we are currently only producing JSON so when that changes we need to change this
                context.Response.ContentType = "application/json";
                context.Response.StatusCode = (int)ex.StatusCode;
                var response = JsonConvert.SerializeObject(apiResponse);
                //write response in event of error
                await context.Response.WriteAsync(response, context.RequestAborted).ConfigureAwait(false);
            }
            catch (Exception ex)
            {
                //log exception
                _logger.LogError(ex, $"{nameof(ex)} occurred");
                //clear response
                context.Response.Clear();
                //create API response
                var apiResponse = new APIResponse<object>(ex);
                //we are currently only producing JSON so when that changes we need to change this
                context.Response.ContentType = "application/json";
                context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;
                var response = JsonConvert.SerializeObject(apiResponse);
                //write response in event of error
                await context.Response.WriteAsync(response, context.RequestAborted).ConfigureAwait(false);
            }

        }
    }
}
