using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;

namespace LBHTenancyAPI.Infrastructure.API
{
    public static class CancellableAPIRequestExtensions
    {
        public static CancellationToken GetCancellationToken(this HttpContext httpContext)
        {
            return httpContext?.RequestAborted ?? CancellationToken.None;
        }
    }
}
