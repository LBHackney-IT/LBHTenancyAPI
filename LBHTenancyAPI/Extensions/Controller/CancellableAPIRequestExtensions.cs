using System.Threading;
using Microsoft.AspNetCore.Http;

namespace LBHTenancyAPI.Extensions.Controller
{
    /// <summary>
    /// Extension Helper class to help get CancellationTokens from APIRequests
    /// </summary>
    public static class CancellableAPIRequestExtensions
    {
        /// <summary>
        /// This allows us to get a CancellationToken from the HTTPContext so if the user
        /// Cancels the request on the browser we can potentally cancel the request in the API
        /// However if the HTTPContext is Null Return a default CancellationToken
        /// </summary>
        /// <param name="httpContext"></param>
        /// <returns></returns>
        public static CancellationToken GetCancellationToken(this HttpContext httpContext)
        {
            if(httpContext == null)
                return CancellationToken.None;
            return httpContext?.RequestAborted ?? CancellationToken.None;
        }
        /// <summary>
        /// This allows us to get a CancellationToken from the HTTPContext so if the user
        /// Cancels the request on the browser we can potentally cancel the request in the API
        /// However if the HTTPContext is Null Return a default CancellationToken
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        public static CancellationToken GetCancellationToken(this HttpRequest request)
        {
            return request?.HttpContext?.RequestAborted ?? CancellationToken.None;
        }
    }
}
