using System.Threading;
using Microsoft.AspNetCore.Http;

namespace LBHTenancyAPI.Extensions.Controller
{
    public static class CancellableAPIRequestExtensions
    {
        public static CancellationToken GetCancellationToken(this HttpContext httpContext)
        {
            if(httpContext == null)
                return CancellationToken.None;
            return httpContext?.RequestAborted ?? CancellationToken.None;
        }

        public static CancellationToken GetCancellationToken(this HttpRequest request)
        {
            return request?.HttpContext?.RequestAborted ?? CancellationToken.None;
        }
    }
}
