using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;

namespace LBHTenancyAPI.Middleware
{
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

        public async Task Invoke(HttpContext context)
        {
            try
            {
                await _next(context).ConfigureAwait(false); 
            }
            catch (Exception ex)
            {
                try
                {
                    _logger.LogError(ex, "Exception occurred");
                    return;
                }
                catch (Exception ex2)
                {
                    _logger.LogError(
                        0, ex2,
                        "An exception was thrown attempting " +
                        "to execute the error handler.");
                }

                // Otherwise this handler will
                // re -throw the original exception
                throw;
            }
        }
    }
}
