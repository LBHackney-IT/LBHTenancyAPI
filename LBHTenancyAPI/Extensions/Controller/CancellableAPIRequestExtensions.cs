using System.Threading;
using Microsoft.AspNetCore.Http;

namespace LBHTenancyAPI.Extensions.Controller
{
    public static class CancellableAPIRequestExtensions
    {
        public static CancellationToken GetCancellationToken(this HttpContext httpContext)
        {
            return httpContext?.RequestAborted ?? CancellationToken.None;
        }
    }
}
