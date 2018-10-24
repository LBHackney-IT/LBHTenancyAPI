using System;
using System.Net;

namespace LBHTenancyAPI.Infrastructure.V1.API
{
    public abstract class ApiException:Exception
    {
        public HttpStatusCode StatusCode { get; protected set; }
    }
}